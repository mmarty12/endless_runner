using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UI_ingame : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI dist;
    [SerializeField] private TextMeshProUGUI coins;
    [SerializeField] private Image heartEmpty;    
    [SerializeField] private Image heartFull;

    private Player player;

    void Start()
    {
        player = GameManager.gameManager.player;
        InvokeRepeating("UpdateInfo", 0, .15f);
    }

    private void UpdateInfo()
    {
        dist.text = GameManager.gameManager.dist.ToString("#,#") + " m";
        coins.text = GameManager.gameManager.coins.ToString("#,#");

        heartEmpty.enabled = !player.extraLife;
        heartFull.enabled = player.extraLife;
    }
}
