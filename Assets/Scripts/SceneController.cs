using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneControllerScript : MonoBehaviour
{
    public static SceneControllerScript instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void NextLevel()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
            Invoke(nameof(UpdateAudio), 0.1f);
        }
        else
        {
            Debug.LogWarning("Sonraki sahne bulunamadı. Build Settings'i kontrol edin!");
        }
    }

    public void LoadScene(string sceneName)
    {
        if (Application.CanStreamedLevelBeLoaded(sceneName))
        {
            SceneManager.LoadScene(sceneName);
            Invoke(nameof(UpdateAudio), 0.1f);
        }
        else
        {
            Debug.LogError("Sahne adı geçerli değil veya Build Settings'te eklenmemiş: " + sceneName);
        }
    }

    private void UpdateAudio()
    {
        AudioManager audioManager = FindObjectOfType<AudioManager>();
        if (audioManager != null)
        {
            audioManager.UpdateMusic();
        }
    }
}
