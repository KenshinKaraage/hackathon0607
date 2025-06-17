using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VotePhaseView : MonoBehaviour
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

    public void ShowSelectButtons(List<IPlayerCharacter> votableTargets,bool isVoter, System.Action<IPlayerCharacter> action)
    {
        header.SetView("投票");
        if (isVoter)
        {
            body.ShowAnswers(votableTargets.Select(x => (x.Answer, x)).ToArray(), action);
            footer.ShowFooterText("投票先を選んでください");
        }
        else
        {
            footer.ShowFooterText("代表者が投票先を選んでいます");
        }
    }

    public void ShowSelectedPlayer(IPlayerCharacter selectedTarget)
    {
        CharacterDataList characterDataList = FindAnyObjectByType<CharacterDataList>();
        footer.ShowSubmit($"{characterDataList.CharacterDatas[selectedTarget.CharacterIndex].characterName}に投票しますか？");
    }
}
