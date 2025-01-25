using System;
using System.Collections;
using UnityEngine;

public class MissileSpawner : MonoBehaviour
{
    
    public GameObject MissilePrefab;
    public bool isPlayer; 
    
    public void Start()
    {
        if (!isPlayer)
        {
            SpawnMissiles();
        }
    }

    public void SpawnMissile()
    {
        GameObject missile = Instantiate(MissilePrefab, transform.position, transform.rotation);
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
