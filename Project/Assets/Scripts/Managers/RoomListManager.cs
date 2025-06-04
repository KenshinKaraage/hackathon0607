using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class RoomListManager : MonoBehaviourPunCallbacks
{
    public static RoomListManager Instance;
    private List<RoomInfo> cachedRoomList = new List<RoomInfo>();

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        PhotonNetwork.JoinLobby(); // ロビーに参加してルームリストを取得
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        cachedRoomList.Clear();
        foreach (RoomInfo room in roomList)
        {
            if (!room.RemovedFromList)
                cachedRoomList.Add(room);
        }

        Debug.Log("取得したルーム数: " + cachedRoomList.Count);
    }

    public List<RoomInfo> GetRoomList()
    {
        return new List<RoomInfo>(cachedRoomList);
    }
}