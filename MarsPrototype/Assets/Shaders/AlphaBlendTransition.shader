 Shader "Custom/AlphaBlendTransition" {
 Properties {
     _Blend ("Blend", Range (0, 1) ) = 0.0
     _MainTex ("Main Texture", 2D) = "" {}
     _OverlayTexture ("Texture 2 with alpha", 2D) = "" {}
 
 }
 SubShader {
         Pass {
             SetTexture[_MainTex]
             SetTexture[_OverlayTexture] {
                 ConstantColor (0,0,0, [_Blend]) 
                 combine texture Lerp(constant) previous
             }
         }
     }
 }