Shader "Custom/City/FullScreenBlob" {
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
  Blend One One
  Offset -1, -1
  GpuProgramID 38743
Program "vp" {
SubProgram "gles " {
"!!GLES
					#version 100
					
					#ifdef VERTEX
					attribute vec4 _glesVertex;
					attribute vec4 _glesColor;
					attribute vec4 _glesMultiTexCoord0;
					uniform highp mat4 glstate_matrix_mvp;
					mediump vec2 tmpvar_1;
					varying mediump vec2 xlv_TEXCOORD0;
					varying lowp vec4 xlv_COLOR;
					void main ()
					{
					  highp vec2 tmpvar_2;
					  tmpvar_2 = _glesMultiTexCoord0.xy;
					  tmpvar_1 = tmpvar_2;
					  gl_Position = (glstate_matrix_mvp * _glesVertex);
					  xlv_TEXCOORD0 = tmpvar_1;
					  xlv_COLOR = _glesColor;
					}
					
					
					#endif
					#ifdef FRAGMENT
					varying mediump vec2 xlv_TEXCOORD0;
					varying lowp vec4 xlv_COLOR;
					void main ()
					{
					  lowp vec4 res_1;
					  lowp vec2 v_2;
					  mediump vec2 tmpvar_3;
					  tmpvar_3 = (vec2(0.5, 0.5) - xlv_TEXCOORD0);
					  v_2 = tmpvar_3;
					  res_1.w = xlv_COLOR.w;
					  res_1.xyz = (xlv_COLOR.xyz * (1.0 - (
					    dot (v_2, v_2)
					   * 5.0)));
					  res_1.xyz = (res_1.xyz * xlv_COLOR.w);
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