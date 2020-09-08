using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Elite.SocialNetworkingKata.Commands
{
    public interface ISocialCommand
    {
        Task Execute();
    }
}
