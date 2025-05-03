Shader "Mobile/Unlit - No Depth" {
Properties {
 _MainTex ("Base (RGB)", 2D) = "white" { }
}
SubShader { 
 LOD 100
 Tags { "QUEUE"="Geometry+100" "RenderType"="Opaque" }
 Pass {
  Tags { "LIGHTMODE"="Vertex" "QUEUE"="Geometry+100" "RenderType"="Opaque" }
  ZTest Always
  GpuProgramID 14076
Program "vp" {
SubProgram "gles " {
"!!GLES
					#version 100
					
					#ifdef VERTEX
					attribute vec4 _glesVertex;
					attribute vec4 _glesMultiTexCoord0;
					uniform highp mat4 glstate_matrix_mvp;
					uniform highp vec4 _MainTex_ST;
					varying lowp vec4 xlv_COLOR0;
					varying highp vec2 xlv_TEXCOORD0;
					void main ()
					{
					  lowp vec4 tmpvar_1;
					  mediump vec4 tmpvar_2;
					  tmpvar_2 = clamp (vec4(0.0, 0.0, 0.0, 1.1), 0.0, 1.0);
					  tmpvar_1 = tmpvar_2;
					  highp vec4 tmpvar_3;
					  tmpvar_3.w = 1.0;
					  tmpvar_3.xyz = _glesVertex.xyz;
					  xlv_COLOR0 = tmpvar_1;
					  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
					  gl_Position = (glstate_matrix_mvp * tmpvar_3);
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform sampler2D _MainTex;
					varying highp vec2 xlv_TEXCOORD0;
					void main ()
					{
					  lowp vec4 col_1;
					  col_1.xyz = texture2D (_MainTex, xlv_TEXCOORD0).xyz;
					  col_1.w = 1.0;
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
					uniform highp mat4 glstate_matrix_modelview0;
					uniform highp vec4 unity_FogParams;
					uniform highp vec4 _MainTex_ST;
					varying lowp vec4 xlv_COLOR0;
					varying highp vec2 xlv_TEXCOORD0;
					varying lowp float xlv_TEXCOORD1;
					void main ()
					{
					  highp vec3 tmpvar_1;
					  tmpvar_1 = _glesVertex.xyz;
					  highp vec4 tmpvar_2;
					  tmpvar_2.w = 1.0;
					  tmpvar_2.xyz = tmpvar_1;
					  highp vec3 tmpvar_3;
					  tmpvar_3 = (glstate_matrix_modelview0 * tmpvar_2).xyz;
					  lowp vec4 tmpvar_4;
					  mediump vec4 tmpvar_5;
					  tmpvar_5 = clamp (vec4(0.0, 0.0, 0.0, 1.1), 0.0, 1.0);
					  tmpvar_4 = tmpvar_5;
					  lowp float tmpvar_6;
					  highp float tmpvar_7;
					  tmpvar_7 = clamp (((
					    sqrt(dot (tmpvar_3, tmpvar_3))
					   * unity_FogParams.z) + unity_FogParams.w), 0.0, 1.0);
					  tmpvar_6 = tmpvar_7;
					  highp vec4 tmpvar_8;
					  tmpvar_8.w = 1.0;
					  tmpvar_8.xyz = tmpvar_1;
					  xlv_COLOR0 = tmpvar_4;
					  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
					  xlv_TEXCOORD1 = tmpvar_6;
					  gl_Position = (glstate_matrix_mvp * tmpvar_8);
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform lowp vec4 unity_FogColor;
					uniform sampler2D _MainTex;
					varying highp vec2 xlv_TEXCOORD0;
					varying lowp float xlv_TEXCOORD1;
					void main ()
					{
					  lowp vec4 col_1;
					  col_1.xyz = texture2D (_MainTex, xlv_TEXCOORD0).xyz;
					  col_1.w = 1.0;
					  col_1.xyz = mix (unity_FogColor.xyz, col_1.xyz, vec3(xlv_TEXCOORD1));
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
 Pass {
  Tags { "LIGHTMODE"="Vertex" "QUEUE"="Geometry+100" "RenderType"="Opaque" }
  GpuProgramID 79612
Program "vp" {
SubProgram "gles " {
"!!GLES
					#version 100
					
					#ifdef VERTEX
					attribute vec4 _glesVertex;
					attribute vec4 _glesMultiTexCoord0;
					uniform highp mat4 glstate_matrix_mvp;
					uniform highp vec4 _MainTex_ST;
					varying lowp vec4 xlv_COLOR0;
					varying highp vec2 xlv_TEXCOORD0;
					void main ()
					{
					  lowp vec4 tmpvar_1;
					  mediump vec4 tmpvar_2;
					  tmpvar_2 = clamp (vec4(0.0, 0.0, 0.0, 1.1), 0.0, 1.0);
					  tmpvar_1 = tmpvar_2;
					  highp vec4 tmpvar_3;
					  tmpvar_3.w = 1.0;
					  tmpvar_3.xyz = _glesVertex.xyz;
					  xlv_COLOR0 = tmpvar_1;
					  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
					  gl_Position = (glstate_matrix_mvp * tmpvar_3);
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform sampler2D _MainTex;
					varying highp vec2 xlv_TEXCOORD0;
					void main ()
					{
					  lowp vec4 col_1;
					  col_1.xyz = texture2D (_MainTex, xlv_TEXCOORD0).xyz;
					  col_1.w = 1.0;
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
					uniform highp mat4 glstate_matrix_modelview0;
					uniform highp vec4 unity_FogParams;
					uniform highp vec4 _MainTex_ST;
					varying lowp vec4 xlv_COLOR0;
					varying highp vec2 xlv_TEXCOORD0;
					varying lowp float xlv_TEXCOORD1;
					void main ()
					{
					  highp vec3 tmpvar_1;
					  tmpvar_1 = _glesVertex.xyz;
					  highp vec4 tmpvar_2;
					  tmpvar_2.w = 1.0;
					  tmpvar_2.xyz = tmpvar_1;
					  highp vec3 tmpvar_3;
					  tmpvar_3 = (glstate_matrix_modelview0 * tmpvar_2).xyz;
					  lowp vec4 tmpvar_4;
					  mediump vec4 tmpvar_5;
					  tmpvar_5 = clamp (vec4(0.0, 0.0, 0.0, 1.1), 0.0, 1.0);
					  tmpvar_4 = tmpvar_5;
					  lowp float tmpvar_6;
					  highp float tmpvar_7;
					  tmpvar_7 = clamp (((
					    sqrt(dot (tmpvar_3, tmpvar_3))
					   * unity_FogParams.z) + unity_FogParams.w), 0.0, 1.0);
					  tmpvar_6 = tmpvar_7;
					  highp vec4 tmpvar_8;
					  tmpvar_8.w = 1.0;
					  tmpvar_8.xyz = tmpvar_1;
					  xlv_COLOR0 = tmpvar_4;
					  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
					  xlv_TEXCOORD1 = tmpvar_6;
					  gl_Position = (glstate_matrix_mvp * tmpvar_8);
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform lowp vec4 unity_FogColor;
					uniform sampler2D _MainTex;
					varying highp vec2 xlv_TEXCOORD0;
					varying lowp float xlv_TEXCOORD1;
					void main ()
					{
					  lowp vec4 col_1;
					  col_1.xyz = texture2D (_MainTex, xlv_TEXCOORD0).xyz;
					  col_1.w = 1.0;
					  col_1.xyz = mix (unity_FogColor.xyz, col_1.xyz, vec3(xlv_TEXCOORD1));
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