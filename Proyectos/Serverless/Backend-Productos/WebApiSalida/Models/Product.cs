using Newtonsoft.Json;

namespace WebApiSalida.Models
{
    public class Product
    {
        [JsonProperty("id")]
        public string Id { get; set; } = string.Empty;
        [JsonProperty("name")]
        public string Name { get; set; } = string.Empty;
        [JsonProperty("category")]
        public string Category { get; set; } = string.Empty;
        [JsonProperty("description")]
        public string Description { get; set; } = string.Empty;
        [JsonProperty("imageUrl")]
        public string ImageUrl { get; set; } = string.Empty;
        [JsonProperty("price")]
        public double Price { get; set; }
        [JsonProperty("stock")]
        public int Stock { get; set; }
    }
}
