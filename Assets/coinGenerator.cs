using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class coinGenerator : MonoBehaviour
{
    [SerializeField] private int amount;
    [SerializeField] private int min;
    [SerializeField] private int max;
    [SerializeField] private int chance;
    [SerializeField] private GameObject coinPrefab;

    void Start()
    {
        int additionalOffset = amount / 2;
        for (int i = 0; i < amount; i++) {
            bool canSpawn = chance > Random.Range(0, 100);
            Vector3 offset = new Vector2(i - additionalOffset, 0);

            if (canSpawn) {
                Instantiate(coinPrefab, transform.position + offset, Quaternion.identity, transform);
            }
        }
    }
}
