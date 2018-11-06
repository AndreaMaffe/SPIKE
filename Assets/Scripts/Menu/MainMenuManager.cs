using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour {

    //Metodo chiamato dopo aver premuto il tasto Play
    public void StartGame()
    {
        //Cambiare il LoadScene con il nome definitivo della scena che continene il livello di gioco
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
    }

    //Metodo chiamato dopo aver premuto il tasto Levels
    public void LevelSelectionButtonPressed()
    {
        SceneManager.LoadScene("LevelSelection");
    }
}
