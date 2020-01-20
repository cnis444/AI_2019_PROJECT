using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class ItemParam 
{
    public string itemName;

    public void tt()
    {

    }
}

[System.Serializable]
public class SetOfItem
{
    public string setName;
    public List<ItemParam> items;
}