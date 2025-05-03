Shader "Hidden/Unlit/Premultiplied Colored (TextureClip)" {
Properties {
 _MainTex ("Base (RGB), Alpha (A)", 2D) = "black" { }
}
SubShader { 
 LOD 200
 Tags { "QUEUE"="Transparent" "IGNOREPROJECTOR"="true" "RenderType"="Transparent" }
 Pass {
  Tags { "QUEUE"="Transparent" "IGNOREPROJECTOR"="true" "RenderType"="Transparent" }
  ZWrite Off
  Cull Off
  Blend One OneMinusSrcAlpha
  ColorMask RGB
  Offset -1, -1
  GpuProgramID 40745
Program "vp" {
SubProgram "gles " {
"!!GLES
					#version 100
					
					#ifdef VERTEX
					attribute vec4 _glesVertex;
					attribute vec4 _glesColor;
					attribute vec4 _glesMultiTexCoord0;
					uniform highp mat4 glstate_matrix_mvp;
					uniform highp vec4 _ClipRange0;
					varying highp vec2 xlv_TEXCOORD0;
					varying highp vec2 xlv_TEXCOORD1;
					varying mediump vec4 xlv_COLOR;
					void main ()
					{
					  gl_Position = (glstate_matrix_mvp * _glesVertex);
					  xlv_TEXCOORD0 = _glesMultiTexCoord0.xy;
					  xlv_TEXCOORD1 = (((
					    (_glesVertex.xy * _ClipRange0.zw)
					   + _ClipRange0.xy) * 0.5) + vec2(0.5, 0.5));
					  xlv_COLOR = _glesColor;
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform sampler2D _MainTex;
					uniform sampler2D _ClipTex;
					varying highp vec2 xlv_TEXCOORD0;
					varying highp vec2 xlv_TEXCOORD1;
					varying mediump vec4 xlv_COLOR;
					void main ()
					{
					  mediump vec4 col_1;
					  mediump float alpha_2;
					  lowp float tmpvar_3;
					  tmpvar_3 = texture2D (_ClipTex, xlv_TEXCOORD1).w;
					  alpha_2 = tmpvar_3;
					  lowp vec4 tmpvar_4;
					  tmpvar_4 = texture2D (_MainTex, xlv_TEXCOORD0);
					  mediump vec4 tmpvar_5;
					  tmpvar_5 = (tmpvar_4 * xlv_COLOR);
					  col_1.w = (tmpvar_5.w * alpha_2);
					  col_1.xyz = (tmpvar_5.xyz * vec3(alpha_2));
					  gl_FragData[0] = col_1;
					}
					
					
					#endif"
}
}
Program "fp" {
SubProgram "gles " {
"!!GLES"
}
}
 }
}
Fallback "Unlit/Premultiplied Colored"
}