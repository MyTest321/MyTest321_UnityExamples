Shader "MyCustomSRP/SRP001/Unlit"
{
	Properties
	{
		[Title(SRP001 Title)]
		[Tooltip(base tex)]
		_MainTex ("_MainTex (RGBA)", 2D) = "white" {}

		[Tooltip(base color)]
		_Color("Main Color", Color) = (1,1,1,1)

		[Tooltip(Z Write)]
		[Tooltip(1 writeable)]
		[Tooltip(0 non_writable)]
		[Enum(Off, 0, On, 1)] _ZWrite ("Z Write", Float) = 1
	}
	SubShader
	{
		Cull 	 Off
		Lighting Off

		Blend SrcAlpha OneMinusSrcAlpha

        HLSLINCLUDE
        #include "../MyCommon/myshader/myshader.hlsl"
        ENDHLSL

		Pass
		{
			ZWrite [_ZWrite]

			Tags { "LightMode" = "SRP001_Pass" }

			HLSLPROGRAM
			#pragma vertex 		SRP001_Unlit_Pass_vert
			#pragma fragment 	SRP001_Unlit_Pass_frag
            #include "SRP001_Unlit_Pass.hlsl"
			ENDHLSL
		}
	}
	CustomEditor "LWGUI.LWGUI"
}
