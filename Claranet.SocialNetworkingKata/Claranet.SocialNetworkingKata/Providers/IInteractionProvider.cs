using System;
using System.Collections.Generic;
using System.Text;

namespace Claranet.SocialNetworkingKata.Providers
{
    interface IInteractionProvider
    {
        void Write(string message);
        void Error(string message);
        void Warn(string message);
        string Read();
        void Exit();
    }
}
