using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Transform t;
    [Header("Player Rotation")]
    public float sensitivity = 1;

    public float rotationMin;
    public float rotationMax;
    float rotationX;
    float rotationY;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        t = this.transform;
        Cursor.lockState = CursorLockMode.Locked;
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
        rotationY = Mathf.Clamp(rotationY, rotationMin, rotationMax);
        t.localRotation = Quaternion.Euler(-rotationY, rotationX, 0);
    }
}
