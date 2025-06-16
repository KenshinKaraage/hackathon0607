using Photon.Pun;
using Photon.Pun.Demo.Cockpit;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class LobbyConnector : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject notConnectOb;
    [SerializeField] private GameObject connectedOb;

    void Start()
    {
        notConnectOb.SetActive(true);
        connectedOb.SetActive(false);

        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings(); //サーバー接続
        }
    }

    public override void OnConnectedToMaster()
    {
        notConnectOb.SetActive(false);
        connectedOb.SetActive(true);
    }
}