using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeathByBullet : PlayerDeathEvent
{
    private GameObject explosionParticle;

    public PlayerDeathByBullet(Player player, Vector3 position) : base(player, position) { }

    public override void StartDeath()
    {
        explosionParticle = Resources.Load<GameObject>("Prefab/Particles/BombSmoke");
        GameObject explosion = GameObject.Instantiate(explosionParticle, position, Quaternion.identity);      //TODO: Sistemare
        GameObject.Destroy(explosion.gameObject, 1f);
        player.SetActiveRagdoll(true);

    }
}
