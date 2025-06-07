using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;
using Cysharp.Threading.Tasks;

public enum Role
{
    None = 0,       // 未割り当て
    Representative = 1, // 代表者 (人間)
    Werewolf = 2,       // 人狼 (人間)
    VillagerAI = 3      // 村人 (AI) - AIプレイヤーの場合
}

[System.Serializable]
public class Assignment
{
    public int id;
    public int role;
    public int characterIndex = -1;
}

//ゲーム内（JobDisctributionフェーズ）で役職配布を行う
//キャラクター配布も行う。
public class RoleDistribution : MonoBehaviourPunCallbacks
{
    private bool allPlayerShuffled;

    //AssingnmentをRPCの引数にするのに必要
    void Awake()
    {
        CustomTypeRegistration.Register();
    }

    /// <summary>
    /// 役職配布
    /// MasterClientのみが呼び出す。プレイヤーが全員揃った後などに呼び出す想定。
    /// </summary>
    public async void AssignRoles()
    {
        UIPresenter_Body body = FindAnyObjectByType<UIPresenter_Body>();
        body.ShowWait("割り当て中");

        if (!PhotonNetwork.IsMasterClient)
        {
            Debug.LogError("役職配布はMasterClientのみが実行できます。");
            return;
        }

        //プレイヤーリストからプレイヤー取得
        PlayerCharacterList playerList = FindAnyObjectByType<PlayerCharacterList>();
        // playersのシャッフル (役職をランダムに配布するため)
        allPlayerShuffled = false;
        playerList.ResetShuffled();
        int[] indeces = Enumerable.Range(0, playerList.Characters.Count()).OrderBy(x => UnityEngine.Random.Range(0f, 1f)).ToArray();
        playerList.Shuffle(indeces);

        await UniTask.WaitUntil(() => allPlayerShuffled);

        List<Assignment> assignments = new List<Assignment>();

        // 全プレイヤーの役職をリセット (再割り当ての場合に備えて)
        foreach (IPlayerCharacter p in playerList.Characters)
        {
            if (!p.IsNPC)
            {
                HumanPlayerCharacter humanPlayerCharacter = p as HumanPlayerCharacter;
                humanPlayerCharacter.Job = Role.None;
            }
            else
            {
                NonPlayerCharacter nonPlayerCharacter = p as NonPlayerCharacter;
                nonPlayerCharacter.Job = Role.None;
            }

        }

        List<IPlayerCharacter> humanPlayerCharacters = playerList.Characters.Where(x => !x.IsNPC).ToList();
        // 代表者 (人間) 1人
        if (humanPlayerCharacters.Count >= 1)
        {
            assignments.Add(new Assignment { id = humanPlayerCharacters[0].ID, role = (int)Role.Representative });
        }
        else
        {
            Debug.LogError("プレイヤー数が不足しています。代表者を割り当てできません。");
            return;
        }

        // 人狼 (人間) 1人
        if (humanPlayerCharacters.Count >= 2)
        {
            assignments.Add(new Assignment { id = humanPlayerCharacters[1].ID, role = (int)Role.Werewolf });
        }
        else
        {
            Debug.LogError("プレイヤー数が不足しています。人狼を割り当てできません。");
            return;
        }


        //AIに役職を割り当てる
        List<IPlayerCharacter> npcPlayers = playerList.Characters.Where(x => x.IsNPC).ToList();
        int villagerAICount = 0;
        for (int i = 0; i < npcPlayers.Count; i++) // 2人目以降のプレイヤーに対して
        {
            if (i < 2) // 最大2人のAI村人
            {
                assignments.Add(new Assignment { id = npcPlayers[i].ID, role = (int)Role.VillagerAI });
                villagerAICount++;
            }
            else
            {
                Debug.LogWarning($"AI村人の上限に達したため、プレイヤー {npcPlayers[i].Displayname} には役職が割り当てられませんでした。");
                // ここで「村人（AI）」の枠に収まらない人間プレイヤーがいた場合の処理を検討する
                // 例：Role.VillagerHumanなど別の役職を割り当てる、エラーとしてルームを閉じるなど
            }
        }

        //キャラクター割り当て
        CharacterDataList characterDataList = FindAnyObjectByType<CharacterDataList>();
        int representetiveID = assignments.Where(x => x.role == (int)Role.Representative).First().id;
        Assignment representativeAssignment = assignments.Where(x => x.id == representetiveID).First();
        int representativeIndex = assignments.IndexOf(representativeAssignment);
        assignments[representativeIndex].characterIndex = 0;

        List<IPlayerCharacter> nonRepresentatives = playerList.Characters.Where(x => x.ID != representetiveID).ToList();
        List<int> characterIndeces = Enumerable.Range(1, characterDataList.CharacterDatas.Count() - 1).OrderBy(x => UnityEngine.Random.Range(0f, 1f)).ToList();

        for (int i = 0; i < nonRepresentatives.Count; i++)
        {
            Assignment assignment = assignments.Where(x => x.id == nonRepresentatives[i].ID).First();
            int index = assignments.IndexOf(assignment);

            if (i < characterIndeces.Count)
            {
                assignments[index].characterIndex = characterIndeces[i];
            }
            else
            {
                assignments[index].characterIndex = characterIndeces[0];
            }
        }

        foreach (var item in assignments)
        {
            Debug.Log($"id{item.id}, job{item.role}, charaindex{item.characterIndex}");

        }
        photonView.RPC(nameof(ExecuteAssignment), RpcTarget.All, assignments.ToArray());
    }

