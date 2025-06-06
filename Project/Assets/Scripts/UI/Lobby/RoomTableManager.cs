using UnityEngine;
using System.Collections;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using System.Linq;

public class RoomTableManager : MonoBehaviourPunCallbacks
{
    public string selectedRoomName { get; set; }
    [SerializeField] private Transform elementsParent;
    private RoomElement[] elements;
    private const float time = 0.2f;
    List<RoomInfo> currentRoomInfos = new();
    private void Awake()
    {
        elements = elementsParent.GetComponentsInChildren<RoomElement>(true);
        StartCoroutine(UpdateCoroutine());
    }

    private IEnumerator UpdateCoroutine()
    {
        WaitForSeconds wait = new WaitForSeconds(time);
        while (true)
        {
            yield return wait;
            if (currentRoomInfos.SequenceEqual(RoomManager.Instance.currentRoomList))
            {
                GeenrateRoomTable();
            }
        }
    }

    public void GeenrateRoomTable()
    {
        List<RoomInfo> roomList = RoomManager.Instance.currentRoomList;
        currentRoomInfos = roomList;

        for (int i = 0; i < elements.Length; i++)
        {
            if (i < roomList.Count)
            {
                RoomInfo info = roomList[i];
                elements[i].SetView(info.Name, info.PlayerCount, info.MaxPlayers);
                elements[i].SetButtonEvent(info.Name);
                elements[i].gameObject.SetActive(true);
            }
            else
            {
                elements[i].gameObject.SetActive(false);
            }
        }
    }

    public void OnClickBack()
    {
        LobbyUIPresentor presentor = FindAnyObjectByType<LobbyUIPresentor>();
        presentor.ShowNameInput();
    }
}
