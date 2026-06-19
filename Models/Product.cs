using System.ComponentModel.DataAnnotations;

namespace _2380601597_TranMinhNhut_WebAPI.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên sản phẩm không được để trống")]
        public string Name { get; set; } = string.Empty;

        [Range(0.01, double.MaxValue, ErrorMessage = "Giá sản phẩm phải lớn hơn 0")]
        public decimal Price { get; set; }

        public string? Description { get; set; }

        public int? CategoryId { get; set; }

        public Category? Category { get; set; }
    }
}