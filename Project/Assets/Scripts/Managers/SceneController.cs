using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class SceneController : MonoBehaviour
{
    public static SceneController Instance;

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

    public void GoToLobby()
    {
        SceneManager.LoadScene("LobbyScene2");
    }

    public void GoToRoom()
    {
        PhotonNetwork.LoadLevel("RoomScene2"); // PhotonNetwork.LoadLevel を推奨
    }

    public void GoToGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void GoToResult()
    {
        SceneManager.LoadScene("ResultScene");
    }

    public void BackToTitle()
    {
        SceneManager.LoadScene("TitleScene");
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
