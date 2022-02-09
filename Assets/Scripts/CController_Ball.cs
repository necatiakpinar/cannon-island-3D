using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class CController_Ball : MonoBehaviour
{
    [Header("Properties")]
    public float playerPowerMultiplier = 5;
    [SerializeField] public SO_Properties_Ball properties;
    public bool isCameraFollow = false;
    private Vector3 direction;

    #region Components
    private Rigidbody rigidBody;
    #endregion

    private void Awake() 
    {
        rigidBody =  GetComponent<Rigidbody>();
        GameObject.Destroy(this.gameObject,5);
    }
    public void SetDirection(Vector3 pDirection) => direction = pDirection;
    public void LaunchToAir()
    {
        rigidBody.mass = properties.BallWeight;

        if(gameObject.layer == LayerMask.NameToLayer("PlayerProjectile"))  rigidBody.AddRelativeForce(direction * properties.LaunchPower, ForceMode.Impulse);
        else rigidBody.AddForce(direction, ForceMode.VelocityChange); 
    }
    private void OnCollisionEnter(Collision pCollision) 
    {
        
        // Layer Indices
        int layerIndex = gameObject.layer;
        int iCollision = pCollision.gameObject.layer;
        int iCastle = LayerMask.NameToLayer("Castle");
        int iPowerUp = LayerMask.NameToLayer("PowerUp");
        int iIsland = LayerMask.NameToLayer("Island");
        int iWater = LayerMask.NameToLayer("Water");
        int iBarrier = LayerMask.NameToLayer("Barrier");
        int iPlayer = LayerMask.NameToLayer("Player");
        int iPlayerProjectile = LayerMask.NameToLayer("PlayerProjectile");
        int iEnemyProjectile = LayerMask.NameToLayer("EnemyProjectile");

        if(layerIndex == iPlayerProjectile)
        {
            // Particles
            if(iCollision == iCastle || iCollision == iPowerUp || iCollision == iIsland)
            {
                GameObject.Instantiate(properties.pfExplosionParticle_00, transform.position, Quaternion.identity);
                GameObject.Instantiate(properties.pfRockParticle, transform.position, Quaternion.Euler(-20,180,104));
            } 
            else if(iCollision == iIsland) GameObject.Instantiate(properties.pfSandParticle, transform.position, Quaternion.identity);
            else if(iCollision == iWater) GameObject.Instantiate(properties.pfWaterParticle, new Vector3(transform.position.x,-0.04f,transform.position.z) , Quaternion.identity);

            // Sounds
            if(iCollision == iCastle || iCollision == iPowerUp || iCollision == iIsland)
            {
                if(iCollision == iCastle) CManager_Sound.Instance.Play("Castle_Hit_00");
                CManager_Sound.Instance.Play("Environment_Explosion_00", 0.3f);
            }
            else if(iCollision == iWater) CManager_Sound.Instance.Play("Environment_WaterSplash");

            // Events
            if(iCollision == iPowerUp)
            {
                CController_PowerUp powerUpObject = pCollision.gameObject.GetComponent<CController_PowerUp>();        
                powerUpObject.Destroy();   
            }

            // Destruction
            if(iCollision == iCastle) GameObject.Destroy(gameObject, 0.5f);
            else GameObject.Destroy(gameObject, 0f);

            gameObject.layer = 0;   // Layer'ı default yaptım ki her girdiği objede particle system'i çağırmasın
        }
        else if(layerIndex == iEnemyProjectile)
        {
            if(iCollision == iPlayer)
            {
                GameObject.Instantiate(properties.pfExplosionParticle_01, transform.position, Quaternion.identity);
                pCollision.gameObject.GetComponentInParent<CController_Player>().TakeDamage(properties.Damage);
            }
            else CManager_Sound.Instance.Play("Environment_WaterSplash", 0.1f);

            GameObject.Destroy(gameObject);
        }
        
        if(iCollision == iBarrier) GameObject.Destroy(gameObject);
        if (isCameraFollow) CManager_Game.Instance.ResetCameraTarget(); //YENİ EKLENDİ
    }
    
    
}
