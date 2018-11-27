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
        SpawnBloodParticles();
        SpawnBloodStainOnPlayer();
        SpawnBloodFountain();
        bloodStain = Resources.Load<GameObject>("Prefab/Particles/BloodStain");
        GameObject bloodStainInstance = GameObject.Instantiate(bloodStain, position, Quaternion.identity, spikes.transform);
        bloodStainInstance.GetComponent<SpriteRenderer>().sortingOrder = spikes.GetComponent<SpriteRenderer>().sortingOrder + 1;

        player.SetActiveRagdoll(true);
        player.GetComponent<PlayerAppearence>().ChangeBodyPiecesSprite("spike");

        PlayDeathSong();
    }
}
