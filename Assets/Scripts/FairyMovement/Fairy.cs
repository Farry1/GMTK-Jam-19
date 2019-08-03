using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(LightDetector))]
public class Fairy : MonoBehaviour
{
    public enum FairyState { Alive, Petrified };
    public FairyState fairyState;

    public bool isSelected;
    public bool canMove = true;
    public float speed = 3.5f;
    public float teamUpDistance = 2f;

    private Renderer renderer;
    private Material initialMaterial;
    public Material petrifiedMaterial;


    [HideInInspector] public LineRenderer lineRenderer;

    [SerializeField] private float maxPathLength = 5f;
    public float pathLengthLeft;

    private NavMeshPath path;


    LightDetector lightDetector;
    NavMeshAgent agent;

    private List<GameObject> otherFairies = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {        
        GetOtherFairies();
        renderer = GetComponent<Renderer>();
        initialMaterial = renderer.material;
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
        switch (fairyState)
        {
            case FairyState.Alive:
                if (isSelected)
                    lineRenderer.enabled = true;
                else
                    lineRenderer.enabled = false;
                break;
            case FairyState.Petrified:
                LookForHelp();
                break;
        }
    }

    private void GetOtherFairies()
    {
        GameObject[] fairies = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject fairy in fairies)
        {
            if (fairy != this.gameObject)
            {
                otherFairies.Add(fairy);
            }
        }

    }

    public void Petrify()
    {
        renderer.material = petrifiedMaterial;
        fairyState = FairyState.Petrified;
        CalculatePath(transform.position);
        SetPath();
        agent.speed = 0;
        GameManager.Instance.CheckForGameOver();
    }

    public void Revive()
    {
        renderer.material = initialMaterial;
        fairyState = FairyState.Alive;
        agent.speed = speed;
    }

    private void LookForHelp()
    {
        foreach (GameObject otherFairy in otherFairies)
        {
            float distance = Vector3.Distance(transform.position, otherFairy.transform.position);

            if (distance < teamUpDistance)
            {
                Revive();
            }
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
        if (pathLengthLeft < 0.35)
            pathLengthLeft = 0;
        isSelected = false;
    }

    public void ResetFairy()
    {
        Debug.Log("Reset Fairy " + gameObject.name);
        pathLengthLeft = maxPathLength;
    }
}
