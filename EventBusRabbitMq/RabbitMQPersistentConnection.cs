using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System;
using System.Net.Sockets;

namespace EventBusRabbitMq
{
    public class RabbitMQPersistentConnection : IRabbitMQPersistentConnection
    {
        private readonly IConnectionFactory _connectionFactory;
        private  IConnection _connection;
        private readonly int _retryCount;
        private readonly ILogger<RabbitMQPersistentConnection> _logger;
        private bool _dispose;

        public RabbitMQPersistentConnection(IConnectionFactory connectionFactor, int retryCount, ILogger<RabbitMQPersistentConnection> logger)
        {
            _connectionFactory = connectionFactor;
            _retryCount = retryCount;
            _logger = logger;
        }

        public bool IsConnected
        {
            get
            {
                return _connection != null && _connection.IsOpen && !_dispose;
            }
        }

        public IModel CreateModel()
        {
            if (!IsConnected)
            {
                throw new InvalidOperationException("Error");
            }

            return _connection.CreateModel();
        }

        public bool TryConnect()
        {
            _logger.LogInformation("RabbiMq client is trying to connect");

            var policy = RetryPolicy.Handle<SocketException>()
                               .Or<BrokerUnreachableException>()
                               .WaitAndRetry(_retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
                                {
                                    _logger.LogWarning("RabbiMq client could not connect after {timeout}s ({ExceptionMessage})", $"{time.TotalSeconds:n1}", ex.Message);
 
                                });

            policy.Execute(() => {
                _connection = _connectionFactory.CreateConnection();
            });

            if (IsConnected)
            {
                _connection.ConnectionShutdown += OnConnectionBlocked;
                _connection.CallbackException += OnCallbackException;
                _connection.ConnectionBlocked += OnConnecitonBlocked;

                _logger.LogInformation("RabbirMQ Client acquired a perisisten conneciton to '{hostname}' and is subscribed to failure events", _connection);
                return true;    
            }
            else
            {
                _logger.LogCritical("fATAL ERRROR ");
                return false;
            }
        }

        private void OnConnectionBlocked(object sender, ShutdownEventArgs e)
        {
            if (_dispose) return;

            _logger.LogWarning("A rabbitmq connection throw exception.");

            TryConnect();
        }

        private void OnConnecitonBlocked(object sender, ConnectionBlockedEventArgs e)
        {
            if (_dispose) return;

            _logger.LogWarning("A rabbitmq connection throw exception.");

            TryConnect();
        }

        private void OnCallbackException(object sender, CallbackExceptionEventArgs e)
        {
            if (_dispose) return;

            _logger.LogWarning("A rabbitmq connection throw exception.");

            TryConnect();
        }

        public void Dispose()
        {
            if (_dispose) return;

            _dispose = true;
            try
            {
                _connection.Dispose();
            }
            catch (System.Exception e)
            {
                _logger.LogCritical(e.ToString());
            }
        }

    }
}
