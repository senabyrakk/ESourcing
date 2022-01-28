using ESourcing.Products.Data.Abstraction.Repository;
using ESourcing.Products.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ESourcing.Products.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private ILogger<ProductController> _logger; 
        public ProductController(IProductRepository productRepository, ILogger<ProductController> logger)
        {
            _productRepository = productRepository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> Get()
        {
            var products =  await _productRepository.GetProducts();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Product>>> Get(string id)
        {
            var product = await _productRepository.GetProdct(id);

            if (product == null)
            {
                _logger.LogError($"Product witdh id {id},hasnt bees found in database");
                return NotFound();
            }

            return Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult<IEnumerable<Product>>> Post(Product product)
        {
              await _productRepository.Create(product);

            return CreatedAtRoute("Get", new { id = product.Id }, product);
        }

        [HttpPut]
        public async Task<ActionResult<IEnumerable<Product>>> Put(Product product)
        {
            await _productRepository.Update(product);

            return CreatedAtRoute("Get", new { id = product.Id }, product);
        }

        [HttpDelete]
        public async Task<ActionResult<IEnumerable<Product>>> Delete(string id)
        {
           var result =  await _productRepository.Delete(id);

            if (!result)
            {
                _logger.LogError($"Product witdh id {id},cant delete");
                return BadRequest();
            }

            return Ok();
        }
    }
}
