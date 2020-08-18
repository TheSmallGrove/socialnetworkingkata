using Claranet.SocialNetworkingKata.Commands;
using Claranet.SocialNetworkingKata.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Claranet.SocialNetworkingKata
{
    class ConsoleListener
    {
        private CommandFactory Factory { get; }
        private IStorageProvider Storage { get; }

        public object Tokenizer { get; private set; }

        public ConsoleListener()
        {
            this.Storage = new InMemoryProvider();
            this.Factory = new CommandFactory(this.Storage);
        }

        public static Task Run()
        {
            var listener = new ConsoleListener();
            return listener.MainLoop();
        }

        private async Task MainLoop()
        {
            while (true)
            {
                var input = this.Prompt();
                var command = this.Factory.Create(input);
                await command.Execute();
            }
        }

        private string Prompt()
        {
            try
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("> ");
                Console.ForegroundColor = ConsoleColor.White;
                return Console.ReadLine();
            }
            finally
            {
                Console.ResetColor();
            }
        }
    }
}
