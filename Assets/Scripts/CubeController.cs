using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeController : MonoBehaviour
{

    private CubeState state;
    public Mesh shapedMesh;
    public GameObject packedGameObject;
    private Mesh originalMesh;

    private CubeStep step;

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
        if(this.state == CubeState.Broken)
        {
            tag = "CleanCube";
            // SetState(Clean) is done for every step
            // SetState(Clean) is the only action to do when state = spawner
            setState(CubeState.Clean);
            if(step == CubeStep.Spawner)
            {
                FixSpawnObject();
            }
            
            else if(step == CubeStep.Shaper)
            {
                ShapeObject();
            }
            else if (step == CubeStep.Packer)
            {
                PackObject();
            }

        }
    }

    public void FixSpawnObject()
    {
        // Stub, no specific action
    }

    public void ShapeObject()
    {
        Debug.Log("Shaping object");
        this.GetComponent<MeshFilter>().mesh = this.shapedMesh;
        this.GetComponent<BoxCollider>().enabled = false;
        this.GetComponent<SphereCollider>().enabled = true;
    }

    public void PackObject()
    {
        Debug.Log("packing object");
        this.gameObject.GetComponent<MeshFilter>().sharedMesh = packedGameObject.GetComponent<MeshFilter>().sharedMesh;
        this.gameObject.GetComponent<MeshRenderer>().sharedMaterial = packedGameObject.GetComponent<MeshRenderer>().sharedMaterial;
        this.gameObject.GetComponent<SphereCollider>().enabled = false;
        this.gameObject.GetComponent<BoxCollider>().enabled = true;
        this.gameObject.transform.rotation = Quaternion.identity;
        this.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        this.GetComponent<Rigidbody>().freezeRotation = true;
    }

    public void OnCollisionEnter(Collision collision)
    {
        Draggable draggable = GetComponent<Draggable>();
        
        if (collision.collider.tag == "Ground")
        {
            draggable.enabled = true;
        }
    }

    public void SetCubeStep(CubeStep step)
    {
        this.step = step;
    }
}
