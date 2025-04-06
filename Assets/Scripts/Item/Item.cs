
using System;
using UnityEngine;

[Serializable]
public class Item
{
    public DropItemType dropType;
    public string name;
    public Sprite sprite;
    public string description;
    public int health;
    public int attack;
    public int speed;
    public int defence;
    [Range(0,1)]public float probability;

    public Item()
    {
        dropType = DropItemType.None;
    }
    public Item(DropItemType dropType)
    {
        Item item = BackpackManager.Instance.itemInfo.GetItemByType(dropType);
        this.dropType = item.dropType;
        this.name = item.name;
        this.sprite = item.sprite;
        this.description = item.description;
        this.health = item.health;
        this.attack = item.attack;
        this.speed = item.speed;
        this.defence = item.defence;
        this.probability = item.probability;
    }
    public Item(Item item)
    {
        this.dropType = item.dropType;
        this.name = item.name;
        this.sprite = item.sprite;
        this.description = item.description;
        this.health = item.health;
        this.attack = item.attack;
        this.speed = item.speed;
        this.defence = item.defence;
        this.probability = item.probability;
    }
}
