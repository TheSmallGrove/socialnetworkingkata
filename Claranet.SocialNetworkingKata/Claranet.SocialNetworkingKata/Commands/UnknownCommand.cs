using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Claranet.SocialNetworkingKata.Commands
{
    class UnknownCommand : ISocialCommand
    {
        public Task Execute()
        {
            Console.WriteLine("Unknown command");
            return Task.CompletedTask;
        }
    }
}
