using UnityEngine;

public enum PowerType { Bullets, ColorSwitch }

public class PowerUp : MonoBehaviour
{
    public PowerType powerType; // Randomized at spawn

    void OnTriggerEnter2D(Collider2D collidedBot)
    {
        BotController bot = collidedBot.GetComponent<BotController>();
        if (bot != null)
        {
            bot.CollectPowerUp(powerType);
            Destroy(gameObject);
        }
    }
}
