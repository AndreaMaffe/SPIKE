using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerDeathEvent : MonoBehaviour
{
    protected Player player;
    protected Vector3 position;

    public PlayerDeathEvent (Player player, Vector3 position)
    {
        this.player = player;
        this.position = position;
    }

    public abstract void StartDeath();

}
