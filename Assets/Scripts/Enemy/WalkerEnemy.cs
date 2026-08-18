using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class WalkerEnemy : BaseEnemy
{
    [SerializeField] private float xVel = 2f;
    private Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb.sleepMode = RigidbodySleepMode2D.NeverSleep;
    }

    public override void TakeDamage(int damage, DamageType damageType = DamageType.Default)
    {
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);

        //early return if the enemy is already dead or squished to prevent further damage processing - the idea is in a function you don't neccesarily need to process everything that is happening, so you cann stop the function from executing further if certain conditions are met. In this case, if the enemy is already dead or squished, we don't want to process any further damage logic, so we return early from the function.
        if (stateInfo.IsName("Death") || stateInfo.IsName("Squish")) return;

        if (damageType == DamageType.JumpedOn)
        {
            anim.SetTrigger("Squish");
            Destroy(transform.parent.gameObject, 0.5f);
        }

        base.TakeDamage(damage, damageType);
    }

    // Update is called once per frame
    void Update()
    {
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsName("Walk"))
        {
            //if (sr.flipX) rb.linearVelocityX = -xVel;
            //else rb.linearVelocityX = xVel;

            //Ternary operator which is a shorthand for an if-else statement that assigned a value based on a condition. In this case, it assigns the value of rb.linearVelocityX based on the value of sr.flipX. The syntax is : variable = condition ? value_if_true : value_if_false;

            rb.linearVelocityX = sr.flipX ? -xVel : xVel;
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Barrier"))
        {
            anim.SetTrigger("Turn");
            sr.flipX = !sr.flipX;
        }
    }
}
