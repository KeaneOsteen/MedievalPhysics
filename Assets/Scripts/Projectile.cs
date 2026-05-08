using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour
{
    private Rigidbody rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        

    }

    public void ShootProjectile(float vel, float ang, float mass, Vector3 dir)
    {
        StartCoroutine(FollowProjectilePath(vel, ang, mass, dir));
    }

    private IEnumerator FollowProjectilePath(float vel, float ang, float mass, Vector3 dir)
    {
        
        rb.linearVelocity = dir * vel;

        yield return null;

    }
}
