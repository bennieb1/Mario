using System;
using UnityEngine;

public class ShellMovement : MonoBehaviour
{
  
    [Tooltip("Horizontal speed of the shell")]
    public float slideSpeed = 5f;

    private Rigidbody2D rb;
    private int direction = 1;  // +1 = right, -1 = left

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        // ensure we start sliding immediately
        rb.linearVelocity = new Vector2(direction * slideSpeed, rb.linearVelocity.y);
    }

    void FixedUpdate()
    {
        // enforce horizontal speed; leave y alone so gravity works
        Vector2 v = rb.linearVelocity;
        v.x = direction * slideSpeed;
        rb.linearVelocity = v;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        // make sure it's a solid collision (not a trigger)
        if (col.collider.isTrigger) return;

        foreach (var contact in col.contacts)
        {
            // a mostly‐vertical normal means a wall
            if (Mathf.Abs(contact.normal.x) > 0.5f)
            {
                // flip direction
                direction = -direction;

                // immediately apply new velocity
                Vector2 v = rb.linearVelocity;
                v.x = direction * slideSpeed;
                rb.linearVelocity = v;

                break;
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        // draw a little arrow so you can see the current direction
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.right * direction * 0.5f);
    }
}
