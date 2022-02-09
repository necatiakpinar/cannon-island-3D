using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CController_Crew : MonoBehaviour
{
    public void AnimEvent_Die()
    {
        GameObject.Destroy(gameObject);
    }
}
