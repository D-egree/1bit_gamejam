using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerState
{
    protected Player player;
    protected Animator anim;

    protected PlayerState(Player player)
    {
        this.player = player;
        this.anim = player.anim;
    }

    public virtual void Enter() { }
    public virtual void Update() { }
    public virtual void Exit() { }
}