#ifndef __mybase_HLSL__
#define __mybase_HLSL__

// Unity HLSL
    // Core RP: https://github.com/Unity-Technologies/Graphics/tree/master/Packages/com.unity.render-pipelines.core/ShaderLibrary
    // Universal RP: https://github.com/Unity-Technologies/Graphics/tree/master/Packages/com.unity.render-pipelines.universal/ShaderLibrary
    // High Definition RP: https://github.com/Unity-Technologies/Graphics/tree/master/Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/CommonMaterial.hlsl"

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/UnityInput.hlsl"

#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/SpaceTransforms.hlsl"

#include "mybase_Macro.hlsl"

 float my_invLerp( float from,  float to,  float value) { return (value - from) / (to - from); }
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

float3x3 my_inverse(float3x3 m) {
    return 1.0 / determinant(m) *
        float3x3(
            m._22 * m._33 - m._23 * m._32,       -(m._12 * m._33 - m._13 * m._32),       m._12 * m._23 - m._13 * m._22,
            -(m._21 * m._33 - m._23 * m._31),    m._11 * m._33 - m._13 * m._31,          -(m._11 * m._23 - m._13 * m._21),
            m._21 * m._32 - m._22 * m._31,       -(m._11 * m._32 - m._12 * m._31),       m._11 * m._22 - m._12 * m._21
        );
}

float4x4 my_inverse(float4x4 m)  {
    float n11 = m[0][0], n12 = m[1][0], n13 = m[2][0], n14 = m[3][0];
    float n21 = m[0][1], n22 = m[1][1], n23 = m[2][1], n24 = m[3][1];
    float n31 = m[0][2], n32 = m[1][2], n33 = m[2][2], n34 = m[3][2];
    float n41 = m[0][3], n42 = m[1][3], n43 = m[2][3], n44 = m[3][3];

    float t11 = n23 * n34 * n42 - n24 * n33 * n42 + n24 * n32 * n43 - n22 * n34 * n43 - n23 * n32 * n44 + n22 * n33 * n44;
    float t12 = n14 * n33 * n42 - n13 * n34 * n42 - n14 * n32 * n43 + n12 * n34 * n43 + n13 * n32 * n44 - n12 * n33 * n44;
    float t13 = n13 * n24 * n42 - n14 * n23 * n42 + n14 * n22 * n43 - n12 * n24 * n43 - n13 * n22 * n44 + n12 * n23 * n44;
    float t14 = n14 * n23 * n32 - n13 * n24 * n32 - n14 * n22 * n33 + n12 * n24 * n33 + n13 * n22 * n34 - n12 * n23 * n34;

    float det = n11 * t11 + n21 * t12 + n31 * t13 + n41 * t14;
    float idet = 1.0f / det;

    float4x4 ret;

    ret[0][0] = t11 * idet;
    ret[0][1] = (n24 * n33 * n41 - n23 * n34 * n41 - n24 * n31 * n43 + n21 * n34 * n43 + n23 * n31 * n44 - n21 * n33 * n44) * idet;
    ret[0][2] = (n22 * n34 * n41 - n24 * n32 * n41 + n24 * n31 * n42 - n21 * n34 * n42 - n22 * n31 * n44 + n21 * n32 * n44) * idet;
    ret[0][3] = (n23 * n32 * n41 - n22 * n33 * n41 - n23 * n31 * n42 + n21 * n33 * n42 + n22 * n31 * n43 - n21 * n32 * n43) * idet;

    ret[1][0] = t12 * idet;
    ret[1][1] = (n13 * n34 * n41 - n14 * n33 * n41 + n14 * n31 * n43 - n11 * n34 * n43 - n13 * n31 * n44 + n11 * n33 * n44) * idet;
    ret[1][2] = (n14 * n32 * n41 - n12 * n34 * n41 - n14 * n31 * n42 + n11 * n34 * n42 + n12 * n31 * n44 - n11 * n32 * n44) * idet;
    ret[1][3] = (n12 * n33 * n41 - n13 * n32 * n41 + n13 * n31 * n42 - n11 * n33 * n42 - n12 * n31 * n43 + n11 * n32 * n43) * idet;

    ret[2][0] = t13 * idet;
    ret[2][1] = (n14 * n23 * n41 - n13 * n24 * n41 - n14 * n21 * n43 + n11 * n24 * n43 + n13 * n21 * n44 - n11 * n23 * n44) * idet;
    ret[2][2] = (n12 * n24 * n41 - n14 * n22 * n41 + n14 * n21 * n42 - n11 * n24 * n42 - n12 * n21 * n44 + n11 * n22 * n44) * idet;
    ret[2][3] = (n13 * n22 * n41 - n12 * n23 * n41 - n13 * n21 * n42 + n11 * n23 * n42 + n12 * n21 * n43 - n11 * n22 * n43) * idet;

    ret[3][0] = t14 * idet;
    ret[3][1] = (n13 * n24 * n31 - n14 * n23 * n31 + n14 * n21 * n33 - n11 * n24 * n33 - n13 * n21 * n34 + n11 * n23 * n34) * idet;
    ret[3][2] = (n14 * n22 * n31 - n12 * n24 * n31 - n14 * n21 * n32 + n11 * n24 * n32 + n12 * n21 * n34 - n11 * n22 * n34) * idet;
    ret[3][3] = (n12 * n23 * n31 - n13 * n22 * n31 + n13 * n21 * n32 - n11 * n23 * n32 - n12 * n21 * n33 + n11 * n22 * n33) * idet;

    return ret;
}

#endif // __mybase_HLSL__