using ShoppingOnlineModels.DTO;

namespace ShoppingOnline.Web.Services.Contracts
{
    public interface IShoppingCartService
    {
        Task<List<CartItemDTO>> GetItems(int userId);
        Task<CartItemDTO> AddItem(CartItemToAddDTO cartItemToAddDTO);
        Task<CartItemDTO> UpdateQuantity(CartItemQuantityUpdateDTO cartItemQuantityUpdateDTO);
        Task<CartItemDTO> DeleteItem(int id);

        event Action<int> ShoppingCartChanged;

        void RaiseEventShoppingCartChanged(int totalQuantity);

    }
}
