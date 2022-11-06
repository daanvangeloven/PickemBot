using System;
using System.Drawing;
using System.IO;
using System.Linq;
using CoreHtmlToImage;
using Discord;
using Discord.WebSocket;
using Microsoft.EntityFrameworkCore;
using PickemBot.Data;
using PickemBot.Enums;
using PickemBot.Models;
using ImageFormat = CoreHtmlToImage.ImageFormat;


namespace PickemBot.Modules
{
    public static class StageEmbedBuilder
    {
        public static EmbedBuilder AdvanceEmbed(EventStage stage, PickemContext dbContext)
        {
            int[] teams;

            if (stage == EventStage.Challenger)
            {
                teams = GlobalVariables.Challengers;
            }
            else
            {
                teams = GlobalVariables.Legends;
            }
            
            EmbedBuilder advanceEmbed = new EmbedBuilder();
            
            advanceEmbed.WithTitle($"{stage.ToFriendlyString()} Results")
                .WithDescription($"These are the current results for the {stage.ToFriendlyString()} stage.");


            string teamdata = "";
            string resultdata = "";

            
            foreach (int teamid in teams)
            {
                Team team = dbContext.Teams.FirstOrDefault(t => t.ID == teamid);
                teamdata += $"**{Emote.Parse(team.EmoteName)} {team.Name}** \n";
                if (stage == EventStage.Challenger)
                {
                    resultdata += $"**{team.ChallengeResult.ToEmote()} {team.ChallengeResult.ToResultString()}** \n";

                }
                else
                {
                    resultdata += $"**{team.LegendResult.ToEmote()} {team.LegendResult.ToResultString()}** \n";

                }
                
            }
            
            
            advanceEmbed.AddField("**Team**", teamdata, true);
                
            advanceEmbed.AddField("**Result**", resultdata, true);

            return advanceEmbed;
        }
        
