using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{
    public GameObject[] powerUpPrefabs;  // Assign your cube prefab in the Inspector
    public Vector2 arenaSize = new Vector2(65f, 39f); // Set arena bounds
    public float spawnInterval = 3f; // Time between spawns
    public float lifetime = 1f; // Time before disappearing
    private GameObject currentPowerUp;

    public void StartSpawning()
    {
        StartCoroutine(SpawnPowerUpRoutine());
    }

    IEnumerator SpawnPowerUpRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            if (currentPowerUp == null) // Ensure only one exists at a time
            {
                SpawnPowerUp();
            }
        }
    }

    void SpawnPowerUp()
    {
        Vector3 spawnPosition = new Vector3(
           UnityEngine.Random.Range(-arenaSize.x / 2, arenaSize.x / 2), // Random X position
           UnityEngine.Random.Range(-arenaSize.y / 2, arenaSize.y / 2),  // Random Y position
            0f // Set the height (Z) so it�s above the ground
        );

        System.Random rnd = new System.Random();
        int spawnPowerUpIdx = rnd.Next(0, powerUpPrefabs.Length);

        currentPowerUp = Instantiate(powerUpPrefabs[spawnPowerUpIdx], spawnPosition, Quaternion.identity);

        PowerUp powerUp = currentPowerUp.GetComponent<PowerUp>();
        if (powerUp.powerType == PowerType.ColorSwitch)
        {
            powerUp.transform.Rotate(new Vector3(90, 0, 0));
        }
        else
        {
            currentPowerUp.AddComponent<PowerUpRotation>(); // Attach rotation script
        }
        StartCoroutine(DestroyPowerUpAfterTime(currentPowerUp, lifetime));
    }


    IEnumerator DestroyPowerUpAfterTime(GameObject powerUp, float time)
    {
        yield return new WaitForSeconds(time);
        if (powerUp != null)
        {
            Destroy(powerUp);
        }
    }
}

