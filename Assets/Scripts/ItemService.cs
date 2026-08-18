using UnityEngine;

public class ItemService : MonoBehaviour
{
    public TextAsset jsonFile;
    Item[] items;

    private void Awake()
    {
        ItemList data = JsonUtility.FromJson<ItemList>(jsonFile.text);
        items = data.items;

        Debug.Log(items[0].name);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
