using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCameraDisabledAtStart : MonoBehaviour
{
    void Start()
    {
        transform.LookAt(Camera.main.transform.position);
        transform.parent.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(Camera.main.transform.position);
    }
}
