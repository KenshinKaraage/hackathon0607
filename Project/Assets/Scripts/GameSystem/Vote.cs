using UnityEngine;

public class Vote : GameStateBehaviour
{
    [SerializeField] private GameObject viewOb;

    public override void Enter()
    {
        UIPresenter presenter = FindAnyObjectByType<UIPresenter>();
        presenter.ResetView();
        viewOb.SetActive(true);
    }
}
