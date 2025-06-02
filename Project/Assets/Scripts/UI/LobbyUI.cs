using Photon.Pun;
using Photon.Pun.Demo.Cockpit;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class LobbyUI : MonoBehaviour
{
    [SerializeField] private TMP_InputField passwordInput;

    void Start()
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings(); //サーバー接続
        }
    }

    public void OnClickCreateRoom()
    {
        RoomManager.Instance.CreateRoom("MyRoom123", "aaa"); // 任意の部屋名
        SceneManager.LoadScene("RoomScene");
    }

    public void OnClickJoinRoom()
    {
        string password = passwordInput.text.Trim();

        if (!string.IsNullOrEmpty(password))
        {
            RoomManager.Instance.JoinRoom("MyRoom123", password);
            SceneManager.LoadScene("RoomScene");
        }
        else
        {SceneManager.LoadScene("RoomScene");
            Debug.LogWarning("部屋名またはパスワードが未入力です。");
        }
    }






}
