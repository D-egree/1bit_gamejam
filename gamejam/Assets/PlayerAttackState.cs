using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEngine;

public class PlayerAttackState : MonoBehaviour
{
    public PlayerAttackState(Player player) : base(player){}
    
    public override void Enter()
    {
        base.Enter()
        anim.SetBool("IsAttacking", true);
        rb.velocity =new Vector2(0, rb.velocity.y);
    }

        public override void Exit()
    {
        base.Exit()
        anim.SetBool("IsAttacking", false);
    }
}
