using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Vote : GameStateBehaviour
{
    [SerializeField] private GameObject voteOb;
    private VoteUIPresenter presenter;

    public GameFlowController gameFlowController;

    private IPlayerCharacter localPlayerCharacter;
    private bool isRepresentativeAndVoting = false;
    public override void Enter()
    {
        FindAnyObjectByType<UIPresenter>().ResetView();
        voteOb.SetActive(true);

        presenter = FindAnyObjectByType<VoteUIPresenter>();
        //-エラー処理-
        if (presenter == null)
        {
            Debug.LogError("[Vote.cs] UIPresenterが見つかりません。");
            return;
        }

        
        OnVoteStarted();
    }

    public void OnVoteStarted()
    {
        Debug.Log("Vote phase started. Displaying characters...");
        PlayerCharacterList characterList = FindAnyObjectByType<PlayerCharacterList>();
        List<IPlayerCharacter> votableTargets = characterList.Characters.Where(x => x.IsAlive && x.Job != Role.Representative).ToList();

        presenter.ShowAnswers(votableTargets.Select(x => (x.Displayname, x.Answer)).ToArray());

        localPlayerCharacter = characterList.GetLocalPlayerCharacter();  //将来的にCharacterList.GetLocalPlayerCharacter()で取得する

        bool isVoter = localPlayerCharacter.Job == Role.Representative && localPlayerCharacter.IsAlive;
        presenter.ShowVoterView(isVoter);

        if (isVoter)
        {
            presenter.ShowVoteUI(votableTargets, (target) =>
            {
                VoteProcess.Instance.RecordVote(target);
            });
        }
        else
        {
            Debug.Log("投票権限がありません");
        }
    }

    //すべてのプレイヤーのリストを取得
    private List<IPlayerCharacter> GetAllCharacters()
    {
        var characters = new List<IPlayerCharacter>();

        // 人間プレイヤーを追加
        foreach (Player p in PhotonNetwork.PlayerList)
        {
            var playerCharacter = new HumanPlayerCharacter(p);
            // とりあえず全員表示。将来的には生存確認などを追加
            // if (playerCharacter.IsAlive)
            // {
            characters.Add(playerCharacter);
            // }
        }

        // NPCを追加
        if (NPCManager.Instance != null)
        {
            foreach (var npc in NPCManager.Instance.NpcList)
            {
                // とりあえず全員表示。将来的には生存確認などを追加
                // if (npc.IsAlive)
                // {
                characters.Add(npc);
                // }
            }
        }

        // 取得したキャラクターリストの全プロパティをログに出力する
        Debug.Log($"Found {characters.Count} characters. Listing properties:");

        // StringBuilderを使うと、複数の文字列を結合する際のパフォーマンスが良い
        var sb = new StringBuilder();
        sb.AppendLine("--- Character List Details ---");

        for (int i = 0; i < characters.Count; i++)
        {
            IPlayerCharacter character = characters[i];
            sb.AppendLine(
                $"  [{i}] Nickname: {character.Displayname}, " +
                $"Job: {character.Job}, " +
                $"IsAlive: {character.IsAlive}, " +
                $"IsNPC: {character.IsNPC}, " +
                $"UniqueID: {character.ID}"
            );
        }

        sb.AppendLine("------------------------------");
        Debug.Log(sb.ToString());

        return characters;
    }


    private void OnTargetSelectedForVote(IPlayerCharacter selectedTarget)
    {

    }

    [PunRPC]
    void ReceiveVoteRPC(int voterActorNumber, int votedTargetId, bool isTargetNPC)
    {
        
    }

    [PunRPC]
    void ProceedToResultPhaseRPC(int votedTargetId, bool isTargetNPC, bool representativeWins)
    {

    }
}
