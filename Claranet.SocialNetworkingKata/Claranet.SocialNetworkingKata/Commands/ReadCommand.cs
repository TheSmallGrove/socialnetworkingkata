using Claranet.SocialNetworkingKata.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Claranet.SocialNetworkingKata.Commands
{
    class ReadCommand : ISocialCommand
    {
        private string User { get; }
        private IStorageProvider Storage { get; }

        public ReadCommand(IStorageProvider storage, string user)
        {
            this.Storage = storage;
            this.User = user;
        }

        public async Task Execute()
        {
            try
            {
                var messages = await this.Storage.GetMessagesByUser(this.User);

                foreach (var m in messages)
                {
                    Console.WriteLine($"{m.Message} ({m.Time.ToSocialTime()})");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"error reading: {ex.Message}");
            }
        }
    }
}
