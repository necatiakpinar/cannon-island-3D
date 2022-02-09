using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RDG;

public class CController_Player : MonoBehaviour
{
    [Header("Properties")]
    public float cooldownFire = 2f;
    public int maximumBallCount = 10;
    [SerializeField] private SO_Properties_Player properties;

    [Header("Watch")]
    public bool inCooldown = false;
    public int createdBallCount = 0;
    public int currentHealth;
    public Vector3 aimDirection;
    public bool doubleFireActivated;

    [Header("Strange Shit")]
    public float autoFireRate = 0.1f;
    public bool autoFire = false;
    public bool ignoreBallCount = false;
    public bool ignoreCooldown = false;
    public bool isBallFollow = false;
    
    [Header("Components")]
    public Transform transformCannonBody;
    public Transform transformBallSpawner;
    public Transform transformEnemyTarget;


    #region "Private Components"
    private Camera mainCamera;
    private ParticleSystem muzzleFlash;
    private Animator playerAnimator;
    #endregion

    #region "Managers"
    private CProjectileTrajectory projectileTrajectory;
    private CManager_Ball managerBall;
    #endregion

    public SO_Properties_Player Properties { get { return properties; } private set {} }
    public int remainingBallCount { get { return maximumBallCount - createdBallCount; }}
    
    private EBallType currentBallType;
    private float currentLaunchPower;
    private float cooldownCurrent = 0;

    private void Awake() 
    {
        mainCamera = Camera.main;
        projectileTrajectory = transformBallSpawner.GetComponent<CProjectileTrajectory>();
        muzzleFlash = GetComponentInChildren<ParticleSystem>();
        playerAnimator = GetComponent<Animator>();
        
        this.properties.RotateVector = new Vector2(mainCamera.transform.eulerAngles.y,-mainCamera.transform.eulerAngles.x);
        currentHealth = Properties.Health;
    }
    private void Start() 
    {
        doubleFireActivated = false;
        managerBall = CManager_Game.Instance.managerBall;    
        CManager_Game.Instance.gameIsOn = true;
        currentLaunchPower = CManager_Game.Instance.poolBall.balls[properties.CurrentBallType].GetComponent<CController_Ball>().properties.LaunchPower;
    }

    void Update()
    {
        if(!CManager_Game.Instance.gameIsOn) return;

        CameraMovement();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            isBallFollow = !isBallFollow;
            // CManager_Game.Instance.managerUI.HideGameplayUI();
        } 

        if(inCooldown)
        {
            cooldownCurrent += Time.deltaTime/cooldownFire;
            CManager_Game.Instance.managerUI.SetFireCooldownFill(cooldownCurrent);
        }
    }
    private void CameraMovement()
    {
        #if UNITY_EDITOR
        if (Input.GetMouseButton(0))
        {
            properties.RotateVector = new Vector2(Mathf.Clamp(properties.RotateVector.x,properties.MinRotateX,properties.MaxRotateX),
                                                  Mathf.Clamp(properties.RotateVector.y,properties.MinRotateY,properties.MaxRotateY));
            properties.rotateVector.x += Input.GetAxis("Mouse X") * properties.RotateSpeedModifier;
            properties.rotateVector.y += Input.GetAxis("Mouse Y") * properties.RotateSpeedModifier;

            if ((properties.RotateVector.x >= properties.MinRotateX && properties.RotateVector.x <= properties.MaxRotateX) || (properties.RotateVector.y >= properties.MinRotateY && properties.RotateVector.y <= properties.MaxRotateY))
            {
                transformCannonBody.localRotation = Quaternion.Euler(-properties.RotateVector.y,properties.RotateVector.x,0f);
                aimDirection = (muzzleFlash.transform.position - transformCannonBody.position).normalized;   
            }
        }
        
        #elif UNITY_ANDROID
        if (Input.touchCount > 0)
        {
            Touch firstTouch = Input.GetTouch(0);
            if (firstTouch.phase == TouchPhase.Moved)
            {
                properties.RotateVector = new Vector2(Mathf.Clamp(properties.RotateVector.x,properties.MinRotateX,properties.MaxRotateX),
                                                    Mathf.Clamp(properties.RotateVector.y,properties.MinRotateY,properties.MaxRotateY));
                properties.rotateVector.x += Input.GetAxis("Mouse X") * properties.RotateSpeedModifier;
                properties.rotateVector.y += Input.GetAxis("Mouse Y") * properties.RotateSpeedModifier;

                if ((properties.RotateVector.x >= properties.MinRotateX && properties.RotateVector.x <= properties.MaxRotateX) || (properties.RotateVector.y >= properties.MinRotateY && properties.RotateVector.y <= properties.MaxRotateY))
                {
                    transformCannonBody.localRotation = Quaternion.Euler(-properties.RotateVector.y,properties.RotateVector.x,0f);
                    aimDirection = (muzzleFlash.transform.position - transformCannonBody.position).normalized;   
                }
            }
        }
        #endif

    }
    
    private void OnDrawGizmos() 
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + (aimDirection * 50));
    }
    public void Fire()
    {
        if(createdBallCount < maximumBallCount)
        {
            if(CManager_Game.Instance.managerUI.vibrationOn) Vibration.Vibrate(333, 100);

            if(doubleFireActivated) playerAnimator.SetTrigger("OnDoubleFire");
            else playerAnimator.SetTrigger("OnFire");
            
            if(!ignoreBallCount)
            {
                createdBallCount++;
                CManager_Game.Instance.CheckBallCount();
                CManager_Game.Instance.managerUI.UpdateCannonBallCount();
            }


            if(!ignoreCooldown) StartCoroutine(Coroutine_FireCooldown());
        }
        else CManager_Game.Instance.managerUI.ShowFailPanel();
    }
    private IEnumerator Coroutine_FireCooldown()
    {
        cooldownCurrent = 0;
        inCooldown = true;
        CManager_Game.Instance.managerUI.DisableFireButton();
        yield return new WaitForSeconds(cooldownFire);
        CManager_Game.Instance.managerUI.EnableFireButton();
        inCooldown = false;
    }
    public void TakeDamage(int pDamage)
    {
        if(CManager_Game.Instance.gameIsOn)
        {
            CManager_Sound.Instance.Play("Player_Hit");
            currentHealth -= pDamage;
            CManager_Game.Instance.managerUI.UpdateHealthSlider();
            if(currentHealth <= 0) CManager_Game.Instance.PlayerLost();
        }
    }
    public void AnimEvent_TriggerFire()
    {
        CManager_Game.Instance.managerBall.ThrowBall(aimDirection, properties.CurrentBallType, transformBallSpawner.position, true, isBallFollow);
        CManager_Sound.Instance.Play("Player_Fire");
        muzzleFlash.Play();
    }

    public IEnumerator CO_AutoFire()
    {
        Fire();
        yield return new WaitForSeconds(autoFireRate);
        StartCoroutine(CO_AutoFire());
    }

    public void AutoFire()
    {
        StartCoroutine(CO_AutoFire());
    }
}
