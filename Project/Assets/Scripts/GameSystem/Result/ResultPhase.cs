using System.Collections;
using Photon.Pun;
using UnityEngine;
using System.Linq;
using TMPro;

public class ResultPhase : GameStateBehaviour
{
    private ResultPhaseView view;

    private int executedPlayerID;

    private void Start()
    {
        view = FindAnyObjectByType<ResultPhaseView>();
    }

    public override void Enter()
    {
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
        view.ShowExecution(executedPlayer);

        yield return new WaitForSeconds(2.0f);

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

        view.ShowResult(characterList.Characters, localWin);

        yield return new WaitForSeconds(5.0f);

        //ホストのみリトライ画面を表示
        view.ShowRetry(PhotonNetwork.IsMasterClient);
    }
}