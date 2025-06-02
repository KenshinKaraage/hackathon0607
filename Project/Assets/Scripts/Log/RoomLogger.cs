using TMPro;
using Photon.Pun;

public class RoomLogger : MonoBehaviourPunCallbacks
{
    public TextMeshProUGUI logText;

    public override void OnJoinedRoom()
    {
        string nickname = PhotonNetwork.NickName;
        string roomName = PhotonNetwork.CurrentRoom.Name;
        logText.text += $"{nickname} joined room \"{roomName}\"\n";
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        logText.text += $"{newPlayer.NickName} has joined the room.\n";
    }
}
