using Photon.Pun;
using TMPro;
using UnityEngine;
using Cysharp.Threading.Tasks;
using ExitGames.Client.Photon;
using Test;

public class PlayerAnswer : GameStateBehaviour
{
    [SerializeField] private GameObject viewOb;
    [SerializeField] private GameObject questionerOb;
    [SerializeField] private GameObject answererOb;
    [SerializeField] private AnswerWaiter waiter;

    [SerializeField] private TMP_Text questionText;
    [SerializeField] private TMP_InputField answerInputField;

    public bool IsAnserwred { get; set; }

    public override void Enter()
    {
        string question = PhotonNetwork.CurrentRoom.CustomProperties["Question"] is string value ? value : "";

        UIPresenter presenter = FindAnyObjectByType<UIPresenter>();
        presenter.ResetView();
        viewOb.SetActive(true);
        questionText.text = question;

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
            questionerOb.SetActive(false);
            answererOb.SetActive(true);

            characterList.GetLocalPlayerCharacter().Answer = "";
            characterList.GetLocalPlayerCharacter().IsAnswered = false;
        }
        else
        {
            answererOb.SetActive(false);
            questionerOb.SetActive(true);
        }
    }

    public void Submit()
    {
        if (string.IsNullOrEmpty(answerInputField.text)) return;

        PlayerCharacterList characterList = FindAnyObjectByType<PlayerCharacterList>();
        characterList.GetLocalPlayerCharacter().Answer = answerInputField.text;
        characterList.GetLocalPlayerCharacter().IsAnswered = true;
    }
}
