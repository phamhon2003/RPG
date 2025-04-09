using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName ="Scriptable object/Item")]
public class Item : ScriptableObject 
{
    public string Name;
    public Sprite image;
    public bool stackable=true;
}

[System.Serializable]
public class ItemInstance
{
    public Item item;
    public int count;

    public ItemInstance(Item item, int count = 1)
    {
        this.item = item;
        this.count = count;
    }

    public bool IsStackable()
    {
        return item.stackable;
    }

    public Sprite GetSprite()
    {
        return item.image;
    }

    public string GetName()
    {
        return item.Name;
    }
}
[System.Serializable]
public class SavedItemData
{
    public string itemName;
    public int count;

    public SavedItemData(string name, int count)
    {
        this.itemName = name;
        this.count = count;
    }
} 