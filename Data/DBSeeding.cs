using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using PickemBot.Models;
using PickemBot.Modules;

namespace PickemBot.Data
{
    public class DBSeeding
    {
        public static void SeedTeams(PickemContext context)
        {
            string fileName = "";
            
                 fileName = "../../../teams.json";
            
            
            string TeamList = File.ReadAllText(fileName);
            // string TeamList = GlobalVariables.teamseed;
            
            List<Team> data = JsonConvert.DeserializeObject<List<Team>>(TeamList);
            foreach (var team in data)
            {
                context.Teams.Add(team);
            }

            context.SaveChanges();
        }
    }
}