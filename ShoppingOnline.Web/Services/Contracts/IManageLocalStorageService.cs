using ShoppingOnlineModels.DTO;

namespace ShoppingOnline.Web.Services.Contracts
{
    public interface IManageLocalStorageService
    {
        Task<IEnumerable<ProductDTO>> GetCollection();
        Task RemoveCollection();
    }
}
