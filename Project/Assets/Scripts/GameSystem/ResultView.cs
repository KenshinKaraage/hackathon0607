using UnityEngine;

public class ResultView : GameStateBehaviour
{
    [SerializeField] private GameObject viewOb;

    public override void Enter()
    {
        UIPresenter presenter = FindAnyObjectByType<UIPresenter>();
        presenter.ResetView();
        viewOb.SetActive(true);
    }
}
