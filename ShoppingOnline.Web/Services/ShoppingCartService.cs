using Newtonsoft.Json;
using ShoppingOnline.Web.Services.Contracts;
using ShoppingOnlineModels.DTO;
using System.Net.Http.Json;
using System.Text;

namespace ShoppingOnline.Web.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly HttpClient httpClient;

       public event Action<int> ShoppingCartChanged;
        public ShoppingCartService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

 

        public async Task<CartItemDTO> AddItem(CartItemToAddDTO cartItemToAddDTO)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync<CartItemToAddDTO>("api/ShoppingCart", cartItemToAddDTO);

                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return default(CartItemDTO);
                    }
                    return await response.Content.ReadFromJsonAsync<CartItemDTO>();
                }

                else
                {
                    var message = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Http status: {response.StatusCode} Message - {message}");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<CartItemDTO> DeleteItem(int id)
        {
            try
            {
                var response = await httpClient.DeleteAsync($"api/ShoppingCart/{id}");

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<CartItemDTO>();
                }

                return default(CartItemDTO);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error message: {ex.Message}");

            }
        }

        public async Task<List<CartItemDTO>> GetItems(int userId)
        {

            try
            {
                var response = await httpClient.GetAsync($"api/ShoppingCart/{userId}/GetItems");

                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return Enumerable.Empty<CartItemDTO>().ToList();
                    }

                    return await response.Content.ReadFromJsonAsync<List<CartItemDTO>>();
                }

                else
                {
                    var message = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Http status: {response.StatusCode} Message - {message}");
                }
            }

            catch (Exception)
            {

                throw;
            }
        }

        public void RaiseEventShoppingCartChanged(int totalQuantity)
        {
            if (ShoppingCartChanged != null)
            {
                ShoppingCartChanged.Invoke(totalQuantity);
            }
        }

        public async Task<CartItemDTO> UpdateQuantity(CartItemQuantityUpdateDTO cartItemQuantityUpdateDTO)
        {
            try
            {
                var jsonRequest = JsonConvert.SerializeObject(cartItemQuantityUpdateDTO);

                var content = new StringContent(jsonRequest, Encoding.UTF8,"application/json-patch+json");

                var response = await httpClient.PatchAsync($"api/ShoppingCart/{cartItemQuantityUpdateDTO.CartItemId}", content);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<CartItemDTO>();
                }

                return null;
            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}
