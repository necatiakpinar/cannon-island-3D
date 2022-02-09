using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CController_Enemy : MonoBehaviour
{
    [Header("Watch")]
    [SerializeField] private bool isAlive;
    [SerializeField] private bool isDisabled;
    [SerializeField] private bool firstAttack;

    [SerializeField] private float projectileSpeed;

    [Header("Attributes")]
    [SerializeField] private float cooldownMinimum;
    [SerializeField] private float cooldownMaximum;
    [SerializeField] private Vector3 shotAccuracyMinimum = new Vector3(-1, -1, -1);
    [SerializeField] private Vector3 shotAccuracyMaximum = new Vector3(1, 1, 1);

    [Header("Objects")]
    [SerializeField] private Transform gunBase; 
    [SerializeField] private Transform gunMuzzle; 
    [SerializeField] private Transform icon;

    [Header("Enemy Humanoid")]
    [SerializeField] private SO_Pool_Enemy poolEnemy;
    [SerializeField] private Transform parentEnemyHumanoid;
    private GameObject enemyHumanoid;
    private Animator animatorEnemyHumanoid;


    private CManager_Enemy managerEnemy;
    private CManager_Ball managerBall;
    [SerializeField] private EBallType ballType;
    private Transform player;
    private Vector3 targetDirection;
    private Vector3 aimDirection;

    public Vector3 startingLocation;
    public float displacementThreshold = 0.5f;
    
    // public Vector3 currentLocation;

    private void CreateEnemyHumanoid()
    {
        int randomNumber = Random.Range(0,poolEnemy.listEnemy.Count);

        enemyHumanoid = GameObject.Instantiate(poolEnemy.listEnemy[randomNumber],parentEnemyHumanoid);
        animatorEnemyHumanoid = enemyHumanoid.GetComponentInChildren<Animator>();
    }
    
    private void Awake()
    {
        managerEnemy = GetComponentInParent<CManager_Enemy>();
        CreateEnemyHumanoid();
        startingLocation = transform.position;
    }
    private void Start()
    {
        managerBall = CManager_Game.Instance.managerBall;
        player = CManager_Game.Instance.controllerPlayer.transform;

        isAlive = true;
        isDisabled = false;
        targetDirection = (player.position - transform.position);
        projectileSpeed = CManager_Game.Instance.poolBall.balls[ballType].GetComponent<CController_Ball>().properties.LaunchPower;


        StartCoroutine(Coroutine_Attack());
    }
    private void Update() // TODO(dukrath): Find a better solution
    {
        if(CheckDisplacement()) DisableEvents();
        // if(icon == null) return;
        // Vector3 v = player.position - transform.position;
        // v.x = v.z = 0.0f;
        // icon.LookAt(player.transform.position - v ); 
        // icon.Rotate(0,180,0);
    }

    private void OnTriggerEnter(Collider P_Collider)
    {
        int triggerLayerIndex = LayerMask.NameToLayer("DeathTrigger");
        if(P_Collider.gameObject.layer == triggerLayerIndex) Die();
    }
    private void Die()
    {
        managerEnemy.Count--;
        isAlive = false;
        Destroy(gameObject);

        if(managerEnemy.Count == 0)
        {
            CManager_Game.Instance.winCondition_Enemy = true;
            if(CManager_Game.Instance.winCondition_Destruction) CManager_Game.Instance.StartWinSequence();
        } 
    }
    private IEnumerator Coroutine_Attack()
    {
        while(true)
        {   
            float cooldown = (firstAttack) ? UnityEngine.Random.Range(cooldownMinimum, cooldownMaximum) : UnityEngine.Random.Range(cooldownMinimum / 2, cooldownMaximum / 2);
            firstAttack = true;

            yield return new WaitForSeconds(cooldown);        
            
            if(isAlive && !isDisabled && CManager_Game.Instance.gameIsOn && enemyHumanoid != null)
            {
                animatorEnemyHumanoid.SetTrigger("Fire");
                yield return new WaitForSeconds(1);        
                CManager_Sound.Instance.Play("Enemy_Fire", 0.1f);
                aimDirection = GetLaunchDirection();
                Quaternion rotation = Quaternion.LookRotation(aimDirection, Vector3.up);
                gunBase.rotation = rotation;
                managerBall.ThrowBall(aimDirection, ballType, gunMuzzle.position, false);
            }
            else
            {
                Die();
            }
        }
    }

    private Vector3 GetLaunchDirection()
    {
        Vector3 targetLocation = player.GetComponent<CController_Player>().transformEnemyTarget.position;
        float speed = projectileSpeed;
        
        Vector3 randomizedOffset = this.RandomVector3(shotAccuracyMinimum, shotAccuracyMaximum);
        Vector3 toTarget = (targetLocation + randomizedOffset) - gunMuzzle.position;

        float gSquared = Physics.gravity.sqrMagnitude;
        float b = speed * speed + Vector3.Dot(toTarget, Physics.gravity);    
        float discriminant = b * b - gSquared * toTarget.sqrMagnitude;

        float discRoot = Mathf.Sqrt(discriminant);


        // Highest shot with the given max speed:
        float T_max = Mathf.Sqrt((b + discRoot) * 2f / gSquared);
        // Most direct shot with the given max speed:
        float T_min = Mathf.Sqrt((b - discRoot) * 2f / gSquared);
        // Lowest-speed arc available:
        float T_lowEnergy = Mathf.Sqrt(Mathf.Sqrt(toTarget.sqrMagnitude * 4f/gSquared));

        float T = T_lowEnergy; // Test T_max, T_min, or some T in-between like T_lowEnergy

        // Convert from time-to-hit to a launch velocity:
        return toTarget / T - Physics.gravity * T / 2f;

    }

    //Animator
    public void PlayVictoryAnimation()
    {
        animatorEnemyHumanoid.SetTrigger("Victory");
    }
    public bool CheckDisplacement()
    {
        if(displacementThreshold < Vector3.Distance(startingLocation, transform.position)) return true;
        return false;
    }
    public void DisableEvents()
    {
        isDisabled = true;
        icon.gameObject.SetActive(false);
    }
}
