using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using UnityEngine;

public class QuestionPhaseView : MonoBehaviour
{
    private UIPresenter_Body body;
    private UIPresenter_Footer footer;
    private UIPresenter_Header header;

    void Start()
    {
        header = FindAnyObjectByType<UIPresenter_Header>();
        body = FindAnyObjectByType<UIPresenter_Body>();
        footer = FindAnyObjectByType<UIPresenter_Footer>();
    }

    public void Show(IPlayerCharacter questioner, List<IPlayerCharacter> answeres, bool isQuestioner)
    {
        int answerCount = PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("AnswerCount", out object count) ? (int)count : 0;
        header.SetView($"質問({answerCount + 1}/{AnswerWaiter.MAXANSWERCOUNT})");

        CharacterDataList characterDataList = FindAnyObjectByType<CharacterDataList>();
        body.ShowQuestion(characterDataList.CharacterDatas[questioner.CharacterIndex].imageSprite, questioner.Displayname, "問題を考えています・・・");
        body.ShowAnswerIcons(answeres.Select(x => (characterDataList.CharacterDatas[x.CharacterIndex].imageSprite, characterDataList.CharacterDatas[x.CharacterIndex].characterName)).ToArray());

        if (isQuestioner)
        {
            footer.ShowInput();
        }
        else
        {
            footer.ShowFooterText("代表者が問題を考えています・・・");
        }
    }

    public string GetQuestion()
    {
        return footer.GetInputFieldText();
    }

}
