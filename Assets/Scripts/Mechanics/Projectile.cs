using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    [SerializeField] private float lifetime = 10f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, lifetime);    
    }

    public void SetVelocity(Vector2 velocity)
    {
        GetComponent<Rigidbody2D>().linearVelocity = velocity;
    }
}
