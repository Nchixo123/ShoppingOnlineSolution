using Microsoft.AspNetCore.Mvc;
using ShoppingOnline.Api.Extensions;
using ShoppingOnline.Api.Repositories.Contracts;
using ShoppingOnlineModels.DTO;

namespace ShoppingOnline.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository productRepository;

        public ProductController(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetItems()
        {
            try
            {
                var products = await this.productRepository.GetItems();
                var productCategories = await this.productRepository.GetCategories();

                if (products == null || productCategories == null)
                {
                    return NotFound();
                }

                else
                {
                    var productDTOs = products.ConvertToDTO(productCategories);

                    return Ok(productDTOs);
                }
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Error Retrieving data from the database");
            }
        }


        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductDTO>> GetItem(int id)
        {
            try
            {
                var product = await this.productRepository.GetItem(id);

                if (product == null)
                {
                    return BadRequest();
                }

                else
                {
                    var productCategory = await this.productRepository.GetCategory(product.CategoryId);

                    var productDTO = product.ConvertToDTO(productCategory);

                    return Ok(productDTO);
                }
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Error Retrieving data from the database");
            }
        }
    }
}
