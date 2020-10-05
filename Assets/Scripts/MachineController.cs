using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineController : MonoBehaviour
{

    private bool supervised = false;

    public TextMesh statusText;

    public GameObject spawner;
    public float ShutdownTimer;
    private bool isShuttingDown = false;
    private float shutdownRemainingTime;
    
    private bool isShutDown = false;
    public float DestroyTimer;
    private float destroyRemainingTime;
    private bool freezeCountdown = false;
    public float freezeTime;

    public SpriteRenderer ManualActionSpriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        if (ManualActionSpriteRenderer != null)
        {
            ManualActionSpriteRenderer.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!supervised)
        {
            if (isShuttingDown && shutdownRemainingTime > 0.0f) {
                statusText.color = Color.yellow;
                statusText.text = "SHUTTING DOWN IN " + Mathf.Ceil(shutdownRemainingTime);
                shutdownRemainingTime -= Time.deltaTime;
            } else if (isShuttingDown && shutdownRemainingTime <= 0.0f) {
                isShutDown = true;
                isShuttingDown = false;
                destroyRemainingTime = DestroyTimer;
            }
        }

        if (isShutDown)
        {
            statusText.color = Color.red;
            spawner.SendMessage("DisableAutomaticAction");
            if (destroyRemainingTime > 0.0f) {
                statusText.text = "MANUAL - BREAKDOWN IN " + string.Format("{0:00}:{1:00}", Mathf.FloorToInt(destroyRemainingTime / 60), Mathf.FloorToInt(destroyRemainingTime % 60));

                if (!freezeCountdown) {
                    destroyRemainingTime -= Time.deltaTime;
                }
            } else {
                // TODO Game over
            }
        }

        manageManualActionSpriteRenderer(isShutDown);
    }

    public void SetSupervised(bool state)
    {
        this.supervised = state;
        //Debug.Log("SUPERVISED : " + this.supervised);

        if (state == false) {
            isShuttingDown = true;
        } else {
            isShuttingDown = false;
            isShutDown = false;
            shutdownRemainingTime = ShutdownTimer;
            destroyRemainingTime = DestroyTimer;

            statusText.color = Color.green;
            statusText.text = "WORKING";
            spawner.SendMessage("EnableAutomaticAction");
        }
    }

    private void OnMouseDown()
    {
        if(isShutDown)
        {
            StopCoroutine("FreezeDestroyCountdown");
            spawner.SendMessage("ManualAction", ActionType.MouseDown);
            StartCoroutine("FreezeDestroyCountdown");
        } else
        {
            Debug.LogWarning("Manual spawn is not enabled at this time");
        }
    }

    private void OnMouseUp()
    {
        Debug.Log("GOT MOUSE UP");
        spawner.SendMessage("ManualAction", ActionType.MouseUp);
        
    }

    IEnumerator FreezeDestroyCountdown()
    {
        freezeCountdown = true;
        yield return new WaitForSeconds(freezeTime);
        freezeCountdown = false;
    }

    private void manageManualActionSpriteRenderer(bool state)
    {
        if(ManualActionSpriteRenderer != null)
        {
            if (state != ManualActionSpriteRenderer.enabled)
            {
                ManualActionSpriteRenderer.enabled = state;
            }
        }
        
    }
}
