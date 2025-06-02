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

    void Start()
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings(); //�T�[�o�[�ڑ�
        }
    }

    public void OnClickCreateRoom()
    {
        RoomManager.Instance.CreateRoom("MyRoom123", "aaa"); // �C�ӂ̕�����
        SceneManager.LoadScene("RoomScene");
    }

    public void OnClickJoinRoom()
    {
        string password = passwordInput.text.Trim();

        if (!string.IsNullOrEmpty(password))
        {
            RoomManager.Instance.JoinRoom("MyRoom123", password);
            SceneManager.LoadScene("RoomScene");
        }
        else
        {SceneManager.LoadScene("RoomScene");
            Debug.LogWarning("�������܂��̓p�X���[�h�������͂ł��B");
        }
    }






}
