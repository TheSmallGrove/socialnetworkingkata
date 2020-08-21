using Claranet.SocialNetworkingKata.Commands;
using Claranet.SocialNetworkingKata.Providers;
using Lamar;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Xml.Linq;

[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
[assembly: InternalsVisibleTo("Claranet.SocialNetworkingKata.Tests")]

namespace Claranet.SocialNetworkingKata
{
    public class Program
    {
        private ICommandFactory Factory { get; }
        private IStorageProvider Storage { get; }
        private ITimeProvider Time { get; }
        private IInteractionProvider Interaction { get; }

        public Program(IInteractionProvider interaction, ITimeProvider time, IStorageProvider storage, ICommandFactory factory)
        {
            this.Interaction = interaction;
            this.Time = time;
            this.Storage = storage;
            this.Factory = factory;
        }

        private async Task MainLoop()
        {
            await this.Storage.InitializeIfRequired();

            while (true)
            {
                var input = this.Interaction.Read();
                var command = this.Factory.Create(input);
                await command.Execute();
            }
        }

        static void Main(string[] args)
        {
            var arguments = GetArguments();

            using (var container = new Container(_ =>
             {
                 _.Injectable<IDictionary<string, string>>();

                 if (arguments.providerType == "sqlite")
                     _.For<IStorageProvider>().Use<SqlLiteProvider>();
                 else
                     _.For<IStorageProvider>().Use<InMemoryProvider>().Singleton();

                 _.For<ITimeProvider>().Use<SystemTimeProvider>();
                 _.For<IInteractionProvider>().Use(_ => new SystemConsoleInteractionProvider(arguments.isDebugMode));
                 _.For<ICommandParser>().Use<CommandParser>();
                 _.For<ICommandFactory>().Use<CommandFactory>();
                 _.For<ISocialCommand>().Use<FollowCommand>().Named(Commands.Commands.FollowName);
                 _.For<ISocialCommand>().Use<WallCommand>().Named(Commands.Commands.WallName);
                 _.For<ISocialCommand>().Use<ReadCommand>().Named(Commands.Commands.ReadName);
                 _.For<ISocialCommand>().Use<PostCommand>().Named(Commands.Commands.PostName);
                 _.For<ISocialCommand>().Use<ExitCommand>().Named(Commands.Commands.ExitName);
                 _.For<ISocialCommand>().Use<NoOpCommand>().Named(Commands.Commands.NoOpName);
                 _.For<ISocialCommand>().Use<UnknownCommand>().Named(Commands.Commands.UnknownName);
             }))
            {
                var program = container.GetInstance<Program>();
                program.MainLoop().Wait();
            }
        }

        private static (string providerType, bool isDebugMode) GetArguments()
        {
            var provider = Ask("Select provider to use", new[] { "sqlite", "memory" }, 0);
            var debugMode = Ask("Enable debug mode?", new[] { "yes", "no" }, 1);

            if (provider == "sqlite" && File.Exists(SqlLiteProvider.SqlLiteDatabasePath))
            {
                var deletedb = Ask("A sqlite database already exists. Would you like to delete it and reset social?", new[] { "yes", "no" }, 1);

                if (deletedb == "yes")
                    File.Delete(SqlLiteProvider.SqlLiteDatabasePath);
            }

            Console.Clear();
            Console.ResetColor();
            return (provider, debugMode == "yes");
        }

        private static string Ask(string message, string[] allowedValues, int defaultValue)
        {
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write($"{message} ({string.Join('|', allowedValues)}) [{allowedValues[defaultValue]}]: ");
                Console.ResetColor();
                var input = Console.ReadLine();

                if (allowedValues.Contains(input))
                    return input;

                if (string.IsNullOrEmpty(input))
                    return allowedValues[defaultValue];

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error: please enter a valid value between ({string.Join('|', allowedValues)})");
                Console.ResetColor();
            }
        }
    }
}
