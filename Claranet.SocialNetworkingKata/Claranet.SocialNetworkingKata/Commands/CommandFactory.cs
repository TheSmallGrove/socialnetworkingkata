using Claranet.SocialNetworkingKata.Providers;
using Lamar;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Claranet.SocialNetworkingKata.Commands
{
    class CommandFactory
    {
        static readonly Regex Regex = new Regex(@"^((?<command>exit)|(?<user>.*?)(\s+(?<command>.*?)(\s+(?<arg>.*?))?)?)$", RegexOptions.IgnoreCase);

        private IContainer Container { get; }

        public CommandFactory(IContainer container)
        {
            this.Container = container;
        }

        public ISocialCommand Create(string input)
        {
            using (var nested = this.Container.GetNestedContainer())
            {
                input = input.Trim();

                if (string.IsNullOrEmpty(input))
                    return nested.GetInstance<ISocialCommand>(Commands.NoOpName);

                var match = CommandFactory.Regex.Match(input);

                if (!match.Success)
                    return nested.GetInstance<ISocialCommand>(Commands.UnknownName);

                IDictionary<string, string> arguments = new Dictionary<string, string>();

                var command = match.Groups["command"].Value;
                arguments.Add("user", match.Groups["user"].Value);
                arguments.Add("arg", match.Groups["arg"].Value);
                nested.Inject<IDictionary<string, string>>(arguments);

                switch (command)
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
        }
    }
}
