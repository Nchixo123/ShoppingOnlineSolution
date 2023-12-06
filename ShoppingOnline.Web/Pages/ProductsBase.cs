using Microsoft.AspNetCore.Components;
using ShoppingOnline.Web.Client;
using ShoppingOnline.Web.Services.Contracts;
using ShoppingOnlineModels.DTO;
using System.Runtime.CompilerServices;

namespace ShoppingOnline.Web.Pages
{
    public class ProductsBase : ComponentBase
    {
        [Inject]
        public IProductService ProductService { get; set; }

        [Inject]

        public IShoppingCartService ShoppingCartService { get; set; }

        [Inject]
        public IManageLocalStorageService ManageLocalStorageService { get; set; }

        [Inject]
        public IManageCartItemsLocalStorageService ManageCartItemsLocalStorageService { get; set; }

        public IEnumerable<ProductDTO> Products { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        public string ErrorMessage { get; set; }
             

        protected override async Task OnInitializedAsync()
        {
            try
            {
                await ClearLocalStorage();

                Products = await ManageLocalStorageService.GetCollection();

                var shoppingCartItems = await ManageCartItemsLocalStorageService.GetCollection();

                var totalQuantity = shoppingCartItems.Sum(x => x.Quantity);

                ShoppingCartService.RaiseEventShoppingCartChanged(totalQuantity);
            }
            catch (Exception)
            {

                throw;
            }

        }

        protected IOrderedEnumerable<IGrouping<int, ProductDTO>> GetGroupedProductsByCategory()
        {
            return from product in Products
                   group product by product.CategoryId into prodByCatGroup
                   orderby prodByCatGroup.Key
                   select prodByCatGroup;
        }

        protected static string GetCategoryName(IGrouping<int, ProductDTO> gropedProductDTOs)
        {
            return gropedProductDTOs.FirstOrDefault(pg => pg.CategoryId == gropedProductDTOs.Key).CategoryName;
        }

        private async Task ClearLocalStorage()
        {
            await ManageLocalStorageService.RemoveCollection();
            await ManageCartItemsLocalStorageService.RemoveCollection();
        }
    }
}
