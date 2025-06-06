using UnityEngine;
using Photon.Pun;
using System.Collections.Generic;

public class NPCManager : MonoBehaviourPunCallbacks
{
    public static NPCManager Instance { get; private set; }
    public List<NonPlayerCharacter> NpcList { get; private set; } = new List<NonPlayerCharacter>();  // NPCクラスのリスト

    private int nextNpcId = 1000; // NPC用の一意なID（ActorNumberと重複しないように）

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // マスタークライアントがゲーム開始時にNPCを生成する
    public void InitializeNPCsForGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            NpcList.Clear();
            // 例: 人間NPCを3体生成
            CreateAndAddNPC("NPCx", JobNames.VILLAGER);
            CreateAndAddNPC("NPCy", JobNames.VILLAGER);
            CreateAndAddNPC("NPCz", JobNames.VILLAGER);

            // 他クライアントへNPCリストを同期
            photonView.RPC("SyncNPCListRPC", RpcTarget.OthersBuffered, SerializeNPCList(NpcList));
        }
    }

    private void CreateAndAddNPC(string name, JobNames job)
    {
        NonPlayerCharacter newNpc = new NonPlayerCharacter(nextNpcId++, name, job);
        NpcList.Add(newNpc);
        Debug.Log(newNpc +"was created");
    }

    public NonPlayerCharacter GetNpcById(int id)
    {
        return NpcList.Find(npc => npc.ID == id);
    }

    // NPCの生存状態を更新 (マスタークライアントから呼ばれることを想定)
    public void SetNpcAliveStatus(int npcId, bool isAlive)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            NonPlayerCharacter npc = GetNpcById(npcId);
            if (npc != null)
            {
                npc.IsAlive = isAlive;
                photonView.RPC("UpdateNpcAliveStatusRPC", RpcTarget.AllBuffered, npcId, isAlive);
            }
        }
    }

    // RPCでNPCの生存状態を更新
    [PunRPC]
    void UpdateNpcAliveStatusRPC(int npcId, bool isAlive)
    {
        NonPlayerCharacter npc = GetNpcById(npcId); // このクライアントのリストからNPCを探す
        if (npc != null)
        {
            npc.IsAlive = isAlive;
            Debug.Log($"NPC Status Update: {npc.Displayname} IsAlive: {npc.IsAlive}");
        }
    }

    private string SerializeNPCList(List<NonPlayerCharacter> list)
    {
        NPCListDataWrapper wrapper = new NPCListDataWrapper { Npcs = list };
        return JsonUtility.ToJson(wrapper);
    }

    //ネットワークで送信されたNPCリストのjson列を安全に復元して返す
    private List<NonPlayerCharacter> DeserializeNPCList(string json)
    {
        NPCListDataWrapper wrapper = JsonUtility.FromJson<NPCListDataWrapper>(json);
        return wrapper?.Npcs ?? new List<NonPlayerCharacter>();
    }

    [PunRPC]
    void SyncNPCListRPC(string npcListDataJson)
    {
        this.NpcList = DeserializeNPCList(npcListDataJson);
        Debug.Log("NPC List Synced. Count: " + this.NpcList.Count);
    }
}

[System.Serializable]
public class NPCListDataWrapper
{
    public List<NonPlayerCharacter> Npcs;
}
