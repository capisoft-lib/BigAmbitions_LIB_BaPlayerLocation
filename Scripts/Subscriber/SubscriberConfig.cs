using System;
using System.Globalization;
using System.IO;

namespace BaPlayerLocation.Subscriber
{
    internal sealed class SubscriberConfig
    {
        internal float PositionThresholdM { get; private set; } = SubscriberDefaults.PositionThresholdM;
        internal float HeadingThresholdDeg { get; private set; } = SubscriberDefaults.HeadingThresholdDeg;
        internal float SpeedThresholdMps { get; private set; } = SubscriberDefaults.SpeedThresholdMps;
        internal bool ConfigFileFound { get; private set; }

        internal static SubscriberConfig Load()
        {
            var config = new SubscriberConfig();
            var path = ModStoragePaths.ConfigFilePath;

            if (!File.Exists(path))
            {
                ModLog.Info("Config not found (using defaults): " + path);
                config.LogThresholds();
                return config;
            }

            config.ConfigFileFound = true;

            try
            {
                var json = File.ReadAllText(path);

                if (SimpleJsonConfig.TryGetFloat(json, "position_threshold_m", out var positionM))
                    config.PositionThresholdM = ClampPositive(positionM, SubscriberDefaults.PositionThresholdM);

                if (SimpleJsonConfig.TryGetFloat(json, "heading_threshold_deg", out var headingDeg))
                    config.HeadingThresholdDeg = ClampPositive(headingDeg, SubscriberDefaults.HeadingThresholdDeg);

                if (SimpleJsonConfig.TryGetFloat(json, "speed_threshold_mps", out var speedMps))
                    config.SpeedThresholdMps = ClampPositive(speedMps, SubscriberDefaults.SpeedThresholdMps);
            }
            catch (Exception ex)
            {
                ModLog.Error("Failed to read subscriber_config.json — using defaults", ex);
            }

            config.LogThresholds();
            return config;
        }

        private void LogThresholds()
        {
            ModLog.Info(
                "Subscriber thresholds | position=" +
                PositionThresholdM.ToString("0.###", CultureInfo.InvariantCulture) + " m" +
                " heading=" + HeadingThresholdDeg.ToString("0.###", CultureInfo.InvariantCulture) + " deg" +
                " speed=" + SpeedThresholdMps.ToString("0.###", CultureInfo.InvariantCulture) + " m/s");
        }

        internal void Apply()
        {
            SubscriberThresholds.Apply(PositionThresholdM, HeadingThresholdDeg, SpeedThresholdMps);
        }

        private static float ClampPositive(float value, float fallback)
        {
            if (value <= 0f || float.IsNaN(value) || float.IsInfinity(value))
            {
                ModLog.Info("Invalid threshold " + value.ToString(CultureInfo.InvariantCulture) + " — using default " +
                            fallback.ToString(CultureInfo.InvariantCulture));
                return fallback;
            }

            return value;
        }
    }
}
