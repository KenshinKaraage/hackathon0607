using UnityEngine;
using System.Collections;
using Photon.Pun;

public class GameDirector : MonoBehaviour
{
    [SerializeField] private UIPresenter presenter;
    private GameFlowController controller;

    private void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            controller = FindAnyObjectByType<GameFlowController>();
            StartCoroutine(Direct());
        }
    }

    private IEnumerator Direct()
    {
        UIPresenter presenter = FindAnyObjectByType<UIPresenter>();
        presenter.ResetView();
        controller.Initialize();

        //仮の処理
        yield return new WaitForSeconds(2.0f);
        controller.SetRoomState(GameState.QUESTION);
        yield return new WaitForSeconds(2.0f);
        controller.SetRoomState(GameState.ANSWER);
        yield return new WaitForSeconds(2.0f);
        controller.SetRoomState(GameState.VOTE);
        yield return new WaitForSeconds(2.0f);
        controller.SetRoomState(GameState.RESULT);
    }
}
