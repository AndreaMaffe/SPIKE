using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonUpDown : MonoBehaviour {

    public Animator animator;
	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
        LevelManager.runLevelEvent += HideButtons;
        ObstacleDragger.onDraggingObstacle += HideButtons;
        ObstacleDragger.onEndDraggingObstacle += ShowButtons;
        AnchorPoint.onDraggingObstacle += HideButtons;
        AnchorPoint.onEndDraggingObstacle += ShowButtons;
        LevelManager.retryLevelEvent += ShowButtons;

    }

    public void HideButtons() {
        animator.SetBool("Up", false);
    }

    public void ShowButtons()
    {
        animator.SetBool("Up", true);
    }

    private void OnDisable()
    {
        AnchorPoint.onDraggingObstacle -= HideButtons;
        AnchorPoint.onEndDraggingObstacle -= ShowButtons;
        LevelManager.runLevelEvent -= HideButtons;
        ObstacleButton.onDraggingObstacle -= HideButtons;
        ObstacleButton.onEndDraggingObstacle -= ShowButtons;
        LevelManager.retryLevelEvent -= ShowButtons;
    }
}
