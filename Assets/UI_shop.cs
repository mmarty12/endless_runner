using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

[Serializable]
public struct ColorToSell {
    public Color color;
    public int price;
}

public class UI_shop : MonoBehaviour
{
    [SerializeField] private ColorToSell[] playerColors;
    [SerializeField] private GameObject playerColorButton;
    [SerializeField] private Transform playerColorParent;
    [SerializeField] private Image playerDisplay;
    [SerializeField] private TextMeshProUGUI notifyText;
    [SerializeField] private TextMeshProUGUI coinsText;

    void Start() {
        coinsText.text = PlayerPrefs.GetInt("TotalCoins", 0).ToString("#,#");  

        for (int i = 0; i < playerColors.Length; i++) {
            Color colorByIdx = playerColors[i].color;
            int priceByIdx = playerColors[i].price;

            GameObject newButton = Instantiate(playerColorButton, playerColorParent);
            newButton.transform.GetChild(0).GetComponent<Image>().color = colorByIdx;
            newButton.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = priceByIdx.ToString("#,#");

            newButton.GetComponent<Button>().onClick.AddListener(() => Purchase(colorByIdx, priceByIdx));
        }
    }

    public void Purchase(Color color, int price) {
        AudioManager.instance.PlaySFX(4);
        
        if (EnoughMoney(price)) {
            GameManager.gameManager.player.GetComponent<SpriteRenderer>().color = color;
            playerDisplay.color = color;
            StartCoroutine(Notify("Purchase successful!", 1));
        } else StartCoroutine(Notify("Not enough coins", 1));
    }

    private bool EnoughMoney(int price) {
        int coins = PlayerPrefs.GetInt("TotalCoins", 0); 

        if (coins > price) {
            int newAmount = coins - price;
            PlayerPrefs.SetInt("TotalCoins", newAmount);
            coinsText.text = PlayerPrefs.GetInt("TotalCoins", 0).ToString("#,#");  
            return true;
        } else return false;
    }

    IEnumerator Notify(string text, float seconds) {
        notifyText.text = text;
        yield return new WaitForSeconds(seconds);
        notifyText.text = "Click to Buy";
    }
}