        public static  EmbedBuilder ChampionEmbed(SocketSlashCommand command)
        {
            EmbedBuilder embed = new();
            embed.WithTitle("Champion");
            embed.WithDescription("The champion of the event is:");
            
            string path = Path.GetTempPath() + "image.jpg";

            
            var converter = new HtmlConverter();
            var html =
                "<style>\n@import 'https://fonts.googleapis.com/css?family=Roboto+Slab:400,700';\nhtml {\n  font-size: 1rem;\n}\n\nbody {\n  background: #36393f;\n}\n\n.bracket {\n  display: inline-block;\n  position: absolute;\n\n  white-space: nowrap;\n  font-size: 0;\n}\n.bracket .round {\n  display: inline-block;\n  vertical-align: middle;\n}\n.bracket .round .winners > div {\n  display: inline-block;\n  vertical-align: middle;\n}\n.bracket .round .winners > div.matchups .matchup:last-child {\n  margin-bottom: 0 !important;\n}\n.bracket .round .winners > div.matchups .matchup .participants {\n  border-radius: 0.25rem;\n  overflow: hidden;\n}\n.bracket .round .winners > div.matchups .matchup .participants .participant {\n  box-sizing: border-box;\n  color: #858585;\n  border-left: 0.25rem solid #858585;\n  background: white;\n  width: 14rem;\n  height: 3rem;\n  box-shadow: 0 2px 2px 0 rgba(0, 0, 0, 0.12);\n}\n.bracket .round .winners > div.matchups .matchup .participants .participant.winner {\n  color: #60c645;\n  border-color: #60c645;\n}\n.bracket .round .winners > div.matchups .matchup .participants .participant.loser {\n  color: #dc563f;\n  border-color: #dc563f;\n}\n.bracket .round .winners > div.matchups .matchup .participants .participant:not(:last-child) {\n  border-bottom: thin solid #f0f2f2;\n}\n.bracket .round .winners > div.matchups .matchup .participants .participant span {\n  margin: 0 1.25rem;\n  line-height: 3;\n  font-size: 1rem;\n  font-family: \"Roboto Slab\";\n}\n.bracket .round .winners > div.connector.filled .line, .bracket .round .winners > div.connector.filled.bottom .merger:after, .bracket .round .winners > div.connector.filled.top .merger:before {\n  border-color: #60c645;\n}\n.bracket .round .winners > div.connector .line, .bracket .round .winners > div.connector .merger {\n  box-sizing: border-box;\n  width: 2rem;\n  display: inline-block;\n  vertical-align: top;\n}\n.bracket .round .winners > div.connector .line {\n  border-bottom: thin solid #c0c0c8;\n  height: 4rem;\n}\n.bracket .round .winners > div.connector .merger {\n  position: relative;\n  height: 8rem;\n}\n.bracket .round .winners > div.connector .merger:before, .bracket .round .winners > div.connector .merger:after {\n  content: \"\";\n  display: block;\n  box-sizing: border-box;\n  width: 100%;\n  height: 50%;\n  border: 0 solid;\n  border-color: #c0c0c8;\n}\n.bracket .round .winners > div.connector .merger:before {\n  border-right-width: thin;\n  border-top-width: thin;\n}\n.bracket .round .winners > div.connector .merger:after {\n  border-right-width: thin;\n  border-bottom-width: thin;\n}\n.bracket .round.quarterfinals .winners:not(:last-child) {\n  margin-bottom: 2rem;\n}\n.bracket .round.quarterfinals .winners .matchups .matchup:not(:last-child) {\n  margin-bottom: 2rem;\n}\n.bracket .round.semifinals .winners .matchups .matchup:not(:last-child) {\n  margin-bottom: 10rem;\n}\n.bracket .round.semifinals .winners .connector .merger {\n  height: 16rem;\n}\n.bracket .round.semifinals .winners .connector .line {\n  height: 8rem;\n}\n.bracket .round.finals .winners .connector .merger {\n  height: 3rem;\n}\n.bracket .round.finals .winners .connector .line {\n  height: 1.5rem;\n}\n</style>\n\n\n<div class=\"bracket\">\n  <section class=\"round quarterfinals\">\n    <div class=\"winners\">\n      <div class=\"matchups\">\n        <div class=\"matchup\">\n          <div class=\"participants\">\n            <div class=\"participant winner\"><span>Heroic</span></div>\n            <div class=\"participant\"><span>Faze</span></div>\n          </div>\n        </div>\n        <div class=\"matchup\">\n          <div class=\"participants\">\n            <div class=\"participant\"><span>Team Liquid</span></div>\n            <div class=\"participant winner\"><span>MOUZ</span></div>\n          </div>\n        </div>\n      </div>\n      <div class=\"connector\">\n        <div class=\"merger\"></div>\n        <div class=\"line\"></div>\n      </div>\n    </div>\n    <div class=\"winners\">\n      <div class=\"matchups\">\n        <div class=\"matchup\">\n          <div class=\"participants\">\n            <div class=\"participant\"><span>Cloud9</span></div>\n            <div class=\"participant winner\"><span>Na'Vi</span></div>\n          </div>\n        </div>\n        <div class=\"matchup\">\n          <div class=\"participants\">\n            <div class=\"participant\"><span>Furia</span></div>\n            <div class=\"participant winner\"><span>Team Vitality</span></div>\n          </div>\n        </div>\n      </div>\n      <div class=\"connector\">\n        <div class=\"merger\"></div>\n        <div class=\"line\"></div>\n      </div>\n    </div>\n  </section>\n  <section class=\"round semifinals\">\n    <div class=\"winners\">\n      <div class=\"matchups\">\n        <div class=\"matchup\">\n          <div class=\"participants\">\n            <div class=\"participant winner\"><span>Heroic</span></div>\n            <div class=\"participant\"><span>MOUZ</span></div>\n          </div>\n        </div>\n        <div class=\"matchup\">\n          <div class=\"participants\">\n            <div class=\"participant winner\"><span>Na'Vi</span></div>\n            <div class=\"participant\"><span>Team Vitality</span></div>\n          </div>\n        </div>\n      </div>\n      <div class=\"connector\">\n        <div class=\"merger\"></div>\n        <div class=\"line\"></div>\n      </div>\n    </div>\n  </section>\n  <section class=\"round finals\">\n    <div class=\"winners\">\n      <div class=\"matchups\">\n        <div class=\"matchup\">\n          <div class=\"participants\">\n            <div class=\"participant winner\"><span>Heroic</span></div>\n            <div class=\"participant\"><span>Team Vitality</span></div>\n          </div>\n        </div>\n      </div>\n    </div>\n  </section>\n</div>";
                var bytes = converter.FromHtmlString(html, quality:100, format: ImageFormat.Png);
            File.WriteAllBytes( Path.Combine(path), bytes);
            
            // Convert HTML to PNG
            embed.WithImageUrl("attachment://" + path);
            
            command.Channel.SendFileAsync(path, embed: embed.Build());
            
            return embed;
        }
    } 
}