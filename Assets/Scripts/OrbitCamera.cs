using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitCamera : MonoBehaviour
{
    public Transform target;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(2))
        {
            Cursor.visible = false;
            //transform.RotateAround(target.position, transform.right, -Input.GetAxis("Mouse Y") * 5);
            transform.RotateAround(target.position, target.transform.up, -Input.GetAxis("Mouse X") * 5);
        } else
        {
            Cursor.visible = true;
        }

    }
}
