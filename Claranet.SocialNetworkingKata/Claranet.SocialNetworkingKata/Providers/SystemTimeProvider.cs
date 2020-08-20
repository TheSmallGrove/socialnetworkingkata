using System;
using System.Collections.Generic;
using System.Text;

namespace Claranet.SocialNetworkingKata.Providers
{
    class SystemTimeProvider : ITimeProvider
    {
        public DateTime Now => DateTime.Now;

        public string ToSocialTime(DateTime time)
        {
            TimeSpan span = this.Now - time;

            if (Math.Round(span.TotalSeconds, 0) < 60)
                return $"{Math.Round(span.TotalSeconds, 0)} seconds ago";
            else if (Math.Round(span.TotalMinutes, 0) < 60)
                return $"{Math.Round(span.TotalMinutes, 0)} minutes ago";
            else if (Math.Round(span.TotalHours, 0) < 24)
                return $"{Math.Round(span.TotalMinutes, 0)} hours ago";

            return $"at {time}";
        }

    }
}