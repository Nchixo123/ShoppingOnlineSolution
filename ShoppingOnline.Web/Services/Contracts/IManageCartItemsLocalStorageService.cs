using ShoppingOnlineModels.DTO;

namespace ShoppingOnline.Web.Services.Contracts
{
    public interface IManageCartItemsLocalStorageService
    {
        Task<List<CartItemDTO>> GetCollection();
        Task SaveCollection(List<CartItemDTO> cartIteDTOs);
        Task RemoveCollection();
    }
}
