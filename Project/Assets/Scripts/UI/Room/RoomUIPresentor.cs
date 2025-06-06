using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Realtime;
using TMPro;
using Photon.Pun;

public class RoomUIPresentor : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private Transform elementsParent;
    private PlayerViewElement[] elements;
    [SerializeField] private TMP_Text passwordText;
    [SerializeField] private GameObject masterButtonsOb;
    [SerializeField] private GameObject clientButtonsOb;


    private Player[] currentPlayers;
    private const float time = 0.2f;

    private void Awake()
    {
        nameText.text = PhotonNetwork.CurrentRoom.Name;
        passwordText.text = $"パスワード:{RoomManager.Instance.password}";
        elements = elementsParent.GetComponentsInChildren<PlayerViewElement>(true);
        GeneratePlayerTable();

        if (PhotonNetwork.IsMasterClient)
        {
            ShowMasterButtons();
        }
        else
        {
            ShowClientButtons();
        }

        StartCoroutine(UpdateCoroutine());
    }

    private IEnumerator UpdateCoroutine()
    {
        WaitForSeconds wait = new WaitForSeconds(time);

        while (true)
        {
            yield return wait;
            if (currentPlayers == null) continue;

            if (currentPlayers.SequenceEqual(PhotonNetwork.PlayerList))
            {
                GeneratePlayerTable();
            }
        }
    }

    public void GeneratePlayerTable()
    {
        Player[] playerList = PhotonNetwork.PlayerList;
        currentPlayers = playerList;

        playerList.OrderByDescending(x => x.IsMasterClient);

        for (int i = 0; i < elements.Length; i++)
        {
            if (i < playerList.Length)
            {
                Player player = playerList[i];
                elements[i].SetView(player.NickName);
                elements[i].gameObject.SetActive(true);
            }
            else
            {
                elements[i].gameObject.SetActive(false);
            }
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        GeneratePlayerTable();
        base.OnPlayerEnteredRoom(newPlayer);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        GeneratePlayerTable();
        base.OnPlayerLeftRoom(otherPlayer);
    }

    public void ShowMasterButtons()
    {
        masterButtonsOb.SetActive(true);
        clientButtonsOb.SetActive(false);
    }

    public void ShowClientButtons()
    {
        masterButtonsOb.SetActive(false);
        clientButtonsOb.SetActive(true);
    }

    public void OnClickStart()
    {
        SceneController sceneController = FindAnyObjectByType<SceneController>();
        sceneController.GoToGame();
    }

    public void OnClickInterupt()
    {
        photonView.RPC("ForceLeaveRoom", RpcTarget.All);
    }

    public void OnClickExit()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel("LobbyScene2");
    }

    [PunRPC]
    private void ForceLeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel("LobbyScene2");
    }
}
