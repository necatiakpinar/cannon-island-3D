using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class CController_Sound : MonoBehaviour
{
    [SerializeField] private AudioSource Source;
    private void Awake() 
    {
        Source = GetComponent<AudioSource>();
    }
    public void Initialize(AudioClip P_Clip, float P_SoundVolume, float P_Delay)
    {
        if(P_Clip == null) return;
        Source.volume = P_SoundVolume;
        Source.clip = P_Clip;
        Source.PlayDelayed(P_Delay);

        GameObject.Destroy(gameObject, P_Clip.length + P_Delay);
    }
}
