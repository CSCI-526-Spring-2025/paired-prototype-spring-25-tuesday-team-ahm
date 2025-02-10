using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BotColors
{
    White, Black
}


public class BotController : MonoBehaviour
{
    public bool isPlayerOne = true;

    public float moveSpeed = 16;
    public float rotateSpeed = 220;

    public float colorSwitchDuration = 2;

    public float health = 100;

    public GameObject rival;

    private bool colorSwitchFlag = false;
    private float currentColorSwitchingDuration = 0;
    private BotColors botColor;

    private BotController rivalController;

    private bool collisionEnabled = true;
    private float collisionTimeout = 0;

    private Rigidbody2D rb;

    public bool ColorSwitchFlag()
    { 
        return colorSwitchFlag; 
    }

    public BotColors BotColor()
    {
        return botColor;
    }

    public GameObject GetBody()
    {
        return transform.Find("Body").gameObject;
    }


    private void handleMaterialColorSwitching(Color color)
    {
        Renderer[] childRenderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in childRenderers)
        {
            if (renderer.material != null)
            {
                renderer.material.color = color;
            }
        }
    }


    public void SwitchColor(bool flag)
    {
        if (flag && colorSwitchFlag) 
        {
            return;
        }

        if (flag)
        {
            colorSwitchFlag = true;
            currentColorSwitchingDuration = 0;

            handleMaterialColorSwitching(isPlayerOne ? Color.black : Color.white);
            botColor = isPlayerOne ? BotColors.Black : BotColors.White;
        }
        else
        {
            colorSwitchFlag = false;

            resetColor();
        }
    }

    private void switchCollidableState(bool collidable)
    {
        if (collidable && collisionEnabled)
        {
            return;
        }

        if (!collidable && !collisionEnabled)
        {
            return;
        }

        GameObject body = GetBody();
        GameObject rivalBody = rivalController.GetBody();

        if (body != null && rivalBody != null)
        {
            Physics2D.IgnoreCollision(body.GetComponent<Collider2D>(),rivalBody.GetComponent<Collider2D>(), !collidable);
        }

        collisionEnabled = collidable;
    }

    private void resetColor()
    {
        handleMaterialColorSwitching(isPlayerOne ? Color.white : Color.black);
        botColor = isPlayerOne ? BotColors.White : BotColors.Black;
    }

    // Start is called before the first frame update
    void Start()
    {
        resetColor();

        rb = GetComponent<Rigidbody2D>();
        rivalController = rival.GetComponent<BotController>();

        if (isPlayerOne)
        {
            SwitchColor(true);  
        }
    }

    // Update is called once per frame
    void Update()
    {
        float verticalInput = Input.GetAxis($"Vertical_{(isPlayerOne ? "P1" : "P2")}");
        float horizontalInput = Input.GetAxis($"Horizontal_{(isPlayerOne ? "P1" : "P2")}");

        transform.position += transform.up * verticalInput * moveSpeed * Time.deltaTime;
        transform.Rotate(0, 0, -horizontalInput * rotateSpeed * Time.deltaTime);

        collisionTimeout += Time.deltaTime;

        // handle color switching
        if (colorSwitchFlag)
        {
            // timer
            currentColorSwitchingDuration += Time.deltaTime;
            if (currentColorSwitchingDuration > colorSwitchDuration)
            {
                SwitchColor(false);
            }

            // collision
            if (rivalController != null)
            {
                switchCollidableState(rivalController.botColor != botColor);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (rivalController.botColor == botColor)
        {
            return;
        }

        if (collisionEnabled && collision.name == "Body_Trigger")
        {
            if (collisionTimeout > 0.5f)
            {
                print("collision");

                collisionTimeout = 0;
            }
        }
    }
}
