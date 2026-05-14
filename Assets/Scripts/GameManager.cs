using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public Button playMoveButton;
    public Button equationsButton;
    public Button tutorialButton;
    public RectTransform equationsPanel;
    public RectTransform tutorialPanel;
    public UnitManager unitManager;
    public float health =100f;

    private float round=0f;

    void Start()
    {
        playMoveButton.onClick.AddListener(playMove);
        equationsButton.onClick.AddListener(HandleEquationSheet);
        tutorialButton.onClick.AddListener(HandleTutorialSheet);
    }

    void Update() { }

    void playMove()
    {

        GameObject[] towers = GameObject.FindGameObjectsWithTag("Tower");

        foreach (GameObject tower in towers)
        {
            tower.GetComponent<Tower>().Shoot();
        }
    }

    public void playEnemyMove()
{
    unitManager.SpawnUnits();
    StartCoroutine(ChargeAfterSpawn());
}

private IEnumerator ChargeAfterSpawn()
{
    // Wait one frame for all spawned units to run Awake/Start
    yield return null;

    UnitMovement[] units = FindObjectsByType<UnitMovement>(FindObjectsSortMode.None);
    Debug.Log($"Found {units.Length} UnitMovement objects");

    foreach (UnitMovement unit in units)
    {
        unit.Charge();
    }
}

    void HandleEquationSheet()
{
    bool isActive = equationsPanel.gameObject.activeSelf;
    equationsPanel.gameObject.SetActive(!isActive);
}
    void OpenEquations()
    {
        equationsPanel.gameObject.SetActive(true);
    }

    void CloseEquations()
    {
        equationsPanel.gameObject.SetActive(false);
    }

    public void TakeDamage(float dmg)
    {
        health -= dmg;
    }

    void HandleTutorialSheet()
    {
        bool isActive = tutorialPanel.gameObject.activeSelf;
        tutorialPanel.gameObject.SetActive(!isActive);
    }

}