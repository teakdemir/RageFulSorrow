using UnityEngine;

public class NextFloor : MonoBehaviour
{
    [SerializeField] private bool goNext; // Bir sonraki sahneye geçiş
    [SerializeField] private string levelName; // Belirtilen sahne adı

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (goNext)
            {
                SceneControllerScript.instance.NextLevel(); // Sonraki sahneyi yükle
            }
            else
            {
                SceneControllerScript.instance.LoadScene(levelName); // Belirtilen sahneyi yükle
            }
        }
    }
}

