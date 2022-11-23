using MatchAssistant.Core.Entities;
using MatchAssistant.Core.Persistence.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MatchAssistant.Core.Persistence.InMemory
{
    public class InMemoryChatRepository : IChatRepository
    {
        private readonly List<GameChat> chats;

        public InMemoryChatRepository()
        {
            chats = new List<GameChat>();
        }

        public Task CreateAsync(GameChat chat)
        {
            chats.Add(chat);
            return Task.CompletedTask;
        }

        public Task<GameChat> GetChatByNameAsync(string name)
        {
            return Task.FromResult(chats.FirstOrDefault(chat => chat.Name == name));
        }
    }
}
