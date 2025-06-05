using Cysharp.Threading.Tasks;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using System.Linq;
using Test;

public class AnswerWaiter : MonoBehaviourPunCallbacks
{
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        GameState currentState = (PhotonNetwork.CurrentRoom.CustomProperties["GameState"] is int value) ? (GameState)value : GameState.JOB_DISTRIBUTION;
        if (currentState != GameState.ANSWER) return;
        if (!changedProps.TryGetValue("IsAnswered", out object answered)) return;
        if (!(bool)answered) return;
        if (!PhotonNetwork.IsMasterClient) return;

        Debug.Log("プレイヤー解答");

        //全プレイヤーが回答済みなら投票フェーズへ
        CheckAllPlayerAnswer();
    }

    public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {
        GameState currentState = (PhotonNetwork.CurrentRoom.CustomProperties["GameState"] is int value) ? (GameState)value : GameState.JOB_DISTRIBUTION;
        if (currentState != GameState.ANSWER) return;
        if (!propertiesThatChanged.TryGetValue("AllAIAnswered", out object answered)) return;
        if (!(bool)answered) return;
        if (!PhotonNetwork.IsMasterClient) return;

        Debug.Log("全AI解答");
        //全プレイヤーが回答済みなら投票フェーズへ
        CheckAllPlayerAnswer();
    }

    private void CheckAllPlayerAnswer()
    {
        //質問者以外のプレイヤー（人間）を取得
        Test_CharacterList characterList = FindAnyObjectByType<Test_CharacterList>();

        Test_IPlayerCharacter[] characters = characterList.Characters.ToArray();

        if (characters.Where(x => x.Job != Role.Representative).All(x => x.IsAnswered))
        {
            GameFlowController controller = FindAnyObjectByType<GameFlowController>();
            controller.SetRoomState(GameState.VOTE);
        }
    }
}
