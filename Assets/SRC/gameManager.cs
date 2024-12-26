using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager;
    public UI ui;
    public Player player;

    [Header("Score info")]
    public int coins;
    public float dist;
    public float score;

    private void Awake() {
        gameManager = this;
        Time.timeScale = 1;
    }

    public void RestartLevel() {
        SceneManager.LoadScene(0);
    }
    public void UnlockPlayer() => player.playerUnlocked = true;

    private void Update() {
        if (player.transform.position.x > dist) dist = player.transform.position.x;
    }

    public void SaveInfo() {
        int savedCoins = PlayerPrefs.GetInt("TotalCoins", 0);      
        PlayerPrefs.SetInt("TotalCoins", savedCoins + coins);

        score = dist * coins;
        PlayerPrefs.SetFloat("LastScore", score);

        if (PlayerPrefs.GetFloat("BestScore") < score) PlayerPrefs.SetFloat("BestScore", score);
    }

    public void GameEnded() {
        SaveInfo();
        ui.OpenEndGameUI();
    }
}
