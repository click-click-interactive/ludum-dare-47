using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Killer : MonoBehaviour
{
    [TagSelector]
    public List<string> TagFilter = new List<string>();
    public ParticleSystem ParticlesOnKill;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider collider)
    {
        Debug.Log("Killer triggered");

        if (TagFilter.Contains(collider.gameObject.tag)) {
            if (ParticlesOnKill != null)
            {
                ParticlesOnKill.transform.position = collider.transform.position;
                ParticlesOnKill.Play();
            }
            Destroy(collider.gameObject);
        }
    }
}
