using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Microsoft.EntityFrameworkCore;
using PickemBot.Enums;
using PickemBot.Models;

namespace PickemBot.Data
{
    public static class ResultHandler
    {
        public static async Task UpdateTeamResultHandler(SocketSlashCommand command, PickemContext dbContext, LoggingService loggingService, DiscordSocketClient client, SocketGuild guild)
        {
            Team team = dbContext.Teams.FirstOrDefault(t => t.ID == (long)command.Data.Options.First().Value)!;
            EventStage stage = (EventStage)Convert.ToInt32(command.Data.Options.First(o => o.Name == "stage").Value);
            AdvanceResults result = (AdvanceResults)Convert.ToInt32(command.Data.Options.Last(o => o.Name == "result").Value);

            switch (stage)
            {
                case EventStage.Challenger:
                    team.ChallengeResult = result;
                    break;
                default:
                    team.LegendResult = result;
                    break;
            }
            dbContext.SaveChanges();

            List<Picker> pickers = dbContext.Pickers
                .Include(p => p.ChallengerPicks).ThenInclude(c => c.Undefeated)
                .Include(p => p.ChallengerPicks).ThenInclude(c => c.NoWin)
                .Include(p => p.ChallengerPicks).ThenInclude(c => c.AdvanceTeams)
                .Include(p => p.LegendPicks).ThenInclude(c => c.Undefeated)
                .Include(p => p.LegendPicks).ThenInclude(c => c.NoWin)
                .Include(p => p.LegendPicks).ThenInclude(c => c.AdvanceTeams).ToList();

            foreach (Picker picker in  pickers)
            {
                int points = picker.correctAdvancePicks(stage);
                if (points > 4 && !picker.pingedStage(stage))
                {
                    IUser user = client.GetUserAsync(picker.DiscordID, RequestOptions.Default).Result;
                    try
                    {
                        loggingService.LogAsync(new LogMessage(LogSeverity.Info, "addresult",
                            "Pinged user " + user.Username + " for " + stage.ToFriendlyString() + " picks"));


                        await user.SendMessageAsync("Congrats! You have 5 correct advance picks for the " +
                                                    stage.ToString() + " stage!");
                        picker.setPingedStage(stage, true);
                        dbContext.SaveChanges();
                    }
                    catch (Exception e)
                    {
                        loggingService.LogAsync(new LogMessage(LogSeverity.Error, "addresult",
                            "Failed to ping user " + picker.ID + " for " + stage.ToFriendlyString() + " picks"));
                        loggingService.LogAsync(new LogMessage(LogSeverity.Error, "addresult", e.Message));
                    }

                }
            }
            
            await loggingService.LogAsync(new LogMessage(LogSeverity.Info, $"/update", $"User: {command.User.Username}#{command.User.Discriminator} - Updated {team.Name} {stage.ToFriendlyString()} result to {result}"));
            await command.RespondAsync($"Result has been updated. Thanks for the help!");
        }
    }
    
}