using Elite.SocialNetworkingKata.Properties;
using Elite.SocialNetworkingKata.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elite.SocialNetworkingKata.Commands
{
    class ReadCommand : ISocialCommand
    {
        private string User { get; }
        private IStorageProvider Storage { get; }
        private ITimeProvider Time { get; }
        private IInteractionProvider Interaction { get; }

        public ReadCommand(IStorageProvider storage, IInteractionProvider interaction, ITimeProvider time, IDictionary<string, string> arguments)
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
        }

        public async Task Execute()
        {
            try
            {
                var messages = await this.Storage.GetMessagesByUser(this.User);

                foreach (var m in messages)
                    this.Interaction.Write(Resources.Message_ReadFormat, m.Message, this.Time.ToSocialTime(m.Time));
            }
            catch (Exception ex)
            {
                this.Interaction.Error(Resources.Message_Exception, ex.Message);
            }
        }
    }
}
