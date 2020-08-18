using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Claranet.SocialNetworkingKata.Commands
{
    class NoOpCommand : ISocialCommand
    {
        public Task Execute()
        {
            return Task.CompletedTask;
        }
    }
}
