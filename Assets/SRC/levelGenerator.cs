using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class levelGenerator : MonoBehaviour
{
    [SerializeField] private Transform[] levelPart;
    [SerializeField] private UnityEngine.Vector3 nextPartPositon;
    [SerializeField] private float distanceToSpawn;
    [SerializeField] private float distanceToDelete;
    [SerializeField] private Transform player;

    void Update()
    {
        DeletePlatform();
        GeneratePlatform();
    }

    private void GeneratePlatform() {
            while (UnityEngine.Vector2.Distance(player.transform.position, nextPartPositon) < distanceToSpawn) {
                Transform part = levelPart[Random.Range(0, levelPart.Length)];
                UnityEngine.Vector2 newPosition = new UnityEngine.Vector2(nextPartPositon.x - part.Find("startPoint").position.x, 0);
                Transform newPart = Instantiate(part, newPosition, transform.rotation, transform);

                nextPartPositon = newPart.Find("endPoint").position;
        }
    }

    private void DeletePlatform() {
        if (transform.childCount > 0) {
            Transform partToDelete = transform.GetChild(0);
            
            if (UnityEngine.Vector2.Distance(player.transform.position, partToDelete.transform.position) > distanceToDelete) {
                Destroy(partToDelete.gameObject);
            }
        }
    }
}
