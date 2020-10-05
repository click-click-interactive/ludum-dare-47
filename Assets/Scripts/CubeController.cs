using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeController : MonoBehaviour
{

    private CubeState state;
    public Mesh brokenMesh;
    private Mesh originalMesh;

    public CubeState getState()
    {
        return this.state;
    }

    public void setState(CubeState state)
    {
        this.state = state;
        if (this.state == CubeState.Clean)
        {
            this.GetComponent<Renderer>().material.color = Color.green;
        }
        else
        {
            this.GetComponent<Renderer>().material.color = Color.red;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        this.originalMesh = this.gameObject.GetComponent<MeshFilter>().mesh;
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnMouseDown()
    {
        if(this.state == CubeState.Dirty)
        {
            setState(CubeState.Clean);
            tag = "CleanCube";
        }
    }

    public void ShapeObject()
    {
        this.GetComponent<MeshFilter>().mesh = this.brokenMesh;
        this.GetComponent<BoxCollider>().enabled = false;
        this.GetComponent<SphereCollider>().enabled = true;
    }

    public void OnTriggerEnter(Collider collider)
    {
        Draggable draggable = GetComponent<Draggable>();
        
        if (collider.tag == "Ground")
        {
            draggable.enabled = true;
        }
    }
}
