using System;

namespace MyLab.Redis
{
    static class OptionsExpiryParser
    {
        public static TimeSpan Parse(string expiry)
        {
            if(int.TryParse(expiry, out int hours))
                return TimeSpan.FromHours(hours);

            return TimeSpan.Parse(expiry);
        }
    }
}