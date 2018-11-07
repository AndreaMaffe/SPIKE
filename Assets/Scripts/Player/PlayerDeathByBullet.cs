using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeathByBullet : PlayerDeathEvent
{
    private GameObject explosionParticle;

    public PlayerDeathByBullet(Player player, Obstacle obstacle, Vector3 position) : base(player, obstacle, position) { }

    public override void StartDeath()
    {
        explosionParticle = Resources.Load<GameObject>("Prefab/Particles/BombSmoke");
        GameObject explosion = GameObject.Instantiate(explosionParticle, position, Quaternion.identity);      
        GameObject.Destroy(explosion.gameObject, 1f);
        player.SetActiveRagdoll(true);
        //player.GetComponent<Rigidbody2D>().AddForce(2 * obstacle.gameObject.GetComponent<Rigidbody2D>().velocity);        //TODO: Sistemare

    }
}
