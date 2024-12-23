using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI : MonoBehaviour
{
    private bool gamePaused;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject endgameMenu;
    [SerializeField] private TextMeshProUGUI lastScoreText;
    [SerializeField] private TextMeshProUGUI bestScoreText;
    [SerializeField] private TextMeshProUGUI coinsText;

    private void Start() {
        SwitchMenu(mainMenu);
        lastScoreText.text = "Last Score: " + PlayerPrefs.GetFloat("LastScore", 0).ToString("#,#");
        bestScoreText.text = "Best Score: " + PlayerPrefs.GetFloat("BestScore", 0).ToString("#,#");
    }
    public void SwitchMenu(GameObject uiMenu) {
        for(int i = 0; i < transform.childCount; i++) {
            transform.GetChild(i).gameObject.SetActive(false);
        }
        
        uiMenu.SetActive(true);
        AudioManager.audioManager.PlaySFX(4);
        coinsText.text = PlayerPrefs.GetInt("TotalCoins", 0).ToString("#,#");
    }

    public void StartGameBtn() => GameManager.gameManager.UnlockPlayer();

    public void PauseGameBtn() {
        if (gamePaused) {
            Time.timeScale = 1;
            gamePaused = false;
        } else {
            Time.timeScale = 0;
            gamePaused = true;
        }
    }

    public void RestartGameBtn() => GameManager.gameManager.RestartLevel();
    
    public void OpenEndGameUI() {
        SwitchMenu(endgameMenu);
    }
}
