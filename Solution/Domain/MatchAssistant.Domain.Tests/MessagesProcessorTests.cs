using MatchAssistant.Domain;
using MatchAssistant.Domain.Contracts.Entities;
using MatchAssistant.Domain.Handlers;
using MatchAssistant.Domain.Interfaces;
using MatchAssistant.Messaging.Telegram;
using MatchAssistant.Persistence.Repositories.InMemory;

namespace MatchAssistant.Core.Tests
{
    [TestClass]
    public class MessagesProcessorTests
    {
        private MessagesProcessor target;

        [TestInitialize]
        public void Init()
        {
            var participantRepository = new InMemoryParticipantRepository();
            var gameRepository = new InMemoryGameRepository();

            var handlers = new List<IHandleCommand>
            {
                new GetParticipantsCountCommandHandler(participantRepository, gameRepository),
                new GetParticipantsListCommandHandler(participantRepository, gameRepository),
                new NewGameCommandHandler(gameRepository),
                new SetParticipantsGroupStateCommandHandler(gameRepository, participantRepository)
            };

            target = new MessagesProcessor(handlers);
        }

        #region Count

        #region Add

        [TestMethod]
        public async Task SinglePlusIncrementsCount()
        {
            await CreateNewGameAsync();

            var message = CreateTextMessageFromUser(1, "Bob", "+");
            await target.ProcessMessageAsync(message);

            var countResponse = await GetCountCommandResultAsync();
            Assert.AreEqual("1", countResponse);
        }

        [TestMethod]
        public async Task PlusWithCountAddsCount()
        {
            await CreateNewGameAsync();

            var message = CreateTextMessageFromUser(1, "Bob", "+ 2 из вк");
            await target.ProcessMessageAsync(message);

            var countResponse = await GetCountCommandResultAsync();
            Assert.AreEqual("2", countResponse);
        }

        [TestMethod]
        public async Task PlusWithCountWithoutNameAddsCount()
        {
            await CreateNewGameAsync();

            var message = CreateTextMessageFromUser(1, "Bob", "+2");
            await target.ProcessMessageAsync(message);

            var countResponse = await GetCountCommandResultAsync();
            Assert.AreEqual("2", countResponse);
        }

        [TestMethod]
        public async Task PlusWithNameIncrementsCount()
        {
            await CreateNewGameAsync();

            var message = CreateTextMessageFromUser(1, "Bob", "+ Вася");
            await target.ProcessMessageAsync(message);

            var countResponse = await GetCountCommandResultAsync();
            Assert.AreEqual("1", countResponse);
        }

        public async Task DuplicatePlusWithNameIncrementsCount()
        {
            await CreateNewGameAsync();

            var message = CreateTextMessageFromUser(1, "Bob", "+ Вася");
            await target.ProcessMessageAsync(message);

            var message2 = CreateTextMessageFromUser(1, "Bob", "+ Вася");
            await target.ProcessMessageAsync(message2);

            var countResponse = await GetCountCommandResultAsync();
            Assert.AreEqual("2", countResponse);
        }

        [TestMethod]
        public async Task DuplicatedSinglePlusIsIgnored()
        {
            await CreateNewGameAsync();

            var message = CreateTextMessageFromUser(1, "Bob", "+");
            await target.ProcessMessageAsync(message);

            var message2 = CreateTextMessageFromUser(1, "Bob", "+");
            await target.ProcessMessageAsync(message2);

            var countResponse = await GetCountCommandResultAsync();
            Assert.AreEqual("1", countResponse);
        }

        [TestMethod]
        public async Task PlusOneIncrementsCount()
        {
            await CreateNewGameAsync();

            var message = CreateTextMessageFromUser(1, "Bob", "+");
            await target.ProcessMessageAsync(message);

            var message2 = CreateTextMessageFromUser(1, "Bob", "+1");
            await target.ProcessMessageAsync(message2);

            var countResponse = await GetCountCommandResultAsync();
            Assert.AreEqual("2", countResponse);
        }

        [TestMethod]
        public async Task DuplicatePlusOneIncrementsCount()
        {
            await CreateNewGameAsync();

            var message = CreateTextMessageFromUser(1, "Bob", "+1");
            await target.ProcessMessageAsync(message);

            var message2 = CreateTextMessageFromUser(1, "Bob", "+1");
            await target.ProcessMessageAsync(message2);

            var countResponse = await GetCountCommandResultAsync();
            Assert.AreEqual("2", countResponse);
        }

