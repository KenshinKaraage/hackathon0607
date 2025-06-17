using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class VotePhase : GameStateBehaviour
{
    private VotePhaseView view;

    public GameFlowController gameFlowController;

    private IPlayerCharacter localPlayerCharacter;
    private bool isRepresentativeAndVoting = false;

    private void Start()
    {
        view = FindAnyObjectByType<VotePhaseView>();
    }

    public override void Enter()
    {
        OnVoteStarted();
    }

    public void OnVoteStarted()
    {

        Debug.Log("Vote phase started. Displaying characters...");
        PlayerCharacterList characterList = FindAnyObjectByType<PlayerCharacterList>();
        List<IPlayerCharacter> votableTargets = characterList.Characters.Where(x => x.IsAlive && x.Job != Role.Representative).ToList();

        CharacterDataList characterDataList = FindAnyObjectByType<CharacterDataList>();
        Debug.Log("characterDataList.CharacterDatas.Count:" + characterDataList.CharacterDatas.Count());
        System.Action<IPlayerCharacter> action = VoteProcess.Instance.OnButtonSelected;

        localPlayerCharacter = characterList.GetLocalPlayerCharacter();  //将来的にCharacterList.GetLocalPlayerCharacter()で取得する
        bool isVoter = localPlayerCharacter.Job == Role.Representative && localPlayerCharacter.IsAlive;

        view.ShowSelectButtons(votableTargets, isVoter, action);
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
