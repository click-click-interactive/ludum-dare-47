using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxingController : MonoBehaviour
{
    [TagSelector]
    public string TagMask;
    public GameObject box;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.ToString());
        if (other.gameObject.CompareTag(TagMask))
        {
            other.gameObject.GetComponent<MeshFilter>().sharedMesh = box.GetComponent<MeshFilter>().sharedMesh;
            other.gameObject.GetComponent<MeshRenderer>().sharedMaterial = box.GetComponent<MeshRenderer>().sharedMaterial;
        }
    }
}
