using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaperController : MonoBehaviour, ITriggerObject
{
    private bool isAutomatic = false;
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
            if (other.gameObject.CompareTag("Product"))
            {
                other.gameObject.transform.parent.SendMessage("ShapeObject");
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
