using Claranet.SocialNetworkingKata.Providers;
using Moq;
using System;
using Xunit;

namespace Claranet.SocialNetworkingKata.Tests
{
    public class SystemTimeProviderTests
    {
        [Fact]
        public void ToSocialTime_ForZero()
        {
            // ARRANGE
            var now = new DateTime(1970, 1, 1);
            var refTime = now;
            var mock = new Mock<SystemTimeProvider>();
            mock.Setup(_ => _.Now).Returns(now);

            // ACT
            var result = mock.Object.ToSocialTime(refTime);

            // ASSERT
            var expected = $"now";
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(5)]
        [InlineData(59)]
        public void ToSocialTime_TheoryForSeconds(int seconds)
        {
            // ARRANGE
            var now = new DateTime(1970, 1, 1);
            var refTime = now.AddSeconds(-seconds);
            var mock = new Mock<SystemTimeProvider>();
            mock.Setup(_ => _.Now).Returns(now);

            // ACT
            var result = mock.Object.ToSocialTime(refTime);

            // ASSERT
            var expected = $"{seconds} seconds ago";
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(5)]
        [InlineData(59)]
        public void ToSocialTime_TheoryForMinutes(int minutes)
        {
            // ARRANGE
            var now = new DateTime(1970, 1, 1);
            var refTime = now.AddMinutes(-minutes);
            var mock = new Mock<SystemTimeProvider>();
            mock.Setup(_ => _.Now).Returns(now);

            // ACT
            var result = mock.Object.ToSocialTime(refTime);

            // ASSERT
            var expected = $"{minutes} minutes ago";
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(5)]
        [InlineData(23)]
        public void ToSocialTime_TheoryForHours(int hours)
        {
            // ARRANGE
            var now = new DateTime(1970, 1, 1);
            var refTime = now.AddHours(-hours);
            var mock = new Mock<SystemTimeProvider>();
            mock.Setup(_ => _.Now).Returns(now);

            // ACT
            var result = mock.Object.ToSocialTime(refTime);

            // ASSERT
            var expected = $"{hours} hours ago";
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("1968-10-27 10:32:01")]
        public void ToSocialTime_TheoryForEarlyDates(string date)
        {
            // ARRANGE
            var now = new DateTime(1970, 1, 1);
            var refTime = DateTime.Parse(date);
            var mock = new Mock<SystemTimeProvider>();
            mock.Setup(_ => _.Now).Returns(now);

            // ACT
            var result = mock.Object.ToSocialTime(refTime);

            // ASSERT
            var expected = $"at {refTime}";
            Assert.Equal(expected, result);
        }
    }
}
