using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Claranet.SocialNetworkingKata.Commands
{
    interface ISocialCommand
    {
        Task Execute();
    }
}
