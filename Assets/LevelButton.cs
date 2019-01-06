using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour {

    public Image[] stars;
    public Color32 starAssigned;
    public Color32 starNotAssigned;

    public void AssignStars(int amount)
    {
        for (int i = 0; i < stars.Length; i++)
        {
            if ((i + 1) <= amount)
                stars[i].color = starAssigned;
            else
                stars[i].color = starNotAssigned;

        }

    }
}
