using Cysharp.Threading.Tasks;
using Photon.Pun;
using TMPro;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class QuestionPhase : GameStateBehaviour
{
    private QuestionPhaseView view;

    public string playerQuestion { get; private set; }

    private void Start()
    {
        view = FindAnyObjectByType<QuestionPhaseView>();
    }

    public override void Enter()
    {
        UIPresenter_Header header = FindAnyObjectByType<UIPresenter_Header>();
        int answerCount = PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("AnswerCount", out object count) ? (int)count : 0;
        header.SetView($"質問({answerCount + 1}/{AnswerWaiter.MAXANSWERCOUNT})");

        UIPresenter_Body body = FindAnyObjectByType<UIPresenter_Body>();

        PlayerCharacterList characterList = FindAnyObjectByType<PlayerCharacterList>();
        Role role = characterList.GetLocalPlayerCharacter().Job;
        CharacterDataList characterDataList = FindAnyObjectByType<CharacterDataList>();
        IPlayerCharacter representative = characterList.Characters.Where(x => x.Job == Role.Representative).First();

        List<IPlayerCharacter> answeringCharacters = characterList.Characters.Where(x => x.IsAlive && x.Job != Role.Representative).ToList();

        view.Show(representative, answeringCharacters, role == Role.Representative);
    }

    public void Submit()
    {
        GameState currentState = (PhotonNetwork.CurrentRoom.CustomProperties["GameState"] is int value) ? (GameState)value : GameState.ROLE_DISTRIBUTION;
        if (currentState != GameState.QUESTION) return;

        string question = view.GetQuestion();

        if (string.IsNullOrEmpty(question)) return;

        var resultProps = new ExitGames.Client.Photon.Hashtable();
        resultProps["Question"] = question;
        PhotonNetwork.CurrentRoom.SetCustomProperties(resultProps);
    }
}

