Shader "Custom/GUI/Circle Ring" {
Properties {
 _borderEdge ("Border Edge", Float) = 0.5
 _borderSize ("Border Size", Float) = 0.1
}
SubShader { 
 LOD 200
 Tags { "QUEUE"="Transparent" "RenderType"="Transparent" }
 Pass {
  Tags { "QUEUE"="Transparent" "RenderType"="Transparent" }
  Blend SrcAlpha OneMinusSrcAlpha
  GpuProgramID 10468
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
					varying highp vec2 xlv_TEXCOORD0;
					void main ()
					{
					  mediump vec4 tmpvar_1;
					  highp vec4 col_2;
					  highp vec2 tmpvar_3;
					  tmpvar_3 = (xlv_TEXCOORD0 - vec2(0.5, 0.5));
					  highp float tmpvar_4;
					  tmpvar_4 = (dot (tmpvar_3, tmpvar_3) * 4.0);
					  col_2.xyz = vec3(1.0, 1.0, 1.0);
					  highp float tmpvar_5;
					  tmpvar_5 = (_borderEdge - _borderSize);
					  highp float edge0_6;
					  edge0_6 = (_borderEdge - 0.01);
					  highp float tmpvar_7;
					  tmpvar_7 = clamp (((tmpvar_4 - edge0_6) / (
					    (_borderEdge + 0.01)
					   - edge0_6)), 0.0, 1.0);
					  highp float edge0_8;
					  edge0_8 = (tmpvar_5 - 0.01);
					  highp float tmpvar_9;
					  tmpvar_9 = clamp (((tmpvar_4 - edge0_8) / (
					    (tmpvar_5 + 0.01)
					   - edge0_8)), 0.0, 1.0);
					  col_2.w = ((tmpvar_9 * (tmpvar_9 * 
					    (3.0 - (2.0 * tmpvar_9))
					  )) * (1.0 - (tmpvar_7 * 
					    (tmpvar_7 * (3.0 - (2.0 * tmpvar_7)))
					  )));
					  tmpvar_1 = col_2;
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