using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Rotation")]
    public Transform defaultTarget;
    public Transform target;
    public float rotationSpeed = 100f;

    [Header("Zoom")]
    public float zoomSpeed = 5f;
    public float minZoom = 5f;
    public float maxZoom = 50f;

    public float minZoomFollow;
    public float maxZoomFollow;

    private float currentZoom = 20f;
    
    private float currentMinZoom;
    private float currentMaxZoom;

    private float yaw = 0f;
    private float pitch = 45f;

    void Start()
    {
        if (target == null)
        {
            GameObject center = new GameObject("MapCenter");
            center.transform.position = Vector3.zero;
            target = center.transform;
        }

        SwitchToIndependent();
    }

    void Update()
    {
        if (TowerUI.Instance != null && TowerUI.Instance.IsPanelOpen) return;

        HandleRotation();
        HandleZoom();
        UpdateCameraPosition();
    }

    private void HandleRotation()
    {
        if (Input.GetMouseButton(0))
        {
            yaw += Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
            pitch -= Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;
            pitch = Mathf.Clamp(pitch, 10f, 80f);
        }
    }

    private void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        currentZoom -= scroll * zoomSpeed;
        currentZoom = Mathf.Clamp(currentZoom, currentMinZoom, currentMaxZoom);
    }

    private void UpdateCameraPosition()
    {
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);
        transform.position = target.position + rotation * new Vector3(0f, 0f, -currentZoom);
        transform.LookAt(target.position);
    }

    public void SwitchToFollow()
    {
        currentMaxZoom = maxZoomFollow;
        currentMinZoom = minZoomFollow;
        currentZoom = maxZoomFollow/2;
    }

    public void SwitchToIndependent()
    {
        target = defaultTarget;
        currentMaxZoom = maxZoom;
        currentMinZoom = minZoom;
        currentZoom = maxZoom/2;
    }
}