Shader "Custom/GUI/Circle Ring 1" {
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
  GpuProgramID 42528
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
					varying highp vec4 xlv_COLOR;
					varying highp vec2 xlv_TEXCOORD1;
					void main ()
					{
					  gl_Position = (glstate_matrix_mvp * _glesVertex);
					  xlv_TEXCOORD0 = _glesMultiTexCoord0.xy;
					  xlv_COLOR = _glesColor;
					  xlv_TEXCOORD1 = ((_glesVertex.xy * _ClipRange0.zw) + _ClipRange0.xy);
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform highp vec2 _ClipArgs0;
					uniform highp float _borderEdge;
					uniform highp float _borderSize;
					varying highp vec2 xlv_TEXCOORD0;
					varying highp vec4 xlv_COLOR;
					varying highp vec2 xlv_TEXCOORD1;
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
					  edge0_6 = (_borderEdge - 0.02);
					  highp float tmpvar_7;
					  tmpvar_7 = clamp (((tmpvar_4 - edge0_6) / (
					    (_borderEdge + 0.02)
					   - edge0_6)), 0.0, 1.0);
					  highp float edge0_8;
					  edge0_8 = (tmpvar_5 - 0.02);
					  highp float tmpvar_9;
					  tmpvar_9 = clamp (((tmpvar_4 - edge0_8) / (
					    (tmpvar_5 + 0.02)
					   - edge0_8)), 0.0, 1.0);
					  col_2.w = ((tmpvar_9 * (tmpvar_9 * 
					    (3.0 - (2.0 * tmpvar_9))
					  )) * (1.0 - (tmpvar_7 * 
					    (tmpvar_7 * (3.0 - (2.0 * tmpvar_7)))
					  )));
					  highp vec2 tmpvar_10;
					  tmpvar_10 = ((vec2(1.0, 1.0) - abs(xlv_TEXCOORD1)) * _ClipArgs0);
					  col_2.w = (col_2.w * clamp (min (tmpvar_10.x, tmpvar_10.y), 0.0, 1.0));
					  col_2.xyz = xlv_COLOR.xyz;
					  col_2.w = (col_2.w * xlv_COLOR.w);
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