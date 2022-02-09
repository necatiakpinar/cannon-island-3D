using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CController_AbilityPortal : MonoBehaviour
{
    public GameObject pfParticleSystem;
    public Vector3 leftOffset;
    public Vector3 rightOffset;
    public bool done;
    private void OnTriggerExit(Collider pOther) 
    {
        if(!done && pOther.gameObject.layer == LayerMask.NameToLayer("PlayerProjectile"))
        {
            GameObject prefab = CManager_Game.Instance.managerBall.GetBall(EBallType.DEFAULT);
            if(prefab == null) return;

            GameObject.Instantiate(pfParticleSystem, transform.position, Quaternion.identity);
            
            Rigidbody rb = pOther.GetComponent<Rigidbody>();

            Vector3 origin = pOther.transform.position;
            Vector3 lv = origin + leftOffset; // Left
            Vector3 rv = origin + rightOffset; // Right

            GameObject left = GameObject.Instantiate(prefab, lv, Quaternion.identity, pOther.transform.parent);
            left.GetComponent<Rigidbody>().velocity = rb.velocity;

            GameObject right = GameObject.Instantiate(prefab, rv, Quaternion.identity, pOther.transform.parent);
            right.GetComponent<Rigidbody>().velocity = rb.velocity;


            done = true;
        }

        GameObject.Destroy(this.gameObject);
    }
}
