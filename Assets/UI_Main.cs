using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{
    private bool gamePaused;
    [SerializeField] private GameObject mainMenu;

    private void Start() {
        Time.timeScale = 1;
        SwitchMenu(mainMenu);
    }
    public void SwitchMenu(GameObject uiMenu) {
        for(int i = 0; i < transform.childCount; i++) {
            transform.GetChild(i).gameObject.SetActive(false);
        }
        
        uiMenu.SetActive(true);
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
}
