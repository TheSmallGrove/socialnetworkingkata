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
            this.Storage = storage;
            this.Interaction = interaction;

            string user;
            if (!arguments.TryGetValue("user", out user))
                throw new ArgumentException(nameof(user));
            this.User = user;

            string userToFollow;
            if (!arguments.TryGetValue("arg", out userToFollow))
                throw new ArgumentException(nameof(userToFollow));
            this.UserToFollow = userToFollow;
        }

        public async Task Execute()
        {
            try
            {
                await this.Storage.AddFollowerToUser(this.User, this.UserToFollow);
                this.Interaction.Write($"{this.User} now follows {this.UserToFollow}");
            }
            catch(Exception ex)
            {
                this.Interaction.Write($"error posting: {ex.Message}");
            }
        }
    }
}
