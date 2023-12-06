using Microsoft.AspNetCore.Components;
using ShoppingOnline.Web.Services.Contracts;
using ShoppingOnlineModels.DTO;

namespace ShoppingOnline.Web.Pages
{
    public class ProductDetailsBase : ComponentBase
    {
        [Parameter]
        public int Id { get; set; }

        [Inject]
        public IProductService ProductService { get; set; }

        [Inject]
        public IShoppingCartService ShoppingCartService { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }    

        public ProductDTO Product { get; set; }

        public string ErrorMessage { get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                Product = await ProductService.GetItem(Id);
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }

        protected async Task AddToCart_Click(CartItemToAddDTO cartItemToAddDTO)
        {
            try
            {
                var cartItemDTO = await ShoppingCartService.AddItem(cartItemToAddDTO);
                NavigationManager.NavigateTo("/ShoppingCart");
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
