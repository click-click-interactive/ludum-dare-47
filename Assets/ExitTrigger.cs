using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitTrigger : MonoBehaviour
{
    public TriggerAnimationOnCollisionsThreshold CubeExit;

    void OnMouseDown()
    {
        CubeExit.frozen = false;
        CubeExit.isTriggered = true;
    }
}
