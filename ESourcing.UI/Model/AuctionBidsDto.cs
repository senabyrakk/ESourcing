using System.Collections.Generic;

namespace ESourcing.UI.Model
{
    public class AuctionBidsDto
    {
        public string AuctionId { get; set; }
        public string ProductId { get; set; }
        public string SellerUserName { get; set; }
        public bool IsAdmin { get; set; }
        public List<BidDto> Bids { get; set; }
    }
}
