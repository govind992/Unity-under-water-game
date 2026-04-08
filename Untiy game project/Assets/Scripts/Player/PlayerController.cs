using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
   Transform t;
[Header("Player Rotation")]
public float sensitivity = 1;


float rotationX;
float rotationY;


    void Start()
    {
       t =  this.transform;
       cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        LookAround();
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }

    void LookAround()
    {
        rotationX += Input.GetAxis("Mouse X") * sensitivity;
        rotationY += Input.GetAxis("Mouse Y") * sensitivity;

        

        t.localRotation = Quaternion.Euler(-rotationY, rotationX, 0);
    }
}