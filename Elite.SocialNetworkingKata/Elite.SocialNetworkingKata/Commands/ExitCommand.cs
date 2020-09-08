using Elite.SocialNetworkingKata.Properties;
using Elite.SocialNetworkingKata.Providers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Elite.SocialNetworkingKata.Commands
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
            if (this.Interaction.IsDebugMode)
                this.Interaction.Warn(Resources.Message_Bye);

            this.Interaction.Exit();
            return Task.CompletedTask;
        }
    }
}
