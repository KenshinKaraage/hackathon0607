using UnityEngine;
using Test;

public class JobDistribution : GameStateBehaviour
{
    [SerializeField] private RoleDistribution jobDistribution;

    public override void Enter()
    {
        UIPresenter_Header header = FindAnyObjectByType<UIPresenter_Header>();
        header.SetView("役職配布");
        header.HideInfo();

        UIPresenter_Body body = FindAnyObjectByType<UIPresenter_Body>();
        body.Hide();

        UIPresenter_Footer footer = FindAnyObjectByType<UIPresenter_Footer>();
        footer.Hide();

        Debug.Log("役職配布");
        jobDistribution.AssignRoles();
    }
}

public abstract class GameStateBehaviour : MonoBehaviour
{
    public abstract void Enter();
}