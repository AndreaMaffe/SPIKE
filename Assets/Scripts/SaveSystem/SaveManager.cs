using System;

//La classe SaveManager serve da contenitore di dati persistenti.
[Serializable]
public class SaveManager
{
    //Variabile relativa al massimo livello raggiunto dal giocatore
    public int maxUnlockedLevel;
    //Variabile relativa al totale dei livelli inseriti
    public int totalLevels;
    //Variabile relativa al volume della musica
    public bool musicVolume;
    //Variabile relativa alle stelle per livello;
    public int[] stars;
    //Variabile relativa al numero totali di morti;
    public int totalDeathsCounter;

    private static SaveManager saveManagerInstance;

    private SaveManager()
    {
        maxUnlockedLevel = 1;
        totalLevels = 100;
        musicVolume = true;
        stars = new int[totalLevels];
        totalDeathsCounter = 0;
    }

    public static SaveManager SaveManagerInstance
    {
        get
        {
            if (saveManagerInstance == null)
            {
                saveManagerInstance = new SaveManager();
            }
            return saveManagerInstance;
        }
    }
}

