using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    private Rigidbody rb;

    [Header("Hit Effects")]
    public GameObject waterHitEffect;
    public GameObject groundHitEffect;
    public GameObject explosionEffect;

    private int waterLayer;
    private int groundLayer;
    private int homeLayer;

    private GameManager game;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        waterLayer = LayerMask.NameToLayer("water");
        groundLayer = LayerMask.NameToLayer("ground");
        homeLayer = LayerMask.NameToLayer("home");
        game = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
    }

    public void ShootAt(Vector3 targetPosition, Vector3 firePointPosition, Vector3 shootDirection, float launchAngle, Collider shooterCollider)
    {
        Collider myCollider = GetComponent<Collider>();
        if (myCollider != null && shooterCollider != null)
            Physics.IgnoreCollision(myCollider, shooterCollider);

        Vector3 toTarget = targetPosition - firePointPosition;
        float horizontalDist = new Vector3(toTarget.x, 0f, toTarget.z).magnitude;
        float verticalDist = toTarget.y;

        float angleRad = launchAngle * Mathf.Deg2Rad;
        float g = Mathf.Abs(Physics.gravity.y);

        float denominator = 2f * Mathf.Cos(angleRad) * Mathf.Cos(angleRad)
                            * (horizontalDist * Mathf.Tan(angleRad) - verticalDist);

        if (denominator <= 0f)
        {
            Debug.LogWarning("EnemyProjectile: Invalid launch angle — can't reach target.");
            Destroy(gameObject);
            return;
        }

        float speed = Mathf.Sqrt((g * horizontalDist * horizontalDist) / denominator);
        Vector3 flatForward = new Vector3(shootDirection.x, 0f, shootDirection.z).normalized;
        Vector3 launchDir = flatForward * Mathf.Cos(angleRad) + Vector3.up * Mathf.Sin(angleRad);

        rb.linearVelocity = launchDir * (speed*.925f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        int hitLayer = collision.gameObject.layer;

        if (hitLayer == waterLayer)
            SpawnEffect(waterHitEffect, collision);
        else if (hitLayer == groundLayer)
            SpawnEffect(groundHitEffect, collision);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == homeLayer)
        {
            game.TakeDamage(25f);
            GameObject instance = Instantiate(explosionEffect, transform.position, Quaternion.identity);
            Destroy(instance, 1f);

            Destroy(gameObject);
        }
    }

    private void SpawnEffect(GameObject effectPrefab, Collision collision)
    {
        if (effectPrefab == null) return;

        ContactPoint contact = collision.GetContact(0);
        GameObject instance = Instantiate(effectPrefab, contact.point, Quaternion.identity);
        Destroy(instance, 1f);

        Destroy(gameObject);
    }
}