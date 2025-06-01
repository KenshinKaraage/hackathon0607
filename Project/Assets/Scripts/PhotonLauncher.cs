using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class PhotonLauncher : MonoBehaviourPunCallbacks
{
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("接続成功！");
        PhotonNetwork.JoinOrCreateRoom("TestRoom", new RoomOptions { MaxPlayers = 4 }, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("ルームに入りました：" + PhotonNetwork.CurrentRoom.Name);

        // 他プレイヤーにも通知（RPCで確認）
        photonView.RPC("ShowJoinLog", RpcTarget.All, PhotonNetwork.NickName);
    }

    [PunRPC]
    void ShowJoinLog(string nickname)
    {
        Debug.Log($"{nickname} が入室しました");
    }

    void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.NickName = "User" + Random.Range(1000, 9999);
    }
}
