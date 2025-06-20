
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singelton<GameManager>
{
    // === ADJUST THESE TO MATCH YOUR BUILD SETTINGS ===
    private const int TitleSceneBuildIndex = 0;
    private const int MainSceneBuildIndex  = 1;
    private const int CreditsSceneBuildIndex = 2;
    // ==================================================

    public int Score { get; private set; }
    public int Coins { get; private set; }
    public int Lives { get; private set; }

    public bool IsMarioBig { get; private set; } = false;
    public void SetMarioBig(bool big) => IsMarioBig = big;

    // Fired whenever score/coins/lives change
    public event Action<int,int,int> OnHUDChanged;
    // Fired when the player runs out of lives
    public event Action OnGameOver;

    // Reference to the Game Over UI Canvas in the Main (game) scene
    private GameObject _gameOverCanvas;
    // Tracks whether we are currently in “Game Over” state
    private bool _isGameOver = false;

    protected override void Awake()
    {
        base.Awake();

        // Initialize stats at the very start
        Lives = 3;
        Score = 0;
        Coins = 0;

        // Listen for scene loads so we can grab the GameOverCanvas reference
        SceneManager.sceneLoaded += OnSceneLoaded;

        // Subscribe internal handler so that when TriggerGameOver() fires, we show UI
        OnGameOver += HandleGameOver;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        OnGameOver -= HandleGameOver;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // When MainScene (by build index) is loaded, find the GameOverCanvas and deactivate it
        if (scene.buildIndex == MainSceneBuildIndex)
        {
            _gameOverCanvas = GameObject.Find("GameOver_Panel");
            if (_gameOverCanvas != null)
            {
                _gameOverCanvas.SetActive(false);
            }

            // Reset the Game Over flag so ESC won't fire immediately
            _isGameOver = false;
        }
    }

    private void Update()
    {
        // If we are in Game Over, pressing ESC returns to Title scene
        if (_isGameOver && Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(TitleSceneBuildIndex);
            _isGameOver = false;
        }
    }

    #region PUBLIC METHODS FOR GAMEPLAY

    public void AddScore(int points)
    {
        Score += points;
        OnHUDChanged?.Invoke(Score, Coins, Lives);
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

    public void PlayerDamaged()
    {
        if (!IsMarioBig)
        {
            // small Mario → lose a life
            Lives--;
            OnHUDChanged?.Invoke(Score, Coins, Lives);

            if (Lives > 0)
            {
                RespawnManager.Instance.RespawnPlayer();
            }
            else
            {
                // no lives left → trigger death animation then Game Over
                PlayerController.Instance.Die();
            }
        }
        else
        {
            // big Mario → shrink back to small
            MarioSizeController.Instance.Shrink();
        }
    }

    // Called by PlayerController.OnDeathAnimationComplete()
    public void TriggerGameOver()
    {
        OnGameOver?.Invoke();
    }

    /// <summary>
    /// Call this from your Title‐scene “Play” button.
    /// Resets all stats, then loads the Main scene by build index.
    /// </summary>
    public void StartNewGame()
    {
        ResetStats();
        SceneManager.LoadScene(MainSceneBuildIndex);
    }
    
    public void LoadCreditsScene()
    {
        SceneManager.LoadScene(CreditsSceneBuildIndex);
    }

    #endregion

    #region INTERNALS

    private void HandleGameOver()
    {
        // Show the Game Over canvas (if found in OnSceneLoaded)
        if (_gameOverCanvas != null)
        {
            _gameOverCanvas.SetActive(true);
        }

        // Enable ESC-to-return
        _isGameOver = true;
    }

    private void ResetStats()
    {
        Lives = 3;
        Score = 0;
        Coins = 0;
        IsMarioBig = false;
        OnHUDChanged?.Invoke(Score, Coins, Lives);
    }

    #endregion
}