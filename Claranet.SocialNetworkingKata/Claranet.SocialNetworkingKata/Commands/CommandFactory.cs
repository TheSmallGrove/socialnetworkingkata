using Claranet.SocialNetworkingKata.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Claranet.SocialNetworkingKata.Commands
{
    class CommandFactory
    {
        static readonly ISocialCommand Exit = new ExitCommand();
        static readonly ISocialCommand NoOp = new NoOpCommand();
        static readonly ISocialCommand Unknown = new UnknownCommand();
        static readonly Regex Regex = new Regex(@"^((?<command>exit)|(?<user>[^\s]*)\s*(?<command>follows|wall|exit|\-\>)?\s*(?<arg>.*))$", RegexOptions.IgnoreCase);

        private IStorageProvider Storage { get; }

        public CommandFactory(IStorageProvider storage)
        {
            this.Storage = storage;
        }

        public ISocialCommand Create(string input)
        {
            input = input.Trim();

            if (string.IsNullOrEmpty(input))
                return CommandFactory.NoOp;

            var match = CommandFactory.Regex.Match(input);

            if (!match.Success)
                return CommandFactory.Unknown;

            var command = match.Groups["command"].Value;
            var user = match.Groups["user"].Value;
            var arg = match.Groups["arg"].Value;

            switch (command)
            {
                case "exit":
                    return CommandFactory.Exit;
                case "follows":
                    return new FollowCommand(this.Storage, user, arg);
                case "wall":
                    return new WallCommand(this.Storage, user);
                case "->":
                    return new PostCommand(this.Storage, user, arg);
            }

            return new ReadCommand(this.Storage, user);
        }
    }
}
