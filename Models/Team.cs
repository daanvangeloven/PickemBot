using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using PickemBot.Enums;

namespace PickemBot.Models
{
    public class Team
    {
        [JsonProperty("id")]
        [Key]
        public int ID       { get; set; }
        
        [JsonProperty("name")]
        public string Name  { get; set; }
        
        [JsonProperty("emoteName")]
        public string EmoteName  { get; set; }
        
        [JsonProperty("color")]
        public string Colour { get; set; }
        // public List<Player> Players { get; set; }
        
        [JsonProperty("ImageURL")]
        public string ImageURL { get; set; }
        
        public ICollection<AdvancePicks>? PickedByAdvance { get; set; } 
        
       
        public ICollection<ChampionPicks>? PickedQuarterFinals { get; set; }
        public ICollection<ChampionPicks>? PickedSemiFinals { get; set; }


        [JsonProperty("ChallengerResult")]
        public AdvanceResults ChallengeResult { get; set; }
       

        [JsonProperty("LegendResult")]
        public AdvanceResults LegendResult { get; set; }
        
        
    }
}