using Microsoft.EntityFrameworkCore;
using WEB_153503_BOBKO.API.Data;
using WEB_153503_BOBKO.Domain.Entities;
using WEB_153503_BOBKO.Domain.Models;

namespace WEB_153503_BOBKO.API.Services.GameGenreService
{
    public class GameGenreService: IGameGenreService
    {
        private readonly AppDbContext _dbContext;

        public GameGenreService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ResponseData<List<GameGenre>>> GetGameGenreListAsync()
        {
            return new ResponseData<List<GameGenre>>
            {
                Data = await _dbContext.GameGenres.ToListAsync(),
            };
        }

    }
}
