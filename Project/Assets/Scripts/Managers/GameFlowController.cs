using UnityEngine;
using Photon;
using Photon.Pun;
using ExitGames.Client.Photon;
using System;
using System.Collections.Generic;

public class GameFlowController : MonoBehaviourPunCallbacks
{
    private JobDistribution distribution;
    private Question question;
    private PlayerAnswer playerAnswer;
    private Vote vote;
    private ResultView result;

    private Dictionary<GameState, GameStateBehaviour> gameStateDict;

    private void Awake()
    {
        distribution = GetComponent<JobDistribution>();
        question = GetComponent<Question>();
        playerAnswer = GetComponent<PlayerAnswer>();
        vote = GetComponent<Vote>();
        result = GetComponent<ResultView>();
    }

    public void Initialize()
    {
        // ゲームステートを Room のカスタムプロパティに設定
        Hashtable props = new Hashtable();
        props["GameState"] = GameState.JOB_DISTRIBUTION;
        props["AnswerCount"] = 0;
        PhotonNetwork.CurrentRoom.SetCustomProperties(props);
    }

    public void SetRoomState(GameState newState)
    {
        var props = new Hashtable { { "GameState", newState } };
        PhotonNetwork.CurrentRoom.SetCustomProperties(props);
    }

    //クライアント全員がカスタムプロパティを観測し、状態が変わったら該当GameStateに移動
    public override void OnRoomPropertiesUpdate(Hashtable changedProps)
    {
        if (changedProps.TryGetValue("GameState", out object stateValue))
        {
            if (gameStateDict == null)
            {
                SetDictionary();
            }

            if (gameStateDict.ContainsKey((GameState)stateValue))
            {
                gameStateDict[(GameState)stateValue].Enter();
            }
        }
    }

    public void SetDictionary()
    {
        gameStateDict = new Dictionary<GameState, GameStateBehaviour>()
        {
            {GameState.JOB_DISTRIBUTION, distribution },
            {GameState.QUESTION, question },
            {GameState.ANSWER, playerAnswer },
            {GameState.VOTE, vote },
            {GameState.RESULT, result },
        };
    }
}
