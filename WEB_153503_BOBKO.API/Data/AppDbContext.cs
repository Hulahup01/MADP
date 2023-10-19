using Microsoft.EntityFrameworkCore;
using WEB_153503_BOBKO.Domain.Entities;

namespace WEB_153503_BOBKO.API.Data
{
    public class AppDbContext: DbContext
    {
        public DbSet<Game> Games { get; set; }

        public DbSet<GameGenre> GameGenres { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
        {

        }
    }
}
