using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GhostController : MonoBehaviour
{
    private NavMeshAgent agent;

    public Material defaultMaterial;
    public Material hightlightMaterial;

    public Transform[] waypoints;
    public Transform center;

    public float waypointPauseTime = 1;

    public int minStep;
    public int maxStep;
    public float speed;

    [HideInInspector] public Light light;
    [HideInInspector] public LightRaycaster lightRaycaster;

    


    private int currentWaypointIndex = 0;

    bool alreadyWaitingForPlayerTurn = false;

    int steps = 1;
    int stepCounter = 0;

    bool nextWaypointSet = true;

    FMODUnity.StudioEventEmitter movementEmitter;
    bool movementAudioIsPlaying = false;


    private static GhostController _instance;
    public static GhostController Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    private void Start()
    {
        light = GetComponentInChildren<Light>();
        movementEmitter = GetComponent<FMODUnity.StudioEventEmitter>();
        agent = GetComponent<NavMeshAgent>();
        steps = CalculateStep();
        lightRaycaster = GetComponent<LightRaycaster>();
    }

    private void Update()
    {
        if (agent.velocity.magnitude > 1f)
        {
            if (!movementAudioIsPlaying)
            {
                movementEmitter.Play();
                movementAudioIsPlaying = true;
            }
        }
        else if (agent.velocity.magnitude <= 3f)
        {
            movementEmitter.Stop();
            movementAudioIsPlaying = false;
        }


        if (GameManager.Instance.gameState == GameManager.GameState.EnemyTurn)
        {
            WaypointNavigation();

            if (stepCounter == steps)
            {
                StartCoroutine(WaitForPlayerTurn());
            }
        }
    }

    void WaypointNavigation()
    {




        if (agent.remainingDistance <= agent.stoppingDistance && stepCounter <= steps)
        {
            if (currentWaypointIndex == waypoints.Length - 1)
            {
                currentWaypointIndex = 0;
                stepCounter++;
            }

            else
            {
                currentWaypointIndex++;
                stepCounter++;
            }
            StartCoroutine(WaitAtWaypoint());
        }
        agent.SetDestination(waypoints[currentWaypointIndex].position);
    }

    public void HighlightTargetWaypoint()
    {
        //Debug.Log("Highlighting next Field");
        int nextWaypointIndex = currentWaypointIndex + steps;

        if (nextWaypointIndex >= waypoints.Length)
            nextWaypointIndex = nextWaypointIndex - waypoints.Length;


        waypoints[nextWaypointIndex].GetComponentInChildren<Renderer>().material = hightlightMaterial;
    }

    public void ResetHighlightColor()
    {
        foreach (Transform waypoint in waypoints)
        {
            waypoint.GetComponentInChildren<Renderer>().material = defaultMaterial;
        }
    }

    IEnumerator WaitAtWaypoint()
    {
        agent.speed = 0;
        yield return new WaitForSeconds(waypointPauseTime);

        agent.speed = speed;
    }

    IEnumerator WaitForPlayerTurn()
    {
        if (!alreadyWaitingForPlayerTurn)
        {
            alreadyWaitingForPlayerTurn = true;
            yield return new WaitForSeconds(waypointPauseTime * 1.5f);
            GameManager.Instance.SwitchToPlayerTurn();
            steps = CalculateStep();
            stepCounter = 0;
            alreadyWaitingForPlayerTurn = false;
        }
    }

    int CalculateStep()
    {
        return Random.Range(minStep, maxStep);
    }
}
