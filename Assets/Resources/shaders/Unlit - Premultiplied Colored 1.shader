Shader "Hidden/Unlit/Premultiplied Colored 1" {
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
  GpuProgramID 48490
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
					varying mediump vec4 xlv_COLOR;
					varying highp vec2 xlv_TEXCOORD0;
					varying highp vec2 xlv_TEXCOORD1;
					void main ()
					{
					  gl_Position = (glstate_matrix_mvp * _glesVertex);
					  xlv_COLOR = _glesColor;
					  xlv_TEXCOORD0 = _glesMultiTexCoord0.xy;
					  xlv_TEXCOORD1 = ((_glesVertex.xy * _ClipRange0.zw) + _ClipRange0.xy);
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform sampler2D _MainTex;
					uniform highp vec4 _ClipArgs0;
					varying mediump vec4 xlv_COLOR;
					varying highp vec2 xlv_TEXCOORD0;
					varying highp vec2 xlv_TEXCOORD1;
					void main ()
					{
					  mediump vec4 col_1;
					  highp vec2 tmpvar_2;
					  tmpvar_2 = ((vec2(1.0, 1.0) - abs(xlv_TEXCOORD1)) * _ClipArgs0.xy);
					  lowp vec4 tmpvar_3;
					  tmpvar_3 = texture2D (_MainTex, xlv_TEXCOORD0);
					  mediump vec4 tmpvar_4;
					  tmpvar_4 = (tmpvar_3 * xlv_COLOR);
					  highp float tmpvar_5;
					  tmpvar_5 = clamp (min (tmpvar_2.x, tmpvar_2.y), 0.0, 1.0);
					  col_1.w = (tmpvar_4.w * tmpvar_5);
					  highp vec3 tmpvar_6;
					  tmpvar_6 = (tmpvar_4.xyz * vec3(tmpvar_5));
					  col_1.xyz = tmpvar_6;
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