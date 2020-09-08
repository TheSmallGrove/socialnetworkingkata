using Elite.SocialNetworkingKata.Properties;
using Elite.SocialNetworkingKata.Providers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Elite.SocialNetworkingKata.Commands
{
    class UnknownCommand : ISocialCommand
    {
        private IInteractionProvider Interaction { get; }
        public UnknownCommand(IInteractionProvider interaction)
        {
            this.Interaction = interaction;
        }

        public Task Execute()
        {
            this.Interaction.Error(Resources.Message_UnknownCommand);
            return Task.CompletedTask;
        }
    }
}
