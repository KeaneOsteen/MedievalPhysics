    using UnityEngine;
using UnityEngine.UI; // Required for Button component

public class GameManager : MonoBehaviour
{
    public Button playMoveButton;
    public UnitPathManager unitManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playMoveButton.onClick.AddListener(playMove);
        //unitManager.SpawnUnit();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void playMove()
    {
        unitManager.SpawnUnit();

        GameObject[] towers = GameObject.FindGameObjectsWithTag("Tower");
        UnitMovement[] units = FindObjectsByType<UnitMovement>(FindObjectsSortMode.None);

        foreach (GameObject tower in towers) {
            tower.GetComponent<Tower>().Shoot();
        }

        foreach (UnitMovement unit in units)
        {
            unit.MoveToNextWaypoint();
        }

    }
}
