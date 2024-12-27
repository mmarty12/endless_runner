using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_jumpBtn : MonoBehaviour, IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData) => GameManager.gameManager.player.JumpButton();
}
