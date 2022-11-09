using ESourcing.Core.Repositories;
using ESourcing.Core.ResultModels;
using ESourcing.UI.Clients;
using ESourcing.UI.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace ESourcing.UI.Controllers
{
    //[Authorize]
    public class AuctionController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly ProductClient _productClient;
        private readonly AuctionClient _auctionClient;
        private readonly BidClient _bidClient;
        public AuctionController(IUserRepository userRepository, ProductClient productClient, AuctionClient auctionClient, BidClient bidClient)
        {
            _userRepository = userRepository;
            _productClient = productClient;
            _auctionClient = auctionClient;
            _bidClient = bidClient;
        }

        public async Task<IActionResult> Index()
        {
            var auctionList = await _auctionClient.GetAuctions();
            if (auctionList.IsSuccess)
                return View(auctionList.Data);
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var productList = await _productClient.GetProducts();
            if (productList.IsSuccess)
                ViewBag.ProductList = productList.Data;

            var userList = await _userRepository.GetAllAsync();
            ViewBag.UserList = userList;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(AuctionDto model)
        {
            model.Status = 0;
            model.CreatedAt = DateTime.Now;
            model.IncludedSellers.Add(model.SellerId);
            var createAuction = await _auctionClient.CreateAuction(model);
            if (createAuction.IsSuccess)
                return RedirectToAction("Index");
            return View(model);
        }

        public async Task<IActionResult> Detail(string id)
        {
            AuctionBidsDto model = new AuctionBidsDto();

            var auctionResponse = await _auctionClient.GetAuctionById(id);
            var bidsResponse = await _bidClient.GelAllBidsByAuctionId(id);

            model.SellerUserName = HttpContext.User?.Identity.Name;
            model.AuctionId = auctionResponse.Data.Id;
            model.ProductId = auctionResponse.Data.ProductId;
            model.Bids = bidsResponse.Data;
            var isAdmin = HttpContext.Session.GetString("IsAdmin");
            model.IsAdmin = Convert.ToBoolean(isAdmin);

            return View(model);
        }
        [HttpPost]
        public async Task<Result<string>> SendBid(BidDto model)
        {
            model.CreateAt = DateTime.Now;
            var sendBidResponse = await _bidClient.SendBid(model);
            return sendBidResponse;
        }

        [HttpPost]
        public async Task<Result<string>> CompleteBid(string id)
        {
            var completeBidResponse = await _auctionClient.CompleteBid(id);
            return completeBidResponse;
        }

    }
}
