using UnityEngine;
using System.Collections.Generic;

public class UnitManager : MonoBehaviour
{
    public List<GameObject> spawnPoints = new List<GameObject>();
    public float spawnRadius;
    public List<GameObject> units = new List<GameObject>();
    public int maxSpawnAttempts = 10;

    void Start() { }
    void Update() { }

    public void SpawnUnits()
    {
        GameObject randomUnit = units[Random.Range(0, units.Count)];
        GameObject randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];

        Vector3? validSpawnPos = GetValidSpawnPosition(randomSpawnPoint.transform.position);

        if (validSpawnPos.HasValue)
        {
            Instantiate(randomUnit, new Vector3(validSpawnPos.Value.x, validSpawnPos.Value.y+10, validSpawnPos.Value.z), Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("Could not find a valid ground spawn position after max attempts.");
        }
    }

    private Vector3? GetValidSpawnPosition(Vector3 origin)
    {
        for (int i = 0; i < maxSpawnAttempts; i++)
        {
            Vector2 randomPoint = Random.insideUnitCircle * spawnRadius;
            Vector3 checkPos = origin + new Vector3(randomPoint.x, 10f, randomPoint.y);

            if (Physics.Raycast(checkPos, Vector3.down, out RaycastHit hit, 100f))
            {
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("ground"))
                {
                    return hit.point;
                }
                // if Water or nothing, loop and try again
            }
        }

        return null;
    }
}