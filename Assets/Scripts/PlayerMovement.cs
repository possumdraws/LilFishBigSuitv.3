using UnityEngine;

public class Player : MonoBehaviour
{
    public Rigidbody rb;

    //Variables
    [SerializeField]
    private float speed = 5f;

    [SerializeField]
    private float topSpeed = 6f;

    private Vector3 moveDirection;

    private Animator animator;

    private bool isMoving;


    [Header("rotation")]
    public GameObject rotEmpty;
    public float PlayerTurnSpeed = 4;
    float PlayerCurrentTime;
    float percentagePlayer;

    void Start()
    {
        animator = GetComponent<Animator>();
    }


    // Update is called once per frame
    void Update()
    {

        HandleMovement();
        rotatePlayerNmove();

        rb.maxLinearVelocity = topSpeed;
        rb.AddForce(moveDirection * speed);

        animator.SetBool("isMoving", isMoving);
    }

    float horizontal;
    float vertical;
    private void HandleMovement()
    {
        horizontal = Input.GetAxis("Horizontal");
        //vertical = Input.GetAxis("Vertical");

        moveDirection = new Vector3(horizontal, vertical, 0f).normalized;

        isMoving = moveDirection.magnitude > 0;




        //transform.Translate(moveDirection * speed * Time.deltaTime);

    }

    Quaternion targetRot;
    Quaternion lastRot;

    private void rotatePlayerNmove()
    {
        if (horizontal > .25f)
        {
            targetRot = Quaternion.Euler(0, -179, 0);
        }
        else if (horizontal < -.25f)
        {
            targetRot = Quaternion.Euler(0, -7, 0);
        }

        if (targetRot != lastRot)
        {
            PlayerCurrentTime = 0;   
        }
        lastRot = targetRot;

        Quaternion playerCurRot = rotEmpty.transform.rotation;

        if (playerCurRot != targetRot)
        {
            percentagePlayer = PlayerCurrentTime / PlayerTurnSpeed;

            if (percentagePlayer < 1)
            {
                PlayerCurrentTime += Time.deltaTime;
            }

            rotEmpty.transform.rotation = Quaternion.Lerp(playerCurRot, targetRot, percentagePlayer);
        }
    }
}