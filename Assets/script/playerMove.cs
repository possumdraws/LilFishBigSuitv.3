using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class playerMove : MonoBehaviour
{
    public Rigidbody rb;
    public GameObject rotEmpty;
    public PlayerInput playerInput;
    Animator animator;

    [Header("breathing")]
    public float OutOfWaterTime = 15; 
    float startOOWT;
    public bool dead = false;
    
    [Header("Cam")]
    public GameObject mainCam;
    public float camTilt;
    public float camTurnSpeed = 4;
    float camCurrentTime;
    float percentageCam;
    public bool inMech = false;
    public GameObject fishFormCamLoc;
    Vector3 startFCL;
    public GameObject mechFormCamLoc;
    Vector3 startMCL;

    [Header("Physics")]
    public bool isInWater;
    public bool isGrounded;
    public float waterLinDamp = 2;
    public float AirLinDamp = .1f;
    
    [Header("Movement")]
    public Vector2 MoveDir;
    Vector2 truemove;
    public float speed;
    public float topSpeed;
    public float verticalRatio;
    public float PlayerTurnSpeed = 4;
    float PlayerCurrentTime;
    float percentagePlayer;

    [Header("Animations")]
    public Animator anim;
    public bool flop = false;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startOOWT = OutOfWaterTime;

        startFCL = fishFormCamLoc.transform.position;
        startMCL = mechFormCamLoc.transform.position;

        //anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isInWater == true)
        {
            isGrounded = false;

            rb.maxLinearVelocity = topSpeed;
            rb.AddForce(truemove * speed);

            rotatePlayerNmove();

            camRotate();

            if (OutOfWaterTime < startOOWT)
            {
                OutOfWaterTime += Time.deltaTime;
            }
            else
            {
                OutOfWaterTime = startOOWT;
            }          
        }
        else
        {
            RPMoutofWater();

            OutOfWaterTime -= Time.deltaTime;
            if (OutOfWaterTime <= 0)
            {
                dead = true;
            }                       
        }

        animate();

        
        fishFormCamLoc.transform.position = transform.position + startFCL;
        mechFormCamLoc.transform.position = transform.position + startMCL; 
        
        if (inMech == false)
        {
            mainCam.transform.position = fishFormCamLoc.transform.position;

            playerInput.actions.FindActionMap("Mech").Disable();
        }
        else
        {
            mainCam.transform.position = mechFormCamLoc.transform.position;

            playerInput.actions.FindActionMap("Mech").Enable();
        }

    }


    private void OnMove(InputValue inputValue)
    {

        //Debug.Log("ah");
        if (dead == false && inMech == false)
        {
            MoveDir = inputValue.Get<Vector2>();
            MoveDir = MoveDir.normalized;
            truemove = MoveDir;
            truemove.y *= verticalRatio;            
        }
    }

    
    private void OnTriggerEnter(Collider collider)
    {
        // if (collider.)
        // {
            
        // }
        isInWater = true;

        rb.linearDamping = waterLinDamp;        
        rb.useGravity = false;
    }

    private void OnTriggerExit(Collider collider)
    {
        isInWater = false;
        
        rb.linearDamping = AirLinDamp;
        rb.useGravity = true;

        rb.AddForce(truemove * speed, ForceMode.Impulse);
    }

    bool staycheck = false;
    Vector3 surfaceNormal;
    private void OnCollisionStay(Collision collision)
    {

        if (isInWater == false && staycheck == false)
        {
            ContactPoint contact = collision.contacts[0];
            surfaceNormal = contact.normal;

            isGrounded = true;

            if (faceRight == true)
            {
                targetRot = Quaternion.Euler(0, 0, 15.5f);
            }
            else
            {
                targetRot = Quaternion.Euler(0, 180, 15.5f);
            }

            if (surfaceNormal == new Vector3(0,1,0))
            {
                transform.position += new Vector3(0, -.17f, 0);                
            }

            
            staycheck = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        isGrounded = false;
        staycheck = false;
    }
    
    
    bool faceRight = true;
    Quaternion targetRot;
    Quaternion lastRot;
    private void rotatePlayerNmove()
    {
        if (MoveDir.x > .25f)
        {
            targetRot = Quaternion.Euler(0, 1, targetRot.z);
            faceRight = true;
        }
        else if (MoveDir.x < -.25f)
        {
            targetRot = Quaternion.Euler(0, 179, targetRot.z);
            faceRight = false;
        }

        if (MoveDir.x != 0)
        {
            if (MoveDir.y > .25f)
            {
                targetRot *= Quaternion.Euler(0, targetRot.y, 30);
            }
            else if (MoveDir.y < -.25f)
            {
                targetRot *= Quaternion.Euler(0, targetRot.y, -30);
            }
        }
        else
        {
            if (MoveDir.y > .25f)
            {
                if (faceRight == true)
                {
                    targetRot = Quaternion.Euler(0, 1, 85);
                }
                else
                {
                    targetRot = Quaternion.Euler(0, 179, 85);
                }
            }
            else if (MoveDir.y < -.25f)
            {
                if (faceRight == true)
                {
                    targetRot = Quaternion.Euler(0, 1, -85);
                }
                else
                {
                    targetRot = Quaternion.Euler(0, 179, -85);
                }
            }

        }

        if (MoveDir == Vector2.zero)
        {
            if (faceRight == true)
            {
                targetRot = Quaternion.Euler(0, 1, 0);
            }
            else
            {
                targetRot = Quaternion.Euler(0, 179, 0);
            }
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

            rotEmpty.transform.rotation = Quaternion.Lerp(playerCurRot, targetRot, percentagePlayer); //  Mathf.SmoothStep(0, 1, percentagePlayer));
        }

        


    
    }
    


    private void RPMoutofWater()
    {
        
        if (isGrounded == false)
        {
            if (faceRight == true)
            {
                targetRot = Quaternion.Euler(0, 0, -60);
            }
            else
            {
                targetRot = Quaternion.Euler(0, 180, -60);
            }   
        }

        Quaternion playerCurRot = rotEmpty.transform.rotation;

        if (MoveDir.y > .5f && isGrounded == true && dead == false && surfaceNormal == new Vector3(0,1,0)) 
        {
            if (MoveDir.x > .25f)
            {
                faceRight = true;
                rb.AddForce(new Vector2(.7f,.8f) * speed * 2, ForceMode.Impulse);

                flop = true;
            }
            else if (MoveDir.x < -.25f)
            {
                faceRight = false;
                rb.AddForce(new Vector2(-.7f,.8f) * speed * 2, ForceMode.Impulse);

                flop = true;
            }
            else
            {
                if (faceRight == true)
                {
                    rb.AddForce(new Vector2(.1f,1) * speed * 2, ForceMode.Impulse);

                    flop = true;
                }
                else
                {
                    rb.AddForce(new Vector2(-.1f,1) * speed * 2, ForceMode.Impulse);

                    flop = true;
                }
            }

            if (faceRight == true)
            {
                targetRot = Quaternion.Euler(-60, -90, 90);
            }
            else
            {
                targetRot = Quaternion.Euler(60, -90, 90);
            }

            percentagePlayer = 1;
        }

        if (playerCurRot != targetRot)
        {
            if (isGrounded == false)
            {
                percentagePlayer = PlayerCurrentTime / (PlayerTurnSpeed * 16);                    
            }
            else
            {
                percentagePlayer = (PlayerCurrentTime / (PlayerTurnSpeed * 4)) + .1f;
            }

            if (percentagePlayer < 1)
            {
                PlayerCurrentTime += Time.deltaTime;
            }

            rotEmpty.transform.rotation = Quaternion.Slerp(playerCurRot, targetRot, percentagePlayer); //  Mathf.SmoothStep(0, 1, percentagePlayer));
        }
        else
        {
            PlayerCurrentTime = 0;            
        }
    }
    
    private void OnJump()
    {
        Debug.Log("You in mech mode fool");
    }
    private void camRotate()
    {
        percentageCam = camCurrentTime / camTurnSpeed;

        if (percentageCam < 1)
        {
            camCurrentTime += Time.deltaTime;
        }

        quaternion camCurRot = mainCam.transform.rotation;

        if (MoveDir.x > .25f || MoveDir.x < -.25f)
        {
            if (faceRight)
            {
                mainCam.transform.rotation = Quaternion.Slerp(camCurRot, Quaternion.Euler(0,camTilt,0), Mathf.SmoothStep(0, 1, percentageCam));

                if (camCurRot == Quaternion.Euler(0,camTilt,0))
                {
                    camCurrentTime = 0;
                }
            }
            else
            {
                mainCam.transform.rotation = Quaternion.Slerp(camCurRot, Quaternion.Euler(0,-camTilt,0), Mathf.SmoothStep(0, 1, percentageCam));

                if (camCurRot == Quaternion.Euler(0,-camTilt,0))
                {
                    camCurrentTime = 0;
                }
            }
        }
        else
        {
            mainCam.transform.rotation = Quaternion.Slerp(camCurRot, Quaternion.Euler(0,0,0), Mathf.SmoothStep(0, 1, percentageCam));

            if (camCurRot == Quaternion.Euler(0,0,0))
            {
                camCurrentTime = 0;
            }
        }
    
    }


    float targetAniSpeed;
    float currentAniSpeed;
    float lastAniSpeed;
    float animtime;
    private void animate()
    {
        if (MoveDir != Vector2.zero && isInWater)
        {
            if (rb.linearVelocity.magnitude < topSpeed / 2)
            {
                targetAniSpeed = 2.5f;
            }
            else
            {
                targetAniSpeed = 1;
            }
        }
        else
        {
            if (isInWater == false && isGrounded == false)
            {
                targetAniSpeed = .5f;
            }
            else
            {
                targetAniSpeed = .04f;                
            }
        }

        if (lastAniSpeed != targetAniSpeed)
        {
            animtime = 0;
        }

        if (animtime < 1.5f)
        {
            animtime += Time.deltaTime;
        }

        if (flop == true)
        {
            //anim.SetBool("flop", true);

            targetAniSpeed = 3;
            currentAniSpeed = targetAniSpeed;
            lastAniSpeed = targetAniSpeed;

            flop = false;
        }
        else
        {
            //anim.SetBool("flop", false);
        }

        currentAniSpeed = Mathf.Lerp( currentAniSpeed, targetAniSpeed, animtime);

        lastAniSpeed = targetAniSpeed;

        anim.speed = currentAniSpeed;

    }

}
