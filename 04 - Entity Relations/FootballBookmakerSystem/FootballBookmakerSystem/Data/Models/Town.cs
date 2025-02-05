using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballBookmakerSystem.Data.Models
{
    public class Town
    {
        public int TownId { get; set; }
        public string Name { get; set; }

        public int CountryId { get; set; }
    }
}
