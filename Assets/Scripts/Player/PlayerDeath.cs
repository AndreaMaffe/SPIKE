using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerDeathEvent
{
    protected Player player;
    protected Vector3 position;
    protected Obstacle obstacle;

    //per l'effetto del sangue che tanto c'e' in ogni morte
    private GameObject bloodParticles;


    public PlayerDeathEvent (Player player, Obstacle obstacle, Vector3 position)
    {
        this.player = player;
        this.position = position;
        this.obstacle = obstacle;
    }

    public abstract void StartDeath();

    protected void SpawnBloodParticles() {
        bloodParticles = Resources.Load<GameObject>("Prefab/Particles/BloodParticles");
        GameObject blood = GameObject.Instantiate(bloodParticles, position, Quaternion.identity);
        GameObject.Destroy(blood.gameObject, 1f);
    }

}
