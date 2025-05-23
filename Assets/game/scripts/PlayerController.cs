using System.Collections;
using UnityEngine;

public class PlayerController : Singelton<PlayerController>
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;
    

    public Rigidbody2D rb2;
    public bool isGrounded;
    private bool canDoubleJump;
    public Transform groundCheck;
    public LayerMask GroundLayerMask;
    public LayerMask waterLayerMask;
    public float KnockBackLength, KnockBackForce;
    private float KnoackBackCounter;
    private Animator anim;
    private bool isDead = false;

    public float BounceFource;
    public bool stopInput;

    private SpriteRenderer sr;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
       
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead) return;


            if (KnoackBackCounter <= 0)
            {


                rb2.linearVelocity = new Vector2(moveSpeed * Input.GetAxis("Horizontal"), rb2.linearVelocity.y);

                isGrounded = Physics2D.OverlapCircle(groundCheck.position, .2f, GroundLayerMask) || Physics2D.OverlapCircle(groundCheck.position, .2f, waterLayerMask);

                if (isGrounded)
                {
                    canDoubleJump = true;
                }

                if (Input.GetButtonDown("Jump"))
                {
                    if (isGrounded)
                    {
                       
                        rb2.linearVelocity = new Vector2(rb2.linearVelocity.x, jumpForce);
                    
                    }
                    else
                    {
                        if (canDoubleJump)
                        {
                            rb2.linearVelocity = new Vector2(rb2.linearVelocity.x, jumpForce);
                       //     AudioManager.Instance.PlaySfx(10);
                            canDoubleJump = false;

                        }
                    }

                }

                if (rb2.linearVelocity.x < 0)
                {
                    sr.flipX = true;
                }
                else if (rb2.linearVelocity.x > 0)
                {
                    sr.flipX = false;
                }
            }
            else
            {
                KnoackBackCounter -= Time.deltaTime;
                if (!sr.flipX)
                {
                    rb2.linearVelocity = new Vector2(-KnockBackForce, rb2.linearVelocity.y);
                }
                else
                {
                    rb2.linearVelocity = new Vector2(KnockBackForce, rb2.linearVelocity.y);
                }
            }
            
        anim.SetFloat("MoveSpeed",Mathf.Abs( rb2.linearVelocity.x));
        anim.SetBool("isGrounded", isGrounded);
    }

    public void KnockBack()
    {
        GameManager.Instance.PlayerDamaged();
              KnoackBackCounter = KnockBackLength;
               rb2.linearVelocity = new Vector2(0f, KnockBackForce);
           anim.SetTrigger("Hurt");
    }

    public void Bounce()
    {
        rb2.linearVelocity = new Vector2(rb2.linearVelocity.x, BounceFource);
       // AudioManager.Instance.PlaySfx(10);
    }
    
    public void Die()
    {
        if (isDead) return;
        isDead = true;

        // stop movement, trigger death anim
        rb2.linearVelocity = Vector2.zero;
        stopInput = true;
        anim.SetTrigger("Dead");

        StartCoroutine( OnDeathAnimationComplete() );
    }

    private IEnumerator OnDeathAnimationComplete()
    {
        // wait for the death clip to finish
        var info = anim.GetCurrentAnimatorStateInfo(0);
        yield return new WaitForSeconds(info.length + 0.1f);
        GameManager.Instance.TriggerGameOver();
        Destroy(gameObject);
    }
}
