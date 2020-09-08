using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Elite.SocialNetworkingKata.Commands
{
    interface ICommandParser
    {
        (string user, string command, string args)? Parse(string input);
    }

    class CommandParser : ICommandParser
    {
        static readonly Regex Regex = new Regex(@"^((?<command>exit)|(?<user>.*?)(\s+(?<command>.*?)(\s+(?<arg>.*?))?)?)$", RegexOptions.IgnoreCase);

        public (string user, string command, string args)? Parse(string input)
        {
            input = input.Trim();

            if (string.IsNullOrEmpty(input))
                return null;

            var match = CommandParser.Regex.Match(input);

            if (!match.Success)
                throw new FormatException("Invalid command format");

            return (match.Groups["user"].Value, match.Groups["command"].Value, match.Groups["arg"].Value);
        }
    }
}
