using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("---------- Audio Source ----------")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource SFXSource;

    [Header("---------- Audio Clip ----------")]
    public AudioClip background;
    
    public AudioClip takeDamage;
    public AudioClip Floor1;
    public AudioClip Floor2;
    public AudioClip Floor3;
    public AudioClip menu;
    public AudioClip Shoot;
    public AudioClip Death;
    public AudioClip thanks;
    public AudioClip doctorDeath;

    public AudioClip nursedeath;

    public AudioClip scissors;

    public AudioClip explosion;
    public AudioClip bossdeath;

    public AudioClip bossCHarge;

    public AudioClip bossshoot;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Sahne değişiminde yok olmasın
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        UpdateMusic();
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        UpdateMusic();
    }

    public void UpdateMusic()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        switch (sceneName)
        {
            case "menu":
                ChangeMusic(menu);
                break;
            case "Floor1":
                ChangeMusic(Floor1);
                break;
            case "Floor2":
                ChangeMusic(Floor2);
                break;
            case "Floor3":
                ChangeMusic(Floor3);
                break;
            case "Thanks": 
                ChangeMusic(thanks);
                break;
            default:
                ChangeMusic(background);
                break;
        }
    }

   private void ChangeMusic(AudioClip newClip)
{
    if (musicSource.clip != newClip)
    {
        musicSource.Stop();
        musicSource.clip = newClip;
        musicSource.loop = true; 
        musicSource.Play();
    }
}


    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }
}
