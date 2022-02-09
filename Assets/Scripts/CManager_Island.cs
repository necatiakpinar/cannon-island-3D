using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CManager_Island : MonoBehaviour
{
    public int islandIndex; // Level index, literally.
    public List<CController_Castle> listCastle;
    public int threeStartMinBallCount;
    public int twoStartMinBallCount;
    private void Awake()
    {
        islandIndex = SceneManager.GetActiveScene().buildIndex;
    }
    public bool IsAllCastlesCollapsed()
    {
        foreach (CController_Castle controllerCastle in listCastle)
        {
            if (!controllerCastle.isCastleCollapsed) return false;
        }
        return true;
    }
}
