using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FairyMovementController : MonoBehaviour
{
    Fairy selectedFairyMovement;

    List<Fairy> allFairies = new List<Fairy>();

    public Material lineDefault;
    public Material lineFail;

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
        RaycastHit hit;

        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100))
        {
            if (selectedFairyMovement != null && selectedFairyMovement.fairyState != Fairy.FairyState.Petrified)
            {
                if (selectedFairyMovement.CalculatePath(hit.point) > selectedFairyMovement.pathLengthLeft)
                {
                    //Debug.Log("Can't Move");
                    selectedFairyMovement.canMove = false;
                    selectedFairyMovement.lineRenderer.SetColors(Color.red, Color.red);
                    selectedFairyMovement.lineRenderer.material = lineFail;
                }
                else
                {
                    //Debug.Log("Can Move");
                    selectedFairyMovement.canMove = true;
                    selectedFairyMovement.lineRenderer.SetColors(Color.white, Color.white);
                    selectedFairyMovement.lineRenderer.material = lineDefault;
                }
            }


            if (Input.GetMouseButtonDown(0))
            {
                if (hit.collider.tag == "Player")
                {
                    if (selectedFairyMovement != null)
                        selectedFairyMovement.isSelected = false;


                    selectedFairyMovement = hit.collider.gameObject.GetComponent<Fairy>();
                    selectedFairyMovement.isSelected = true;

                    if (selectedFairyMovement != null && selectedFairyMovement.fairyState == Fairy.FairyState.Petrified)
                    {
                        selectedFairyMovement.isSelected = false;
                        selectedFairyMovement = null;
                    }

                }
                else
                {
                    if (selectedFairyMovement != null && selectedFairyMovement.canMove)
                    {
                        selectedFairyMovement.SetPath();
                        selectedFairyMovement = null;
                    }
                }
            }

            if (Input.GetMouseButtonDown(1))
            {
                if (selectedFairyMovement != null)
                {
                    selectedFairyMovement.isSelected = false;
                    selectedFairyMovement = null;
                }

            }
        }
    }

    void GetAllFairies()
    {
        GameObject[] fairiesObjects = GameObject.FindGameObjectsWithTag("Player");

        foreach(GameObject fairy in fairiesObjects)
        {
            allFairies.Add(fairy.GetComponent<Fairy>());
        }
    }

    public void ResetAllFairies()
    {
        foreach(Fairy fairy in allFairies)
        {
            fairy.ResetFairy();
        }
    }
}
