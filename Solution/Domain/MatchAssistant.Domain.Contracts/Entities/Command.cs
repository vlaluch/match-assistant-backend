﻿namespace MatchAssistant.Domain.Contracts.Entities
{
    public class Command
    {
        public ChatMessage Message { get; }
        public CommandType CommandType { get; }

        public Command(ChatMessage message, CommandType commandType)
        {
            Message = message;
            CommandType = commandType;
        }
    }
}
