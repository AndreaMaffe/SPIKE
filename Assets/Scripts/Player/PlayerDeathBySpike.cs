using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeathBySpike : PlayerDeathEvent
{
    private GameObject bloodParticles;
    private GameObject bloodStain;
    private GameObject spikes;

    public PlayerDeathBySpike(Player player, Obstacle obstacle, Vector3 position, GameObject spikes) : base(player, obstacle, position)
    {
        this.spikes = spikes;
    }

    //TODO: il player si attacca alle spine e ci rimane impalato
    public override void StartDeath()
    {
        bloodParticles = Resources.Load<GameObject>("Prefab/Particles/BloodParticles");
        bloodStain = Resources.Load<GameObject>("Prefab/Particles/BloodStain");
        GameObject blood = GameObject.Instantiate(bloodParticles, position, Quaternion.identity);
        GameObject.Destroy(blood.gameObject, 1f);
        GameObject bloodStainInstance = GameObject.Instantiate(bloodStain, position, Quaternion.identity, spikes.transform);
        bloodStainInstance.GetComponent<SpriteRenderer>().sortingOrder = obstacle.GetComponent<SpriteRenderer>().sortingOrder + 1;

        player.SetActiveRagdoll(true);
    }
}
