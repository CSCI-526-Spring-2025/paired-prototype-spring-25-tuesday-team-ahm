using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // for Slider

public class GameManager : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public TMP_Text timerText;
    public Button restartButton;

    public float gameDuration = 180f;
    private float timer;
    private bool isGameActive = false;

    public Slider playerOneHealthSlider;
    public Slider playerTwoHealthSlider;

    public TMP_Text playerOnePowerText;  // UI text to display P1's power
    public TMP_Text playerTwoPowerText;  // UI text to display P2's power

    public GameObject playerOne;
    public GameObject playerTwo;
    public PowerUpSpawner powerUpSpawner;


    void Start()
    {
        ShowMainMenu();
    }

    void ShowMainMenu()
    {
        Time.timeScale = 0f; // Pause the game at the start
        mainMenuPanel.SetActive(true);
        restartButton.gameObject.SetActive(false);
    }

    public void StartGame()
    {
        mainMenuPanel.SetActive(false);

        timer = gameDuration;
        isGameActive = true;
        Time.timeScale = 1f;
        powerUpSpawner.StartSpawning();
    }


    void Update()
    {
        if (isGameActive)
        {
            timer -= Time.deltaTime;
            UpdateTimerUI();
            UpdatePlayerPowerUI();
            // Check if either player's health has reached zero
            if (playerOneHealthSlider.value <= 0 || playerTwoHealthSlider.value <= 0)
            {
                EndGame();
            }

            if (timer <= 0)
            {
                EndGame();
            }
        }
    }

    void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(timer / 60);
        int seconds = Mathf.FloorToInt(timer % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
    void UpdatePlayerPowerUI()
    {
        if (playerOne != null && playerTwo != null)
        {
            BotController p1 = playerOne.GetComponent<BotController>();
            BotController p2 = playerTwo.GetComponent<BotController>();

            string p1Power = p1.ColorSwitchTimes > 0 ? $"Color Switch: {p1.ColorSwitchTimes}" : (p1.ShootingEabled && p1.BulletCount > 0) ? $"Bullets: {p1.BulletCount}" : "None";
            string p2Power = p2.ColorSwitchTimes > 0 ? $"Color Switch: {p2.ColorSwitchTimes}" : (p2.ShootingEabled && p2.BulletCount > 0) ? $"Bullets: {p2.BulletCount}" : "None";

            playerOnePowerText.text = "P1 Power: " + p1Power;
            playerTwoPowerText.text = "P2 Power: " + p2Power;
        }
    }

    void EndGame()
    {
        isGameActive = false;
        timer = 0;

        if (playerOneHealthSlider.value > playerTwoHealthSlider.value)
        {
            timerText.text = "Game Over! Player One Wins!";
        }
        else
        {
            timerText.text = "Game Over! Player Two Wins!";
        }

        restartButton.gameObject.SetActive(true);
        Time.timeScale = 0f; // Stops all in-game movement
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }
}
