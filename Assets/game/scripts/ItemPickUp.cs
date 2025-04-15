using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created  public ItemData itemData;
    [Tooltip("Drag in the ItemData ScriptableObject for this pickup")]
    public ItemData itemData;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        GameManager.Instance.CollectItem(itemData);
        Destroy(gameObject);
    }
}
