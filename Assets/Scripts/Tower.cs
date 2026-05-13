using UnityEngine;
using System.Collections;

public class Tower : MonoBehaviour
{
    [Header("Projectile")]
    public float velocity;
    public float angle;
    public float height;
    public float mass;

    [Header("Tower Rotation")]
    public bool followProjectile;
    public float rotation;
    public float rotOffset;

    [Header("References")]
    public Transform barrel;
    public Transform barrelPivot;

    public GameObject projectileObject;
    public GameObject fireParticle;

    public CameraController anchor;

    [Header("Laser")]
    public LineRenderer laser;
    public float laserLength = 500f;

    void Start()
    {
        TowerUI.Instance.Open(this);

        if (laser != null)
        {
            laser.positionCount = 2;
        }

        laser.startWidth = 0.05f;
        laser.endWidth = 0.05f;
    }

    void Awake()
    {

    }

    void Update()
    {
        // Rotate tower horizontally
        transform.rotation = Quaternion.Euler(0, rotation, 0);

        // Rotate barrel vertically
        barrelPivot.localRotation = Quaternion.Euler(angle + rotOffset, 0, 0);

        // Draw aiming laser
        DrawLaser();
    }

    private void DrawLaser()
    {
        if (laser == null || barrel == null)
            return;

        Vector3 start = barrel.position;
        Vector3 end;

        RaycastHit hit;

        if (Physics.Raycast(start, barrel.forward, out hit, laserLength))
        {
            end = hit.point;
        }
        else
        {
            end = start + barrel.forward * laserLength;
        }

        laser.SetPosition(0, start);
        laser.SetPosition(1, end);
    }

    public void Shoot()
    {
        StartCoroutine(ShootSequence());
    }

    private IEnumerator ShootSequence()
    {
        // Fire effect
        Instantiate(fireParticle, barrel.position, Quaternion.identity);

        yield return new WaitForSeconds(0.05f);

        // Spawn projectile
        GameObject projectile = Instantiate(
            projectileObject,
            barrel.position,
            Quaternion.identity
        );

        // Camera follow
        if (followProjectile)
        {
            anchor.SwitchToFollow();
            anchor.target = projectile.transform;
        }

        // Shoot projectile
        Projectile projectileMovement = projectile.GetComponent<Projectile>();

        if (projectileMovement != null)
        {
            projectileMovement.ShootProjectile(
                velocity,
                angle,
                mass,
                barrel.forward
            );
        }
    }
}