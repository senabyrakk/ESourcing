using ESourcing.Core.Common;
using ESourcing.Core.ResultModels;
using ESourcing.UI.Model;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ESourcing.UI.Clients
{
    public class ProductClient
    {
        public HttpClient _client { get; }
        public ProductClient(HttpClient client)
        {
            _client = client;
            _client.BaseAddress = new System.Uri(CommonInfo.BaseAddress);
        }

        public async Task<Result<List<ProductDto>>> GetProducts()
        {
            var response = await _client.GetAsync("/Product");
            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<List<ProductDto>>(responseData);
                if (result.Any())
                    return new Result<List<ProductDto>>(true, ResultConstant.RecordFound, result.ToList());
                return new Result<List<ProductDto>>(false, ResultConstant.RecordNotFound);
            }
            return new Result<List<ProductDto>>(false, ResultConstant.RecordNotFound);
        }
    }
}
