using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeathBySpike : PlayerDeathEvent
{
    private GameObject bloodParticles;

    public PlayerDeathBySpike(Player player, Obstacle obstacle, Vector3 position) : base(player, obstacle, position) { }

    //TODO: il player si attacca alle spine e ci rimane impalato
    public override void StartDeath()
    {
        bloodParticles = Resources.Load<GameObject>("Prefab/Particles/BloodParticles");
        GameObject blood = GameObject.Instantiate(bloodParticles, position, Quaternion.identity);
        GameObject.Destroy(blood.gameObject, 1f);
        player.SetActiveRagdoll(true);
        //player.transform.parent = obstacle.GetDeadlyGameObject().transform;
        //player.RagdollPieces[0].isKinematic = true;

    }
}
