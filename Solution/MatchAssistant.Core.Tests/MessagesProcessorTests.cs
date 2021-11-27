using MatchAssistant.Core.BusinessLogic;
using MatchAssistant.Core.BusinessLogic.Services;
using MatchAssistant.Core.Entities;
using MatchAssistant.Core.Tests.Infrastructure.Mappers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MatchAssistant.Core.Tests
{
    [TestClass]
    public class MessagesProcessorTests
    {
        private MessagesProcessor target;

        [TestInitialize]
        public void Init()
        {
            var participantsService = new ParticipantsService(
                    new InMemoryGameMapper(),
                    new InMemoryParticipantMapper());

            var chatsService = new ChatsService(
                new InMemoryChatMapper(),
                new InMemoryUserMapper()
                );

            target = new MessagesProcessor(participantsService, chatsService);
        }

        #region Count

        #region Add

        [TestMethod]
        public void SinglePlusIncrementsCount()
        {
            CreateNewGame();

            var message = CreateTextMessageFromUser(1, "Bob", "+");
            target.ProcessMessage(message);

            var countResponse = GetCountCommandResult();
            Assert.AreEqual("1", countResponse);
        }

        [TestMethod]
        public void PlusWithCountAddsCount()
        {
            CreateNewGame();

            var message = CreateTextMessageFromUser(1, "Bob", "+ 2 из вк");
            target.ProcessMessage(message);

            var countResponse = GetCountCommandResult();
            Assert.AreEqual("2", countResponse);
        }

        [TestMethod]
        public void PlusWithCountWithoutNameAddsCount()
        {
            CreateNewGame();

            var message = CreateTextMessageFromUser(1, "Bob", "+2");
            target.ProcessMessage(message);

            var countResponse = GetCountCommandResult();
            Assert.AreEqual("2", countResponse);
        }

        [TestMethod]
        public void PlusWithNameIncrementsCount()
        {
            CreateNewGame();

            var message = CreateTextMessageFromUser(1, "Bob", "+ Вася");
            target.ProcessMessage(message);

            var countResponse = GetCountCommandResult();
            Assert.AreEqual("1", countResponse);
        }

        public void DuplicatePlusWithNameIsIgnored()
        {
            CreateNewGame();

            var message = CreateTextMessageFromUser(1, "Bob", "+ Вася");
            target.ProcessMessage(message);

            var message2 = CreateTextMessageFromUser(1, "Bob", "+ Вася");
            target.ProcessMessage(message2);

            var countResponse = GetCountCommandResult();
            Assert.AreEqual("1", countResponse);
        }

        [TestMethod]
        public void DuplicatedSinglePlusIsIgnored()
        {
            CreateNewGame();

            var message = CreateTextMessageFromUser(1, "Bob", "+");
            target.ProcessMessage(message);

            var message2 = CreateTextMessageFromUser(1, "Bob", "+");
            target.ProcessMessage(message2);

            var countResponse = GetCountCommandResult();
            Assert.AreEqual("1", countResponse);
        }

        [TestMethod]
        public void PlusOneIncrementsCount()
        {
            CreateNewGame();

            var message = CreateTextMessageFromUser(1, "Bob", "+");
            target.ProcessMessage(message);

            var message2 = CreateTextMessageFromUser(1, "Bob", "+1");
            target.ProcessMessage(message2);

            var countResponse = GetCountCommandResult();
            Assert.AreEqual("2", countResponse);
        }

        [TestMethod]
        public void DuplicatePlusOneIncrementsCount()
        {
            CreateNewGame();

            var message = CreateTextMessageFromUser(1, "Bob", "+1");
            target.ProcessMessage(message);

            var message2 = CreateTextMessageFromUser(1, "Bob", "+1");
            target.ProcessMessage(message2);

            var countResponse = GetCountCommandResult();
            Assert.AreEqual("2", countResponse);
        }

        [TestMethod]
        public void PlusNumberIncrementsCount()
        {
            CreateNewGame();

            var message = CreateTextMessageFromUser(1, "Bob", "+2");
            target.ProcessMessage(message);

            var message2 = CreateTextMessageFromUser(1, "Bob", "+3");
            target.ProcessMessage(message2);

            var countResponse = GetCountCommandResult();
            Assert.AreEqual("5", countResponse);
        }

        [TestMethod]
        public void PlusWithCountCanAddCountSeveralTimes()
        {
            CreateNewGame();

            var message = CreateTextMessageFromUser(1, "Bob", "+ 2 из вк");
            target.ProcessMessage(message);

            var message2 = CreateTextMessageFromUser(1, "Bob", "+ 2 из вк");
            target.ProcessMessage(message2);

            var countResponse = GetCountCommandResult();
            Assert.AreEqual("4", countResponse);
        }

        [TestMethod]
        public void PlusWithCountWithoutNameCanAddCountSeveralTimes()
        {
            CreateNewGame();

            var message = CreateTextMessageFromUser(1, "Bob", "+2");
            target.ProcessMessage(message);

            var message2 = CreateTextMessageFromUser(1, "Bob", "+2");
            target.ProcessMessage(message2);

            var countResponse = GetCountCommandResult();
            Assert.AreEqual("4", countResponse);
        }

        #endregion

        #region Remove

        [TestMethod]
        public void SingleMinusDecrementsCount()
        {
            CreateNewGame();

            var message = CreateTextMessageFromUser(1, "Bob", "+");
            target.ProcessMessage(message);

            var message2 = CreateTextMessageFromUser(1, "Bob", "-");
            target.ProcessMessage(message2);

            var countResponse = GetCountCommandResult();
            Assert.AreEqual("0", countResponse);
        }

        [TestMethod]
        public void MinusWithCountRemovesCount()
        {
            CreateNewGame();

            var message = CreateTextMessageFromUser(1, "Bob", "+ 2 из вк");
            target.ProcessMessage(message);

            var message2 = CreateTextMessageFromUser(1, "Bob", "- 2 из вк");
            target.ProcessMessage(message2);

            var countResponse = GetCountCommandResult();
            Assert.AreEqual("0", countResponse);
        }

        [TestMethod]
        public void MinusWithCountWithoutNameRemovesCount()
        {
            CreateNewGame();

            var message = CreateTextMessageFromUser(1, "Bob", "+2");
            target.ProcessMessage(message);

            var message2 = CreateTextMessageFromUser(1, "Bob", "-2");
            target.ProcessMessage(message2);

            var countResponse = GetCountCommandResult();
            Assert.AreEqual("0", countResponse);
        }

        [TestMethod]
        public void MinusWithNameDecrementsCount()
        {
            CreateNewGame();

            var message = CreateTextMessageFromUser(1, "Bob", "+ Вася");
            target.ProcessMessage(message);

            var message2 = CreateTextMessageFromUser(1, "Bob", "- Вася");
            target.ProcessMessage(message2);

            var countResponse = GetCountCommandResult();
            Assert.AreEqual("0", countResponse);
        }

        [TestMethod]
        public void DuplicatedSingleMinusIsIgnored()
        {
            CreateNewGame();

            var message = CreateTextMessageFromUser(1, "Bob", "+");
            target.ProcessMessage(message);

            var message2 = CreateTextMessageFromUser(1, "Bob", "-");
            target.ProcessMessage(message2);

            var message3 = CreateTextMessageFromUser(1, "Bob", "-");
            target.ProcessMessage(message3);

            var countResponse = GetCountCommandResult();
            Assert.AreEqual("0", countResponse);
        }

        [TestMethod]
        public void MinusWithCountCanRemoveCountSeveralTimes()
        {
            CreateNewGame();

            var message = CreateTextMessageFromUser(1, "Bob", "+ 6 из вк");
            target.ProcessMessage(message);

            var message2 = CreateTextMessageFromUser(1, "Bob", "- 2 из вк");
            target.ProcessMessage(message2);

            var message3 = CreateTextMessageFromUser(1, "Bob", "- 2 из вк");
            target.ProcessMessage(message3);

            var countResponse = GetCountCommandResult();
            Assert.AreEqual("2", countResponse);
        }

        [TestMethod]
        public void MinusWithCountWithoutNameCanRemoveCountSeveralTimes()
        {
            CreateNewGame();

            var message = CreateTextMessageFromUser(1, "Bob", "+6");
            target.ProcessMessage(message);

            var message2 = CreateTextMessageFromUser(1, "Bob", "-2");
            target.ProcessMessage(message2);

            var message3 = CreateTextMessageFromUser(1, "Bob", "-2");
            target.ProcessMessage(message3);

            var countResponse = GetCountCommandResult();
            Assert.AreEqual("2", countResponse);
        }

        [TestMethod]
        public void MinusWithBiggerCountWithoutNameRemovesOnlyExistingCount()
        {
            CreateNewGame();

            var message = CreateTextMessageFromUser(1, "Bob", "+2");
            target.ProcessMessage(message);

            var message2 = CreateTextMessageFromUser(1, "Bob", "-4");
            target.ProcessMessage(message2);

            var countResponse = GetCountCommandResult();
            Assert.AreEqual("0", countResponse);
        }

        [TestMethod]
        public void MinusWithBiggerCountRemovesOnlyExistingCount()
        {
            CreateNewGame();

            var message = CreateTextMessageFromUser(1, "Bob", "+ 2 из вк");
            target.ProcessMessage(message);

            var message2 = CreateTextMessageFromUser(1, "Bob", "- 4 из вк");
            target.ProcessMessage(message2);

            var countResponse = GetCountCommandResult();
            Assert.AreEqual("0", countResponse);
        }

        #endregion

        #endregion

        #region List

        #region Add


        [TestMethod]
        public void PlusWithCountUpdatesListWithCorrectWeightAndName()
        {
            CreateNewGame();

            var message = CreateTextMessageFromUser(1, "Bob", "+ 2 из вк");
            target.ProcessMessage(message);

            var listResponse = GetListCommandResult();
            Assert.AreEqual("1-2. из вк - 2\r\n", listResponse);
        }

        [TestMethod]
        public void PlusWithNameUpdatesListWithWholeName()
        {
            CreateNewGame();

            var message = CreateTextMessageFromUser(1, "Bob", "+ Иван Иванов");
            target.ProcessMessage(message);

            var listResponse = GetListCommandResult();
            Assert.AreEqual("1. Иван Иванов\r\n", listResponse);
        }

        #endregion

        #endregion

        #region Not sure

        [TestMethod]
        public void PlusAndMinusIncrementsNotSureCount()
        {
            CreateNewGame();

            var message = CreateTextMessageFromUser(1, "Bob", "+-");
            target.ProcessMessage(message);

            var countResponse = GetCountCommandResult();
            Assert.AreEqual("1±", countResponse);
        }

        [TestMethod]
        public void PlusAndSlashAndMinusIncrementsNotSureCount()
        {
            CreateNewGame();

            var message = CreateTextMessageFromUser(1, "Bob", "+/-");
            target.ProcessMessage(message);

            var countResponse = GetCountCommandResult();
            Assert.AreEqual("1±", countResponse);
        }

        [TestMethod]
        public void PlusAndBackslashAndMinusIncrementsNotSureCount()
        {
            CreateNewGame();

            var message = CreateTextMessageFromUser(1, "Bob", "+\\-");
            target.ProcessMessage(message);

            var countResponse = GetCountCommandResult();
            Assert.AreEqual("1±", countResponse);
        }

        [TestMethod]
        public void PlusAndMinusAsSingleSymbolIncrementsNotSureCount()
        {
            CreateNewGame();

            var message = CreateTextMessageFromUser(1, "Bob", "±");
            target.ProcessMessage(message);

            var countResponse = GetCountCommandResult();
            Assert.AreEqual("1±", countResponse);
        }

        [TestMethod]
        public void PlusAndMinusWithCountAddsNotSuredWithCount()
        {
            CreateNewGame();

            var message = CreateTextMessageFromUser(1, "Bob", "+/- 2 из вк");
            target.ProcessMessage(message);

            var countResponse = GetCountCommandResult();
            Assert.AreEqual("2±", countResponse);
        }

        [TestMethod]
        public void PlusAndMinusWithNameIncrementsNotSureCount()
        {
            CreateNewGame();

            var message = CreateTextMessageFromUser(1, "Bob", "+/- Вася");
            target.ProcessMessage(message);

            var countResponse = GetCountCommandResult();
            Assert.AreEqual("1±", countResponse);
        }

        [TestMethod]
        public void PlusAndNotSureAreBothShown()
        {
            CreateNewGame();

            var message = CreateTextMessageFromUser(1, "Bob", "+/-");
            target.ProcessMessage(message);

            var message2 = CreateTextMessageFromUser(2, "Jack", "+");
            target.ProcessMessage(message2);

            var countResponse = GetCountCommandResult();
            Assert.AreEqual("1 и 1±", countResponse);
        }

        [TestMethod]
        public void UpdateOfSureStateWithTrueIncrementsCountOfSure()
        {
            CreateNewGame();

            var message = CreateTextMessageFromUser(1, "Bob", "+/-");
            target.ProcessMessage(message);

            var message2 = CreateTextMessageFromUser(1, "Bob", "+");
            target.ProcessMessage(message2);

            var countResponse = GetCountCommandResult();
            Assert.AreEqual("1", countResponse);
        }

        [TestMethod]
        public void UpdateOfSureStateWithFalseDosntIncrementCountOfSure()
        {
            CreateNewGame();

            var message = CreateTextMessageFromUser(1, "Bob", "+/-");
            target.ProcessMessage(message);

            var message2 = CreateTextMessageFromUser(1, "Bob", "-");
            target.ProcessMessage(message2);

            var countResponse = GetCountCommandResult();
            Assert.AreEqual("0", countResponse);
        }

        #endregion

        #region Helpers

        private ChatMessage CreateTextMessageFromUser(int userId, string userName, string messageText)
        {
            var message = new ChatMessage
            {
                Author = new ChatUser() { Id = userId, UserName = userName, Name = userName },
                Chat = new GameChat() { Name = "My chat" },
                Text = messageText
            };

            return message;
        }

        private string CreateNewGame()
        {
            var newGameCommand = new ChatMessage
            {
                Author = new ChatUser() { Id = 1, UserName = "Bob" },
                Chat = new GameChat() { Name = "My chat" },
                Text = "/new"
            };

            return target.ProcessMessage(newGameCommand);
        }

        private string GetCountCommandResult()
        {
            var countCommand = new ChatMessage
            {
                Author = new ChatUser() { Id = 1, UserName = "Bob" },
                Chat = new GameChat() { Name = "My chat" },
                Text = "/count"
            };

            return target.ProcessMessage(countCommand);
        }

        private string GetListCommandResult()
        {
            var listCommand = new ChatMessage
            {
                Author = new ChatUser() { Id = 1, UserName = "Bob" },
                Chat = new GameChat() { Name = "My chat" },
                Text = "/list"
            };

            return target.ProcessMessage(listCommand);
        }

        #endregion
    }
}
