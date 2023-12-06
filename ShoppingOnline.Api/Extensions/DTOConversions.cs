using ShopOnline.Api.Entities;
using ShoppingOnlineModels.DTO;
namespace ShoppingOnline.Api.Extensions
{
    public static class DTOConversions
    {
        public static IEnumerable<ProductDTO> ConvertToDTO(this IEnumerable<Product> products,IEnumerable<ProductCategory> productCategories)
        {
            return (from product in products
                    join productCategory in productCategories
                    on product.CategoryId equals productCategory.Id
                    select new ProductDTO
                    {
                        Id = product.Id,
                        Name = product.Name,
                        Description = product.Description,
                        ImageURL = product.ImageURL,
                        Price = product.Price,
                        Quantity = product.Quantity,
                        CategoryId = product.CategoryId,
                        CategoryName = productCategory.Name
                    }).ToList();
        }


        public static ProductDTO ConvertToDTO(this Product products, ProductCategory productCategory)
        {
            return new ProductDTO
            {
                Id = products.Id,
                Name = products.Name,
                Description = products.Description,
                ImageURL = products.ImageURL,
                Price = products.Price,
                Quantity = products.Quantity,
                CategoryId = products.CategoryId,
                CategoryName = productCategory.Name
            };
        }

        public static IEnumerable<CartItemDTO> ConvertToDTO(this IEnumerable<CartItem> cartItems, IEnumerable<Product> products)
        {
            return (from cartItem in cartItems
                    join product in products
                    on cartItem.ProductId equals product.Id
                    select new CartItemDTO
                    {
                        Id = cartItem.Id,
                        ProductId = cartItem.ProductId,
                        ProductName = product.Name,
                        ProductDescription = product.Description,
                        ProductImageURL = product.ImageURL,
                        Price = product.Price,
                        CartId = cartItem.CartId,
                        Quantity= cartItem.Quantity,
                        TotalPrice = product.Price * cartItem.Quantity
                    }).ToList();
        }

        public static CartItemDTO ConvertToDTO(this CartItem cartItem, Product product)
        {
            return new CartItemDTO
            {
                Id = cartItem.Id,
                ProductId = cartItem.ProductId,
                ProductName = product.Name,
                ProductDescription = product.Description,
                ProductImageURL = product.ImageURL,
                Price = product.Price,
                CartId = cartItem.CartId,
                Quantity = cartItem.Quantity,
                TotalPrice = product.Price * cartItem.Quantity
            };
        }
    }
}
