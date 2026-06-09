using UnityEngine;

namespace BaPlayerLocation.Subscriber
{
    public readonly struct PlayerLocationSnapshot
    {
        public MovementKind MovementKind { get; }
        public Vector3 Position { get; }
        public float HeadingDeg { get; }
        public float SpeedMps { get; }
        public string Place { get; }

        public PlayerLocationSnapshot(
            MovementKind movementKind,
            Vector3 position,
            float headingDeg,
            float speedMps,
            string place)
        {
            MovementKind = movementKind;
            Position = position;
            HeadingDeg = headingDeg;
            SpeedMps = speedMps;
            Place = place ?? string.Empty;
        }

        public bool IsAvailable => MovementKind != MovementKind.Unavailable;
    }
}
