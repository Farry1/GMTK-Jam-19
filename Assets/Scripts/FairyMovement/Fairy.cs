using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using FMODUnity;

[RequireComponent(typeof(NavMeshAgent))]
public class Fairy : MonoBehaviour
{
    public enum FairyState { Alive, Petrified };
    public FairyState fairyState;

    public bool isSelected;
    public bool canMove = true;
    public float speed = 3.5f;
    public float teamUpDistance = 2f;
    private Collider collider;
    private Renderer rendererSphere;
    private Renderer rendererBody;
    private Material initialMaterial;
    private Material initialMaterialBody;
    public Material petrifiedMaterial;
    public Material petrifiedMaterialBody;
    public GameObject selectedIndicator;
    public GameObject energyContainer;
    public GameObject energyBar;

    public GameObject eye;
    public GameObject eye2;


    bool playIntro = false;
    bool playOutro = true;
    private Vector3 startPosition;
    private Vector3 targetPosition;
    float t;
    public float timeToReachTarget = 0.1f;

    [HideInInspector] public LineRenderer lineRenderer;

    [SerializeField] private float maxPathLength = 5f;
    [HideInInspector] public float pathLengthLeft;

    private NavMeshPath path;

    [EventRef]
    public string setDestinationSound;

    [EventRef]
    public string reviveSound;

    [EventRef]
    public string freezeSound;

    NavMeshAgent agent;

    private List<GameObject> otherFairies = new List<GameObject>();

    public delegate void PathIsSave();
    public static event PathIsSave OnFairyDiesOnPath;
    public static event PathIsSave OnFairySecurePath;


