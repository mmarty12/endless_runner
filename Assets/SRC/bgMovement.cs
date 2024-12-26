using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgMovement : MonoBehaviour
{
    private GameObject cam;
    [SerializeField] private float parallaxEffect;
    private float length;
    private float xPos;

    void Start() {
        cam = Camera.main.gameObject;
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null) {
            length = spriteRenderer.bounds.size.x;
        } else {
            Debug.LogError("SpriteRenderer not found on object!");
        }
        xPos = transform.position.x;
    }

    void Update() {
        float distMoved = cam.transform.position.x * (1 - parallaxEffect);
        float dist = cam.transform.position.x * parallaxEffect;

        transform.position = new Vector3(xPos + dist, transform.position.y, transform.position.z);
        if (distMoved > xPos + length) {
            xPos += length;
        } else if (distMoved < xPos - length) {
            xPos -= length;
        }
    }
}
