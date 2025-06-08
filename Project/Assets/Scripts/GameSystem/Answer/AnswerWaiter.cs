using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using System.Linq;
using UnityEngine.InputSystem.LowLevel;

public class AnswerWaiter : MonoBehaviourPunCallbacks
{
    public static int MAXANSWERCOUNT = 3;

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        GameState currentState = (PhotonNetwork.CurrentRoom.CustomProperties["GameState"] is int value) ? (GameState)value : GameState.JOB_DISTRIBUTION;
        if (currentState != GameState.ANSWER) return;
        if (!changedProps.TryGetValue("IsAnswered", out object answered)) return;
        if ((bool)answered)
        {
            if (!PhotonNetwork.IsMasterClient) return;
            Debug.Log("プレイヤー解答");
            //全プレイヤーが回答済みなら投票フェーズへ
            CheckAllPlayerAnswer();
        }
        else
        {
            if (!PhotonNetwork.IsMasterClient) return;
        }

    }

    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        GameState currentState = (PhotonNetwork.CurrentRoom.CustomProperties["GameState"] is int value) ? (GameState)value : GameState.JOB_DISTRIBUTION;
        if (currentState != GameState.ANSWER) return;
        if (propertiesThatChanged.TryGetValue("AllAIAnswered", out object answered))
        {
            if (!(bool)answered) return;
            if (!PhotonNetwork.IsMasterClient) return;

            Debug.Log("全AI解答");
            //全プレイヤーが回答済みなら投票フェーズへ
            CheckAllPlayerAnswer();
        }
        else if (propertiesThatChanged.TryGetValue("AnswerCount", out object count))
        {
            if (!PhotonNetwork.IsMasterClient) return;

            GameFlowController controller = FindAnyObjectByType<GameFlowController>();
            controller.SetRoomState(GameState.QUESTION);
        }
    }

    private void CheckAllPlayerAnswer()
    {
        //質問者以外のプレイヤー（人間）を取得
        PlayerCharacterList characterList = FindAnyObjectByType<PlayerCharacterList>();

        IPlayerCharacter[] characters = characterList.Characters.ToArray();

        if (characters.Where(x => x.Job != Role.Representative).All(x => x.IsAnswered))
        {
            StartCoroutine(ShowAnswerCoroutine());
            photonView.RPC(nameof(ShowAnswerRPC), RpcTarget.All);

        }
    }

    [PunRPC]
    private void ShowAnswerRPC()
    {
        StartCoroutine(ShowAnswerCoroutine());
    }

    private IEnumerator ShowAnswerCoroutine()
    {
        //回答一覧表示
        PlayerCharacterList characterList = FindAnyObjectByType<PlayerCharacterList>();
        List<IPlayerCharacter> votableTargets = characterList.Characters.Where(x => x.IsAlive && x.Job != Role.Representative).ToList();

        UIPresenter_Body body = FindAnyObjectByType<UIPresenter_Body>();
        CharacterDataList characterDataList = FindAnyObjectByType<CharacterDataList>();
        Debug.Log("characterDataList.CharacterDatas.Count:" + characterDataList.CharacterDatas.Count());

        IPlayerCharacter localPlayerCharacter = characterList.GetLocalPlayerCharacter();  //将来的にCharacterList.GetLocalPlayerCharacter()で取得する
        body.ShowAnswers(votableTargets.Select(x => x.Answer).ToArray());

        UIPresenter_Footer footer = FindAnyObjectByType<UIPresenter_Footer>();
        footer.ShowFooterText("回答を表示します");

        yield return new WaitForSeconds(5.0f);

        if (PhotonNetwork.IsMasterClient)
        {
            int answerCount = PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("AnswerCount", out object count) ? (int)count : 0;

            if (answerCount + 1 >= MAXANSWERCOUNT)
            {
                GameFlowController controller = FindAnyObjectByType<GameFlowController>();
                controller.SetRoomState(GameState.VOTE);
            }
            else
            {
                var props = new ExitGames.Client.Photon.Hashtable { { "AnswerCount", answerCount + 1} };
                PhotonNetwork.CurrentRoom.SetCustomProperties(props);
            }
        }
    }
}
