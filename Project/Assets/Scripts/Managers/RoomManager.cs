using Photon.Pun;
using Photon.Pun.Demo.Cockpit;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq; // �����񑀍�ɕ֗�

public class RoomManager : MonoBehaviourPunCallbacks
{
    public static RoomManager Instance; //RoomManager.Instance�łǂ�����ł��A�N�Z�X�\�ɂȂ�
    private string tempEnteredPassword; // �ꎞ�I�ɕۑ����Ă���

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
        roomOptions.IsVisible = true;
        Debug.Log("������:"+roomName);

        // �J�X�^���v���p�e�B�Ƀp�X���[�h��ǉ�
        ExitGames.Client.Photon.Hashtable customProperties = new ExitGames.Client.Photon.Hashtable();
        customProperties["password"] = password;
        roomOptions.CustomRoomProperties = customProperties;

        // �O�������������悤�ɂ���i���r�[�\���̂��߁j
        roomOptions.CustomRoomPropertiesForLobby = new string[] { "password" };

        PhotonNetwork.CreateRoom(roomName, roomOptions, TypedLobby.Default);
    }

    // �p�X���[�h���ƍ����ĕ����ɎQ���i�Q�X�g���j
    public void JoinRoom(string roomName, string enteredPassword)
    {
        tempEnteredPassword = enteredPassword;
        PhotonNetwork.JoinRoom(roomName);
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("�}�X�^�[�T�[�o�[�ɐڑ�����܂���");
        PhotonNetwork.JoinLobby(); // ���r�[�ɓ���
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        if (roomList.Count == 0)
        {
            Debug.Log("���݁A���p�\�ȕ����͂���܂���B");
            return;
        }

        Debug.Log($"���݂̕�����: {roomList.Count}");

        foreach (RoomInfo room in roomList)
        {
            // ���ۂɂ͂����� cachedRoomList ���X�V����
            if (room.RemovedFromList)
            {
                Debug.Log($"���[�� '{room.Name}' ���폜����܂����B");
            }
            else
            {
                string roomInfo = $"������: {room.Name}, " +
                                  $"�Q���l��: {room.PlayerCount}/{room.MaxPlayers}";

                if (room.CustomProperties.TryGetValue("password", out object passwordObj))
                {
                    // ���r�[�ł͎��ۂ̃p�X���[�h��\���������ɁA�p�X���[�h�̗L��������������ʓI�ł��B
                    // roomInfo += $", �p�X���[�h�ݒ�: {(string.IsNullOrEmpty((string)passwordObj) ? "�Ȃ�" : "����")}";
                    // �f�o�b�O�p�Ɏ��ۂ̃p�X���[�h��\������ꍇ�͂��̂܂܂ł�OK
                    roomInfo += $", �p�X���[�h: {(string)passwordObj}";
                }
                Debug.Log(roomInfo);
            }
        }
    }


    // �쐬������
    public override void OnCreatedRoom()
    {
        Debug.Log("Room created: " + PhotonNetwork.CurrentRoom.Name);
        // IsHost = true; // �K�v�ł���΂����Ńz�X�g�t���O��ݒ�
        // RoomName = PhotonNetwork.CurrentRoom.Name; // �K�v�ł���Ε�������ݒ�
        Debug.Log("�z�X�g�Ƃ��ă��[�����쐬���܂����BRoomScene�ɑJ�ڂ��܂��B");
        PhotonNetwork.LoadLevel("RoomScene"); // PhotonNetwork.LoadLevel �𐄏�
    }

    // �����������i���ʁj
    public override void OnJoinedRoom()
    {
        Debug.Log("Joined room: " + PhotonNetwork.CurrentRoom.Name);
        // RoomName = PhotonNetwork.CurrentRoom.Name; // �K�v�ł���Ε�������ݒ�

        // �}�X�^�[�N���C�A���g�i�z�X�g�j�łȂ��ꍇ�̂݃p�X���[�h�ƍ����s��
        if (!PhotonNetwork.IsMasterClient)
        {
            // IsHost = false; // �K�v�ł���΂����ŃQ�X�g�t���O��ݒ�
            Debug.Log("�Q�X�g�Ƃ��ă��[���ɎQ�����܂����B�p�X���[�h���m�F���܂��B");
            if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("password", out object roomPasswordObj))
            {
                string actualRoomPassword = roomPasswordObj as string;

                // ��̃p�X���[�h "" �� null �𓯈ꎋ����ꍇ��A��茵���Ȕ�r���K�v�ȏꍇ�͒������Ă��������B
                if (!string.IsNullOrEmpty(actualRoomPassword)) // �p�X���[�h���ݒ肳��Ă��镔���̏ꍇ
                {
                    if (actualRoomPassword == tempEnteredPassword)
                    {
                        Debug.Log("�p�X���[�h����v���܂����BRoomScene�ɑJ�ڂ��܂��B");
                        PhotonNetwork.LoadLevel("RoomScene"); // PhotonNetwork.LoadLevel �𐄏�
                    }
                    else
                    {
                        Debug.LogWarning($"�p�X���[�h����v���܂���B���͂��ꂽ�p�X���[�h: '{tempEnteredPassword}', ���ۂ̃p�X���[�h: '{actualRoomPassword}'�B�ގ����܂��B");
                        PhotonNetwork.LeaveRoom();
                        // �K�v�ł����UI�ŃG���[���b�Z�[�W��\��
                        // UIManager.Instance.ShowMessage("�p�X���[�h���Ԉ���Ă��܂��B");
                    }
                }
                else // �p�X���[�h���ݒ肳��Ă��Ȃ������̏ꍇ�i��{�I�ɂ͂��肦�Ȃ����O�̂��߁j
                {
                    Debug.Log("���̕����ɂ̓p�X���[�h���ݒ肳��Ă��܂��񂪁A�p�X���[�h�Ȃ��ŎQ���������p�����܂��BRoomScene�ɑJ�ڂ��܂��B");
                    PhotonNetwork.LoadLevel("RoomScene");
                }
            }
            else // ���[���� "password" �v���p�e�B�����݂��Ȃ��ꍇ (�p�X���[�h�Ȃ����[��)
            {
                Debug.Log("���̕����̓p�X���[�h�v���p�e�B���ݒ肳��Ă��܂���i�p�X���[�h�Ȃ��j�BRoomScene�ɑJ�ڂ��܂��B");
                // �p�X���[�h�Ȃ��̕����ւ̎Q����������ꍇ
                PhotonNetwork.LoadLevel("RoomScene");
            }
        }
        else // �}�X�^�[�N���C�A���g�i�z�X�g�j�̏ꍇ
        {
            Debug.Log("�z�X�g�Ƃ��ă��[���ɎQ���������������܂����B");
            // �z�X�g�� OnCreatedRoom �Ŋ��ɃV�[���J�ڂ��Ă���̂ŁA�����ł͉������Ȃ����A
            // ���� OnCreatedRoom �ŃV�[���J�ڂ��Ȃ��݌v�ɂ���ꍇ�͂����ōs���B
            // ���݂̃R�[�h�ł� OnCreatedRoom �őJ�ڂ��Ă���̂ŁA�����ł͒ǉ��̑J�ڂ͕s�v�B
        }
        tempEnteredPassword = null; // �ꎞ�p�X���[�h���N���A
    }

    public override void OnLeftRoom()
    {
        Debug.Log("������ޏo���܂���");
    }


    // �G���[�����i�C�Ӂj
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogError("Create failed: " + message);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        // �G���[�R�[�h�Ɋ�Â����ڍׂȃt�B�[�h�o�b�N
        string errorMessage = $"���[���ւ̎Q���Ɏ��s���܂��� ({returnCode}): {message}";
        if (returnCode == ErrorCode.GameDoesNotExist)
        {
            errorMessage = "�w�肳�ꂽ�����͑��݂��܂���B";
        }
        else if (returnCode == ErrorCode.GameFull)
        {
            errorMessage = "�����������ł��B";
        }
        else if (returnCode == ErrorCode.GameClosed)
        {
            errorMessage = "�����͊��ɕ����Ă��܂��B";
        }
        // �J�X�^���F�؃G���[�Ȃǂ��l���ɓ����Ɨǂ��ł��傤 (PUN�ł͂��܂��ʓI�ł͂Ȃ���)

        Debug.LogWarning(errorMessage);
        tempEnteredPassword = null; // ���s�����̂ňꎞ�p�X���[�h���N���A
        // UIManager.Instance.ShowMessage(errorMessage);
    }

}