using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class CManager_UILevelMap : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject prefabLevel;

    [Header("Level Variables")]
    public Transform levelParent;
    public Transform levelPositionParent;
    public List<Vector2> listLevelPosition;
    public List<CController_UILevel> listLevel;
    public SO_GameData gameData;
    [SerializeField] private GameObject SettingsPanel;
    [SerializeField] private Button SettingsBtn;
    [SerializeField] private Button button_Settings_Sound;
    [SerializeField] private Button button_Settings_Vibration;
    [SerializeField] private Button button_Settings_Close;
    [SerializeField] private Sprite untickedBox;
    [SerializeField] private Sprite tickedBox;
    private bool soundOn = true; 
    private bool vibrationOn = true;
 

    private void CreateLevels()
    {
        foreach (Transform levelObject in levelPositionParent)
        {
            RectTransform levelTransform = levelObject.GetComponent<RectTransform>();
            listLevelPosition.Add(levelTransform.anchoredPosition);
        }

        for (int i = 0; i < gameData.TotalLevelIndex; i++)
        {
            GameObject level = GameObject.Instantiate(prefabLevel,levelParent);
            CController_UILevel controllerLevel = level.GetComponent<CController_UILevel>();
            listLevel.Add(controllerLevel);
            listLevel[i].transform.name = "Level " +  (i + 1).ToString();
            listLevel[i].GetComponent<RectTransform>().anchoredPosition = listLevelPosition[i];
            listLevel[i].SetLevelId(i);
            listLevel[i].SetLevelButton(i);
            int starCount = gameData.listLevelStar[i];
            listLevel[i].SetStars(starCount);
            if (i < gameData.MaximumLevelIndex) listLevel[i].ActivateLevel();
            else if(i == gameData.MaximumLevelIndex) listLevel[i].NextLevel();
            else listLevel[i].DeactivateLevel();
        }
    }

    private void Awake() 
    {
        SaveSystem.Load();

        gameData.listLevelStar.Clear();
        for (int i = 0; i<SceneManager.sceneCountInBuildSettings - 1; i++)
        {
            gameData.listLevelStar.Add(0);
        }
        gameData.listLevelStar = SaveSystem.Data.listLevelStar;
        gameData.MaximumLevelIndex = SaveSystem.Data.MaximumLevelIndex;
        gameData.CurrentLevelIndex = SaveSystem.Data.CurrentLevelIndex;
        CreateLevels();

        SaveSystem.Save();
    }

    private void Start()
    {
        if(SettingsBtn != null)
        {
           ButtonEvent_ShowSettingPanel();
        }
        if(button_Settings_Sound != null)
        {
            button_Settings_Sound.onClick.AddListener(() => {
                ButtonEvent_Sound();
            });
        }
        if(button_Settings_Vibration != null)
        {
            button_Settings_Vibration.onClick.AddListener(() => {
                
            if(vibrationOn)
            {
                button_Settings_Vibration.gameObject.GetComponent<Image>().sprite = untickedBox;
                vibrationOn = false;
                return;
            }
            button_Settings_Vibration.gameObject.GetComponent<Image>().sprite = tickedBox;
            vibrationOn = true;
            });
        }
        if(button_Settings_Close != null)
        {
            ButtonEvent_CloseSettingPanel();
        }

    }
    public void ButtonEvent_ShowSettingPanel()
    {
         SettingsPanel.SetActive(true);
    }

    public void ButtonEvent_Sound()
    {
        if(soundOn) // Bu sound ve vibration u şimdilik göstermelik yaptım bunlar sonra save system den çekilecek
        {
            // Oyuna ses eklenince seleri buradan kapatırız
            button_Settings_Sound.gameObject.GetComponent<Image>().sprite = untickedBox;
            soundOn = false;
            return;
        }
        // Oyuna ses eklenince seleri buradan açarız
        button_Settings_Sound.gameObject.GetComponent<Image>().sprite = tickedBox;
        soundOn = true;
    }

    public void ButtonEvent_CloseSettingPanel()
    {
        SettingsPanel.SetActive(false);
    }

}
