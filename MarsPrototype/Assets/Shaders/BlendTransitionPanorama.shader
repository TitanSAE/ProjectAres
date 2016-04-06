Shader "Custom/BlendTransitionPanorama" {
	Properties {
		 _Blend ("Blend", Range (0, 1) ) = 0.0
		 _MainTex ("Main Texture", 2D) = "" {}
		 _OverlayTexture ("Texture 2 with alpha", 2D) = "" {}
	 
	}
	
	SubShader {
		Pass {
			Cull Front

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata {
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f {
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			
			v2f vert (appdata v) {
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target {
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);
				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
			ENDCG
		}

		Pass {
			Cull Front 
			 SetTexture[_MainTex]
			 SetTexture[_OverlayTexture] {
				 ConstantColor (0,0,0, [_Blend]) 
				 combine texture Lerp(constant) previous
			 }
		}
	}


//	SubShader {
//		Pass {
//		Cull Front 
//			 SetTexture[_MainTex]
//			 SetTexture[_OverlayTexture] {
//				 ConstantColor (0,0,0, [_Blend]) 
//				 combine texture Lerp(constant) previous
//			 }
//		}
//	}
 }