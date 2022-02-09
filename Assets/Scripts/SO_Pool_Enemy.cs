using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(menuName = "Pool/Enemy",order = 2)]
public class SO_Pool_Enemy : ScriptableObject
{
    public List<GameObject> listEnemy;
}
