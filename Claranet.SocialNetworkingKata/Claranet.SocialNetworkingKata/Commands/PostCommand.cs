using Claranet.SocialNetworkingKata.Providers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Claranet.SocialNetworkingKata.Commands
{
    class PostCommand : ISocialCommand
    {
        private string User { get; }
        private string Message { get; }
        private IStorageProvider Storage { get; }

        public PostCommand(IStorageProvider storage, string user, string message)
        {
            this.Storage = storage;
            this.User = user;
            this.Message = message;
        }

        public async Task Execute()
        {
            try
            {
                await this.Storage.AddMessageForUser(this.User, this.Message, DateTime.Now);
                Console.WriteLine("post sent");
            }
            catch(Exception ex)
            {
                Console.WriteLine($"error posting: {ex.Message}");
            }
        }
    }
}
