Shader "Custom/City/BurstFX" {
Properties {
 _MainTex ("Base texture", 2D) = "white" { }
}
SubShader { 
 LOD 200
 Tags { "QUEUE"="Transparent" "RenderType"="Transparent" }
 Pass {
  Tags { "QUEUE"="Transparent" "RenderType"="Transparent" }
  Blend SrcAlpha OneMinusSrcAlpha
  GpuProgramID 46120
Program "vp" {
SubProgram "gles " {
"!!GLES
					#version 100
					
					#ifdef VERTEX
					attribute vec4 _glesVertex;
					attribute vec4 _glesColor;
					attribute vec4 _glesMultiTexCoord0;
					uniform highp mat4 glstate_matrix_mvp;
					uniform highp vec4 _MainTex_ST;
					varying highp vec2 xlv_TEXCOORD0;
					varying highp vec4 xlv_COLOR;
					void main ()
					{
					  gl_Position = (glstate_matrix_mvp * _glesVertex);
					  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
					  xlv_COLOR = _glesColor;
					}
					
					
					#endif
					#ifdef FRAGMENT
					varying highp vec2 xlv_TEXCOORD0;
					varying highp vec4 xlv_COLOR;
					void main ()
					{
					  mediump vec4 res_1;
					  mediump float ring_2;
					  mediump vec2 v_3;
					  highp vec2 tmpvar_4;
					  tmpvar_4 = (vec2(0.5, 0.5) - xlv_TEXCOORD0);
					  v_3 = tmpvar_4;
					  mediump float tmpvar_5;
					  tmpvar_5 = (1.0 - dot (v_3, v_3));
					  mediump float tmpvar_6;
					  tmpvar_6 = clamp (((tmpvar_5 - 0.75) / 0.00999999), 0.0, 1.0);
					  ring_2 = ((tmpvar_6 * (tmpvar_6 * 
					    (3.0 - (2.0 * tmpvar_6))
					  )) * (1.0 - tmpvar_5));
					  highp vec4 tmpvar_7;
					  tmpvar_7.xyz = xlv_COLOR.xyz;
					  tmpvar_7.w = (ring_2 * xlv_COLOR.w);
					  res_1 = tmpvar_7;
					  gl_FragData[0] = res_1;
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