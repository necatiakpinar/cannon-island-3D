using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(menuName = "Pool/Ball",order = 2)]
public class SO_Pool_Ball : ScriptableObject
{
    public CSerializedDictionary<EBallType, GameObject> balls;
}
