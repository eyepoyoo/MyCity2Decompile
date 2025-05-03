Shader "Custom/City/BlackCircle" {
Properties {
 _MainTex ("Base texture", 2D) = "white" { }
}
SubShader { 
 LOD 200
 Tags { "QUEUE"="Transparent" "RenderType"="Transparent" }
 Pass {
  Tags { "QUEUE"="Transparent" "RenderType"="Transparent" }
  Blend SrcAlpha OneMinusSrcAlpha
  GpuProgramID 16743
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
					void main ()
					{
					  mediump vec2 v_1;
					  highp vec2 tmpvar_2;
					  tmpvar_2 = (vec2(0.5, 0.5) - xlv_TEXCOORD0);
					  v_1 = tmpvar_2;
					  mediump float tmpvar_3;
					  tmpvar_3 = clamp (((
					    (1.0 - (dot (v_1, v_1) * 2.0))
					   - 0.5) / 0.02999997), 0.0, 1.0);
					  mediump vec4 tmpvar_4;
					  tmpvar_4.xyz = vec3(0.0, 0.0, 0.0);
					  tmpvar_4.w = (tmpvar_3 * (tmpvar_3 * (3.0 - 
					    (2.0 * tmpvar_3)
					  )));
					  gl_FragData[0] = tmpvar_4;
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
Fallback "Diffuse"
}