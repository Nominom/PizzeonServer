using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pizzeon_server.Models
{
    public class SessionStats
    {
        public int PlayedGames { get; set; }
        public float Accuracy { get; set; }
        public float Distance { get; set; }
        public int Points { get; set; }
        public float WinLoss { get; set; }
        public int Hits { get; set; }
        public int Dropped { get; set; }
    }
}
