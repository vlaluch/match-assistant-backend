using System;

namespace MatchAssistant.Core.Entities
{
    public enum TeamGenerationAlgorithm
    {
        Baskets,
        Snake
    }

    public static class PlayerPosition
    {
        public const string Goalkeeper = "Goalkeeper";
        public const string FieldPlayer = "FieldPlayer";
    }

    public static class ParticipantState
    {
        public const string Accepted = "Accepted";
        public const string Declined = "Declined";
        public const string NotSured = "NotSured";
        public const string Unknown = "Unknown";
    }

    public enum CommandType
    {
        NotCommand,
        NewGame,
        SetState,        
        Count,
        List,
        Ping,
        Shuffle
    }

    [Flags]
    public enum ListFilters
    {
        None = 0,
        Accepted = 1,
        NotSured = 2,
        Declined = 4,
        All = Accepted | NotSured | Declined
    }

    [Flags]
    public enum PingFilters
    {
        None = 0,
        Recent = 1,
        NotSured = 2,        
        All = Recent | NotSured
    }
}
