using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
    public GameObject cube;
    
    public float spawnInterval;

    // Percentage chance of spawn to fail.
    // A failed spawn instantiates an 'errorObjectToSpawn'
    // Value range [0, 1]
    public double failureRate;

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
            GameObject spawnedObject = Instantiate(cube, transform.position, transform.rotation, transform);
            if (rng.NextDouble() <= failureRate)
            {
                // an error happened
                spawnedObject.GetComponent<CubeController>().setState(CubeState.Dirty);
                spawnedObject.name = "BrokenCube";
                spawnedObject.tag = "BrokenCube";
            } else
            {
                spawnedObject.GetComponent<CubeController>().setState(CubeState.Clean);
                spawnedObject.name = "CleanCube";
                spawnedObject.tag = "CleanCube";
            }
            
            spawnedObject.SetActive(true);

            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
