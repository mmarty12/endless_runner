using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bgMovement : MonoBehaviour
{
    private GameObject cam;
    [SerializeField] private float parallaxEffect;
    private float length;
    private float xPos;
    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.Find("Main Camera");
        length = GetComponent<SpriteRenderer>().bounds.size.x;
        xPos = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        float distMoved = cam.transform.position.x * (1-parallaxEffect);
        float dist = cam.transform.position.x * parallaxEffect;

        transform.position = new Vector3(xPos + dist, transform.position.y);
        if(distMoved > xPos + length) {
            xPos = xPos + length;
        }
    }
}
