using UnityEngine;
using System.Collections;

public class Items
{
    //Variables for the items
    public string itemName;
    public int itemQuantity;
    public float itemRange;

    //Constructor for the items
    public Items(string name, int quantity, float range)
    {
        itemName = name;
        itemQuantity = quantity;
        itemRange = range;
    }
}
