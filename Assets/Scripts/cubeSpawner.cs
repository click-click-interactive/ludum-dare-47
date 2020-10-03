using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cubeSpawner : MonoBehaviour
{
    public GameObject nominalObjectToSpawn;

    public GameObject mishapedObjectToSpawn;

    public GameObject brokenObjectToSpawn;

    public float spawnInterval;

    // Percentage chance of spawn to fail.
    // A failed spawn instantiates an 'errorObjectToSpawn'
    // Value range [0, 1]
    public double failureRate;

    private double brokenCubeSpawnRate;

    private double mishapedObjectSpawnRate;

    private double noSpawnRate;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Spawn());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Spawn() {
        
        System.Random rng = new System.Random((Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds);
        
        while(true) {
            GameObject objectToSpawn = null;
            if(rng.NextDouble() <= failureRate)
            {
                // an error happened
                double randVar = rng.NextDouble();
                Debug.Log("Generating random error "+ randVar);
                if (randVar < 0.33f)
                {
                    Debug.Log("Generating broken object");
                    objectToSpawn = brokenObjectToSpawn;
                } else if (randVar >= 0.33f && randVar < 0.66f)
                {
                    Debug.Log("Generating mishaped object");
                    objectToSpawn = mishapedObjectToSpawn;
                } else if (randVar >= 0.66f)
                {
                    Debug.Log("Generating NO object");
                    objectToSpawn = null;
                }
            } else
            {
                // No error, nominal case
                Debug.Log("Generating normal object");
                objectToSpawn = nominalObjectToSpawn;
            }
            
            if(objectToSpawn != null)
            {
                GameObject spawnedObject = Instantiate(objectToSpawn, transform.position, transform.rotation, transform);
            }
            
                        
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
