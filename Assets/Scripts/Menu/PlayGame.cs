using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayGame : MonoBehaviour {

    public void StartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}