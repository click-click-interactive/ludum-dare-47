using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateReflector : MonoBehaviour
{
    public Color WorkingColor;
    public Color ShuttingDownColor;
    public Color ShutdownColor;
    public Color BrokenColor;
    public MachineState machineState = MachineState.Shutdown;
    private MeshRenderer meshRenderer;

    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (machineState)
        {
            case MachineState.Working:
                meshRenderer.material.color = WorkingColor;
                break;
            
            case MachineState.ShuttingDown:
                meshRenderer.material.color = ShuttingDownColor;
                break;
            
            case MachineState.Shutdown:
                meshRenderer.material.color = ShutdownColor;
                break;

            case MachineState.Broken:
                meshRenderer.material.color = BrokenColor;
                break;

            default:
                break;
        }
    }
}
