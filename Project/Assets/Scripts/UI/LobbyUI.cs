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
            PhotonNetwork.ConnectUsingSettings(); //�T�[�o�[�ڑ�
        }
    }

    public void OnClickCreateRoom()
    {
        RoomManager.Instance.CreateRoom("MyRoom123"); // �C�ӂ̕�����
    }

    public void OnClickJoinRoom()
    {
        RoomManager.Instance.JoinRoom("MyRoom123");
    }
}
