using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WEB_153503_BOBKO.API.Data;
using WEB_153503_BOBKO.API.Services.GameGenreService;
using WEB_153503_BOBKO.API.Services.GameServices;
using WEB_153503_BOBKO.Domain.Entities;
using WEB_153503_BOBKO.Domain.Models;

namespace WEB_153503_BOBKO.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private readonly IGameService _gameService;

        public GamesController(IGameService gameService)
        {
            _gameService = gameService;
        }

        // GET: api/Games
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Game>>> GetGames(string? gameGenre, int pageNo = 1, int pageSize = 3)
        {
            return Ok(await _gameService.GetGameListAsync(gameGenre, pageNo, pageSize));
        }


        // GET: api/Games/game-5
        [HttpGet("game-{id}")]
        public async Task<ActionResult<Game>> GetGame(int id)
        {
            var response = await _gameService.GetGameByIdAsync(id);
            return response.Success ? Ok(response) : NotFound(response);
        }

        // PUT: api/Games/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGame(int id, Game game)
        {
            try
            {
                await _gameService.UpdateGameAsync(id, game);
            }
            catch (Exception ex)
            {
                return NotFound(new ResponseData<Game>()
                {
                    Success = false,
                    ErrorMessage = ex.Message
                });
            }

            return Ok(new ResponseData<Game>()
            {
                Data = game,
                Success = true,
            });
        }

        // POST: api/Games
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Game>> PostGame(Game game)
        {
            var response = await _gameService.CreateGameAsync(game);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        // DELETE: api/Games/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGame(int id)
        {
            try
            {
                await _gameService.DeleteGameAsync(id);
            }
            catch (Exception ex)
            {
                return NotFound(new ResponseData<Game>()
                {
                    Success = false,
                    ErrorMessage = ex.Message
                });
            }

            return NoContent();
        }

        // POST: api/Tools/5
        [HttpPost("{id}")]
        public async Task<ActionResult<ResponseData<string>>> PostImage(int id, IFormFile formFile)
        {
            var response = await _gameService.SaveImageAsync(id, formFile);
            if (response.Success)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        private async Task<bool> GameExists(int id)
        {
            return (await _gameService.GetGameByIdAsync(id)).Success;
        }
    }
}
