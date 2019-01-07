using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeathStats : MonoBehaviour {

    public Text overallDeathText;

    private SaveManager saveManager;

	void Start () {

        saveManager = SaveManager.SaveManagerInstance;
        saveManager = SaveUtility.LoadObject(saveManager, "saveFile");
        overallDeathText.text = saveManager.totalDeathsCounter.ToString();

    }

    private void Update()
    {
        overallDeathText.text = saveManager.totalDeathsCounter.ToString();
    }

}
