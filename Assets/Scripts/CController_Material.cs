using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CController_Material : MonoBehaviour
{
    public Texture BaseTexture;
    public Color BaseColor;
    private Renderer Renderer;
    private MaterialPropertyBlock PropertyBlock;

    private void Awake()
    {
        Renderer = GetComponent<Renderer>();
        PropertyBlock = new MaterialPropertyBlock();
    } 
    private void OnValidate()
    {
        if(PropertyBlock == null) PropertyBlock = new MaterialPropertyBlock();
        if(Renderer == null) Renderer = GetComponent<Renderer>();

        PropertyBlock.SetColor("_BaseColor", BaseColor);
        if(BaseTexture != null) PropertyBlock.SetTexture("_BaseMap", BaseTexture);
        Renderer.SetPropertyBlock(PropertyBlock);
    }
    private void Start() 
    {
        if(PropertyBlock == null) PropertyBlock = new MaterialPropertyBlock();
        if(Renderer == null) Renderer = GetComponent<Renderer>();

        PropertyBlock.SetColor("_BaseColor", BaseColor);
        if(BaseTexture != null) PropertyBlock.SetTexture("_BaseMap", BaseTexture);
        Renderer.SetPropertyBlock(PropertyBlock);
    }
}
