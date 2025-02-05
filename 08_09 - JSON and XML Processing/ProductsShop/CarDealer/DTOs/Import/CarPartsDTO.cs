using CarDealer.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarDealer.DTOs.Import
{
    public class CarPartsDTO
    {
        public string Make { get; set; } = null!;
        public string Model { get; set; } = null!;
        public long TraveledDistance { get; set; }
        public List<int> PartsIds { get; set; }
    }
}
