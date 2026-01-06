using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerState
{
    public PlayerJumpState(Player player) : base(player) {}

    public override void Enter()
    {
        anim.SetBool("IsJumping", true);
    }

    public override void Exit()
    {
        anim.SetBool("IsJumping", false);
    }

    public override void Update()
    {
        base.Update();

        if (player.IsGrounded && player.rb.velocity.y <= 0.01f)
        {
            if (player.IsMoving)
                player.ChangeState(player.runState);
            else
                player.ChangeState(player.idleState);
        }
    }
}
