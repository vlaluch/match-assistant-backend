using MatchAssistant.Core.BusinessLogic;
using MatchAssistant.Core.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace MatchAssistant.Core.Tests
{
    [TestClass]
    public class TeamsGeneratorTests
    {
        private TeamsGenerator _target;

        [TestInitialize]
        public void Init()
        {
            _target = new TeamsGenerator();
        }

        [TestMethod]
        public void Generate_Snake_ShouldGenerateTeamsWithEqualRatings()
        {
            var players = new List<Player>();
            players.AddRange(GenerateGoalKeepers(3, 1));
            players.AddRange(GenerateFieldPlayers(15, 4));

            var teams = _target.Generate(players, TeamGenerationAlgorithm.Snake).ToArray();

            Assert.AreEqual(3, teams.Length);
            Assert.IsTrue(teams.All(team => HasKeeper(team)));
            Assert.IsTrue(RatingSum(teams[0]) == RatingSum(teams[1]) && RatingSum(teams[1]) == RatingSum(teams[2]));
        }

        [TestMethod]
        public void Generate_Baskets_ShouldGenerateTeamsWithKeepers()
        {
            var players = new List<Player>();
            players.AddRange(GenerateGoalKeepers(3, 1));
            players.AddRange(GenerateFieldPlayers(15, 4));
            var teams = _target.Generate(players, TeamGenerationAlgorithm.Baskets).ToArray();

            Assert.AreEqual(3, teams.Length);
            Assert.IsTrue(teams.All(team => HasKeeper(team)));
        }

        private int RatingSum(IEnumerable<Player> team)
        {
            return team.Sum(player => player.Rating.Value);
        }

        private bool HasKeeper(IEnumerable<Player> team)
        {
            return team.Any(player => player.Position == PlayerPosition.Goalkeeper);
        }

        private IEnumerable<Player> GenerateGoalKeepers(int count, int minRating)
        {
            return GeneratePlayers(count, minRating, PlayerPosition.Goalkeeper);
        }

        private IEnumerable<Player> GenerateFieldPlayers(int count, int minRating)
        {
            return GeneratePlayers(count, minRating, PlayerPosition.FieldPlayer);
        }

        private IEnumerable<Player> GeneratePlayers(int count, int minRating, string position)
        {
            var players = new List<Player>();

            while (players.Count < count)
            {
                players.Add(new Player
                {
                    Position = position,
                    Rating = minRating + players.Count - 1
                });
            }

            return players;
        }
    }
}
