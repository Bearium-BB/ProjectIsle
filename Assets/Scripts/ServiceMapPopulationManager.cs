using System.Collections.Generic;
using UnityEngine;

public class ServiceMapPopulationManager : MonoBehaviour
{
    [SerializeField]
    List<RegionSpawnData> regionSpawnData = new List<RegionSpawnData>();

    public Dictionary<Vector2, int> objectSpawnPositions = new Dictionary<Vector2, int>();

    public void CalculateSpawnOfObjects(MapGeneration mapGeneration)
    {
        objectSpawnPositions.Clear();

        // Create a list for every region.
        Dictionary<int, List<Vector2>> regionPositions =
            new Dictionary<int, List<Vector2>>();

        // Automatically create the lists based on the regions we know about.
        foreach (RegionSpawnData region in regionSpawnData)
        {
            if (!regionPositions.ContainsKey(region.regionID))
            {
                regionPositions.Add(
                    region.regionID,
                    new List<Vector2>()
                );
            }
        }

        // Go through the map ONCE.
        foreach (MapNode node in mapGeneration.mapNodes)
        {
            if (regionPositions.TryGetValue(
                node.regionID,
                out List<Vector2> positions))
            {
                positions.Add(node.pos);
            }
        }

        int objectsToSpawn = 1000;

        while (objectsToSpawn > 0)
        {
            // Get all regions that still have available positions.
            List<int> availableRegions = new List<int>();

            foreach (KeyValuePair<int, List<Vector2>> region in regionPositions)
            {
                if (region.Value.Count > 0)
                {
                    availableRegions.Add(region.Key);
                }
            }

            // No more valid positions.
            if (availableRegions.Count == 0)
                break;

            // Pick a random region.
            int randomRegionIndex =
                Random.Range(0, availableRegions.Count);

            int regionID = availableRegions[randomRegionIndex];

            List<Vector2> positionsForRegion =
                regionPositions[regionID];

            // Find what object this region spawns.
            RegionSpawnData spawnData = GetRegionSpawnData(regionID);

            if (spawnData == null || spawnData.spawnObject == null)
                continue;

            // Pick a random position.
            int randomPositionIndex =
                Random.Range(0, positionsForRegion.Count);

            Vector2 position =
                positionsForRegion[randomPositionIndex];

            // Remove the position in O(1).
            positionsForRegion[randomPositionIndex] =
                positionsForRegion[positionsForRegion.Count - 1];

            positionsForRegion.RemoveAt(
                positionsForRegion.Count - 1
            );

            // Add to spawn dictionary.
            objectSpawnPositions.Add(
                position,
                spawnData.spawnObject
            );

            objectsToSpawn--;
        }

        Debug.Log(
            $"Successfully populated {objectSpawnPositions.Count} objects."
        );
    }

    private RegionSpawnData GetRegionSpawnData(int regionID)
    {
        foreach (RegionSpawnData region in regionSpawnData)
        {
            if (region.regionID == regionID)
                return region;
        }

        return null;
    }
}


[System.Serializable]
public class RegionSpawnData
{
    public int regionID;
    public int spawnObject;
}