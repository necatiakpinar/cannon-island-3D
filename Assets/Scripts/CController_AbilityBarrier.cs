using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CController_AbilityBarrier : MonoBehaviour
{
    public bool done;

    public GameObject prefabBarrier;
    private void OnTriggerExit(Collider pOther) 
    {
        if(!done && pOther.gameObject.layer == LayerMask.NameToLayer("PlayerProjectile"))
        {
            GameObject abilityBarrier = GameObject.Instantiate(prefabBarrier,null);
            GameObject.Destroy(abilityBarrier,7);
            done = true;
        }

        GameObject.Destroy(this.gameObject);
    }
}
