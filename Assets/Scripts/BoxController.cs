using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxController : MonoBehaviour
{
    public void OnCollisionEnter(Collision collision)
    {
        Draggable draggable = GetComponent<Draggable>();
        
        Debug.Log("Enabling draggable ?");

        if (collision.collider.tag == "Ground")
        {
            Debug.Log("Enabling draggable");
            draggable.enabled = true;
        }
    }
}
