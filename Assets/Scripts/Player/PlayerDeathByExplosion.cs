using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeathByExplosion : PlayerDeathEvent
{
    private GameObject explosionParticle;

    public PlayerDeathByExplosion(Player player, Obstacle obstacle, Vector3 position) : base(player, obstacle, position) { }

    public override void StartDeath()
    {
        SpawnExplosionParticles();
        SpawnBloodParticles();

        player.SetActiveRagdoll(true);
        Vector2 angleOfImpact = (player.transform.position - obstacle.transform.position).normalized;
        player.ApplyRagdollImpulse(20, angleOfImpact);
        player.GetComponent<PlayerAppearence>().ChangeBodyPiecesSprite("explosion");
    }

    void SpawnExplosionParticles() {
        explosionParticle = Resources.Load<GameObject>("Prefab/Particles/BombSmoke");
        GameObject explosion = GameObject.Instantiate(explosionParticle, position, Quaternion.identity);
        GameObject.Destroy(explosion.gameObject, 1f);
    }
}
