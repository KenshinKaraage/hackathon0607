using System.Collections.Generic;
using UnityEngine;

public class LobbyUI : MonoBehaviour
{
    public TMPro.TextMeshProUGUI roomCodeText;

    public void OnClickCreateRoom()
    {
        RoomManager.Instance.CreateRoom();
        roomCodeText.text = RoomManager.Instance.RoomID;
    }

    public void OnClickJoinRoom(string inputCode)
    {
        bool joined = RoomManager.Instance.JoinRoom(inputCode);
        if (joined)
            Debug.Log("Joined room: " + inputCode);
        else
            Debug.LogWarning("Join failed.");
    }
}
