using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeathByBomb : PlayerDeathEvent
{
    private float thrust;
    private Vector2 direction;

    public PlayerDeathByBomb(Player player, Obstacle obstacle, Vector3 position, Vector2 direction, float thrust) : base(player, obstacle, position)
    {
        this.thrust = thrust;
        this.direction = direction;
    }

    public override void StartDeath()
    {
        base.StartDeath();
        player.ApplyRagdollImpulse(thrust, direction);
        player.GetComponent<PlayerAppearence>().ChangeBodyPiecesSprite("explosion");
    }
}
