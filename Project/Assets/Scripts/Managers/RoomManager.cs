using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public static RoomManager Instance; //RoomManager.Instance�łǂ�����ł��A�N�Z�X�\�ɂȂ�

    public string RoomID { get; private set; }
    public bool IsHost { get; private set; }

    public List<PlayerInfo> Players = new List<PlayerInfo>();  //�v���C���[�̃��X�g

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
    public void CreateRoom()
    {
        RoomID = GenerateRandomRoomID();
        IsHost = true;
        Players.Clear();
        AddPlayer("You", true);  // �������z�X�g�Ƃ��Ēǉ�
    }

    // �����ɎQ���i�Q�X�g���j
    public bool JoinRoom(string roomId)
    {
        // �����ł̓��[�J���ɂ������݂��Ȃ����߁A�ʐM�ł̑��݊m�F�͕ʓr�K�v
        RoomID = roomId;
        IsHost = false;
        Players.Clear();
        AddPlayer("You", false);
        return true;
    }

    // �v���C���[��ǉ�����֐�
    public void AddPlayer(string name, bool isHost)
    {
        PlayerInfo newPlayer = new PlayerInfo
        {
            Name = name,
            IsHost = isHost
        };
        Players.Add(newPlayer);
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
}

[System.Serializable]
public class PlayerInfo
{
    public string Name;
    public bool IsHost;
}
