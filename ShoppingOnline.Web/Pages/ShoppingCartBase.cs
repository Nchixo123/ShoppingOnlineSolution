using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using ShoppingOnline.Web.Client;
using ShoppingOnline.Web.Services.Contracts;
using ShoppingOnlineModels.DTO;
using System.Runtime.CompilerServices;

namespace ShoppingOnline.Web.Pages
{
    public class ShoppingCartBase : ComponentBase
    {
        [Inject]
        public IJSRuntime Js { get; set; }

        [Inject]
        public IShoppingCartService ShoppingCartService { get; set; }

        [Inject]
        public IManageCartItemsLocalStorageService ManageCartItemsLocalStorageService { get; set; }

        public List<CartItemDTO> ShoppingCartItems { get; set; }

        public string ErrorMessage { get; set; }

        protected string TotalPrice { get; set; }
        protected int TotalQuantity { get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                ShoppingCartItems = await ManageCartItemsLocalStorageService.GetCollection();
                CartChanged();
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }

        public async Task DeleteCartItem_Click(int id)
        {
            var cartItem = await this.ShoppingCartService.DeleteItem(id);

            RemoveCartItem(id);
            CartChanged();

        }

        protected async Task UpdateQuantityCartItem_Click(int id, int quantity)
        {
            if (quantity > 0)
            {
                var updateItemDTO = new CartItemQuantityUpdateDTO
                {
                    CartItemId = id,
                    Quantity = quantity

                };

                var returnedUpdateItemDTO = await ShoppingCartService.UpdateQuantity(updateItemDTO);

               await UpdateItemTotalPrice(returnedUpdateItemDTO);

                CartChanged();

                await MakeUpdateQuantityButtonVisible(id, false);

            }

            else
            {
                var item = ShoppingCartItems.FirstOrDefault(x => x.Id == id);
                if (item != null)
                {
                    item.Quantity = 1;
                    item.TotalPrice = item.Price;
                }
            }
        }

        protected async Task UpdateQuantity_Input(int id)
        {
            await MakeUpdateQuantityButtonVisible(id, true);
        }

        private async Task MakeUpdateQuantityButtonVisible(int id, bool visible)
        {
            await Js.InvokeVoidAsync("MakeUpdateQuantityButtonVisible", id, visible);
        }

        private async Task UpdateItemTotalPrice(CartItemDTO cartItemDTO)
        {
            var item = GetCartItem(cartItemDTO.Id);

            if (item != null)
            {
                item.TotalPrice = cartItemDTO.Price * cartItemDTO.Quantity;
            }

            await ManageCartItemsLocalStorageService.SaveCollection(ShoppingCartItems);
        }

        private void CalculateTotal()
        {
            SetTotalPrice();
            SetTotalQuantity();
        }

        private void SetTotalPrice()
        {
            TotalPrice = ShoppingCartItems.Sum(x => x.TotalPrice).ToString("C");
        }

        private void SetTotalQuantity()
        {
            TotalQuantity = ShoppingCartItems.Sum(x => x.Quantity);
        }

        private CartItemDTO GetCartItem(int id)
        {
            return ShoppingCartItems.FirstOrDefault(x => x.Id == id);
        }

        private async Task RemoveCartItem(int id)
        {
            var cartItemDTO = GetCartItem(id);

            ShoppingCartItems.Remove(cartItemDTO);

            await ManageCartItemsLocalStorageService.SaveCollection(ShoppingCartItems);
        }

        //Try and put the settotalprice & settotal quantity methonds in this method only next time.
        private void CartChanged()
        {
            CalculateTotal();
            ShoppingCartService.RaiseEventShoppingCartChanged(TotalQuantity);
        }
    }
}
