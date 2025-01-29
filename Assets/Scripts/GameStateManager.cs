using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance { get; private set; }
    public bool IsGameFrozen { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void FreezeGame()
    {
        IsGameFrozen = true;
        // Don't set timeScale to 0 anymore
        // Instead, we'll handle freezing through velocity checks
    }

    public void UnfreezeGame()
    {
        IsGameFrozen = false;
    }
}