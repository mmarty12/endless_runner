using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
   private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.GetComponent<Player>() != null) {
            AudioManager.audioManager.PlaySFX(0);
            GameManager.gameManager.coins++;
            Destroy(gameObject);
        }
    }
}
