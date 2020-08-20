using Claranet.SocialNetworkingKata.Providers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Claranet.SocialNetworkingKata.Commands
{
    class ExitCommand : ISocialCommand
    {
        private IInteractionProvider Interaction { get; }

        public ExitCommand(IInteractionProvider interaction)
        {
            this.Interaction = interaction;
        }

        public Task Execute()
        {
            this.Interaction.Warn("Bye.");
            this.Interaction.Exit();
            return Task.CompletedTask;
        }
    }
}
