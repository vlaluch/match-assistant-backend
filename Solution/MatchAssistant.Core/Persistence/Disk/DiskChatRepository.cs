using MatchAssistant.Core.Entities;
using MatchAssistant.Core.Persistence.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MatchAssistant.Core.Persistence.Disk
{
    public class DiskChatRepository : IChatRepository
    {
        private List<GameChat> chats;
        private readonly JsonStorage<List<GameChat>> storage;

        public DiskChatRepository(JsonStorage<List<GameChat>> storage)
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
