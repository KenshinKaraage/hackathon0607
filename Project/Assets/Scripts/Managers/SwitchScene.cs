using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    //�X�^�[�g��ʂ��烍�r�[�ɑJ�ڂ���Ƃ��Ɏg�p
    public void GoToLobby()
    {
        SceneManager.LoadScene("LobbyScene");
    }

    public void GoToRoom()
    {
        SceneManager.LoadScene("RoomScene");
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
