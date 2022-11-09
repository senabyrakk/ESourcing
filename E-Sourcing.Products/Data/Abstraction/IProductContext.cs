using ESourcing.Products.Entities;
using MongoDB.Driver;

namespace ESourcing.Products.Data.Abstraction
{
    public interface IProductContext
    {
        IMongoCollection<Product> Products { get;} 
    }
}
