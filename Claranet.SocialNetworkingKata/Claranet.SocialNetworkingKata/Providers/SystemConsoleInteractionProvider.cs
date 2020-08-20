using System;
using System.Collections.Generic;
using System.Text;

namespace Claranet.SocialNetworkingKata.Providers
{
    class SystemConsoleInteractionProvider : IInteractionProvider
    {
        public string Read()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("> ");
            Console.ResetColor();
            return Console.ReadLine();
        }

        public void Write(string message)
        {
            Console.WriteLine(message);
            Console.ResetColor();
        }

        public void Warn(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(message);
            Console.ResetColor();
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
