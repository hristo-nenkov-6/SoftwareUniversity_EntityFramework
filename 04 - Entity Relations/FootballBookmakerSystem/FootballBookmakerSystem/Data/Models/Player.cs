using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballBookmakerSystem.Data.Models
{
    public class Player
    {
        [Key]
        public int PlayerId { get; set; }
        [Required]
        public string Name { get; set; }
        public byte SquadNumber { get; set; }
        public bool IsInjured { get; set; }
        public int PositionId { get; set; }
        public int TeamId { get; set; }
        public int TownId { get; set; }
    }
}
