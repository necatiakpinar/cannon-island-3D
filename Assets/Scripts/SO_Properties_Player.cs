using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Properties/Properties_Player",order = 1)]
public class SO_Properties_Player : ScriptableObject
{

    public int Health;

    [Header("Rotatiton Controllers")]
    [SerializeField] private float rotateSpeedModifier;
    public float RotateSpeedModifier{ get {return rotateSpeedModifier;}  private set {} }
    public int MyProperty { get; set; }

    #region "Camera Rotation Variables"
    [SerializeField] private float minRotateX = -90;
    public float MinRotateX { get {return minRotateX;}  private set {} }
    [SerializeField] private float maxRotateX = 90;
    public float MaxRotateX { get {return maxRotateX;}  private set {} }
    [SerializeField] private float minRotateY = -20;
    public float MinRotateY { get {return minRotateY;}  private set {} }
    [SerializeField] private float maxRotateY = 20;
    public float MaxRotateY { get {return maxRotateY;}  private set {} }

    public Vector2 rotateVector;
    public Vector2 RotateVector { get {return rotateVector;} set{ rotateVector = value; } }
    #endregion

    [Header("Ball Variables")]
    [SerializeField] private EBallType currentBallType;
    public EBallType CurrentBallType { get {return currentBallType;} set {currentBallType = value;} }

}
