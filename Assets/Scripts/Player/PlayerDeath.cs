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

    protected void SpawnBloodFountain()
    {
        GameObject bloodFountain = Resources.Load<GameObject>("Prefab/Particles/BloodFountain");
        GameObject bloodFountainInstance = GameObject.Instantiate(bloodFountain, player.transform.position + new Vector3(0,0.7f,0), Quaternion.Euler(-90,20,0), player.transform.Find("Body").transform);
        player.GetComponent<PlayerAppearence>().AssignBloodFountainParticle(bloodFountainInstance);
    }

    protected void SpawnBloodStainOnPlayer() {
        GameObject bloodStain = Resources.Load<GameObject>("Prefab/Particles/BloodStainPlayer");
        GameObject bloodStainInstance = GameObject.Instantiate(bloodStain, position - new Vector3(Random.Range(-0.3f, 0.3f), Random.Range(0.4f, 0.7f), 0), Quaternion.identity, player.transform.Find("Body").transform);
        bloodStainInstance.GetComponent<SpriteRenderer>().sortingOrder = player.transform.Find("Body").GetComponent<SpriteRenderer>().sortingOrder + 1;
        bloodStainInstance.transform.localScale = new Vector3(1, 1, 0);
    }

}