    private void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        startPosition = new Vector3(transform.position.x, 50f, transform.position.z);
        targetPosition = transform.position;
        t = 0;
        selectedIndicator.SetActive(false);
        energyContainer.SetActive(false);
        GetOtherFairies();
        rendererSphere = transform.Find("FairyBody").Find("Sphere").GetComponent<Renderer>();
        rendererBody = transform.Find("FairyBody").GetComponent<Renderer>();
        initialMaterial = rendererSphere.material;
        initialMaterialBody = rendererBody.material;
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
        collider = GetComponent<Collider>();
        path = new NavMeshPath();
        pathLengthLeft = maxPathLength;
        agent = GetComponent<NavMeshAgent>();
        agent.updatePosition = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.gameState == GameManager.GameState.PlayerTurn)
        {
            if (fairyState == FairyState.Alive)
            {
                energyContainer.SetActive(true);
            }
        }
        else
        {
            energyContainer.SetActive(false);
        }

        switch (fairyState)
        {
            case FairyState.Alive:
                if (isSelected && !Input.GetMouseButton(2))
                    lineRenderer.enabled = true;
                else
                    lineRenderer.enabled = false;
                break;
            case FairyState.Petrified:
                LookForHelp();
                break;
        }


        switch (GameManager.Instance.gameState)
        {
            case GameManager.GameState.Intro:

                //Intro Animation
                if (playIntro)
                {
                    t += Time.deltaTime / timeToReachTarget;
                    transform.position = Vector3.Lerp(startPosition, targetPosition, t);
                }
                else
                {
                    transform.position = startPosition;
                }
                break;

            case GameManager.GameState.PlayerTurn:

                break;

            case GameManager.GameState.Outro:
                t += Time.deltaTime / timeToReachTarget;
                transform.position = Vector3.Lerp(startPosition, targetPosition, t);
                break;
        }

        UpdateEnergyBar();
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
        if (GameManager.Instance.gameState != GameManager.GameState.Outro ||
           GameManager.Instance.gameState != GameManager.GameState.Intro ||
           GameManager.Instance.gameState != GameManager.GameState.PreLevel)
        {
            collider.enabled = false;
            rendererSphere.material = petrifiedMaterial;
            rendererBody.material = petrifiedMaterialBody;
            fairyState = FairyState.Petrified;
            CalculatePath(transform.position);
            SetPath();
            agent.speed = 0;
            GameManager.Instance.CheckForGameOver();
            eye.SetActive(false);
            eye2.SetActive(false);
        }
    }

    public void Revive()
    {
        collider.enabled = true;
        rendererSphere.material = initialMaterial;
        rendererBody.material = initialMaterialBody;
        fairyState = FairyState.Alive;
        agent.speed = speed;
        eye.SetActive(true);
        eye2.SetActive(true);
        FMODUnity.RuntimeManager.PlayOneShot(reviveSound);
    }

    private void LookForHelp()
    {
        foreach (GameObject otherFairy in otherFairies)
        {
            float distance = Vector3.Distance(transform.position, otherFairy.transform.position);

            if (distance < teamUpDistance &&
                otherFairy.GetComponent<Fairy>().fairyState != Fairy.FairyState.Petrified
                && !GhostController.Instance.lightRaycaster.CurrentlyHitByLight(this.transform))
            {
                Debug.Log("reviving");
                Revive();
            }
        }
    }

    public float CalculatePath(Vector3 targetPosition)
    {
        NavMesh.CalculatePath(transform.position, targetPosition, NavMesh.AllAreas, path);

        bool willDie = false;

        if (path != null && path.corners.Length > 1)
        {
            lineRenderer.positionCount = path.corners.Length;
            for (int i = 0; i < path.corners.Length; i++)
            {
                lineRenderer.SetPosition(i, path.corners[i] + new Vector3(0, transform.position.y, 0));
            }

            //Check if Route is save and set Icon
            //for (int i = 0; i < path.corners.Length-1; i++)
            //{                
            //    for (float j = 0; j <= 1; j += 0.05f)
            //    {
            //        Vector3 lerpedPosition = Vector3.Lerp(path.corners[i], path.corners[i+1], j);
            //        if (!FairyMovementController.Instance.IsPositionSave(lerpedPosition))
            //        {
            //            UIController.Instance.diePositionIcon.transform.position = lerpedPosition;
            //            UIController.Instance.diePositionIcon.SetActive(true);
            //            WillDieOnThisPath = true;
            //            break;
            //        }
            //        else
            //        {
            //            UIController.Instance.diePositionIcon.SetActive(false);
            //            WillDieOnThisPath = false;
            //        }
            //    }
            //}

            bool pathInsecure = false;

            for (int i = path.corners.Length - 1; i > 0; i--)
            {
                for (float j = 0; j <= 1; j += 0.05f)
                {
                    Vector3 lerpedPosition = Vector3.Lerp(path.corners[i - 1], path.corners[i], j);
                    if (FairyMovementController.Instance.PositionIsSave(lerpedPosition))
                    {
                        if (!pathInsecure)
                            OnFairySecurePath();
                    }
                    else
                    {
                        UIController.Instance.diePositionIcon.transform.position = lerpedPosition;

                        OnFairyDiesOnPath();
                        pathInsecure = true;
                        break;
                    }
                }
            }
        }

        lineRenderer.SetPositions(path.corners);
        float length = PathCalculations.PathLength(path);
        return length;
    }


    void UpdateEnergyBar()
    {
        float pathLengthPercent = pathLengthLeft / maxPathLength;
        energyBar.transform.localScale = new Vector3(Mathf.Lerp(energyBar.transform.localScale.x, pathLengthPercent, Time.deltaTime * 3), 1, 1);
    }


    public void SetPath()
    {
        agent.SetPath(path);
        pathLengthLeft -= PathCalculations.PathLength(agent.path);
        if (MovementLeft())
            pathLengthLeft = 0;
        isSelected = false;
        selectedIndicator.SetActive(false);
        if (fairyState != FairyState.Petrified)
        {
            FMODUnity.RuntimeManager.PlayOneShot(setDestinationSound);
        }
    }

    public bool MovementLeft()
    {
        if (pathLengthLeft < 0.35f)
        {
            return true;
        }
        else
            return false;
    }

    public void ResetFairy()
    {
        //Debug.Log("Reset Fairy " + gameObject.name);
        pathLengthLeft = maxPathLength;
    }

    public float GetAverageDistanceToAllFairies()
    {
        float sum = 0;
        foreach (GameObject fairy in otherFairies)
        {
            sum += Vector3.Distance(transform.position, fairy.transform.position);
        }

        return sum / otherFairies.Count;
    }

    public void PlayIntroAnimation()
    {
        t = 0;
        playIntro = true;
    }

    public void PlayOutroAnimation()
    {
        collider.enabled = false;
        t = 0;
        startPosition = transform.position;
        targetPosition = new Vector3(transform.position.x, transform.position.y + 50, transform.position.z);

        playOutro = true;
    }
}
