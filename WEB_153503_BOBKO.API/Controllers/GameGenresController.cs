using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WEB_153503_BOBKO.API.Data;
using WEB_153503_BOBKO.Domain.Entities;

namespace WEB_153503_BOBKO.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameGenresController : ControllerBase
    {
        private readonly AppDbContext _context;

        public GameGenresController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/GameGenres
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GameGenre>>> GetGameGenres()
        {
          if (_context.GameGenres == null)
          {
              return NotFound();
          }
            return await _context.GameGenres.ToListAsync();
        }

        // GET: api/GameGenres/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GameGenre>> GetGameGenre(int id)
        {
          if (_context.GameGenres == null)
          {
              return NotFound();
          }
            var gameGenre = await _context.GameGenres.FindAsync(id);

            if (gameGenre == null)
            {
                return NotFound();
            }

            return gameGenre;
        }

        // PUT: api/GameGenres/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGameGenre(int id, GameGenre gameGenre)
        {
            if (id != gameGenre.Id)
            {
                return BadRequest();
            }

            _context.Entry(gameGenre).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GameGenreExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/GameGenres
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<GameGenre>> PostGameGenre(GameGenre gameGenre)
        {
          if (_context.GameGenres == null)
          {
              return Problem("Entity set 'AppDbContext.GameGenres'  is null.");
          }
            _context.GameGenres.Add(gameGenre);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGameGenre", new { id = gameGenre.Id }, gameGenre);
        }

        // DELETE: api/GameGenres/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGameGenre(int id)
        {
            if (_context.GameGenres == null)
            {
                return NotFound();
            }
            var gameGenre = await _context.GameGenres.FindAsync(id);
            if (gameGenre == null)
            {
                return NotFound();
            }

            _context.GameGenres.Remove(gameGenre);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool GameGenreExists(int id)
        {
            return (_context.GameGenres?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
