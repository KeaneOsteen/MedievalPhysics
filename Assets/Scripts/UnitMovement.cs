using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitMovement : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float health;

    private List<Transform> _path;
    private int _currentWaypointIndex = 0;
    private bool _isMoving = false;
    private int _pendingMoves = 0; // tracks queued advances

    public void SetPath(List<Transform> path)
    {
        _path = path;
        _currentWaypointIndex = 0;
    }

    public void MoveToNextWaypoint()
    {
        if (_path == null || _currentWaypointIndex + _pendingMoves >= _path.Count) return;

        _pendingMoves++;

        if (!_isMoving)
            StartCoroutine(ProcessMoveQueue());
    }

    private IEnumerator ProcessMoveQueue()
    {
        _isMoving = true;

        while (_pendingMoves > 0)
        {
            if (_currentWaypointIndex >= _path.Count) break;

            Transform target = _path[_currentWaypointIndex];
            _currentWaypointIndex++;
            _pendingMoves--;

            while (Vector3.Distance(transform.position, target.position) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(
                    transform.position,
                    target.position,
                    moveSpeed * Time.deltaTime
                );
                yield return null;
            }

            transform.position = target.position;
        }

        _isMoving = false;

        if (_currentWaypointIndex >= _path.Count)
            Destroy(gameObject);
    }
}