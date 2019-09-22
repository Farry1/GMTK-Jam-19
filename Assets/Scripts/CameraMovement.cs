using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform center;   
    public float moveSpeed = 50f;
    public float scrollSpeed = 10f;
    public float minY = 7f;
    public float maxRadius = 40;    

    
    void Update()
    {
        float distance = Vector3.Distance(transform.position, center.position);

        if (Input.GetMouseButton(2))
        {
            Cursor.visible = false;
            transform.RotateAround(center.position, center.transform.up, -Input.GetAxis("Mouse X") * 5);
            transform.RotateAround(center.position, center.transform.right, -Input.GetAxis("Mouse Y") * 5);
        }
        else
        {
            Cursor.visible = true;
        }

        Vector3 fakeForward = transform.forward;
        fakeForward.y = 0.0f;
        fakeForward.Normalize();

        Vector3 movement =
            Input.GetAxis("Horizontal") * transform.right * moveSpeed * Time.deltaTime +
            Input.GetAxis("Vertical") * fakeForward * moveSpeed * Time.deltaTime;

        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            transform.position += scrollSpeed * new Vector3(0, -Input.GetAxis("Mouse ScrollWheel"), 0);
        }

        transform.position += movement;
        Vector3 newLocation = transform.position;

        //Is Camera Inside Circle?
        if (distance > maxRadius)
        {
            Vector3 fromOriginToObject = newLocation - center.position; 
            fromOriginToObject *= maxRadius / distance; 
            newLocation = center.position + fromOriginToObject; 
            transform.position = newLocation;
        }

        //Is Camera Not too high?
        if (transform.position.y < minY)
        {
            transform.position = new Vector3(
                transform.position.x,
                minY,
                transform.position.z);
        }


        transform.LookAt(center.position);
    }



}
