#ifndef __mybase_Macro_HLSL__
#define __mybase_Macro_HLSL__

#define MY_TEXTURE2D(T) \
	TEXTURE2D(T); \
	float4 T##_ST; \
	SAMPLER(sampler_##T); \
//-----

#define MY_SAMPLE_TEXTURE2D(T, uv) \
	SAMPLE_TEXTURE2D(T, sampler_##T, uv * T##_ST.xy + T##_ST.zw) \
//-----

#define MY_SAMPLER2D(T) \
  sampler2D T; \
  float4 T##_ST; \
//-----

#define MY_SAMPLE_SAMPLER2D(T, uv) tex2D(T, uv)

#define MY_INPUT_PROP(name) UNITY_ACCESS_INSTANCED_PROP(UnityPerMaterial, name)

#endif // __mybase_Macro_HLSL__
