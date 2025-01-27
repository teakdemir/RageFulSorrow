using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneControllerScript : MonoBehaviour
{
    public static SceneControllerScript instance;

    private void Awake()
    {
        // Singleton kontrolü
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Sahne değişiminde yok olmasın
        }
        else
        {
            Destroy(gameObject); // Fazladan oluşan GameObject'leri yok et
        }
    }

    public void NextLevel()
    {
        // Aktif sahnenin bir sonrakini yükler
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.LogWarning("Sonraki sahne bulunamadı. Build Settings'i kontrol edin!");
        }
    }

    public void LoadScene(string sceneName)
    {
        // Belirtilen sahneyi yükler
        if (Application.CanStreamedLevelBeLoaded(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogError("Sahne adı geçerli değil veya Build Settings'te eklenmemiş: " + sceneName);
        }
    }
}
