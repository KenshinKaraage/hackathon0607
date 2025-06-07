using Photon.Pun;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Realtime;

//ゲーム開始時にプレイヤー配布
public class PlayerCharacterList : MonoBehaviour
{
    private List<IPlayerCharacter> _characters = new List<IPlayerCharacter>();
    public List<IPlayerCharacter> Characters => _characters;
    
    private void Awake()
    {
        SetPlayerList();
    }

    public void SetPlayerList()
    {
        foreach (var player in PhotonNetwork.PlayerList)
        {
            _characters.Add(new HumanPlayerCharacter(player));
        }

        //AIの人数分追加。今回はテストとして二人
        int npcID = 1000;
        _characters.Add(new NonPlayerCharacter(npcID++, "サトシ"));
        _characters.Add(new NonPlayerCharacter(npcID++, "タケシ"));
    }

    //PhotonNetwork.LocalPlayerから自分のキャラクターを取得
    public IPlayerCharacter GetLocalPlayerCharacter()
    {
        foreach (var character in _characters)
        {
            if (character.IsNPC) continue;
            HumanPlayerCharacter humanPlayerCharacter = character as HumanPlayerCharacter;
            if (humanPlayerCharacter.GetPhotonPlayer() != PhotonNetwork.LocalPlayer) continue;
            return character;
        }

        return null;
    }
}
