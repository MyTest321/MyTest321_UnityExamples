#ifndef __MY_COMMON_HLSL__
#define __MY_COMMON_HLSL__

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl"

#define MY_TEXTURE2D(T) \
	TEXTURE2D(T); \
	float4 T##_ST; \
	SAMPLER(sampler##T);

#define MY_SAMPLE_TEXTURE2D(T, uv) \
	SAMPLE_TEXTURE2D(T, sampler##T, uv * T##_ST.xy + T##_ST.zw);

#endif // __MY_COMMON_HLSL__