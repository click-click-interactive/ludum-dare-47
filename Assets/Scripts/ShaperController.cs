using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaperController : MonoBehaviour, ITriggerObject
{
    private bool isAutomatic = false;
    [TagSelector]
    public List<string> TagMask = new List<string>();
    // Start is called before the first frame update
    void Start()
    {
        transform.TransformDirection(Vector3.down);
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if(isAutomatic)
        {
            if(TagMask.Contains(other.gameObject.tag))
            {
                if(other.gameObject.GetComponent<CubeController>().getState() == CubeState.Clean)
                {
                    other.gameObject.GetComponent<CubeController>().SendMessage("ShapeObject");
                }
                other.gameObject.GetComponent<CubeController>().SetCubeStep(CubeStep.Shaper);
            }
        }
    }


    public void EnableAutomaticAction()
    {
        isAutomatic = true;
    }
    public void DisableAutomaticAction()
    {
        isAutomatic = false;
    }
    public void ManualAction()
    {
        throw new System.NotImplementedException();
    }
}
