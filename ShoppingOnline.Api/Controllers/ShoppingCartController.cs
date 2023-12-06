using Microsoft.AspNetCore.Mvc;
using ShoppingOnline.Api.Extensions;
using ShoppingOnline.Api.Repositories.Contracts;
using ShoppingOnlineModels.DTO;

namespace ShoppingOnline.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingCartController : ControllerBase
    {
        private readonly IShoppingCartRepository shoppingCartRepository;

        private readonly IProductRepository productRepository;
        public ShoppingCartController(IShoppingCartRepository shoppingCartRepository, IProductRepository productRepository)
        {
            this.shoppingCartRepository = shoppingCartRepository;
            this.productRepository = productRepository;

        }

        [HttpGet]
        [Route("{userId}/GetItems")]
        public async Task<ActionResult<IEnumerable<CartItemDTO>>> GetItems(int userId)
        {
            try
            {
                var cartItems = await this.shoppingCartRepository.GetItems(userId);

                if (cartItems == null)
                {
                    return NoContent();
                }

                var products = await this.productRepository.GetItems();

                if (products == null)
                {
                    throw new Exception("No products exist in the system");
                }

                var cartItemsDto = cartItems.ConvertToDTO(products);

                return Ok(cartItemsDto);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);

            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<CartItemDTO>> GetItem(int userId)
        {

            try
            {
                var cartItem = await this.shoppingCartRepository.GetItem(userId);

                if (cartItem == null)
                {
                    return NotFound();  
                }

                var product = await this.productRepository.GetItem(userId);

                if (product == null)
                {
                    return NotFound();
                }

                var cartItemDTO = cartItem.ConvertToDTO(product);

                return Ok(cartItemDTO);    
            }
           
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<CartItemDTO>> PostItem([FromBody] CartItemToAddDTO cartItemToAddDTO)
        {
            try
            {
                var newCartItem = await this.shoppingCartRepository.AddItem(cartItemToAddDTO);

                if (newCartItem == null)
                {
                    return NoContent();
                }

                var product = await this.productRepository.GetItem(newCartItem.ProductId);

                if (product == null)
                {
                    return NoContent();
                }

                var newCartItemDTO = newCartItem.ConvertToDTO(product);

                return CreatedAtAction(nameof(GetItem), new { id = newCartItemDTO.Id }, newCartItemDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<CartItemDTO>> DeleteItem(int id)
        {
            try
            {
                var item = await this.shoppingCartRepository.DeleteItem(id);

                if (item == null)
                {
                    return NotFound();
                }

                var product = await this.productRepository.GetItem(item.ProductId);

                if(product == null)
                {
                    return NotFound();
                }

                var itemDTO = item.ConvertToDTO(product);
                
                return Ok(itemDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPatch("{id:int}")]
        public async Task<ActionResult<CartItemDTO>> UpdateQuantity(int id, CartItemQuantityUpdateDTO cartItemQuantityUpdateDTO)
        {
            try
            {
                var cartItem = await shoppingCartRepository.UpdateQuantity(id, cartItemQuantityUpdateDTO);
                if (cartItem == null)
                {
                    return NotFound();
                }

                var product = await productRepository.GetItem(cartItem.ProductId);

                var cartItemDTO = cartItem.ConvertToDTO(product);   

                return Ok(cartItemDTO);

                
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
