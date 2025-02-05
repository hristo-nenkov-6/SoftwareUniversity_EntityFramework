using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballBookmakerSystem.Data.Models
{
    public class Color
    {
        public int ColorId { get; set; }
        public string Name { get; set; }

        ICollection<Team> PrimaryKitTeams { get; }
        ICollection<Team> SecondartKitTeams { get; }
    }
}
