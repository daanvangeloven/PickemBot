using Discord;
using Discord.Rest;
using Discord.WebSocket;
using Microsoft.EntityFrameworkCore;
using PickemBot.Enums;
using PickemBot.Models;

namespace PickemBot.Modules
{
    public static class AdvancePickHandler
    {
        /// <summary>
        /// Handle the challenger pick button press
        /// </summary>
        /// <param name="dbContext">Database Context</param>
        /// <param name="component">Button Component</param>
        /// <param name="picker">Picker</param>
        /// <param name="team">The next team</param>
        /// <param name="lastTeam">The picked Team</param>
        /// <param name="counter"></param>
        /// <param name="prediction">The status of the picked team</param>
        /// <param name="pickChannel">The picking channel</param>
        /// <param name="stage">The stage</param>
        public static async void HandleAdvancePick(DbContext dbContext, SocketMessageComponent component,
            Picker picker, Team team, Team lastTeam, int counter, AdvanceResults prediction, SocketTextChannel pickChannel, EventStage stage = EventStage.Challenger) 
        {
            AdvancePicks picks;
            if (stage == EventStage.Legend)
            {
                picks = picker.LegendPicks!;
            }
            else
            {
                picks = picker.ChallengerPicks!;
            }
            // process the pick into the database
            switch (prediction)
            {
                // nowin pick
                case AdvanceResults.NoWin:
                    picks.NoWin = lastTeam;
                    await dbContext.SaveChangesAsync();
                    break;
                // advance pick
                case AdvanceResults.WillAdvance:
                    picks.AdvanceTeams!.Add(lastTeam);
                    await dbContext.SaveChangesAsync();
                    break;
                // undefeated pick
                case AdvanceResults.Undefeated:
                    picks.Undefeated = lastTeam;
                    await dbContext.SaveChangesAsync();
                    break;
            }
            
            // check if all the picks have been put in
            if (counter == 16)
            {
                // complete picks
                var deleteBtnBuilder = new ComponentBuilder()
                    .WithButton(Discord.ButtonBuilder.CreateDangerButton("Delete this channel", "delete-" + pickChannel.Id));
                
                await pickChannel.SendMessageAsync(
                    $"Nice ones! You got your {stage.ToFriendlyString()} picks sorted. You can check them by using the /picks command. \n Use the button below to delete this channel.", components: deleteBtnBuilder.Build());
            }
            else
            {
                await pickChannel.SendMessageAsync("Nice pick! Onto the next one:");
            
                var buttonBuilder = new ComponentBuilder();
                
                // Check if the no win buttoncan be displayed
                if (picks.NoWin == null)
                {
                    buttonBuilder.WithButton(ButtonBuilder.NoWinButton(picker.ID, team.ID, stage, counter,
                        pickChannel.Id));
                }

                // Check if the not advance button can be displayed
                int totalpicks = 0;
                if (picks.Undefeated != null) totalpicks++;
                if (picks.NoWin != null) totalpicks++;
                totalpicks += picks.AdvanceTeams?.Count ?? 0;
                if (counter - totalpicks < 7)
                {
                    buttonBuilder.WithButton(ButtonBuilder.NotAdvanceButton(picker.ID, team.ID, stage, counter,
                        pickChannel.Id));
                }

                // Check if the advance button can be displayed
                if (picks.AdvanceTeams!.Count < 7)
                {
                    buttonBuilder.WithButton(ButtonBuilder.AdvanceButton(picker.ID, team.ID, stage, counter,
                        pickChannel.Id));
                }

                // Check if the undefeated button can be displayed
                if (picks.Undefeated == null)
                {
                    buttonBuilder.WithButton(ButtonBuilder.UndefeatedButton(picker.ID, team.ID, stage, counter,
                        pickChannel.Id));
                }

                await pickChannel.SendMessageAsync(embed: TeamEmbedBuilder.TeamAdvanceEmbed(team).Build(),
                    components: buttonBuilder.Build());
            }
        }
    }
}