using UnityEngine;

public class Life : BasePickup
{ 
    [SerializeField] private int livesToAdd = 1;
    private Rigidbody2D rb;

    public override void OnPickup(GameObject player)
    {
        GameManager.Instance.Lives = Mathf.Min(GameManager.Instance.Lives + livesToAdd, GameManager.Instance.maxLives);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = new Vector2(-4, 4);
    }

    // Update is called once per frame
    void Update()
    {
        rb.linearVelocityX = -2f;
    }
}
