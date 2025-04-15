using System;
using UnityEngine;

public class Coins : MonoBehaviour
{
    [Tooltip("Assign your Coin ItemData asset here")]
    public ItemData itemData;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (GameManager.Instance == null)
        {
            Debug.LogError("Coins: GameManager.Instance is null! Did you forget to add a GameManager to the scene?");
            return;
        }

        if (itemData == null)
        {
            Debug.LogError($"Coins ({name}): itemData is not assigned in the Inspector!");
            return;
        }

        // All good — collect the coin
        GameManager.Instance.CollectItem(itemData);

        // Remove the coin from the scene
        Destroy(gameObject);
    }
}
