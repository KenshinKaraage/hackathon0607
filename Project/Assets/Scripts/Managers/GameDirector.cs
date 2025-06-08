using UnityEngine;
using System.Collections;
using Photon.Pun;

public class GameDirector : MonoBehaviourPunCallbacks
{
    [SerializeField] private UIPresenter presenter;
    private GameFlowController controller;

    private void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            controller = FindAnyObjectByType<GameFlowController>();
            controller.Initialize();
        }
    }
}
