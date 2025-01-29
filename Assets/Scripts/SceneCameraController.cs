using UnityEngine;

public class SceneViewCamera : MonoBehaviour
{
    public float moveSpeed = 10f;        // Speed of camera movement
    public float fastSpeedMultiplier = 2f; // Multiplier when holding Shift
    public float mouseSensitivity = 2f; // Mouse sensitivity for rotation
    public float zoomSpeed = 10f;       // Speed of zooming with the mouse wheel
    public float panSpeed = 0.5f;

    private float rotationX;            // Horizontal rotation
    private float rotationY;            // Vertical rotation

    void Start()
    {
        // Initialize rotation to match the starting camera orientation
        Vector3 startRotation = transform.eulerAngles;
        rotationX = startRotation.y; // Horizontal rotation comes from Y
        rotationY = startRotation.x; // Vertical rotation comes from X
    }

    void Update()
    {
        // Right-click to activate camera controls
        if (Input.GetMouseButton(1)) // Right mouse button
        {
            // Mouse Look
            rotationX += Input.GetAxis("Mouse X") * mouseSensitivity;
            rotationY -= Input.GetAxis("Mouse Y") * mouseSensitivity;
            rotationY = Mathf.Clamp(rotationY, -90f, 90f); // Prevent flipping
            transform.localEulerAngles = new Vector3(rotationY, rotationX, 0);

            // Movement
            float speed = moveSpeed * (Input.GetKey(KeyCode.LeftShift) ? fastSpeedMultiplier : 1f);
            float moveX = Input.GetAxis("Horizontal"); // A/D or Left/Right
            float moveZ = Input.GetAxis("Vertical");   // W/S or Up/Down
            float moveY = 0f;

            if (Input.GetKey(KeyCode.E)) moveY += 1f;  // Move up
            if (Input.GetKey(KeyCode.Q)) moveY -= 1f;  // Move down

            Vector3 move = transform.right * moveX + transform.forward * moveZ + transform.up * moveY;
            transform.position += move * speed * Time.deltaTime;
        }
        // Middle Mouse Button for Panning
        if (Input.GetMouseButton(2)) // Middle mouse button
        {
            float panX = -Input.GetAxis("Mouse X") * panSpeed; // Invert X for intuitive movement
            float panY = -Input.GetAxis("Mouse Y") * panSpeed; // Invert Y for intuitive movement
            transform.position += transform.right * panX + transform.up * panY;
        }

        // Mouse Wheel Zoom
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        transform.position += transform.forward * scroll * zoomSpeed * Time.deltaTime;
    }
}
