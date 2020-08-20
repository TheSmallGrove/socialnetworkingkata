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
            this.Interaction.Error("Unknown command");
            return Task.CompletedTask;
        }
    }
}
