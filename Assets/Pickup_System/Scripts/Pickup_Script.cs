using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using TMPro.EditorUtilities;

public class Pickup_Script : MonoBehaviour
{
    //Variables
    public bool canPickup; //Object can be picked up?
    public float pickupRange = 1.8f; //Range at which the player can pick up the pickup
    public TextMeshProUGUI inventoryText; //Reference to the inventory text

    //References
    public Transform playerToPickup, pickupToPlayer; //Used for the distanceToPlayer calculation
    public Inventory inventory;

    void Start()
    {
        canPickup = true; //Set canPickup to true at the start of the game
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(playerToPickup.position, pickupToPlayer.position);
        //If the distance is less than or equal to the pickup range, set canPickup to true; otherwise, set it to false
        if (distanceToPlayer <= pickupRange)
        {
            Debug.Log(distanceToPlayer + " is less than or equal to " + pickupRange);
            canPickup = true;
            inventoryText.text = "Press E to pick up"; //Display the pickup prompt when the player is in range
        }
        else
        {
            canPickup = false;
            inventoryText.text = ""; //Clear the inventory text when the player is out of range
        }

        if (Input.GetKeyDown(KeyCode.E) && canPickup)
        {
            canPickup = false;
            inventory.AddItem();
            gameObject.SetActive(false); //Hides the pickup; Debugging purposes, can be removed later
            //DestroyImmediate(gameObject); //Destroys the pickup
            inventoryText.text = ""; //Clear the inventory text when the player is out of range
        }
    }
}
