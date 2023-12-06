using ShoppingOnline.Web.Services.Contracts;
using ShoppingOnlineModels.DTO;
using System.ComponentModel.DataAnnotations;
using System.Net.Http.Json;

namespace ShoppingOnline.Web.Services
{
    public class ProductService : IProductService
    {
        private readonly HttpClient httpClient;

        public ProductService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<ProductDTO> GetItem(int id)
        {

            try
            {
                var response = await this.httpClient.GetAsync($"api/Product/{id}");
                
                if (response.IsSuccessStatusCode) 
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return default(ProductDTO);
                    }

                    return await response.Content.ReadFromJsonAsync<ProductDTO>();
                }

                else
                {
                    var message = await response.Content.ReadAsStringAsync();
                    throw new Exception(message);
                }

            }
            catch (Exception)
            {

                throw;
            }

        }

        public async Task<IEnumerable<ProductDTO>> GetItems()
        {
            try
            {
                var response = await this.httpClient.GetAsync("api/Product");

                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return Enumerable.Empty<ProductDTO>();
                    }

                    return await response.Content.ReadFromJsonAsync<IEnumerable<ProductDTO>>();
                }

                else
                {
                    var message = await response.Content.ReadAsStringAsync();
                    throw new Exception(message);
                }
               
            }

            catch (Exception)
            {

                throw;
            }
        }
    }
}
