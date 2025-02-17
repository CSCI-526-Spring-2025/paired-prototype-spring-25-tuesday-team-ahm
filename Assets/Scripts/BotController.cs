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

    public float maxMoveSpeed = 24;
    public float rotateSpeed = 220;

    public float colorSwitchDuration = 3;

    public float baseDamage = 1;

    public GameObject rival;

    public int maxColorSwitchTimes = 2;
    public float colorSwitchTimeout = 2;
    private float currentColorSwitchTimeout = 2;

    private int colorSwitchTimes = 0;
    public int ColorSwitchTimes
    {
        get { return colorSwitchTimes; }
    }

    private bool isOverlappingWithOtherBots = false;

    private bool colorSwitchFlag = false;
    private float currentColorSwitchingDuration = 0;
    private BotColors botColor;

    private BotController rivalController;

    private bool collisionEnabled = true;
    public bool CollisionEnabled
    {
        get { return collisionEnabled; }
    }

    private bool shootingEabled = false;
    public bool ShootingEabled
    {
        get { return shootingEabled; }
    }

    private float collisionTimeout = 0;

    private Rigidbody2D rb;

    private HealthManager healthManager;

    private projectile_attacks projectileAttackController;
    public int BulletCount
    {
        get
        {
            return projectileAttackController.BulletCount;
        }
    }

    private string playerTag = "";
    private bool skillHasPressed = false;

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

    private float velocity = 0;

    public float Velocity
    {
        get { return velocity; }
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

        if (currentColorSwitchTimeout < colorSwitchTimeout)
        {
            return;
        }


        if (flag)
        {
            colorSwitchTimes -= 1;

            colorSwitchFlag = true;
            currentColorSwitchingDuration = 0;

            handleMaterialColorSwitching(isPlayerOne ? Color.black : Color.white);
            botColor = isPlayerOne ? BotColors.Black : BotColors.White;
        }
        else
        {
            if (isOverlappingWithOtherBots)
            {
                return;
            }

            colorSwitchFlag = false;
            currentColorSwitchTimeout = 0;

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

        Collider2D collider = body.GetComponent<Collider2D>();
        Collider2D rivalCollider = rivalBody.GetComponent<Collider2D>();

        if (body != null && rivalBody != null)
        {
            Physics2D.IgnoreCollision(collider, rivalCollider, !collidable);
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
        playerTag = isPlayerOne ? "P1" : "P2";

        resetColor();

        rb = GetComponent<Rigidbody2D>();

        rivalController = rival.GetComponent<BotController>();

        healthManager = GetComponent<HealthManager>();

        projectileAttackController = transform.Find("Gun").GetComponent<projectile_attacks>();
    }

    private void handleInput()
    {
        float verticalInput = Input.GetAxis($"Vertical_{playerTag}");
        float horizontalInput = Input.GetAxis($"Horizontal_{playerTag}");

        rb.MoveRotation(rb.rotation - horizontalInput * rotateSpeed * Time.fixedDeltaTime);
        rb.AddForce(transform.up * 10000 * verticalInput);

        this.velocity = rb.velocity.magnitude;
        if (rb.velocity.magnitude > maxMoveSpeed)
        {
            rb.velocity = Vector2.Scale(rb.velocity.normalized, new Vector2(maxMoveSpeed, maxMoveSpeed));
        }

        // currently using space & left ctrl for skills
        float skillInput = Input.GetAxis($"Skill_{playerTag}");
        if (skillInput == 1 && !skillHasPressed)
        {
            skillHasPressed = true;

            // handle skill input
            if (colorSwitchTimes > 0)
            {
                SwitchColor(true);
            }
            else if (!colorSwitchFlag && shootingEabled)
            {
                projectileAttackController.FireBullet();
            }
        }
        else if (skillInput == 0)
        {
            skillHasPressed = false;
        }
    }


    private void handleColorSwitchTimeout()
    {
        if (colorSwitchFlag)
        {
            // timer
            currentColorSwitchingDuration += Time.fixedDeltaTime;
            if (currentColorSwitchingDuration > colorSwitchDuration)
            {
                SwitchColor(false);
            }
        }

        // collision
        if (rivalController != null)
        {
            switchCollidableState(rivalController.botColor != botColor);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        handleInput();

        collisionTimeout += Time.fixedDeltaTime;
        currentColorSwitchTimeout += Time.fixedDeltaTime;

        // handle color switching
        handleColorSwitchTimeout();
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (rivalController.botColor == botColor)
        {
            return;
        }

        if (collisionEnabled && collision.collider.name == "Body")
        {
            if (collisionTimeout > 0.5f)
            {
                float rivalSpeed = rivalController.Velocity;
                if (velocity == 0 && rivalSpeed == 0)
                {
                    return;
                }

                float speedFactor = rivalSpeed / (velocity + rivalSpeed);
                speedFactor = (Mathf.Pow(2, speedFactor) - 1) * rivalSpeed / maxMoveSpeed; // 2^x - 1

                ContactPoint2D contact = collision.contacts[0];
                float contactDir = Vector2.Dot(contact.normal, transform.up);
                float rivalDir = Vector2.Dot(transform.up, rival.transform.up);

                if (rivalDir < -0.3f) // 45deg
                {
                    speedFactor *= 1.5f;
                }

                float angleFactor = 1 - contactDir;
                angleFactor = -0.5f * Mathf.Pow(angleFactor, 2) + 2 * angleFactor + 1; // -1/2 x^2 + 2x + 1

                float damage = baseDamage * speedFactor * angleFactor;

                Debug.Log($"velocity: {velocity}, speed factor: {speedFactor}, angle factor: {angleFactor}, damage: {damage}");

                healthManager.TakeDamage(Mathf.Min(damage, 15));

                collisionTimeout = 0;
            }
        }
    }

    public void CollectPowerUp(PowerType powerType)
    {
        string player = isPlayerOne ? "Player 1" : "Player 2";
        Debug.Log($"{player} collected {powerType}");

        if (powerType == PowerType.Bullets)
        {
            // Enable bullet shooting
            colorSwitchTimes = 0;
            shootingEabled = true;
            projectileAttackController.ResetBullets();
        }
        else if (powerType == PowerType.ColorSwitch)
        {
            colorSwitchTimes = maxColorSwitchTimes;
            shootingEabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (colorSwitchFlag && collision.gameObject.GetComponent<BotController>() != null)
        {
            isOverlappingWithOtherBots = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<BotController>() != null)
        {
            isOverlappingWithOtherBots = false;
        }
    }
}
