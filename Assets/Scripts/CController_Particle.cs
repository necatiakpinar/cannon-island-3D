using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CController_Particle : MonoBehaviour
{
    private ParticleSystem ps;
    private void Awake()
    {
        ps = GetComponent<ParticleSystem>();
    }
    public void OnParticleSystemStopped()
    {
        Destroy(gameObject);
    }
}
