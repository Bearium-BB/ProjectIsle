using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class MapGeneration : MonoBehaviour
{
    MapNode[,] mapNodes;
    int MapSize = 1024;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
        GenerateMap();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public MapNode[,] GenerateMap()
    {
        mapNodes = new MapNode[MapSize, MapSize];

        for (int x = 0; x < MapSize; x++)
        {
            for (int y = 0; y < MapSize; y++)
            {
                mapNodes[x, y] = new MapNode(MapNodeType.water, new Vector2(x, y));
            }
        }

        List<Vector2> blobMap = new();

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

        List<Vector2> outlinePoints = MakePositive(GetOutline(blobMap));

        for (int i = 0; i < outlinePoints.Count; i++)
        {
            mapNodes[(int)outlinePoints[i].x, (int)outlinePoints[i].y] = new MapNode(MapNodeType.border, new Vector2((int)outlinePoints[i].x, (int)outlinePoints[i].y));
        }

        List<Vector2> innerPoints = GetInsideFromOutline(outlinePoints);

        for (int i = 0; i < innerPoints.Count; i++)
        {
            mapNodes[(int)innerPoints[i].x, (int)innerPoints[i].y] = new MapNode(MapNodeType.land, new Vector2((int)innerPoints[i].x, (int)innerPoints[i].y));
        }

        Debug.Log(innerPoints.Count);

        Vector2 regionStartingPosition1 = innerPoints[Random.Range(0, innerPoints.Count)];

        Vector2 regionStartingPosition2 = innerPoints[Random.Range(0, innerPoints.Count)];

        Vector2 regionStartingPosition3 = innerPoints[Random.Range(0, innerPoints.Count)];

        Vector2 regionStartingPosition4 = innerPoints[Random.Range(0, innerPoints.Count)];

        Vector2 regionStartingPosition5 = innerPoints[Random.Range(0, innerPoints.Count)];

        Vector2 regionStartingPosition6 = innerPoints[Random.Range(0, innerPoints.Count)];

        Vector2 regionStartingPosition7 = innerPoints[Random.Range(0, innerPoints.Count)];


        mapNodes[(int)regionStartingPosition1.x, (int)regionStartingPosition1.y].regionID = 1;
        mapNodes[(int)regionStartingPosition2.x, (int)regionStartingPosition2.y].regionID = 2;
        mapNodes[(int)regionStartingPosition3.x, (int)regionStartingPosition3.y].regionID = 3;
        mapNodes[(int)regionStartingPosition4.x, (int)regionStartingPosition1.y].regionID = 1;
        mapNodes[(int)regionStartingPosition5.x, (int)regionStartingPosition2.y].regionID = 2;
        mapNodes[(int)regionStartingPosition6.x, (int)regionStartingPosition3.y].regionID = 3;
        mapNodes[(int)regionStartingPosition7.x, (int)regionStartingPosition2.y].regionID = 1;



        ExpandRegions(mapNodes);

        return mapNodes;
    }

    public List<Vector2> MakePositive(List<Vector2> points)
    {
        if (points == null || points.Count == 0)
            return new List<Vector2>();

        float minX = points[0].x;
        float minY = points[0].y;
        float maxX = points[0].x;
        float maxY = points[0].y;

        // Find the bounding box
        foreach (Vector2 point in points)
        {
            minX = Mathf.Min(minX, point.x);
            minY = Mathf.Min(minY, point.y);
            maxX = Mathf.Max(maxX, point.x);
            maxY = Mathf.Max(maxY, point.y);
        }

        Vector2 offset = new Vector2(-minX, -minY);

        List<Vector2> result = new List<Vector2>(points.Count);

        foreach (Vector2 point in points)
        {
            result.Add(point + offset);
        }

        return result;
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

    public List<Vector2> GetInsideFromOutline(List<Vector2> outline)
    {
        HashSet<Vector2Int> border = new HashSet<Vector2Int>();

        foreach (Vector2 p in outline)
        {
            border.Add(new Vector2Int(
                Mathf.RoundToInt(p.x),
                Mathf.RoundToInt(p.y)
            ));
        }


        int minX = border.Min(p => p.x);
        int maxX = border.Max(p => p.x);
        int minY = border.Min(p => p.y);
        int maxY = border.Max(p => p.y);


        // Expand bounds so outside exists
        minX--;
        minY--;
        maxX++;
        maxY++;


        HashSet<Vector2Int> outside = new HashSet<Vector2Int>();
        Queue<Vector2Int> queue = new Queue<Vector2Int>();


        Vector2Int start = new Vector2Int(minX, minY);

        outside.Add(start);
        queue.Enqueue(start);


        Vector2Int[] dirs =
        {
        new(1,0),
        new(-1,0),
        new(0,1),
        new(0,-1)
    };


        while (queue.Count > 0)
        {
            Vector2Int current = queue.Dequeue();

            foreach (var dir in dirs)
            {
                Vector2Int next = current + dir;


                if (next.x < minX || next.x > maxX ||
                   next.y < minY || next.y > maxY)
                    continue;


                if (border.Contains(next))
                    continue;


                if (outside.Contains(next))
                    continue;


                outside.Add(next);
                queue.Enqueue(next);
            }
        }


        // Everything not outside and not border is inside
        List<Vector2> inside = new();


        for (int x = minX; x <= maxX; x++)
        {
            for (int y = minY; y <= maxY; y++)
            {
                Vector2Int pos = new Vector2Int(x, y);

                if (!outside.Contains(pos) &&
                   !border.Contains(pos))
                {
                    inside.Add(pos);
                }
            }
        }


        return inside;
    }


    public void ExpandRegions(MapNode[,] mapNodes)
    {
        int width = mapNodes.GetLength(0);
        int height = mapNodes.GetLength(1);

        Queue<MapNode> queue = new Queue<MapNode>();

        // Add every existing region to the queue.
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (mapNodes[x, y].regionID > 0)
                {
                    queue.Enqueue(mapNodes[x, y]);
                }
            }
        }

        int[] dx = { 1, -1, 0, 0 };
        int[] dy = { 0, 0, 1, -1 };

        while (queue.Count > 0)
        {
            MapNode current = queue.Dequeue();

            for (int i = 0; i < 4; i++)
            {
                int nx = (int)current.pos.x + dx[i];
                int ny = (int)current.pos.y + dy[i];

                if (nx < 0 || ny < 0 || nx >= width || ny >= height)
                    continue;

                MapNode neighbor = mapNodes[nx, ny];

                // Don't spread into borders.
                if (neighbor.type == MapNodeType.border)
                    continue;

                if (neighbor.type == MapNodeType.water)
                    continue;

                // Only claim unowned cells.
                if (neighbor.regionID == 0)
                {
                    neighbor.regionID = current.regionID;
                    queue.Enqueue(neighbor);
                }
            }
        }
    }

}
