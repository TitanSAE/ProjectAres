 Shader "Custom/AlphaBlendTransition" {
 Properties {
     _Blend ("Blend", Range (0, 1) ) = 0.0
     _MainTex ("Main Texture", 2D) = "" {}
     _OverlayTexture ("Texture 2 with alpha", 2D) = "" {}
     _Color1 ("Main Color", Color) = (1,1,1,1)
	 _Color2 ("Alt Color", Color) = (1,1,1,1)
 }
 SubShader {
 
		Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
		LOD 200
			
		Pass {
             SetTexture[_MainTex]
             SetTexture[_OverlayTexture] {
                 ConstantColor (0,0,0, [_Blend]) 
                 combine texture Lerp(constant) previous
             }
         }

     }
 }