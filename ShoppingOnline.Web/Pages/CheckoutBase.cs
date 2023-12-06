using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using ShoppingOnline.Web.Client;
using ShoppingOnline.Web.Services.Contracts;
using ShoppingOnlineModels.DTO;

namespace ShoppingOnline.Web.Pages
{
    public class CheckoutBase:ComponentBase
    {
        [Inject]
        public IJSRuntime Js { get; set; }

        protected IEnumerable<CartItemDTO> ShoppingCartItems { get; set; }

        protected int TotalQuantity { get; set; }

        protected string PaymentDescription { get; set; }

        protected decimal PaymentAmount { get; set; }

        [Inject]
        public IShoppingCartService ShoppingCartService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                ShoppingCartItems = await ShoppingCartService.GetItems(HardCoded.userId);

                if (ShoppingCartItems !=null)
                {
                    Guid orderGuid = Guid.NewGuid();

                    PaymentAmount = ShoppingCartItems.Sum(p => p.TotalPrice);
                    TotalQuantity = ShoppingCartItems.Sum(p => p.Quantity);
                    PaymentDescription = $"O_{HardCoded.userId}_{orderGuid}";
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            try
            {
                if (firstRender)
                {
                    await Js.InvokeVoidAsync("initPayPalButton");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
