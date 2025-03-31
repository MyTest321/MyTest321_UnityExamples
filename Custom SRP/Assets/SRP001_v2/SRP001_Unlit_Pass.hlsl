#ifndef __SRP001_UNLIT_PASS_HLSL__
#define __SRP001_UNLIT_PASS_HLSL__

struct appdata
{
    float4 vertex : POSITION;
    float2 uv : TEXCOORD0;
};

struct v2f
{
    float2 uv : TEXCOORD0;
    float4 vertex : SV_POSITION;
};

CBUFFER_START(UnityPerMaterial)
MY_SAMPLER2D(_MainTex)
float4 _Color;
CBUFFER_END

v2f SRP001_Unlit_Pass_vert(appdata v)
{
    v2f o;
    o.vertex = TransformObjectToHClip(v.vertex.xyz);
    o.uv = TRANSFORM_TEX(v.uv, _MainTex);
    return o;
}

float4 SRP001_Unlit_Pass_frag(v2f i) : SV_TARGET
{
    float4 col = MY_SAMPLE_SAMPLER2D(_MainTex, i.uv) * _Color;
    return col;
}

#endif // __SRP001_UNLIT_PASS_HLSL__
