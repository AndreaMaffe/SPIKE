using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeathByBullet : PlayerDeathEvent
{
    private GameObject explosionParticle;

    public PlayerDeathByBullet(Player player, Vector3 position) : base(player, position) { }

    public override void StartDeath()
    {
        explosionParticle = Resources.Load<GameObject>("/Prefab/Particles/BombSmoke");
        Debug.Log(explosionParticle);
        GameObject explosion = Instantiate(explosionParticle, transform.position, Quaternion.identity);      //TODO: Sistemare
        Destroy(explosion.gameObject, 1f);
        player.SetActiveRagdoll(true);

    }
}
