using MatchAssistant.Domain.Core.Entities;
using MatchAssistant.Domain.Core.Interfaces;

namespace MatchAssistant.Persistence.Repositories.InMemory
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
