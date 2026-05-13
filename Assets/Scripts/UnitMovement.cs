using UnityEngine;

public class UnitMovement : MonoBehaviour
{
    public int currentCharge;
    public int charge;

    [Header("Projectile")]
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float launchAngle = 45f;
    public Transform target;

    [Header("Rotation")]
    public float rotationOffset = 0f;

    void Start()
    {
        target = GameObject.FindWithTag("Home").transform;
    }

    void Awake()
    {
        currentCharge = 0;
    }

    void Update()
    {
        if (target == null) return;

        Vector3 lookDir = target.position - transform.position;
        lookDir.y = 0f;
        if (lookDir != Vector3.zero)
        {
            Quaternion targetRot = Quaternion.LookRotation(lookDir);
            transform.rotation = targetRot * Quaternion.Euler(0f, rotationOffset, 0f);
        }
    }

    public void Charge()
    {
        currentCharge++;
        if (currentCharge >= charge)
        {
            currentCharge = 0;
            Fire();
        }
    }

    private void Fire()
    {
        if (projectilePrefab == null || target == null || firePoint == null)
        {
            Debug.LogWarning("UnitMovement: Missing projectile prefab, fire point, or target.");
            return;
        }

        GameObject proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        EnemyProjectile ep = proj.GetComponent<EnemyProjectile>();

        if (ep != null)
            ep.ShootAt(target.position, firePoint.position, firePoint.forward, launchAngle, GetComponent<Collider>());
        else
            Debug.LogWarning("UnitMovement: Projectile prefab is missing EnemyProjectile component.");
    }
}