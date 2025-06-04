using UnityEngine;
using Photon;
using Photon.Pun;
using ExitGames.Client.Photon;
using System;
using System.Collections.Generic;

public enum GameState
{
    JOB_DISTRIBUTION,
    QUESTION,
    ANSWER,
    VOTE,
    RESULT,
}


public class GameFlowController : MonoBehaviourPunCallbacks
{
    private JobDistribution distribution;
    private Question question;
    private PlayerAnswer playerAnswer;
    private Vote vote;
    private ResultView result;

    private Dictionary<GameState, GameStateBehaviour> gameStateDict = new();

    private void Awake()
    {
        distribution = GetComponent<JobDistribution>();
        question = GetComponent<Question>();
        playerAnswer = GetComponent<PlayerAnswer>();
        vote = GetComponent<Vote>();
        result = GetComponent<ResultView>();

        gameStateDict = new Dictionary<GameState, GameStateBehaviour>()
        {
            {GameState.JOB_DISTRIBUTION, distribution },
            {GameState.QUESTION, question },
            {GameState.ANSWER, playerAnswer },
            {GameState.VOTE, vote },
            {GameState.RESULT, result },

        };
    }

    public void Initialize()
    {
        // ゲームステートを Room のカスタムプロパティに設定
        Hashtable props = new Hashtable();
        props["GameState"] = GameState.JOB_DISTRIBUTION;
        PhotonNetwork.CurrentRoom.SetCustomProperties(props);
    }

    public void SetRoomState(GameState newState)
    {
        var props = new Hashtable { { "GameState", newState } };
        PhotonNetwork.CurrentRoom.SetCustomProperties(props);
    }

    public override void OnRoomPropertiesUpdate(Hashtable changedProps)
    {
        if (changedProps.TryGetValue("GameState", out object stateValue))
        {
            gameStateDict[(GameState)stateValue].Enter();
        }
    }
}
