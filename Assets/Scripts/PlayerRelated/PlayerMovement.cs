using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

/**
 * This class handles the basic, phsyics based, player movement. Many of the variables should be
 * specified in the editor.
 */
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    /** Value for speed to apply to the player */
    private float moveSpeed = 4.5f;
    /** Multiplier to overcome drag for player speed */
    private float movementMultiplier = 10f;
    /** Multiplier to reduce movement speed in air */
    private float airMultiplier = .2f;
    /** Value obtained in GetInput() for the horizontal player movement */
    private float horizontalMovement;
    /** Value obtained in GetInput() for the Vertical player movement */
    private float verticalMovement;
    /** Direction the player is facing, calculated in GetInput() */
    private Vector3 moveDirection;
    private Vector3 slopeMoveDirection;

    [Header("Jumping")]
    /** Force for jumping, set in editor */
    private float jumpForce = 14;
    /** Multiplier for gravity. Drag is used to limit X and Y movement in air, may need extra gravity because of this. */
    private float gravityMultiplier = 2.6f;
    /** Multiplier for gravity within a gravitational field */
    private float fieldGravityMultiplier = .6f;
    private bool jumped;

    [Header("Drag")]
    /** Ammount of drag to be applied to player movement (ground higher than air) */
    private float groundDrag = 6;
    private float airDrag = 1.5f;

    [Header("Ground Detection")]
    [SerializeField] private Transform groundCheck;
    private bool isGrounded;
    /** Distance from ground that counts as touching it */
    private float groundDistance = .4f;
    /** Mask containing only the ground */
    [SerializeField] LayerMask groundMask;
    RaycastHit slopeHit;

    [Header("Player info")]
    /** Set to player's height */
    private float playerHeight = 2;
    /** Rigidbody of the player */
    public Rigidbody player;
    private bool paused = false;
    private bool walking;
    [SerializeField] Animator bodyAnimator;

    public List<GravityField> currentFieldCollisions = new List<GravityField>();
    [SerializeField] Canvas pauseMenu;
    [SerializeField] PlayerInput input;

    private bool isOnMovingPlatform;
    [SerializeField] private LayerMask movingPlatformMask;
    private Collider playerCollider;
    private bool inWater;
    
    void Start()
    {
        player.useGravity = false;
        player.freezeRotation = true;
        moveSpeed = 4.5f;
        movementMultiplier = 10f;
        airMultiplier = .2f;
        jumpForce = 14;
        gravityMultiplier = 2.6f;
        fieldGravityMultiplier = .6f;
        groundDrag = 6;
        airDrag = 1.5f;
        groundDistance = .4f;
        playerCollider = this.GetComponentInChildren<Collider>();
    }

    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        isOnMovingPlatform = Physics.CheckSphere(groundCheck.position, groundDistance, movingPlatformMask);
        //GetInput();
        ControlDrag();
        if (jumped && isGrounded)
        {
            Jump();
        }
        if (paused)
        {
            //GameManager.instance.PauseGame();
            paused = false;
        }
        if (isOnMovingPlatform) {
            playerCollider.material.dynamicFriction = 1;
            playerCollider.material.staticFriction = 1;
        } else
        {
            playerCollider.material.dynamicFriction = 0;
            playerCollider.material.staticFriction = 0;
        }

    }

    public void OnJump(InputAction.CallbackContext context)
    {
        jumped = context.action.triggered;
    }

    private void Jump()
    {
        jumped = false;
        player.velocity = new Vector3(player.velocity.x, 0, player.velocity.z);
        player.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    /** Also controls jumping for animator */
    void ControlDrag()
    {
        if (isGrounded)
        {
            bodyAnimator.SetBool("InAir", false);
            player.drag = groundDrag;
        } else
        {
            bodyAnimator.SetBool("InAir", true);
            player.drag = airDrag;
        }
        if (inWater)
        {
            player.drag = player.drag * 1.5f;
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        horizontalMovement = context.ReadValue<Vector2>().x;
        if (Mathf.Abs(horizontalMovement) > 0)
        {
            walking = true;
        } else
        {
            walking = false;
        }
        bodyAnimator.SetBool("Walking", walking);
        if (currentFieldCollisions.Count > 0)
        {
            verticalMovement = context.ReadValue<Vector2>().y;
        } else
        {
            verticalMovement = 0;
        }
    }

    public void OnRestart(InputAction.CallbackContext context)
    {
        if (context.action.triggered)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    void FixedUpdate()
    {
        CalculateGravity();
        MovePlayer();
    }

    /**
     * Calculates which type of gravity to use.
     */
    void CalculateGravity()
    {
        if (currentFieldCollisions.Count > 0)
        {
            Vector3 forceSum = new Vector3(0, 0, 0);
            foreach(GravityField f in currentFieldCollisions)
            {
                if (f != null && f.GetActive())
                {
                    /** accurate */
                    Vector3 direction = ((this.transform.position - f.GetPosition()) * f.GetOutwardForce()).normalized;
                    float magnitude = ((this.transform.position - f.GetPosition()) * f.GetOutwardForce()).magnitude;
                    magnitude = Mathf.Clamp(magnitude, 0, 3.5f);
                    magnitude = 3.5f - magnitude;
                    magnitude = magnitude / 10.5f;
                    magnitude += 2.1f;
                    forceSum += direction * magnitude;
                    Debug.Log(magnitude + " " + ((this.transform.position - f.GetPosition()) * f.GetOutwardForce()).magnitude);
                    

                    /** how its been */
                    //forceSum += ((this.transform.position - f.GetPosition()) * f.GetOutwardForce());
                } else
                {
                    currentFieldCollisions.Remove(f);
                    break;
                }
            }
            player.AddForce(forceSum * Physics.gravity.magnitude * fieldGravityMultiplier, ForceMode.Acceleration);
        } else if (!OnSlope())
        {
            player.AddForce(Physics.gravity * gravityMultiplier, ForceMode.Acceleration);
        } else
        {
            player.AddForce(-slopeHit.normal * Physics.gravity.magnitude, ForceMode.Acceleration);
        }
    }

    void MovePlayer()
    {
        moveDirection = new Vector3(1, 0, 0) * horizontalMovement + new Vector3(0, 1, 0) * verticalMovement;

        if (isGrounded && !OnSlope())
        {
            //Debug.Log("1 " + moveDirection + " " + moveSpeed + " " + movementMultiplier + " " + airMultiplier + " " + this.GetComponent<Rigidbody>().velocity.magnitude);
            player.AddForce(movementMultiplier * moveSpeed * moveDirection, ForceMode.Acceleration);
        } else if (isGrounded)
        {
            //Debug.Log("2 " + moveDirection + " " + moveSpeed + " " + movementMultiplier + " " + airMultiplier + " " + this.GetComponent<Rigidbody>().velocity.magnitude);
            slopeMoveDirection = Vector3.ProjectOnPlane(moveDirection, slopeHit.normal);
            player.AddForce(moveSpeed * movementMultiplier * slopeMoveDirection.normalized, ForceMode.Acceleration);
        } else
        {
            //Debug.Log("3 " + moveDirection + " " + moveSpeed + " " + movementMultiplier + " " + airMultiplier + " " + this.GetComponent<Rigidbody>().velocity.magnitude);
            player.AddForce(airMultiplier * movementMultiplier * moveSpeed * moveDirection.normalized, ForceMode.Acceleration);
        }
        
    }

    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * .5f + .5f)) {
            if (slopeHit.normal != Vector3.up)
            {
                return true;
            }
        }
        return false;
    }

    public void Pause(InputAction.CallbackContext context)
    {
        if (context.action.triggered)
        {
            if (!paused)
            {
                paused = true;
                input.DeactivateInput();
                GetComponentInChildren<ArmScript>().enabled = false;
                Time.timeScale = 0;
                pauseMenu.gameObject.SetActive(true);
            }
            else
            {
                ResumeGame();
            }
        }
        
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        pauseMenu.gameObject.SetActive(false);
        input.ActivateInput();
        GetComponentInChildren<ArmScript>().enabled = true;

        paused = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 7 || other.gameObject.layer == 8)
        {
            currentFieldCollisions.Add(other.gameObject.GetComponentInParent<GravityField>());
        }
        if (other.gameObject.layer == 4)
        {
            inWater = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 7 || other.gameObject.layer == 8)
        {
            currentFieldCollisions.Remove(other.gameObject.GetComponentInParent<GravityField>());
        }
        if (other.gameObject.layer == 4)
        {
            inWater = false;
        }
    }

    public void LoadMainMenu()
    {
        ResumeGame();
        if (MusicManager.musicManager != null)
        {
            MusicManager.musicManager.PlayMusic(-1);
        }
        SceneManager.LoadScene(0);
    }

    public void HitByArrow()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
