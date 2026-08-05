using System;
using System.Collections.Generic;
using System.Drawing;
using Unity.Mathematics;
using UnityEngine;
using static UnityEditor.Progress;
using Random = UnityEngine.Random;

public class MapGeneration : MonoBehaviour
{
    MapNode[,] mapNodes;
    int MapSize = 1000;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mapNodes = new MapNode[MapSize, MapSize];

        for (int x = 0; x < MapSize; x++)
        {
            for (int y = 0; y < MapSize; y++)
            {
                mapNodes[x, y] = new MapNode(MapNodeType.water, new Vector2(x,y));
            }
        }

        List<Vector2>  blobMap = new();

        Vector2 newStartingPoint = Vector2.zero;

        for (int i = 0; i < 50; i++)
        {
            List<Vector2> blog = GenerateNoiseBlog(25, 10, 1, 20);
            if (i != 0)
            {
                for (int y = 0; y < blog.Count; y++)
                {
                    blog[y] += newStartingPoint;
                }

                newStartingPoint = blog[Random.Range(0, blog.Count)];
            }


            blobMap.AddRange(blog);

        }

        List<Vector2> outlinePoints = GetOutline(blobMap);

        foreach (var item in outlinePoints)
        {
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.position = new Vector3(item.x, 0, item.y);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    List<Vector2> GenerateNoiseBlog(float baseRadius = 25, int pointCount = 64, float noiseScale = 2f, float variation = 40f)
    {
        float seedX = Random.Range(0f, 10000f);
        float seedY = Random.Range(0f, 10000f);

        List<Vector2> points = new();

        for (int i = 0; i < pointCount; i++)
        {
            float angle = i * Mathf.PI * 2 / pointCount;

            float nx = Mathf.Cos(angle) * noiseScale + seedX;
            float ny = Mathf.Sin(angle) * noiseScale + seedY;

            float noise = Mathf.PerlinNoise(nx, ny);

            float radius = baseRadius + (noise - 0.5f) * variation;

            points.Add(new Vector2(
                Mathf.RoundToInt(Mathf.Cos(angle) * radius),
                Mathf.RoundToInt(Mathf.Sin(angle) * radius)
            ));
        }

        List<Vector2> pointLine = new();

        for (int i = 0; i < points.Count; i++)
        {
            Vector2 start = points[i];
            Vector2 end = points[(i + 1) % points.Count];

            List<Vector2Int> line = GetLine(
                new Vector2Int(Mathf.RoundToInt(start.x), Mathf.RoundToInt(start.y)),
                new Vector2Int(Mathf.RoundToInt(end.x), Mathf.RoundToInt(end.y))
            );

            foreach (var linePoint in line)
            {
                pointLine.Add(linePoint);
            }
        }

        points.AddRange(pointLine);

        return points;
    }

    public List<Vector2Int> GetLine(Vector2Int start, Vector2Int end)
    {
        List<Vector2Int> points = new List<Vector2Int>();

        int x0 = start.x;
        int y0 = start.y;
        int x1 = end.x;
        int y1 = end.y;

        int dx = Mathf.Abs(x1 - x0);
        int dy = Mathf.Abs(y1 - y0);

        int sx = x0 < x1 ? 1 : -1;
        int sy = y0 < y1 ? 1 : -1;

        int err = dx - dy;

        while (true)
        {
            points.Add(new Vector2Int(x0, y0));

            if (x0 == x1 && y0 == y1)
                break;

            int e2 = 2 * err;

            if (e2 > -dy)
            {
                err -= dy;
                x0 += sx;
            }

            if (e2 < dx)
            {
                err += dx;
                y0 += sy;
            }
        }

        return points;
    }


    List<Vector2> GetOutline(List<Vector2> blobMap)
    {
        // 1. Find bounds of the actual data, with padding so flood-fill can wrap around it
        int padding = 2;
        float minX = float.MaxValue, maxX = float.MinValue;
        float minY = float.MaxValue, maxY = float.MinValue;
        foreach (var p in blobMap)
        {
            minX = Mathf.Min(minX, p.x);
            maxX = Mathf.Max(maxX, p.x);
            minY = Mathf.Min(minY, p.y);
            maxY = Mathf.Max(maxY, p.y);
        }

        int originX = Mathf.FloorToInt(minX) - padding;
        int originY = Mathf.FloorToInt(minY) - padding;
        int width = Mathf.CeilToInt(maxX) - originX + padding + 1;
        int height = Mathf.CeilToInt(maxY) - originY + padding + 1;

        bool[,] occupied = new bool[width, height];

        // 2. Mark boundary points on the grid (shifted into local grid space)
        foreach (var p in blobMap)
        {
            int x = Mathf.RoundToInt(p.x) - originX;
            int y = Mathf.RoundToInt(p.y) - originY;
            occupied[x, y] = true;
        }

        // 3. Flood-fill from the grid border inward through unoccupied cells -> "outside"
        bool[,] isOutside = new bool[width, height];
        Queue<Vector2Int> queue = new();

        void TrySeed(int x, int y)
        {
            if (!occupied[x, y] && !isOutside[x, y])
            {
                isOutside[x, y] = true;
                queue.Enqueue(new Vector2Int(x, y));
            }
        }

        for (int x = 0; x < width; x++)
        {
            TrySeed(x, 0);
            TrySeed(x, height - 1);
        }
        for (int y = 0; y < height; y++)
        {
            TrySeed(0, y);
            TrySeed(width - 1, y);
        }

        Vector2Int[] dirs = { new(1, 0), new(-1, 0), new(0, 1), new(0, -1) };
        while (queue.Count > 0)
        {
            var c = queue.Dequeue();
            foreach (var d in dirs)
            {
                int nx = c.x + d.x, ny = c.y + d.y;
                if (nx < 0 || nx >= width || ny < 0 || ny >= height) continue;
                if (occupied[nx, ny] || isOutside[nx, ny]) continue;
                isOutside[nx, ny] = true;
                queue.Enqueue(new Vector2Int(nx, ny));
            }
        }

        // 4. Land = anything not reached by the outside flood-fill
        // 5. Outline = land cells that touch an outside (water) cell
        List<Vector2> outline = new();
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (isOutside[x, y]) continue; // this cell is water, skip

                bool touchesWater = false;
                for (int dx = -1; dx <= 1 && !touchesWater; dx++)
                {
                    for (int dy = -1; dy <= 1 && !touchesWater; dy++)
                    {
                        if (dx == 0 && dy == 0) continue;
                        int nx = x + dx, ny = y + dy;
                        if (nx < 0 || nx >= width || ny < 0 || ny >= height || isOutside[nx, ny])
                            touchesWater = true;
                    }
                }
                if (touchesWater)
                    outline.Add(new Vector2(x + originX, y + originY)); // shift back to world space
            }
        }

        return outline;
    }
}
