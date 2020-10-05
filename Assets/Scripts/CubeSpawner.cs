using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSpawner : MonoBehaviour, ITriggerObject
{
    public GameObject cube;
    
    public float spawnInterval;

    // Percentage chance of spawn to fail.
    // A failed spawn instantiates an 'errorObjectToSpawn'
    // Value range [0, 1]
    [Range(0, 1)]
    public double failureRate;

    private bool isCoroutineActive = false;
    private IEnumerator spawnRoutine;

    

    // Start is called before the first frame update
    void Start()
    {
        spawnRoutine = Spawn();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void DisableAutomaticAction()
    {
        if(isCoroutineActive)
        {
            StopCoroutine(spawnRoutine);
            isCoroutineActive = false;
        }
    }

    public void EnableAutomaticAction()
    {
        if (!isCoroutineActive)
        {
            StartCoroutine(spawnRoutine);
            isCoroutineActive = true;
        }
    }

    private void resumeCoroutine()
    {
        
    }
    IEnumerator Spawn() {
        
        System.Random rng = new System.Random((Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds);
        
        while(true) {
            GameObject spawnedObject = Instantiate(cube, transform.position, transform.rotation, transform);
            if (rng.NextDouble() <= failureRate)
            {
                // an error happened
                spawnedObject.GetComponent<CubeController>().setState(CubeState.Broken);
                spawnedObject.name = "BrokenCube";
                spawnedObject.tag = "BrokenCube";
            } else
            {
                spawnedObject.GetComponent<CubeController>().setState(CubeState.Clean);
                spawnedObject.name = "CleanCube";
                spawnedObject.tag = "CleanCube";
            }
            spawnedObject.GetComponent<CubeController>().SetCubeStep(CubeStep.Spawner);
            spawnedObject.SetActive(true);

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    public void ManualAction()
    {
        GameObject spawnedObject = Instantiate(cube, transform.position, transform.rotation, transform);
        spawnedObject.GetComponent<CubeController>().setState(CubeState.Clean);
        spawnedObject.name = "CleanCube";
        spawnedObject.tag = "CleanCube";
        spawnedObject.SetActive(true);
    }
}
