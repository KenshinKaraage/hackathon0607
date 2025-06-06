using Photon.Pun;
using TMPro;
using UnityEngine;

public class NameInputManager : MonoBehaviour
{
    public void OnClickCreateRoom()
    {
        string password = PasswordGenerator.Generate(5);
        if (!string.IsNullOrEmpty(password) && !string.IsNullOrEmpty(PhotonNetwork.NickName))
        {
            RoomManager.Instance.password = password;

            RoomManager.Instance.CreateRoom($"{PhotonNetwork.NickName}の部屋", password); // 任意の部屋名
        }
        else
        {
            Debug.LogWarning("ユーザー名が未入力です。");
        }
    }

    public void OnClickJoinRoom()
    {
        if (!string.IsNullOrEmpty(PhotonNetwork.NickName))
        {
            LobbyUIPresentor presentor = FindAnyObjectByType<LobbyUIPresentor>();
            presentor.ShowRoomTable();
        }
        else
        {
            Debug.LogWarning("ユーザー名が未入力です。");
        }
    }
}
