using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineController : MonoBehaviour
{

    private bool supervised = false;

    public TextMesh statusText;

    public GameObject spawner;
    
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (!supervised)
        {
            statusText.color = Color.red;
            statusText.text = "AAAAAAAAAAAAAH";
            spawner.SendMessage("DisableAutomaticAction");

        }

        if (supervised)
        {
            statusText.color = Color.green;
            statusText.text = "WORKING";
            spawner.SendMessage("EnableAutomaticAction");
        }
    }

    public void SetSupervised(bool state)
    {
        this.supervised = state;
        Debug.Log("SUPERVISED : " + this.supervised);
    }

    private void OnMouseDown()
    {
        if(!supervised)
        {
            spawner.SendMessage("ManualAction");
        } else
        {
            Debug.LogWarning("Manual spawn is not enabled at this time");
        }
    }
}
