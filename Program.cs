using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.Net;
using Discord.Rest;
using Discord.WebSocket;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PickemBot.Commands;
using PickemBot.Data;
using PickemBot.Enums;
using PickemBot.Models;
using PickemBot.Modules;
using ButtonBuilder = PickemBot.Modules.ButtonBuilder;

namespace PickemBot
{
    public class Program
    {
        private DiscordSocketClient _client = null!;
        private LoggingService _loggingService = null!;
        private PickemContext _dbContext = null!;
        private SocketGuild _guild = null!;


        private async Task MainAsync()
        {
            _client = new DiscordSocketClient();
            _client.ButtonExecuted += ButtonRouter;
            _dbContext = new PickemContext();

            _loggingService = new LoggingService(_client, new CommandService());
            _client.SlashCommandExecuted += SlashCommandHandler;

            //  You can assign your bot token to a string, and pass that in to connect.
            //  This is, however, insecure, particularly if you plan to have your code hosted in a public repository.
            var token = GlobalVariables.token;
            if (GlobalVariables.Environment == "test")
            {
                token = GlobalVariables.testtoken;
            }

            // Some alternative options would be to keep your token in an Environment Variable or a standalone file.
            // var token = Environment.GetEnvironmentVariable("NameOfYourEnvironmentVariable");
            // var token = File.ReadAllText("token.txt");
            // var token = JsonConvert.DeserializeObject<AConfigurationClass>(File.ReadAllText("config.json")).Token;

            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();
            _client.Ready += Client_Ready;

            // Block this task until the program is closed.
            await Task.Delay(-1);
        }
        
        public static Task Main(string[] args) => new Program().MainAsync();

        /// <summary>
        /// This Task adds the slash commands to the bot
        /// </summary>
        private async Task Client_Ready()
        {
            _guild = _client.GetGuild(GlobalVariables.Environment == "test"
                ? GlobalVariables.testguildid
                : GlobalVariables.guildid);

            List<SlashCommandBuilder> commands = new();

            // Simple hello command to test the communication 
            var globalCommand = new SlashCommandBuilder();
            globalCommand.WithName("hello");
            globalCommand.WithDescription("Command to Test bot communication");
            commands.Add(globalCommand);

            globalCommand = new SlashCommandBuilder();
            globalCommand.WithName("challengers");
            globalCommand.WithDescription("Show all challenger teams with their results");
            commands.Add(globalCommand);

            globalCommand = new SlashCommandBuilder();
            globalCommand.WithName("legends");
            globalCommand.WithDescription("Show all legend teams with their results");
            commands.Add(globalCommand);

            globalCommand = new SlashCommandBuilder();
            globalCommand.WithName("champion");
            globalCommand.WithDescription("Show all champion teams with their results");
            commands.Add(globalCommand);


            try
            {
                // Command for listing a users picks
                await _client.Rest.CreateGlobalCommand(PickCommands.PicksCommand().Build());
                await _loggingService.LogAsync(new LogMessage(LogSeverity.Info, "SlashCommand",
                    "/picks command registered"));
                
                // Command for letting a user input their stage picks
                await _client.Rest.CreateGlobalCommand(PickCommands.PickCommand().Build());
                await _loggingService.LogAsync(new LogMessage(LogSeverity.Info, "SlashCommand",
                    "/pick command registered"));
                
                // Command for adding a team result
                await _client.Rest.CreateGuildCommand(PickCommands.AddResultCommand(_dbContext).Build(), _guild.Id);
                await _loggingService.LogAsync(new LogMessage(LogSeverity.Info, "SlashCommand",
                    "/AddResult command registered"));


            }
            catch (HttpException exception)
            {
                var json = JsonConvert.SerializeObject(exception.Errors, Formatting.Indented);
                Console.WriteLine(json);
            }


            try
            {
                foreach (SlashCommandBuilder builder in commands)
                {
                    // With global commands we don't need the guild.
                    await _client.CreateGlobalApplicationCommandAsync(builder.Build());

                    // log command creation
                    await _loggingService.LogAsync(new LogMessage(LogSeverity.Info, "Slash Command",
                        $"Global Command Created: {builder.Name}"));

                    // Using the ready event is a simple implementation for the sake of the example. Suitable for testing and development.
                    // For a production bot, it is recommended to only run the CreateGlobalApplicationCommandAsync() once for each command.
                }
            }
            catch (HttpException exception)
            {
                // If our command was invalid, we should catch an ApplicationCommandException. This exception contains the path of the error as well as the error message. You can serialize the Error field in the exception to get a visual of where your error is.
                var json = JsonConvert.SerializeObject(exception.Errors, Formatting.Indented);

                // You can send this error somewhere or just print it to the console, for this example we're just going to print it.
                Console.WriteLine(json);
            }
        }

