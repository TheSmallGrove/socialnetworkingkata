using Claranet.SocialNetworkingKata.Commands;
using Claranet.SocialNetworkingKata.Providers;
using Lamar;
using System;
using System.Collections.Generic;
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
            using (var container = new Container(_ =>
             {
                 _.Injectable<IDictionary<string, string>>();

                 if (args.Length > 0 && args[0] == "sqlite")
                     _.For<IStorageProvider>().Use<SqlLiteProvider>();
                 else
                     _.For<IStorageProvider>().Use<InMemoryProvider>().Singleton();

                 _.For<ITimeProvider>().Use<SystemTimeProvider>();
                 _.For<IInteractionProvider>().Use<SystemConsoleInteractionProvider>();
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
    }
}
