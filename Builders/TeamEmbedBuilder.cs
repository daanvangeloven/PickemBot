using System.Drawing;
using Discord;
using PickemBot.Models;
using Color = Discord.Color;

namespace PickemBot.Modules
{
    public static class TeamEmbedBuilder
    {
        public static EmbedBuilder TeamAdvanceEmbed(Team team)
        {
            var emote = Emote.Parse(team.EmoteName);

            
            return new EmbedBuilder()
                .WithTitle("Team: " + team.Name)
                .WithImageUrl(team.ImageURL)
                .WithDescription($"Will {team.Name} advance to the next stage?")
                .WithColor((Color) ColorTranslator.FromHtml(team.Colour)); 
        }
    }
}