        public async Task ButtonRouter(SocketMessageComponent component)
        {
            await ButtonHandler.MyButtonHandler(component, _loggingService, _dbContext, _guild);
        }


        /// <summary>
        /// Route the slashcommand usage to the correct handler
        /// </summary>
        /// <param name="command">Command data</param>
        private async Task SlashCommandHandler(SocketSlashCommand command)
        {
            switch (command.Data.Name)
            {
                case "hello":
                    await command.RespondAsync($"Hello you dingus");
                    break;
                case "pick":
                    await HandlePickCommand(command);
                    break;
                case "picks":
                    await HandlePicksCommand(command);
                    break;
                case "addresult":
                    await ResultHandler.UpdateTeamResultHandler(command, _dbContext, _loggingService, _client, _guild);
                    break;
                case "challengers":
                    await command.RespondAsync(embed: StageEmbedBuilder.AdvanceEmbed(EventStage.Challenger, _dbContext)
                        .Build());
                    break;
                case "legends":
                    await command.RespondAsync(embed: StageEmbedBuilder.AdvanceEmbed(EventStage.Legend, _dbContext).Build());
                    // await command.RespondAsync($"Legend stage is not ready yet");
                    break;
                case "champions":
                    await command.RespondAsync($"Champions stage is not ready yet");
                    break;
            }
        }

        /// <summary>
        /// This task handles the user pick input
        /// </summary>
        /// <param name="command"></param>
        private async Task HandlePickCommand(SocketSlashCommand command)
        {
            // Check if picker already exists in database
            if (!_dbContext.Pickers.Any(p => p.DiscordID == command.User.Id))
            {
                _dbContext.Pickers.Add(new Picker()
                {
                    ChallengerPicks = new AdvancePicks(),
                    LegendPicks = new AdvancePicks(),
                    ChampionPicks = new ChampionPicks(),
                    DiscordID = command.User.Id
                });
                await _dbContext.SaveChangesAsync();
            }

            EventStage stage = (EventStage) Convert.ToInt32(command.Data.Options.First().Value);

            await _loggingService.LogAsync(new LogMessage(LogSeverity.Info, $"/pick",
                $"User: {command.User.Username}#{command.User.Discriminator} - Started their picks for the {stage} stage"));

            if (stage == EventStage.Champion)
            {
                await command.RespondAsync($"Champion Stage pickems haven't opened yet. Come back later");
            }
            else
            {
                // Refresh picker from database
                Picker picker = _dbContext.Pickers.FirstOrDefault(p => p.DiscordID == command.User.Id)!;

                {
                    // Use this whilest legend stage hasn't started yet
                    // if (stage == "legend")
                    // {
                    //     await command.RespondAsync($"Legend Stage pickems haven't opened yet. Come back later");
                    // }
                    // else
                    // {
                    if (stage == EventStage.Challenger)
                    {
                        picker.ChallengerPicks = new AdvancePicks()
                        {
                            AdvanceTeams = new List<Team>(),
                            NoWin = null,
                            Undefeated = null
                        };
                    }
                    else
                    {
                        picker.LegendPicks = new AdvancePicks()
                        {
                            AdvanceTeams = new List<Team>()
                        };
                    }

                    await _dbContext.SaveChangesAsync();

                    // create a new channel for the picks
                    RestTextChannel newChannel = CreatePickChannel(stage.ToFriendlyString(), command).Result;

                    // Send the starting message
                    await newChannel.SendMessageAsync("Hello " + command.User.Mention +
                                                      " \n Lets get your picks sorted.");

                    // select the first team
                    Team team = (stage == EventStage.Challenger
                        ? _dbContext.Teams.FirstOrDefault(t => t.ID == GlobalVariables.Challengers[0])
                        : _dbContext.Teams.FirstOrDefault(t => t.ID == GlobalVariables.Legends[0]))!;

                    // button id format = pick-[userid]-[teamid]-[stage]-[status]-[counter]-[channelid]
                    var buttonBuilder = ButtonBuilder.AdvanceButtonComponent(picker.ID, team.ID, stage, 0, newChannel.Id);

                    // send the first team embed
                    await newChannel.SendMessageAsync(embed: TeamEmbedBuilder.TeamAdvanceEmbed(team).Build(),
                        components: buttonBuilder.Build());
                }
            }
            // }
        }

