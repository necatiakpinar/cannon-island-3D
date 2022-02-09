using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CDEBUG_ShipExplosion : MonoBehaviour
{
    public GameObject PF_DestroyedShip;
    public MeshRenderer MeshRenderer_ActualShip;
    private GameObject DestroyedShip;

    private Vector3 DefaultGravity;

    private void Awake() 
    {
        DefaultGravity = Physics.gravity;    
    }
    [ContextMenu("Refresh Ship")]
    private void Refresh()
    {
        Transform child = transform.GetChild(0);

        Physics.gravity = DefaultGravity;
        GameObject.Instantiate(PF_DestroyedShip, child.position, child.rotation, transform);
        GameObject.Destroy(child.gameObject);
    }
}
