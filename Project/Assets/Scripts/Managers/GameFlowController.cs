using UnityEngine;
using Photon;
using Photon.Pun;
using ExitGames.Client.Photon;
using System;
using System.Collections.Generic;
using UnityEngine.InputSystem.XR;

public class GameFlowController : MonoBehaviourPunCallbacks
{
    private RoleDistributionPhase distribution;
    private QuestionPhase question;
    private AnswerPhase playerAnswer;
    private VotePhase vote;
    private ResultPhase result;

    private Dictionary<GameState, GameStateBehaviour> gameStateDict;

    private void Awake()
    {
        distribution = GetComponent<RoleDistributionPhase>();
        question = GetComponent<QuestionPhase>();
        playerAnswer = GetComponent<AnswerPhase>();
        vote = GetComponent<VotePhase>();
        result = GetComponent<ResultPhase>();
    }

    private void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Initialize();
        }
    }
    public void Initialize()
    {
        // ゲームステートを Room のカスタムプロパティに設定
        Hashtable props = new Hashtable();
        props["GameState"] = GameState.ROLE_DISTRIBUTION;
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
            {GameState.ROLE_DISTRIBUTION, distribution },
            {GameState.QUESTION, question },
            {GameState.ANSWER, playerAnswer },
            {GameState.VOTE, vote },
            {GameState.RESULT, result },
        };
    }
}
