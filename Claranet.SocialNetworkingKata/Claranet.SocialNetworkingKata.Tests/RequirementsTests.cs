using Claranet.SocialNetworkingKata.Commands;
using Claranet.SocialNetworkingKata.Properties;
using Claranet.SocialNetworkingKata.Providers;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

[assembly: CollectionBehavior(DisableTestParallelization = true)]

namespace Claranet.SocialNetworkingKata.Tests
{
    public class ProviderFixture : IDisposable
    {
        public Mock<IInteractionProvider> Interaction { get; private set; }
        public IStorageProvider Provider { get; private set; }
        public ICommandFactory Factory { get; private set; }

        public ProviderFixture()
        {
            this.Interaction = new Mock<IInteractionProvider>();
            this.ResetInteraction();

            this.Provider = new InMemoryProvider();
            var time = new SystemTimeProvider();
            var container = ContainerMock.SetupContainer(this.Provider, time, this.Interaction);
            var parser = new CommandParser();
            this.Factory = new CommandFactory(container.container.Object, parser);
        }

        public void Dispose()
        {
            this.Provider = null;
        }

        internal void ResetInteraction()
        {
            this.Interaction.Reset();
            this.Interaction.SetupGet(_ => _.IsDebugMode).Returns(true);
        }
    }

    public class IntegrationTestOrdering : ITestCaseOrderer
    {
        public IEnumerable<TTestCase> OrderTestCases<TTestCase>(IEnumerable<TTestCase> testCases) 
            where TTestCase : ITestCase
        {
            var result = testCases.ToList();
            result.Sort((x, y) => StringComparer.OrdinalIgnoreCase.Compare(x.TestMethod.Method.Name, y.TestMethod.Method.Name));
            return result;
        }
    }

    [TestCaseOrderer("Claranet.SocialNetworkingKata.Tests.IntegrationTestOrdering", "Claranet.SocialNetworkingKata.Tests")]
    public class RequirementsTests : IClassFixture<ProviderFixture>
    {
        private ProviderFixture Fixture { get; }

        public RequirementsTests(ProviderFixture fixture)
        {
            this.Fixture = fixture;
        }

        [Fact]
        public async Task Test01()
        {
            // ARRANGE
            this.Fixture.ResetInteraction();

            // ACT
            var command = this.Fixture.Factory.Create("Alice -> I love the weather today");
            await command.Execute();

            // ASSERT
            this.Fixture.Interaction.Verify(_ => _.Warn(Resources.Message_PostSent), Times.Once);

            var posts = await this.Fixture.Provider.GetMessagesByUser("Alice");
            Assert.True(posts.Count() == 1);
            Assert.True(posts.ElementAt(0).Author == "Alice");
            Assert.True(posts.ElementAt(0).Message == "I love the weather today");
        }

        [Fact]
        public async Task Test02()
        {
            // ARRANGE
            this.Fixture.ResetInteraction();

            // ACT
            var command = this.Fixture.Factory.Create("Bob -> Damn! We lost!");
            await command.Execute();

            // ASSERT
            this.Fixture.Interaction.Verify(_ => _.Warn(Resources.Message_PostSent), Times.Once);

            var posts = await this.Fixture.Provider.GetMessagesByUser("Bob");
            Assert.True(posts.Count() == 1);
            Assert.True(posts.ElementAt(0).Author == "Bob");
            Assert.True(posts.ElementAt(0).Message == "Damn! We lost!");
        }

        [Fact]
        public async Task Test03()
        {
            // ARRANGE
            this.Fixture.ResetInteraction();

            // ACT
            var command = this.Fixture.Factory.Create("Bob -> Good game though.");
            await command.Execute();

            // ASSERT
            this.Fixture.Interaction.Verify(_ => _.Warn(Resources.Message_PostSent), Times.Once);

            var posts = await this.Fixture.Provider.GetMessagesByUser("Bob");
            Assert.True(posts.Count() == 2);
            Assert.True(posts.ElementAt(0).Author == "Bob");
            Assert.True(posts.ElementAt(0).Message == "Good game though.");
            Assert.True(posts.ElementAt(1).Author == "Bob");
            Assert.True(posts.ElementAt(1).Message == "Damn! We lost!");
        }

