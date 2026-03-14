using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// A global audio controller that persists across scenes, playing the correct BGM 
/// for each scene and providing a method to play UI sound effects.
/// </summary>
public class GlobalAudioController : MonoBehaviour
{
    private static GlobalAudioController instance;
    private AudioSource bgmSource;
    private AudioSource sfxSource;

    [Header("Audio Clips")]
    public AudioClip menuBGM;
    public AudioClip levelBGM;
    public AudioClip winBGM;
    public AudioClip loseBGM;
    public AudioClip uiClickClip;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        Object.DontDestroyOnLoad(gameObject);

        // Setup Audio Sources
        bgmSource = gameObject.AddComponent<AudioSource>();
        bgmSource.loop = true;
        bgmSource.volume = 0.4f;

        sfxSource = gameObject.AddComponent<AudioSource>();
        sfxSource.loop = false;
        sfxSource.volume = 0.8f;

        // Subscribe to scene change events
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        AudioClip nextBGM = null;
        
        // Scene 0 = Main Menu
        // Scene 1 = How to Play
        // Scene 2 = Level (Gameplay)
        // Scene 3 = Lose
        // Scene 4 = Win
        int buildIndex = scene.buildIndex;

        if (buildIndex == 0 || buildIndex == 1)
        {
            nextBGM = menuBGM;
        }
        else if (buildIndex == 2)
        {
            nextBGM = levelBGM;
        }
        else if (buildIndex == 3)
        {
            nextBGM = loseBGM;
            // Stop looping so it just plays the sad melody once if desired, 
            // but for now we loop it
            bgmSource.loop = false; 
        }
        else if (buildIndex == 4)
        {
            nextBGM = winBGM;
            bgmSource.loop = true;
        }

        // Only change if the BGM is actually different (so it doesn't restart going from Menu to HowToPlay)
        if (nextBGM != null && bgmSource.clip != nextBGM)
        {
            bgmSource.Stop();
            bgmSource.clip = nextBGM;
            bgmSource.Play();
        }
    }

    /// <summary>
    /// Play the UI click sound. Can be hooked up to Button OnClick events via code or Inspector (if it wasn't DontDestroyOnLoad).
    /// </summary>
    public static void PlayUIClick()
    {
        if (instance != null && instance.uiClickClip != null)
        {
            instance.sfxSource.PlayOneShot(instance.uiClickClip);
        }
    }
}
