using System;
using System.Collections.Generic;
using System.Text;

namespace Claranet.SocialNetworkingKata
{
    static class Extensions
    {
        public static string ToSocialTime(this DateTime time)
        {
            TimeSpan span = DateTime.Now - time;

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
