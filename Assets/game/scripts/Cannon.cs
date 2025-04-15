using UnityEngine;

public class Cannon : MonoBehaviour
{
   [Header("Bullet Settings")]
    public GameObject bulletPrefab;      // must have a Rigidbody2D + SpriteRenderer
    public Transform firePoint;          // child at muzzle, flipped in X
    public float bulletSpeed = 5f;

    [Header("Target")]
    public Transform target;             // e.g. Mario

    [Header("Firing Interval")]
    public float minFireInterval = 1f;
    public float maxFireInterval = 3f;

    float fireTimer;
    Vector3 baseFireLocalPos;

    void Start()
    {
        if (bulletPrefab==null || firePoint==null)
            Debug.LogError("Cannon missing bulletPrefab or firePoint!", this);

        // target may be null in the Inspector, so check that later
        baseFireLocalPos = firePoint.localPosition;
        ResetFireTimer();
    }

    void Update()
    {
        // If there's no valid target, do nothing
        if (target == null) return;

        // Only tick down if our muzzle is on screen
        if (!IsInView(firePoint.position)) return;

        fireTimer -= Time.deltaTime;
        if (fireTimer <= 0f)
        {
            Fire();
            ResetFireTimer();
        }
    }

    void ResetFireTimer()
    {
        float a = Mathf.Min(minFireInterval, maxFireInterval);
        float b = Mathf.Max(minFireInterval, maxFireInterval);
        fireTimer = Random.Range(a, b);
    }

    void Fire()
    {
        // Guard against missing target
        if (target == null) return;

        // 1) Flip the firePoint to face Mario horizontally
        float sign = (target.position.x > transform.position.x) ? 1f : -1f;
        firePoint.localPosition = new Vector3(
            Mathf.Abs(baseFireLocalPos.x) * sign,
            baseFireLocalPos.y,
            baseFireLocalPos.z
        );

        // 2) Determine bullet direction from the firePoint offset
        float dirX = Mathf.Sign(firePoint.localPosition.x);
        Vector2 dir = new Vector2(dirX, 0f);

        // 3) Spawn the bullet
        var bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

        // 4) Physics
        var rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.gravityScale = 0f;
            rb.linearVelocity     = dir * bulletSpeed;
            rb.constraints  = RigidbodyConstraints2D.FreezeRotation;
        }
        else Debug.LogWarning("Cannon: bullet prefab missing Rigidbody2D", bullet);

        // 5) Flip the bullet's sprite when firing right
        var sr = bullet.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.flipX = dirX > 0f;
        }
    }

    bool IsInView(Vector2 pos)
    {
        var vp = Camera.main.WorldToViewportPoint(pos);
        return vp.z > 0 && vp.x >= 0 && vp.x <= 1 && vp.y >= 0 && vp.y <= 1;
    }

    void OnDrawGizmos()
    {
        if (firePoint != null && target != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(firePoint.position, 0.1f);
            float sign = (target.position.x > transform.position.x) ? 1f : -1f;
            Gizmos.DrawLine(firePoint.position, firePoint.position + Vector3.right * sign * 0.5f);
        }
    }
}
