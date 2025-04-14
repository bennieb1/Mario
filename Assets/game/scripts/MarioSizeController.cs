using UnityEngine;

public class MarioSizeController : Singelton<MarioSizeController>
{
 [Header("Collider Sizes & Offsets")]
    [SerializeField] private Vector2 smallColliderSize   = new Vector2(1f, 1f);
    [SerializeField] private Vector2 smallColliderOffset = Vector2.zero;
    [SerializeField] private Vector2 bigColliderSize     = new Vector2(1f, 2f);
    [SerializeField] private Vector2 bigColliderOffset   = Vector2.zero;

    [Header("Ground Checker Offsets")]
    [SerializeField] private Vector2 smallGroundCheckPos = new Vector2(0f, -0.6f);
    [SerializeField] private Vector2 bigGroundCheckPos   = new Vector2(0f, -1.1f);

    [Header("References")]
    [Tooltip("The BoxCollider2D attached to Mario’s body")]
    [SerializeField] private BoxCollider2D bodyCollider;
    [Tooltip("A child Transform you use for ground‑checking")]
    [SerializeField] private Transform groundCheckTransform;

    private bool isBig = false;

    // If you need to add any Awake logic, override and call base:
   

    private void Start()
    {
        if (bodyCollider == null || groundCheckTransform == null)
        {
            Debug.LogError("MarioSizeController: Missing references!");
            enabled = false;
            return;
        }
        ApplySizeState();
    }

    /// <summary>Grow Mario to big size.</summary>
    public void Grow()
    {
        isBig = true;
        ApplySizeState();
    }

    /// <summary>Shrink Mario back to small size.</summary>
    public void Shrink()
    {
        isBig = false;
        ApplySizeState();
    }

    private void ApplySizeState()
    {
        // Adjust collider
        if (isBig)
        {
            bodyCollider.size   = bigColliderSize;
            bodyCollider.offset = bigColliderOffset;
            groundCheckTransform.localPosition = bigGroundCheckPos;
        }
        else
        {
            bodyCollider.size   = smallColliderSize;
            bodyCollider.offset = smallColliderOffset;
            groundCheckTransform.localPosition = smallGroundCheckPos;
        }

        // Nudge upward slightly in case you're overlapping ground
        var rb = GetComponent<Rigidbody2D>();
        if (rb != null)
            rb.MovePosition(rb.position + Vector2.up * 0.05f);
    }
}
