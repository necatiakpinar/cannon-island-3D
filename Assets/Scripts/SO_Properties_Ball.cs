using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(menuName = "Properties/Properties_Ball",order = 2)]
public class SO_Properties_Ball : ScriptableObject
{
	
	public int Damage;
    public GameObject pfExplosionParticle_00;
    public GameObject pfExplosionParticle_01;
    public GameObject pfRockParticle;
	public GameObject pfWaterParticle;
	public GameObject pfSandParticle;

    [SerializeField] private float ballWeight = 5;
    public float BallWeight { get {return ballWeight;} private set{} }
    [SerializeField] private float weightLaunchModifier = 25;
    [SerializeField] private float launchPower => ballWeight * weightLaunchModifier; 
    public float LaunchPower { get {return launchPower;} private set{} }

    #if UNITY_EDITOR
	private void OnEnable()
	{
		EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
	}

	private void OnPlayModeStateChanged(PlayModeStateChange state)
	{
		if (state != PlayModeStateChange.ExitingPlayMode) return;
		Resources.UnloadAsset(this);

	}
	#endif
}
