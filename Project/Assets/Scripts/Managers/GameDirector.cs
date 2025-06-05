using UnityEngine;
using System.Collections;
using Photon.Pun;

public class GameDirector : MonoBehaviour
{
    [SerializeField] private UIPresenter presenter;
    private GameFlowController controller;

    private void Start()
    {
            controller = FindAnyObjectByType<GameFlowController>();
            StartCoroutine(Direct());

    }

    private IEnumerator Direct()
    {
        UIPresenter presenter = FindAnyObjectByType<UIPresenter>();
        presenter.ResetView(); //UI要素をリセット
        controller.Initialize(); //GameStateの初期化
        yield return new WaitForSeconds(1);
        NPCManager.Instance.InitializeNPCsForGame();

        //仮の処理
        controller.SetRoomState(GameState.VOTE);
        yield return null;
    }
}
