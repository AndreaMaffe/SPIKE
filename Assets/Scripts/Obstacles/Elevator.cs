﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : Obstacle
{
    public float speed;
    [Tooltip("Maximum vertical height reach by the platform")]
    public float maxHeight;

    private GameObject platform;
    private Animator animator;

    private Vector3 originalPlatformPosition;
    private Vector3 direction;

    //start apposito per gli ostacoli, usare questo anziché Start().
    protected override void StartObstacle()
    {
        platform = transform.Find("Platform").gameObject;
        animator = GetComponent<Animator>();

        direction = Vector3.down;

        DisablePhysics();
    }

    //update apposito per gli ostacoli, usare questo anziché Update().
    protected override void UpdateObstacle()
    {
        Vector3 newPlatformPosition = new Vector3(originalPlatformPosition.x, platform.transform.position.y, 0) + direction * speed/1000;
        platform.transform.position = newPlatformPosition;

        if (platform.transform.position.y >= maxHeight)
            InvertDirection();
    }

    //chiamato al RunLevel()
    protected override void WakeUp()
    {
        //permette di entrare nell'UpdateObstacle()
        SetActive(true);

        //accendi la motosega
        animator.SetBool("On", true);

        EnablePhysics();
    }

    //chiamato al RetryLevel()
    protected override void Sleep()
    {
        //impedisce di entrare nell'UpdateObstacle()
        SetActive(false);

        //spegni la motosega
        animator.SetBool("On", false);

        //rimetti la piattaforma nella posizione originaria
        platform.transform.position = originalPlatformPosition;

        direction = Vector3.down;

        DisablePhysics();
    }

    public override void OnObstacleDropped()
    {
        base.OnObstacleDropped();
        originalPlatformPosition = platform.transform.position;
    }

    public void InvertDirection()
    {
        if (direction == Vector3.down)
            direction = Vector3.up;
        else direction = Vector3.down;

    }

    public override ObstacleType GetObstacleType()
    {
        return ObstacleType.Elevator;
    }


}