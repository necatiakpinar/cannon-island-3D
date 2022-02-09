using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CController_Camera : MonoBehaviour
{
    
    [SerializeField] CinemachineVirtualCamera mainCamera;
    [SerializeField] CinemachineBlendListCamera victoryBlend;
    [SerializeField] CinemachineBlendListCamera defeatBlend;
    [SerializeField] CinemachineBlendListCamera currentBlend;

    private ICinemachineCamera currentCamera;
    private ICinemachineCamera lastCamera;

    public float waitTime_VictoryPanel = 1;
    public float waitTime_DefeatPanel = 3;
    private float waitTime_CurrentEnding;

    private bool isStarted = false;
    
    private void Start()
    {
        mainCamera = Camera.main.GetComponentInChildren<CinemachineVirtualCamera>();
    }
    private void Update()
    {
        if(!isStarted || currentBlend == null) return;
        if(lastCamera == currentBlend.LiveChild && !currentBlend.IsBlending) StartCoroutine("Coroutine_OpenEndingPanel");
    }
    public void StartEndingScene(bool pIsDefeat = false)
    {
        currentBlend = (pIsDefeat) ? defeatBlend : victoryBlend;
        if(currentBlend == null) return;
        
        currentBlend.Priority = mainCamera.Priority + 1;

        lastCamera = currentBlend.ChildCameras[currentBlend.ChildCameras.Length - 1].GetComponent<ICinemachineCamera>() as ICinemachineCamera;

        CManager_Game.Instance.managerUI.HideGameplayUI();
        isStarted = true;
        waitTime_CurrentEnding = (pIsDefeat) ? waitTime_DefeatPanel : waitTime_VictoryPanel;
    }
    private IEnumerator Coroutine_OpenEndingPanel()
    {
        isStarted = false;
        yield return new WaitForSeconds(waitTime_CurrentEnding);

        if(currentBlend == victoryBlend) CManager_Game.Instance.managerUI.ShowWinPanel();        
        else CManager_Game.Instance.managerUI.ShowFailPanel();        

        currentBlend = null;
    }
}
