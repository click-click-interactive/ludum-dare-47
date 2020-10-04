using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UI;

public class RobotController : MonoBehaviour
{

    public int Segments = 32;
    public Color InnerColor = Color.red;
    public float InnerRadius = 3;
    public float InnerObjectMultiplicator = 3.0f;

    public Color MediumColor = Color.yellow;
    public float MediumRadius = 6;
    public float MediumObjectMultiplicator = 1.5f;

    public Color OuterColor = Color.green;
    public float OuterRadius = 10;
    public float OuterObjectMultiplicator = 1.0f;

    public List<BeliefEntity> Beliefs = new List<BeliefEntity>();
    public Desire Intention = Desire.Work;

    public float FocusGauge = 100f;
    [Range(0, 100)]
    public float WorkThreshold = 75f;

    [Range(0, 100)]
    public float GazeThreshold = 50f;

    [Range(0, 100)]
    public float WanderThreshold = 25f;

    [Range(0, 100)]
    public float LeaveThreshold = 0f;

    public float Speed = 1.0f;

    private float step;

    public GameObject exitTarget;


    private List<GameObject> innerObjects = new List<GameObject>();
    private List<GameObject> mediumObjects = new List<GameObject>();
    private List<GameObject> outerObjects = new List<GameObject>();

    public TextMesh bdiText;

    public GameObject target;


    void Awake()
    {
        step = Speed * Time.deltaTime;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        updateBeliefs();
        float newValue = updateFocusGauge();
        if(this.FocusGauge + newValue <= 100 && this.FocusGauge + newValue >= 0)
        {
            this.FocusGauge += newValue;
        }
        
        this.Intention = updateIntention(this.FocusGauge);
        bdiText.text = getBdiDebugText();

        if (this.Intention == Desire.Gaze || this.Intention == Desire.Wander)
        {
            if(target == null)
            {
                target = getNearestTarget(innerObjects.Concat(mediumObjects).Concat(outerObjects).ToList());
            }
            else
            {
                if (!innerObjects.Concat(mediumObjects).ToList().FirstOrDefault(distraction => distraction.GetHashCode() == target.GetHashCode()))
                {
                    target = null;
                }
            }


            
        }


        if(this.Intention == Desire.Gaze)
        {
            if (target != null)
            {
                transform.LookAt(new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z));
            }
        }

        if(this.Intention == Desire.Wander)
        {
            
            if(target != null)
            {
                
                if (Vector3.Distance(transform.position, target.transform.position) > 2)
                {
                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z), step);
                }
                
