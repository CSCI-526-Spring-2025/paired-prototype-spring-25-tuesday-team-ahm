using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotController : MonoBehaviour
{
    public bool isPlayerOne = true;

    public float moveSpeed = 16f;
    public float rotateSpeed = 160f;


    // Start is called before the first frame update
    void Start()
    {
       
        Renderer[] childRenderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in childRenderers)
        {
            if (renderer.material != null)
            {
                renderer.material.color = isPlayerOne ? Color.white : Color.black;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        float verticalInput = Input.GetAxis($"Vertical_{(isPlayerOne ? "P1" : "P2")}");
        float horizontalInput = Input.GetAxis($"Horizontal_{(isPlayerOne ? "P1" : "P2")}");

        transform.position += transform.up * verticalInput * moveSpeed * Time.deltaTime;
        transform.Rotate(0, 0, -horizontalInput * rotateSpeed * Time.deltaTime);
    }
}
