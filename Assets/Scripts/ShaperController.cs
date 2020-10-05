using Assets.Scripts;
using Assets.Scripts.PropertyDrawer;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShaperController : MonoBehaviour, ITriggerObject
{
    private bool isAutomatic = false;
    [TagSelector]
    public List<string> TagMask = new List<string>();
    [Range(0, 1)]
    public double failureRate;
    public StateReflector stateReflector;
    [Header("Dynamo Settings")]
    public float clickValue;
    public float isShuttingDownThreshold;
    public float reloadedThreshold;
    public float reloadMaxValue;

    [Header("UI Feedback")]
    public Image FeedbackImage;
    public Color FeedbackColor;
    public Color FeedbackLoadedColor;
    public float FeedbackFadingTime;
    private float FeedbackRemainingTime;
    [SerializeField]
    [ReadOnly]
    private float reloadValue;
    [SerializeField]
    [ReadOnly]
    private bool isReloaded = false;
    private MachineController machineController;
    private System.Random rng;

    void Start()
    {
        machineController = GetComponentInParent<MachineController>();
        rng = new System.Random((Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds);
        transform.TransformDirection(Vector3.down);
    }

    // Update is called once per frame
    void Update()
    {
        if (machineController.IsShutdown())
        {
            if (reloadValue >= reloadedThreshold)
            {
                isReloaded = true;
                machineController.StopCoroutine(machineController.FreezeDestroyCountdown());
                machineController.StartCoroutine(machineController.FreezeDestroyCountdown());
                if (reloadValue < isShuttingDownThreshold)
                {
                    stateReflector.machineState = MachineState.ShuttingDown;
                }
                else {
                    stateReflector.machineState = MachineState.Working;
                }
            }
            else {
                isReloaded = false;
                stateReflector.machineState = MachineState.Shutdown;
                machineController.StopCoroutine(machineController.FreezeDestroyCountdown());
            }

            if (reloadValue > 0) {
                reloadValue -= Time.deltaTime;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (TagMask.Contains(other.gameObject.tag) && other.gameObject.GetComponent<CubeController>().GetCubeStep() == CubeStep.Shaper)
        {
            return;
        }

        bool isEnabled = isAutomatic || isReloaded;

        if(isEnabled)
        {
            if(TagMask.Contains(other.gameObject.tag))
            {
                if(other.gameObject.GetComponent<CubeController>().getState() == CubeState.Clean)
                {
                    if (rng.NextDouble() > failureRate)
                    {
                        other.gameObject.GetComponent<CubeController>().SendMessage("ShapeObject");
                    }
                    else
                    {
                        other.gameObject.GetComponent<CubeController>().setState(CubeState.Broken);
                    }
                }
                other.gameObject.GetComponent<CubeController>().SetCubeStep(CubeStep.Shaper);
            }
        }
    }


    public void EnableAutomaticAction()
    {
        isAutomatic = true;
        stateReflector.machineState = MachineState.Working;
    }
    public void DisableAutomaticAction()
    {
        isAutomatic = false;
        stateReflector.machineState = MachineState.Shutdown;
    }
    public void ManualAction(ActionType actionType)
    {
        if (actionType == ActionType.MouseDown)
        {
            if (reloadValue < reloadMaxValue) {
                reloadValue += clickValue;
            } else {
                reloadValue = reloadMaxValue;
            }

            StopCoroutine("DynamoVisualFeedback");
            FeedbackImage.gameObject.SetActive(false);
            FeedbackRemainingTime = FeedbackFadingTime;
            StartCoroutine("DynamoVisualFeedback");
        }
    }

    IEnumerator DynamoVisualFeedback()
    {
        FeedbackImage.gameObject.SetActive(true);

        while (FeedbackRemainingTime > 0) {
            FeedbackImage.transform.position = Input.mousePosition;
            FeedbackImage.fillAmount = reloadValue / reloadMaxValue;
            FeedbackRemainingTime -= Time.deltaTime;
            Color newColor;
            if (reloadValue >= reloadedThreshold) {
                FeedbackImage.transform.localScale = Vector3.one * 1.2f;
                newColor = FeedbackLoadedColor;
            } else {
                FeedbackImage.transform.localScale = Vector3.one;
                newColor = FeedbackColor;
            }
            newColor.a = FeedbackRemainingTime / FeedbackFadingTime;
            FeedbackImage.color = newColor;

            yield return null;
        }

        FeedbackImage.gameObject.SetActive(false);
    }
}
