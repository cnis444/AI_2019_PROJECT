using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public enum RARITY
    {
        JUNK, NORMAL, RARE, EXCEPTIONAL, UNIQUE, LEGENDARY
    }

    public string itemName;
    public int itemId;
    public string itemDesc;
    public Sprite itemIcon;
    public GameObject itemModel;
    public float itemCost;
    public float itemWeight;
    public int itemMaxStack;
    public RARITY itemRarity;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
