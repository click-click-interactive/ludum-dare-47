using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorBelt : MonoBehaviour
{
    public float speed;
    void Update()
    {
        // GetComponent<Rigidbody>().velocity = transform.forward;
        Debug.DrawRay(transform.position, transform.forward * 1000, Color.yellow);
    }
    void OnCollisionStay(Collision collision)
    {
        Rigidbody rb = collision.collider.GetComponent<Rigidbody>();

        if (rb != null) {
            rb.velocity = transform.forward * speed;
            Debug.DrawRay(collision.collider.transform.position, transform.forward, Color.blue);
            // Debug-draw all contact points and normals
            foreach (ContactPoint contact in collision.contacts)
            {
                Debug.DrawRay(contact.point, contact.normal * 10, Color.white);
            } 
        }
        
        // Rigidbody rb = collision.collider.GetComponent<Rigidbody>()
        // Vector3 velocityProjection = Vector3.Project(rb.velocity, transform.forward);
        // rb.velocity = rb.velocity - velocityProjection + (transform.forward * 0.75f);
    }

    
}
