using Photon.Pun;
using TMPro;
using UnityEngine;
using Cysharp.Threading.Tasks;
using ExitGames.Client.Photon;
using Test;
using System.Linq;

public class AnswerPhase : GameStateBehaviour
{
    private AnswerPhaseView answerPhaseView;

    private void Start()
    {
        answerPhaseView = FindAnyObjectByType<AnswerPhaseView>();
    }

    public override void Enter()
    {
        string question = PhotonNetwork.CurrentRoom.CustomProperties["Question"] is string value ? value : "";

        PlayerCharacterList characterList = FindAnyObjectByType<PlayerCharacterList>();

        //全プレイヤーの回答状況をリセット
        foreach (var player in characterList.Characters)
        {
            if (player.IsNPC)
            {
                player.IsAnswered = false;
            }
            else
            {
                if (player == characterList.GetLocalPlayerCharacter())
                {
                    player.Answer = "";
                    player.IsAnswered = false;
                }
            }
        }

        Role role = characterList.GetLocalPlayerCharacter().Job;
        bool isAnswerer = role == Role.Werewolf;
        answerPhaseView.ShowQuestion(question, isAnswerer);

        if (PhotonNetwork.IsMasterClient)
        {
            APIQuestionSender sender = FindAnyObjectByType<APIQuestionSender>();
            sender.ResetAIAnswer();
            sender.Send(question);
        }
    }

    public void Submit()
    {
        GameState currentState = (PhotonNetwork.CurrentRoom.CustomProperties["GameState"] is int value) ? (GameState)value : GameState.ROLE_DISTRIBUTION;
        if (currentState != GameState.ANSWER) return;

        string answer = answerPhaseView.GetAnswer();

        if (string.IsNullOrEmpty(answer)) return;

        PlayerCharacterList characterList = FindAnyObjectByType<PlayerCharacterList>();
        characterList.GetLocalPlayerCharacter().Answer = answer;
        characterList.GetLocalPlayerCharacter().IsAnswered = true;

        answerPhaseView.WaitForAllAnsweres();
    }
}
