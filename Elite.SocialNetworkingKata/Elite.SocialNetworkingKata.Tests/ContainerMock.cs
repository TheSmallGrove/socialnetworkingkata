using Elite.SocialNetworkingKata.Commands;
using Elite.SocialNetworkingKata.Providers;
using Lamar;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Elite.SocialNetworkingKata.Tests
{
    static class ContainerMock
    {
        public static (Mock<IContainer> container, Mock<INestedContainer> nested) SetupContainer()
        {
            var storage = new Mock<IStorageProvider>();
            var interaction = new Mock<IInteractionProvider>();
            var time = new SystemTimeProvider();
            return SetupContainer(storage.Object, time, interaction);
        }

        public static (Mock<IContainer> container, Mock<INestedContainer> nested) SetupContainer(IStorageProvider storage)
        {
            var interaction = new Mock<IInteractionProvider>();
            var time = new SystemTimeProvider();
            return SetupContainer(storage, time, interaction);
        }

        public static (Mock<IContainer> container, Mock<INestedContainer> nested) SetupContainer(IStorageProvider storage, ITimeProvider time, Mock<IInteractionProvider> interaction)
        {
            // variable to get injected arguments
            IDictionary<string, string> arguments = new Dictionary<string, string>();

            // setup nested container
            var nestedContainer = new Mock<INestedContainer>();
            // grab arguments injected in nested container to create command
            nestedContainer.Setup(_ => _.Inject<IDictionary<string, string>>(It.IsAny<IDictionary<string, string>>())).Callback<IDictionary<string, string>>(v => arguments = v);
            // setup cor commands
            nestedContainer.Setup(_ => _.GetInstance<ISocialCommand>(Commands.Commands.UnknownName)).Returns(new UnknownCommand(interaction.Object));
            nestedContainer.Setup(_ => _.GetInstance<ISocialCommand>(Commands.Commands.NoOpName)).Returns(new NoOpCommand());
            nestedContainer.Setup(_ => _.GetInstance<ISocialCommand>(Commands.Commands.ExitName)).Returns(new ExitCommand(interaction.Object));
            // create these at runtime because we have to wait for arguments to be injected
            nestedContainer.Setup(_ => _.GetInstance<ISocialCommand>(Commands.Commands.FollowName))
                .Returns((Func<string, ISocialCommand>)(n => new FollowCommand(storage, interaction.Object, arguments)));
            nestedContainer.Setup(_ => _.GetInstance<ISocialCommand>(Commands.Commands.WallName))
                .Returns((Func<string, ISocialCommand>)(n => new WallCommand(storage, interaction.Object, time, arguments)));
            nestedContainer.Setup(_ => _.GetInstance<ISocialCommand>(Commands.Commands.PostName))
                .Returns((Func<string, ISocialCommand>)(n => new PostCommand(storage, interaction.Object, time, arguments)));
            nestedContainer.Setup(_ => _.GetInstance<ISocialCommand>(Commands.Commands.ReadName))
                .Returns((Func<string, ISocialCommand>)(n => new ReadCommand(storage, interaction.Object, time, arguments)));

            // setup container
            var container = new Mock<IContainer>();
            container.Setup(_ => _.GetNestedContainer()).Returns(nestedContainer.Object);

            // that's all folks...
            return (container, nestedContainer);
        }

    }
}
