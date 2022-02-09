using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CManager_Sound : MonoBehaviour
{
    [SerializeField] private CSerializedDictionary<string, AudioClip> SFX;
    [SerializeField] private GameObject PF_SoundObject;    
    public static CManager_Sound Instance;
    private void Awake()
    {
        Instance = this;
    }

    public void Play(string P_Name, float P_SoundVolume = 1f, float P_Delay = 0)
    {
        if(!SFX.ContainsKey(P_Name) || !CManager_Game.Instance.managerUI.soundOn) return;
        AudioClip clip = SFX[P_Name];

        CController_Sound aObject = GameObject.Instantiate(PF_SoundObject).GetComponent<CController_Sound>();
        aObject.gameObject.name = $"SoundObject_{P_Name}";
        aObject.Initialize(clip, P_SoundVolume, P_Delay);
    }

}
