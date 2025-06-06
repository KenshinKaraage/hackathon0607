using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    //スタート画面からロビーに遷移するときに使用
    public void GoToLobby()
    {
        SceneManager.LoadScene("LobbyScene2");
    }

    public void GoToRoom()
    {
        SceneManager.LoadScene("RoomScene2");
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
}
