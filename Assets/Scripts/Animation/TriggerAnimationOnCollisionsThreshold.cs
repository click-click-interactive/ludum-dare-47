using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class TriggerAnimationOnCollisionsThreshold : MonoBehaviour
{
    public int CollisionNumberBeforeTrigger;
    public bool isTriggered = false;
    public string AnimatorTrigger;
    private int collisionCount = 0;

    void Update()
    {
        if (isTriggered)
        {
            GetComponent<Animator>().SetTrigger(AnimatorTrigger);

            isTriggered = false;
            collisionCount = 0;
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        collisionCount += 1;
        
        if (collisionCount == CollisionNumberBeforeTrigger)
        {
            isTriggered = true;
        }
    }
}
