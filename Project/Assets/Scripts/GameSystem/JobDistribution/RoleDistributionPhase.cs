using UnityEngine;
using Test;

public class RoleDistributionPhase : GameStateBehaviour
{
    [SerializeField] private RoleDistribution roleDistribution;

    public override void Enter()
    {
        Debug.Log("役職配布");
        roleDistribution.AssignRoles();
    }
}

public abstract class GameStateBehaviour : MonoBehaviour
{
    public abstract void Enter();
}