        [Fact]
        public async Task Test04()
        {
            // ARRANGE
            this.Fixture.ResetInteraction();

            // ACT
            var command = this.Fixture.Factory.Create("Alice");
            await command.Execute();

            // ASSERT
            this.Fixture.Interaction.Verify(_ => _.Write(Resources.Message_ReadFormat, "I love the weather today", It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task Test05()
        {
            // ARRANGE
            this.Fixture.ResetInteraction();

            // ACT
            var command = this.Fixture.Factory.Create("Bob");
            await command.Execute();

            // ASSERT
            this.Fixture.Interaction.Verify(_ => _.Write(Resources.Message_ReadFormat, "Good game though.", It.IsAny<string>()), Times.Once);
            this.Fixture.Interaction.Verify(_ => _.Write(Resources.Message_ReadFormat, "Damn! We lost!", It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task Test06()
        {
            // ARRANGE
            this.Fixture.ResetInteraction();

            // ACT
            var command = this.Fixture.Factory.Create("Charlie -> I'm in New York today! Anyone wants to have a coffee?");
            await command.Execute();

            // ASSERT
            this.Fixture.Interaction.Verify(_ => _.Warn(Resources.Message_PostSent), Times.Once);

            var posts = await this.Fixture.Provider.GetMessagesByUser("Charlie");
            Assert.True(posts.Count() == 1);
            Assert.True(posts.ElementAt(0).Author == "Charlie");
            Assert.True(posts.ElementAt(0).Message == "I'm in New York today! Anyone wants to have a coffee?");
        }

        [Fact]
        public async Task Test07()
        {
            // ARRANGE
            this.Fixture.ResetInteraction();

            // ACT
            var command = this.Fixture.Factory.Create("Charlie follows Alice");
            await command.Execute();

            // ASSERT
            this.Fixture.Interaction.Verify(_ => _.Warn(Resources.Message_NowFollows, "Charlie", "Alice"), Times.Once);
        }

        [Fact]
        public async Task Test08()
        {
            // ARRANGE
            this.Fixture.ResetInteraction();

            // ACT
            var command = this.Fixture.Factory.Create("Charlie wall");
            await command.Execute();

            // ASSERT
            this.Fixture.Interaction.Verify(_ => _.Write(Resources.Message_WallFormat, "Charlie", "I'm in New York today! Anyone wants to have a coffee?", It.IsAny<string>()), Times.Once);
            this.Fixture.Interaction.Verify(_ => _.Write(Resources.Message_WallFormat, "Alice", "I love the weather today", It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task Test09()
        {
            // ARRANGE
            this.Fixture.ResetInteraction();

            // ACT
            var command = this.Fixture.Factory.Create("Charlie follows Bob");
            await command.Execute();

            // ASSERT
            this.Fixture.Interaction.Verify(_ => _.Warn(Resources.Message_NowFollows, "Charlie", "Bob"), Times.Once);
        }

        [Fact]
        public async Task Test10()
        {
            // ARRANGE
            this.Fixture.ResetInteraction();

            // ACT
            var command = this.Fixture.Factory.Create("Charlie wall");
            await command.Execute();

            // ASSERT
            this.Fixture.Interaction.Verify(_ => _.Write(Resources.Message_WallFormat, "Charlie", "I'm in New York today! Anyone wants to have a coffee?", It.IsAny<string>()), Times.Once);
            this.Fixture.Interaction.Verify(_ => _.Write(Resources.Message_WallFormat, "Bob", "Good game though.", It.IsAny<string>()), Times.Once);
            this.Fixture.Interaction.Verify(_ => _.Write(Resources.Message_WallFormat, "Bob", "Damn! We lost!", It.IsAny<string>()), Times.Once);
            this.Fixture.Interaction.Verify(_ => _.Write(Resources.Message_WallFormat, "Alice", "I love the weather today", It.IsAny<string>()), Times.Once);
        }
    }
}
