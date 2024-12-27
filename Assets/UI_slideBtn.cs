using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_slideBtn : MonoBehaviour, IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData) => GameManager.gameManager.player.slideButton();
}
