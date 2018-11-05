using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "Level", order = 1)]
public class Level : ScriptableObject
{

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
    public MovementType type;
    public float startTime;
}

public enum ObstacleType
{
    Bomb,
    Cannon,
    Spring,
    Pendolum,
    Laser
   
}

public enum MovementType
{
    Jump,
    Stop,
    Move,
    WaitingForJump
}
