using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using UnityEngine;

public class AnswerPhaseView : MonoBehaviour
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

    public void ShowQuestion(string question,bool isAnswerer)
    {
        int answerCount = PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("AnswerCount", out object count) ? (int)count : 0;
        header.SetView($"回答({answerCount + 1}/{AnswerWaiter.MAXANSWERCOUNT})");
        body.ShowQuestion(question);
        body.ShowAnswersThinking();
        if (isAnswerer)
        {
            footer.ShowInput();
        }
        else
        {
            footer.ShowFooterText("回答者が入力中・・・");
        }
    }

    public void ShowAnswers(List<IPlayerCharacter> players)
    {
        body.ShowAnswers(players.Select(x => x.Answer).ToArray());
        footer.ShowFooterText("回答を表示します");
    }

    public void WaitForAllAnsweres()
    {
        footer.ShowFooterText("全員の回答を待機しています");
    }

    public string GetAnswer()
    {
        return footer.GetInputFieldText();
    }
}
