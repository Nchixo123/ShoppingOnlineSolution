using ShoppingOnlineModels.DTO;

namespace ShoppingOnline.Web.Services.Contracts
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDTO>> GetItems();
        Task<ProductDTO> GetItem (int id);
    }
}
