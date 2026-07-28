using UnityEngine;

/// <summary>
/// Responsible for taking input and applying it to the rigidbody component of the player object.
/// </summary>
[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D), typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    #region Tunable Variables
    [SerializeField]
    private float speed = 5f;
    [SerializeField]
    private float jumpForce = 5f;
    [SerializeField]
    private int maxJumpCount = 2;
    [SerializeField]
    private LayerMask groundLayer;
    [SerializeField]
    private float groundCheckRadius = 0.02f;
    #endregion

    #region Component References
    // private and public - public variables can be accessed from other scripts, private variables cannot - within unity, public variables are also visible in the inspector, private variables are not - by default, variables are private unless specified otherwise
    private Rigidbody2D rb;
    private Collider2D col;
    private SpriteRenderer sr;
    private Animator anim;
    //private GroundCheck check;
    private GroundCheck1 check;
    #endregion
    
    private int jumpCount = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        //check = GetComponent<GroundCheck>();
        //check.Init(col, rb);

        check = new GroundCheck1(col, rb, groundLayer, groundCheckRadius);

        rb.linearVelocity = Vector2.zero;

        //stale code but leaving it around so that you can reference how to create a gameobject directly though code - and then how to parent it to another game object
        //if (groundCheckTransform == null)
        //{
        //    Debug.LogError("Ground check transform is not assigned in the inspector.");
        //    groundCheckTransform = new GameObject("GroundCheck").transform;
        //    groundCheckTransform.SetParent(transform);
        //    groundCheckTransform.localPosition = Vector3.zero;
        //}
    }

    // Update is called once per frame
    void Update()
    {
        bool isGroundedThisFrame = check.CheckGround();

        float horizontalInput = Input.GetAxis("Horizontal");

        float moveX = horizontalInput * speed;

        rb.linearVelocityX = moveX;

        if (Input.GetButtonDown("Jump"))
        {
            if (jumpCount < maxJumpCount)
            {
                jumpCount++;
                rb.linearVelocityY = 0f;
                rb.AddForceY(jumpForce, ForceMode2D.Impulse);
                Debug.Log("Jump Count: " + jumpCount.ToString() + " Max Jumps: " + maxJumpCount.ToString());
            }
        }

        if (isGroundedThisFrame && rb.linearVelocityY <= 0)
        {
            jumpCount = 0;
        }

        SpriteFlip(horizontalInput);

        // Update animator parameters
        anim.SetBool("isGrounded", isGroundedThisFrame);
        anim.SetFloat("horizontalInput", Mathf.Abs(horizontalInput));
    }

    private void SpriteFlip(float horizontalInput)
    {
        if (sr.flipX && horizontalInput > 0 || !sr.flipX && horizontalInput < 0)
        {
            sr.flipX = !sr.flipX;
        }
    }
}
