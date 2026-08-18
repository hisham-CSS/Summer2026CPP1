using System;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    private SpriteRenderer sr;

    //assumed the projectile will be shot from the right side of the player, if the player is facing left, the projectile will use a derived left shot velocity, which is this velocity but negative in the x direction
    [SerializeField] private Vector2 initShotVelocity = new Vector2(5, 5);
    [SerializeField] private Transform spawnPointLeft;
    [SerializeField] private Transform spawnPointRight;
    [SerializeField] private Projectile projectilePrefab;

    // Derived left shot velocity, which is the same as initShotVelocity but with a negative x component
    private Vector2 leftShotVelocity;

    public Action<Vector2> OnShotFired;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        if (initShotVelocity == Vector2.zero)
        {
            initShotVelocity = new Vector2(5, 5);
            Debug.LogWarning("Shoot: InitShotVelocity not defined - setting to a default 5, 5");
        }

        if (spawnPointLeft == null || spawnPointRight == null || projectilePrefab == null)
        {
            Debug.LogError("Shoot: one or more spawn points or projectile prefab is not assigned - in order to use the shoot component - it has to be assigned");
        }

        leftShotVelocity = new Vector2(-initShotVelocity.x, initShotVelocity.y);

    }

    // Update is called once per frame
    public void Fire()
    {
        if (spawnPointLeft == null || spawnPointRight == null || projectilePrefab == null)
        {
            Debug.LogError("Fire will not work because the shoot script is missing a spawn point or projectile prefab reference");
            return;
        }

        Projectile curProjectile;

        if (!sr.flipX)
        {
            curProjectile = Instantiate(projectilePrefab, spawnPointRight.position, Quaternion.identity);
            curProjectile.SetVelocity(initShotVelocity);
            OnShotFired?.Invoke(initShotVelocity);
        }
        else
        {
            curProjectile = Instantiate(projectilePrefab, spawnPointLeft.position, Quaternion.identity);
            curProjectile.SetVelocity(leftShotVelocity);
            OnShotFired?.Invoke(leftShotVelocity);
        }
        
    }
}
