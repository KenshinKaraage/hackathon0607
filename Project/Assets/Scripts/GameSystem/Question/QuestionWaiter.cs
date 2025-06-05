using ExitGames.Client.Photon;
using System.Linq;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class QuestionWaiter : MonoBehaviourPunCallbacks
{
    public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {
        Debug.Log("changed");
        GameState currentState = (PhotonNetwork.CurrentRoom.CustomProperties["GameState"] is int value) ? (GameState)value : GameState.JOB_DISTRIBUTION;
        Debug.Log(currentState);
        if (currentState != GameState.QUESTION) return;
        Debug.Log("inQuestion");
        if (!propertiesThatChanged.TryGetValue("Question", out object question)) return;
        Debug.Log("QuestionChanged");
        if (!PhotonNetwork.IsMasterClient) return;
        Debug.Log("IsMasterClient");

        //質問が送られたら、回答フェーズへ
        GameFlowController controller = FindAnyObjectByType<GameFlowController>();
        controller.SetRoomState(GameState.ANSWER);
    }
}
