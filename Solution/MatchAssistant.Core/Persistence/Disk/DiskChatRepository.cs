using MatchAssistant.Core.Entities;
using MatchAssistant.Core.Persistence.Interfaces;
using System.Collections.Generic;
using System.Linq;

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

        public void Create(GameChat chat)
        {
            chats.Add(chat);
            storage.Save(chats);
        }

        public GameChat GetChatByName(string name)
        {
            return chats.FirstOrDefault(chat => chat.Name == name);
        }
    }
}
