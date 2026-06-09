namespace BaPlayerLocation.Subscriber
{
    public enum MovementKind
    {
        Unavailable,
        Indoor,
        Walk,
        Car,
        Subway
    }

    public static class MovementKindLabels
    {
        public static string ToLabel(MovementKind kind)
        {
            switch (kind)
            {
                case MovementKind.Indoor: return "Indoor";
                case MovementKind.Walk: return "Walk";
                case MovementKind.Car: return "Car";
                case MovementKind.Subway: return "Subway";
                default: return "Unavailable";
            }
        }
    }
}
