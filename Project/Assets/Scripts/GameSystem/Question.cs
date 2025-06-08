using Cysharp.Threading.Tasks;
using Photon.Pun;
using TMPro;
using UnityEngine;
using System.Linq;

public class Question : GameStateBehaviour
{
    public string playerQuestion { get; private set; }

    public override void Enter()
    {
        UIPresenter_Header header = FindAnyObjectByType<UIPresenter_Header>();
        header.SetView("質問");

        UIPresenter_Body body = FindAnyObjectByType<UIPresenter_Body>();
        UIPresenter_Footer footer = FindAnyObjectByType<UIPresenter_Footer>();

        PlayerCharacterList characterList = FindAnyObjectByType<PlayerCharacterList>();
        Role role = characterList.GetLocalPlayerCharacter().Job;

        CharacterDataList characterDataList = FindAnyObjectByType<CharacterDataList>();
        IPlayerCharacter[] answeringCharacters = characterList.Characters.Where(x => x.IsAlive && x.Job != Role.Representative).ToArray();
        body.ShowAnswerIcons(answeringCharacters.Select(x => (characterDataList.CharacterDatas[x.CharacterIndex].imageSprite, characterDataList.CharacterDatas[x.CharacterIndex].characterName)).ToArray());

        if (role == Role.Representative)
        {
            body.ShowWait("問題を入力してください");
            footer.ShowInput();
        }
        else
        {
            body.ShowWait("代表者が問題を入力しています");
            footer.Hide();
        }
    }

    public void Submit()
    {
        GameState currentState = (PhotonNetwork.CurrentRoom.CustomProperties["GameState"] is int value) ? (GameState)value : GameState.JOB_DISTRIBUTION;
        if (currentState != GameState.QUESTION) return;

        UIPresenter_Footer footer = FindAnyObjectByType<UIPresenter_Footer>();
        string question = footer.GetInputFieldText();

        if (string.IsNullOrEmpty(question)) return;

        var resultProps = new ExitGames.Client.Photon.Hashtable();
        resultProps["Question"] = question;
        PhotonNetwork.CurrentRoom.SetCustomProperties(resultProps);
    }
}