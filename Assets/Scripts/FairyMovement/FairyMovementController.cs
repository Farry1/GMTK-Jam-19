using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FairyMovementController : MonoBehaviour
{
    Fairy selectedFairyMovement;

    List<Fairy> allFairies = new List<Fairy>();

    public Material lineDefault;
    public Material lineFail;

    public RaycastHit mouseHit;

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
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out mouseHit, 100))
        {
            if (selectedFairyMovement != null &&
                selectedFairyMovement.fairyState != Fairy.FairyState.Petrified &&
                GameManager.Instance.gameState == GameManager.GameState.PlayerTurn)
            {
                if (selectedFairyMovement.CalculatePath(mouseHit.point) > selectedFairyMovement.pathLengthLeft)
                {
                    //Debug.Log("Can't Move");
                    selectedFairyMovement.canMove = false;
                    selectedFairyMovement.lineRenderer.material = lineFail;
                }
                else
                {
                    //Debug.Log("Can Move");
                    selectedFairyMovement.canMove = true;
                    selectedFairyMovement.lineRenderer.material = lineDefault;
                }
            }


            if (Input.GetMouseButtonDown(0) &&
                GameManager.Instance.gameState == GameManager.GameState.PlayerTurn)
            {
                if (mouseHit.collider.tag == "Player")
                {
                    if (selectedFairyMovement != null)
                    {
                        selectedFairyMovement.selectedIndicator.SetActive(false);
                        selectedFairyMovement.isSelected = false;
                    }

                    selectedFairyMovement = mouseHit.collider.gameObject.GetComponent<Fairy>();
                    selectedFairyMovement.isSelected = true;
                    selectedFairyMovement.selectedIndicator.SetActive(true);

                    if (selectedFairyMovement != null && selectedFairyMovement.fairyState == Fairy.FairyState.Petrified)
                    {
                        selectedFairyMovement.isSelected = false;
                        selectedFairyMovement.selectedIndicator.SetActive(false);
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
                    selectedFairyMovement.selectedIndicator.SetActive(false);
                    selectedFairyMovement = null;
                }
            }
        }
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
}
