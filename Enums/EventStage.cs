namespace PickemBot.Enums
{
    public enum EventStage
    {
        Challenger = 1,
        Legend = 2,
        Champion = 3
    }
    public static class EventStageExtensions
    {
        public static string ToFriendlyString(this EventStage stage)
        {
            switch (stage)
            {
                case EventStage.Challenger:
                    return "Challenger";
                case EventStage.Legend:
                    return "Legend";
                case EventStage.Champion:
                    return "Champion";
                default:
                    return "Unknown";
            }
        }
    }
}