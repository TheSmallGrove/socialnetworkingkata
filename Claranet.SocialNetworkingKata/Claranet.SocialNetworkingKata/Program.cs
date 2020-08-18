using Claranet.SocialNetworkingKata.Commands;
using Claranet.SocialNetworkingKata.Providers;
using System;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Claranet.SocialNetworkingKata
{
    class Program
    {
        private CommandFactory Factory { get; }
        private IStorageProvider Storage { get; }

        public object Tokenizer { get; private set; }

        public Program(string storageType)
        {
            this.Storage = (storageType == "sqllite") ? (IStorageProvider)new SqlLiteProvider() : (IStorageProvider)new InMemoryProvider();
            this.Factory = new CommandFactory(this.Storage);
        }

        private async Task MainLoop()
        {
            await this.Storage.InitializeIfRequired();

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

        static void Main(string[] args)
        {
            var type = (args.Length == 1) ? args[0] : "sqllite";

            var program = new Program(type);
            program.MainLoop().Wait();
        }
    }
}
