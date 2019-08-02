using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FairyMovementController : MonoBehaviour
{
    FairyMovement selectedFairyMovement;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;

        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100))
        {
            if (selectedFairyMovement != null)
            {
                if (selectedFairyMovement.CalculatePath(hit.point) > selectedFairyMovement.pathLengthLeft)
                {
                    Debug.Log("Can't Move");
                    selectedFairyMovement.canMove = false;
                    selectedFairyMovement.lineRenderer.SetColors(Color.red, Color.red);
                }
                else
                {
                    Debug.Log("Can Move");
                    selectedFairyMovement.canMove = true;
                    selectedFairyMovement.lineRenderer.SetColors(Color.white, Color.white);
                }
            }


            if (Input.GetMouseButtonDown(0))
            {
                if (hit.collider.tag == "Player")
                {
                    if (selectedFairyMovement != null)
                        selectedFairyMovement.isSelected = false;

                    selectedFairyMovement = hit.collider.gameObject.GetComponent<FairyMovement>();
                    selectedFairyMovement.isSelected = true;
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


}
