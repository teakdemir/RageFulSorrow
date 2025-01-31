using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class VideoEndSceneLoader : MonoBehaviour
{
    public VideoPlayer videoPlayer; // Video Player bileşenini buraya atayın
    public int nextSceneIndex;      // Geçilecek sahnenin index numarası

    private void Start()
    {
        // Video tamamlandığında bir event tetikle
        videoPlayer.loopPointReached += OnVideoEnd;
    }

    private void OnVideoEnd(VideoPlayer vp)
    {
        // Belirtilen indexteki sahneye geç
        SceneManager.LoadScene(nextSceneIndex);
    }
}
