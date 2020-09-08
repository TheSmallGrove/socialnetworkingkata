using System;
using System.Collections.Generic;
using System.Text;

namespace Elite.SocialNetworkingKata.Providers
{
    public interface IInteractionProvider
    {
        bool IsDebugMode { get; }
        void Write(string format, params object [] args);
        void Write(string message);
        void Error(string format, params object[] args);
        void Error(string message);
        void Warn(string format, params object[] args);
        void Warn(string message);
        string Read();
        void Exit();
    }
}
