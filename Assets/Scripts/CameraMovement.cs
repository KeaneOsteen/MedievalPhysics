using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Rotation")]
    public Transform target;
    public float rotationSpeed = 100f;

    [Header("Zoom")]
    public float zoomSpeed = 5f;
    public float minZoom = 5f;
    public float maxZoom = 50f;

    private float currentZoom = 20f;
    private float yaw = 0f;
    private float pitch = 45f;

    void Start()
    {
        // If no target assigned, default to the center of the map
        if (target == null)
        {
            GameObject center = new GameObject("MapCenter");
            center.transform.position = Vector3.zero;
            target = center.transform;
        }

        currentZoom = Vector3.Distance(transform.position, target.position);
    }

    void Update()
    {
        HandleRotation();
        HandleZoom();
        UpdateCameraPosition();
    }

    private void HandleRotation()
    {
        // Hold right mouse button to rotate
        if (Input.GetMouseButton(0))
        {
            yaw += Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
            pitch -= Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;
            pitch = Mathf.Clamp(pitch, 10f, 80f); // Prevent flipping over
        }
    }

    private void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        currentZoom -= scroll * zoomSpeed;
        currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);
    }

    private void UpdateCameraPosition()
    {
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);
        transform.position = target.position + rotation * new Vector3(0f, 0f, -currentZoom);
        transform.LookAt(target.position);
    }
}