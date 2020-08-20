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
        private IInteractionProvider Interaction { get; }

        public PostCommand(IStorageProvider storage, IInteractionProvider interaction, IDictionary<string, string> arguments)
        {
            this.Interaction = interaction;
            this.Storage = storage;

            string user;
            if (!arguments.TryGetValue("user", out user))
                throw new ArgumentException(nameof(user));
            this.User = user;

            string message;
            if (!arguments.TryGetValue("arg", out message))
                throw new ArgumentException(nameof(message));
            this.Message = message;
        }

        public async Task Execute()
        {
            try
            {
                await this.Storage.AddMessageForUser(this.User, this.Message, DateTime.Now);
                this.Interaction.Warn("post sent");
            }
            catch(Exception ex)
            {
                this.Interaction.Error($"error posting: {ex.Message}");
            }
        }
    }
}
