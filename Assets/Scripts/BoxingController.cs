using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxingController : MonoBehaviour, ITriggerObject
{
    [TagSelector]
    public List<string> TagMask;
    public GameObject box;
    private bool isAutomatic = true;
    [Range(0, 1)]
    public double failureRate = 1f;
    private System.Random rng;
    public float ejectForce = 2.3f;

    void Start()
    {
        rng = new System.Random((Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(isAutomatic && TagMask.Contains(other.tag))
        {


            if (other.gameObject.GetComponent<CubeController>().getState() == CubeState.Clean)
            {
                if (rng.NextDouble() <= failureRate)
                {
                    other.gameObject.GetComponent<CubeController>().SendMessage("PackObject", false);
                } else
                {
                    other.gameObject.GetComponent<CubeController>().SendMessage("PackObject", true);
                }
            }

            other.gameObject.GetComponent<CubeController>().SetCubeStep(CubeStep.Packer);

        }
    }

    public void DisableAutomaticAction()
    {
        isAutomatic = false;
    }

    public void EnableAutomaticAction()
    {
        isAutomatic = true;
    }

    public void ManualAction(ActionType actionType)
    {
        throw new System.NotImplementedException();
    }
}
