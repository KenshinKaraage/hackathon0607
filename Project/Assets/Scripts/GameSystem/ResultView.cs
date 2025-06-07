using System.Collections;
using Photon.Pun;
using UnityEngine;
using System.Linq;
using TMPro;

public class ResultView : GameStateBehaviour
{
    private int executedPlayerID;

    public override void Enter()
    {
        UIPresenter_Header header = FindAnyObjectByType<UIPresenter_Header>();
        header.SetView("結果発表");

        executedPlayerID = PhotonNetwork.CurrentRoom.CustomProperties["VotedTargetID"] is int value ? value : -1;
        if (executedPlayerID == -1)
        {
            Debug.Log("executedPlayerIDが無効です");
            return;
        }

        StartCoroutine(ResultCoroutine());

    }

    private IEnumerator ResultCoroutine()
    {
        // 処刑されたプレイヤー
        PlayerCharacterList characterList = FindAnyObjectByType<PlayerCharacterList>();
        IPlayerCharacter executedPlayer = characterList.Characters.Where(x => x.ID == executedPlayerID).First();

        // ① 処刑情報の表示
        string executedRoleStr = executedPlayer.Job == Role.Werewolf ? "人狼" : "村人";

        UIPresenter_Body body = FindAnyObjectByType<UIPresenter_Body>();
        UIPresenter_Footer footer = FindAnyObjectByType<UIPresenter_Footer>();

        CharacterDataList characterDataList = FindAnyObjectByType<CharacterDataList>();
        body.ShowExecute(characterDataList.CharacterDatas[executedPlayer.CharacterIndex].imageSprite, characterDataList.CharacterDatas[executedPlayer.CharacterIndex].characterName);
        footer.Hide();

        //executionText.text = $"処刑されたのは {executedPlayer.Displayname}（{executedRoleStr}）です。";

        yield return new WaitForSeconds(2.0f);

        // ② 勝敗の判定と表示
        bool localWin = false;

        string aiStr = "AIでした！";

        body.ShowAnswers(characterList.Characters.Where(x =>x.Job != Role.Representative).Select(x => x.Job == Role.VillagerAI ? aiStr : $"{x.Displayname}でした！").ToArray());

        IPlayerCharacter localCharacter = characterList.GetLocalPlayerCharacter();
        if (executedPlayer.Job == Role.Werewolf)
        {
            // 人狼が死んだ → 代表者の勝利
            if (localCharacter.Job == Role.Representative)
                localWin = true;
        }
        else
        {
            // 村人が死んだ → 人狼の勝利
            if (localCharacter.Job == Role.Werewolf)
                localWin = true;
        }

        string result = localWin ? "あなたは勝ちました！" : "あなたは負けました。";
        footer.ShowFooterText(result);
    }
}
