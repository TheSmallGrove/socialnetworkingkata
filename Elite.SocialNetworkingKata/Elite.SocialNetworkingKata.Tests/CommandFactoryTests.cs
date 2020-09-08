using Elite.SocialNetworkingKata.Commands;
using Elite.SocialNetworkingKata.Providers;
using Lamar;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Elite.SocialNetworkingKata.Tests
{
    public class CommandFactoryTests
    {
        [Theory]
        [InlineData("Alice -> I love the weather today", typeof(PostCommand))]
        [InlineData("Bob -> Damn! We lost!", typeof(PostCommand))]
        [InlineData("Bob -> Good game though.", typeof(PostCommand))]
        [InlineData("Alice", typeof(ReadCommand))]
        [InlineData("Bob", typeof(ReadCommand))]
        [InlineData("Charlie -> I'm in New York today! Anyone wants to have a coffee?", typeof(PostCommand))]
        [InlineData("Charlie follows Alice", typeof(FollowCommand))]
        [InlineData("Charlie wall", typeof(WallCommand))]
        [InlineData("Charlie follows Bob", typeof(FollowCommand))]
        [InlineData("", typeof(NoOpCommand))]
        [InlineData("exit", typeof(ExitCommand))]
        [InlineData("aaa xxx bbb", typeof(UnknownCommand))]
        public void CommandFactory_Should_Create_Command_From_Input(string input, Type commandType)
        {
            // ARRANGE
            var containers = ContainerMock.SetupContainer();
            var parser = new CommandParser();
            CommandFactory factory = new CommandFactory(containers.container.Object, parser);

            // ACT
            var command = factory.Create(input);

            // ASSERT
            Assert.IsType(commandType, command);
            containers.nested.Verify(_ => _.GetInstance<ISocialCommand>(It.IsAny<string>()), Times.Once);

            if (commandType != typeof(NoOpCommand))
                containers.nested.Verify(_ => _.Inject<IDictionary<string, string>>(It.IsAny<IDictionary<string, string>>()), Times.Once);

            containers.nested.Verify(_ => _.Dispose(), Times.Once);
        }
    }
}
