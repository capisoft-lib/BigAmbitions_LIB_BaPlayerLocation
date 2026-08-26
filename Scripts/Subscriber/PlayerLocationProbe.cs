using Helpers;
using UnityEngine;

namespace BaPlayerLocation.Subscriber
{
    internal static class PlayerLocationProbe
    {
        internal static bool TryProbe(out PlayerLocationSnapshot snapshot)
        {
            snapshot = default;

            try
            {
                var kind = ResolveMovementKind();
                if (kind == MovementKind.Unavailable)
                    return false;

                if (!TryGetWorldPosition(kind, out var position))
                    return false;

                if (position.sqrMagnitude <= 0.01f)
                    return false;

                var headingDeg = ResolveHeading(kind);
                var speedMps = ResolveSpeed(kind);
                var place = PlaceResolver.Resolve(position, kind);
                snapshot = new PlayerLocationSnapshot(kind, position, headingDeg, speedMps, place);
                return true;
            }
            catch
            {
                return false;
            }
        }

        internal static bool HasSignificantChange(
            PlayerLocationSnapshot current,
            PlayerLocationSnapshot last,
            bool hasSnapshot)
        {
            if (!hasSnapshot)
                return true;

            if (current.MovementKind != last.MovementKind)
                return true;

            if (!string.Equals(current.Place, last.Place, System.StringComparison.Ordinal))
                return true;

            var positionThreshold = SubscriberThresholds.PositionM;
            if ((current.Position - last.Position).sqrMagnitude >
                positionThreshold * positionThreshold)
                return true;

            if (Mathf.Abs(Mathf.DeltaAngle(last.HeadingDeg, current.HeadingDeg)) >
                SubscriberThresholds.HeadingDeg)
                return true;

            return Mathf.Abs(current.SpeedMps - last.SpeedMps) > SubscriberThresholds.SpeedMps;
        }

        internal static string FormatMovementKind(MovementKind kind)
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

        private static MovementKind ResolveMovementKind()
        {
            if (SubwaySystem.IsRiding)
                return MovementKind.Subway;

            try
            {
                if (BuildingManager.IsInitialized && BuildingManager.IsInsideBuilding)
                    return MovementKind.Indoor;
            }
            catch
            {
                // ignore
            }

            try
            {
                var selectedVehicle = GameManager.IsInitialized ? GameManager.Instance?.selectedVehicle : null;
                if (selectedVehicle != null)
                {
                    // A hand truck / flatbed also sets selectedVehicle, but those are
                    // spawnInPlayerObject cargo tools parented to the player's hands: the
                    // player is walking, not driving. Reporting Car would make consumers
                    // teleport the tool away from the player (lost cargo, IK glitches).
                    // Treat them as Walk so the player position/heading drive navigation.
                    var vehicleType = selectedVehicle.vehicleType;
                    if (vehicleType == null || !vehicleType.spawnInPlayerObject)
                        return MovementKind.Car;
                }
            }
            catch
            {
                // ignore
            }

            try
            {
                if (GameManager.IsInitialized &&
                    (GameManager.Instance?.playerController != null || PlayerHelper.PlayerController != null))
                    return MovementKind.Walk;
            }
            catch
            {
                // ignore
            }

            return MovementKind.Unavailable;
        }

        private static bool TryGetWorldPosition(MovementKind kind, out Vector3 position)
        {
            position = default;

            try
            {
                position = PlayerHelper.GetPosition();
                if (position.sqrMagnitude > 0.01f)
                    return true;
            }
            catch
            {
                // Fall through to state-specific sources.
            }

            switch (kind)
            {
                case MovementKind.Subway:
                    position = SubwaySystem.CurrentPosition;
                    return position.sqrMagnitude > 0.01f;

                case MovementKind.Car:
                    var vehicle = GameManager.Instance?.selectedVehicle;
                    if (vehicle == null)
                        return false;
                    position = vehicle.FrontPoint;
                    return position.sqrMagnitude > 0.01f;

                default:
                    var player = GameManager.Instance?.playerController ?? PlayerHelper.PlayerController;
                    if (player == null)
                        return false;
                    position = player.transform.position;
                    return position.sqrMagnitude > 0.01f;
            }
        }

        private static float ResolveHeading(MovementKind kind)
        {
            try
            {
                if (kind == MovementKind.Car)
                {
                    var vehicle = GameManager.Instance?.selectedVehicle;
                    if (vehicle == null)
                        return 0f;

                    var forward = vehicle.transform.forward;
                    forward.y = 0f;
                    if (forward.sqrMagnitude < 0.01f)
                        return 0f;

                    forward.Normalize();
                    return Mathf.Atan2(forward.x, forward.z) * 57.29578f;
                }

                var player = GameManager.Instance?.playerController ?? PlayerHelper.PlayerController;
                if (player != null)
                    return player.transform.eulerAngles.y;
            }
            catch
            {
                // ignore
            }

            return 0f;
        }

        private static float ResolveSpeed(MovementKind kind)
        {
            try
            {
                if (kind == MovementKind.Car)
                {
                    var vehicle = GameManager.Instance?.selectedVehicle;
                    if (vehicle != null)
                        return Mathf.Max(0f, vehicle.CurrentSpeed);
                }

                var player = GameManager.Instance?.playerController ?? PlayerHelper.PlayerController;
                var agent = player?.Character?.navmeshAgent;
                if (agent != null)
                {
                    var velocity = agent.velocity;
                    velocity.y = 0f;
                    return velocity.magnitude;
                }
            }
            catch
            {
                // ignore
            }

            return 0f;
        }
    }
}
