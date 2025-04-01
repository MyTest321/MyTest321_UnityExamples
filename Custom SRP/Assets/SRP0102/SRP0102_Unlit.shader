Shader "MyCustomSRP/SRP0102/Unlit"
{
	Properties
	{
		[Title(SRP0102 Title)]
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
			Tags { "LightMode" = "SRP0102_Pass" }
			ZWrite [_ZWrite]

			HLSLPROGRAM
			#pragma vertex 		SRP0102_Unlit_Pass_vert
			#pragma fragment 	SRP0102_Unlit_Pass_frag
            #include "SRP0102_Unlit_Pass.hlsl"
			ENDHLSL
		}
	}
	CustomEditor "LWGUI.LWGUI"
}
