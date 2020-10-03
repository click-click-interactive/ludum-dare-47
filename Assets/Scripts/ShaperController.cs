using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaperController : MonoBehaviour
{
    // Start is called before the first frame update
    private Vector3 right;

    
    void Start()
    {
        right = transform.TransformDirection(Vector3.down);
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnCollisionEnter(Collision collision)
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.ToString());
        if (other.gameObject.CompareTag("Product"))
        {
            
            other.gameObject.transform.parent.SendMessage("ShapeObject");
            
        }
    }
}
