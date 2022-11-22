namespace MatchAssistant.Core.Entities
{
    public class ChatMessage
    {
        public GameChat Chat { get; set; }

        public ChatUser Author { get; set; }

        public string Text { get; set; }
    }
}
