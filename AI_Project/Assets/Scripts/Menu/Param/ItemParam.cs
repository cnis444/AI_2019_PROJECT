using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class ItemParam 
{
    public string itemName;
    public int itemId;
    public string itemDesc;
    public float itemCost;
    public float itemWeight;
    public int itemMaxStack;
    public Item.RARITY itemRarity;
}

[System.Serializable]
public class SetOfItem
{
    public string setName;
    public List<ItemParam> items;
}