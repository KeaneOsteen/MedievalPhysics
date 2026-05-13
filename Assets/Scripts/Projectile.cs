using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour
{
    private Rigidbody rb;

    [Header("Hit Effects")]
    public GameObject waterHitEffect;
    public GameObject groundHitEffect;

    private int waterLayer;
    private int groundLayer;

    private GameManager game;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        waterLayer = LayerMask.NameToLayer("water");
        groundLayer = LayerMask.NameToLayer("ground");

        game = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
    }

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

    private void OnCollisionEnter(Collision collision)
    {
        int hitLayer = collision.gameObject.layer;

        if (hitLayer == waterLayer)
        {
            SpawnEffect(waterHitEffect, collision);
        }
        else if (hitLayer == groundLayer)
        {
            SpawnEffect(groundHitEffect, collision);
        }
    }

    private void SpawnEffect(GameObject effectPrefab, Collision collision)
    {
        if (effectPrefab == null) return;

        ContactPoint contact = collision.GetContact(0);
        GameObject instance = Instantiate(effectPrefab, contact.point, Quaternion.identity);
        Debug.Log("spawned particle");
        Destroy(instance, 1f);

        CameraController camControl = Camera.main.GetComponent<CameraController>();
        if(camControl.target != camControl.defaultTarget)
        {
            Camera.main.GetComponent<CameraController>().SwitchToIndependent();
        }


        game.playEnemyMove();
        Destroy(gameObject);
    }
}