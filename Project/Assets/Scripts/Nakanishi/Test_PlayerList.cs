using Photon.Pun;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Realtime;

namespace Test
{
    //ゲーム開始時にプレイヤー配布
    public class Test_CharacterList : MonoBehaviour
    {
        private List<Test_IPlayerCharacter> _characters = new List<Test_IPlayerCharacter>();
        public List<Test_IPlayerCharacter> Characters => _characters;
        private void Awake()
        {
            SetPlayerList();
        }

        public void SetPlayerList()
        {
            foreach (var player in PhotonNetwork.PlayerList)
            {
                _characters.Add(new Test_HumanPlayerCharacter(player));
            }

            //AIの人数分追加
            int num = 2;
            for (int i = 0; i < num; i++)
            {
                _characters.Add(new Test_NonPlayerCharacter(-(i + 1), "タケシ"));
            }
        }

        //PhotonNetwork.LocalPlayerから自分のキャラクターを取得
        public Test_IPlayerCharacter GetLocalPlayerCharacter()
        {
            foreach (var character in _characters)
            {
                if (character.IsNPC) continue;
                Test_HumanPlayerCharacter humanPlayerCharacter = character as Test_HumanPlayerCharacter;
                if (humanPlayerCharacter.GetPhotonPlayer() != PhotonNetwork.LocalPlayer) continue;
                return character;
            }
            
            return null;
        }
    }

}
