using Claranet.SocialNetworkingKata.Commands;
using Claranet.SocialNetworkingKata.Providers;
using Lamar;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Xml.Linq;

[assembly: InternalsVisibleTo("Claranet.SocialNetworkingKata.Tests")]

namespace Claranet.SocialNetworkingKata
{
    class Program
    {
        private CommandFactory Factory { get; }
        private IStorageProvider Storage { get; }
        private ITimeProvider Time { get; }
        private IInteractionProvider Interaction { get; }

        public Program(IContainer container)
        {
            this.Interaction = container.GetInstance<IInteractionProvider>();
            this.Time = container.GetInstance<ITimeProvider>();
            this.Storage = container.GetInstance<IStorageProvider>();
            this.Factory = new CommandFactory(container);
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

                 if (args.Length > 0 && args[0] == "sqllite")
                     _.For<IStorageProvider>().Use<SqlLiteProvider>();
                 else
                     _.For<IStorageProvider>().Use<InMemoryProvider>();

                 _.For<ITimeProvider>().Use<SystemTimeProvider>();
                 _.For<IInteractionProvider>().Use<SystemConsoleInteractionProvider>();
                 _.For<ISocialCommand>().Use<FollowCommand>().Named("follow");
                 _.For<ISocialCommand>().Use<WallCommand>().Named("wall");
                 _.For<ISocialCommand>().Use<ReadCommand>().Named("read");
                 _.For<ISocialCommand>().Use<PostCommand>().Named("post");
                 _.For<ISocialCommand>().Use<ExitCommand>().Named("exit");
                 _.For<ISocialCommand>().Use<NoOpCommand>().Named("noop");
                 _.For<ISocialCommand>().Use<UnknownCommand>().Named("unknown");
             }))
            {
                var program = new Program(container);
                program.MainLoop().Wait();
            }            
        }
    }
}
