using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WEB_153503_BOBKO.Domain.Entities;
using WEB_153503_BOBKO.Services.GameService;

namespace WEB_153503_BOBKO.Areas.Admin.Pages
{
    public class CreateModel : PageModel
    {
        private readonly IGameService _gameService;

        public CreateModel(IGameService gameService)
        {
            _gameService = gameService;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Game Game { get; set; } = default!;

        [BindProperty]
        public IFormFile Image { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var response = await _gameService.CreateGameAsync(Game, Image);

            if (!response.Success)
                return Page();

            return RedirectToPage("./Index");
        }
    }
}
