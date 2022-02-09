using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks; 
//using HomaGames.HomaBelly;
using Cinemachine;

// public struct SLayers
// {
//     public int Player;
//     public int Enemy;
//     public int Water;
//     public int Castle;
//     public int DeathTrigger;
//     public int PlayerProjectile;
//     public int EnemyProjectile;
//     public int Island;
//     public int AbilityPortal;
//     public int PowerUp;
//     public int AbilityBarrier;
//     public int Barrier;
// }
public class CManager_Game : MonoBehaviour
{
    [Header("Attrributes")]
    public SO_GameData gameData; // Use this later
    public int currentLevelIndex;
    public int levelIndexOffset = 1; // + 1 as an offset to main menu scene
    public float waitTime_WinSequence = 1.5f;

    [Header("Attrributes| Watch")]
    public ELevelStarState starState;
    public bool gameIsOn;
    public bool winCondition_Destruction;
    public bool winCondition_Enemy;
    public bool loseCondition_BallStock;


    [Header("Managers")]
    public CManager_Enemy managerEnemy;
    public CManager_Ball managerBall;
    public CManager_UI managerUI;
    public CManager_Island managerIsland;

    [Header("Pools")]
    public SO_Pool_Ball poolBall;

    [Header("Controllers")]
    public CController_Ship controllerShip;
    public CController_Player controllerPlayer;
    public CController_Camera controllerCamera;
    public CController_Castle controllerCastle;
    

    [Header("Containers")]
    public Transform containerCannonBall;
    public CinemachineVirtualCamera cmVirtualCamera;

    [Header("Ball Camera")]
    [SerializeField] private float ballCamera_FOV = 54.7f;
    [SerializeField] private Vector3 ballCamera_FollowOffset = new Vector3(0,1.04f,0);
    [SerializeField] private Vector3 ballCamera_Aim = Vector3.zero;
    [SerializeField] private Vector3 ballCamera_ScreenXY = new Vector3(0.5f,0.74f,0);

    private float ballCamera_DefaultFOV;
    private Vector3 ballCamera_DefaultFollowOffset;
    private Vector3 ballCamera_DefaultAim;
    private Vector3 ballCamera_DefaultScreenXY;

    public static CManager_Game Instance;

    private Vector3 gravityDefault = new Vector3(0,-9.81f,0);

    
    private void Awake() 
    {
        Instance = this;    

        Physics.gravity = gravityDefault;

        managerBall = GetComponentInChildren<CManager_Ball>();
        controllerPlayer = (controllerPlayer != null) ? controllerPlayer : GameObject.FindObjectOfType<CController_Player>();
        controllerShip = (controllerPlayer != null) ? controllerPlayer.GetComponentInParent<CController_Ship>() : null;
        controllerCamera = (controllerCamera != null) ? controllerCamera : GameObject.FindObjectOfType<CController_Camera>();
        controllerCastle = (controllerCastle != null) ? controllerCastle : GameObject.FindObjectOfType<CController_Castle>();

        CUtility.Initalize();
    }
    private void Start() 
    {
        starState = ELevelStarState.THREE;
        winCondition_Destruction = false;
        winCondition_Enemy = false;

        StartCoroutine(InstantiateHomaBelly());
    }

