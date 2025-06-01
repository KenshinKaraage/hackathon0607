using Photon.Pun;
using Photon.Pun.Demo.Cockpit;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;

public class LobbyUI : MonoBehaviour
{

    void Start()
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings(); //サーバー接続
        }
    }

    public void OnClickCreateRoom()
    {
        RoomManager.Instance.CreateRoom("MyRoom123"); // 任意の部屋名
    }

    public void OnClickJoinRoom()
    {
        RoomManager.Instance.JoinRoom("MyRoom123");
    }
}
