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
    [Range(0, 1)]
    public double failureRate;
    private System.Random rng;
    private bool isAutomatic = true;

    void Start()
    {
        rng = new System.Random((Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<CubeController>().GetCubeStep() == CubeStep.Packer)
        {
            return;
        }

        if(isAutomatic && TagMask.Contains(other.tag))
        {
            if(other.gameObject.GetComponent<CubeController>().getState() == CubeState.Clean)
            {
                // other.gameObject.transform.parent.SendMessage("PackObject");
                if (rng.NextDouble() > failureRate)
                {
                    other.gameObject.GetComponent<CubeController>().SendMessage("PackObject");
                }
                else
                {
                    other.gameObject.GetComponent<CubeController>().setState(CubeState.Broken);
                }
                /*other.gameObject.GetComponent<MeshFilter>().sharedMesh = box.GetComponent<MeshFilter>().sharedMesh;
                other.gameObject.GetComponent<MeshRenderer>().sharedMaterial = box.GetComponent<MeshRenderer>().sharedMaterial;
                other.gameObject.GetComponent<SphereCollider>().enabled = false;
                other.gameObject.GetComponent<BoxCollider>().enabled = true;
                other.gameObject.transform.position = other.gameObject.transform.position + new Vector3(0, 0.5f, 0);
                other.gameObject.transform.rotation = Quaternion.identity;
                other.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;*/
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
