using System.Collections;
using UnityEngine;
using UnityEngine.Playables;


public class CManager_DestroyedShip : MonoBehaviour {
    public float WaitTimeForDestruction;
    public PlayableDirector Director;
    public Vector3 Gravity_Sinking;
    public float ForceMultiplier_Back;
    public float ForceMultiplier_Front;
    public Transform ExplosionOrigin_Back;
    public Transform ExplosionOrigin_Front;
    public Transform Parent_Back;
    public Transform Parent_Front;
 
    private void OnEnable() 
    {
        StartCoroutine(Coroutine_ExplodeTwoSides());
    }
    private void Explode()
    {
        Physics.gravity = Gravity_Sinking;
        Director.Play(Director.playableAsset, DirectorWrapMode.Hold);
    }
    private IEnumerator Coroutine_ExplodeTwoSides()
    {
        yield return new WaitForSeconds(WaitTimeForDestruction);
        Explode();
    }
}