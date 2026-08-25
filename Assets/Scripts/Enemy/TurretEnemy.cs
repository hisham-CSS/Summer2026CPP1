using UnityEngine;

[RequireComponent(typeof(Shoot))]
public class TurretEnemy : BaseEnemy
{
    PlayerController playerInstance;

    [SerializeField] private float fireRate = 1f; // Time in seconds between shots
    private float timeSinceLastShot = 0f;

    Shoot shoot;

    private void Awake() => GameManager.Instance.OnPlayerSpawned += (player) => playerInstance = player;
   
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Start()
    {
        base.Start();

        shoot = GetComponent<Shoot>();

        if (fireRate <= 0f)
        {
            Debug.LogWarning("Fire rate must be greater than 0. Setting to default value of 1 second.");
            fireRate = 1f;
        }

        shoot.OnShotFired += (velocity) => timeSinceLastShot = 0f;
    }

    // Update is called once per frame
    void Update()
    { 
        AnimatorStateInfo animState = anim.GetCurrentAnimatorStateInfo(0);

        if (animState.IsName("Idle"))
        {
            timeSinceLastShot += Time.deltaTime;

            if (timeSinceLastShot >= fireRate)
            {
                anim.SetTrigger("Fire");
            }
        }
    }
}
