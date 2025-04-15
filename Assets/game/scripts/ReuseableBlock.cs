using System.Collections;
using UnityEngine;

public class ReuseableBlock : MonoBehaviour
{
    [Header("Item Data")]
    public ItemData itemData;
    public Transform spawnPoint;

    [Header("Bump Settings")]
    [Tooltip("How high the block moves when hit")]
    public float bumpHeight = 0.5f;
    [Tooltip("How fast the block moves")]
    public float bumpSpeed = 4f;

    private SpriteRenderer sr;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (!col.gameObject.CompareTag("Player")) return;

        // only trigger when hit from below
        ContactPoint2D contact = col.contacts[0];
        if (contact.normal.y > 0.5f)
        {
            // start the bump animation
            StartCoroutine(BumpCoroutine());
            // spawn the item each time
            SpawnItem();
        }
    }

    private IEnumerator BumpCoroutine()
    {
        Vector3 original = transform.localPosition;
        Vector3 peak     = original + Vector3.up * bumpHeight;

        // move up
        while (transform.localPosition.y < peak.y)
        {
            transform.localPosition = Vector3.MoveTowards(
                transform.localPosition, peak, bumpSpeed * Time.deltaTime);
            yield return null;
        }

        // move back down
        while (transform.localPosition.y > original.y)
        {
            transform.localPosition = Vector3.MoveTowards(
                transform.localPosition, original, bumpSpeed * Time.deltaTime);
            yield return null;
        }
    }

    private void SpawnItem()
    {
        if (itemData == null || itemData.prefab == null || spawnPoint == null) 
            return;

        Vector3 pos = (Vector2)spawnPoint.position + itemData.spawnOffset;
        Instantiate(itemData.prefab, pos, Quaternion.identity);

        if (itemData.spawnSound != null)
        {
            // play sound if you like
            var src = GetComponent<AudioSource>();
            if (src != null) src.PlayOneShot(itemData.spawnSound);
        }
    }
}
