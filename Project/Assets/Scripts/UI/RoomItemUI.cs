using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using Photon.Pun;

public class RoomItemUI : MonoBehaviour
{
    [SerializeField] private Text roomNameText;

    private RoomInfo roomInfo;

    public void SetUp(RoomInfo info)
    {
        roomInfo = info;
        roomNameText.text = info.Name;
    }
}
