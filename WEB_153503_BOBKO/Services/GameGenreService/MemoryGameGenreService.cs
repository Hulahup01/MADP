using WEB_153503_BOBKO.Domain.Entities;
using WEB_153503_BOBKO.Domain.Models;

namespace WEB_153503_BOBKO.Services.GameGenreService
{
    public class MemoryGameGenreService : IGameGenreService
    {
        public Task<ResponseData<List<GameGenre>>> GetGameGenreListAsync()
        {
            var gameGenres = new List<GameGenre>
            {
                new GameGenre { Id = 1, Name = "Шутер", NormalizedName = "shooter"},
                new GameGenre { Id = 2, Name = "Гонки", NormalizedName = "race"},
                new GameGenre { Id = 3, Name = "Файтинг", NormalizedName = "fighting"},
                new GameGenre { Id = 4, Name = "Симулятор", NormalizedName = "simulator"}
            };
            var result = new ResponseData<List<GameGenre>>();
            result.Data = gameGenres;
            return Task.FromResult(result);
        }
    }
}
