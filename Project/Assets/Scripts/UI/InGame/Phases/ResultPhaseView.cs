using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class ResultPhaseView : MonoBehaviour
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

    public void ShowExecution(IPlayerCharacter executedPlayer)
    {
        header.SetView("処刑タイム");
        CharacterDataList characterDataList = FindAnyObjectByType<CharacterDataList>();
        body.ShowExecute(characterDataList.CharacterDatas[executedPlayer.CharacterIndex].imageSprite, characterDataList.CharacterDatas[executedPlayer.CharacterIndex].characterName);
        footer.Hide();
    }

    public void ShowResult(List<IPlayerCharacter> players, bool win)
    {
        header.SetView("結果発表");
        string[] playerIdentities = players.Where(x => x.Job != Role.Representative).Select(x => x.Job == Role.VillagerAI ? "AIでした！" : $"{x.Displayname}でした！").ToArray();
        body.ShowAnswers(playerIdentities);
        string result = win ? "あなたは勝ちました！" : "あなたは負けました。";
        footer.ShowFooterText(result);
    }

    public void ShowRetry(bool isHost)
    {
        if (isHost)
        {
            footer.ShowRetry();
        }
        else
        {
            footer.ShowFooterText("ホストが再戦するか選択しています・・・");
        }
    }
}
