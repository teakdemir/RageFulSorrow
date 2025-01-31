using UnityEngine;
using UnityEngine.SceneManagement;
using static PlayerStats;

public class RetryButton : MonoBehaviour
{
    public void OnRetryButtonClicked()
    {
        // Destroy the persistent player
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            Destroy(player);
        }

        // Reset GameStateManager
        if (GameStateManager.Instance != null)
        {
            GameStateManager.Instance.UnfreezeGame();
        }

        // Destroy the SceneController if needed
        if (SceneControllerScript.instance != null)
        {
            Destroy(SceneControllerScript.instance.gameObject);
        }

        // Load the last played level
        SceneManager.LoadScene(GameData.LastPlayedLevel);
    }
}