using Photon.Pun;
using Photon.Pun.Demo.Cockpit;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomManager : MonoBehaviourPunCallbacks
{
    public static RoomManager Instance; //RoomManager.Instance�łǂ�����ł��A�N�Z�X�\�ɂȂ�


    public string RoomName { get; private set; } //����ID
    public bool IsHost { get; private set; }  //�z�X�g���ǂ���


    //Instance�̏�����
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {//�d��������h�����߂ɔj������
            Destroy(gameObject);
        }
    }

    // �������쐬�i�z�X�g���j
    public void CreateRoom(string roomName, string password)
    {
        var roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 4;

        // �J�X�^���v���p�e�B�Ƀp�X���[�h��ǉ�
        ExitGames.Client.Photon.Hashtable customProperties = new ExitGames.Client.Photon.Hashtable();
        customProperties["password"] = password;
        roomOptions.CustomRoomProperties = customProperties;

        // �O�������������悤�ɂ���i���r�[�\���̂��߁j
        roomOptions.CustomRoomPropertiesForLobby = new string[] { "password" };

        PhotonNetwork.CreateRoom(roomName, roomOptions, TypedLobby.Default);
    }

    // �����ɎQ���i�Q�X�g���j
    public void JoinRoom(string roomName, string enteredPassword)
    {
        // �����ꗗ���擾���ďƍ�
        foreach (RoomInfo room in RoomListManager.Instance.GetRoomList())
        {
            if (room.Name == roomName)
            {
                if (room.CustomProperties.TryGetValue("password", out object pwObj))
                {
                    string realPassword = pwObj as string;
                    if (realPassword == enteredPassword)
                    {
                        PhotonNetwork.JoinRoom(roomName);
                        return;
                    }
                    else
                    {
                        Debug.LogWarning("�p�X���[�h���Ⴂ�܂��I");
                        return;
                    }
                }
            }
        }

        Debug.LogWarning("�w�肳�ꂽ������������܂���ł���");
    }


    // �����_����RoomID�𐶐�����֐�
    private string GenerateRandomRoomID()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        for (int i = 0; i < 6; i++)
            sb.Append(chars[Random.Range(0, chars.Length)]);
        return sb.ToString();
    }

    // �쐬������
    public override void OnCreatedRoom()
    {
        Debug.Log("Room created");
    }

    // �����������i���ʁj
    public override void OnJoinedRoom()
    {
        Debug.Log("Joined room: " + PhotonNetwork.CurrentRoom.Name);
        //SceneManager.LoadScene("RoomScene");
    }

    // �G���[�����i�C�Ӂj
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogError("Create failed: " + message);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.LogError("Join failed: " + message);
    }

}