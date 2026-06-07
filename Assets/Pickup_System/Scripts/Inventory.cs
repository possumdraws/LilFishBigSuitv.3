using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class Inventory : MonoBehaviour
{
    public TextMeshProUGUI inventoryText; //Text component to display the inventory
    public int numberOfKeys = 0;
    
    void Start()
    {
        inventoryText.text = "Inventory\n-Keys: " + numberOfKeys;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateInventoryText();
    }

    public void UpdateInventoryText()
    {
        inventoryText.text = "Inventory\n-Keys: " + numberOfKeys;
    }

    public void AddItem()
    {
        numberOfKeys++;
        UpdateInventoryText();
    }

    public void RemoveItem()
    {
        numberOfKeys--;
        UpdateInventoryText();
    }
}
