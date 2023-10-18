using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WEB_153503_BOBKO.Domain.Entities;
using WEB_153503_BOBKO.Domain.Models;
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
            if (gameGenreNormalized == "Все")
               gameGenreNormalized = null;

            var genres = (await _gameGenreService.GetCategoryListAsync()).Data;

            ViewData["genres"] = genres;
            ViewData["currentGenre"] =
                gameGenreNormalized == null ? 
                new GameGenre { Id = 3, Name = "Все", NormalizedName = null } :
                genres.Where(genre => genre.NormalizedName.Equals(gameGenreNormalized)).ToList().First();

            var productResponse = await _gameService.GetGameListAsync(gameGenreNormalized, pageNo);


            //  FEEGWEGKL:EGHJKL:SGJKDJGJDSGJDGJ:DSKL:JGLKDSGKLJDL:GJSDLGJ:D
            if (productResponse.Success)
                return NotFound(productResponse.ErrorMessage);


            return View(new ListModel<Game>
            {
                Items = productResponse.Data.Items,
                CurrentPage = pageNo,
                TotalPages = productResponse.Data.TotalPages,
            });
        }
    }
}
