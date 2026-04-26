using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Transform t;
    public static bool inWater;
    public static bool isSwimming;

    // it just store the last swimming state to print only when state change
    bool lastSwimmingState;

    public LayerMask waterMask;

    // here added water surface height
    // this is the Y position of the water plane in the scene
    // pne example okay: if your water plane is at Y = 9 in here below code,
    // set this value to 9 in the Inspector in water plane position y = 9 okay.

    public float waterLevel = 15.97f;

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

    void Start()
    {
        t = this.transform;
        Cursor.lockState = CursorLockMode.Locked;
        inWater = false;
    }

    private void FixedUpdate()
    {
        SwimmingOrFloating();
        Move();
    }

    private void OnTriggerEnter(Collider other)
    {
        SwitchMovement();
    }

    private void OnTriggerExit(Collider other)
    {
        SwitchMovement();
    }

    void SwitchMovement()
    {
        inWater = !inWater;
    }

    void SwimmingOrFloating()
    {
        bool swimCheck = false;

        // here i have compared the player's Y position with the water level
        // if player position is BELOW the water surface then it is swimming
        // if player position is ABOVE the water surface then it is not swimming
        // simple but professional logical code written and this does not 
        // need any water collider to work as i have said there is no use of 
        // water collider in your project in class

        if (t.position.y < waterLevel)
        {
            swimCheck = true;
        }
        else
        {
            swimCheck = false;
        }

        if (inWater)
        {
            RaycastHit hit;

            if (Physics.Raycast(new Vector3(t.position.x, t.position.y + 0.5f, t.position.z), Vector3.down, out hit, Mathf.Infinity, waterMask))
            {
                if (hit.distance < 0.1f)
                {
                    swimCheck = true;
                }
            }
            else
            {
                swimCheck = true;
            }
        }

        isSwimming = swimCheck;


        // it just print if state change
        if (isSwimming != lastSwimmingState)
        {
            Debug.Log("isSwimming = " + isSwimming);
            lastSwimmingState = isSwimming;
        }
    }

    void Update()
    {
        LookAround();

        if (Input.GetKeyDown(KeyCode.Escape))
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