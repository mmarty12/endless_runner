using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_endgame : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI dist;
    [SerializeField] private TextMeshProUGUI score;
    [SerializeField] private TextMeshProUGUI coins;
    void Start()
    {
        dist.text = "Distance: " + GameManager.gameManager.dist.ToString("#,#") + " m";
        score.text = "Score: " + GameManager.gameManager.score.ToString("#,#");
        coins.text = "Coins: " + GameManager.gameManager.coins.ToString("#,#");
        Time.timeScale = 0;
    }
}
