using System.ComponentModel.DataAnnotations.Schema;

namespace ShopOnline.Api.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string ImageURL { get; set; } = null!;

        [Column(TypeName = "smallmoney")]
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public int CategoryId { get; set; }


    }
}
