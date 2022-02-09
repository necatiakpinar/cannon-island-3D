using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
[DefaultExecutionOrder(+150)]
public class CManager_UI : MonoBehaviour
{
    public bool inMainMenu;

    [Header("Prefabs")]
    [SerializeField] private GameObject PF_LevelMapNode;

    [Header("Panels")]
    [SerializeField] private GameObject panelInGame;
    [SerializeField] private GameObject panelWin;
    [SerializeField] private GameObject panelFail;
    [SerializeField] private GameObject panelSettings;
    [SerializeField] private GameObject panelLevelMap;
    [SerializeField] private GameObject panelPause;

    [Header("Buttons")]
    //[SerializeField] private Button button_Gameplay_LoadNextLevel;
    [SerializeField] private Button button_Gameplay_Fire;
    [SerializeField] private Button button_Gameplay_Settings;
    [SerializeField] private Button button_Gameplay_SettingsClose;
    [Space]
    //Pause
    [SerializeField] private Button button_Gameplay_Pause;
    [SerializeField] private Button button_Gameplay_PauseClose;
    [SerializeField] private Button button_Gameplay_PauseResume;
    [SerializeField] private Button button_Gameplay_PauseReplay;
    [SerializeField] private Button button_Gameplay_PauseMenu;
    [Space]

    [SerializeField] private Button button_Settings_Sound;
    [SerializeField] private Button button_Settings_Vibration;

    [SerializeField] private Button button_Win_Continue;
    [SerializeField] private Button button_Win_Replay;
    [SerializeField] private Button button_Win_Menu;
    [SerializeField] private Button button_Fail_Replay;
    [SerializeField] private Button button_Fail_Menu;
    [Header("Sprites")]
    [SerializeField] private Sprite tickedBox;
    [SerializeField] private Sprite untickedBox;
    [SerializeField] private Sprite starSpriteWinPanel;
    [Space]
    [SerializeField] private Sprite CannonBall1Star;
    [SerializeField] private Sprite CannonBall2Star;
    [SerializeField] private Sprite CannonBall3Star;
    [Header("Sliders")]
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Slider levelSlider;
    ELevelStarState starCount;
    [Header("Images")]
    [SerializeField] private Image star2_WinPanel;
    [SerializeField] private Image star3_WinPanel;
    [SerializeField] private Image CannonBallStarsImage;
    [SerializeField] private Image image_FireCooldown;
    [Header("Texts")]
    [SerializeField] private TMP_Text textCurrentLevel;
    [SerializeField] private TMP_Text textNextLevel;
    [SerializeField] private TMP_Text BallCount;
    [SerializeField] private TMP_Text winLevelTxt;
    [SerializeField] private TMP_Text failLevelTxt;
    [Header("Animators")]    
    [SerializeField] private Animator animatorStars;

    [Header("Tutorial")]
    [SerializeField] private GameObject tutorialFire;
    public bool soundOn = true;
    public bool vibrationOn = true;
    

    #region "Managers"
    private CManager_Ball managerBall;
    private CController_Player controllerPlayer;

