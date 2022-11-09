using Orders.Domain.Entities.Base;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Orders.Domain.Entities
{
    [Table("Order")]
    public class Order : EntityBase
    {
        public string AuctionId { get; set; }
        public string SellerUserName{ get; set; }
        public string ProductId{ get; set; }
        public decimal UnitPrice{ get; set; }
        public decimal TotalPrice{ get; set; }
        public DateTime CreatedAt{ get; set; }
    }
}
