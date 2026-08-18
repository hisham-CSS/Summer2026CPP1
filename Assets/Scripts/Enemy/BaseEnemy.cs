using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(Animator))]
public abstract class BaseEnemy : MonoBehaviour
{
    //private: variables that can only be accessed within this class
    //public: variables that can be accessed from other classes if you have a reference to this class
    //protected: variables that can be accessed from this class and any class that inherits from it

    protected SpriteRenderer sr;
    protected Animator anim;
    protected int health;

    [SerializeField] protected int maxHealth = 1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public virtual void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        if (maxHealth <= 0)
        {
            maxHealth = 1;
            Debug.LogWarning("BaseEnemy: MaxHealth not defined - setting to a default of 1");
        }

        health = maxHealth;
    }

    public virtual void TakeDamage(int damage, DamageType damageType = DamageType.Default)
    {
        health -= damage;
        if (health <= 0)
        {
            anim.SetTrigger("Death");

            //destroying the gameobject after a certain period of time to allow for the death animation to play

            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject, 0.5f);
            }
            else
                Destroy(gameObject, 0.5f);
        }
    }
}

public enum DamageType
{
    Default,
    JumpedOn
}
