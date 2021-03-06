﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Elite.SocialNetworkingKata.Providers
{
    public interface ITimeProvider
    {
        DateTime Now { get; }
        string ToSocialTime(DateTime time);
    }
}
