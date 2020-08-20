using Claranet.SocialNetworkingKata.Properties;
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
        private ITimeProvider Time { get; }

        public PostCommand(IStorageProvider storage, IInteractionProvider interaction, ITimeProvider time, IDictionary<string, string> arguments)
        {
            if (storage == null)
                throw new ArgumentNullException(nameof(storage));

            this.Storage = storage;

            if (interaction == null)
                throw new ArgumentNullException(nameof(interaction));

            this.Interaction = interaction;

            if (time == null)
                throw new ArgumentNullException(nameof(time));

            this.Time = time;

            if (arguments == null)
                throw new ArgumentNullException(nameof(arguments));

            string user;
            if (!arguments.TryGetValue("user", out user))
                throw new ArgumentException(Resources.Exception_MissingArgument, nameof(user));
            this.User = user;

            string message;
            if (!arguments.TryGetValue("arg", out message))
                throw new ArgumentException(Resources.Exception_MissingArgument, nameof(message));
            this.Message = message;
        }

        public async Task Execute()
        {
            try
            {
                await this.Storage.AddMessageForUser(this.User, this.Message, this.Time.Now);
                this.Interaction.Warn(Resources.Message_PostSent);
            }
            catch(Exception ex)
            {
                this.Interaction.Error(Resources.Message_Exception, ex.Message);
            }
        }
    }
}
