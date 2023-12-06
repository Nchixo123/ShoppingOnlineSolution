using Microsoft.EntityFrameworkCore;
using ShopOnline.Api.Data;
using ShopOnline.Api.Entities;
using ShoppingOnline.Api.Repositories.Contracts;
using ShoppingOnlineModels.DTO;

namespace ShoppingOnline.Api.Repositories
{
    public class ShoppingCartRepository : IShoppingCartRepository
    {
        private readonly ShopOnlineDbContext shopOnlineDbContext;

        public ShoppingCartRepository(ShopOnlineDbContext shopOnlineDbContext)
        {
            this.shopOnlineDbContext = shopOnlineDbContext;
        }

        private async Task<bool> CartItemExists(int cartId, int productId)
        {
            return await shopOnlineDbContext.CartItems.AnyAsync(c => c.CartId == cartId && c.ProductId == productId);
        }

        public async Task<CartItem> AddItem(CartItemToAddDTO cartItemToAddDTO)
        {
            if (await CartItemExists(cartItemToAddDTO.CartId, cartItemToAddDTO.ProductId) == false)
            {
                var item = await (from product in shopOnlineDbContext.Products
                                  where product.Id == cartItemToAddDTO.ProductId
                                  select new CartItem
                                  {
                                      CartId = cartItemToAddDTO.CartId,
                                      ProductId = product.Id,
                                      Quantity = cartItemToAddDTO.Quantity
                                  }).SingleOrDefaultAsync();

                if (item != null)
                {
                    var result = await shopOnlineDbContext.CartItems.AddAsync(item);
                    await shopOnlineDbContext.SaveChangesAsync();
                    return result.Entity;

                }
            }

            return null;
        }

        public async Task<CartItem> DeleteItem(int id)
        {
            var item = await shopOnlineDbContext.CartItems.FindAsync(id);

            if (item != null)
            {
                shopOnlineDbContext.CartItems.Remove(item);
                await shopOnlineDbContext.SaveChangesAsync();
            }

            return item;
        }

        public async Task<CartItem> GetItem(int id)
        {
            return await (from cart in shopOnlineDbContext.Carts
                          join cartItem in shopOnlineDbContext.CartItems
                          on cart.Id equals cartItem.CartId
                          where cartItem.Id == id
                          select new CartItem
                          {
                              Id = cartItem.Id,
                              ProductId = cartItem.ProductId,
                              Quantity = cartItem.Quantity,
                              CartId = cartItem.CartId
                          }).SingleAsync();

        }

        public async Task<IEnumerable<CartItem>> GetItems(int userId)
        {
            return await (from cart in shopOnlineDbContext.Carts
                          join cartItem in shopOnlineDbContext.CartItems
                          on cart.Id equals cartItem.CartId
                          where cart.UserId == userId
                          select new CartItem
                          {
                              Id = cartItem.Id,
                              ProductId = cartItem.ProductId,
                              Quantity = cartItem.Quantity,
                              CartId = cartItem.CartId
                          }).ToListAsync();
        }

        public async Task<CartItem> UpdateQuantity(int id, CartItemQuantityUpdateDTO cartItemQuantityUpdateDTO)
        {
           var item = await shopOnlineDbContext.CartItems.FindAsync(id);

            if (item != null)
            {
                item.Quantity = cartItemQuantityUpdateDTO.Quantity;
                await shopOnlineDbContext.SaveChangesAsync();
                return item;
            }

            return null;
        }
    }
}
