﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FairyMovementController : MonoBehaviour
{
    Fairy selectedFairy;

    [HideInInspector] public List<Fairy> allFairies = new List<Fairy>();

    public Material lineDefault;
    public Material lineFail;

    public RaycastHit mouseHit;

    int notNavigableLayer = 11;

    private static FairyMovementController _instance;
    public static FairyMovementController Instance { get { return _instance; } }

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



    // Start is called before the first frame update
    void Start()
    {
        GetAllFairies();
    }

    // Update is called once per frame
    void Update()
    {
        if (NoFairyCanMove() && GameManager.Instance.gameState == GameManager.GameState.PlayerTurn)
        {
            UIController.Instance.nextTurnTooltip.SetActive(true);
        }
        else
        {
            UIController.Instance.nextTurnTooltip.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            SelectNextFairy();
        };

        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out mouseHit, Mathf.Infinity))
        {
            if (selectedFairy != null &&
                selectedFairy.fairyState != Fairy.FairyState.Petrified &&
                GameManager.Instance.gameState == GameManager.GameState.PlayerTurn)
            {

                if (!IsPositionSave(mouseHit.point))
                    MouseStateIndicator.Instance.SetMouseStateDanger();
                else
                    MouseStateIndicator.Instance.UnsetMouseState();


                if (selectedFairy.CalculatePath(mouseHit.point) > selectedFairy.pathLengthLeft)
                {
                    //Debug.Log("Can't Move");
                    selectedFairy.canMove = false;
                    selectedFairy.lineRenderer.material = lineFail;
                }
                else
                {
                    selectedFairy.canMove = true;
                    selectedFairy.lineRenderer.material = lineDefault;
                }
            }


            if (Input.GetMouseButtonDown(0) &&
                GameManager.Instance.gameState == GameManager.GameState.PlayerTurn)
            {



                if (mouseHit.collider.tag == "Player")
                {
                    if (selectedFairy != null)
                    {
                        selectedFairy.selectedIndicator.SetActive(false);
                        //selectedFairy.energyContainer.SetActive(false);
                        selectedFairy.isSelected = false;
                    }

                    selectedFairy = mouseHit.collider.gameObject.GetComponent<Fairy>();
                    selectedFairy.isSelected = true;
                    selectedFairy.selectedIndicator.SetActive(true);
                    //selectedFairy.energyContainer.SetActive(true);

                    if (selectedFairy != null && selectedFairy.fairyState == Fairy.FairyState.Petrified)
                    {
                        selectedFairy.isSelected = false;
                        selectedFairy.selectedIndicator.SetActive(false);
                        //selectedFairy.energyContainer.SetActive(false);
                        selectedFairy = null;
                    }
                }
                else
                {
                    if (selectedFairy != null && selectedFairy.canMove)
                    {
                        selectedFairy.SetPath();
                        selectedFairy = null;
                        MouseStateIndicator.Instance.UnsetMouseState();
                    }
                }
            }

            if (Input.GetMouseButtonDown(1))
            {
                if (selectedFairy != null)
                {
                    selectedFairy.isSelected = false;
                    selectedFairy.selectedIndicator.SetActive(false);
                    //selectedFairy.energyContainer.SetActive(false);
                    selectedFairy = null;
                    MouseStateIndicator.Instance.UnsetMouseState();
                }
            }
        }
    }


    void SelectNextFairy()
    {
        if (GameManager.Instance.gameState == GameManager.GameState.PlayerTurn)
            Debug.Log("Get Next");        

    }


    private bool IsPositionSave(Vector3 point)
    {
        point += new Vector3(0, 0.355f, 0);



        Vector3 direction = GhostController.Instance.light.transform.position - point;
        RaycastHit hit;

        Ray ray = new Ray(point, direction);

        Debug.DrawRay(ray.origin, ray.direction * 6, Color.red);

        if (Physics.SphereCast(ray, 0.2f, out hit, GhostController.Instance.lightRaycaster.maxDetectionRange))
        {
            if (hit.collider.tag == "Ghost_Center")
            {
                return false;
            }
        }
        return true;
    }

    public bool AllFairiesPetrified()
    {
        foreach (Fairy fairy in allFairies)
        {
            if (fairy.fairyState == Fairy.FairyState.Alive)
                return false;
        }

        return true;
    }

    public bool NoFairyPetrified()
    {
        foreach (Fairy fairy in allFairies)
        {
            if (fairy.fairyState == Fairy.FairyState.Petrified)
                return false;
        }

        return true;
    }

    public bool NoFairyCanMove()
    {
        foreach (Fairy fairy in allFairies)
        {
            if (!fairy.MovementLeft() && fairy.fairyState == Fairy.FairyState.Alive)
                return false;
        }
        return true;
    }

    void GetAllFairies()
    {
        GameObject[] fairiesObjects = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject fairy in fairiesObjects)
        {
            allFairies.Add(fairy.GetComponent<Fairy>());
        }
    }

    public void ResetAllFairies()
    {
        foreach (Fairy fairy in allFairies)
        {
            fairy.ResetFairy();
        }
    }

    public void DeactivateAllFairies()
    {
        foreach (Fairy fairy in allFairies)
        {
            fairy.gameObject.SetActive(false);
        }
    }

    public bool AllFairiesInTeamRange()
    {
        float sum = 0;

        foreach (Fairy fairy in allFairies)
        {
            sum += fairy.GetAverageDistanceToAllFairies();
        }

        sum = sum / allFairies.Count;

        if (sum <= allFairies[0].teamUpDistance && NoFairyPetrified())
            return true;
        else
            return false;
    }
}
