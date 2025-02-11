using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCollision : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject shooter;
    public float damageByBullet = 20f;
    void OnCollisionEnter2D(Collision2D collision) {

        // // Ignore collision if the bullet hits the shooter itself
        if (collision.gameObject == shooter) {
            return;
        }


        // Check if the collided object has a HealthManager component
        HealthManager healthManager = collision.gameObject.GetComponent<HealthManager>();
        if (healthManager != null){
            // Apply damage to the bot
            healthManager.TakeDamage(damageByBullet);
        }


        // Check if the bullet hit a bot
        if (collision.gameObject.CompareTag("Bot_1") || collision.gameObject.CompareTag("Bot_2")) {
            // Destroy the bullet on impact
            Destroy(gameObject);
        }
    
    }
}
