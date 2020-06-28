using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sphereRespawn : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform sphere;
    [SerializeField] private Transform cube;
    [SerializeField] private Transform respawnPoint;

    void OnTriggerEnter(Collider other)
    {
        player.transform.position = new Vector3(Random.Range(-5.0f, 5.0f), 1.0f, Random.Range(-5.0f, 5.0f));
    }

}
