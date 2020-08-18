using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Claranet.SocialNetworkingKata.Commands
{
    class ExitCommand : ISocialCommand
    {
        public Task Execute()
        {
            Environment.Exit(0);
            return Task.CompletedTask;
        }
    }
}
