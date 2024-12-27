using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deadZoneTrigger : MonoBehaviour
{
    protected virtual void OnTriggerEnter2D(Collider2D collision) {
        if (collision.GetComponent<Player>() != null) {
            AudioManager.audioManager.PlaySFX(3);
            GameManager.gameManager.GameEnded();
        }
    }
}