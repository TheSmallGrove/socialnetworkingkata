using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Elite.SocialNetworkingKata.Commands
{
    class NoOpCommand : ISocialCommand
    {
        public Task Execute()
        {
            return Task.CompletedTask;
        }
    }
}
