using System.ComponentModel.DataAnnotations;

namespace PickemBot.Models
{
    public class Player
    {
        [Key]
        public int ID { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Nickname { get; set; }
    }
}