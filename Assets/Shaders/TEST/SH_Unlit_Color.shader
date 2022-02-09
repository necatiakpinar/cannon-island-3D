Shader "Unlit/SH_Unlit_Color"
{
    Properties
    {
        BaseColor ("Base Color", Color) = (1, 1, 1, 1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        Pass
        {
            CGPROGRAM
            #pragma vertex Vertex
            #pragma fragment Fragment
            
            #include "UnityCG.cginc"

            float4 BaseColor;

            struct MeshData  // pper-vertex mesh data
            {
                float4 vertex : POSITION; //local space vertex direction
                float3 normals : NORMAL; // local space normal direction
                // float4 tangent : TANGENT; tangent direction (xyz) tangent sign (w)
                // float4 color : COLOR; //vertex colors
                float4 uv0 : TEXCOORD0; // uv coordinates diffuse/normal map textures
                //float4 uv1 : TEXCOORD1; // uv coordinates lightmap textures
                //float4 uv2 : TEXCOORD2; // uv coordinates lightmap textures
            };


            // data passed from the vertex shader to the fragment shader
            // this will interpolate/blend across the triangle!
            struct Interpolators
            {
                float4 vertex : SV_POSITION; //clip space position
                float3 normal : TEXCOORD0; 
                float2 uv : TEXCOORD1;
            };

            Interpolators Vertex (MeshData v)
            {
                Interpolators o;
                o.vertex = UnityObjectToClipPos(v.vertex); // local space to clip space
                o.normal = UnityObjectToWorldNormal(v.normals); // just pass through
                o.uv = v.uv0; 
                return o;
            }

            float4 Fragment (Interpolators i) : SV_Target
            {
                return BaseColor;
            }

            ENDCG
        }
    }
}



