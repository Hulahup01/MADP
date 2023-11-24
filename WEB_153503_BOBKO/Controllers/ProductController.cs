using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WEB_153503_BOBKO.Domain.Entities;
using WEB_153503_BOBKO.Domain.Models;
using WEB_153503_BOBKO.Extensions;
using WEB_153503_BOBKO.Services.GameGenreService;
using WEB_153503_BOBKO.Services.GameService;

namespace WEB_153503_BOBKO.Controllers
{
    public class ProductController : Controller
    {
        private IGameGenreService _gameGenreService;
        private IGameService _gameService;

        public ProductController(IGameService gameService, IGameGenreService gameGenreService)
        {
            _gameGenreService = gameGenreService;
            _gameService = gameService;
        }

        public async Task<IActionResult> Index(string? gameGenreNormalized, int pageNo = 1)
        {
            var genresResponse = await _gameGenreService.GetGameGenreListAsync();
            if (!genresResponse.Success)
                return NotFound(genresResponse.ErrorMessage);

            var genres = genresResponse.Data;

            ViewData["genres"] = genres;
            ViewData["currentGenre"] =
                gameGenreNormalized == null ?
                new GameGenre { Name = "Все", NormalizedName = null } :
                genres.Where(genre => genre.NormalizedName.Equals(gameGenreNormalized)).ToList().First();

            if (gameGenreNormalized == "Все")
                gameGenreNormalized = null;

            var gameResponse = await _gameService.GetGameListAsync(gameGenreNormalized, pageNo);
            if (!gameResponse.Success)
                return NotFound(gameResponse.ErrorMessage);

            if (Request.IsAjaxRequest())
            {
                ListModel<Game> data = gameResponse.Data!;
                return PartialView("_ProductIndexPartial", new
                {
                    Items = data.Items,
                    CurrentPage = pageNo,
                    TotalPages =  data.TotalPages,
                    GameGenreNormalized = gameGenreNormalized
                });
            }
            else
            {
                return View(new ListModel<Game>
                {
                    Items = gameResponse.Data.Items,
                    CurrentPage = pageNo,
                    TotalPages = gameResponse.Data.TotalPages,
                });
            }
            
        }
    }
}
