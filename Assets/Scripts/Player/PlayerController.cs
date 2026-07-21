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
    #endregion

    #region Component References
    // private and public - public variables can be accessed from other scripts, private variables cannot - within unity, public variables are also visible in the inspector, private variables are not - by default, variables are private unless specified otherwise
    private Rigidbody2D rb;
    private Collider2D col;
    private SpriteRenderer sr;
    private Animator anim;
    #endregion

    #region Ground Check Stuff
    // Ground check variables that are set in the inspector
    [SerializeField]
    private LayerMask groundLayer;
    [SerializeField]
    private float groundCheckRadius = 0.2f;

    // Ground check position is calculated based on the collider's bounds
    private Vector2 groundCheckPos => CalculateGroundCheckPos();
    private bool isGrounded;
    private int jumpCount = 0;

    // Foot position helper function to calculate the ground check position based on the collider's bounds
    private Vector2 CalculateGroundCheckPos()
    {
        Bounds bounds = col.bounds;
        return new Vector2(bounds.center.x, bounds.min.y);
    }
    #endregion

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

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
        if (rb.linearVelocityY <= 0)
        {
            isGrounded = Physics2D.OverlapCircle(groundCheckPos, groundCheckRadius, groundLayer);
        }

        float horizontalInput = Input.GetAxis("Horizontal");

        float moveX = horizontalInput * speed;

        rb.linearVelocityX = moveX;

        if (Input.GetButtonDown("Jump"))
        {
            if (jumpCount < maxJumpCount)
            {
                jumpCount++;
                isGrounded = false;
                rb.linearVelocityY = 0f;
                rb.AddForceY(jumpForce, ForceMode2D.Impulse);
                Debug.Log("Jump Count: " + jumpCount.ToString() + " Max Jumps: " + maxJumpCount.ToString());
            }
        }

        if (isGrounded)
        {
            jumpCount = 0;
        }

        SpriteFlip(horizontalInput);

        // Update animator parameters
        anim.SetBool("isGrounded", isGrounded);
        anim.SetFloat("horizontalInput", Mathf.Abs(horizontalInput));
    }

    private void SpriteFlip(float horizontalInput) => sr.flipX = (horizontalInput < 0);
    //{
    //    if (sr.flipX && horizontalInput > 0 || !sr.flipX && horizontalInput < 0)
    //    {
    //        sr.flipX = !sr.flipX;
    //    }
    //}
}