    #endregion
    #region Base
    private void Awake() 
    {
        //Button Initializers
        if(button_Gameplay_Fire != null)
        {
            button_Gameplay_Fire.onClick.AddListener(() => {
                ButtonEvent_Fire();
            });
        }
        if(button_Gameplay_Settings != null)
        {
            button_Gameplay_Settings.onClick.AddListener(() => {
                CManager_Sound.Instance.Play("UI_ButtonClick");
                ButtonEvent_ShowSettingsPanel();
            });
        }
        if(button_Gameplay_SettingsClose != null)
        {
            button_Gameplay_SettingsClose.onClick.AddListener(() => {
                CManager_Sound.Instance.Play("UI_ButtonClick");
                ButtonEvent_HideSettingsPanel();
            });
        }
        if(button_Settings_Sound != null)
        {
            button_Settings_Sound.onClick.AddListener(() => {
                CManager_Sound.Instance.Play("UI_ButtonClick");
                ButtonEvent_Sound();
            });
        }
        if(button_Settings_Vibration != null)
        {
            button_Settings_Vibration.onClick.AddListener(() => {
                CManager_Sound.Instance.Play("UI_ButtonClick");
                ButtonEvent_Vibration();
            });
        }
        if(button_Win_Continue != null)
        {
            button_Win_Continue.onClick.AddListener(() => {
                CManager_Sound.Instance.Play("UI_ButtonClick");
                ButtonEvent_LoadNextLevel();
            });
        }
        if(button_Win_Replay != null && button_Fail_Replay != null && button_Gameplay_PauseReplay != null)
        {
            button_Win_Replay.onClick.AddListener(() => {
                CManager_Sound.Instance.Play("UI_ButtonClick");
                ButtonEvent_RestartLevel();
            });
            button_Fail_Replay.onClick.AddListener(() => {
                CManager_Sound.Instance.Play("UI_ButtonClick");
                ButtonEvent_RestartLevel();
            });
            button_Gameplay_PauseReplay.onClick.AddListener(() => {
                CManager_Sound.Instance.Play("UI_ButtonClick");
                ButtonEvent_RestartLevel();
            });
        }
        if(button_Win_Menu != null && button_Fail_Menu != null && button_Gameplay_PauseMenu != null)
        {
            button_Win_Menu.onClick.AddListener(() => {
                CManager_Sound.Instance.Play("UI_ButtonClick");
                ButtonEvent_LoadMainMenu();
            });
            button_Fail_Menu.onClick.AddListener(() => {
                CManager_Sound.Instance.Play("UI_ButtonClick");
                ButtonEvent_LoadMainMenu();
            });
            button_Gameplay_PauseMenu.onClick.AddListener(() => {
                CManager_Sound.Instance.Play("UI_ButtonClick");
                ButtonEvent_LoadMainMenu();
            });
        }
        if(button_Gameplay_Pause != null)
        {
            button_Gameplay_Pause.onClick.AddListener(() => {
                CManager_Sound.Instance.Play("UI_ButtonClick");
                ButtonEvent_ShowPausePanel();
            });
        }
        if(button_Gameplay_PauseClose != null && button_Gameplay_PauseResume != null)
        {
            button_Gameplay_PauseClose.onClick.AddListener(() => {
                CManager_Sound.Instance.Play("UI_ButtonClick");
                ButtonEvent_HidePausePanel();
            });

            button_Gameplay_PauseResume.onClick.AddListener(() => {
                CManager_Sound.Instance.Play("UI_ButtonClick");
                ButtonEvent_HidePausePanel();
            });
        }
        Time.timeScale = 1;
        //  Set Sliders
      
        // Level slider eklenmeli


        if (SaveSystem.Data.CurrentLevelIndex == 0 && SaveSystem.Data.IsFireTutorialFinished == 0) tutorialFire.SetActive(true);
        else  tutorialFire.SetActive(false);
        
    }
    private void Start() 
    {
        //Managers
        managerBall = CManager_Game.Instance.managerBall;
        controllerPlayer = CManager_Game.Instance.controllerPlayer;

        if(!inMainMenu)
        {
            healthSlider.minValue = 0;
            healthSlider.maxValue = CManager_Game.Instance.controllerPlayer.Properties.Health;            
            UpdateHealthSlider();

            levelSlider.maxValue =  1 -  CManager_Game.Instance.controllerCastle.winPercentage;

            UpdateCannonBallCount();
            failLevelTxt.text = "Level "+ (SaveSystem.Data.CurrentLevelIndex + 1).ToString();
            winLevelTxt.text = "Level "+ (SaveSystem.Data.CurrentLevelIndex + 1).ToString();
            
            if(panelWin != null) HideEndingPanel();

            UpdateLevelText();

            if(CManager_Game.Instance.managerIsland.islandIndex > 1) tutorialFire.SetActive(false);

            SaveSystem.Load();
            soundOn = SaveSystem.Data.SoundIsOn;
            vibrationOn = SaveSystem.Data.VibrationIsOn;
            button_Settings_Sound.gameObject.GetComponent<Image>().sprite = (soundOn) ? tickedBox : untickedBox;
            button_Settings_Vibration.gameObject.GetComponent<Image>().sprite = (vibrationOn) ? tickedBox : untickedBox;
        }
        InitializeLevelMap();
    }
    private void InitializeLevelMap()
    {
        if(panelLevelMap == null) return;
        // panelLevelMap.SetActive(true);

        int max = (CManager_Game.Instance.gameData.MaximumLevelIndex == 0) ? 1 : CManager_Game.Instance.gameData.MaximumLevelIndex + 1;
        for(int index = 0; index < max; index++)
        {
            int levelIndex = (max - 1) - index;

            GameObject newLevelNode = GameObject.Instantiate(CManager_Game.Instance.managerUI.PF_LevelMapNode, panelLevelMap.transform);
            newLevelNode.name = $"LevelMapNode_{levelIndex.ToString()}";

            Button button = newLevelNode.GetComponent<Button>();
            if(button == null) break;

            button.onClick.AddListener( () => { ButtonEvent_LoadLevel(levelIndex); } );
            Text text = button.GetComponentInChildren<Text>();
            text.text = (levelIndex + 1).ToString();
        }
    }
    #endregion 
    public void UpdateLevelSlider()
    {
        // 
        levelSlider.value = 1 - CManager_Game.Instance.controllerCastle.currentPercentage;
        // levelSlider.maxValue =  CManager_Game.Instance.controllerCastle.winPercentage;
    }
    public void UpdateHealthSlider()
    {
        healthSlider.value = controllerPlayer.currentHealth;
    }
    public void UpdateLevelText()
    {
        textCurrentLevel.text = CManager_Game.Instance.managerIsland.islandIndex.ToString(); 
        textNextLevel.text = (CManager_Game.Instance.managerIsland.islandIndex + 1).ToString(); 
    }
    public void UpdateCannonBallCount()
    {
        if (controllerPlayer != null)
        {
            BallCount.text = ""+controllerPlayer.remainingBallCount;
            if(controllerPlayer.remainingBallCount >= CManager_Game.Instance.managerIsland.threeStartMinBallCount)  return;
            if(controllerPlayer.remainingBallCount <= CManager_Game.Instance.managerIsland.threeStartMinBallCount && 
            controllerPlayer.remainingBallCount >= CManager_Game.Instance.managerIsland.twoStartMinBallCount)
            {
                CannonBallStarsImage.sprite = CannonBall2Star;
                return;
            }
            CannonBallStarsImage.sprite = CannonBall1Star;
        }
    }

