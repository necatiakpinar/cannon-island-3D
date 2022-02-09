using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class CController_PowerUp : MonoBehaviour
{
    public EPowerUpType type;
    public int PowerUpAmount;

    public float waitTime_Destruction = 2;
    public GameObject PF_Default;
    public GameObject PF_Destroyed;

    public ParticleSystem PS_Explosion;

    private Transform meshHolder;
    private GameObject spawnedObject;

    private void Awake()
    {
        meshHolder = transform.GetChild(0);
        spawnedObject = meshHolder.GetChild(0).gameObject;
        PS_Explosion = transform.GetChild(1).GetComponent<ParticleSystem>();
    }
    public void Destroy()
    {
        GameObject.Destroy(spawnedObject);
        spawnedObject = null;
        PS_Explosion.Play();
        SpawnFracturedMeshes();
    }
    private async void SpawnFracturedMeshes()
    {
        await Task.Delay((int)(1000 * waitTime_Destruction));
        spawnedObject = GameObject.Instantiate(PF_Destroyed, transform.position, Quaternion.identity, meshHolder);
        ExecutePowerUpEffect();
    }
    private void ExecutePowerUpEffect()
    {
        if(type == EPowerUpType.HEALTH) CManager_Game.Instance.controllerPlayer.currentHealth += PowerUpAmount;
        else if(type == EPowerUpType.AMMO) CManager_Game.Instance.controllerPlayer.maximumBallCount += PowerUpAmount;
        else if(type == EPowerUpType.DOUBLE_SHOT) CManager_Game.Instance.controllerPlayer.doubleFireActivated = true;
    }
}