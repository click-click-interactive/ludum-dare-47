﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Killer : MonoBehaviour
{
    [TagSelector]
    public string TagFilter = "";
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

        if (collider.gameObject.tag == TagFilter) {
            if (ParticlesOnKill != null)
            {
                ParticlesOnKill.transform.position = collider.transform.position;
                ParticlesOnKill.Play();
            }
            Destroy(collider.gameObject);
        }
    }
}
