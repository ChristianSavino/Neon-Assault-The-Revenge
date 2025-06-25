using UnityEngine;

namespace Keru.Scripts.Engine.Helper
{
    public static class StringFormatterHelper
    {
        public static string GetFormattedKeyCode(KeyCode keyCode)
        {
            var keyCodeFormatted = keyCode.ToString();

            if (keyCodeFormatted.Contains("Mouse"))
            {
                var split = keyCodeFormatted.Split("Mouse");
                keyCodeFormatted = $"M{split[1]}";
            }
            return keyCodeFormatted.ToUpper();
        }
    }
}

