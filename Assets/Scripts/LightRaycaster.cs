﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightRaycaster : MonoBehaviour
{
    private Transform light;
    public float maxDetectionRange = 6;

    List<Fairy> fairies = new List<Fairy>();

    void Start()
    {
        fairies = FairyMovementController.Instance.allFairies;
        light = GameObject.Find("Point Light").transform;
    }


    void Update()
    {
        if (GameManager.Instance.gameState == GameManager.GameState.EnemyTurn ||
            GameManager.Instance.gameState == GameManager.GameState.PlayerTurn)
        {
            foreach (Fairy fairy in fairies)
            {
                Vector3 direction = fairy.transform.position - light.transform.position;
                RaycastHit hit;

                Ray ray = new Ray(transform.position, direction);
                if (Physics.SphereCast(ray, 0.2f, out hit, maxDetectionRange))
                {
                    Debug.Log(hit.collider.name);

                    if (hit.collider.tag == "Player")
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
    }

    public bool CurrentlyHitByLight(Transform fairy)
    {
        Vector3 direction = fairy.transform.position - light.transform.position;
        RaycastHit hit;

        Ray ray = new Ray(light.transform.position, direction);

        if (Physics.SphereCast(ray, 0.25f, out hit, maxDetectionRange))
        {
            if (hit.collider.tag == "Player")
            {
                return true;
            }
        }

        return false;
    }
}
