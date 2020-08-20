using Claranet.SocialNetworkingKata.Properties;
using Claranet.SocialNetworkingKata.Providers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Claranet.SocialNetworkingKata.Commands
{
    class FollowCommand : ISocialCommand
    {
        private string User { get; }
        private string UserToFollow { get; }
        private IInteractionProvider Interaction { get; }
        private IStorageProvider Storage { get; }

        public FollowCommand(IStorageProvider storage, IInteractionProvider interaction, IDictionary<string, string> arguments)
        {
            if (storage == null)
                throw new ArgumentNullException(nameof(storage));

            this.Storage = storage;

            if (interaction == null)
                throw new ArgumentNullException(nameof(interaction));

            this.Interaction = interaction;

            if (arguments == null)
                throw new ArgumentNullException(nameof(arguments));

            string user;
            if (!arguments.TryGetValue("user", out user))
                throw new ArgumentException(Resources.Exception_MissingArgument, nameof(user));
            this.User = user;

            string userToFollow;
            if (!arguments.TryGetValue("arg", out userToFollow))
                throw new ArgumentException(Resources.Exception_MissingArgument, nameof(userToFollow));
            this.UserToFollow = userToFollow;
        }

        public async Task Execute()
        {
            try
            {
                await this.Storage.AddFollowerToUser(this.User, this.UserToFollow);
                this.Interaction.Warn(Resources.Message_NowFollows, this.User, this.UserToFollow);
            }
            catch(Exception ex)
            {
                this.Interaction.Error(Resources.Message_Exception, ex.Message);
            }
        }
    }
}
