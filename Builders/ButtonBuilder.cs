using Discord;
using PickemBot.Enums;

namespace PickemBot.Modules
{
    public static class ButtonBuilder
    {
        public static ComponentBuilder AdvanceButtonComponent(int userid, int teamid, EventStage stage, int counter, ulong channelid)
        {
            return  new ComponentBuilder()
                .WithButton(ButtonBuilder.NoWinButton(userid, teamid, stage, counter, channelid))
                .WithButton(ButtonBuilder.NotAdvanceButton(userid, teamid, stage, counter, channelid))
                .WithButton(ButtonBuilder.AdvanceButton(userid, teamid, stage, counter, channelid))
                .WithButton(ButtonBuilder.UndefeatedButton(userid, teamid, stage, counter, channelid));
        }
        
        public static Discord.ButtonBuilder NoWinButton(int userid, int teamid, EventStage stage, int counter, ulong channelid)
        {
            // button id format = pick-[userid]-[teamid]-[stage]-[status]-[counter]-[channelid]

            return Discord.ButtonBuilder.CreateDangerButton("Eliminated Without Win",
                $"pick-{userid}-{teamid}-{(int)stage}-0-{counter}-" + channelid);
        }
        
        public static Discord.ButtonBuilder NotAdvanceButton(int userid, int teamid, EventStage stage, int counter, ulong channelid)
        {
            // button id format = pick-[userid]-[teamid]-[stage]-[status]-[counter]-[channelid]

            return Discord.ButtonBuilder.CreateSecondaryButton("Will Not Advance",
                $"pick-{userid}-{teamid}-{(int)stage}-1-{counter}-" + channelid);
        }
        
        public static Discord.ButtonBuilder AdvanceButton(int userid, int teamid, EventStage stage, int counter, ulong channelid)
        {
            // button id format = pick-[userid]-[teamid]-[stage]-[status]-[counter]-[channelid]

            return Discord.ButtonBuilder.CreatePrimaryButton("Will Advance",
                $"pick-{userid}-{teamid}-{(int)stage}-2-{counter}-" + channelid);
        }
        
        public static Discord.ButtonBuilder UndefeatedButton(int userid, int teamid, EventStage stage, int counter, ulong channelid)
        {
            // button id format = pick-[userid]-[teamid]-[stage]-[status]-[counter]-[channelid]

            return Discord.ButtonBuilder.CreateSuccessButton("Will Advance Undefeated",
                $"pick-{userid}-{teamid}-{(int)stage}-3-{counter}-" + channelid);
        }
        
    }
}