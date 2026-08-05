using UnityEngine;

public class MapNode
{
    MapNodeType type;
    Vector2 pos;

    public MapNode(MapNodeType type, Vector2 pos)
    {
        this.type = type;
        this.pos = pos;

    }
}

public enum MapNodeType
{
    border,
    water,
    land,
}
