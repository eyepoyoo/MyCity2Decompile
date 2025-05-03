Shader "Custom/City/CityBackdropMaskRender" {
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
  Blend SrcAlpha OneMinusSrcAlpha
  Offset -1, -1
  GpuProgramID 13885
Program "vp" {
SubProgram "gles " {
"!!GLES
					#version 100
					
					#ifdef VERTEX
					attribute vec4 _glesVertex;
					attribute vec4 _glesMultiTexCoord0;
					uniform highp mat4 glstate_matrix_mvp;
					mediump vec2 tmpvar_1;
					mediump vec2 tmpvar_2;
					varying mediump vec2 xlv_TEXCOORD0;
					varying mediump vec2 xlv_TEXCOORD1;
					void main ()
					{
					  highp vec2 tmpvar_3;
					  tmpvar_3 = _glesMultiTexCoord0.xy;
					  tmpvar_1 = tmpvar_3;
					  tmpvar_1.x = (tmpvar_1.x * 5.0);
					  tmpvar_2 = tmpvar_3;
					  gl_Position = (glstate_matrix_mvp * _glesVertex);
					  xlv_TEXCOORD0 = tmpvar_1;
					  xlv_TEXCOORD1 = tmpvar_2;
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform sampler2D _MainTex;
					varying mediump vec2 xlv_TEXCOORD0;
					varying mediump vec2 xlv_TEXCOORD1;
					void main ()
					{
					  lowp vec2 v_1;
					  lowp vec4 res_2;
					  lowp vec4 tmpvar_3;
					  tmpvar_3 = texture2D (_MainTex, xlv_TEXCOORD0);
					  res_2.xyz = tmpvar_3.xyz;
					  mediump vec2 tmpvar_4;
					  tmpvar_4 = (vec2(0.5, 0.5) - xlv_TEXCOORD1);
					  v_1 = tmpvar_4;
					  res_2.w = (tmpvar_3.w * (1.0 - (
					    dot (v_1, v_1)
					   * 5.0)));
					  gl_FragData[0] = res_2;
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