using System.Collections;
using UnityEngine;

public class Block : MonoBehaviour
{
   [Header("Item Data")]
    public ItemData itemData;
    public Transform spawnPoint;

    [Header("Bump Settings")]
    [Tooltip("How high the block moves when hit")]
    public float bumpHeight = 0.5f;
    [Tooltip("How fast the block moves")]
    public float bumpSpeed = 4f;
    [Tooltip("Sprite to show after block is used")]
    public Sprite usedBlockSprite;

    private Animator       anim;
    private AudioSource    audioSrc;
    private SpriteRenderer sr;
    private bool           used = false;

    void Awake()
    {
        anim     = GetComponent<Animator>();
        audioSrc = GetComponent<AudioSource>();
        sr       = GetComponent<SpriteRenderer>();
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (used) return;
        if (!col.gameObject.CompareTag("Player")) return;

        // only if hit from below
        ContactPoint2D contact = col.contacts[0];
        if (contact.normal.y > 0.5f)
        {
            used = true;
            anim.SetTrigger("Hit");               // fire your bump animation (e.g. shake or color)
            StartCoroutine(BumpCoroutine());      // physically move the block
            SpawnItem();                          // your existing spawn logic
        }
    }

    
    private IEnumerator BumpCoroutine()
    {
        Vector3 original = transform.localPosition;
        Vector3 target   = original + Vector3.up * bumpHeight;

        // move up
        while (transform.localPosition.y < target.y)
        {
            transform.localPosition = Vector3.MoveTowards(
                transform.localPosition, target, bumpSpeed * Time.deltaTime);
            yield return null;
        }

        // move back down
        while (transform.localPosition.y > original.y)
        {
            transform.localPosition = Vector3.MoveTowards(
                transform.localPosition, original, bumpSpeed * Time.deltaTime);
            yield return null;
        }

        // swap to used sprite
        if (usedBlockSprite != null)
            sr.sprite = usedBlockSprite;
    }

    private void SpawnItem()
    {
        if (itemData == null || itemData.prefab == null || spawnPoint == null) return;

        Vector3 pos = (Vector2)spawnPoint.position + itemData.spawnOffset;
        Instantiate(itemData.prefab, pos, Quaternion.identity);

        if (itemData.spawnSound != null)
            audioSrc.PlayOneShot(itemData.spawnSound);
    }
}
