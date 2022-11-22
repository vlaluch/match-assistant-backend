using MatchAssistant.Core.Entities;
using MatchAssistant.Core.Persistence.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace MatchAssistant.Core.Tests.Infrastructure.Mappers
{
    public class InMemoryChatMapper : IChatRepository
    {
        private readonly List<GameChat> chats;

        public InMemoryChatMapper()
        {
            chats = new List<GameChat>();
        }

        public void Create(GameChat chat)
        {
            chats.Add(chat);
        }

        public GameChat GetChatByName(string name)
        {
            return chats.FirstOrDefault(chat => chat.Name == name);
        }
    }
}
