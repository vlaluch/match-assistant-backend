using MatchAssistant.Domain.Contracts.Entities;
using Telegram.Bot.Types;

namespace MatchAssistant.Messaging.Telegram
{
    public static class TelegramMessagesConverter
    {
        public static ChatMessage ToChatMessage(this Message message)
        {
            return new ChatMessage
            {
                Text = message.Text,
                Author = GetAuthor(message.From),
                Chat = GetChat(message.Chat)
            };
        }

        private static ChatUser GetAuthor(User user)
        {
            return new ChatUser
            {
                Id = user.Id,
                Name = GetUserName(user),
                UserName = user.Username
            };
        }

        private static GameChat GetChat(Chat chat)
        {
            return new GameChat
            {
                Id = chat.Id,
                Name = GetChatName(chat)
            };
        }

        private static string GetUserName(User user)
        {
            if (!string.IsNullOrEmpty(user.FirstName) && !string.IsNullOrEmpty(user.LastName))
            {
                return $"{user.FirstName} {user.LastName}";
            }
            if (!string.IsNullOrEmpty(user.FirstName))
            {
                return $"{user.FirstName}";
            }
            if (!string.IsNullOrEmpty(user.LastName))
            {
                return $"{user.LastName}";
            }

            return "Unknown user";
        }

        private static string GetChatName(Chat chat)
        {
            return !string.IsNullOrEmpty(chat.Title) ? chat.Title : $"{chat.FirstName} {chat.LastName}";
        }
    }
}