using UnityEngine;
using System.Collections.Generic;

public class UnitManager : MonoBehaviour
{
    public List<GameObject> spawnPoints = new List<GameObject>();
    public float spawnRadius;
    public List<GameObject> units = new List<GameObject>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnUnits()
    {
        /*
        foreach (GameObject unit in units) {
            
        }
        */
        GameObject randomUnit = units[Random.Range(0, units.Count)];
        GameObject randomSpawnPoint = spawnPoints[Random.Range(0, units.Count)];

        Vector2 randomPoint = Random.insideUnitCircle * spawnRadius;
        Vector3 spawnPos = randomSpawnPoint.transform.position + new Vector3(randomPoint.x, 0, randomPoint.y);

        Instantiate(randomUnit, spawnPos, Quaternion.identity);
    }
}
