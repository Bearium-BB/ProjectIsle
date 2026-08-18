using System.Collections.Generic;
using UnityEngine;

public class ChunkManagerService : MonoBehaviour
{
    int mapSize = 1024;
    int chunkSize = 32;

    List<Chunk> chunks = new List<Chunk>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GenerateTestMap();

        // Test: find chunks within 3 chunks of the center
        List<Chunk> nearbyChunks = GetChunksInRadius(16, 16, 3);

        Debug.Log("Found " + nearbyChunks.Count + " nearby chunks.");
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
        int chunkX = Mathf.FloorToInt(worldPosition.x / chunkSize);
        int chunkY = Mathf.FloorToInt(worldPosition.y / chunkSize);

        return new Vector2Int(chunkX, chunkY);
    }

    public Vector2 ChunkToWorldPosition(Vector2Int chunkPosition)
    {
        return new Vector2(
            chunkPosition.x * chunkSize,
            chunkPosition.y * chunkSize
        );
    }
}

public class Chunk
{
    public int chunkX;
    public int chunkY;

    public List<MapNode> nodes = new List<MapNode>();

    public Chunk(int chunkX, int chunkY)
    {
        this.chunkX = chunkX;
        this.chunkY = chunkY;
    }
}
