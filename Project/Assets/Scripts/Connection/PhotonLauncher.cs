using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using TMPro;


public class PhotonLauncher : MonoBehaviourPunCallbacks
{

    [SerializeField] private TMP_Text nicknameText;

    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    private void Update()
    {
        nicknameText.text = "Name: " + PhotonNetwork.NickName;
    }

    //�T�[�o�[�ڑ���
    public override void OnConnectedToMaster()
    {
        Debug.Log("�ڑ������I");
        //PhotonNetwork.JoinOrCreateRoom("TestRoom", new RoomOptions { MaxPlayers = 4 }, TypedLobby.Default);
    }


    //�����ɓ�������
    public override void OnJoinedRoom()
    {

        Debug.Log("���[���ɓ���܂����F" + PhotonNetwork.CurrentRoom.Name);


        // ���v���C���[�ɂ��ʒm�iRPC�Ŋm�F�j
        photonView.RPC("ShowJoinLog", RpcTarget.All, PhotonNetwork.NickName);
    }




    //���v���C���[�̃N���C�A���g��ł��Ăяo����  RPC = Remote Procedure Call
    [PunRPC]
    void ShowJoinLog(string nickname)
    {
        Debug.Log($"{nickname} ���������܂���");
    }

    void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        //PhotonNetwork.NickName = "User" + Random.Range(1000, 9999);  //���[�U�[���̊��蓖��
    }
}
