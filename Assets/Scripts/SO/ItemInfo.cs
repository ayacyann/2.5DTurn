using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemInfo", menuName = "SOInfo/ItemInfo")]
public class ItemInfo : ScriptableObject
{
    public List<Item> items;

    public Item GetItemByType(DropItemType type)
    {
        return items.Find(x => x.dropType == type);
    }

    public float GetProbabilityByType(DropItemType type)
    {
        return items.Find(x => x.dropType == type).probability;
    }
}