        /// <summary>
        /// This task lists a users picks for a certain stage
        /// </summary>
        /// <param name="command"></param>
        private async Task HandlePicksCommand(SocketSlashCommand command)
        {
            SocketGuildUser user = (SocketGuildUser) command.Data.Options.First().Value;
            EventStage stage = (EventStage) Convert.ToInt32(command.Data.Options.Last().Value);


            await _loggingService.LogAsync(new LogMessage(LogSeverity.Info, $"/picks",
                $"User: {command.User.Username}#{command.User.Discriminator} - Requested {stage} picks from {user.Username}#{user.Discriminator}"));

            if (_dbContext.Pickers.FirstOrDefault(p => p.DiscordID == user.Id) != null)
            {
                if (stage is EventStage.Challenger or EventStage.Legend)
                {
                    if (stage == EventStage.Challenger)
                    {
                        try
                        {
                            Picker picker = _dbContext.Pickers
                                .Include("ChallengerPicks")
                                .Include("ChallengerPicks.AdvanceTeams")
                                .Include("ChallengerPicks.Undefeated")
                                .Include("ChallengerPicks.NoWin")
                                .FirstOrDefault(p => p.DiscordID == user.Id)!;
                            if (picker.ChallengerPicks!.AdvanceTeams!.Count == 7 &&
                                picker.ChallengerPicks.Undefeated != null &&
                                picker.ChallengerPicks.NoWin != null)
                            {
                                List<Team> advanceList = picker.ChallengerPicks.AdvanceTeams.ToList();
                                await command.RespondAsync("Getting picks...");


                                await command.Channel.SendMessageAsync(embed: UserPickEmbedBuilder.CreateAdvanceListEmbed(
                                    picker.ChallengerPicks.NoWin, picker.ChallengerPicks.Undefeated, advanceList, stage,
                                    user.Username).Build());
                            }
                            else
                            {
                                await command.RespondAsync($"{user.Nickname} has not made any challenger picks yet");
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                            await command.RespondAsync($"{user.Nickname} has not made any challenger picks yet");
                        }
                    }
                    else
                    {
                        
                        try
                        {
                            Picker picker = _dbContext.Pickers
                                .Include("LegendPicks")
                                .Include("LegendPicks.AdvanceTeams")
                                .Include("LegendPicks.Undefeated")
                                .Include("LegendPicks.NoWin")
                                .FirstOrDefault(p => p.DiscordID == user.Id)!;
                            if (picker.LegendPicks!.AdvanceTeams!.Count == 7 &&
                                picker.LegendPicks.Undefeated != null &&
                                picker.LegendPicks.NoWin != null)
                            {
                                List<Team> advanceList = picker.LegendPicks.AdvanceTeams.ToList();
                                await command.RespondAsync("Getting picks...");


                                await command.Channel.SendMessageAsync(embed: UserPickEmbedBuilder.CreateAdvanceListEmbed(
                                    picker.LegendPicks.NoWin, picker.LegendPicks.Undefeated, advanceList, stage,
                                    user.Username).Build());
                            }
                            else
                            {
                                await command.RespondAsync($"{user.Nickname} has not made any legend picks yet");
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                            await command.RespondAsync($"{user.Nickname} has not made any legend picks yet");
                        }
                        
                        // await command.RespondAsync($"The legend stage pickems haven't opened yet. Come back later");
                    }
                }
                else
                {   
                    await command.RespondAsync($"Generating bracket...");
                    
                    try
                    {
                        await command.Channel.SendMessageAsync(embed: StageEmbedBuilder.ChampionEmbed(command).Build());
                    }
                    catch(HttpException e)
                    {
                        Console.WriteLine(e.Message);
                    }
                    //await command.RespondAsync($"The champion stage pickems haven't opened yet. Come back later");
                }
            }
            else
            {
                await command.RespondAsync($"{user.Username} has not made any picks yet");
            }
        }


        private async Task<RestTextChannel> CreatePickChannel(string stage, SocketSlashCommand command)
        {
            // generate a channel name for the picks
            ulong categoryId = GlobalVariables.Environment == "test"
                ? GlobalVariables.testcategoryID
                : GlobalVariables.categoryID;

            string channelname = $"{stage} Picks: " + command.User.Username;
            RestTextChannel newChannel =
                await _guild.CreateTextChannelAsync(channelname, prop => prop.CategoryId = categoryId);
            await newChannel.AddPermissionOverwriteAsync(command.User,
                new OverwritePermissions(sendMessages: PermValue.Allow, readMessageHistory: PermValue.Allow,
                    viewChannel: PermValue.Allow));
            await command.RespondAsync($"You can enter your picks in the new channel I pinged you in");


            return newChannel;
        }
    }
}