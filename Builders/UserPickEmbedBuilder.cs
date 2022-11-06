using System;
using System.Collections.Generic;
using System.Linq;
using Discord;
using Microsoft.EntityFrameworkCore.Query;
using PickemBot.Enums;
using PickemBot.Models;

namespace PickemBot.Modules
{
    public static class UserPickEmbedBuilder
    {
        public static EmbedBuilder CreateAdvanceListEmbed(Team nowin, Team undefeated, List<Team> AdvanceList, EventStage stage, string username)
        {
            EmbedBuilder embed = new EmbedBuilder()
                .WithTitle($"{username}'s picks")
                .WithColor(new Color(255, 255, 255))
                .WithDescription($"Stage: ***{stage.ToFriendlyString()}***");

            embed.AddField("**Type**",
                $"**{AdvanceResultsExtensions.ToPredictionString(AdvanceResults.Undefeated)}** \n " +
                $"**{AdvanceResultsExtensions.ToPredictionString(AdvanceResults.WillAdvance)}** \n " +
                $"**{AdvanceResultsExtensions.ToPredictionString(AdvanceResults.WillAdvance)}** \n " +
                $"**{AdvanceResultsExtensions.ToPredictionString(AdvanceResults.WillAdvance)}** \n " +
                $"**{AdvanceResultsExtensions.ToPredictionString(AdvanceResults.WillAdvance)}** \n " +
                $"**{AdvanceResultsExtensions.ToPredictionString(AdvanceResults.WillAdvance)}** \n " +
                $"**{AdvanceResultsExtensions.ToPredictionString(AdvanceResults.WillAdvance)}** \n " +
                $"**{AdvanceResultsExtensions.ToPredictionString(AdvanceResults.WillAdvance)}** \n " +
                $"**{AdvanceResultsExtensions.ToPredictionString(AdvanceResults.NoWin)}** \n ",
                true);
            
            embed.AddField("Picked",
                $"** {Emote.Parse(undefeated.EmoteName)} {undefeated.Name}** \n  " +
                $"**{Emote.Parse(AdvanceList[0].EmoteName)} {AdvanceList[0].Name}** \n " +
                $"**{Emote.Parse(AdvanceList[1].EmoteName)} {AdvanceList[1].Name}** \n " +
                $"**{Emote.Parse(AdvanceList[2].EmoteName)} {AdvanceList[2].Name}** \n " +
                $"**{Emote.Parse(AdvanceList[3].EmoteName)} {AdvanceList[3].Name}** \n " +
                $"**{Emote.Parse(AdvanceList[4].EmoteName)} {AdvanceList[4].Name}** \n " +
                $"**{Emote.Parse(AdvanceList[5].EmoteName)} {AdvanceList[5].Name}** \n " +
                $"**{Emote.Parse(AdvanceList[6].EmoteName)} {AdvanceList[6].Name}** \n " +
                $"**{Emote.Parse(nowin.EmoteName)} {nowin.Name}**"
                , true);
            
            string resultString = "";
            int scoreCounter = 0;
            Emoji emote;
           
            
            if (stage == EventStage.Challenger)
            {

                if (undefeated.ChallengeResult == AdvanceResults.Undefeated)
                {
                    scoreCounter++;
                    emote =AdvanceResults.WillAdvance.ToEmote();
                }
                else if(undefeated.ChallengeResult == AdvanceResults.TBD)
                {
                    emote =AdvanceResults.TBD.ToEmote();
                }
                else
                {
                    emote = AdvanceResults.WillNotAdvance.ToEmote();
                }
               
                resultString +=
                    $"** {emote} **\n";
                
                for (var i = 0; i < AdvanceList.Count; i++)
                {
                    if (AdvanceList[i].ChallengeResult == AdvanceResults.Undefeated || AdvanceList[i].ChallengeResult == AdvanceResults.WillAdvance )
                    {
                        scoreCounter++;
                        emote =AdvanceResults.WillAdvance.ToEmote();
                    }
                    else if(AdvanceList[i].ChallengeResult == AdvanceResults.TBD)
                    {
                        emote =AdvanceResults.TBD.ToEmote();
                    }
                    else
                    {
                        emote = AdvanceResults.WillNotAdvance.ToEmote();
                    }
                    resultString +=
                            $"** {emote} **\n";
                }
                
                if (nowin.ChallengeResult == AdvanceResults.NoWin)
                {
                    scoreCounter++;
                    emote =AdvanceResults.WillAdvance.ToEmote();
                }
                else if(nowin.ChallengeResult == AdvanceResults.TBD)
                {
                    emote =AdvanceResults.TBD.ToEmote();
                }
                else
                {
                    emote = AdvanceResults.WillNotAdvance.ToEmote();
                }
               
                resultString +=
                    $"** {emote} **\n";
                
                
            }

            else
            {
                // Check if the user picked the correct undefeated team
                switch (undefeated.LegendResult)
                {
                    case AdvanceResults.Undefeated:
                        scoreCounter++;
                        emote =AdvanceResults.WillAdvance.ToEmote();
                        break;
                    case AdvanceResults.TBD:
                        emote =AdvanceResults.TBD.ToEmote();
                        break;
                    default:
                        emote = AdvanceResults.WillNotAdvance.ToEmote();
                        break;
                }
               
                resultString +=
                    $"** {emote} **\n";
                
                // See if the user picked the correct teams to advance
                for (var i = 0; i < AdvanceList.Count; i++)
                {
                    switch (AdvanceList[i].LegendResult)
                    {
                        case AdvanceResults.Undefeated:
                        case AdvanceResults.WillAdvance:
                            scoreCounter++;
                            emote =AdvanceResults.WillAdvance.ToEmote();
                            break;
                        case AdvanceResults.TBD:
                            emote =AdvanceResults.TBD.ToEmote();
                            break;
                        default:
                            emote = AdvanceResults.WillNotAdvance.ToEmote();
                            break;
                    }

                    resultString +=
                            $"** {emote} **\n";
                }
                
                // Check if the user picked the correct no win team
                switch (nowin.LegendResult)
                {
                    case AdvanceResults.NoWin:
                        scoreCounter++;
                        emote =AdvanceResults.WillAdvance.ToEmote();
                        break;
                    case AdvanceResults.TBD:
                        emote =AdvanceResults.TBD.ToEmote();
                        break;
                    default:
                        emote = AdvanceResults.WillNotAdvance.ToEmote();
                        break;
                }
                
                resultString +=
                    $"** {emote} **\n";
                
            }
            
            embed.AddField("Result", resultString, true);
            embed.AddField("Score", $"**{scoreCounter}**");

            return embed;
        }
    }
}