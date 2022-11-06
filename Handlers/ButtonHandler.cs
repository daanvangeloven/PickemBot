using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using PickemBot.Data;
using PickemBot.Enums;
using PickemBot.Models;

namespace PickemBot.Modules
{
    public static class ButtonHandler
    {
         // Get input from the buttons
        public static async Task MyButtonHandler(SocketMessageComponent component, LoggingService loggingService, PickemContext dbContext, SocketGuild guild)
        {
            // split the customid into several categories
            // pick button id format = pick-[userid]-[teamid]-[stage]-[status]-[counter]-[channelid]
            // delete channel: delete-[channelid]
            // then split the string by hyphens and continue the logic
            
            string[] splitStrings = component.Data.CustomId.Split('-');
            if (splitStrings[0] == "delete")
            {
                SocketGuildChannel deleteChannel = guild.GetChannel(ulong.Parse(splitStrings[1]));
                await deleteChannel.DeleteAsync();
                await component.RespondAsync($"Channel deleted!");
                loggingService.LogAsync(new LogMessage(LogSeverity.Info, "DeleteButton", $"Channel {deleteChannel.Name} deleted by {component.User.Username}#{component.User.Discriminator}"));
            }
            else if (splitStrings[0] == "pick")
            {
                EventStage stage = (EventStage)int.Parse(splitStrings[3]);

                if (stage is EventStage.Challenger or EventStage.Legend)
                {
                    Team team;

                    // Get pick channel
                    SocketTextChannel pickChannel = guild.GetTextChannel(ulong.Parse(splitStrings[6]));
                    int counter = int.Parse(splitStrings[5]);
                
                    Team lastTeam = (stage == EventStage.Challenger
                        ? dbContext.Teams.FirstOrDefault(t => t.ID == GlobalVariables.Challengers[counter])
                        : dbContext.Teams.FirstOrDefault(t => t.ID == GlobalVariables.Legends[counter]))!;
                
                    counter++;
                    if (counter == 16)
                    {
                        team = lastTeam;
                    }
                    else
                    {
                        team = (stage == EventStage.Challenger
                            ? dbContext.Teams.FirstOrDefault(t => t.ID == GlobalVariables.Challengers[counter])
                            : dbContext.Teams.FirstOrDefault(t => t.ID == GlobalVariables.Legends[counter]))!;   
                    }

                    Picker picker = dbContext.Pickers.FirstOrDefault(p => p.ID == int.Parse(splitStrings[1]))!;


                    // Clear channel messages
                    IAsyncEnumerable<IReadOnlyCollection<IMessage>> messages = pickChannel.GetMessagesAsync(10);
                    await foreach (var messageCollection in messages)
                    {
                        foreach (var message in messageCollection)
                        {
                            await message.DeleteAsync();
                        }
                    }
                    
                    AdvanceResults prediction = (AdvanceResults) int.Parse(splitStrings[4]);

                    // Handle the button press depending on the stage
                    if (stage == EventStage.Challenger)
                    {
                        AdvancePickHandler.HandleAdvancePick(dbContext, component, picker, team, lastTeam, counter,
                            prediction, pickChannel);
                    }
                    else
                    {
                        AdvancePickHandler.HandleAdvancePick(dbContext, component, picker, team, lastTeam, counter,
                            prediction, pickChannel, stage);
                    }
                }
            }
        }
    }
}