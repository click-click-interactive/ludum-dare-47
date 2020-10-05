using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaperController : MonoBehaviour, ITriggerObject
{
    private bool isAutomatic = false;
    [TagSelector]
    public List<string> TagMask = new List<string>();
    [Range(0, 1)]
    public double failureRate;
    private System.Random rng;

    void Start()
    {
        rng = new System.Random((Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds);
        transform.TransformDirection(Vector3.down);
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<CubeController>().GetCubeStep() == CubeStep.Shaper)
        {
            return;
        }

        if(isAutomatic)
        {
            if(TagMask.Contains(other.gameObject.tag))
            {
                if(other.gameObject.GetComponent<CubeController>().getState() == CubeState.Clean)
                {
                    if (rng.NextDouble() > failureRate)
                    {
                        other.gameObject.GetComponent<CubeController>().SendMessage("ShapeObject");
                    }
                    else
                    {
                        other.gameObject.GetComponent<CubeController>().setState(CubeState.Broken);
                    }
                }
                other.gameObject.GetComponent<CubeController>().SetCubeStep(CubeStep.Shaper);
            }
        }
    }


    public void EnableAutomaticAction()
    {
        isAutomatic = true;
    }
    public void DisableAutomaticAction()
    {
        isAutomatic = false;
    }
    public void ManualAction(ActionType actionType)
    {
        throw new System.NotImplementedException();
    }
}
