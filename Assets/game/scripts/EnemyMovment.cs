using UnityEngine;

public class EnemyMovment : MonoBehaviour
{
    [Header("Horizontal Movement")]
    [Tooltip("Units per second")]
    public float speed = 2f;

    [Header("Jump Settings")]
    [Tooltip("Time between jumps in seconds")]
    public float jumpInterval = 1.5f;
    [Tooltip("Initial upward velocity applied on jump")]
    public float jumpForce = 5f;
    [Tooltip("Optional ground‑check so it only jumps when touching ground")]
    public Transform groundCheck;
    public LayerMask groundLayer;
    public float groundCheckRadius = 0.1f;

    [Header("Sprite Flipping")]
    [Tooltip("SpriteRenderer of the visual child")]
    public SpriteRenderer spriteRenderer;
    [Tooltip("Does your sprite face right when flipX = false?")]
    public bool defaultFacingRight = true;

    // internals
    private Rigidbody2D rb;
    private EnemyPathFollow path;
    private float jumpTimer;
    private Vector3 lastPosition;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        path = GetComponent<EnemyPathFollow>();
        if (spriteRenderer == null)
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    void Start()
    {
        lastPosition = transform.position;
        jumpTimer = jumpInterval; // so it jumps immediately if desired
    }

    void Update()
    {
        // 1) Handle flipping based on actual movement
        Vector3 delta = transform.position - lastPosition;
        if (spriteRenderer != null)
        {
            if (delta.x > 0.001f)        spriteRenderer.flipX = defaultFacingRight ? false : true;
            else if (delta.x < -0.001f)  spriteRenderer.flipX = defaultFacingRight ? true : false;
        }
        lastPosition = transform.position;

        // 2) Jump timer
        jumpTimer -= Time.deltaTime;
        if (jumpTimer <= 0f)
        {
            if (CanJump())
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            }
            jumpTimer = jumpInterval;
        }
    }

    void FixedUpdate()
    {
        // 3) Horizontal path movement
        if (path.CurrentPoint == null) return;
        
        // move enemy left to right without path following

        Vector2 move = transform.position; 
        Vector2 movementDir = move * Vector2.right * (speed * Time.fixedDeltaTime);
        rb.MovePosition(rb.position + movementDir);

        Vector2 pos    = transform.position;
        Vector2 target = path.CurrentPoint.position;
        float dir      = target.x > pos.x + 0.01f ? 1f : target.x < pos.x - 0.01f ? -1f : 0f;

        rb.linearVelocity = new Vector2(dir * speed, rb.linearVelocity.y);

        // Advance when we arrive
        if (Mathf.Abs(pos.x - target.x) < 0.05f)
            path.AdvancePoint();
    }

    private bool CanJump()
    {
        if (groundCheck == null) return true;
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer) != null;
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}
