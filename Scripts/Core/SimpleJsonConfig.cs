using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace BaPlayerLocation.Subscriber
{
    internal static class SimpleJsonConfig
    {
        internal static bool TryGetFloat(string json, string key, out float value)
        {
            value = 0f;
            var match = Regex.Match(
                json ?? string.Empty,
                "\"" + Regex.Escape(key) + "\"\\s*:\\s*(-?[0-9]+(?:\\.[0-9]+)?)",
                RegexOptions.IgnoreCase,
                TimeSpan.FromMilliseconds(250));
            if (!match.Success)
                return false;

            return float.TryParse(match.Groups[1].Value, NumberStyles.Float, CultureInfo.InvariantCulture, out value);
        }
    }
}
