using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public Button playMoveButton;
    public UnitManager unitManager;

    private float round=0f;

    void Start()
    {
        playMoveButton.onClick.AddListener(playMove);
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
}