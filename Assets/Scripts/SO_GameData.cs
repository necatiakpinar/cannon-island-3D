using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


[CreateAssetMenu(menuName = "gameData", order = 0)]
public class SO_GameData : ScriptableObject
{
    public int TotalLevelIndex => SceneManager.sceneCountInBuildSettings - 1; //TotalLevels - 1 - MainMenuScene
    public int MaximumLevelIndex;
    public int CurrentLevelIndex;
    public List<int> listLevelStar; 
    // public bool SoundOn;
    // public bool VibrationOn;

}