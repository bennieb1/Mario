using UnityEngine;

public class BULLETmOVEMENT : MonoBehaviour
{
    public Vector2 direction = Vector2.right;
    public float speed = 5f;
    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType     = RigidbodyType2D.Kinematic;
        rb.gravityScale = 0f;
        rb.freezeRotation = true;
    }

    void Update()
    {
        // move at constant speed & height
        transform.Translate(direction.normalized * speed * Time.deltaTime);
    }
}
