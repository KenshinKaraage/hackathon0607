using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon; // Hashtableのために必要
using System.Text;

public class VoteProcess : MonoBehaviourPunCallbacks
{
    // シングルトンとしてインスタンスを公開
    public static VoteProcess Instance { get; private set; }

    void Awake()
    {
        // シングルトンの設定
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 投票ボタンが押されたときに呼び出されるメソッド。
    /// </summary>
    /// <param name="votedTarget">投票されたキャラクター</param>
    public void RecordVote(IPlayerCharacter votedTarget)
    {
        if (votedTarget == null)
        {
            Debug.LogError("[VoteProcess] 投票対象が無効です。");
            return;
        }

        Debug.Log($"[VoteProcess] {PhotonNetwork.LocalPlayer.NickName} が {votedTarget.Displayname} への投票を記録しようとしています。");

        // 代表者のみが投票を記録できるようにチェック（オプションですが推奨）
        var localPlayerCharacter = new HumanPlayerCharacter(PhotonNetwork.LocalPlayer);
       // if (localPlayerCharacter.Job != JobNames.REPRESENTATIVE)
       // {
         //   Debug.LogWarning("[VoteProcess] 代表者ではないため、投票を記録できません。");
         //   return;
       // }

        // --- ここが中心的な処理 ---
        // ルームのカスタムプロパティとして、投票された対象の情報を設定する
        Hashtable props = new Hashtable
        {
            { "VotedTargetID", votedTarget.ID },
            // 必要であれば、投票者の情報なども追加できる
            // { "VoterID", PhotonNetwork.LocalPlayer.ActorNumber }
        };

        // ルームのプロパティを更新。これにより全プレイヤーに情報が同期される。
        PhotonNetwork.CurrentRoom.SetCustomProperties(props);

        Debug.Log($"[VoteProcess] ルームプロパティを更新しました。VotedTargetID: {votedTarget.ID}");
    }
    public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {
        // StringBuilderを使って、変更されたプロパティの内容を整形
        var sb = new StringBuilder();
        sb.AppendLine("[VoteProcess] OnRoomPropertiesUpdate - カスタムプロパティが変更されました:");

        foreach (var prop in propertiesThatChanged)
        {
            sb.AppendLine($"  キー: {prop.Key}, 値: {prop.Value}");
        }

        // 整形したメッセージをログに出力
        Debug.Log(sb.ToString());
    }

}
