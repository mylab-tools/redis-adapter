using System;

namespace MyLab.Redis
{
    static class OptionsExpiryParser
    {
        public static TimeSpan Parse(string expiry)
        {
            if(int.TryParse(expiry, out int sec))
                return TimeSpan.FromSeconds(sec);

            return TimeSpan.Parse(expiry);
        }
    }
}