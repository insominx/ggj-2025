using System;
using System.Collections;
using UnityEngine;

public class MissileSpawner : MonoBehaviour
{
    public GameObject MissilePrefab;
    public int spawnerType; 
    public GameObject parentObject;
    
    public void Start()
    {
        if (spawnerType == 1)
        {
            SpawnMissiles();
        }
    }

    public void SpawnMissile()
    {
        GameObject missile = Instantiate(MissilePrefab, transform.position, transform.rotation);
        Physics.IgnoreCollision(missile.GetComponent<Collider>(), parentObject.GetComponent<Collider>());
    }

    // Continuously spawns missiles over time
    public void SpawnMissiles()
    {
        StartCoroutine(waitThenSpawnMissiles());
    }

    // Currently waits for three seconds then spawns a missile
    IEnumerator waitThenSpawnMissiles()
    {
        yield return new WaitForSeconds(3f);
        SpawnMissile();
        StartCoroutine(waitThenSpawnMissiles());
    }
}
