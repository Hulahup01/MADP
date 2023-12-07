using System.Text.Json;
using System.Text;
using WEB_153503_BOBKO.Domain.Models;
using WEB_153503_BOBKO.Domain.Entities;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Http;


namespace WEB_153503_BOBKO.BlazorWasm.Services
{
    public class DataService : IDataService
    {
        private readonly HttpClient _httpClient;
        private readonly int _pageSize = 3;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public DataService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _pageSize = configuration.GetSection("PageSize").Get<int>();
            _jsonSerializerOptions = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
        }

        public List<GameGenre> Genres { get; set; }
        public List<Game> GameList { get; set; }
        public bool Success { get; set; } = true;
        public string ErrorMessage { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }

        public async Task GetGameListAsync(string? genreNormalizedName, int pageNo = 1)
        {
            var urlString = new StringBuilder($"{_httpClient.BaseAddress!.AbsoluteUri}api/Games/");
            if (genreNormalizedName != null)
            {
                urlString.Append($"{genreNormalizedName}/");
            };
            if (pageNo > 1)
            {
                urlString.Append($"page{pageNo}");
            };
            if (!_pageSize.Equals("3"))
            {
                urlString.Append(QueryString.Create("pageSize", _pageSize.ToString()));
            }

            var response = await _httpClient.GetAsync(new Uri(urlString.ToString()));
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    var responseData = await response.Content.ReadFromJsonAsync<ResponseData<ListModel<Game>>>(_jsonSerializerOptions);
                    GameList = responseData?.Data.Items;
                    TotalPages = responseData?.Data?.TotalPages ?? 0;
                    CurrentPage = responseData?.Data?.CurrentPage ?? 0;
                }
                catch (JsonException ex)
                {
                    Success = false;
                    ErrorMessage = $"Ошибка: {ex.Message}";
                }
            }
            else
            {
                Success = false;
                ErrorMessage = $"Данные не получены от сервера. Error:{response.StatusCode}";
            }
        }

        public async Task<Game?> GetGameByIdAsync(int id)
        {
            var urlString = new StringBuilder($"{_httpClient.BaseAddress!.AbsoluteUri}api/games/{id}");
            var response = await _httpClient.GetAsync(new Uri(urlString.ToString()));

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    return (await response.Content.ReadFromJsonAsync<ResponseData<Game>>(_jsonSerializerOptions))?.Data;
                }
                catch (JsonException ex)
                {
                    Success = false;
                    ErrorMessage = $"Ошибка: {ex.Message}";
                    return null;
                }
            }
            Success = false;
            ErrorMessage = $"Данные не получены от сервера. Error:{response.StatusCode}";
            return null;
        }

        public async Task GetGenreListAsync()
        {
            var urlString = new StringBuilder($"{_httpClient.BaseAddress?.AbsoluteUri}api/GameGenres/");
            var response = await _httpClient.GetAsync(new Uri(urlString.ToString()));
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    var responseData = await response.Content.ReadFromJsonAsync<ResponseData<List<GameGenre>>>(_jsonSerializerOptions);
                    Genres = responseData?.Data;
                }
                catch (JsonException ex)
                {
                    Success = false;
                    ErrorMessage = $"Ошибка: {ex.Message}";
                }
            }
            else
            {
                Success = false;
                ErrorMessage = $"Данные не получены от сервера. Error:{response.StatusCode}";
            }
        }
    }
}
