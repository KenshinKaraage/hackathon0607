using Photon.Pun;
using TMPro;
using UnityEngine;
using Cysharp.Threading.Tasks;
using ExitGames.Client.Photon;
using Test;
using System.Linq;

public class PlayerAnswer : GameStateBehaviour
{
    public bool IsAnserwred { get; set; }

    public override void Enter()
    {
        string question = PhotonNetwork.CurrentRoom.CustomProperties["Question"] is string value ? value : "";

        UIPresenter_Header header = FindAnyObjectByType<UIPresenter_Header>();
        header.SetView("回答");

        UIPresenter_Body body = FindAnyObjectByType<UIPresenter_Body>();
        UIPresenter_Footer footer = FindAnyObjectByType<UIPresenter_Footer>();

        if (PhotonNetwork.IsMasterClient)
        {
            APIQuestionSender sender = FindAnyObjectByType<APIQuestionSender>();
            sender.ResetAIAnswer();
            sender.Send(question);
        }

        PlayerCharacterList characterList = FindAnyObjectByType<PlayerCharacterList>();
        Role role = characterList.GetLocalPlayerCharacter().Job;
        if (role == Role.Werewolf)
        {
            body.ShowQuestion(question);
            footer.ShowInput();

            characterList.GetLocalPlayerCharacter().Answer = "";
            characterList.GetLocalPlayerCharacter().IsAnswered = false;
        }
        else
        {
            body.ShowQuestion(question);
            footer.ShowFooterText("回答を入力中・・・");
        }

        CharacterDataList characterDataList = FindAnyObjectByType<CharacterDataList>();
        IPlayerCharacter[] answeringCharacters = characterList.Characters.Where(x => x.IsAlive && x.Job != Role.Representative).ToArray();
        body.ShowAnswersThinking();
    }

    public void Submit()
    {
        GameState currentState = (PhotonNetwork.CurrentRoom.CustomProperties["GameState"] is int value) ? (GameState)value : GameState.JOB_DISTRIBUTION;
        if (currentState != GameState.ANSWER) return;

        UIPresenter_Footer footer = FindAnyObjectByType<UIPresenter_Footer>();
        string answer = footer.GetInputFieldText();

        if (string.IsNullOrEmpty(answer)) return;

        PlayerCharacterList characterList = FindAnyObjectByType<PlayerCharacterList>();
        characterList.GetLocalPlayerCharacter().Answer = answer;
        characterList.GetLocalPlayerCharacter().IsAnswered = true;

        footer.ShowFooterText("全員の回答を待機しています");
    }
}
