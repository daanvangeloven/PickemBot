using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using PickemBot.Enums;
using PickemBot.Modules;

namespace PickemBot.Models
{
    public class Picker
    {
        [Key]
        public int ID { get; set; }
        public ulong DiscordID { get; set; }
        public AdvancePicks? ChallengerPicks { get; set; }
        public AdvancePicks? LegendPicks { get; set; }
        
        public ChampionPicks? ChampionPicks { get; set; }
        
        public bool ChallengerPinged { get; set; }
        
        public bool LegendPinged { get; set; }
        
        public bool ChampionPinged { get; set; }
        
        public int correctAdvancePicks(EventStage stage)
        {
            int counter = 0;
            // try
            // {
                if (stage == EventStage.Challenger && ChallengerPicks != null)
                {
                    if (ChallengerPicks.Undefeated.ChallengeResult == AdvanceResults.Undefeated) counter++;
                    if (ChallengerPicks.NoWin.ChallengeResult == AdvanceResults.NoWin) counter++;

                    foreach (Team team in ChallengerPicks.AdvanceTeams)
                    {
                        if (team.ChallengeResult == AdvanceResults.WillAdvance  || team.ChallengeResult == AdvanceResults.Undefeated) counter++;
                    }
                }
                else if(LegendPicks != null)
                {
                    if (LegendPicks.Undefeated.LegendResult == AdvanceResults.Undefeated) counter++;
                    if (LegendPicks.NoWin.LegendResult == AdvanceResults.NoWin) counter++;

                    foreach (Team team in LegendPicks.AdvanceTeams)
                    {
                        if (team.LegendResult == AdvanceResults.WillAdvance) counter++;
                    }
                }
                else
                {
                    return 0;
                }
            // }
            // catch
            // {
            //     return 0;
            // }

            return counter;
        }
        public bool pingedStage(EventStage stage)
        {
            switch (stage)
            {
                case EventStage.Challenger:
                    return ChallengerPinged;
                case EventStage.Legend:
                    return LegendPinged;
                case EventStage.Champion:
                    return ChampionPinged;
                default:
                    return false;
            }
        }
        
        public void setPingedStage(EventStage stage, bool value)
        {
            switch (stage)
            {
                case EventStage.Challenger:
                    ChallengerPinged = value;
                    break;
                case EventStage.Legend:
                    LegendPinged = value;
                    break;
                case EventStage.Champion:
                    ChampionPinged = value;
                    break;
            }
        }
    }
}