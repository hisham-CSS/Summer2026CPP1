using NUnit.Framework.Internal;
using System.Collections;
using UnityEngine;

/// <summary>
/// Responsible for taking input and applying it to the rigidbody component of the player object.
/// </summary>
[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D), typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    #region Tunable Variables
    [SerializeField] private float speed = 5f;
    [SerializeField] private int maxJumpCount = 2;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCheckRadius = 0.02f;
    
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float jumpForcePowerup = 15f;
    [SerializeField] private float initalPowerupDuration = 5f;
    #endregion


    #region Component References
    // private and public - public variables can be accessed from other scripts, private variables cannot - within unity, public variables are also visible in the inspector, private variables are not - by default, variables are private unless specified otherwise
    private Rigidbody2D rb;
    private Collider2D col;
    private SpriteRenderer sr;
    private Animator anim;
    //private GroundCheck check;
    private GroundCheck1 check;
    private Shoot shoot;
    #endregion
    
    #region Lives
    public int maxLives = 9;
    private int _lives = 3;
    //C# style getters and setters - properties - they do the same thing as the above C++ style getters and setters, but they are more concise and easier to read - they are also more flexible, as they can have logic in them, and can be read-only or write-only
    public int Lives
    {
        get => _lives;
        set
        {
            if (value > maxLives)
            {
                maxLives = value;
            }
            else if (value < 0)
            {
                _lives = 0;
                //game over logic happens here
            }
            else if (value < _lives)
            {
                _lives = value;
                //respawn logic happens here
                Debug.Log("Respawn logic happens here");
            }
            else
            {
                _lives = value;
            }

            Debug.Log("Lives: " + _lives.ToString() + " Max Lives: " + maxLives.ToString());

        }
    }

    //C++ style getters and setters - these are properties in C# - they are a way to encapsulate the access to a variable - they can have logic in them, and can be read-only or write-only
    //public void SetLives(int value)
    //{
    //    if (value > maxLives)
    //    {
    //        maxLives = value;
    //    }
    //    else if (value < 0)
    //    {
    //        lives = 0;
    //        //game over logic happens here
    //    }
    //    else if (value < lives)
    //    {
    //        lives = value;
    //        //respawn logic happens here
    //    }
    //    else
    //    {
    //        lives = value;
    //    }
    //}

    //public int GetLives()
    //{
    //    return lives;
    //}
    #endregion


    private int jumpCount = 0;
    private float currentPowerupDuration = 0f;
    private float initalJumpForce = 5f;

    //In programming the term routine is a synonym for function or method. A coroutine is a special type of routine that can be paused and resumed, allowing for concurrent behavior in Unity. Coroutines are used to perform actions over time, such as waiting for a certain duration or waiting for a condition to be met before continuing execution. This is not asynchronous as multithreading, but it allows for more complex behaviors in a single thread without blocking the main game loop.
    private Coroutine jumpForceCoroutine = null;

    

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        shoot = GetComponent<Shoot>();

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

        initalJumpForce = jumpForce;


        //recoil feature that needs some work
        //shoot.OnShotFired += (velocity) => rb.AddForce(-velocity, ForceMode2D.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        AnimatorClipInfo[] clipInfo = anim.GetCurrentAnimatorClipInfo(0);
        bool isGroundedThisFrame = check.CheckGround();

        //input checks
        float horizontalInput = Input.GetAxis("Horizontal");
        bool jumpInput = Input.GetButtonDown("Jump");
        bool fireInput = Input.GetButtonDown("Fire1");


        //movement along x axis
        float moveX = horizontalInput * speed;
        rb.linearVelocityX = moveX;

        //jump along y axis
        if (jumpInput)
        {
            if (jumpCount < maxJumpCount)
            {
                jumpCount++;
                rb.linearVelocityY = 0f;
                rb.AddForceY(jumpForce, ForceMode2D.Impulse);
                Debug.Log("Jump Count: " + jumpCount.ToString() + " Max Jumps: " + maxJumpCount.ToString());
            }
        }

        //if we are grounded while we are not falling
        if (isGroundedThisFrame && rb.linearVelocityY <= 0)
        {
            jumpCount = 0;
        }

        if (clipInfo[0].clip.name == "Fire" && isGroundedThisFrame)
        {
            rb.linearVelocityX = 0;
        }

        SpriteFlip(horizontalInput);

        // Update animator parameters
        anim.SetBool("isGrounded", isGroundedThisFrame);
        anim.SetFloat("horizontalInput", Mathf.Abs(horizontalInput));
        if (fireInput) anim.SetTrigger("Fire");
    }

    private void SpriteFlip(float horizontalInput)
    {
        if (sr.flipX && horizontalInput > 0 || !sr.flipX && horizontalInput < 0)
        {
            sr.flipX = !sr.flipX;
        }
    }

    public void StartJumpForceChange()
    {
        if (jumpForceCoroutine != null)
        {
            StopCoroutine(jumpForceCoroutine);
            jumpForceCoroutine = null;
            jumpForce = initalJumpForce;
        }

        jumpForceCoroutine = StartCoroutine(JumpForceChangeCoroutine());
    }

    IEnumerator JumpForceChangeCoroutine()
    {
        //this code will run immediately when the coroutine is started
        currentPowerupDuration = initalPowerupDuration + currentPowerupDuration;
        jumpForce = jumpForcePowerup;

        while (currentPowerupDuration > 0)
        {
            currentPowerupDuration -= Time.deltaTime;
            Debug.Log("Current Powerup Duration: " + currentPowerupDuration.ToString());
            yield return null; // Wait for the next frame
        }

        jumpForce = initalJumpForce;
        jumpForceCoroutine = null;
        currentPowerupDuration = 0f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Squish") && rb.linearVelocityY <= 0)
        {
            BaseEnemy enemy = collision.GetComponentInParent<BaseEnemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(0, DamageType.JumpedOn);
                rb.linearVelocityY = 0;
                rb.AddForceY(jumpForce, ForceMode2D.Impulse);
            }
        }
    }
}
