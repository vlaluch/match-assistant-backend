using MatchAssistant.Core.BusinessLogic;
using MatchAssistant.Core.Entities;
using MatchAssistant.Core.Persistence.InMemory;
    
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
                    new InMemoryGameRepository(),
                    new InMemoryParticipantRepository());

            var chatsService = new ChatsService(
                new InMemoryChatMapper(),
                new InMemoryUserRepository()
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
            target.ProcessMessageAsync(message);

            var countResponse = GetCountCommandResult();
            Assert.AreEqual("1", countResponse);
        }

        [TestMethod]
        public void PlusWithCountAddsCount()
        {
            CreateNewGame();

            var message = CreateTextMessageFromUser(1, "Bob", "+ 2 из вк");
            target.ProcessMessageAsync(message);

            var countResponse = GetCountCommandResult();
            Assert.AreEqual("2", countResponse);
        }

        [TestMethod]
        public void PlusWithCountWithoutNameAddsCount()
        {
            CreateNewGame();

            var message = CreateTextMessageFromUser(1, "Bob", "+2");
            target.ProcessMessageAsync(message);

            var countResponse = GetCountCommandResult();
            Assert.AreEqual("2", countResponse);
        }

        [TestMethod]
        public void PlusWithNameIncrementsCount()
        {
            CreateNewGame();

            var message = CreateTextMessageFromUser(1, "Bob", "+ Вася");
            target.ProcessMessageAsync(message);

            var countResponse = GetCountCommandResult();
            Assert.AreEqual("1", countResponse);
        }

        public void DuplicatePlusWithNameIncrementsCount()
        {
            CreateNewGame();

            var message = CreateTextMessageFromUser(1, "Bob", "+ Вася");
            target.ProcessMessageAsync(message);

            var message2 = CreateTextMessageFromUser(1, "Bob", "+ Вася");
            target.ProcessMessageAsync(message2);

            var countResponse = GetCountCommandResult();
            Assert.AreEqual("2", countResponse);
        }

        [TestMethod]
        public void DuplicatedSinglePlusIsIgnored()
        {
            CreateNewGame();

            var message = CreateTextMessageFromUser(1, "Bob", "+");
            target.ProcessMessageAsync(message);

            var message2 = CreateTextMessageFromUser(1, "Bob", "+");
            target.ProcessMessageAsync(message2);

            var countResponse = GetCountCommandResult();
            Assert.AreEqual("1", countResponse);
        }

        [TestMethod]
        public void PlusOneIncrementsCount()
        {
            CreateNewGame();

            var message = CreateTextMessageFromUser(1, "Bob", "+");
            target.ProcessMessageAsync(message);

            var message2 = CreateTextMessageFromUser(1, "Bob", "+1");
            target.ProcessMessageAsync(message2);

            var countResponse = GetCountCommandResult();
            Assert.AreEqual("2", countResponse);
        }

        [TestMethod]
        public void DuplicatePlusOneIncrementsCount()
        {
            CreateNewGame();

            var message = CreateTextMessageFromUser(1, "Bob", "+1");
            target.ProcessMessageAsync(message);

            var message2 = CreateTextMessageFromUser(1, "Bob", "+1");
            target.ProcessMessageAsync(message2);

            var countResponse = GetCountCommandResult();
            Assert.AreEqual("2", countResponse);
        }

        [TestMethod]
        public void PlusNumberIncrementsCount()
        {
            CreateNewGame();

            var message = CreateTextMessageFromUser(1, "Bob", "+2");
            target.ProcessMessageAsync(message);

            var message2 = CreateTextMessageFromUser(1, "Bob", "+3");
            target.ProcessMessageAsync(message2);

            var countResponse = GetCountCommandResult();
            Assert.AreEqual("5", countResponse);
        }

        [TestMethod]
        public void PlusWithCountCanAddCountSeveralTimes()
        {
            CreateNewGame();

            var message = CreateTextMessageFromUser(1, "Bob", "+ 2 из вк");
            target.ProcessMessageAsync(message);

            var message2 = CreateTextMessageFromUser(1, "Bob", "+ 2 из вк");
            target.ProcessMessageAsync(message2);

            var countResponse = GetCountCommandResult();
            Assert.AreEqual("4", countResponse);
        }

        [TestMethod]
        public void PlusWithCountWithoutNameCanAddCountSeveralTimes()
        {
            CreateNewGame();

            var message = CreateTextMessageFromUser(1, "Bob", "+2");
            target.ProcessMessageAsync(message);

            var message2 = CreateTextMessageFromUser(1, "Bob", "+2");
            target.ProcessMessageAsync(message2);

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
            target.ProcessMessageAsync(message);

            var message2 = CreateTextMessageFromUser(1, "Bob", "-");
            target.ProcessMessageAsync(message2);

            var countResponse = GetCountCommandResult();
            Assert.AreEqual("0", countResponse);
        }

        [TestMethod]
        public void MinusWithCountRemovesCount()
        {
            CreateNewGame();

            var message = CreateTextMessageFromUser(1, "Bob", "+ 2 из вк");
            target.ProcessMessageAsync(message);

            var message2 = CreateTextMessageFromUser(1, "Bob", "- 2 из вк");
            target.ProcessMessageAsync(message2);

            var countResponse = GetCountCommandResult();
            Assert.AreEqual("0", countResponse);
        }

        [TestMethod]
        public void MinusWithCountWithoutNameRemovesCount()
        {
            CreateNewGame();

            var message = CreateTextMessageFromUser(1, "Bob", "+2");
            target.ProcessMessageAsync(message);

            var message2 = CreateTextMessageFromUser(1, "Bob", "-2");
            target.ProcessMessageAsync(message2);

            var countResponse = GetCountCommandResult();
            Assert.AreEqual("0", countResponse);
        }

        [TestMethod]
        public void MinusWithNameDecrementsCount()
        {
            CreateNewGame();

            var message = CreateTextMessageFromUser(1, "Bob", "+ Вася");
            target.ProcessMessageAsync(message);

            var message2 = CreateTextMessageFromUser(1, "Bob", "- Вася");
            target.ProcessMessageAsync(message2);

            var countResponse = GetCountCommandResult();
            Assert.AreEqual("0", countResponse);
        }

        [TestMethod]
        public void DuplicatedSingleMinusIsIgnored()
        {
            CreateNewGame();

            var message = CreateTextMessageFromUser(1, "Bob", "+");
            target.ProcessMessageAsync(message);

            var message2 = CreateTextMessageFromUser(1, "Bob", "-");
            target.ProcessMessageAsync(message2);

            var message3 = CreateTextMessageFromUser(1, "Bob", "-");
            target.ProcessMessageAsync(message3);

            var countResponse = GetCountCommandResult();
            Assert.AreEqual("0", countResponse);
        }

        [TestMethod]
        public void MinusWithCountCanRemoveCountSeveralTimes()
        {
            CreateNewGame();

            var message = CreateTextMessageFromUser(1, "Bob", "+ 6 из вк");
            target.ProcessMessageAsync(message);

            var message2 = CreateTextMessageFromUser(1, "Bob", "- 2 из вк");
            target.ProcessMessageAsync(message2);

            var message3 = CreateTextMessageFromUser(1, "Bob", "- 2 из вк");
            target.ProcessMessageAsync(message3);

            var countResponse = GetCountCommandResult();
            Assert.AreEqual("2", countResponse);
        }

        [TestMethod]
        public void MinusWithCountWithoutNameCanRemoveCountSeveralTimes()
        {
            CreateNewGame();

            var message = CreateTextMessageFromUser(1, "Bob", "+6");
            target.ProcessMessageAsync(message);

            var message2 = CreateTextMessageFromUser(1, "Bob", "-2");
            target.ProcessMessageAsync(message2);

            var message3 = CreateTextMessageFromUser(1, "Bob", "-2");
            target.ProcessMessageAsync(message3);

            var countResponse = GetCountCommandResult();
            Assert.AreEqual("2", countResponse);
        }

        [TestMethod]
        public void MinusWithBiggerCountWithoutNameRemovesOnlyExistingCount()
        {
            CreateNewGame();

            var message = CreateTextMessageFromUser(1, "Bob", "+2");
            target.ProcessMessageAsync(message);

            var message2 = CreateTextMessageFromUser(1, "Bob", "-4");
            target.ProcessMessageAsync(message2);

            var countResponse = GetCountCommandResult();
            Assert.AreEqual("0", countResponse);
        }

        [TestMethod]
        public void MinusWithBiggerCountRemovesOnlyExistingCount()
        {
            CreateNewGame();

            var message = CreateTextMessageFromUser(1, "Bob", "+ 2 из вк");
            target.ProcessMessageAsync(message);

            var message2 = CreateTextMessageFromUser(1, "Bob", "- 4 из вк");
            target.ProcessMessageAsync(message2);

            var countResponse = GetCountCommandResult();
            Assert.AreEqual("0", countResponse);
        }

        #endregion

        [TestMethod]
        public void ShouldRestoreCorrectStateForPlusOne()
        {
            CreateNewGame();

            var message = CreateTextMessageFromUser(1, "Bob", "+1");
            target.ProcessMessageAsync(message);

            var message2 = CreateTextMessageFromUser(1, "Bob", "+1");
            target.ProcessMessageAsync(message2);

            var message3 = CreateTextMessageFromUser(1, "Bob", "-2");
            target.ProcessMessageAsync(message3);

            var message4 = CreateTextMessageFromUser(1, "Bob", "+1");
            target.ProcessMessageAsync(message4);

            var countResponse = GetCountCommandResult();
            Assert.AreEqual("1", countResponse);
        }

        [TestMethod]
        public void ShouldRestoreCorrectStateForPlusNumber()
        {
            CreateNewGame();

            var message = CreateTextMessageFromUser(1, "Bob", "+1");
            target.ProcessMessageAsync(message);

            var message2 = CreateTextMessageFromUser(1, "Bob", "+1");
            target.ProcessMessageAsync(message2);

            var message3 = CreateTextMessageFromUser(1, "Bob", "-2");
            target.ProcessMessageAsync(message3);

            var message4 = CreateTextMessageFromUser(1, "Bob", "+2");
            target.ProcessMessageAsync(message4);

            var countResponse = GetCountCommandResult();
            Assert.AreEqual("2", countResponse);
        }

        #endregion

        #region List

        #region Add


        [TestMethod]
        public void PlusWithCountUpdatesListWithCorrectWeightAndName()
        {
            CreateNewGame();

            var message = CreateTextMessageFromUser(1, "Bob", "+ 2 из вк");
            target.ProcessMessageAsync(message);

            var listResponse = GetListCommandResult();
            Assert.AreEqual("1-2. из вк - 2\r\n", listResponse);
        }

        [TestMethod]
        public void PlusWithNameUpdatesListWithWholeName()
        {
            CreateNewGame();

            var message = CreateTextMessageFromUser(1, "Bob", "+ Иван Иванов");
            target.ProcessMessageAsync(message);

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
            target.ProcessMessageAsync(message);

            var countResponse = GetCountCommandResult();
            Assert.AreEqual("1±", countResponse);
        }

        [TestMethod]
        public void PlusAndSlashAndMinusIncrementsNotSureCount()
        {
            CreateNewGame();

            var message = CreateTextMessageFromUser(1, "Bob", "+/-");
            target.ProcessMessageAsync(message);

            var countResponse = GetCountCommandResult();
            Assert.AreEqual("1±", countResponse);
        }

        [TestMethod]
        public void PlusAndBackslashAndMinusIncrementsNotSureCount()
        {
            CreateNewGame();

            var message = CreateTextMessageFromUser(1, "Bob", "+\\-");
            target.ProcessMessageAsync(message);

            var countResponse = GetCountCommandResult();
            Assert.AreEqual("1±", countResponse);
        }

        [TestMethod]
        public void PlusAndMinusAsSingleSymbolIncrementsNotSureCount()
        {
            CreateNewGame();

            var message = CreateTextMessageFromUser(1, "Bob", "±");
            target.ProcessMessageAsync(message);

            var countResponse = GetCountCommandResult();
            Assert.AreEqual("1±", countResponse);
        }

        [TestMethod]
        public void PlusAndMinusWithCountAddsNotSuredWithCount()
        {
            CreateNewGame();

            var message = CreateTextMessageFromUser(1, "Bob", "+/- 2 из вк");
            target.ProcessMessageAsync(message);

            var countResponse = GetCountCommandResult();
            Assert.AreEqual("2±", countResponse);
        }

        [TestMethod]
        public void PlusAndMinusWithNameIncrementsNotSureCount()
        {
            CreateNewGame();

            var message = CreateTextMessageFromUser(1, "Bob", "+/- Вася");
            target.ProcessMessageAsync(message);

            var countResponse = GetCountCommandResult();
            Assert.AreEqual("1±", countResponse);
        }

        [TestMethod]
        public void PlusAndNotSureAreBothShown()
        {
            CreateNewGame();

            var message = CreateTextMessageFromUser(1, "Bob", "+/-");
            target.ProcessMessageAsync(message);

            var message2 = CreateTextMessageFromUser(2, "Jack", "+");
            target.ProcessMessageAsync(message2);

            var countResponse = GetCountCommandResult();
            Assert.AreEqual("1 и 1±", countResponse);
        }

        [TestMethod]
        public void UpdateOfSureStateWithTrueIncrementsCountOfSure()
        {
            CreateNewGame();

            var message = CreateTextMessageFromUser(1, "Bob", "+/-");
            target.ProcessMessageAsync(message);

            var message2 = CreateTextMessageFromUser(1, "Bob", "+");
            target.ProcessMessageAsync(message2);

            var countResponse = GetCountCommandResult();
            Assert.AreEqual("1", countResponse);
        }

        [TestMethod]
        public void UpdateOfSureStateWithFalseDosntIncrementCountOfSure()
        {
            CreateNewGame();

            var message = CreateTextMessageFromUser(1, "Bob", "+/-");
            target.ProcessMessageAsync(message);

            var message2 = CreateTextMessageFromUser(1, "Bob", "-");
            target.ProcessMessageAsync(message2);

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

            return target.ProcessMessageAsync(newGameCommand);
        }

        private string GetCountCommandResult()
        {
            var countCommand = new ChatMessage
            {
                Author = new ChatUser() { Id = 1, UserName = "Bob" },
                Chat = new GameChat() { Name = "My chat" },
                Text = "/count"
            };

            return target.ProcessMessageAsync(countCommand);
        }

        private string GetListCommandResult()
        {
            var listCommand = new ChatMessage
            {
                Author = new ChatUser() { Id = 1, UserName = "Bob" },
                Chat = new GameChat() { Name = "My chat" },
                Text = "/list"
            };

            return target.ProcessMessageAsync(listCommand);
        }

        #endregion
    }
}
