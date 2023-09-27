Shader "Mask/DarkMask" {

	SubShader{

		Tags {"Queue" = "AlphaTest+100" }


		ColorMask 0
		ZWrite On
		Cull Off

		Pass
		{

		}
	}
}