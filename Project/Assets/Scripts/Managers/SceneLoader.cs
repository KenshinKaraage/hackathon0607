using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance;

    private void Awake()
    {
        // �V���O���g���̃Z�b�g
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // �V�[�����܂����ł��j������Ȃ�
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // �V�[�������[�h����i�t�F�[�h�t���Ȃǂɂ��g���\�j
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
