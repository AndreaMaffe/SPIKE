using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerMovement
{
    Run,
    Jump, 
    Stop
}

[CreateAssetMenu(fileName = "Level", menuName = "Level", order = 1)]
public class Level : ScriptableObject
{
    [Header("General")]
    public string description;
    public Vector3 startingPoint;
    public Vector3 endingPoint;
    [Header("Platforms")]
    public PlatformData[] platformDatas;
    [Header("Obstacles")]
    public ObstacleData[] obstacleDatas;
    [Header("Player movements")]
    public MovementData[] movementDatas;
    [Header("Stars")]
    public int pointsForOneStar;
    public int pointsForTwoStars;
    public int pointsForThreeStars;

}

[System.Serializable]
public struct ObstacleData
{
    public ObstacleType type;
    public int obstacleMaxAmount;
    public int points;
}

[System.Serializable]
public struct PlatformData
{
    public GameObject platformType;
    public Vector3 platformPos;
}

[System.Serializable]
public struct MovementData
{
    public PlayerMovement movement;
    public float startTime;
}





