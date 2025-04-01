Shader "MyCustomSRP/SRP0101/UnlitOpaque"
{
	Properties
	{
		_MainTex ("_MainTex (RGBA)", 2D) = "white" {}
		[HDR] _Color("Main Color", Color) = (1,1,1,1)
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }

		Pass
		{
			Tags { "LightMode" = "SRP0101_Pass" }

			HLSLPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "../MyCommon/myshader/myshader.hlsl"

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
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = TransformObjectToHClip(v.vertex.xyz);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}
			
			float4 frag (v2f i) : SV_Target
			{
				float4 col = MY_SAMPLE_SAMPLER2D(_MainTex, i.uv) * _Color;
				return col;
			}
			ENDHLSL
		}
	}
}
