using UnityEngine;
public enum ItemType { Coin, OneUp, PowerUp }
public enum PowerUpType { SuperMushroom, FireFlower, Star }

[CreateAssetMenu(fileName = "New Item Data", menuName = "Items/ItemData")]
public class ItemData : ScriptableObject
{
    [Header("Basic Info")]
    public string displayName;
    public Sprite icon;

    [Header("Type")]
    public ItemType itemType;
    [Tooltip("Only used if itemType == PowerUp")]
    public PowerUpType powerUpType;

    [Header("Spawnable Prefab")]
    public GameObject prefab;

    [Header("Optional Settings")]
    public AudioClip spawnSound;
    public Vector2 spawnOffset = Vector2.zero;
}
