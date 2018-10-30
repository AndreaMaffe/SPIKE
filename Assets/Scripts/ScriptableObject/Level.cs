using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "Level", order = 1)]
public class Level : ScriptableObject
{

    //Lista della posizione delle piattaforme presenti nel livello;
    public platformData[] platformDatas;
    //Lista degli ostacoli presenti nel livello
    public ObstacleData[] obstacleDatas;

}
