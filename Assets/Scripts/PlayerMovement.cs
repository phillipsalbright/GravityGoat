using UnityEngine;
using UnityEngine.InputSystem;

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
    private float fieldGravityMultiplier = .5f;
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
    private bool paused;

    /** True or false depending on whether the player is in our out of each respective field */
    private bool outField = false;
    private bool inField = false;
    private Vector3 fieldLocation;

    void Start()
    {
        player.useGravity = false;
        player.freezeRotation = true;
    }

    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
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

    void ControlDrag()
    {
        if (isGrounded)
        {
            player.drag = groundDrag;
        } else
        {
            player.drag = airDrag;
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        horizontalMovement = context.ReadValue<Vector2>().x;
        if (inField || outField)
        {
            verticalMovement = context.ReadValue<Vector2>().y;
        } else
        {
            verticalMovement = 0;
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
        if (inField)
        {
            player.AddForce((this.transform.position - fieldLocation) * Physics.gravity.magnitude * fieldGravityMultiplier, ForceMode.Acceleration);
        } else if (outField)
        {
            player.AddForce(-(this.transform.position - fieldLocation) * Physics.gravity.magnitude * fieldGravityMultiplier, ForceMode.Acceleration);
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
            player.AddForce(movementMultiplier * moveSpeed * moveDirection, ForceMode.Acceleration);
        } else if (isGrounded)
        {
            slopeMoveDirection = Vector3.ProjectOnPlane(moveDirection, slopeHit.normal);
            player.AddForce(moveSpeed * movementMultiplier * slopeMoveDirection.normalized, ForceMode.Acceleration);
        } else
        {
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
        paused = context.action.triggered;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 7)
        {
            fieldLocation = other.gameObject.transform.position;
            outField = true;
        } else if (other.gameObject.layer == 8)
        {
            fieldLocation = other.gameObject.transform.position;
            inField = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 7)
        {
            outField = false;
        }
        else if (other.gameObject.layer == 8)
        {
            inField = false;
        }
    }
}
