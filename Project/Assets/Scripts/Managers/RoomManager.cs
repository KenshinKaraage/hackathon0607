using Photon.Pun;
using Photon.Pun.Demo.Cockpit;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq; // 文字列操作に便利

public class RoomManager : MonoBehaviourPunCallbacks
{
    public static RoomManager Instance; //RoomManager.Instanceでどこからでもアクセス可能になる
    private string tempEnteredPassword; // 一時的に保存しておく

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
        roomOptions.IsVisible = true;
        Debug.Log("部屋名:" + roomName);

        // カスタムプロパティにパスワードを追加
        ExitGames.Client.Photon.Hashtable customProperties = new ExitGames.Client.Photon.Hashtable();
        customProperties["password"] = password;
        roomOptions.CustomRoomProperties = customProperties;

        // 外部からも見えるようにする（ロビー表示のため）
        roomOptions.CustomRoomPropertiesForLobby = new string[] { "password" };

        PhotonNetwork.CreateRoom(roomName, roomOptions, TypedLobby.Default);
    }

    // パスワードを照合して部屋に参加（ゲスト側）
    public void JoinRoom(string roomName, string enteredPassword)
    {
        tempEnteredPassword = enteredPassword;
        PhotonNetwork.JoinRoom(roomName);
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("マスターサーバーに接続されました");
        PhotonNetwork.JoinLobby(); // ロビーに入る
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        if (roomList.Count == 0)
        {
            Debug.Log("現在、利用可能な部屋はありません。");
            return;
        }

        Debug.Log($"現在の部屋数: {roomList.Count}");

        foreach (RoomInfo room in roomList)
        {
            // 実際にはここで cachedRoomList を更新する
            if (room.RemovedFromList)
            {
                Debug.Log($"ルーム '{room.Name}' が削除されました。");
            }
            else
            {
                string roomInfo = $"部屋名: {room.Name}, " +
                                  $"参加人数: {room.PlayerCount}/{room.MaxPlayers}";

                if (room.CustomProperties.TryGetValue("password", out object passwordObj))
                {
                    // ロビーでは実際のパスワードを表示する代わりに、パスワードの有無を示す方が一般的です。
                    // roomInfo += $", パスワード設定: {(string.IsNullOrEmpty((string)passwordObj) ? "なし" : "あり")}";
                    // デバッグ用に実際のパスワードを表示する場合はこのままでもOK
                    roomInfo += $", パスワード: {(string)passwordObj}";
                }
                Debug.Log(roomInfo);
            }
        }
    }


    // 作成成功時
    public override void OnCreatedRoom()
    {
        Debug.Log("Room created: " + PhotonNetwork.CurrentRoom.Name);
        // IsHost = true; // 必要であればここでホストフラグを設定
        // RoomName = PhotonNetwork.CurrentRoom.Name; // 必要であれば部屋名を設定
        Debug.Log("ホストとしてルームを作成しました。RoomSceneに遷移します。");
        PhotonNetwork.LoadLevel("RoomScene"); // PhotonNetwork.LoadLevel を推奨
    }

    // 入室成功時（共通）
    public override void OnJoinedRoom()
    {
        Debug.Log("Joined room: " + PhotonNetwork.CurrentRoom.Name);
        // RoomName = PhotonNetwork.CurrentRoom.Name; // 必要であれば部屋名を設定

        // マスタークライアント（ホスト）でない場合のみパスワード照合を行う
        if (!PhotonNetwork.IsMasterClient)
        {
            // IsHost = false; // 必要であればここでゲストフラグを設定
            Debug.Log("ゲストとしてルームに参加しました。パスワードを確認します。");
            if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("password", out object roomPasswordObj))
            {
                string actualRoomPassword = roomPasswordObj as string;

                // 空のパスワード "" と null を同一視する場合や、より厳密な比較が必要な場合は調整してください。
                if (!string.IsNullOrEmpty(actualRoomPassword)) // パスワードが設定されている部屋の場合
                {
                    if (actualRoomPassword == tempEnteredPassword)
                    {
                        Debug.Log("パスワードが一致しました。RoomSceneに遷移します。");
                        PhotonNetwork.LoadLevel("RoomScene"); // PhotonNetwork.LoadLevel を推奨
                    }
                    else
                    {
                        Debug.LogWarning($"パスワードが一致しません。入力されたパスワード: '{tempEnteredPassword}', 実際のパスワード: '{actualRoomPassword}'。退室します。");
                        PhotonNetwork.LeaveRoom();
                        // 必要であればUIでエラーメッセージを表示
                        // UIManager.Instance.ShowMessage("パスワードが間違っています。");
                    }
                }
                else // パスワードが設定されていない部屋の場合（基本的にはありえないが念のため）
                {
                    Debug.Log("この部屋にはパスワードが設定されていませんが、パスワードなしで参加処理を継続します。RoomSceneに遷移します。");
                    PhotonNetwork.LoadLevel("RoomScene");
                }
            }
            else // ルームに "password" プロパティが存在しない場合 (パスワードなしルーム)
            {
                Debug.Log("この部屋はパスワードプロパティが設定されていません（パスワードなし）。RoomSceneに遷移します。");
                // パスワードなしの部屋への参加を許可する場合
                PhotonNetwork.LoadLevel("RoomScene");
            }
        }
        else // マスタークライアント（ホスト）の場合
        {
            Debug.Log("ホストとしてルームに参加処理を完了しました。");
            // ホストは OnCreatedRoom で既にシーン遷移しているので、ここでは何もしないか、
            // もし OnCreatedRoom でシーン遷移しない設計にする場合はここで行う。
            // 現在のコードでは OnCreatedRoom で遷移しているので、ここでは追加の遷移は不要。
        }
        tempEnteredPassword = null; // 一時パスワードをクリア
    }

    public override void OnLeftRoom()
    {
        Debug.Log("部屋を退出しました");
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
    }

    // エラー処理（任意）
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogError("Create failed: " + message);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        // エラーコードに基づいた詳細なフィードバック
        string errorMessage = $"ルームへの参加に失敗しました ({returnCode}): {message}";
        if (returnCode == ErrorCode.GameDoesNotExist)
        {
            errorMessage = "指定された部屋は存在しません。";
        }
        else if (returnCode == ErrorCode.GameFull)
        {
            errorMessage = "部屋が満員です。";
        }
        else if (returnCode == ErrorCode.GameClosed)
        {
            errorMessage = "部屋は既に閉じられています。";
        }
        // カスタム認証エラーなども考慮に入れると良いでしょう (PUNではあまり一般的ではないが)

        Debug.LogWarning(errorMessage);
        tempEnteredPassword = null; // 失敗したので一時パスワードをクリア
        // UIManager.Instance.ShowMessage(errorMessage);
    }

}