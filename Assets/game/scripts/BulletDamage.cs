using UnityEngine;

public class BulletDamage : MonoBehaviour
{
    [Tooltip("Destroy bullet on hit")]
    public bool destroyOnHit = true;

    void Awake()
    {
        // Make sure this collider is a trigger so it doesn’t physically block Mario
        var col = GetComponent<Collider2D>();
        col.isTrigger = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        // Tell the GameManager that Mario got damaged:
        GameManager.Instance.PlayerDamaged();

        if (destroyOnHit)
            Destroy(gameObject);
    }
}
