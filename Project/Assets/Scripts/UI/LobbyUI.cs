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

    [SerializeField] private TMP_InputField hostPasswordInput;


    void Start()
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings(); //�T�[�o�[�ڑ�
        }
    }

    public void OnClickCreateRoom()
    {
        string password = hostPasswordInput.text.Trim();

        if (!string.IsNullOrEmpty(password) && !string.IsNullOrEmpty(PhotonNetwork.NickName))
        {
            RoomManager.Instance.CreateRoom("MyRoom123", password); // �C�ӂ̕�����

        }
        else
        {
            Debug.LogWarning("���[�U�[���������͂ł��B");
        }

    }

    public void OnClickJoinRoom()
    {
        string password = passwordInput.text.Trim();

        if (!string.IsNullOrEmpty(password) && !string.IsNullOrEmpty(PhotonNetwork.NickName))
        {
            RoomManager.Instance.JoinRoom("MyRoom123", password);
        }
        else
        {
            Debug.LogWarning("���[�U�[���܂��̓p�X���[�h�������͂ł��B");
        }
    }






}
