using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float speed = 5f;
    private Vector3 moveDirection;

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        moveDirection = new Vector3(horizontal, vertical, 0f).normalized;

        transform.Translate(moveDirection * speed * Time.deltaTime);
        
        /*
        //Debugging
        switch (horizontal)
        {
            case <= 0f:
                Debug.Log("Moving Left");
                break;
            case >= 0f:
                Debug.Log("Moving Right");
                break;
        }
        switch (vertical)
        {
            case >= 0f:
                Debug.Log("Moving Up");
                break;
            case <= 0f:
                Debug.Log("Moving Down");
                break;
        }
        */
    }
}
