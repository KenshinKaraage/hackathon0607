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
        Debug.Log("�ڑ������I");
        PhotonNetwork.JoinOrCreateRoom("TestRoom", new RoomOptions { MaxPlayers = 4 }, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("���[���ɓ���܂����F" + PhotonNetwork.CurrentRoom.Name);

        // ���v���C���[�ɂ��ʒm�iRPC�Ŋm�F�j
        photonView.RPC("ShowJoinLog", RpcTarget.All, PhotonNetwork.NickName);
    }

    [PunRPC]
    void ShowJoinLog(string nickname)
    {
        Debug.Log($"{nickname} ���������܂���");
    }

    void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.NickName = "User" + Random.Range(1000, 9999);
    }
}
