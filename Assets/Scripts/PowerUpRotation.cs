using UnityEngine;

public class PowerUpRotation : MonoBehaviour
{
    public float rotationSpeed = 50f; // Adjust speed as needed

    void Update()
    {
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }
}