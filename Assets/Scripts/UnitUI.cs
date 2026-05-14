using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class UnitUI : MonoBehaviour
{
    public static UnitUI Instance;

    [Header("Panel")]
    public RectTransform panel;
    public Camera mainCamera;

    [Header("Fields")]
    public TMP_Text distanceText;

    [Header("Cached References")]
    private GameObject tower;
    private Transform towerPoint;

    private UnitMovement current;

    void Awake()
    {
        Instance = this;
        panel = GameObject.Find("UnitPanel").GetComponent<RectTransform>();
        distanceText = GameObject.Find("DistanceText").GetComponent<TMP_Text>();
        mainCamera = Camera.main;
        tower = GameObject.FindWithTag("Tower");
        towerPoint = GameObject.FindWithTag("TowerPoint")?.transform;
        panel.gameObject.SetActive(false);
    }

    void Update()
    {
        if (current != null && panel.gameObject.activeSelf)
{
    Vector2 screenPos = mainCamera.WorldToScreenPoint(current.transform.position);
    panel.position = screenPos + new Vector2(120f, 0f);

    if (tower != null && towerPoint != null)
    {
        Vector3 currentFlat = new Vector3(current.transform.position.x, 0f, current.transform.position.z);
        Vector3 towerFlat = new Vector3(tower.transform.position.x, 0f, tower.transform.position.z);

        float dist = Vector3.Distance(currentFlat, towerFlat);
        float heightDiff = towerPoint.position.y - current.transform.position.y;
        distanceText.text = $"Distance: {dist:F2} | Height Diff: {heightDiff:F2}";
    }
    else
    {
        distanceText.text = "Distance: N/A";
    }
}

        if (Input.GetMouseButtonDown(0))
        {
            if (IsClickingOnUI()) return;

            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, 1000f))
            {
                if (hit.collider.gameObject.CompareTag("Enemy"))
                {
                    Open(hit.collider.gameObject.GetComponent<UnitMovement>());
                    return;
                }
            }

            if (current != null)
                Close();
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

    public void Open(UnitMovement obj)
    {
        current = obj;
        panel.gameObject.SetActive(true);
        tower.GetComponent<Tower>().rotateToward(current.gameObject.transform);
    }

    public void Close()
    {
        current = null;
        panel.gameObject.SetActive(false);
    }
}