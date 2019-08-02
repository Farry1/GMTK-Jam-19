using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightDetector : MonoBehaviour
{
    public bool isDetected;
    LayerMask layerMask;

    // Start is called before the first frame update
    void Start()
    {

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
        Debug.DrawRay(transform.position, direction, Color.red);


        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            if (hit.collider.tag == "Ghost")
            {
                Debug.Log("hit");
            }
        }
    }
}
