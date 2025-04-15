using System;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : Singelton<GameManager>    
{
    public int Score { get; private set; }
    public int Coins { get; private set; }
    public int Lives { get; private set; }

    public bool IsMarioBig { get; private set; } = false;
    public void SetMarioBig(bool big) => IsMarioBig = big;

    
    public event Action<int,int,int> OnHUDChanged;  // (score, coins, lives)
    public event Action OnGameOver;

    protected override void Awake()
    {
        base.Awake();
        Lives = 3;
        Score = 0;
        Coins = 0;
    }
  
    public void CollectItem(ItemData itemData)
    {
        switch (itemData.itemType)
        {
            case ItemType.Coin:
                Coins++;
                Score += 100;
                break;
            case ItemType.OneUp:
                Lives++;
                break;
            case ItemType.PowerUp:
                ApplyPowerUp(itemData.powerUpType);
                break;
        }
        OnHUDChanged?.Invoke(Score, Coins, Lives);
    }

    private void ApplyPowerUp(PowerUpType type)
    {
        switch (type)
        {
            case PowerUpType.SuperMushroom:
                MarioSizeController.Instance.Grow();
                playerHealthController.Instance.HealPlayer();
                break;
            case PowerUpType.FireFlower:
                MarioSizeController.Instance.GainFireFlower();
                break;
            case PowerUpType.Star:
                MarioSizeController.Instance.StartStarInvincibility();
                break;
        }
    }
    public void TriggerGameOver()
    {
        OnGameOver?.Invoke();
    }

    public void PlayerDamaged()
    {
        if (!IsMarioBig)
        {
            // Mario is small → die
            PlayerController.Instance.Die();
        }
        else
        {
            // Mario is big → shrink him
            MarioSizeController.Instance.Shrink();
            // notify the manager that he’s now small
            SetMarioBig(false);
        }
    }
}

