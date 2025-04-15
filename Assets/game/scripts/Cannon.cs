using UnityEngine;

public class Cannon : MonoBehaviour
{
    [Header("Bullet Settings")]
    [Tooltip("The bullet prefab to fire (must have a Rigidbody2D)")]
    public GameObject bulletPrefab;
    [Tooltip("Local point (child transform) where bullets appear")]
    public Transform firePoint;
    [Tooltip("Initial speed of the bullet")]
    public float bulletSpeed = 5f;
    [Tooltip("Direction in which to shoot (e.g. (0,1) = straight up)")]
    public Vector2 shootDirection = Vector2.up;

    [Header("Firing Rate")]
    [Tooltip("Seconds between shots")]
    public float fireInterval = 2f;

    private float fireTimer;

    void Start()
    {
        if (bulletPrefab == null || firePoint == null)
            Debug.LogError("Cannon: Assign both bulletPrefab and firePoint!", this);

        fireTimer = fireInterval;
    }

    void Update()
    {
        fireTimer -= Time.deltaTime;
        if (fireTimer <= 0f)
        {
            Fire();
            fireTimer = fireInterval;
        }
    }

    private void Fire()
    {
        // Instantiate bullet
        GameObject bullet = Instantiate(
            bulletPrefab,
            firePoint.position,
            Quaternion.identity
        );

        // Give it a Rigidbody2D velocity and zero gravity
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.gravityScale = 0f;  // disable gravity
            rb.linearVelocity     = shootDirection.normalized * bulletSpeed;
            rb.constraints  = RigidbodyConstraints2D.FreezeRotation;
        }
        else
        {
            Debug.LogWarning("Cannon: Bullet prefab has no Rigidbody2D!", bullet);
        }
    }

    // Draw a little gizmo so you can see the fire point & direction in Scene view
    void OnDrawGizmos()
    {
        if (firePoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(firePoint.position, 0.1f);
            Gizmos.DrawLine(
                firePoint.position,
                firePoint.position + (Vector3)shootDirection.normalized
            );
        }
    }
}
