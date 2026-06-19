using System.Text.Json.Serialization;

namespace _2380601597_TranMinhNhut_WebAPI.Models
{
    public class Category
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        [JsonIgnore]
        public ICollection<Product>? Products { get; set; }
    }
}