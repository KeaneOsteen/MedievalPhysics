using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class TowerUI : MonoBehaviour
{
    public static TowerUI Instance;

    [Header("Panel")]
    public RectTransform panel;
    public Camera mainCamera;

    [Header("Fields")]
    public TMP_InputField velocityField;
    public TMP_InputField angleField;
    public Slider massField;
    public Toggle followToggle;

    [Header("Drag Settings")]
    public float dragHoldTime = 0.2f;
    public LayerMask placementLayer;

    private Tower current;

    // Drag state
    private bool isDragging = false;
    private float mouseHeldTime = 0f;
    private GameObject dragTarget = null;

    public bool IsPanelOpen => panel.gameObject.activeSelf;

    void Awake()
    {
        Instance = this;
        panel.gameObject.SetActive(false);
    }

    void Update()
    {
        if (current != null && panel.gameObject.activeSelf)
        {
            Vector2 screenPos = mainCamera.WorldToScreenPoint(current.transform.position);
            panel.position = screenPos + new Vector2(120f, 0f);
        }

        HandleClick();
        HandleDrag();
    }

    private void HandleClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (IsClickingOnUI()) return;

            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, 1000f))
            {
                if (hit.collider.gameObject.CompareTag("Tower"))
                {
                    dragTarget = hit.collider.gameObject;
                    mouseHeldTime = 0f;
                    Open(hit.collider.gameObject.GetComponent<Tower>());
                    return;
                }
            }

            if (current != null && !isDragging)
                Close();
        }

        if (Input.GetMouseButton(0) && dragTarget != null && !isDragging)
        {
            mouseHeldTime += Time.deltaTime;

            if (mouseHeldTime >= dragHoldTime)
            {
                isDragging = true;
                panel.gameObject.SetActive(false);
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (isDragging)
                PlaceObject();

            isDragging = false;
            dragTarget = null;
            mouseHeldTime = 0f;
        }
    }

    private void HandleDrag()
    {
        if (!isDragging || dragTarget == null) return;

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 1000f, placementLayer))
        {
            dragTarget.transform.position = hit.point;
        }
    }

    private void PlaceObject()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 1000f, placementLayer))
        {
            dragTarget.transform.position = hit.point;
        }
    }

    private bool IsClickingOnUI()
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        return results.Count > 0;
    }

    public void Open(Tower obj)
    {
        current = obj;

        velocityField.text = obj.velocity.ToString();
        angleField.text    = obj.angle.ToString();
        massField.value    = obj.rotation;
        followToggle.isOn  = obj.followProjectile;

        massField.onValueChanged.RemoveAllListeners();
        massField.onValueChanged.AddListener(_ => Apply());

        followToggle.onValueChanged.RemoveAllListeners();
        followToggle.onValueChanged.AddListener(_ => Apply());

        panel.gameObject.SetActive(true);
    }

    public void Close()
    {
        Apply();
        current = null;
        panel.gameObject.SetActive(false);
    }

    public void Apply()
    {
        if (current == null) return;

        if (float.TryParse(velocityField.text, out float v)) current.velocity = v;
        if (float.TryParse(angleField.text,    out float a)) current.angle    = a;
        current.rotation = massField.value;
        current.followProjectile = followToggle.isOn;
    }
}