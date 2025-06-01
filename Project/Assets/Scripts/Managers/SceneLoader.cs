using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance;

    private void Awake()
    {
        // シングルトンのセット
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // シーンをまたいでも破棄されない
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // シーンをロードする（フェード付きなどにも拡張可能）
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
