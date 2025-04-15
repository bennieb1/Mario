using UnityEngine;

public class MushroomMove : MonoBehaviour
{
    [Header("Movement")]
    [Tooltip("Horizontal speed of the mushroom")]
    public float moveSpeed = 2f;

    [Header("Ground Check")]
    [Tooltip("Empty child used to detect ground below the mushroom")]
    public Transform groundCheck;
    [Tooltip("Radius of the ground‑check circle")]
    public float groundCheckRadius = 0.1f;
    [Tooltip("Which layers count as ground")]
    public LayerMask groundLayer;

    // internal state
    private Rigidbody2D rb;
    private float direction = 1f;  // +1 = right, -1 = left

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        // give it an initial horizontal push
        rb.linearVelocity = new Vector2(direction * moveSpeed, rb.linearVelocity.y);
    }

    void FixedUpdate()
    {
        // 1) Are we standing on ground?
        bool isGrounded = Physics2D.OverlapCircle(
            groundCheck.position,
            groundCheckRadius,
            groundLayer
        );

        if (isGrounded)
        {
            // 2) Maintain horizontal speed
            rb.linearVelocity = new Vector2(direction * moveSpeed, rb.linearVelocity.y);
        }
        // if not grounded, we do nothing—gravity will pull it down
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Bounce off any wall (horizontal surface)
        foreach (var contact in collision.contacts)
        {
            if (Mathf.Abs(contact.normal.x) > 0.5f)
            {
                // invert direction
                direction *= -1f;
                // immediately apply the new horizontal speed
                rb.linearVelocity = new Vector2(direction * moveSpeed, rb.linearVelocity.y);
                break;
            }
        }
    }

    // Draw the ground‑check in the Scene view
    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}
