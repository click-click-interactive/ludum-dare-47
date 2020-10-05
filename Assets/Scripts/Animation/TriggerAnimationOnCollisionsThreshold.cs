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
    private int brokenCubeCount = 0;
    public bool frozen;

    void Update()
    {
        if (isTriggered && !frozen)
        {
            GetComponent<Animator>().SetTrigger(AnimatorTrigger);

            isTriggered = false;
            collisionCount = 0;
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        collisionCount += 1;

        if (collider.tag == "BrokenCube")
        {
            brokenCubeCount += 1;
            frozen = true;
        }

        if (collisionCount == CollisionNumberBeforeTrigger)
        {
            isTriggered = true;
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.tag == "BrokenCube")
        {
            brokenCubeCount -= 1;
        }

        if (brokenCubeCount == 0) {
            frozen = false;
        }
    }
}
