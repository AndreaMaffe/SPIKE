using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerDeathEvent
{
    protected Player player;
    protected Vector3 position;
    protected Obstacle obstacle;

    public PlayerDeathEvent (Player player, Obstacle obstacle, Vector3 position)
    {
        this.player = player;
        this.position = position;
        this.obstacle = obstacle;
    }

    public abstract void StartDeath();

}
