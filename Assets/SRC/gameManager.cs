using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager;
    public int coins;

    private void Awake() {
        gameManager = this;
    }

    public void RestartLevel() => SceneManager.LoadScene(0);
}
