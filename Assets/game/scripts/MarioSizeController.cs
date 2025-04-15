
using Unity.VisualScripting;
using UnityEngine;


public enum MarioForm { Small, Big, Fire, Star }

public class MarioSizeController : Singelton<MarioSizeController>
{
      [Header("Collider Sizes & Offsets")]
    [SerializeField] private float smallColliderRadius   = 0.5f;
    [SerializeField] private Vector2 smallColliderOffset = Vector2.zero;
    [SerializeField] private float bigColliderRadius     = 1.5f;
    [SerializeField] private Vector2 bigColliderOffset   = Vector2.zero;

    [Header("Ground Checker Offsets")]
    [SerializeField] private Vector2 smallGroundCheckPos = new Vector2(0f, -0.6f);
    [SerializeField] private Vector2 bigGroundCheckPos   = new Vector2(0f, -1.1f);

    [Header("Scale Settings")]
    [SerializeField] private Vector3 smallScale = new Vector3(1f, 1f, 1f);
    [SerializeField] private Vector3 bigScale   = new Vector3(1.5f, 1.5f, 1f);

    [Header("Star Power Settings")]
    [SerializeField] private float starDuration = 10f;
    [SerializeField] private GameObject starEffectPrefab;

    [Header("References")]
    [Tooltip("CircleCollider2D on Mario's body")]
    [SerializeField] private CircleCollider2D bodyCollider;
    [Tooltip("Child Transform used for ground checks")]
    [SerializeField] private Transform groundCheckTransform;


    private float     starTimer;
    private GameObject starEffectInstance;
    private Animator  anim;
    private Rigidbody2D rb;
    public enum MarioForm { Small, Big, Fire, Star }
    private MarioForm currentForm = MarioForm.Small;

    protected override void Awake()
    {
        base.Awake();
        anim = GetComponent<Animator>();
        rb   = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        if (bodyCollider == null || groundCheckTransform == null || anim == null || rb == null)
        {
            Debug.LogError("MarioSizeController: Missing references!");
            enabled = false;
            return;
        }
        ApplyFormState();
    }

    void Update()
    {
        if (currentForm == MarioForm.Star)
        {
            starTimer -= Time.deltaTime;
            if (starTimer <= 0f)
                EndStarPower();
        }
    }

    /// <summary>Called by GameManager when Mario picks up a Super Mushroom</summary>
    public void Grow()
    {
        if (bodyCollider == null || groundCheckTransform == null || anim == null || rb == null)
        {
            Debug.LogError("MarioSizeController.Grow(): one or more references are null! " +
                           "Make sure bodyCollider, groundCheckTransform, Animator, and Rigidbody2D are assigned.", this);
            return;
        }
        if (currentForm != MarioForm.Small) return;
        currentForm = MarioForm.Big;
        ApplyFormState();
        GameManager.Instance.SetMarioBig(true);
    }

    /// <summary>Called by GameManager when Mario picks up a Fire Flower</summary>
    public void GainFireFlower()
    {
        if (currentForm != MarioForm.Big) return;
        currentForm = MarioForm.Fire;
        ApplyFormState();
        GameManager.Instance.SetMarioBig(true);
    }

    /// <summary>Called by GameManager when Mario picks up a Star</summary>
    public void StartStarInvincibility()
    {
        currentForm = MarioForm.Star;
        ApplyFormState();
        starTimer = starDuration;
        if (starEffectPrefab != null)
            starEffectInstance = Instantiate(starEffectPrefab, transform);
        GameManager.Instance.SetMarioBig(true);
    }

    /// <summary>Called when Mario takes damage and should shrink</summary>
    public void Shrink()
    {
        if (currentForm != MarioForm.Big) return;
        currentForm = MarioForm.Small;
        ApplyFormState();
        GameManager.Instance.SetMarioBig(false);
    }

    private void EndStarPower()
    {
        if (starEffectInstance != null)
            Destroy(starEffectInstance);

        currentForm = MarioForm.Big;
        ApplyFormState();
        GameManager.Instance.SetMarioBig(true);
    }

    /// <summary>Apply collider size, ground‑check offset, scale, and animator flags</summary>
    private void ApplyFormState()
    {
        // 1) Collider & ground‑check
        if (currentForm == MarioForm.Small)
        {
            bodyCollider.radius = smallColliderRadius;
            bodyCollider.offset = smallColliderOffset;
            groundCheckTransform.localPosition = smallGroundCheckPos;
        }
        else
        {
            bodyCollider.radius = bigColliderRadius;
            bodyCollider.offset = bigColliderOffset;
            groundCheckTransform.localPosition = bigGroundCheckPos;
        }

        // Nudge up to avoid embedding in ground
        rb.MovePosition(rb.position + Vector2.up * 0.05f);

        // 2) Scale
        transform.localScale = (currentForm == MarioForm.Small)
            ? smallScale
            : bigScale;

        // 3) Animator flags
        anim.SetBool("isBig",        currentForm == MarioForm.Big || currentForm == MarioForm.Fire || currentForm == MarioForm.Star);
        anim.SetBool("hasFireFlower", currentForm == MarioForm.Fire);
        anim.SetBool("isStar",        currentForm == MarioForm.Star);
    }
}
