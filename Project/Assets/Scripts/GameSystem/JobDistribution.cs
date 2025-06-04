using UnityEngine;

public class JobDistribution : GameStateBehaviour
{
    [SerializeField] private GameObject viewOb;

    public override void Enter()
    {
        UIPresenter presenter = FindAnyObjectByType<UIPresenter>();
        presenter.ResetView();
        viewOb.SetActive(true);
    }
}

public abstract class GameStateBehaviour : MonoBehaviour
{
    public abstract void Enter();
}