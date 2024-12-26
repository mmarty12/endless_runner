using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ledgeDetection : MonoBehaviour
{
    [SerializeField] private float radius;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private Player player;

    private bool canDetect;
    private BoxCollider2D bc => GetComponent<BoxCollider2D>();

    void Update() {
        if (canDetect) {
            player.ledgeDetected = Physics2D.OverlapCircle(transform.position, radius, whatIsGround);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.layer == LayerMask.NameToLayer("platform")) {
            canDetect = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {

        if (collision.gameObject.layer == LayerMask.NameToLayer("platform")) {
            canDetect = true;
        }
    }
    private void OnDrawGizmos() {
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
