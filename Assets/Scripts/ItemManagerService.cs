using System.IO;
using System.Linq;
using UnityEngine;

public class ItemManagerService
{
    Item[] items;

    public ItemManagerService()
    {
        string pathTextureData = Path.Combine(
            Application.streamingAssetsPath,
            "items.json"
        );
        string jsonData = File.ReadAllText(pathTextureData);

        ItemList data = JsonUtility.FromJson<ItemList>(jsonData);

        items = data.items;
    }

    public Item GetRandomItem()
    {
        return items[Random.Range(0, items.Length)];
    }
    public Item GetItemById(int id)
    {
        
        return items.Where(x => x.id == id).FirstOrDefault();
    }
}
