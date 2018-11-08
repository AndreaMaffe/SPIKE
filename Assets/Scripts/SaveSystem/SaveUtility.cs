using UnityEngine;
using System.IO;

public class SaveUtility
{
    //Metodo utilizzato per salvare un oggetto generico (qualsiasi classe) in un file sottoforma di stringa JSON
    public static void SaveObject<T>(T objectParam, string fileName)
    {
        string json = JsonUtility.ToJson(objectParam);
        SaveJsonOnFile(json, fileName);
    }

    //Metodo utilizzato per caricare un oggetto generico (qualsiasi classe) da un file
    public static T LoadObject<T>(T objectParam, string fileName)
    {
        string json = LoadJsonFromFile(fileName);
        objectParam = JsonUtility.FromJson<T>(json);
        return objectParam;
    }

    //Metodo utilizzato per salvare una stringa JSON nel file Application.persistentDataPath/fileName
    public static void SaveJsonOnFile(string json, string fileName)
    {
        File.WriteAllText(Application.persistentDataPath + "/" + fileName, json);
    }

    //Metodo utilizatto per caricare una stringa JSON dal file Application.persistentDataPath/fileName
    public static string LoadJsonFromFile(string fileName)
    {
        string json = File.ReadAllText(Application.persistentDataPath + "/" + fileName);
        return json;
    }

}