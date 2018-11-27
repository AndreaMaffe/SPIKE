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
    public float musicVolume;

    private static SaveManager saveManagerInstance;

    private SaveManager()
    {
        maxUnlockedLevel = 0;
        totalLevels = 0;
        musicVolume = 1;
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

