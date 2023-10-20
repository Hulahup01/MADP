using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WEB_153503_BOBKO.API.Data;
using WEB_153503_BOBKO.API.Services.GameGenreService;
using WEB_153503_BOBKO.Domain.Entities;
using WEB_153503_BOBKO.Domain.Models;

namespace WEB_153503_BOBKO.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameGenresController : ControllerBase
    {
        private readonly IGameGenreService _gameGenreService;

        public GameGenresController(IGameGenreService gameGenreService)
        {
            _gameGenreService = gameGenreService;
        }

        // GET: api/GameGenres
        [HttpGet]
        public async Task<ActionResult<ResponseData<GameGenre>>> GetGameGenres()
        {
            var responce = await _gameGenreService.GetGameGenreListAsync();
            return responce.Success ? Ok(responce) : BadRequest(responce);
        }

    }
}
