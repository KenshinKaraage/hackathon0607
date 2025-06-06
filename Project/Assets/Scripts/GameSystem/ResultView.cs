using System.Collections;
using Photon.Pun;
using UnityEngine;
using System.Linq;
using TMPro;

public class ResultView : GameStateBehaviour
{
    [SerializeField] private TMP_Text executionText;
    [SerializeField] private TMP_Text resultText;

    [SerializeField] private GameObject viewOb;

    public override void Enter()
    {
        UIPresenter presenter = FindAnyObjectByType<UIPresenter>();
        presenter.ResetView();
        viewOb.SetActive(true);

        int executedPlayerID = PhotonNetwork.CurrentRoom.CustomProperties["VotedTargetID"] is int value ? value : -1;
        if (executedPlayerID == -1)
        {
            Debug.Log("executedPlayerIDが無効です");
            return;
        }

        // 処刑されたプレイヤー
        CharacterList characterList = FindAnyObjectByType<CharacterList>();
        IPlayerCharacter executedPlayer = characterList.Characters.Where(x => x.ID == executedPlayerID).First();

        // ① 処刑情報の表示
        string executedRoleStr = executedPlayer.Job == Role.Werewolf ? "人狼" : "村人";
        executionText.text = $"処刑されたのは {executedPlayer.Displayname}（{executedRoleStr}）です。";

        // ② 勝敗の判定と表示
        bool localWin = false;

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

        resultText.text = localWin ? "あなたは勝ちました！" : "あなたは負けました。";
    }
}
