using ESourcing.Products.Data.Abstraction;
using ESourcing.Products.Entities;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;

namespace ESourcing.Products.Data.Manager
{
    public class ProductContext : IProductContext
    {
        private readonly IConfiguration _configuration;
        public ProductContext(IConfiguration configuration)
        {
            _configuration = configuration;

            var client = new MongoClient(_configuration.GetSection("ProductConnectionString").GetSection("ConnectionStrings").Value);
            var database = client.GetDatabase(_configuration.GetSection("ProductConnectionString").GetSection("DatabaseName").Value);

            Products = database.GetCollection<Product>(_configuration.GetSection("ProductConnectionString").GetSection("CollectionName").Value);
            ProductSeed.SeedData(Products);


        }


        public IMongoCollection<Product> Products { get; }
    }
}
