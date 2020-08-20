using Claranet.SocialNetworkingKata.Providers;
using Lamar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Claranet.SocialNetworkingKata.Commands
{
    class CommandFactory
    {
        static readonly Regex Regex = new Regex(@"^((?<command>exit)|(?<user>[^\s]*)\s*(?<command>follows|wall|exit|\-\>)?\s*(?<arg>.*))$", RegexOptions.IgnoreCase);

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
                    return nested.GetInstance<ISocialCommand>("noop");

                var match = CommandFactory.Regex.Match(input);

                if (!match.Success)
                    return nested.GetInstance<ISocialCommand>("unknown");

                IDictionary<string, string> arguments = new Dictionary<string, string>();

                var command = match.Groups["command"].Value;
                arguments.Add("user", match.Groups["user"].Value);
                arguments.Add("arg", match.Groups["arg"].Value);
                nested.Inject<IDictionary<string, string>>(arguments);

                switch (command)
                {
                    case "exit":
                        return nested.GetInstance<ISocialCommand>("exit");
                    case "follows":
                        return nested.GetInstance<ISocialCommand>("follow");
                    case "wall":
                        return nested.GetInstance<ISocialCommand>("wall");
                    case "->":
                        return nested.GetInstance<ISocialCommand>("post");
                }

                return nested.GetInstance<ISocialCommand>("read");
            }
        }
    }
}
