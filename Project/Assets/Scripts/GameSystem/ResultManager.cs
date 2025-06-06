using UnityEngine;
using UnityEngine.UI;

public class ResultManager : MonoBehaviour
{
    public Text executionText;   // 処刑された人の名前と役職を表示
    public Text resultText;      // 勝ったか負けたかを表示

    public PlayerInfo localPlayer; // 自分のプレイヤー情報（事前に代入しておく）

    // 投票結果を受け取り、処理を実行
    public void ShowResult(VotingResult votingResult)
    {
        // 処刑されたプレイヤー
        PlayerInfo executed = votingResult.ExecutedPlayer;

        // ① 処刑情報の表示
        string executedRoleStr = executed.Role == Role.Werewolf ? "人狼" : "村人";
        executionText.text = $"処刑されたのは {executed.Name}（{executedRoleStr}）です。";

        // ② 勝敗の判定と表示
        bool localWin = false;

        if (executed.Role == Role.Werewolf)
        {
            // 人狼が死んだ → 代表者の勝利
            if (localPlayer.Role == Role.Representative)
                localWin = true;
        }
        else
        {
            // 村人が死んだ → 人狼の勝利
            if (localPlayer.Role == Role.Werewolf)
                localWin = true;
        }

        resultText.text = localWin ? "あなたは勝ちました！" : "あなたは負けました。";
    }
}
