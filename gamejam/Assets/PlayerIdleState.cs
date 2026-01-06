using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerState
{
    public PlayerIdleState (Player player) : base (player) {}    public override void Enter()
    {
        player.anim.SetBool("isIdle", true);
        base.Enter();
    }
}
