Shader "Custom/GUI/Circle Ring Textured X4" {
Properties {
 _MainTex ("Base texture", 2D) = "white" { }
 _borderEdge ("Border Edge", Float) = 0.5
 _borderSize ("Border Size", Float) = 0.1
 _borderAA ("BorderAA", Float) = 0.005
}
SubShader { 
 LOD 200
 Tags { "QUEUE"="Transparent" "RenderType"="Transparent" }
 Pass {
  Tags { "QUEUE"="Transparent" "RenderType"="Transparent" }
  Blend SrcAlpha OneMinusSrcAlpha
  GpuProgramID 61921
Program "vp" {
SubProgram "gles " {
"!!GLES
					#version 100
					
					#ifdef VERTEX
					attribute vec4 _glesVertex;
					attribute vec4 _glesColor;
					attribute vec4 _glesMultiTexCoord0;
					uniform highp mat4 glstate_matrix_mvp;
					varying highp vec2 xlv_TEXCOORD0;
					varying highp vec4 xlv_COLOR;
					void main ()
					{
					  gl_Position = (glstate_matrix_mvp * _glesVertex);
					  xlv_TEXCOORD0 = _glesMultiTexCoord0.xy;
					  xlv_COLOR = _glesColor;
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform highp float _borderEdge;
					uniform highp float _borderSize;
					uniform highp float _borderAA;
					uniform sampler2D _MainTex;
					varying highp vec2 xlv_TEXCOORD0;
					varying highp vec4 xlv_COLOR;
					void main ()
					{
					  mediump vec4 tmpvar_1;
					  mediump vec4 c0_2;
					  highp vec4 col_3;
					  highp vec2 tmpvar_4;
					  tmpvar_4 = (xlv_TEXCOORD0 - vec2(0.5, 0.5));
					  highp float tmpvar_5;
					  tmpvar_5 = (dot (tmpvar_4, tmpvar_4) * 4.0);
					  highp float tmpvar_6;
					  tmpvar_6 = (_borderEdge - _borderSize);
					  highp float edge0_7;
					  edge0_7 = (_borderEdge - _borderAA);
					  highp float tmpvar_8;
					  tmpvar_8 = clamp (((tmpvar_5 - edge0_7) / (
					    (_borderEdge + _borderAA)
					   - edge0_7)), 0.0, 1.0);
					  highp float edge0_9;
					  edge0_9 = (tmpvar_6 - _borderAA);
					  highp float tmpvar_10;
					  tmpvar_10 = clamp (((tmpvar_5 - edge0_9) / (
					    (tmpvar_6 + _borderAA)
					   - edge0_9)), 0.0, 1.0);
					  col_3.w = (((tmpvar_10 * 
					    (tmpvar_10 * (3.0 - (2.0 * tmpvar_10)))
					  ) * (1.0 - 
					    (tmpvar_8 * (tmpvar_8 * (3.0 - (2.0 * tmpvar_8))))
					  )) * xlv_COLOR.w);
					  lowp vec4 tmpvar_11;
					  highp vec2 P_12;
					  P_12 = (xlv_TEXCOORD0 * 4.0);
					  tmpvar_11 = texture2D (_MainTex, P_12);
					  c0_2 = tmpvar_11;
					  col_3.xyz = (c0_2.xyz * xlv_COLOR.xyz);
					  tmpvar_1 = col_3;
					  gl_FragData[0] = tmpvar_1;
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
}