    private IEnumerator InstantiateHomaBelly()
    {
        yield return new WaitForSeconds(1);
        // if (!HomaBelly.Instance.IsInitialized)
        // {
        //     print("NOT Initialized!");
        //     StartCoroutine(InstantiateHomaBelly());
        // }
        // else
        // {
        //     print("Initialized!");
        // }

    }
    public void LoadLevel(int pIndex)
    {
        SaveSystem.Data.CurrentLevelIndex = pIndex;
        SaveSystem.Save();
        SceneManager.LoadScene(pIndex + levelIndexOffset); 
    }
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("LevelMap");
    }
    public void LoadNextLevel()
    {
        if (SaveSystem.Data.CurrentLevelIndex < gameData.MaximumLevelIndex)
        {
            SaveSystem.Data.CurrentLevelIndex++;
            SaveSystem.Save();
            LoadLevel(SaveSystem.Data.CurrentLevelIndex);
        }
    }
    public void ReloadLevel()
    {
       LoadLevel(SaveSystem.Data.CurrentLevelIndex);
    }
    public void PlayerWon()
    {
        CManager_Sound.Instance.Play("Player_Cheer", 1, 2f);
        controllerShip.StartCrewVictoryAnimations();
        controllerCamera.StartEndingScene();

        if(controllerPlayer.remainingBallCount >= managerIsland.threeStartMinBallCount) 
        {
            starState = ELevelStarState.THREE;
            SaveSystem.Data.listLevelStar[SaveSystem.Data.CurrentLevelIndex] = 3;
            SaveSystem.Save();
        }
        else if(controllerPlayer.remainingBallCount >= managerIsland.twoStartMinBallCount)
        {
            starState = ELevelStarState.TWO;
            if (SaveSystem.Data.listLevelStar[SaveSystem.Data.CurrentLevelIndex] < 2)
            {
                SaveSystem.Data.listLevelStar[SaveSystem.Data.CurrentLevelIndex] = 2;
                SaveSystem.Save();
            }
        } 
        else 
        {
            starState = ELevelStarState.ONE;
            if (SaveSystem.Data.listLevelStar[SaveSystem.Data.CurrentLevelIndex] < 1)
            {
                SaveSystem.Data.listLevelStar[SaveSystem.Data.CurrentLevelIndex] = 1;
                SaveSystem.Save();
            }
        }

        if(SaveSystem.Data.CurrentLevelIndex == gameData.MaximumLevelIndex && gameData.MaximumLevelIndex < gameData.TotalLevelIndex - 1) 
        {
            gameData.MaximumLevelIndex++;
            SaveSystem.Data.MaximumLevelIndex = gameData.MaximumLevelIndex;
            SaveSystem.Save();
        }
        
        // managerUI.ShowWinPanel();        
    }
    public void PlayerLost()
    {
        controllerShip.StartCrewDefeatAnimations();
        controllerCamera.StartEndingScene(true);
        controllerShip.StartDestruction();
        starState = ELevelStarState.ZERO;
    }
    public void CheckBallCount()
    {
        int remaining = controllerPlayer.remainingBallCount;

        if(remaining >= managerIsland.threeStartMinBallCount) starState = ELevelStarState.THREE;
        else if(remaining >= managerIsland.twoStartMinBallCount) starState = ELevelStarState.TWO;
        else starState = ELevelStarState.ONE;

        if(controllerPlayer.createdBallCount == controllerPlayer.maximumBallCount) loseCondition_BallStock = true;
    }
    public async void StartWinSequence()
    {  
        await Task.Delay((int)(1000 * waitTime_WinSequence));
        PlayerWon();
    }


    // TESTING,
    
    
    public void FollowBall(Transform pBall)
    {
        CinemachineTransposer transposer = cmVirtualCamera.GetCinemachineComponent<CinemachineTransposer>();
        CinemachineComposer composer = cmVirtualCamera.GetCinemachineComponent<CinemachineComposer>();

        ballCamera_DefaultFollowOffset = transposer.m_FollowOffset;
        ballCamera_DefaultAim = composer.m_TrackedObjectOffset;
        ballCamera_DefaultFOV = cmVirtualCamera.m_Lens.FieldOfView;
        ballCamera_DefaultScreenXY = new Vector3(composer.m_ScreenX, composer.m_ScreenY, 0);
        composer.m_TrackedObjectOffset = ballCamera_Aim;
        composer.m_ScreenX = ballCamera_ScreenXY.x;
        composer.m_ScreenY = ballCamera_ScreenXY.y;

        transposer.m_FollowOffset = ballCamera_FollowOffset;
        cmVirtualCamera.m_Lens.FieldOfView = ballCamera_FOV;
        cmVirtualCamera.transform.position = Vector3.zero; // ?
        cmVirtualCamera.m_Follow = pBall;
        cmVirtualCamera.m_LookAt = pBall;
    }

    public void ResetCameraTarget()
    {
        
        CinemachineTransposer transposer = cmVirtualCamera.GetCinemachineComponent<CinemachineTransposer>();
        CinemachineComposer composer = cmVirtualCamera.GetCinemachineComponent<CinemachineComposer>();

        cmVirtualCamera.m_Follow = controllerPlayer.transformCannonBody;
        cmVirtualCamera.m_LookAt = controllerPlayer.transformBallSpawner;
        cmVirtualCamera.m_Lens.FieldOfView = ballCamera_DefaultFOV;
        transposer.m_FollowOffset = ballCamera_DefaultFollowOffset;
        composer.m_TrackedObjectOffset = ballCamera_DefaultAim;
        composer.m_ScreenX = ballCamera_DefaultScreenXY.x;
        composer.m_ScreenY = ballCamera_DefaultScreenXY.y;
    }
}

