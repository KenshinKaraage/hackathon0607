using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public AudioSource seAudioSource; // 効果音用（PlayOneShot）

    [Header("Audio Source")]
    public AudioSource bgmAudioSource; // BGM用（Loop再生）

    [Header("Sound Effects")]
    public AudioClip buttonSound;
    public AudioClip distributionSound;
    public AudioClip winSound;
    public AudioClip loseSound;
    // 必要なSEをここに追加

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // 効果音を名前で再生
    public void PlaySE(string name)
    {
        AudioClip clip = null;

        switch (name)
        {
            case "click":
                clip = buttonSound;
                break;
            case "win":
                clip = winSound;
                break;
            case "lose":
                clip = loseSound;
                break;
            case "cancel":
                clip = distributionSound;
                break;
            default:
                Debug.LogWarning($"SE '{name}' not found.");
                break;
        }

        if (clip != null)
            seAudioSource.PlayOneShot(clip);

    }

    // BGMの制御
    public void PlayBGM(AudioClip clip, bool loop = true)
    {
        if (bgmAudioSource.clip == clip) return;

        bgmAudioSource.clip = clip;
        bgmAudioSource.loop = loop;
        bgmAudioSource.Play();
    }

    public void StopBGM()
    {
        bgmAudioSource.Stop();
    }
}
