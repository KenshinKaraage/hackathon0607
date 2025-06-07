using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using TMPro;
using UnityEngine;
using System.Linq;
using UnityEngine.TextCore.Text;

public class APIQuestionSender : MonoBehaviourPunCallbacks
{
    [SerializeField, TextArea(1, 20)] private string basePrompt;

    public bool AllAPIAnswered { get; private set; }

    public void Send(string question)
    {
        //AIプレイヤーの人数分リクエストを送る
        PlayerCharacterList characterList = FindAnyObjectByType<PlayerCharacterList>();
        List<NonPlayerCharacter> playerCharacters = characterList.Characters.Where(x => x.IsNPC).Select(x => x as NonPlayerCharacter).ToList();
        foreach (var character in playerCharacters)
        {
            Send(character.ID, question);
        }
    }

    private async void Send(int ID, string question)
    {
        APIConnector apiConnector = FindAnyObjectByType<APIConnector>();
        PlayerCharacterList characterList = FindAnyObjectByType<PlayerCharacterList>();
        CharacterDataList characterDataList = FindAnyObjectByType<CharacterDataList>();

        IPlayerCharacter player = characterList.Characters.Where(x => x.ID == ID).First();

        CharacterData characterData = characterDataList.CharacterDatas[player.CharacterIndex];

        string prompt = characterData.characterPrompt + "\n" + basePrompt;
        string result = await apiConnector.SendRequest(prompt, question);


        GeminiResponse response = JsonUtility.FromJson<GeminiResponse>(result);
        AnswerContent answer = JsonUtility.FromJson<AnswerContent>(response.answer);

        Debug.Log("AIAnswered:" + answer.word);

        photonView.RPC(nameof(ChangeAIAnswer), RpcTarget.All, ID, answer.word);
    }

    public void ResetAIAnswer()
    {
        photonView.RPC(nameof(ResetAIAnswerRPC), RpcTarget.All);

        var resultProps = new ExitGames.Client.Photon.Hashtable();
        resultProps["AllAIAnswered"] = false;
        PhotonNetwork.CurrentRoom.SetCustomProperties(resultProps);
    }

    [PunRPC]
    private void ResetAIAnswerRPC()
    {
        PlayerCharacterList characterList = FindAnyObjectByType<PlayerCharacterList>();
        IPlayerCharacter[] npcCharacters = characterList.Characters.Where(x => x.IsNPC).ToArray();

        foreach (var character in npcCharacters)
        {
            character.IsAnswered = false;
        }
    }

    [PunRPC]
    private void ChangeAIAnswer(int characterID, string answer)
    {
        PlayerCharacterList test_CharacterList = FindAnyObjectByType<PlayerCharacterList>();

        IPlayerCharacter targetCharacter = test_CharacterList.Characters.Where(x => x.ID == characterID).First();
        targetCharacter.Answer = answer;
        targetCharacter.IsAnswered = true;

        if (PhotonNetwork.IsMasterClient)
        {
            if (test_CharacterList.Characters.Where(x => x.IsNPC).All(x => x.IsAnswered))
            {
                Debug.Log("AllAIAnswered");

                var resultProps = new ExitGames.Client.Photon.Hashtable();
                resultProps["AllAIAnswered"] = true;
                PhotonNetwork.CurrentRoom.SetCustomProperties(resultProps);
            }
            else
            {
                Debug.Log("NotAllAIAnswered");
                Debug.Log(test_CharacterList.Characters.Where(x => x.IsNPC && x.IsAnswered).Count());

            }
        }
    }
}
