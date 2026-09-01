using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    [SerializeField] private float lifetime = 10f;
    [SerializeField] private int damage = 1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, lifetime);    
    }

    public void SetVelocity(Vector2 velocity)
    {
        GetComponent<Rigidbody2D>().linearVelocity = velocity;
    }


    //collision detection functions - one of the two colliding bodies has to be a dynamic rigidbody for these functions to be called
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Edge")
        {
            Debug.Log("Projectile hit the ground!");
        }

        if (collision.gameObject.CompareTag("Enemy") && transform.gameObject.CompareTag("PlayerProjectile"))
        {
            BaseEnemy enemy = collision.gameObject.GetComponent<BaseEnemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                Destroy(gameObject);
            }
        }

        if (collision.gameObject.CompareTag("Player") && transform.gameObject.CompareTag("EnemyProjectile"))
        {
            GameManager.Instance.Lives--;
            Destroy(gameObject);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        
    }

    //collision detection functions for trigger colliders - less restrictions on colliding bodies because these colliders do not block collisions - but they are still useful for things like pickups (hint hint)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        
    }
}