    [PunRPC]
    private void ExecuteAssignment(Assignment[] assignments)
    {
        PlayerCharacterList playerList = FindAnyObjectByType<PlayerCharacterList>();

        foreach (var assignment in assignments)
        {
            IPlayerCharacter player = playerList.Characters.Where(x => x.ID == assignment.id).First();

            if (!player.IsNPC)
            {
                HumanPlayerCharacter humanPlayerCharacter = player as HumanPlayerCharacter;
                if (humanPlayerCharacter.GetPhotonPlayer() == PhotonNetwork.LocalPlayer)
                {
                    player.Job = (Role)assignment.role;
                    player.CharacterIndex = assignment.characterIndex;
                }
            }
            else
            {
                player.Job = (Role)assignment.role;
                player.CharacterIndex = assignment.characterIndex;
            }
        }
    }

    /// <summary>
    /// Listをシャッフルするヘルパーメソッド
    /// </summary>
    /// <param name="list"></param>
    private void Shuffle<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            T temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

    /// <summary>
    /// 各プレイヤーの役職を表示する
    /// </summary>
    private async void DisplayPlayerRoles()
    {
        PlayerCharacterList playerList = FindAnyObjectByType<PlayerCharacterList>();
        List<IPlayerCharacter> characters = new List<IPlayerCharacter>(playerList.Characters);
        List<IPlayerCharacter> humanPlayerCharacters = characters.Where(x => !x.IsNPC).ToList();

        foreach (IPlayerCharacter player in humanPlayerCharacters)
        {
            HumanPlayerCharacter humanPlayerCharacter = player as HumanPlayerCharacter;
            if (!humanPlayerCharacter.GetPhotonPlayer().IsLocal) continue;
            CharacterDataList characterDataList = FindAnyObjectByType<CharacterDataList>();

            UIPresenter_Header header = FindAnyObjectByType<UIPresenter_Header>();
            header.SetView("役職配布", player.Job, characterDataList.CharacterDatas[player.CharacterIndex].imageSprite, player.Displayname, characterDataList.CharacterDatas[player.CharacterIndex].characterName);

            UIPresenter_Body body = FindAnyObjectByType<UIPresenter_Body>();
            body.ShowDistribution(player.Job, characterDataList.CharacterDatas[player.CharacterIndex].imageSprite, characterDataList.CharacterDatas[player.CharacterIndex].characterName, characterDataList.CharacterDatas[player.CharacterIndex].description);

            UIPresenter_Footer footer = FindAnyObjectByType<UIPresenter_Footer>();
            footer.Hide();
        }

        if (PhotonNetwork.IsMasterClient)
        {
            await UniTask.WaitForSeconds(2.0f);

            GameFlowController controller = GetComponent<GameFlowController>();
            controller.SetRoomState(GameState.QUESTION);
        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        GameState currentState = (PhotonNetwork.CurrentRoom.CustomProperties["GameState"] is int value) ? (GameState)value : GameState.RESULT;
        if (currentState != GameState.JOB_DISTRIBUTION) return;
        if (changedProps.TryGetValue("Job", out object job) || changedProps.TryGetValue("CharacterIndex", out object index))
        {
            PlayerCharacterList playerList = FindAnyObjectByType<PlayerCharacterList>();
            List<IPlayerCharacter> characters = new List<IPlayerCharacter>(playerList.Characters);

            if (characters.All(x => x.Job != Role.None && x.CharacterIndex != -2))
            {
                //全プレイヤーが回答済みなら役職を表示させる
                DisplayPlayerRoles();
            }
        }
        else if(changedProps.TryGetValue("Shuffled", out object shuffle))
        {
            if (PhotonNetwork.PlayerList.All(x => x.CustomProperties.TryGetValue("Shuffled", out object shuffle) ? (bool)shuffle : false))
            {
                allPlayerShuffled = true;
            }
        }
    }
}