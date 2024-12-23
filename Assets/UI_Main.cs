using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    private bool gamePaused;
    private bool gameMuted;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject endgameMenu;
    [SerializeField] private TextMeshProUGUI lastScoreText;
    [SerializeField] private TextMeshProUGUI bestScoreText;
    [SerializeField] private TextMeshProUGUI coinsText;

    [Header("Volume Info")]
    [SerializeField] private audioMixer[] sliders;
    [SerializeField] private Image muteIcon;
    [SerializeField] private Image ingameMuteIcon;

    private void Start() {
        for (int i = 0; i < sliders.Length; i++) {
            sliders[i].SliderSetup();
        }


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

    public void StartGameBtn() {
        muteIcon = ingameMuteIcon;
        
        if (gameMuted) muteIcon.color = new Color(1, 1, 1, .3f);
        GameManager.gameManager.UnlockPlayer();
    }
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

    public void MuteBtn() {
        gameMuted = !gameMuted;

        if (gameMuted) {
            muteIcon.color = new Color(1, 1, 1, .3f);
            AudioListener.volume = 0;
        } else {
            muteIcon.color = Color.white;
            AudioListener.volume = 1;
        }
    }
}
