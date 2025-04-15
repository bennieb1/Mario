using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    void Awake()
    {
        GetComponent<Collider2D>().isTrigger = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        RespawnManager.Instance.SetSpawnPoint(transform.position);
        // (Optional) play a sound or animate the flag raising
    }
}
