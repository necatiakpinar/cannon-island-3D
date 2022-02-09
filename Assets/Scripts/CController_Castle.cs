using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class CController_Castle : MonoBehaviour
{

    [Range(0f, 1f)]public float winPercentage = 0.5f;

    [Header("Watch")]
    [SerializeField] private int totalJointCount;
    [SerializeField] private int currentJointCount;
    public float currentPercentage;
    [SerializeField] private List<FixedJoint> joints;

    public bool isCastleCollapsed = false;

    private void Awake()
    {
        int towerCount = transform.childCount;
        
        for(int  towerIndex = 0; towerIndex < towerCount; towerIndex++)
        {
            Transform towerParent = transform.GetChild(towerIndex);
            
            for(int towerPart = 0; towerPart < towerParent.childCount; towerPart++)
            {
                Transform towerPartParent = transform.GetChild(towerIndex).GetChild(towerPart);
                for (int meshIndex = 0; meshIndex < towerPartParent.childCount; meshIndex++)
                {
                    FixedJoint meshJoint = towerPartParent.GetChild(meshIndex).GetComponent<FixedJoint>();
                    if(meshJoint != null) joints.Add(meshJoint); 
                }
            }
        } 
        totalJointCount = joints.Count;
        currentJointCount = totalJointCount;
        // currentPercentage = (float)currentJointCount / (float)totalJointCount;

        isCastleCollapsed = false;
    }
    private void Start() 
    {
        Async_JointCheck();
    }
    public async void Async_JointCheck()
    {
        await Task.Delay((int)(1000 * 1));
        if (!isCastleCollapsed)
        {
            CheckJoints();
            Async_JointCheck();
        }
    }
    private void CheckJoints()
    {
        for(int index = 0; index < joints.Count; index++)
        {
            if(joints[index] == null) joints.RemoveAt(index);
        }
        currentJointCount = joints.Count;
        currentPercentage = (float)currentJointCount / (float)totalJointCount;
        
        if(currentJointCount != totalJointCount) // which means castle got hit
        {
            CManager_Game.Instance.managerUI.UpdateLevelSlider();
        } 

        if(currentPercentage <= winPercentage) 
        {
            isCastleCollapsed = true;
            if (CManager_Game.Instance.managerIsland.IsAllCastlesCollapsed())
            {
                CManager_Game.Instance.winCondition_Destruction = true;
                if(CManager_Game.Instance.winCondition_Enemy) CManager_Game.Instance.StartWinSequence();
            }
        }
    }
}
