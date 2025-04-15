using System;
using UnityEngine;

public class ShellMovement : MonoBehaviour
{
  
    public float slideSpeed = 5f;

    private Rigidbody2D rb;
    private int direction = 1;  // +1 = right, -1 = left

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Always enforce horizontal speed; leave y‑velocity alone so gravity works
        Vector2 v = rb.linearVelocity;
        v.x = direction * slideSpeed;
        rb.linearVelocity = v;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        
        
        // Look for any contact with a mostly‑vertical normal (i.e. walls)
        foreach (var contact in col.contacts)
        {
            if (Mathf.Abs(contact.normal.x) > 0.5f)
            {
                // flip direction
                direction = -direction;
                break;
            }
        }
    }

    // Optional: visualize a little arrow in Scene view showing direction
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.right * direction * 0.5f);
    }
}
