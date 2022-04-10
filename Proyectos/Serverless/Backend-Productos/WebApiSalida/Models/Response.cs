using Newtonsoft.Json;

namespace WebApiSalida.Models
{
    public class Response<T>
    {
        [JsonProperty("data")]
        public T? Data { get; set; }
        [JsonProperty("success")]
        public bool Success { get; set; }
        [JsonProperty("method")]
        public string Method { get; set; } = string.Empty;
    }
}
