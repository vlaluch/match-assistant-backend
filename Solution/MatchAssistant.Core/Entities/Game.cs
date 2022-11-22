using System;
using System.Collections.Generic;

namespace MatchAssistant.Core.Entities
{
    public class Game
    {
        public Game(string title, DateTime date)
        {
            Title = title;
            Date = date;
            Participants = new List<ParticipantsGroup>();
        }

        public Game(int id, string title, DateTime date) : this(title, date)
        {
            Id = id;
        }

        public int Id { get; set; }

        public string Title { get; set; }

        public DateTime Date { get; set; }

        public List<ParticipantsGroup> Participants { get; set; }
    }
}
