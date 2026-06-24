using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using TMPro.EditorUtilities;

public class DoorOpen_Script : MonoBehaviour
{
    //Variables
    [SerializeField]
    public bool canDoorBeOpen; 
    public bool doorUnlocked; //Used for when the player uses a key to unlock the door, the door will stay unlockeable.
    public float doorRange = 1.8f; //The range at which the player can open the door.
    //References
    public TextMeshProUGUI openDoorText;
    [Header("References for the distance between the player and the door")]
    public Transform playerToDoor;
    public Transform doorToPlayer;
    public GameObject Door;
    public GameObject Door2ndLocation; //Used to move the door that is shown in scene to another position
    public GameObject range;
    [Space]
    private float _doorMoveSpeed = 2f; //The speed at which the door will move to the new position when it is unlocked.
    public Inventory inventory;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Door2ndLocation.SetActive(false);
        canDoorBeOpen = false;
    }

    // Update is called once per frame
    void Update()
    {
        distanceToDoor();
    }

    void distanceToDoor()
    {
        float distanceToDoor = Vector3.Distance(playerToDoor.position, doorToPlayer.position);

        if (distanceToDoor <= doorRange)
        {
            //Breaks out of the loop, avoids having the text pop up when the door is already unlocked.
            if (canDoorBeOpen == true)
            {
                return;
            }
            CheckForKey();
        }
    }

    void CheckForKey()
    {
        //This function will check if the player has the key to unlock the door.
        if (inventory.numberOfKeys == 0)
        {
            openDoorText.text = "You need a key to open this door.";
            Debug.Log("Player does not have the key to unlock the door.");
        }
        else if (inventory.numberOfKeys > 0)
        {
            Debug.Log("Player has the key to unlock the door.");
            openDoorText.text = "Press E to open the door. -1 Key";
        }

        //Open door mechanic
        if (Input.GetKeyDown(KeyCode.E) && inventory.numberOfKeys > 0)
        {
            inventory.numberOfKeys -= 1;
            Door.transform.position = Vector3.MoveTowards(Door.transform.position, Door2ndLocation.transform.position, _doorMoveSpeed);
            canDoorBeOpen = true;
            doorUnlocked = true;
            range.SetActive(false);
            Debug.Log("Door is now unlocked.");
            openDoorText.text = "";
        }
    }
}