                transform.LookAt(new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z));
            }
            
        }

        if(this.Intention == Desire.Leave)
        {
            if(target != null)
            {
                GetComponent<Rigidbody>().velocity = Vector3.zero;
                target = null;
            }
            transform.LookAt(new Vector3(exitTarget.transform.position.x, transform.position.y, exitTarget.transform.position.z));
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(exitTarget.transform.position.x, transform.position.y, exitTarget.transform.position.z), step);
        }

        

    }


    void OnMouseDown()
    {
        if(innerObjects.Count() > 0)
        {
            Debug.Log("----- INNER OBJECTS -----\n" + gameObjectListToString(innerObjects) + "\n" + "-------------------------");
        }

        if (mediumObjects.Count() > 0)
        {
            Debug.Log("----- MEDIUM OBJECTS -----\n" + gameObjectListToString(mediumObjects) + "\n" + "-------------------------");
        }

        if (outerObjects.Count() > 0)
        {
            Debug.Log("----- OUTER OBJECTS -----\n" + gameObjectListToString(outerObjects) + "\n" + "-------------------------");
        }   
    }

    private void updateBeliefs()
    {
        innerObjects = getGameObjectsWithinRadius(transform.position, InnerRadius);
        mediumObjects = getGameObjectsWithinRadius(transform.position, MediumRadius);
        mediumObjects = removeDuplicatesFromLists(mediumObjects, innerObjects);
        outerObjects = getGameObjectsWithinRadius(transform.position, OuterRadius);
        outerObjects = removeDuplicatesFromLists(outerObjects, mediumObjects.Concat(innerObjects).ToList());
    }


    private float updateFocusGauge()
    {
        float newValues = 0;

        innerObjects.ForEach(delegate (GameObject innerObject)
        {
            BeliefEntity belief = Beliefs.Find(b => b.name == innerObject.name);
            newValues += belief.value * InnerObjectMultiplicator;
        });

        mediumObjects.ForEach(delegate (GameObject mediumObject)
        {
            BeliefEntity belief = Beliefs.Find(b => b.name == mediumObject.name);
            newValues += belief.value * MediumObjectMultiplicator;
        });

        outerObjects.ForEach(delegate (GameObject outerObject)
        {
            BeliefEntity belief = Beliefs.Find(b => b.name == outerObject.name);
            newValues += belief.value * OuterObjectMultiplicator;
        });

        return newValues;
    }

    private Desire updateIntention(float focus)
    {
        Desire newIntention;
        if(focus >= WorkThreshold)
        {
            newIntention = Desire.Work;
        } 
        else if (focus >= GazeThreshold)
        {
            newIntention = Desire.Gaze;
        } 
        else if(focus >= WanderThreshold)
        {
            newIntention = Desire.Wander;
        }
        else
        {
            newIntention = Desire.Leave;
        }
        return newIntention;
    }

    private List<GameObject> getGameObjectsWithinRadius(Vector3 center, float radius)
    {
        List<GameObject> foundObjects = new List<GameObject>();
        Collider[] hitColliders = Physics.OverlapSphere(center, radius);
        foreach (var hitCollider in hitColliders)
        {
            if(Beliefs.SingleOrDefault(belief => hitCollider.gameObject.CompareTag(belief.name)) != null)
            {
                if(hitCollider.gameObject.GetHashCode() != this.gameObject.GetHashCode())
                {
                    foundObjects.Add(hitCollider.gameObject);
                }
            }
            
        }
        return foundObjects;
    }

    private void OnDrawGizmos()
    {
        DrawEllipse(transform.position, transform.up, transform.right, InnerRadius * transform.localScale.x, InnerRadius * transform.localScale.y, Segments, InnerColor);
        DrawEllipse(transform.position, transform.up, transform.right, MediumRadius * transform.localScale.x, MediumRadius * transform.localScale.y, Segments, MediumColor);
        DrawEllipse(transform.position, transform.up, transform.right, OuterRadius * transform.localScale.x, OuterRadius * transform.localScale.y, Segments, OuterColor);
    }

    private static void DrawEllipse(Vector3 pos, Vector3 up, Vector3 right, float radiusX, float radiusY, int segments, Color color, float duration = 0)
    {
        float angle = 0f;
        Quaternion rot = Quaternion.LookRotation(up, right);
        Vector3 lastPoint = Vector3.zero;
        Vector3 thisPoint = Vector3.zero;

        for (int i = 0; i < segments + 1; i++)
        {
            thisPoint.x = Mathf.Sin(Mathf.Deg2Rad * angle) * radiusX;
            thisPoint.y = Mathf.Cos(Mathf.Deg2Rad * angle) * radiusY;

            if (i > 0)
            {
                Debug.DrawLine(rot * lastPoint + pos, rot * thisPoint + pos, color, duration);
            }

            lastPoint = thisPoint;
            angle += 360f / segments;
        }
    }

    private string gameObjectListToString(List<GameObject> gameObjectList)
    {
        string str = "";
        gameObjectList.ForEach(delegate (GameObject go)
        {
            str += "\t" + go.name + " " + go.tag + "\n";
        });

        return str;

    }

    private List<GameObject> removeDuplicatesFromLists(List<GameObject> largerList, List<GameObject> smallerList)
    {
        List<GameObject> uniques = new List<GameObject>();

        foreach(GameObject gameObj in largerList)
        {
            if (!smallerList.Contains(gameObj))
            {
                uniques.Add(gameObj);
            }
        }
        return uniques;
    }

    private string getBdiDebugText()
    {
        return "Current belief : " + this.Intention.ToString() +"\nFocus Gauge : " + this.FocusGauge.ToString("0.000");   
    }


    private GameObject getNearestTarget(List<GameObject> distractions) 
    {
        GameObject bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;
        foreach (GameObject potentialTarget in distractions)
        {
            Vector3 directionToTarget = potentialTarget.transform.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget;
            }
        }

        return bestTarget;
    }
}
