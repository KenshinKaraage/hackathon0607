using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using TMPro;
using System.Collections.Generic;


public class PhotonLauncher : MonoBehaviourPunCallbacks
{

    [SerializeField] private TMP_Text nicknameText;

    private List<RoomInfo> cachedRoomList = new List<RoomInfo>();
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    private void Update()
    {
        nicknameText.text = "Name: " + PhotonNetwork.NickName;
    }


    //部屋に入った時
    public override void OnJoinedRoom()
    {

        Debug.Log("ルームに入りました：" + PhotonNetwork.CurrentRoom.Name);

        // 他プレイヤーにも通知（RPCで確認）
        photonView.RPC("ShowJoinLog", RpcTarget.All, PhotonNetwork.NickName);
    }





    //他プレイヤーのクライアント上でも呼び出せる  RPC = Remote Procedure Call
    [PunRPC]
    void ShowJoinLog(string nickname)
    {
        Debug.Log($"{nickname} が入室しました");
    }

    void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        //PhotonNetwork.NickName = "User" + Random.Range(1000, 9999);  //ユーザー名の割り当て
    }
}
