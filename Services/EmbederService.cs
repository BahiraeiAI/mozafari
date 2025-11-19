using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace RoshedTehran.Services
{
    public class EmbederService
    {
        private readonly HttpClient _httpClient;

        public EmbederService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<float[]> GetEmbeddingAsyncEntity(Uri? uri,string? TitleTag,string? MetaDescription)
        {
            string? domain = null;
            if (uri is not null)
            {
                string host = uri.Host; // "www.example.com"
                var parts = host.Split('.');
                domain = parts.Length > 2 ? parts[1] : parts[0]; // "example"
            }

            // Concatenate only non-null, non-empty parts
            string text = string.Join(" ", new[] { domain, TitleTag, MetaDescription }
                                      .Where(s => !string.IsNullOrWhiteSpace(s)));


            if (string.IsNullOrWhiteSpace(text))
                throw new ArgumentException("Text cannot be empty.", nameof(text));

            Console.WriteLine("hit here in embeder");
            var response = await _httpClient.PostAsJsonAsync("http://127.0.0.1:8000/embed", new { text = text });

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<EmbeddingResponse>();

            return result?.Embedding ?? throw new Exception("Failed to parse embedding response.");
        }



        public async Task<float[]> GetEmbeddingAsyncQuery(string searchQuery)
        {
            if (string.IsNullOrWhiteSpace(searchQuery))
                throw new ArgumentException("Text cannot be empty.", nameof(searchQuery));
            var response = await _httpClient.PostAsJsonAsync("http://127.0.0.1:8000/embed", new { searchQuery = searchQuery });

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<EmbeddingResponse>();

            return result?.Embedding ?? throw new Exception("Failed to parse embedding response.");
        }



        private class EmbeddingResponse
        {
            public float[] Embedding { get; set; } = Array.Empty<float>();
        }
    }
}