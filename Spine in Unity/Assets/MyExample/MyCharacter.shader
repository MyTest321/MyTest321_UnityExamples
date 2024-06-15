Shader "MyCharacter"
{
	Properties
	{
        [NoScaleOffset] _MainTex 		("Main Texture", 2D) 		= "black" {}
        [NoScaleOffset]	_NormalMap 		("NormalMap", 2D) 			= "black" {}
        [HDR]			_MainColor 		("MainColor", Color) 		= (1, 1, 1, 1)
        [HDR]			_EmissionColor 	("EmissionColor", Color) 	= (1, 0, 0, 1)
						_Power			("Power", Range(-50, 50)) 	= 0.7

		[KeywordEnum(ON, OFF)] _MY_DEBUG("MyDebug", Int) 			= 0 // just for test
	}

	SubShader
	{
		HLSLINCLUDE
			#include "MyCommon.hlsl"
		ENDHLSL

		Tags {
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
			"PreviewType" = "Plane"
			"RenderPipeline" = "UniversalPipeline"
		}

		Cull Off
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
			Name "hello world"
			HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

			#pragma shader_feature_local _MY_DEBUG_ON // just for test

			struct Attributes
			{
				float4 positionOS : POSITION;
				float2 uv : TEXCOORD0;
				float3 normalOS : NORMAL;
			};

			struct Varyings
			{
				float4 positionHCS : SV_POSITION;
				float2 uv : TEXCOORD0;
				//float3 normalWS : NORMAL; // no use now
				//float3 positionWS : TEXCOORD8; // no use now
			};

			MY_TEXTURE2D(_MainTex);
			MY_TEXTURE2D(_NormalMap);

			float4 _MainColor;
			float4 _EmissionColor;
			float  _Power;

			Varyings vert(Attributes i)
			{
				Varyings o;
				o.positionHCS = TransformObjectToHClip(i.positionOS.xyz);
				//o.positionWS = TransformObjectToWorld(i.positionOS.xyz); // no use now
				//o.normalWS = TransformObjectToWorldDir(i.normalOS);	// no use now
				o.uv = i.uv;
				return o;
			}

			float4 frag(Varyings i) : SV_Target
			{
				float4 o = MY_SAMPLE_TEXTURE2D(_MainTex, i.uv);
				float4 mainCol = _MainColor;

				#if _MY_DEBUG_ON
					float4 normalMapCol = MY_SAMPLE_TEXTURE2D(_NormalMap, i.uv);
					float3 lightCol  = _EmissionColor.rgb * normalMapCol.r;
					
					o *= mainCol; // combine main color first
					
					float3 simpleBloomCol = lerp(1, o.rgb, _Power) * (lightCol * o.a);
					o.rgb += simpleBloomCol;

					return float4(max(0, o.rgb), saturate(o.a));
				#else
					return o;
				#endif

			}
			ENDHLSL
		}
	}
}
