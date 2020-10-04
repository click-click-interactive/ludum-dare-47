using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Draggable : MonoBehaviour
{
    private Vector3 screenPoint;
    private Vector3 offset;
    private Rigidbody rigidbody;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }
 
    void OnMouseDown()
    {
        screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
    
        offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));

        if (rigidbody != null) {
            rigidbody.freezeRotation = true;
            rigidbody.useGravity = false;
        }
    }

    void OnMouseDrag()
    {
        // Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
 
        // Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
        // transform.position = curPosition;

        // Vector3 fwd = transform.TransformDirection(curScreenPoint);

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        if (Physics.Raycast(ray, out hit, 100)) {
            Transform objectHit = hit.transform;
            
            Debug.DrawRay(ray.origin, ray.direction, Color.yellow);

            Debug.Log(hit.transform.name);

            if (rigidbody != null)
                rigidbody.position = Vector3.Lerp(rigidbody.position, hit.point + Vector3.up * 2, 1);
            // Do something with the object that was hit by the raycast.
        }
    }
    void OnMouseUp()
    {
        if (rigidbody != null) {
            rigidbody.freezeRotation = false;
            rigidbody.useGravity = true;
        }
    }
}
