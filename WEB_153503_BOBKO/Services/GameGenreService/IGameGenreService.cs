using WEB_153503_BOBKO.Domain.Models;
using WEB_153503_BOBKO.Domain.Entities;

namespace WEB_153503_BOBKO.Services.GameGenreService
{
    public interface IGameGenreService
    {
        public Task<ResponseData<List<GameGenre>>> GetCategoryListAsync();
    }
}
