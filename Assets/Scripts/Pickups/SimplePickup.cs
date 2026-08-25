using UnityEngine;

public class SimplePickup : MonoBehaviour
{
    public enum PickupType
    {
        Health,
        JumpBoost,
    }

    [SerializeField] private PickupType type;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            switch (type)
            {
                case PickupType.Health:
                    // Implement health pickup logic here
                    GameManager.Instance.Lives = Mathf.Min(++GameManager.Instance.Lives, GameManager.Instance.maxLives);
                    Debug.Log("Picked up Health!");
                    break;
                case PickupType.JumpBoost:
                    // Implement jump boost logic here
                    Debug.Log("Picked up Jump Boost!");
                    player.StartJumpForceChange();
                    break;
            }
            // Destroy the pickup after it has been collected
            Destroy(gameObject);
        }
    }
}
