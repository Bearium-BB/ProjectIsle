using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class ChunkManagerService : MonoBehaviour
{
    int mapSize = 1024;
    int chunkSize = 32;
    float tileSize = 4f;

    List<Chunk> chunks = new List<Chunk>();

    public GeneratingMeshes generatingMeshes = new GeneratingMeshes();

    public MapGeneration mapGeneration = new MapGeneration();

    public ServiceMapPopulationManager mapPopulationManager;

    public Transform player;

    public List<GameObject> meshes = new List<GameObject>();

    public Vector2Int oldChunkPosition = Vector2Int.zero;

    public int chunkRadius = 3;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        chunks = ConvertMapToChunks(mapGeneration.GenerateMap(), chunkSize);

        mapPopulationManager.CalculateSpawnOfObjects(mapGeneration);

        foreach (var item in mapPopulationManager.objectSpawnPositions)
        {
            bool isVal = TryGetChunkAndNodeIndex(item.Key, out Chunk chunk , out int index);
            Debug.Log(isVal);

            if (isVal)
            {
                chunk.objects.TryAdd(item.Key, item.Value);
            }
        }

        Vector2Int chunkPosition = WorldToChunkPosition(new Vector2(player.position.x, player.position.z));
        List<Chunk> nearbyChunks = GetChunksInRadius(chunkPosition.x, chunkPosition.y, chunkRadius);

        for (int i = 0; i < nearbyChunks.Count; i++)
        {
            GameObject gameObject = generatingMeshes.GenerateGrid(nearbyChunks[i], tileSize);

            foreach (var item in nearbyChunks[i].objects)
            {
                Instantiate(item.Value, new Vector3(item.Key.x * tileSize, 0, item.Key.y * tileSize), Quaternion.identity, gameObject.transform);
            }
            meshes.Add(gameObject);

        }

        Debug.Log("Found " + nearbyChunks.Count + " nearby chunks.");
    }

    private void Update()
    {
        Vector2Int chunkPosition = WorldToChunkPosition(new Vector2(player.position.x, player.position.z));

        if (oldChunkPosition != chunkPosition)
        {
            foreach (GameObject gameObject in meshes)
            {
                Destroy(gameObject);
            }

            meshes.Clear();

            List<Chunk> nearbyChunks = GetChunksInRadius(chunkPosition.x, chunkPosition.y, chunkRadius);

            for (int i = 0; i < nearbyChunks.Count; i++)
            {
                GameObject gameObject = generatingMeshes.GenerateGrid(nearbyChunks[i], tileSize);

                foreach (var item in nearbyChunks[i].objects)
                {
                    Instantiate(item.Value, new Vector3(item.Key.x * tileSize, 0, item.Key.y * tileSize), Quaternion.identity, gameObject.transform);
                }
                meshes.Add(gameObject);

            }

            oldChunkPosition = chunkPosition;
        }

    }

    void GenerateTestMap()
    {
        chunks.Clear();

        int chunksPerSide = mapSize / chunkSize;

        for (int chunkX = 0; chunkX < chunksPerSide; chunkX++)
        {
            for (int chunkY = 0; chunkY < chunksPerSide; chunkY++)
            {
                Chunk chunk = new Chunk(chunkX, chunkY);

                for (int x = 0; x < chunkSize; x++)
                {
                    for (int y = 0; y < chunkSize; y++)
                    {
                        int worldX = chunkX * chunkSize + x;
                        int worldY = chunkY * chunkSize + y;

                        MapNode node = new MapNode(
                            MapNodeType.water,
                            new Vector2(worldX, worldY)
                        );

                        chunk.nodes.Add(node);
                    }
                }

                chunks.Add(chunk);
            }
        }

        Debug.Log(
            "Generated " + chunks.Count +
            " chunks containing " +
            (mapSize * mapSize) + " nodes."
        );
    }

    public List<Chunk> GetChunksInRadius(int centerChunkX, int centerChunkY, int radius)
    {
        List<Chunk> nearbyChunks = new List<Chunk>();

        int chunksPerSide = mapSize / chunkSize;

        int minX = Mathf.Max(0, centerChunkX - radius);
        int maxX = Mathf.Min(chunksPerSide - 1, centerChunkX + radius);

        int minY = Mathf.Max(0, centerChunkY - radius);
        int maxY = Mathf.Min(chunksPerSide - 1, centerChunkY + radius);

        for (int x = minX; x <= maxX; x++)
        {
            for (int y = minY; y <= maxY; y++)
            {
                int dx = x - centerChunkX;
                int dy = y - centerChunkY;

                if (dx * dx + dy * dy <= radius * radius)
                {
                    Chunk chunk = GetChunk(x, y);

                    if (chunk != null)
                        nearbyChunks.Add(chunk);
                }
            }
        }

        return nearbyChunks;
    }

    Chunk GetChunk(int chunkX, int chunkY)
    {
        int chunksPerSide = mapSize / chunkSize;

        if (chunkX < 0 || chunkX >= chunksPerSide ||
            chunkY < 0 || chunkY >= chunksPerSide)
        {
            return null;
        }

        int index = chunkY * chunksPerSide + chunkX;

        return chunks[index];
    }

    public Vector2Int WorldToChunkPosition(Vector2 worldPosition)
    {
        float worldChunkSize = chunkSize * tileSize;

        int chunkX = Mathf.FloorToInt(worldPosition.x / worldChunkSize);
        int chunkY = Mathf.FloorToInt(worldPosition.y / worldChunkSize);

        return new Vector2Int(chunkX, chunkY);
    }

    public Vector2 ChunkToWorldPosition(Vector2Int chunkPosition)
    {
        float worldChunkSize = chunkSize * tileSize;

        return new Vector2(
            chunkPosition.x * worldChunkSize,
            chunkPosition.y * worldChunkSize
        );
    }

    public List<Chunk> ConvertMapToChunks(MapNode[,] mapNodes, int chunkSize)
    {
        List<Chunk> chunks = new List<Chunk>();

        int mapWidth = mapNodes.GetLength(0);
        int mapHeight = mapNodes.GetLength(1);

        int chunksX = Mathf.CeilToInt((float)mapWidth / chunkSize);
        int chunksY = Mathf.CeilToInt((float)mapHeight / chunkSize);

        for (int chunkY = 0; chunkY < chunksY; chunkY++)
        {
            for (int chunkX = 0; chunkX < chunksX; chunkX++)
            {
                Chunk chunk = new Chunk(chunkX, chunkY);

                int startX = chunkX * chunkSize;
                int startY = chunkY * chunkSize;

                int endX = Mathf.Min(startX + chunkSize, mapWidth);
                int endY = Mathf.Min(startY + chunkSize, mapHeight);

                for (int y = startY; y < endY; y++)
                {
                    for (int x = startX; x < endX; x++)
                    {
                        chunk.nodes.Add(mapNodes[x, y]);
                    }
                }

                chunks.Add(chunk);
            }
        }

        return chunks;
    }

    public bool TryGetChunkAndNodeIndex(
    Vector2 position,
    out Chunk chunk,
    out int nodeIndex)
    {
        chunk = null;
        nodeIndex = -1;

        foreach (Chunk currentChunk in chunks)
        {
            for (int i = 0; i < currentChunk.nodes.Count; i++)
            {
                if (currentChunk.nodes[i].pos == position)
                {
                    chunk = currentChunk;
                    nodeIndex = i;
                    return true;
                }
            }
        }

        return false;
    }
}

public class Chunk
{
    public int chunkX;
    public int chunkY;

    public List<MapNode> nodes = new List<MapNode>();

    public Dictionary<Vector2, GameObject> objects = new Dictionary<Vector2, GameObject>();
    public Chunk(int chunkX, int chunkY)
    {
        this.chunkX = chunkX;
        this.chunkY = chunkY;
    }
}
