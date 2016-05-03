Shader "Replacement_Example" {
	SubShader{
		Pass{
		Tags{ "RenderType" = "Opaque" }
		CGPROGRAM
#pragma fragment frag
		half4 frag() : COLOR{ return half4(1,1,0,1); }
		ENDCG
	}
	}
}
