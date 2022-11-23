namespace MatchAssistant.Domain.Core.Entities
{
    public class ParticipantsGroup
    {
        public ParticipantsGroup() { }

        public ParticipantsGroup(string name)
        {
            Name = name;
        }

        public ParticipantsGroup(string name, int count)
        {
            Name = name;
            Count = count;
        }

        public ParticipantsGroup(string name, int count, int stateId)
        {
            Name = name;
            Count = count;
            StateId = stateId;
        }

        public string Name { get; set; }

        public int Count { get; set; } = 1;

        public bool IsSinglePerson { get; set; }

        public int StateId { get; set; }

        public string State
        {
            get
            {
                if (StateId == 1) { return ParticipantState.Accepted; }
                if (StateId == 2) { return ParticipantState.Declined; }
                if (StateId == 3) { return ParticipantState.NotSured; }
                return ParticipantState.Unknown;
            }
            set
            {
                if (value == ParticipantState.Accepted)
                {
                    StateId = 1;
                }
                if (value == ParticipantState.Declined)
                {
                    StateId = 2;
                }
                if (value == ParticipantState.NotSured)
                {
                    StateId = 3;
                }                
            }
        }
    }
}
