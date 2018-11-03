using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayGame : MonoBehaviour {

    public void startGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}