using Cysharp.Threading.Tasks;
using Photon.Pun;
using TMPro;
using UnityEngine;

public class Question : GameStateBehaviour
{
    [SerializeField] private GameObject viewOb;
    [SerializeField] private GameObject questionerOb;
    [SerializeField] private GameObject answererOb;

    [SerializeField] private TMP_InputField questionInputField;

    public string playerQuestion { get; private set; }

    public override void Enter()
    {
        UIPresenter presenter = FindAnyObjectByType<UIPresenter>();
        presenter.ResetView();
        viewOb.SetActive(true);

        CharacterList characterList = FindAnyObjectByType<CharacterList>();
        Role role = characterList.GetLocalPlayerCharacter().Job;

        if (role == Role.Representative)
        {
            answererOb.SetActive(false);
            questionerOb.SetActive(true);
        }
        else
        {
            questionerOb.SetActive(false);
            answererOb.SetActive(true);
        }
    }

    public void Submit()
    {
        if (string.IsNullOrEmpty(questionInputField.text)) return;

        var resultProps = new ExitGames.Client.Photon.Hashtable();
        resultProps["Question"] = questionInputField.text;
        PhotonNetwork.CurrentRoom.SetCustomProperties(resultProps);
    }
}