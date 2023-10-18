using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEB_153503_BOBKO.Domain.Entities
{
    public class GameGenre
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? NormalizedName { get; set; }
    }
}
