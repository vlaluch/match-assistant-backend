using MatchAssistant.Domain.Interfaces;
using MatchAssistant.Domain.Core.Entities;

namespace MatchAssistant.Domain
{
    public class TeamsGenerator : ITeamsGenerator
    {
        private const int teamMembersCount = 6;

        public IEnumerable<Player>[] Generate(ICollection<Player> gameParticipants, TeamGenerationAlgorithm algorithm)
        {
            if (gameParticipants == null || gameParticipants.Count == 0)
            {
                return null;
            }

            var fullTeamsCount = gameParticipants.Count / teamMembersCount;
            var remainingCount = gameParticipants.Count - fullTeamsCount * teamMembersCount;
            var teamsCount = remainingCount == 0 || remainingCount == fullTeamsCount ? fullTeamsCount : fullTeamsCount + 1;
            var teams = new List<Player>[teamsCount];

            for (var i = 0; i < teamsCount; i++)
            {
                teams[i] = new List<Player>();
            }

            var playersPool = new List<Player>(gameParticipants);

            CollectTeams(teams, playersPool, algorithm);

            return teams;
        }

        private void CollectTeams(ICollection<Player>[] teams, ICollection<Player> playersPool, TeamGenerationAlgorithm algorithm)
        {
            switch (algorithm)
            {
                case TeamGenerationAlgorithm.Baskets:
                    CollectTeamsUsingBaskets(teams, playersPool);
                    break;
                case TeamGenerationAlgorithm.Snake:
                    CollectTeamsUsingSnake(teams, playersPool);
                    break;
            }
        }

        private static void CollectTeamsUsingSnake(ICollection<Player>[] teams, ICollection<Player> playersPool)
        {
            var ratedPlayers = playersPool.OrderByDescending(player => player.Rating).ThenBy(player => player.Position).ToArray();

            var teamIndex = 0;
            var step = 1;

            for (var playerIndex = 0; playerIndex < ratedPlayers.Length; playerIndex++)
            {
                teams[teamIndex].Add(ratedPlayers[playerIndex]);

                teamIndex += step;

                if (teamIndex > teams.Length - 1 && step == 1)
                {
                    step = -1;
                    teamIndex = teams.Length - 1;
                }
                else if (teamIndex < 0 && step == -1)
                {
                    step = 1;
                    teamIndex = 0;
                }
            }
        }

        private static void CollectTeamsUsingBaskets(ICollection<Player>[] teams, ICollection<Player> playersPool)
        {
            var ratedPlayers = playersPool.OrderByDescending(player => player.Rating).ThenBy(player => player.Position).ToArray();

            var teamIndex = 0;
            var basketIndex = 0;
            int[] indexes = GenerateRandomSequenceWithUniqueValues(teams.Length - 1).ToArray();

            for (var playerIndex = 0; playerIndex < ratedPlayers.Length; playerIndex++)
            {
                if (basketIndex >= teams.Length)
                {
                    indexes = GenerateRandomSequenceWithUniqueValues(teams.Length - 1).ToArray();
                    basketIndex = 0;
                }

                teamIndex = indexes[basketIndex];
                teams[teamIndex].Add(ratedPlayers[playerIndex]);

                basketIndex++;
            }
        }

        private static IEnumerable<int> GenerateRandomSequenceWithUniqueValues(int maxValue)
        {
            var values = new List<int>();

            for (var i = 0; i <= maxValue; i++)
            {
                values.Add(i);
            }

            var sequence = new List<int>();

            var randomGenerator = new Random();

            while (values.Count > 0)
            {
                var index = randomGenerator.Next(0, values.Count);
                sequence.Add(values[index]);
                values.RemoveAt(index);
            }

            return sequence;
        }
    }
}
