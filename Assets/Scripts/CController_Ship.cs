using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;


public class CController_Ship : MonoBehaviour
{
    private MeshRenderer meshRenderer;
    [SerializeField] private CManager_DestroyedShip destroyedShip;
    public float WaitTime_ExplosionPlayer = 1;
    public float WaitTime_ExplosionShip = 0.2f;
    public float WaitTime_Sinking = 0.5f;
    
    public Transform Particle_Player;
    public Transform Parent_Particles;
    public Transform Parent_Crew;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.enabled = true;
        destroyedShip.gameObject.SetActive(false);
    }

    public void StartDestruction()
    {
        StartCoroutine(Coroutine_PlayerExplosionParticle());
    }
    private IEnumerator Coroutine_MeshSwap()
    {
        yield return new WaitForSeconds(WaitTime_Sinking);
        meshRenderer.enabled = false;
        destroyedShip.gameObject.SetActive(true);
    }
    
    private IEnumerator Coroutine_PlayerExplosionParticle()
    {
        yield return new WaitForSeconds(WaitTime_ExplosionPlayer);
        CManager_Sound.Instance.Play("Environment_Explosion_01");
        Particle_Player.gameObject.SetActive(true);
        StartCoroutine(Coroutine_ShipExplosionParticles());       
    }
    private IEnumerator Coroutine_ShipExplosionParticles()
    {
        yield return new WaitForSeconds(WaitTime_ExplosionShip);
        CManager_Sound.Instance.Play("Environment_Explosion_01");
        Parent_Particles.gameObject.SetActive(true);
        StartCoroutine("Coroutine_MeshSwap");       
    }
    public void StartCrewVictoryAnimations()
    {
        foreach(Transform crew in Parent_Crew)
        {
            crew.GetComponent<Animator>().SetTrigger("OnVictory");
        }
    }
    public void StartCrewDefeatAnimations()
    {
        foreach(Transform crew in Parent_Crew)
        {
            crew.GetComponent<Animator>().SetTrigger("OnDefeat");
        }
    }
    
}
