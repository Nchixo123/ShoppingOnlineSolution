using Microsoft.AspNetCore.Components;
using ShoppingOnline.Web.Client;
using ShoppingOnline.Web.Services.Contracts;
using ShoppingOnlineModels.DTO;

namespace ShoppingOnline.Web.Pages
{
    public class ProductsBase : ComponentBase
    {
        [Inject]
        public IProductService ProductService { get; set; }

        [Inject]

        public IShoppingCartService ShoppingCartService { get; set; }

        public IEnumerable<ProductDTO> Products { get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                Products = await ProductService.GetItems();

                var shoppingCartItems = await ShoppingCartService.GetItems(HardCoded.userId);

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
    }
}
