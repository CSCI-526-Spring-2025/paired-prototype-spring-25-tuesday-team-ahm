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
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Bot_1 fires with space
        if (gameObject.name == "Gun" && transform.parent.name == "Bot_1" && Input.GetKeyDown(KeyCode.Q)) {
            FireBullet();
        }
        // Bot_2 fires with Enter
        else if (gameObject.name == "Gun" && transform.parent.name == "Bot_2" && Input.GetKeyDown(KeyCode.Slash)) {
            FireBullet();
        }
    }

    void FireBullet() {
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

        // Destory the bullet after 5 seconds to prevent clutter
        Destroy(bullet, 5f);
    }
}
