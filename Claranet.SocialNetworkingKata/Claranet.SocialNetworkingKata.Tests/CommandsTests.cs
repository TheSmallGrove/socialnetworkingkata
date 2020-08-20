using Castle.Core.Internal;
using Claranet.SocialNetworkingKata.Commands;
using Claranet.SocialNetworkingKata.Entities;
using Claranet.SocialNetworkingKata.Properties;
using Claranet.SocialNetworkingKata.Providers;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Moq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Claranet.SocialNetworkingKata.Tests
{
    public class CommandsTests
    {
        [Fact]
        public async void ExitCommand_Execute()
        {
            // ARRANGE
            var interaction = new Mock<IInteractionProvider>();
            var command = new ExitCommand(interaction.Object);

            // ACT
            await command.Execute();

            // ASSERT
            interaction.Verify(_ => _.Warn(It.IsAny<string>()));
            interaction.Verify(_ => _.Exit());
        }

        [Fact]
        public async void UnknownCommand_Execute()
        {
            // ARRANGE
            var interaction = new Mock<IInteractionProvider>();
            var command = new UnknownCommand(interaction.Object);

            // ACT
            await command.Execute();

            // ASSERT
            interaction.Verify(_ => _.Error(It.IsAny<string>()));
        }

        [Fact]
        public async void NoOpCommand_Execute()
        {
            // ARRANGE
            var command = new NoOpCommand();

            // ACT
            await command.Execute();

            // ASSERT
            Assert.True(true); // no-op must does nothing... :)
        }

        [Fact]
        public void FollowCommand_Ctor_ParamValidation()
        {
            // ARRANGE
            var user = "andrea";
            var arg = "pippo";

            var storage = new Mock<IStorageProvider>();
            var interaction = new Mock<IInteractionProvider>();

            // ACT 

            // missing "arg"
            IDictionary<string, string> arguments = new Dictionary<string, string> { { nameof(user), user } };

            ArgumentException ex1 = Assert.Throws<ArgumentException>(() =>
            {
                var command = new FollowCommand(storage.Object, interaction.Object, arguments);
            });

            // missing "user"
            arguments = new Dictionary<string, string> { { nameof(arg), arg } };

            ArgumentException ex2 = Assert.Throws<ArgumentException>(() =>
            {
                var command = new FollowCommand(storage.Object, interaction.Object, arguments);
            });

            // missing "storage"
            ArgumentNullException ex3 = Assert.Throws<ArgumentNullException>(() =>
            {
                var command = new FollowCommand(null, interaction.Object, arguments);
            });

            // missing "interaction"
            ArgumentNullException ex4 = Assert.Throws<ArgumentNullException>(() =>
            {
                var command = new FollowCommand(storage.Object, null, arguments);
            });

            // missing "arguments"
            ArgumentNullException ex5 = Assert.Throws<ArgumentNullException>(() =>
            {
                var command = new FollowCommand(storage.Object, interaction.Object, null);
            });

            // ASSERT
            Assert.Equal("userToFollow", ex1.ParamName);
            Assert.Equal("user", ex2.ParamName);
            Assert.Equal("storage", ex3.ParamName);
            Assert.Equal("interaction", ex4.ParamName);
            Assert.Equal("arguments", ex5.ParamName);
        }

        [Fact]
        public async void FollowCommand_Execute()
        {
            // ARRANGE
            var user = "andrea";
            var arg = "pippo";

            var arguments = new Dictionary<string, string>
            {
                { nameof(user), user }, { nameof(arg), arg }
            };

            var storage = new Mock<IStorageProvider>();
            var interaction = new Mock<IInteractionProvider>();
            var command = new FollowCommand(storage.Object, interaction.Object, arguments);

            // ACT
            await command.Execute();

            // ASSERT
            storage.Verify(_ => _.AddFollowerToUser(user, arg));
            interaction.Verify(_ => _.Write(Resources.Message_NowFollows, user, arg));
        }

        [Fact]
        public void WallCommand_Ctor_ParamValidation()
        {
            // ARRANGE
            var storage = new Mock<IStorageProvider>();
            var interaction = new Mock<IInteractionProvider>();
            var time = new Mock<ITimeProvider>();

            // ACT 

            // missing "arg"
            IDictionary<string, string> arguments = new Dictionary<string, string> { };

            ArgumentException ex1 = Assert.Throws<ArgumentException>(() =>
            {
                var command = new WallCommand(storage.Object, interaction.Object, time.Object, arguments);
            });

            // missing "storage"
            ArgumentNullException ex2 = Assert.Throws<ArgumentNullException>(() =>
            {
                var command = new WallCommand(null, interaction.Object, time.Object, arguments);
            });

            // missing "interaction"
            ArgumentNullException ex3 = Assert.Throws<ArgumentNullException>(() =>
            {
                var command = new WallCommand(storage.Object, null, time.Object, arguments);
            });

            // missing "time"
            ArgumentNullException ex4 = Assert.Throws<ArgumentNullException>(() =>
            {
                var command = new WallCommand(storage.Object, interaction.Object, null, arguments);
            });

            // missing "arguments"
            ArgumentNullException ex5 = Assert.Throws<ArgumentNullException>(() =>
            {
                var command = new WallCommand(storage.Object, interaction.Object, time.Object, null);
            });

            // ASSERT
            Assert.Equal("user", ex1.ParamName);
            Assert.Equal("storage", ex2.ParamName);
            Assert.Equal("interaction", ex3.ParamName);
            Assert.Equal("time", ex4.ParamName);
            Assert.Equal("arguments", ex5.ParamName);
        }

        [Fact]
        public async void WallCommand_Execute()
        {
            // ARRANGE
            var user = "andrea";

            var arguments = new Dictionary<string, string>
            {
                { nameof(user), user }
            };

            var storage = new Mock<IStorageProvider>();
            var posts = new Post[]
                {
                    new Post{ Author = "andrea", Message = "message1", Time = new DateTime(1970, 1, 1, 0, 0, 1)},
                    new Post{ Author = "andrea", Message = "message2", Time = new DateTime(1970, 1, 1, 0, 0, 2)},
                    new Post{ Author = "andrea", Message = "message3", Time = new DateTime(1970, 1, 1, 0, 0, 3)}
                };
            storage.Setup(_ => _.GetWallByUser(user)).Returns(
                Task.FromResult<IEnumerable<Post>>(posts));

            var interaction = new Mock<IInteractionProvider>();
            var time = new Mock<ITimeProvider>();
            var command = new WallCommand(storage.Object, interaction.Object, time.Object, arguments);

            // ACT
            await command.Execute();

            // ASSERT
            storage.Verify(_ => _.GetWallByUser(user));
            interaction.Verify(_ => _.Write(
                Resources.Message_WallFormat, 
                user, 
                It.IsAny<string>(), 
                It.IsAny<string>()), Times.Exactly(posts.Length));
        }

        [Fact]
        public void ReadCommand_Ctor_ParamValidation()
        {
            // ARRANGE
            var storage = new Mock<IStorageProvider>();
            var interaction = new Mock<IInteractionProvider>();
            var time = new Mock<ITimeProvider>();

            // ACT 

            // missing "arg"
            IDictionary<string, string> arguments = new Dictionary<string, string> { };

            ArgumentException ex1 = Assert.Throws<ArgumentException>(() =>
            {
                var command = new ReadCommand(storage.Object, interaction.Object, time.Object, arguments);
            });

            // missing "storage"
            ArgumentNullException ex2 = Assert.Throws<ArgumentNullException>(() =>
            {
                var command = new ReadCommand(null, interaction.Object, time.Object, arguments);
            });

            // missing "interaction"
            ArgumentNullException ex3 = Assert.Throws<ArgumentNullException>(() =>
            {
                var command = new ReadCommand(storage.Object, null, time.Object, arguments);
            });

            // missing "time"
            ArgumentNullException ex4 = Assert.Throws<ArgumentNullException>(() =>
            {
                var command = new ReadCommand(storage.Object, interaction.Object, null, arguments);
            });

            // missing "arguments"
            ArgumentNullException ex5 = Assert.Throws<ArgumentNullException>(() =>
            {
                var command = new ReadCommand(storage.Object, interaction.Object, time.Object, null);
            });

            // ASSERT
            Assert.Equal("user", ex1.ParamName);
            Assert.Equal("storage", ex2.ParamName);
            Assert.Equal("interaction", ex3.ParamName);
            Assert.Equal("time", ex4.ParamName);
            Assert.Equal("arguments", ex5.ParamName);
        }

        [Fact]
        public async void ReadCommand_Execute()
        {
            // ARRANGE
            var user = "andrea";

            var arguments = new Dictionary<string, string>
            {
                { nameof(user), user }
            };

            var storage = new Mock<IStorageProvider>();
            var posts = new Post[]
                {
                    new Post{ Author = "andrea", Message = "message1", Time = new DateTime(1970, 1, 1, 0, 0, 1)},
                    new Post{ Author = "andrea", Message = "message2", Time = new DateTime(1970, 1, 1, 0, 0, 2)},
                    new Post{ Author = "andrea", Message = "message3", Time = new DateTime(1970, 1, 1, 0, 0, 3)}
                };
            storage.Setup(_ => _.GetMessagesByUser(user)).Returns(
                Task.FromResult<IEnumerable<Post>>(posts));

            var interaction = new Mock<IInteractionProvider>();
            var time = new Mock<ITimeProvider>();
            var command = new ReadCommand(storage.Object, interaction.Object, time.Object, arguments);

            // ACT
            await command.Execute();

            // ASSERT
            storage.Verify(_ => _.GetMessagesByUser(user));
            interaction.Verify(_ => _.Write(Resources.Message_ReadFormat, It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(posts.Length));
        }

        [Fact]
        public void PostCommand_Ctor_ParamValidation()
        {
            // ARRANGE
            var user = "andrea";
            var arg = "this is a simple message!";
            var storage = new Mock<IStorageProvider>();
            var interaction = new Mock<IInteractionProvider>();
            var time = new Mock<ITimeProvider>();

            // ACT 

            // missing "user"
            IDictionary<string, string> arguments = new Dictionary<string, string> { { nameof(user), user } };

            ArgumentException ex1 = Assert.Throws<ArgumentException>(() =>
            {
                var command = new PostCommand(storage.Object, interaction.Object, time.Object, arguments);
            });

            // missing "arg"
            arguments = new Dictionary<string, string> { { nameof(arg), arg } };

            ArgumentException ex2 = Assert.Throws<ArgumentException>(() =>
            {
                var command = new PostCommand(storage.Object, interaction.Object, time.Object, arguments);
            });

            // missing "storage"
            ArgumentNullException ex3 = Assert.Throws<ArgumentNullException>(() =>
            {
                var command = new PostCommand(null, interaction.Object, time.Object, arguments);
            });

            // missing "interaction"
            ArgumentNullException ex4 = Assert.Throws<ArgumentNullException>(() =>
            {
                var command = new PostCommand(storage.Object, null, time.Object, arguments);
            });

            // missing "time"
            ArgumentNullException ex5 = Assert.Throws<ArgumentNullException>(() =>
            {
                var command = new PostCommand(storage.Object, interaction.Object, null, arguments);
            });

            // missing "arguments"
            ArgumentNullException ex6 = Assert.Throws<ArgumentNullException>(() =>
            {
                var command = new PostCommand(storage.Object, interaction.Object, time.Object, null);
            });

            // ASSERT
            Assert.Equal("message", ex1.ParamName);
            Assert.Equal("user", ex2.ParamName);
            Assert.Equal("storage", ex3.ParamName);
            Assert.Equal("interaction", ex4.ParamName);
            Assert.Equal("time", ex5.ParamName);
            Assert.Equal("arguments", ex6.ParamName);
        }

        [Fact]
        public async void PostCommand_Execute()
        {
            // ARRANGE
            var refDate = DateTime.Now;
            var user = "andrea";
            var arg = "this is a simple message!";

            var arguments = new Dictionary<string, string>
            {
                { nameof(user), user }, { nameof(arg), arg }
            };

            var storage = new Mock<IStorageProvider>();
            var interaction = new Mock<IInteractionProvider>();
            var time = new Mock<ITimeProvider>();
            time.SetupGet(_ => _.Now).Returns(refDate);
            
            var command = new PostCommand(storage.Object, interaction.Object, time.Object, arguments);

            // ACT
            await command.Execute();

            // ASSERT
            storage.Verify(_ => _.AddMessageForUser(user, arg, refDate));
            interaction.Verify(_ => _.Warn(It.IsAny<string>()));
        }
    }
}
