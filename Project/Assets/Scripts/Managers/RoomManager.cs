using Photon.Pun;
using Photon.Pun.Demo.Cockpit;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomManager : MonoBehaviourPunCallbacks
{
    public static RoomManager Instance; //RoomManager.Instanceでどこからでもアクセス可能になる


    public string RoomName { get; private set; } //部屋ID
    public bool IsHost { get; private set; }  //ホストかどうか


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
    public void CreateRoom(string roomName, string password)
    {
        var roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 4;

        // カスタムプロパティにパスワードを追加
        ExitGames.Client.Photon.Hashtable customProperties = new ExitGames.Client.Photon.Hashtable();
        customProperties["password"] = password;
        roomOptions.CustomRoomProperties = customProperties;

        // 外部からも見えるようにする（ロビー表示のため）
        roomOptions.CustomRoomPropertiesForLobby = new string[] { "password" };

        PhotonNetwork.CreateRoom(roomName, roomOptions, TypedLobby.Default);
    }

    // 部屋に参加（ゲスト側）
    public void JoinRoom(string roomName, string enteredPassword)
    {
        // 部屋一覧を取得して照合
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
                        Debug.LogWarning("パスワードが違います！");
                        return;
                    }
                }
            }
        }

        Debug.LogWarning("指定された部屋が見つかりませんでした");
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

    // 作成成功時
    public override void OnCreatedRoom()
    {
        Debug.Log("Room created");
    }

    // 入室成功時（共通）
    public override void OnJoinedRoom()
    {
        Debug.Log("Joined room: " + PhotonNetwork.CurrentRoom.Name);
        //SceneManager.LoadScene("RoomScene");
    }

    // エラー処理（任意）
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogError("Create failed: " + message);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.LogError("Join failed: " + message);
    }

}