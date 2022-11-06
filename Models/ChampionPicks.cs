using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PickemBot.Models
{
    public class ChampionPicks
    {
        [Key]
        public int ID     { get; set; }
        
        public ICollection<Team> QuarterFinalists { get; set; }
        public ICollection<Team> SemiFinalists {get; set; }
        public Team Winner {get; set; }
    }
}