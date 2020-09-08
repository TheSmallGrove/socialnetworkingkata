using System;
using System.Collections.Generic;
using System.Text;

namespace Elite.SocialNetworkingKata.Providers
{
    class SystemConsoleInteractionProvider : IInteractionProvider
    {
        public bool IsDebugMode { get; }

        public SystemConsoleInteractionProvider(bool isDebugMode = false)
        {
            this.IsDebugMode = isDebugMode;
        }

        public string Read()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("> ");
            Console.ResetColor();
            return Console.ReadLine();
        }

        public void Write(string format, params object[] args)
        {
            this.Write(string.Format(format, args));
        }

        public void Write(string message)
        {
            Console.WriteLine(message);
            Console.ResetColor();
        }

        public void Warn(string format, params object[] args)
        {
            this.Warn(string.Format(format, args));
        }

        public void Warn(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        public void Error(string format, params object[] args)
        {
            this.Error(string.Format(format, args));
        }

        public void Error(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        public void Exit()
        {
            Environment.Exit(0);
        } 
    }
}
