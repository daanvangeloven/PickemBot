using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PickemBot.Models
{
    public class AdvancePicks
    {
        [Key]
        public int ID     { get; set; }
        
        public Team? Undefeated { get; set; }
        
        public Team? NoWin { get; set; }
        public ICollection<Team>? AdvanceTeams { get; set; }
    }
}