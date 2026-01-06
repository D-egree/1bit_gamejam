using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunState : PlayerState
{
    public PlayerRunState(Player player) : base(player) {}

    public override void Enter()
    {
        anim.SetBool("IsRunning", true);
    }

    public override void Exit()
    {
        anim.SetBool("IsRunning", false);
    }

    public override void Update()
    {
        base.Update();

        // Stop running → idle
        if (!player.IsMoving)
        {
            player.ChangeState(player.idleState);
            return;
        }

        // Jump
        if (player.jumpPressed)
        {
            player.jumpPressed = false;
            player.ChangeState(player.jumpState);
            return;
        }
    }
}