    #region Toggles
    public void HideGameplayUI()
    {
        panelInGame.gameObject.SetActive(false);
    }
    public void ShowWinPanel(bool pLost = false)
    {
        button_Win_Continue.gameObject.SetActive(!pLost);
        panelWin.SetActive(true);
        CManager_Game.Instance.gameIsOn = false;
        if(CManager_Game.Instance.starState == ELevelStarState.THREE) 
        {
            star2_WinPanel.sprite = starSpriteWinPanel;
            star3_WinPanel.sprite = starSpriteWinPanel;
            animatorStars.SetTrigger("Star3");
            return;
        }
        else if(CManager_Game.Instance.starState == ELevelStarState.TWO) 
        {
            star2_WinPanel.sprite = starSpriteWinPanel;
            animatorStars.SetTrigger("Star2");
        }
        else if(CManager_Game.Instance.starState == ELevelStarState.ONE) 
        {
            animatorStars.SetTrigger("Star1");
        }

    } 
    public void ShowFailPanel()
    {
        CManager_Sound.Instance.Play("Level_Fail");
        panelFail.SetActive(true);
        Time.timeScale = 0;
    }
    public void HideEndingPanel() => panelWin.SetActive(false);

    public void EnableFireButton()
    {
        button_Gameplay_Fire.interactable = true;
        image_FireCooldown.gameObject.SetActive(false);
    } 
    public void DisableFireButton() 
    {
        button_Gameplay_Fire.interactable = false;
        image_FireCooldown.gameObject.SetActive(true);
    }
    public void SetFireCooldownFill(float pFill)
    {
        image_FireCooldown.fillAmount = pFill;
    }
    #endregion

    #region Button Events 
    public void ButtonEvent_ShowSettingsPanel() 
    {
        panelSettings.SetActive(true);
        Time.timeScale = 0;
        // Input'ları kapat
    } 
    public void ButtonEvent_HideSettingsPanel()
    {
        panelSettings.SetActive(false);
        Time.timeScale = 1;
    } 
    public void ButtonEvent_ShowPausePanel()
    {
        panelPause.SetActive(true);
        Time.timeScale = 0;
    }
    public void ButtonEvent_HidePausePanel()
    {
        panelPause.SetActive(false);
        Time.timeScale = 1;
    }
    public void ButtonEvent_ShowLevelMapPanel() => panelLevelMap.SetActive(true);
    public void ButtonEvent_HideLevelMapPanel() => panelLevelMap.SetActive(false);
    public void ButtonEvent_LoadLevel(int pIndex) => CManager_Game.Instance.LoadLevel(pIndex);
    public void ButtonEvent_LoadMainMenu() => CManager_Game.Instance.LoadMainMenu();
    public void ButtonEvent_LoadNextLevel() => CManager_Game.Instance.LoadNextLevel();
    public void ButtonEvent_RestartLevel() => CManager_Game.Instance.ReloadLevel();

    public void ButtonEvent_Fire()
    {
        if(CManager_Game.Instance.gameIsOn && !CManager_Game.Instance.controllerPlayer.inCooldown)
        {
            if (SaveSystem.Data.CurrentLevelIndex == 0 && SaveSystem.Data.IsFireTutorialFinished == 0)
            {
                tutorialFire.SetActive(false);
                SaveSystem.Data.IsFireTutorialFinished = 1;
                SaveSystem.Save();
            }
            // if(controllerPlayer.autoFire) controllerPlayer.AutoFire();
            // else controllerPlayer.Fire();
            foreach(Transform child in CManager_Game.Instance.controllerShip.transform)
            {
                CController_Player player = child.GetComponent<CController_Player>();
                if(player != null) 
                {
                    if(player.autoFire) player.AutoFire();
                    else player.Fire();
                }
            }         
        }
    }

    private void ButtonEvent_Sound()
    {
        soundOn = !soundOn;
        button_Settings_Sound.gameObject.GetComponent<Image>().sprite = (soundOn) ? tickedBox : untickedBox;
        SaveSystem.Data.SoundIsOn = soundOn;
        SaveSystem.Save();
    }

    private void ButtonEvent_Vibration()
    {
        vibrationOn = !vibrationOn;
        button_Settings_Vibration.gameObject.GetComponent<Image>().sprite = (vibrationOn) ? tickedBox : untickedBox;
        SaveSystem.Data.VibrationIsOn = vibrationOn;
        SaveSystem.Save();
    }
    #endregion
}
