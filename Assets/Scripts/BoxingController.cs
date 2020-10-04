using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxingController : MonoBehaviour, ITriggerObject
{
    [TagSelector]
    public string TagMask;
    public GameObject box;

    private bool isAutomatic = true;


    private void OnTriggerEnter(Collider other)
    {
        if(isAutomatic)
        {
            if (other.gameObject.CompareTag(TagMask))
            {
                other.gameObject.GetComponent<MeshFilter>().sharedMesh = box.GetComponent<MeshFilter>().sharedMesh;
                other.gameObject.GetComponent<MeshRenderer>().sharedMaterial = box.GetComponent<MeshRenderer>().sharedMaterial;
            }
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

    public void ManualAction()
    {
        throw new System.NotImplementedException();
    }
}
