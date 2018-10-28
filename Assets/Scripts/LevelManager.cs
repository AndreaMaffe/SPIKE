using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

    //array che contiene gli scruptable object di tutti i livelli
    Level[] allLevels;
    //numero del livello attuale partendo da 1
    private int currentLevel;

    private void Start()
    {
        LoadLevel();
    }

    void LoadLevel() {

    }

    public Level GetActualLevel() {
        //i livelli partono da 1
        return allLevels[currentLevel - 1];
    }
}
