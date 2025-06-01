using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public static RoomManager Instance; //RoomManager.Instanceでどこからでもアクセス可能になる

    public string RoomID { get; private set; }
    public bool IsHost { get; private set; }

    public List<PlayerInfo> Players = new List<PlayerInfo>();  //プレイヤーのリスト

    //Instanceの初期化
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {//重複生成を防ぐために破棄する
            Destroy(gameObject);
        }
    }

    // 部屋を作成（ホスト側）
    public void CreateRoom()
    {
        RoomID = GenerateRandomRoomID();
        IsHost = true;
        Players.Clear();
        AddPlayer("You", true);  // 自分をホストとして追加
    }

    // 部屋に参加（ゲスト側）
    public bool JoinRoom(string roomId)
    {
        // ここではローカルにしか存在しないため、通信での存在確認は別途必要
        RoomID = roomId;
        IsHost = false;
        Players.Clear();
        AddPlayer("You", false);
        return true;
    }

    // プレイヤーを追加する関数
    public void AddPlayer(string name, bool isHost)
    {
        PlayerInfo newPlayer = new PlayerInfo
        {
            Name = name,
            IsHost = isHost
        };
        Players.Add(newPlayer);
    }

    // ランダムなRoomIDを生成する関数
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
