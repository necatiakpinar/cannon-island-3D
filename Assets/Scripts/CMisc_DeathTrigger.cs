using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMisc_DeathTrigger : MonoBehaviour
{

    private void OnTriggerEnter(Collider pCollider) 
    {
        int enemyLayerIndex = LayerMask.NameToLayer("Enemy");
        if(pCollider.gameObject.layer == enemyLayerIndex && !pCollider.GetComponent<CController_Enemy>()) 
        {
            Destroy(pCollider.gameObject);
        }
    }
}
