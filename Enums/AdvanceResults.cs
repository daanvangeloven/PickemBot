using Discord;

namespace PickemBot.Enums
{
    public enum AdvanceResults
    {
        NoWin = 0,
        WillNotAdvance = 1,
        WillAdvance = 2,
        Undefeated = 3,
        TBD = 4
        
    }

    public static class AdvanceResultsExtensions
    {
        public static string ToPredictionString(this AdvanceResults me)
        {
            switch (me)
            {
                case AdvanceResults.NoWin:
                    return "Eliminated Without Winning";
                case AdvanceResults.WillNotAdvance:
                    return "Will Not Advance";
                case AdvanceResults.WillAdvance:
                    return "Will Advance";
                case AdvanceResults.Undefeated:
                    return "Will Advance Undefeated";
                case AdvanceResults.TBD:
                    return "TBD";
                default:
                    return "Unkown Result";
            }
        }

        public static string ToResultString(this AdvanceResults me)
        {
            switch (me)
            {
                case AdvanceResults.NoWin:
                    return "Eliminated Without Winning";
                case AdvanceResults.WillNotAdvance:
                    return "Did Not Advance";
                case AdvanceResults.WillAdvance:
                    return "Advanced";
                case AdvanceResults.Undefeated:
                    return "Advanced Undefeated";
                case AdvanceResults.TBD:
                    return "TBD";
                default:
                    return "Unkown Result";
            }
        }

        public static Emoji ToEmote(this AdvanceResults me)
        {
            switch (me)
            {
                case AdvanceResults.NoWin:
                    return Emoji.Parse(":o:");
                case AdvanceResults.WillNotAdvance:
                    return Emoji.Parse(":x:");
                case AdvanceResults.WillAdvance:
                    return Emoji.Parse(":white_check_mark:");
                case AdvanceResults.Undefeated:
                    return Emoji.Parse(":trophy:");
                case AdvanceResults.TBD:
                    return Emoji.Parse(":grey_question:");
                default:
                    return Emoji.Parse(":grey_question:");
            }
        }
    }
}