        [TestMethod]
        public async Task PlusNumberIncrementsCount()
        {
            await CreateNewGameAsync();

            var message = CreateTextMessageFromUser(1, "Bob", "+2");
            await target.ProcessMessageAsync(message);

            var message2 = CreateTextMessageFromUser(1, "Bob", "+3");
            await target.ProcessMessageAsync(message2);

            var countResponse = await GetCountCommandResultAsync();
            Assert.AreEqual("5", countResponse);
        }

        [TestMethod]
        public async Task PlusWithCountCanAddCountSeveralTimes()
        {
            await CreateNewGameAsync();

            var message = CreateTextMessageFromUser(1, "Bob", "+ 2 из вк");
            await target.ProcessMessageAsync(message);

            var message2 = CreateTextMessageFromUser(1, "Bob", "+ 2 из вк");
            await target.ProcessMessageAsync(message2);

            var countResponse = await GetCountCommandResultAsync();
            Assert.AreEqual("4", countResponse);
        }

        [TestMethod]
        public async Task PlusWithCountWithoutNameCanAddCountSeveralTimes()
        {
            await CreateNewGameAsync();

            var message = CreateTextMessageFromUser(1, "Bob", "+2");
            await target.ProcessMessageAsync(message);

            var message2 = CreateTextMessageFromUser(1, "Bob", "+2");
            await target.ProcessMessageAsync(message2);

            var countResponse = await GetCountCommandResultAsync();
            Assert.AreEqual("4", countResponse);
        }

        #endregion

        #region Remove

        [TestMethod]
        public async Task SingleMinusDecrementsCount()
        {
            await CreateNewGameAsync();

            var message = CreateTextMessageFromUser(1, "Bob", "+");
            await target.ProcessMessageAsync(message);

            var message2 = CreateTextMessageFromUser(1, "Bob", "-");
            await target.ProcessMessageAsync(message2);

            var countResponse = await GetCountCommandResultAsync();
            Assert.AreEqual("0", countResponse);
        }

        [TestMethod]
        public async Task MinusWithCountRemovesCount()
        {
            await CreateNewGameAsync();

            var message = CreateTextMessageFromUser(1, "Bob", "+ 2 из вк");
            await target.ProcessMessageAsync(message);

            var message2 = CreateTextMessageFromUser(1, "Bob", "- 2 из вк");
            await target.ProcessMessageAsync(message2);

            var countResponse = await GetCountCommandResultAsync();
            Assert.AreEqual("0", countResponse);
        }

        [TestMethod]
        public async Task MinusWithCountWithoutNameRemovesCount()
        {
            await CreateNewGameAsync();

            var message = CreateTextMessageFromUser(1, "Bob", "+2");
            await target.ProcessMessageAsync(message);

            var message2 = CreateTextMessageFromUser(1, "Bob", "-2");
            await target.ProcessMessageAsync(message2);

            var countResponse = await GetCountCommandResultAsync();
            Assert.AreEqual("0", countResponse);
        }

        [TestMethod]
        public async Task MinusWithNameDecrementsCount()
        {
            await CreateNewGameAsync();

            var message = CreateTextMessageFromUser(1, "Bob", "+ Вася");
            await target.ProcessMessageAsync(message);

            var message2 = CreateTextMessageFromUser(1, "Bob", "- Вася");
            await target.ProcessMessageAsync(message2);

            var countResponse = await GetCountCommandResultAsync();
            Assert.AreEqual("0", countResponse);
        }

        [TestMethod]
        public async Task DuplicatedSingleMinusIsIgnored()
        {
            await CreateNewGameAsync();

            var message = CreateTextMessageFromUser(1, "Bob", "+");
            await target.ProcessMessageAsync(message);

            var message2 = CreateTextMessageFromUser(1, "Bob", "-");
            await target.ProcessMessageAsync(message2);

            var message3 = CreateTextMessageFromUser(1, "Bob", "-");
            await target.ProcessMessageAsync(message3);

            var countResponse = await GetCountCommandResultAsync();
            Assert.AreEqual("0", countResponse);
        }

        [TestMethod]
        public async Task MinusWithCountCanRemoveCountSeveralTimes()
        {
            await CreateNewGameAsync();

            var message = CreateTextMessageFromUser(1, "Bob", "+ 6 из вк");
            await target.ProcessMessageAsync(message);

            var message2 = CreateTextMessageFromUser(1, "Bob", "- 2 из вк");
            await target.ProcessMessageAsync(message2);

            var message3 = CreateTextMessageFromUser(1, "Bob", "- 2 из вк");
            await target.ProcessMessageAsync(message3);

            var countResponse = await GetCountCommandResultAsync();
            Assert.AreEqual("2", countResponse);
        }

