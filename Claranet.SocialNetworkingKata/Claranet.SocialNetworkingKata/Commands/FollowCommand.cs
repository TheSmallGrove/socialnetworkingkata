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
        private IStorageProvider Storage { get; }

        public FollowCommand(IStorageProvider storage, string user, string userToFollow)
        {
            this.Storage = storage;
            this.User = user;
            this.UserToFollow = userToFollow;
        }

        public async Task Execute()
        {
            try
            {
                await this.Storage.AddFollowerToUser(this.User, this.UserToFollow);
                Console.WriteLine($"{this.User} now follows {this.UserToFollow}");
            }
            catch(Exception ex)
            {
                Console.WriteLine($"error posting: {ex.Message}");
            }
        }
    }
}
