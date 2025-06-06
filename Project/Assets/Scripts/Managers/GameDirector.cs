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

            UIPresenter presenter = FindAnyObjectByType<UIPresenter>();
            presenter.ResetView();
            controller.Initialize();
    }
}
