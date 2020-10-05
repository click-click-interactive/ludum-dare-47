using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxingController : MonoBehaviour, ITriggerObject
{
    [TagSelector]
    public List<string> TagMask;
    public GameObject box;
    private bool isAutomatic = true;


    private void OnTriggerEnter(Collider other)
    {
        if(isAutomatic && TagMask.Contains(other.tag))
        {
            if(other.gameObject.GetComponent<CubeController>().getState() == CubeState.Clean)
            {
                // other.gameObject.transform.parent.SendMessage("PackObject");
                other.gameObject.GetComponent<CubeController>().SendMessage("PackObject");
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
