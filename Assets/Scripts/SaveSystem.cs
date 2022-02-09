using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class SaveData
{
    public List<int> listLevelStar;
    public int MaximumLevelIndex;
    public int CurrentLevelIndex;
    public bool SoundIsOn;
    public bool VibrationIsOn;

    //Tutorial
    public int IsFireTutorialFinished;
    public SaveData()
    {
        SoundIsOn = true;
        VibrationIsOn = true;
        
        MaximumLevelIndex = 0;
        CurrentLevelIndex = 0;
        listLevelStar = new List<int>();
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings - 1; i++)
        {
            listLevelStar.Add(0);
        }

        //Tutorial
        IsFireTutorialFinished = 0;
        
    }
    
};
public class SaveSystem : MonoBehaviour
{
    private const string DATANAME = "SaveData.json";
    public static SaveData Data = new SaveData();

    public static void Save()
    {
        string filePath = Application.persistentDataPath + "/" + DATANAME;
        string rawJson = JsonUtility.ToJson(Data);
        System.IO.File.WriteAllText(filePath, rawJson);
    }
    public static void Load()
    {
        string filePath = Application.persistentDataPath + "/" + DATANAME;
        if(File.Exists(filePath))
        {
            string fileContents = File.ReadAllText(filePath);
            Data = JsonUtility.FromJson<SaveData>(fileContents);   
        }
    }   
}

