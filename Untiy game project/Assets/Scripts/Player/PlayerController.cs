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

    [Header("Player Movement")]
    public float speed = 1;
    float moveX;
    float moveY;
    float moveZ;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        t = this.transform;
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void FixedUpdate()
    {
        Move();
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
    void Move()
    {
        moveX = Input.GetAxis("Horizontal");
        moveY = Input.GetAxis("Vertical");
        moveZ = Input.GetAxis("Forward");

        t.Translate(new Vector3(moveX, 0, moveZ) * speed * Time.deltaTime);
        t.Translate(new Vector3(0, moveY, 0) * speed * Time.deltaTime, Space.World);
    }
}
