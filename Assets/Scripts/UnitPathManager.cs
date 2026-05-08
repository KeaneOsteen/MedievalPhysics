using UnityEngine;
using System.Collections.Generic;

public class UnitPathManager : MonoBehaviour
{
    public List<Transform> paths = new List<Transform>();
    public List<GameObject> units = new List<GameObject>();

    public float spawnRate = 2f;
    private float timer = 0;

    void Update()
    {/*
        timer += Time.deltaTime;
        if (timer >= spawnRate)
        {
            timer = 0;
            SpawnUnit();
        }
        */
    }

    public void SpawnUnit()
    {
        int randIndex = Random.Range(0, units.Count);
        GameObject unit = Instantiate(units[randIndex], paths[0].position, Quaternion.identity);

        // Hand the path off to the unit
        UnitMovement movement = unit.GetComponent<UnitMovement>();
        if (movement != null)
            movement.SetPath(paths);
    }
}