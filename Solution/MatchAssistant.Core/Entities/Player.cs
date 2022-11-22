namespace MatchAssistant.Core.Entities
{
    public class Player
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int? Rating { get; set; }

        public string Position { get; set; }

        public override string ToString()
        {
            return $"Name: {Name}, Rating: {Rating ?? 0}";
        }
    }
}
