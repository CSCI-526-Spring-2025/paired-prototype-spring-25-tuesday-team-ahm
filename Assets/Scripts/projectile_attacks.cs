using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectile_attacks : MonoBehaviour
{
    public GameObject bulletPrefab;
    // The point from where the bullet will be fired
    public Transform firePoint;
    // Speed of the bullet
    public float bulletSpeed = 40f;

    // Count the bullets fired
    private int bulletFired;

    //Maximum number of bullets allowed
    public int maxBullet = 5;

    public int BulletCount
    {
        get { return maxBullet - bulletFired; }
    }

    public void ResetBullets()
    {
        bulletFired = 0;
    }


    void Start()
    {
        
    }


    public void FireBullet() 
    {

        // Check if the bot has bullets left
        if (bulletFired >= maxBullet) {
            return;
        }

        // Instantiate the bullet at firepoint's position and rotation
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        // Pass the shooter GameObject reference to the bullet
        BulletCollision bulletScript = bullet.GetComponent<BulletCollision>();
        if (bulletScript != null)
        {
            bulletScript.shooter = transform.root.gameObject;  // Pass the shooter bot itself
        }

        // Apply velocity to the bullet's Riditbody 2D to make it move
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        // Moves in the direction the gun is facing
        rb.velocity = firePoint.up * bulletSpeed;

        bulletFired++;

        // Destory the bullet after 5 seconds to prevent clutter
        Destroy(bullet, 5f);
    }
}
