﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightDetector : MonoBehaviour
{
    public bool isDetected;
    public float maxDetectionRange = 10f;
    Fairy fairyMovement;
    LayerMask layerMask;

    // Start is called before the first frame update
    void Start()
    {
        fairyMovement = GetComponent<Fairy>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckForLightHit();
    }

    void CheckForLightHit()
    {
        Vector3 direction = GhostController.Instance.transform.position - transform.position;
        RaycastHit hit;

        Ray ray = new Ray(transform.position, direction);

        if (Physics.Raycast(ray, out hit, maxDetectionRange))
        {
            if (hit.collider.tag == "Ghost")
            {
                if(fairyMovement.fairyState != Fairy.FairyState.Petrified)
                {
                    FMODUnity.RuntimeManager.PlayOneShot(fairyMovement.freezeSound);
                    fairyMovement.Petrify();
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
}
