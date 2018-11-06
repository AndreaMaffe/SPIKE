using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeathByBullet : PlayerDeathEvent
{
    public GameObject explosionParticle;

    public PlayerDeathByBullet(Player player, Vector3 position) : base(player, position) { }

    public override void StartDeath()
    {
        GameObject explosion = Instantiate(explosionParticle, transform.position, Quaternion.identity);
        Destroy(explosion.gameObject, 1f);
        player.SetActiveRagdoll(true);

    }
}
