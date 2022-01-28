using ESourcing.Products.Data.Abstraction;
using ESourcing.Products.Data.Abstraction.Repository;
using ESourcing.Products.Entities;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ESourcing.Products.Data.Manager.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly IProductContext _productContext;
        public ProductRepository(IProductContext productContext)
        {
            _productContext = productContext;
        }

        public async Task Create(Product product)
        {
            await _productContext.Products.InsertOneAsync(product);
        }

        public async Task<bool> Delete(string id)
        {
            var filter = Builders<Product>.Filter.Eq(m => m.Id, id);

            DeleteResult result = await _productContext.Products.DeleteOneAsync(filter);

            return result.IsAcknowledged && result.DeletedCount > 0;
        }

        public async Task<Product> GetProdct(string id)
        {
            return await _productContext.Products.Find(p => p.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await _productContext.Products.Find(p => true).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductByCategory(string category)
        {
            var filter = Builders<Product>.Filter.Eq(p => p.Category, category);

            return await _productContext.Products.Find(filter).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductByName(string name)
        {
            var filter = Builders<Product>.Filter.ElemMatch(p => p.Name, name);

            return await _productContext.Products.Find(filter).ToListAsync();
        }

        public async Task<bool> Update(Product product)
        {
            var updateResult = await _productContext.Products.ReplaceOneAsync(filter: g => g.Id == product.Id, replacement: product);
            return  updateResult.IsAcknowledged && updateResult.ModifiedCount > 0;
        }
    }
}
