using Elite.SocialNetworkingKata.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Elite.SocialNetworkingKata.Tests
{
    public class CommandParserTests
    {
        [Theory]
        [InlineData("Alice -> I love the weather today", "Alice", "I love the weather today")]
        [InlineData("   Alice -> I love the weather today", "Alice", "I love the weather today")]
        [InlineData("  Alice        -> I love the weather today", "Alice", "I love the weather today")]
        [InlineData("Bob -> Damn! We lost!", "Bob", "Damn! We lost!")]
        [InlineData("Bob    ->      Damn! We lost!", "Bob", "Damn! We lost!")]
        [InlineData("Bob ->      Damn! We lost!", "Bob", "Damn! We lost!")]
        [InlineData("Bob ->   Good game though.", "Bob", "Good game though.")]
        [InlineData("Bob -> Good game though.", "Bob", "Good game though.")]
        [InlineData("Charlie -> I'm in New York today! Anyone wants to have a coffee?", "Charlie", "I'm in New York today! Anyone wants to have a coffee?")]
        [InlineData("   Charlie   ->         I'm in New York today! Anyone wants to have a coffee?", "Charlie", "I'm in New York today! Anyone wants to have a coffee?")]
        public  void Parse_Returns_Match_With_PostCommand(string input, string expecteduser, string expectedarg)
        {
            // ARRANGE
            var parser = new CommandParser();

            // ACT
            var parsed = parser.Parse(input);

            // ASSERT
            Assert.True(parsed.HasValue);
            Assert.Equal(expecteduser, parsed.Value.user);
            Assert.Equal("->", parsed.Value.command);
            Assert.Equal(expectedarg, parsed.Value.args);
        }

        [Theory]
        [InlineData("Alice", "Alice")]
        [InlineData("    Alice", "Alice")]
        [InlineData("Alice    ", "Alice")]
        [InlineData("Bob", "Bob")]
        [InlineData(" Bob", "Bob")]
        [InlineData("Bob ", "Bob")]
        [InlineData("Charlie", "Charlie")]
        [InlineData("   Charlie", "Charlie")]
        [InlineData("  Charlie  ", "Charlie")]
        public void Parse_Returns_Match_With_ReadCommand(string input, string expecteduser)
        {
            // ARRANGE
            var parser = new CommandParser();

            // ACT
            var parsed = parser.Parse(input);

            // ASSERT
            Assert.True(parsed.HasValue);
            Assert.Equal(expecteduser, parsed.Value.user);
            Assert.Equal("", parsed.Value.command);
            Assert.Equal("", parsed.Value.args);
        }

        [Theory]
        [InlineData("Charlie follows Alice", "Charlie", "Alice")]
        [InlineData("Charlie   follows Alice", "Charlie", "Alice")]
        [InlineData("Charlie   follows   Alice", "Charlie", "Alice")]
        [InlineData("   Charlie   follows   Alice", "Charlie", "Alice")]
        [InlineData("Charlie follows Bob", "Charlie", "Bob")]
        [InlineData("Charlie follows   Bob", "Charlie", "Bob")]
        [InlineData("Charlie    follows   Bob", "Charlie", "Bob")]
        [InlineData("   Charlie    follows   Bob", "Charlie", "Bob")]
        public void Parse_Returns_Match_With_FollowCommand(string input, string expecteduser, string expectedarg)
        {
            // ARRANGE
            var parser = new CommandParser();

            // ACT
            var parsed = parser.Parse(input);

            // ASSERT
            Assert.True(parsed.HasValue);
            Assert.Equal(expecteduser, parsed.Value.user);
            Assert.Equal("follows", parsed.Value.command);
            Assert.Equal(expectedarg, parsed.Value.args);
        }

        [Theory]
        [InlineData("Charlie wall", "Charlie")]
        [InlineData("Charlie    wall", "Charlie")]
        [InlineData("   Charlie    wall", "Charlie")]
        public void Parse_Returns_Match_With_WallCommand(string input, string expecteduser)
        {
            // ARRANGE
            var parser = new CommandParser();

            // ACT
            var parsed = parser.Parse(input);

            // ASSERT
            Assert.True(parsed.HasValue);
            Assert.Equal(expecteduser, parsed.Value.user);
            Assert.Equal("wall", parsed.Value.command);
            Assert.Equal("", parsed.Value.args);
        }

        [Theory]
        [InlineData("Alice XXX", "Alice", "XXX", "")]
        [InlineData("Bob    YYY ZZZ", "Bob", "YYY", "ZZZ")]
        [InlineData("   Charlie    *=> AAAAAAAAAAAaaa", "Charlie", "*=>", "AAAAAAAAAAAaaa")]
        public void Parse_Returns_Match_With_UnknownCommand(string input, string expecteduser, string expectedcommand, string expectedarg)
        {
            // ARRANGE
            var parser = new CommandParser();

            // ACT
            var parsed = parser.Parse(input);

            // ASSERT
            Assert.True(parsed.HasValue);
            Assert.Equal(expecteduser, parsed.Value.user);
            Assert.Equal(expectedcommand, parsed.Value.command);
            Assert.Equal(expectedarg, parsed.Value.args);
        }

        [Theory]
        [InlineData("exit")]
        [InlineData("   exit")]
        [InlineData("  exit ")]
        public void Parse_Returns_Match_With_ExitCommand(string input)
        {
            // ARRANGE
            var parser = new CommandParser();

            // ACT
            var parsed = parser.Parse(input);

            // ASSERT
            Assert.True(parsed.HasValue);
            Assert.Equal("", parsed.Value.user);
            Assert.Equal("exit", parsed.Value.command);
            Assert.Equal("", parsed.Value.args);
        }

    }
}
