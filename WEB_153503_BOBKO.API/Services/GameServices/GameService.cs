using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using WEB_153503_BOBKO.API.Data;
using WEB_153503_BOBKO.Domain.Entities;
using WEB_153503_BOBKO.Domain.Models;

namespace WEB_153503_BOBKO.API.Services.GameServices
{
    public class GameService : IGameService
    {
        private readonly int _maxPageSize = 20;
        private readonly AppDbContext _dbContext;


        public GameService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task<ResponseData<Game>> CreateGameAsync(Game game)
        {

            try
            {
                _dbContext.Games.Add(game);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return new ResponseData<Game>
                {
                    Success = false,
                    ErrorMessage = ex.Message,
                };
            }

            return new ResponseData<Game>
            {
                Data = game,
                Success = true,
            };
        }


        public async Task DeleteGameAsync(int id)
        {
            var game = await _dbContext.Games.FindAsync(id);

            if (game is null)
                return;


            _dbContext.Games.Remove(game);
            await _dbContext.SaveChangesAsync();
        }


        public async Task<ResponseData<Game>> GetGameByIdAsync(int id)
        {
            var game = await _dbContext.Games.FindAsync(id);

            if (game is null)
            {
                return new ResponseData<Game>
                {
                    Success = false,
                    ErrorMessage = "Game not founded",
                };
            }

            return new ResponseData<Game>
            {
                Data = game,
                Success = true,
            };
        }


        public async Task<ResponseData<ListModel<Game>>> GetGameListAsync(string? genreNormalizedName, int pageNo = 1, int pageSize = 3)
        {
            if (pageSize > _maxPageSize)
                pageSize = _maxPageSize;

            var query = _dbContext.Games.AsQueryable();
            var dataList = new ListModel<Game>();
            query = query.Where(d => genreNormalizedName == null || d.Genre!.NormalizedName.Equals(genreNormalizedName));

            var count = await query.CountAsync();

            if (count == 0)
            {
                return new ResponseData<ListModel<Game>>
                {
                    Data = dataList,
                    Success = true,
                };
            }



            //var filteredGames =
            //   genreNormalizedName != null ?
            //   _games.Where(game => game.Genre?.NormalizedName == genreNormalizedName).ToList() :
            //   _games;

            //int itemsPerPage = _config.GetValue<int>("ItemsPerPage");

            //int totalPages =
            //    filteredGames.Count() % itemsPerPage == 0 ?
            //    filteredGames.Count() / itemsPerPage :
            //    filteredGames.Count() / itemsPerPage + 1;


            //var responseData = new ResponseData<ListModel<Game>>
            //{
            //    Data = new ListModel<Game>
            //    {
            //        Items = filteredGames.Skip((pageNo - 1) * itemsPerPage).Take(itemsPerPage).ToList(),
            //        CurrentPage = pageNo,
            //        TotalPages = totalPages,
            //    }
            //};

            int totalPages =
                    count % pageSize == 0 ?
                    count / pageSize :
                    count / pageSize + 1;

            if (pageNo > totalPages)
            {
                return new ResponseData<ListModel<Game>>
                {
                    Success = false,
                    ErrorMessage = "No such page"
                };
            }

            dataList.Items = await query.Skip((pageNo - 1) * pageSize).Take(pageSize).ToListAsync();
            dataList.CurrentPage = pageNo;
            dataList.TotalPages = totalPages;

            return new ResponseData<ListModel<Game>>
            {
                Data = dataList,
                Success = true,
            };
        }


        public async Task<ResponseData<string>> SaveImageAsync(int id, IFormFile formFile)
        {
            var responseData = new ResponseData<string>();
            var game = await _dbContext.Games.FindAsync(id);

            if (game is null)
            {
                return new ResponseData<string>
                {
                    Success = false,
                    ErrorMessage = "No item found",
                };
            }

            var host = "https://"/* + _httpContextAccessor.HttpContext?.Request.Host*/;
            var imageFolder = Path.Combine(/*_webHostEnvironment.WebRootPath, */"images");

            if (formFile is not null)
            {
                if (!string.IsNullOrEmpty(game.Path))
                {
                    var prevImage = Path.GetFileName(game.Path);
                    var prevImagePath = Path.Combine(imageFolder, prevImage);
                    if (File.Exists(prevImagePath))
                    {
                        File.Delete(prevImagePath);
                    }
                }
                var ext = Path.GetExtension(formFile.FileName);
                var fName = Path.ChangeExtension(Path.GetRandomFileName(), ext);
                var filePath = Path.Combine(imageFolder, fName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await formFile.CopyToAsync(stream);
                }

                game.Path = $"{host}/images/{fName}";
                await _dbContext.SaveChangesAsync();
            }

            return new ResponseData<string>()
            {
                Data = game.Path,
                Success = true,
            };
        }


        public async Task UpdateGameAsync(int id, Game game)
        {
            var updatingGame = await _dbContext.Games.FindAsync(id);

            if (updatingGame is null)
                return;

            updatingGame.Name = game.Name;
            updatingGame.Description = game.Description;
            updatingGame.Price = game.Price;
            updatingGame.Path = game.Path;
            updatingGame.Genre = game.Genre;
        }

    }
}
