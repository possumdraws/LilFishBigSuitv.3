using UnityEngine;

public class Player : MonoBehaviour
{
    //Variables
    [SerializeField]
    private float speed = 5f;

    private Vector3 moveDirection;

    private Animator animator;

    private bool isMoving;

    void Start()
    {
        animator = GetComponent<Animator>();
    }


    // Update is called once per frame
    void Update()
    {
        HandleMovement();

        animator.SetBool("isMoving", isMoving);
    }

    private void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        moveDirection = new Vector3(horizontal, vertical, 0f).normalized;

        isMoving = moveDirection.magnitude > 0;

        transform.Translate(moveDirection * speed * Time.deltaTime);

    }
}
