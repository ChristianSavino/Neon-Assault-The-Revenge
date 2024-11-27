using System;

namespace Keru.Scripts.Helpers
{
    public static class ClockHelper
    {
        public static string GetCurrentTime()
        {
            var currentTime = DateTime.Now;
            return currentTime.ToString("HH:mm:ss");
        }
    }
}