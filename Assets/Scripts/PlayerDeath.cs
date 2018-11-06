using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerDeathEvent : MonoBehaviour
{
    private GameObject obstacle;
    private Vector3 position;

    private Animator playerAnimator;


    public PlayerDeathEvent (GameObject player, GameObject obstacle, Vector3 position)
    {
        this.obstacle = obstacle;
        this.position = position;
        this.playerAnimator = player.GetComponent<Animator>();
    }

    public abstract void StartDeath();

}
