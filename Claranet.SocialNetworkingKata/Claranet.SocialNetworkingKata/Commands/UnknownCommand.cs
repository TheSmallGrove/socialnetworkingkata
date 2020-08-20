using Claranet.SocialNetworkingKata.Properties;
using Claranet.SocialNetworkingKata.Providers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Claranet.SocialNetworkingKata.Commands
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
