using Elite.SocialNetworkingKata.Providers;
using Lamar;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Elite.SocialNetworkingKata.Commands
{
    public interface ICommandFactory
    {
        ISocialCommand Create(string input);
    }

    class CommandFactory : ICommandFactory
    {
        private IContainer Container { get; }
        private ICommandParser Parser { get; }

        public CommandFactory(IContainer container, ICommandParser parser)
        {
            this.Container = container;
            this.Parser = parser;
        }

        public ISocialCommand Create(string input)
        {
            using (var nested = this.Container.GetNestedContainer())
            {
                try
                {
                    var parsed = this.Parser.Parse(input);

                    if (!parsed.HasValue)
                        return nested.GetInstance<ISocialCommand>(Commands.NoOpName);

                    IDictionary<string, string> arguments = new Dictionary<string, string>();
                    arguments.Add("user", parsed.Value.user);
                    arguments.Add("arg", parsed.Value.args);
                    nested.Inject<IDictionary<string, string>>(arguments);

                    switch (parsed.Value.command)
                    {
                        case Commands.ExitCode:
                            return nested.GetInstance<ISocialCommand>(Commands.ExitName);
                        case Commands.FollowCode:
                            return nested.GetInstance<ISocialCommand>(Commands.FollowName);
                        case Commands.WallCode:
                            return nested.GetInstance<ISocialCommand>(Commands.WallName);
                        case Commands.PostCode:
                            return nested.GetInstance<ISocialCommand>(Commands.PostName);
                        case "":
                            break;
                        default:
                            return nested.GetInstance<ISocialCommand>(Commands.UnknownName);
                    }

                    return nested.GetInstance<ISocialCommand>(Commands.ReadName);
                }
                catch (FormatException)
                {
                    return nested.GetInstance<ISocialCommand>(Commands.UnknownName);
                }
            }
        }
    }
}
