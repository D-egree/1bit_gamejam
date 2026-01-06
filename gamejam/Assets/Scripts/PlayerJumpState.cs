using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerState
{


public PlayerJumpState(Player player) : base(player){}

    public override void Enter()
    {
        base.Enter();
        anim.SetBool ("IsJumping", true);
    }
    public override void Exit()
    {
        base.Exit();
        anim.SetBool ("IsJumping", false);
    }}