        [TestMethod]
        public async Task MinusWithCountWithoutNameCanRemoveCountSeveralTimes()
        {
            await CreateNewGameAsync();

            var message = CreateTextMessageFromUser(1, "Bob", "+6");
            await target.ProcessMessageAsync(message);

            var message2 = CreateTextMessageFromUser(1, "Bob", "-2");
            await target.ProcessMessageAsync(message2);

            var message3 = CreateTextMessageFromUser(1, "Bob", "-2");
            await target.ProcessMessageAsync(message3);

            var countResponse = await GetCountCommandResultAsync();
            Assert.AreEqual("2", countResponse);
        }

        [TestMethod]
        public async Task MinusWithBiggerCountWithoutNameRemovesOnlyExistingCount()
        {
            await CreateNewGameAsync();

            var message = CreateTextMessageFromUser(1, "Bob", "+2");
            await target.ProcessMessageAsync(message);

            var message2 = CreateTextMessageFromUser(1, "Bob", "-4");
            await target.ProcessMessageAsync(message2);

            var countResponse = await GetCountCommandResultAsync();
            Assert.AreEqual("0", countResponse);
        }

        [TestMethod]
        public async Task MinusWithBiggerCountRemovesOnlyExistingCount()
        {
            await CreateNewGameAsync();

            var message = CreateTextMessageFromUser(1, "Bob", "+ 2 из вк");
            await target.ProcessMessageAsync(message);

            var message2 = CreateTextMessageFromUser(1, "Bob", "- 4 из вк");
            await target.ProcessMessageAsync(message2);

            var countResponse = await GetCountCommandResultAsync();
            Assert.AreEqual("0", countResponse);
        }

        #endregion

        [TestMethod]
        public async Task ShouldRestoreCorrectStateForPlusOne()
        {
            await CreateNewGameAsync();

            var message = CreateTextMessageFromUser(1, "Bob", "+1");
            await target.ProcessMessageAsync(message);

            var message2 = CreateTextMessageFromUser(1, "Bob", "+1");
            await target.ProcessMessageAsync(message2);

            var message3 = CreateTextMessageFromUser(1, "Bob", "-2");
            await target.ProcessMessageAsync(message3);

            var message4 = CreateTextMessageFromUser(1, "Bob", "+1");
            await target.ProcessMessageAsync(message4);

            var countResponse = await GetCountCommandResultAsync();
            Assert.AreEqual("1", countResponse);
        }

        [TestMethod]
        public async Task ShouldRestoreCorrectStateForPlusNumber()
        {
            await CreateNewGameAsync();

            var message = CreateTextMessageFromUser(1, "Bob", "+1");
            await target.ProcessMessageAsync(message);

            var message2 = CreateTextMessageFromUser(1, "Bob", "+1");
            await target.ProcessMessageAsync(message2);

            var message3 = CreateTextMessageFromUser(1, "Bob", "-2");
            await target.ProcessMessageAsync(message3);

            var message4 = CreateTextMessageFromUser(1, "Bob", "+2");
            await target.ProcessMessageAsync(message4);

            var countResponse = await GetCountCommandResultAsync();
            Assert.AreEqual("2", countResponse);
        }

        #endregion

        #region List

        #region Add


        [TestMethod]
        public async Task PlusWithCountUpdatesListWithCorrectWeightAndName()
        {
            await CreateNewGameAsync();

            var message = CreateTextMessageFromUser(1, "Bob", "+ 2 из вк");
            await target.ProcessMessageAsync(message);

            var listResponse = await GetListCommandResultAsync();
            Assert.AreEqual("1-2. из вк - 2\r\n", listResponse);
        }

        [TestMethod]
        public async Task PlusWithNameUpdatesListWithWholeName()
        {
            await CreateNewGameAsync();

            var message = CreateTextMessageFromUser(1, "Bob", "+ Иван Иванов");
            await target.ProcessMessageAsync(message);

            var listResponse = await GetListCommandResultAsync();
            Assert.AreEqual("1. Иван Иванов\r\n", listResponse);
        }

        #endregion

        #endregion

        #region Not sure

        [TestMethod]
        public async Task PlusAndMinusIncrementsNotSureCount()
        {
            await CreateNewGameAsync();

            var message = CreateTextMessageFromUser(1, "Bob", "+-");
            await target.ProcessMessageAsync(message);

            var countResponse = await GetCountCommandResultAsync();
            Assert.AreEqual("1±", countResponse);
        }

