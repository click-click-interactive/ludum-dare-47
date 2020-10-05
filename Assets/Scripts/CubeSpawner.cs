using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSpawner : MonoBehaviour, ITriggerObject
{
    public GameObject cube;
    public float spawnInterval;
    [Range(0, 1)]
    public double failureRate;
    private bool isCoroutineActive = false;
    private IEnumerator spawnRoutine;
    private System.Random rng;

    // Start is called before the first frame update
    void Start()
    {
        rng = new System.Random((Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds);
        spawnRoutine = Spawn();
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

    IEnumerator Spawn() {
        while(true) {
            Action();

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    public void ManualAction()
    {
        Action();
    }

    private void Action()
    {
        GameObject spawnedObject;

        if (rng.NextDouble() <= failureRate)
        {
            spawnedObject = ErrorAction();
        } else
        {
            spawnedObject = SuccessAction();
        }

        spawnedObject.GetComponent<CubeController>().SetCubeStep(CubeStep.Spawner);
        spawnedObject.SetActive(true);
    }

    private GameObject SuccessAction()
    {
        GameObject spawnedObject = Instantiate(cube, transform.position, transform.rotation, transform);
        spawnedObject.GetComponent<CubeController>().setState(CubeState.Clean);
        spawnedObject.name = "CleanCube";
        spawnedObject.tag = "CleanCube";

        return spawnedObject;
    }

    private GameObject ErrorAction()
    {
        GameObject spawnedObject = Instantiate(cube, transform.position, transform.rotation, transform);
        spawnedObject.GetComponent<CubeController>().setState(CubeState.Broken);
        spawnedObject.name = "BrokenCube";
        spawnedObject.tag = "BrokenCube";

        return spawnedObject;
    }
}
