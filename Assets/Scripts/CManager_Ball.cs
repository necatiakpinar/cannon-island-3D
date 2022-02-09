using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CManager_Ball : MonoBehaviour
{
    private SO_Pool_Ball pool;
    private void Start()
    {
        pool = CManager_Game.Instance.poolBall;
    }

    public void ThrowBall(Vector3 pDirection, EBallType pBallType, Vector3 pSpawnLocation, bool P_OwnerIsPlayer = true, bool pFollowBall = false)
    {
        if(!pool.balls.ContainsKey(pBallType)) return;


        GameObject ballObject = GameObject.Instantiate(pool.balls[pBallType], pSpawnLocation, Quaternion.identity);
        
        if(P_OwnerIsPlayer) ballObject.layer = LayerMask.NameToLayer("PlayerProjectile");
        else ballObject.layer = LayerMask.NameToLayer("EnemyProjectile");

        ballObject.transform.parent = CManager_Game.Instance.containerCannonBall;
        CController_Ball ballController = ballObject.GetComponent<CController_Ball>();

        if(pFollowBall)
        {
            CManager_Game.Instance.FollowBall(ballObject.transform);
            ballController.isCameraFollow = true;
        }

        ballController.SetDirection(pDirection);
        ballController.LaunchToAir();

    }
    public GameObject GetBall(EBallType pType) => (pool.balls.ContainsKey(pType)) ? pool.balls[pType] : null;
}
