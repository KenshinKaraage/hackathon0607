using UnityEngine;
using Test;

public class JobDistribution : GameStateBehaviour
{
    [SerializeField] private GameObject viewOb;
    [SerializeField] private RoleDistribution jobDistribution;

    public override void Enter()
    {
        UIPresenter presenter = FindAnyObjectByType<UIPresenter>();
        presenter.ResetView();
        viewOb.SetActive(true);

        jobDistribution.AssignRoles();
    }
}

public abstract class GameStateBehaviour : MonoBehaviour
{
    public abstract void Enter();
}