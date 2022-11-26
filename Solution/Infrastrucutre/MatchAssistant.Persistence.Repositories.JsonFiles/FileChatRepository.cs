using MatchAssistant.Domain.Core.Entities;
using MatchAssistant.Domain.Core.Interfaces;

namespace MatchAssistant.Persistence.Repositories.JsonFiles
{
    public class FileChatRepository : IChatRepository
    {
        private List<GameChat> chats;
        private readonly JsonStorage<List<GameChat>> storage;

        public FileChatRepository(JsonStorage<List<GameChat>> storage)
        {
            this.storage = storage;

            chats = new List<GameChat>();

            var storedChats = storage.Load();

            if (storedChats != null)
            {
                chats.AddRange(storedChats);
            }
        }

        public Task CreateAsync(GameChat chat)
        {
            chats.Add(chat);
            storage.Save(chats);

            return Task.CompletedTask;
        }

        public Task<GameChat> GetChatByNameAsync(string name)
        {
            return Task.FromResult(chats.FirstOrDefault(chat => chat.Name == name));
        }
    }
}
