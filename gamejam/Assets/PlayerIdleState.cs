using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerState
{
    public PlayerIdleState(Player player) : base(player) {}

    public override void Enter()
    {
        anim.SetBool("isIdle", true);
    }

    public override void Exit()
    {
        anim.SetBool("isIdle", false);
    }
}
