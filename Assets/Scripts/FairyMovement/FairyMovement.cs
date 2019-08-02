using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(LightDetector))]
public class FairyMovement : MonoBehaviour
{
    public bool isSelected;
    public bool canMove;
    public float speed = 3.5f;

    public LineRenderer lineRenderer;

    [SerializeField] private float maxPathLength = 5f;
    public float pathLengthLeft;

    private NavMeshPath path;


    LightDetector lightDetector;
    NavMeshAgent agent;



    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;

        path = new NavMeshPath();
        pathLengthLeft = maxPathLength;
        lightDetector = GetComponent<LightDetector>();
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isSelected)
        {
            lineRenderer.enabled = true;
            //Debug.Log(PathCalculations.PathLength(agent.path));
            //Debug.Log(pathLengthLeft);
        }
        else
        {
            lineRenderer.enabled = false;
        }
    }

    public float CalculatePath(Vector3 targetPosition)
    {
        NavMesh.CalculatePath(transform.position, targetPosition, NavMesh.AllAreas, path);

        if (path != null && path.corners.Length > 1)
        {
            lineRenderer.positionCount = path.corners.Length;
            for (int i = 0; i < path.corners.Length; i++)
            {
                lineRenderer.SetPosition(i, path.corners[i] + new Vector3(0, transform.position.y, 0));
            }
        }


        lineRenderer.SetPositions(path.corners);
        return PathCalculations.PathLength(path);
    }


    public void SetPath()
    {
        agent.SetPath(path);
        pathLengthLeft -= PathCalculations.PathLength(agent.path);
        isSelected = false;
    }
}
