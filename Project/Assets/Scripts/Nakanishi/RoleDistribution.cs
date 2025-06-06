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


namespace Test
{
    //ゲーム内（JobDisctributionフェーズ）で役職配布を行う
    public class RoleDistribution : MonoBehaviourPunCallbacks
    {
        [SerializeField] private TMP_Text roleText;
        private bool allNPCAttributed;

        /// <summary>
        /// 役職配布
        /// MasterClientのみが呼び出す。プレイヤーが全員揃った後などに呼び出す想定。
        /// </summary>
        public void AssignRoles()
        {
            roleText.text = $"割り当て中";

            allNPCAttributed = false;

            if (!PhotonNetwork.IsMasterClient)
            {
                Debug.LogError("役職配布はMasterClientのみが実行できます。");
                return;
            }

            //プレイヤーリストからプレイヤー取得
            CharacterList playerList = FindAnyObjectByType<CharacterList>();
            List<IPlayerCharacter> characters = new List<IPlayerCharacter>(playerList.Characters);

            // 全プレイヤーの役職をリセット (再割り当ての場合に備えて)
            foreach (IPlayerCharacter p in characters)
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

            // playersのシャッフル (役職をランダムに配布するため)
            characters.OrderBy(x => UnityEngine.Random.Range(0f, 1f));

            // 役職を格納するDictionary
            Dictionary<int, Role> playerRoles = new Dictionary<int, Role>();

            List<IPlayerCharacter> humanPlayerCharacters = characters.Where(x => !x.IsNPC).ToList();
            // 代表者 (人間) 1人
            if (humanPlayerCharacters.Count >= 1)
            {
                // (カスタムプロパティで同期)
                humanPlayerCharacters[0].Job = Role.Representative;
            }
            else
            {
                Debug.LogError("プレイヤー数が不足しています。代表者を割り当てできません。");
                return;
            }

            // 人狼 (人間) 1人
            if (humanPlayerCharacters.Count >= 2)
            {
                // (カスタムプロパティで同期)
                humanPlayerCharacters[1].Job = Role.Werewolf;
            }
            else
            {
                Debug.LogError("プレイヤー数が不足しています。人狼を割り当てできません。");
                return;
            }

            //AIに役職を割り当てる（RPC）
            photonView.RPC(nameof(AssignRoleToAI), RpcTarget.All);
        }

        [PunRPC]
        private void AssignRoleToAI()
        {
            CharacterList playerList = FindAnyObjectByType<CharacterList>();
            List<IPlayerCharacter> characters = new List<IPlayerCharacter>(playerList.Characters);
            List<IPlayerCharacter> npcPlayers = characters.Where(x => x.IsNPC).ToList();


            int villagerAICount = 0;
            for (int i = 0; i < npcPlayers.Count; i++) // 2人目以降のプレイヤーに対して
            {
                if (i < 2) // 最大2人のAI村人
                {
                    npcPlayers[i].Job = Role.VillagerAI;
                    villagerAICount++;
                }
                else
                {
                    Debug.LogWarning($"AI村人の上限に達したため、プレイヤー {npcPlayers[i].Displayname} には役職が割り当てられませんでした。");
                    // ここで「村人（AI）」の枠に収まらない人間プレイヤーがいた場合の処理を検討する
                    // 例：Role.VillagerHumanなど別の役職を割り当てる、エラーとしてルームを閉じるなど
                }
            }


            allNPCAttributed = true;
            OnAllNPCAttributed();
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
        /// 各プレイヤーの役職を表示する (デバッグ用)
        /// </summary>
        private async void DisplayPlayerRoles()
        {
            CharacterList playerList = FindAnyObjectByType<CharacterList>();
            List<IPlayerCharacter> characters = new List<IPlayerCharacter>(playerList.Characters);
            List<IPlayerCharacter> humanPlayerCharacters = characters.Where(x => !x.IsNPC).ToList();

            foreach (IPlayerCharacter player in humanPlayerCharacters)
            {
                HumanPlayerCharacter humanPlayerCharacter = player as HumanPlayerCharacter;
                if (!humanPlayerCharacter.GetPhotonPlayer().IsLocal) continue;

                roleText.text = $"あなたは{player.Job}です";
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
            if (!changedProps.TryGetValue("Job", out object job)) return;

            CharacterList playerList = FindAnyObjectByType<CharacterList>();
            List<IPlayerCharacter> characters = new List<IPlayerCharacter>(playerList.Characters);

            Debug.Log("human役職割り当て " + job);
            if (characters.All(x => x.Job != Role.None) && allNPCAttributed)
            {
                //全プレイヤーが回答済みなら役職を表示させる
                DisplayPlayerRoles();
            }

        }

        private void OnAllNPCAttributed()
        {
            GameState currentState = (PhotonNetwork.CurrentRoom.CustomProperties["GameState"] is int value) ? (GameState)value : GameState.RESULT;
            if (currentState != GameState.JOB_DISTRIBUTION) return;

            Debug.Log("ai役職割り当て ");

            CharacterList playerList = FindAnyObjectByType<CharacterList>();
            List<IPlayerCharacter> characters = new List<IPlayerCharacter>(playerList.Characters);
            if (characters.All(x => x.Job != Role.None))
            {
                //全プレイヤーが回答済みなら役職を表示させる
                DisplayPlayerRoles();
            }
        }
    }
}