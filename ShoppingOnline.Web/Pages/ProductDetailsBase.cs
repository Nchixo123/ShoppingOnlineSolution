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
        public IManageLocalStorageService ManageLocalStorageService { get; set; }

        [Inject]
        public IManageCartItemsLocalStorageService ManageCartItemsLocalStorageService { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }    

        public ProductDTO Product { get; set; }

        public string ErrorMessage { get; set; }

        private List<CartItemDTO> ShoppingCartItems { get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                ShoppingCartItems = await ManageCartItemsLocalStorageService.GetCollection();
                Product = await GetProductById(Id);
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

                if (cartItemDTO != null)
                {
                    ShoppingCartItems.Add(cartItemDTO);
                    await ManageCartItemsLocalStorageService.SaveCollection(ShoppingCartItems);
                }

                NavigationManager.NavigateTo("/ShoppingCart");
            }
            catch (Exception)
            {

                throw;
            }
        }

        private async Task<ProductDTO> GetProductById(int id)
        {
            var productDTOs = await ManageLocalStorageService.GetCollection();

            if (productDTOs != null)
            {
                return productDTOs.SingleOrDefault(x => x.Id == id);

            }
            return null;
        }

    }
}
