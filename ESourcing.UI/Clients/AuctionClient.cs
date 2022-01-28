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
    public class AuctionClient
    {
        public HttpClient _client { get; }
        public AuctionClient(HttpClient client)
        {
            _client = client;
            _client.BaseAddress = new System.Uri(CommonInfo.BaseAddress);
        }

        public async Task<Result<AuctionDto>> CreateAuction(AuctionDto dto)
        {
            var dataAsString = JsonConvert.SerializeObject(dto);
            var content = new StringContent(dataAsString);
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            var response = await _client.PostAsync("/Auction", content);

            if (response.IsSuccessStatusCode)
            {
                var responsaData = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<AuctionDto>(responsaData);
                if (result != null)
                    return new Result<AuctionDto>(true, ResultConstant.RecordCreateSuccessfully, result);
                else
                    return new Result<AuctionDto>(false, ResultConstant.RecordCreateNotSuccessfully);
            }
            return new Result<AuctionDto>(false, ResultConstant.RecordCreateNotSuccessfully);
        }


        public async Task<Result<List<AuctionDto>>> GetAuctions()
        {
            var response = await _client.GetAsync("/Auction");
            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<List<AuctionDto>>(responseData);
                if (result.Any())
                    return new Result<List<AuctionDto>>(true, ResultConstant.RecordFound, result.ToList());
                return new Result<List<AuctionDto>>(false, ResultConstant.RecordNotFound);
            }
            return new Result<List<AuctionDto>>(false, ResultConstant.RecordNotFound);
        }

        public async Task<Result<AuctionDto>> GetAuctionById(string id)
        {
            var response = await _client.GetAsync("/Auction/" + id);
            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<AuctionDto>(responseData);
                if (result != null)
                    return new Result<AuctionDto>(true, ResultConstant.RecordFound, result);
                return new Result<AuctionDto>(false, ResultConstant.RecordNotFound);
            }
            return new Result<AuctionDto>(false, ResultConstant.RecordNotFound);
        }

        public async Task<Result<string>> CompleteBid(string id)
        {
            var dataAsString = JsonConvert.SerializeObject(id);
            var content = new StringContent(dataAsString);
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            var response = await _client.PostAsync("/Auction/CompleteAuction", content);
            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();
                return new Result<string>(true, ResultConstant.RecordCreateSuccessfully, responseData);
            }
            return new Result<string>(false, ResultConstant.RecordCreateNotSuccessfully);
        }
    }
}
