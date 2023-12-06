using Microsoft.AspNetCore.Components;
using ShoppingOnlineModels.DTO;

namespace ShoppingOnline.Web.Pages
{
    public class DisplayProductsBase :ComponentBase
    {
        [Parameter]
        public IEnumerable<ProductDTO> Products { get; set; }
    }
}
