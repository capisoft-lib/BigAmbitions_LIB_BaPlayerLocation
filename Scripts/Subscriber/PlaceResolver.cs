using UnityEngine;

namespace BaPlayerLocation.Subscriber
{
    internal static class PlaceResolver
    {
        internal static string Resolve(Vector3 position, MovementKind kind)
        {
            try
            {
                if (kind == MovementKind.Indoor)
                    return ResolveBuildingName();

                if (kind is MovementKind.Walk or MovementKind.Car)
                    return ResolveNeighborhood(position);
            }
            catch
            {
                // Non-fatal — place is optional metadata.
            }

            return string.Empty;
        }

        private static string ResolveBuildingName()
        {
            var buildingVersion = BuildingManager.Instance?.currentBuildingVersion;
            if (buildingVersion != null && !string.IsNullOrWhiteSpace(buildingVersion.name))
                return buildingVersion.name;

            return "Indoor";
        }

        private static string ResolveNeighborhood(Vector3 position)
        {
            try
            {
                var zonesType = typeof(CityMapNeighborhoodZones);
                var getZoneMethod = zonesType.GetMethod(
                    "GetNeighbourhoodZoneAtPosition",
                    System.Reflection.BindingFlags.Public |
                    System.Reflection.BindingFlags.Static);

                if (getZoneMethod != null)
                {
                    var zone = getZoneMethod.Invoke(null, new object[] { position });
                    if (zone != null)
                    {
                        var neighborhoodField = zone.GetType().GetField("neighbourhood");
                        if (neighborhoodField?.GetValue(zone) != null)
                            return neighborhoodField.GetValue(zone).ToString();
                    }
                }
            }
            catch
            {
                // Reflection path may differ between game versions.
            }

            return string.Empty;
        }
    }
}
