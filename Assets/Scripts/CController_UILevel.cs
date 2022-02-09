using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CController_UILevel : MonoBehaviour
{
    [Header("Texts")]
    public TMP_Text textLevelId;
    [Header("Images")]
    public Image imageLevel;
    public List<Image> listStar;
    
    [Header("Buttons")]
    public Button buttonLevel;


    [Header("Star Sprites")]
    public Sprite spriteStarEmpty;
    public Sprite spriteStarFull;
    public Sprite spriteActiveLevel;
    public Sprite spriteDeactiveLevel;
    public GameObject Stars;

    public void SetLevelId(int pLevelId)
    {
        textLevelId.text = (pLevelId + 1).ToString();
    }

    public void SetLevelButton(int pLevelId)
    {
        buttonLevel.onClick.AddListener( () => {
            CManager_Game.Instance.LoadLevel(pLevelId);
        });
        
    }
    public void SetStars(int pStarCount)
    {
        for (int i=0;i<listStar.Count;i++)
        {
            if (i <= pStarCount-1)
            {
                listStar[i].sprite = spriteStarFull;
            }
        }
    }

    public void ActivateLevel()
    {
        imageLevel.sprite = spriteActiveLevel;
        buttonLevel.interactable = true;
    }
    public void DeactivateLevel()
    {
        imageLevel.sprite = spriteDeactiveLevel;
        buttonLevel.interactable = false;
        Stars.SetActive(false);
    }
    public void NextLevel()
    {
        imageLevel.sprite = spriteDeactiveLevel;
        buttonLevel.interactable = true;
    }


}

