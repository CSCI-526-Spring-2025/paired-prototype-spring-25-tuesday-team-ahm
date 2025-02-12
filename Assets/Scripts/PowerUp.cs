using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public enum PowerType { Bullets, ColorSwitch }
    public PowerType powerType; // Randomized at spawn


    void Start()
    {
        powerType = (Random.value > 0.5f) ? PowerType.Bullets : PowerType.ColorSwitch;
    }

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
