using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightDetector : MonoBehaviour
{
    public bool isDetected;
    public float maxDetectionRange = 10f;
    Fairy fairy;
    LayerMask layerMask;

    // Start is called before the first frame update
    void Start()
    {
        fairy = GetComponent<Fairy>();
    }

    // Update is called once per frame
    void Update()
    {
        //CheckForLightHit();
    }

    /*
    void CheckForLightHit()
    {
        if (GameManager.Instance.gameState == GameManager.GameState.EnemyTurn ||
            GameManager.Instance.gameState == GameManager.GameState.PlayerTurn)
        {
            Vector3 direction = GhostController.Instance.transform.position - transform.position;
            RaycastHit hit;

            Ray ray = new Ray(transform.position, direction);

            if (Physics.Raycast(ray, out hit, maxDetectionRange))
            {
                if (hit.collider.tag == "Ghost")
                {
                    if (fairy.fairyState != Fairy.FairyState.Petrified)
                    {
                        FMODUnity.RuntimeManager.PlayOneShot(fairy.freezeSound);
                        fairy.Petrify();
                    }
                }
            }
        }
    }

    public bool CurrentlyHitByLight()
    {
        Vector3 direction = GhostController.Instance.transform.position - transform.position;
        RaycastHit hit;

        Ray ray = new Ray(transform.position, direction);

        if (Physics.Raycast(ray, out hit, maxDetectionRange))
        {
            if (hit.collider.tag == "Ghost")
            {
                return true;
            }
        }

        return false;
    }
    */
}