        [TestMethod]
        public async Task PlusAndSlashAndMinusIncrementsNotSureCount()
        {
            await CreateNewGameAsync();

            var message = CreateTextMessageFromUser(1, "Bob", "+/-");
            await target.ProcessMessageAsync(message);

            var countResponse = await GetCountCommandResultAsync();
            Assert.AreEqual("1±", countResponse);
        }

        [TestMethod]
        public async Task PlusAndBackslashAndMinusIncrementsNotSureCount()
        {
            await CreateNewGameAsync();

            var message = CreateTextMessageFromUser(1, "Bob", "+\\-");
            await target.ProcessMessageAsync(message);

            var countResponse = await GetCountCommandResultAsync();
            Assert.AreEqual("1±", countResponse);
        }

        [TestMethod]
        public async Task PlusAndMinusAsSingleSymbolIncrementsNotSureCount()
        {
            await CreateNewGameAsync();

            var message = CreateTextMessageFromUser(1, "Bob", "±");
            await target.ProcessMessageAsync(message);

            var countResponse = await GetCountCommandResultAsync();
            Assert.AreEqual("1±", countResponse);
        }

        [TestMethod]
        public async Task PlusAndMinusWithCountAddsNotSuredWithCount()
        {
            await CreateNewGameAsync();

            var message = CreateTextMessageFromUser(1, "Bob", "+/- 2 из вк");
            await target.ProcessMessageAsync(message);

            var countResponse = await GetCountCommandResultAsync();
            Assert.AreEqual("2±", countResponse);
        }

        [TestMethod]
        public async Task PlusAndMinusWithNameIncrementsNotSureCount()
        {
            await CreateNewGameAsync();

            var message = CreateTextMessageFromUser(1, "Bob", "+/- Вася");
            await target.ProcessMessageAsync(message);

            var countResponse = await GetCountCommandResultAsync();
            Assert.AreEqual("1±", countResponse);
        }

        [TestMethod]
        public async Task PlusAndNotSureAreBothShown()
        {
            await CreateNewGameAsync();

            var message = CreateTextMessageFromUser(1, "Bob", "+/-");
            await target.ProcessMessageAsync(message);

            var message2 = CreateTextMessageFromUser(2, "Jack", "+");
            await target.ProcessMessageAsync(message2);

            var countResponse = await GetCountCommandResultAsync();
            Assert.AreEqual("1 и 1±", countResponse);
        }

        [TestMethod]
        public async Task UpdateOfSureStateWithTrueIncrementsCountOfSure()
        {
            await CreateNewGameAsync();

            var message = CreateTextMessageFromUser(1, "Bob", "+/-");
            await target.ProcessMessageAsync(message);

            var message2 = CreateTextMessageFromUser(1, "Bob", "+");
            await target.ProcessMessageAsync(message2);

            var countResponse = await GetCountCommandResultAsync();
            Assert.AreEqual("1", countResponse);
        }

        [TestMethod]
        public async Task UpdateOfSureStateWithFalseDosntIncrementCountOfSure()
        {
            await CreateNewGameAsync();

            var message = CreateTextMessageFromUser(1, "Bob", "+/-");
            await target.ProcessMessageAsync(message);

            var message2 = CreateTextMessageFromUser(1, "Bob", "-");
            await target.ProcessMessageAsync(message2);

            var countResponse = await GetCountCommandResultAsync();
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

        private async Task<string> CreateNewGameAsync()
        {
            var newGameCommand = new ChatMessage
            {
                Author = new ChatUser() { Id = 1, UserName = "Bob" },
                Chat = new GameChat() { Name = "My chat" },
                Text = "/new"
            };

            var response = await target.ProcessMessageAsync(newGameCommand);
            return TelegramCommandResponseFormatter.FormatCommandResponse(newGameCommand, response);
        }

        private async Task<string> GetCountCommandResultAsync()
        {
            var countCommand = new ChatMessage
            {
                Author = new ChatUser() { Id = 1, UserName = "Bob" },
                Chat = new GameChat() { Name = "My chat" },
                Text = "/count"
            };

            var response = await target.ProcessMessageAsync(countCommand);
            return TelegramCommandResponseFormatter.FormatCommandResponse(countCommand, response);
        }

        private async Task<string> GetListCommandResultAsync()
        {
            var listCommand = new ChatMessage
            {
                Author = new ChatUser() { Id = 1, UserName = "Bob" },
                Chat = new GameChat() { Name = "My chat" },
                Text = "/list"
            };

            var response = await target.ProcessMessageAsync(listCommand);
            return TelegramCommandResponseFormatter.FormatCommandResponse(listCommand, response);
        }

        #endregion
    }
}
