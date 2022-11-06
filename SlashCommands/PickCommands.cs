using System.Collections.Generic;
using System.Text.RegularExpressions;
using Discord;
using PickemBot.Data;
using PickemBot.Models;

namespace PickemBot.Commands
{
    public static class PickCommands
    {
        public static SlashCommandBuilder PickCommand()
        {
            return new SlashCommandBuilder()
                .WithName("pick")
                .WithDescription("Select your picks")
                .AddOption(new SlashCommandOptionBuilder()
                    .WithName("stage")
                    .WithDescription("Which stage do you want to pick?")
                    .WithRequired(true)
                    .AddChoice("Challenger", "1")
                    .AddChoice("Legend", "2")
                    .AddChoice("Champion", "3")
                    .WithType(ApplicationCommandOptionType.Integer)
                    .WithRequired(true));
        }

        public static SlashCommandBuilder PicksCommand()
        {
            return new SlashCommandBuilder()
                .WithName("picks")
                .WithDescription("Shows users picks.")
                .AddOption("user", ApplicationCommandOptionType.User, "The users whos roles you want to be listed",
                    isRequired: true)
                .AddOption(new SlashCommandOptionBuilder()
                    .WithName("stage")
                    .WithDescription("Which stage do you want to pick?")
                    .WithRequired(true)
                    .AddChoice("Challenger", "1")
                    .AddChoice("Legend", "2")
                    .AddChoice("Champion", "3")
                    .WithType(ApplicationCommandOptionType.Integer)
                    .WithRequired(true));
        }
        
        public static SlashCommandBuilder AddResultCommand(PickemContext dbContext)
        {
            List<ApplicationCommandOptionChoiceProperties> options = new();
            // Add all teams as options
            foreach (Team team in dbContext.Teams)
            {
                if (team.Name != "TBD")
                {
                    options.Add(new ApplicationCommandOptionChoiceProperties()
                    {
                        Name = (Regex.Replace(team.Name, @"\s+", "").ToLower()),
                        Value = team.ID
                    });
                }
            } 
            
            return new SlashCommandBuilder()
                .WithName("addresult")
                .WithDescription(".")
                .AddOption(new SlashCommandOptionBuilder()
                    {
                        Choices = options
                    }
                    .WithName("team")
                    .WithDescription("Which team do you want to update?")
                    .WithRequired(true)
                    .WithType(ApplicationCommandOptionType.Integer)
                )
                .AddOption(new SlashCommandOptionBuilder()
                    .WithName("stage")
                    .WithDescription("Which stage do you want to pick?")
                    .WithRequired(true)
                    .AddChoice("Challenger", "1")
                    .AddChoice("Legend", "2")
                    .WithType(ApplicationCommandOptionType.Integer)
                    .WithRequired(true))
                .AddOption(new SlashCommandOptionBuilder()
                    .WithName("result")
                    .WithDescription("Result")
                    .WithRequired(true)
                    .AddChoice("TBD", "4")
                    .AddChoice("Advanced Without Losing", "3")
                    .AddChoice("Advanced", "2")
                    .AddChoice("Did Not Advance", "1")
                    .AddChoice("Eliminated without win", "0")
                    .WithType(ApplicationCommandOptionType.Integer));
        }
    }
}