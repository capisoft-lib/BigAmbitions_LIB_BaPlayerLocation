namespace BaPlayerLocation.Subscriber
{
    internal static class SubscriberThresholds
    {
        internal static float PositionM { get; private set; } = SubscriberDefaults.PositionThresholdM;
        internal static float HeadingDeg { get; private set; } = SubscriberDefaults.HeadingThresholdDeg;
        internal static float SpeedMps { get; private set; } = SubscriberDefaults.SpeedThresholdMps;

        internal static void Apply(float positionM, float headingDeg, float speedMps)
        {
            PositionM = positionM;
            HeadingDeg = headingDeg;
            SpeedMps = speedMps;
        }

        internal static void ResetToDefaults()
        {
            PositionM = SubscriberDefaults.PositionThresholdM;
            HeadingDeg = SubscriberDefaults.HeadingThresholdDeg;
            SpeedMps = SubscriberDefaults.SpeedThresholdMps;
        }
    }
}
