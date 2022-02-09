using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CManager_Enemy : MonoBehaviour
{
    public int Count;
    public List<CController_Enemy> listEnemies;
    private void Awake()
    {
        Count = transform.childCount;
        for(int eIndex = 0; eIndex < transform.childCount; eIndex++)
        {
            CController_Enemy cEnemy = transform.GetChild(eIndex).GetComponent<CController_Enemy>();
            listEnemies.Add(cEnemy);
        }
    }    
    private void Start()
    {
        if(Count == 0) CManager_Game.Instance.winCondition_Enemy = true;
    }
    public void CheckEnemyDisplacements()
    {
        foreach(CController_Enemy cEnemy in listEnemies)
        {
            bool state = cEnemy.CheckDisplacement();
            if(state) cEnemy.DisableEvents();
        } 
    }
    public void PlayEnemiesVictoryAnimation()
    {
        foreach(CController_Enemy cEnemy in listEnemies) cEnemy.PlayVictoryAnimation();
        
    }
}
