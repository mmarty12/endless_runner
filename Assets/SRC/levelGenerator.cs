using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class levelGenerator : MonoBehaviour
{
    [SerializeField] private Transform[] levelPart;
    [SerializeField] private UnityEngine.Vector3 nextPartPositon;
    [SerializeField] private float distanceToSpawn;
    [SerializeField] private float distanceToDelete;
    [SerializeField] private Transform player;

    private bool useSpecificPart = true;
    private Transform part;

    void Update()
    {
        DeletePlatform();
        GeneratePlatform();
    }

    private Transform toggleLevelPart() {
        if (useSpecificPart) {
            part = levelPart[3];
        } else {
            part = levelPart[Random.Range(0, levelPart.Length)];
        }
        
        useSpecificPart = !useSpecificPart;
        return part;
    }

    private void GeneratePlatform() {
            while (Vector2.Distance(player.transform.position, nextPartPositon) < distanceToSpawn) {
                Transform part = toggleLevelPart();
                Vector2 newPosition = new Vector2(nextPartPositon.x - part.Find("startPoint").position.x, 0);
                Transform newPart = Instantiate(part, newPosition, transform.rotation, transform);

                nextPartPositon = newPart.Find("endPoint").position;
        }
    }

    private void DeletePlatform() {
        if (transform.childCount > 0) {
            Transform partToDelete = transform.GetChild(0);
            
            if (Vector2.Distance(player.transform.position, partToDelete.transform.position) > distanceToDelete) Destroy(partToDelete.gameObject);
        }
    }
}
