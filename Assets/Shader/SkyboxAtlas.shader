Shader "Custom/SkyboxAtlas" {
Properties {
 _TintA ("Tint Color A", Color) = (0.5,0.5,0.5,0.5)
 _TintB ("Tint Color B", Color) = (0.5,0.5,0.5,0.5)
 _MainTex ("Texture", 2D) = "white" { }
}
SubShader { 
 Tags { "QUEUE"="Background" "RenderType"="Background" }
 Pass {
  Tags { "QUEUE"="Background" "RenderType"="Background" }
  ZWrite Off
  Cull Off
  GpuProgramID 17192
Program "vp" {
SubProgram "gles " {
"!!GLES
					#version 100
					
					#ifdef VERTEX
					attribute vec4 _glesVertex;
					attribute vec4 _glesMultiTexCoord0;
					uniform highp mat4 glstate_matrix_mvp;
					uniform highp vec4 _MainTex_ST;
					varying highp vec2 xlv_TEXCOORD0;
					void main ()
					{
					  gl_Position = (glstate_matrix_mvp * _glesVertex);
					  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform sampler2D _MainTex;
					uniform lowp vec4 _TintA;
					uniform lowp vec4 _TintB;
					varying highp vec2 xlv_TEXCOORD0;
					void main ()
					{
					  mediump vec4 tmpvar_1;
					  lowp vec4 col_2;
					  lowp vec4 tmpvar_3;
					  tmpvar_3 = texture2D (_MainTex, xlv_TEXCOORD0);
					  col_2.xyz = (tmpvar_3.xyz * mix (_TintA, _TintB, tmpvar_3.wwww).xyz);
					  col_2.w = tmpvar_3.w;
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
Fallback "VertexLit"
}