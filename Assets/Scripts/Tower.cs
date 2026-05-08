using UnityEngine;

public class Tower : MonoBehaviour
{
    /*
    velocity
    angle
    height
    */
    
    public float velocity;
    public float angle;
    public float height;

    public float mass;

    public float rotation;
    public Transform barrel;
    public Transform barrelPivot;

    public float rotOffset;
    public GameObject projectileObject;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        TowerUI.Instance.Open(this);
    }

    void Awake()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation=Quaternion.Euler(0,rotation,0);
        barrelPivot.localRotation=Quaternion.Euler(angle + rotOffset,0,0);
    }

    public void Shoot()
    {
        GameObject projectile = Instantiate(projectileObject, barrel.position, Quaternion.identity);

        // Hand the path off to the unit
        Projectile projectileMovement = projectile.GetComponent<Projectile>();
        if (projectileMovement != null)
            projectileMovement.ShootProjectile(velocity, angle, mass, barrel.forward);
    }
}
