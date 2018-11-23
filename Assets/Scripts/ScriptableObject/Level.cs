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
    public Vector3 startingPoint;
    public Vector3 endingPoint;
    //Lista della posizione delle piattaforme presenti nel livello;
    public PlatformData[] platformDatas;
    //Lista degli ostacoli presenti nel livello
    public ObstacleData[] obstacleDatas;
    //Lista dei movimenti del giocatore per livello
    public MovementData[] movementDatas;

}

[System.Serializable]
public struct ObstacleData
{
    public ObstacleType type;
    public int obstacleMaxAmount;
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
