using UnityEngine;
using System.Collections;

public class Tower : MonoBehaviour
{
    public float velocity;
    public float angle;
    public float height;

    public float mass;

    public bool followProjectile;

    public float rotation;
    public Transform barrel;
    public Transform barrelPivot;

    public float rotOffset;
    public GameObject projectileObject;

    public GameObject fireParticle;

    public CameraController anchor;

    void Start()
    {
        TowerUI.Instance.Open(this);
    }

    void Awake() { }

    void Update()
    {
        transform.rotation = Quaternion.Euler(0, rotation, 0);
        barrelPivot.localRotation = Quaternion.Euler(angle + rotOffset, 0, 0);
    }

    public void Shoot()
{
    StartCoroutine(ShootSequence());
}

private IEnumerator ShootSequence()
{
    Instantiate(fireParticle, barrel.position, Quaternion.identity);
    yield return new WaitForSeconds(0.05f);

    GameObject projectile = Instantiate(projectileObject, barrel.position, Quaternion.identity);

    if(followProjectile)
    {
        anchor.SwitchToFollow();
        anchor.target = projectile.transform;
    }

    Projectile projectileMovement = projectile.GetComponent<Projectile>();
    if (projectileMovement != null)
        projectileMovement.ShootProjectile(velocity, angle, mass, barrel.forward);
}
}