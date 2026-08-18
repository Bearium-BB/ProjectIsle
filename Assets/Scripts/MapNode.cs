using UnityEngine;

public class MapNode
{
    public MapNodeType type;
    public Vector2 pos;
    public int regionID;

    public MapNode(MapNodeType type, Vector2 pos)
    {
        this.type = type;
        this.pos = pos;
        regionID = 0;
    }
}

public enum MapNodeType
{
    border,
    water,
    land,
}
