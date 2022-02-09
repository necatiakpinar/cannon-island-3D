using UnityEngine;
[System.Serializable]
public struct SAnimationSound
{
    public string Name;
    public float Volume;
}
public class CAnimator : MonoBehaviour 
{
    public void PlaySound(AnimationEvent P_Event)
    {
        CManager_Sound.Instance.Play(P_Event.stringParameter, P_Event.floatParameter);
    }    
}