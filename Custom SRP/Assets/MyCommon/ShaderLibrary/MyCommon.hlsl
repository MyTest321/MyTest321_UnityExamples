#ifndef _MY_COMMON_HLSL_
#define _MY_COMMON_HLSL_

#include "InputMacro.hlsl"
#include "UnityBuiltIn.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/SpaceTransforms.hlsl"

// Below functions are missing in core RP...
float3 TransformObjectToViewPos(float3 positionOS)
{
    return mul(GetWorldToViewMatrix(), mul(GetObjectToWorldMatrix(), float4(positionOS, 1.0))).xyz;
}

float4 ComputeScreenPos(float4 positionCS)
{
    float4 o = positionCS * 0.5f;
    o.xy = float2(o.x, o.y * _ProjectionParams.x) + o.w;
    o.zw = positionCS.zw;
    return o;
}

float4 ComputeGrabScreenPos (float4 pos) 
{
    #if UNITY_UV_STARTS_AT_TOP
        float scale = -1.0;
    #else
        float scale = 1.0;
    #endif

    float4 o = pos * 0.5f;
    o.xy = float2(o.x, o.y*scale) + o.w;

    #ifdef UNITY_SINGLE_PASS_STEREO
        o.xy = TransformStereoScreenSpaceTex(o.xy, pos.w);
    #endif
    
    o.zw = pos.zw;
    return o;
}

#define MY_TEXTURE2D(T) \
	TEXTURE2D(T); \
	float4 T##_ST; \
	SAMPLER(sampler_##T); \
//-----

#define MY_SAMPLE_TEXTURE2D(T, uv) \
	SAMPLE_TEXTURE2D(T, sampler_##T, uv * T##_ST.xy + T##_ST.zw) \
//-----

float  my_invLerp(float  from, float  to, float  value) { return (value - from) / (to - from); }
float2 my_invLerp(float2 from, float2 to, float2 value) { return (value - from) / (to - from); }
float3 my_invLerp(float3 from, float3 to, float3 value) { return (value - from) / (to - from); }
float4 my_invLerp(float4 from, float4 to, float4 value) { return (value - from) / (to - from); }

float my_remap(float origFrom, float origTo, float targetFrom, float targetTo, float value){
  float rel = my_invLerp(origFrom, origTo, value);
  return lerp(targetFrom, targetTo, rel);
}

float2 my_remap(float2 origFrom, float2 origTo, float2 targetFrom, float2 targetTo, float2 value){
  float2 rel = my_invLerp(origFrom, origTo, value);
  return lerp(targetFrom, targetTo, rel);
}

float3 my_remap(float3 origFrom, float3 origTo, float3 targetFrom, float3 targetTo, float3 value){
  float3 rel = my_invLerp(origFrom, origTo, value);
  return lerp(targetFrom, targetTo, rel);
}

float4 my_remap(float4 origFrom, float4 origTo, float4 targetFrom, float4 targetTo, float4 value){
  float4 rel = my_invLerp(origFrom, origTo, value);
  return lerp(targetFrom, targetTo, rel);
}

#endif // _MY_COMMON_HLSL_