using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerState
{   
    protected Player player;
    protected Animator anim;
    public PlayerState (Player player)
    {
        this.player = player;
    }
    public virtual void Enter() {}
    public virtual void Exit() {}
    
    public virtual void Update(){}
    public virtual void FixedUpdate(){}
}
