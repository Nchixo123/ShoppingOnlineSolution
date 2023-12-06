using ShopOnline.Api.Entities;
using ShoppingOnlineModels.DTO;

namespace ShoppingOnline.Api.Repositories.Contracts
{
    public interface IShoppingCartRepository
    {
        Task<CartItem> AddItem(CartItemToAddDTO cartItemToAddDTO);
        Task<CartItem> UpdateQuantity(int id, CartItemQuantityUpdateDTO cartItemQuantityUpdateDTO);
        Task<CartItem> DeleteItem(int id);
        Task<CartItem> GetItem(int id);
        Task<IEnumerable<CartItem>> GetItems(int userId);

    }
}
