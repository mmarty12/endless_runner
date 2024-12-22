using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class limitedGizmos : MonoBehaviour
{
    [SerializeField] private Transform start;
    [SerializeField] private Transform end;
    [SerializeField] private Transform groundLevel;
    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(start.position, new Vector2(start.position.x, start.position.y + 5000));
        Gizmos.DrawLine(start.position, new Vector2(start.position.x, start.position.y - 5000));

        Gizmos.DrawLine(end.position, new Vector2(end.position.x, end.position.y + 5000));
        Gizmos.DrawLine(end.position, new Vector2(end.position.x, end.position.y - 5000));
        
        Gizmos.DrawLine(groundLevel.position, new Vector2(groundLevel.position.x + 5000, groundLevel.position.y));
        Gizmos.DrawLine(groundLevel.position, new Vector2(groundLevel.position.x - 5000, groundLevel.position.y));
    }
}
