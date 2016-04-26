Shader "Custom/VertexShader"
{
	Properties
	{
		_MainTex("Base (RGB)", 2D) = "white" {}

	_HeightMin("Height Min", Float) = -1
		_HeightMax("Height Max", Float) = 1

		_ColorOne("Tint Color At Band 1", Color) = (0,0,0,1)
		_ColorTwo("Tint Color At Band 2", Color) = (1,1,1,1)
		_ColorThree("Tint Color At Band 3", Color) = (0,0,0,1)
		_ColorFour("Tint Color At Band 4", Color) = (1,1,1,1)
	}

		SubShader
	{
		Tags{ "RenderType" = "Opaque" }

		CGPROGRAM
#pragma surface surf Lambert

		sampler2D _MainTex;

	fixed4 _ColorOne;
	fixed4 _ColorTwo;
	fixed4 _ColorThree;
	fixed4 _ColorFour;

	float _HeightMin;
	float _HeightMax;




	struct Input
	{
		float2 uv_MainTex;
		float3 worldPos;
	};

	void surf(Input IN, inout SurfaceOutput o)
	{
		half4 c = tex2D(_MainTex, IN.uv_MainTex);

		float height = (_HeightMax - IN.worldPos.y) / (_HeightMax - _HeightMin);

		fixed4 gradientColor = _ColorOne * 0.5 + _ColorTwo * 0.5 + _ColorThree * 0.5 + _ColorFour * 1.0 * height;


		o.Albedo = c.rgb * gradientColor.rgb;

		o.Alpha = c.a * gradientColor.a;
	}
	ENDCG
	}
		Fallback "Diffuse"
}
