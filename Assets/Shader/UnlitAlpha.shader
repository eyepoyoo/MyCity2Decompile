Shader "Unlit/Transparent Alpha" {
Properties {
 _MainTex ("Base (RGB) Trans (A)", 2D) = "white" { }
 _Alpha ("Alpha", Float) = 1
}
SubShader { 
 LOD 100
 Tags { "QUEUE"="Transparent" "IGNOREPROJECTOR"="true" "RenderType"="Transparent" }
 Pass {
  Tags { "QUEUE"="Transparent" "IGNOREPROJECTOR"="true" "RenderType"="Transparent" }
  ZWrite Off
  Blend SrcAlpha OneMinusSrcAlpha
  GpuProgramID 63051
Program "vp" {
SubProgram "gles " {
"!!GLES
					#version 100
					
					#ifdef VERTEX
					attribute vec4 _glesVertex;
					attribute vec4 _glesMultiTexCoord0;
					uniform highp mat4 glstate_matrix_mvp;
					uniform highp vec4 _MainTex_ST;
					varying mediump vec2 xlv_TEXCOORD0;
					void main ()
					{
					  mediump vec2 tmpvar_1;
					  tmpvar_1 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
					  gl_Position = (glstate_matrix_mvp * _glesVertex);
					  xlv_TEXCOORD0 = tmpvar_1;
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform sampler2D _MainTex;
					uniform highp float _Alpha;
					varying mediump vec2 xlv_TEXCOORD0;
					void main ()
					{
					  lowp vec4 col_1;
					  lowp vec4 tmpvar_2;
					  tmpvar_2 = texture2D (_MainTex, xlv_TEXCOORD0);
					  col_1.xyz = tmpvar_2.xyz;
					  col_1.w = (tmpvar_2.w * _Alpha);
					  gl_FragData[0] = col_1;
					}
					
					
					#endif"
}
SubProgram "gles " {
Keywords { "FOG_LINEAR" }
					"!!GLES
					#version 100
					
					#ifdef VERTEX
					attribute vec4 _glesVertex;
					attribute vec4 _glesMultiTexCoord0;
					uniform highp mat4 glstate_matrix_mvp;
					uniform highp vec4 unity_FogParams;
					uniform highp vec4 _MainTex_ST;
					varying mediump vec2 xlv_TEXCOORD0;
					varying highp float xlv_TEXCOORD1;
					void main ()
					{
					  highp vec4 tmpvar_1;
					  mediump vec2 tmpvar_2;
					  tmpvar_1 = (glstate_matrix_mvp * _glesVertex);
					  tmpvar_2 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
					  gl_Position = tmpvar_1;
					  xlv_TEXCOORD0 = tmpvar_2;
					  xlv_TEXCOORD1 = ((tmpvar_1.z * unity_FogParams.z) + unity_FogParams.w);
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform lowp vec4 unity_FogColor;
					uniform sampler2D _MainTex;
					uniform highp float _Alpha;
					varying mediump vec2 xlv_TEXCOORD0;
					varying highp float xlv_TEXCOORD1;
					void main ()
					{
					  lowp vec4 col_1;
					  lowp vec4 tmpvar_2;
					  tmpvar_2 = texture2D (_MainTex, xlv_TEXCOORD0);
					  col_1.w = (tmpvar_2.w * _Alpha);
					  highp float tmpvar_3;
					  tmpvar_3 = clamp (xlv_TEXCOORD1, 0.0, 1.0);
					  col_1.xyz = mix (unity_FogColor.xyz, tmpvar_2.xyz, vec3(tmpvar_3));
					  gl_FragData[0] = col_1;
					}
					
					
					#endif"
}
}
Program "fp" {
SubProgram "gles " {
"!!GLES"
}
SubProgram "gles " {
Keywords { "FOG_LINEAR" }
					"!!GLES"
}
}
 }
}
}