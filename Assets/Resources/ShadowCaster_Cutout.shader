Shader "Hidden/VacuumShaders/Curved World/ShadowCaster Cutout" {
Properties {
[CurvedWorldLargeLabel]  V_CW_Label_Albedo ("Albedo", Float) = 0
 _Color ("  Color", Color) = (1,1,1,1)
 _MainTex ("  Map (RGB) Trans (A)", 2D) = "white" { }
[CurvedWorldUVScroll]  _V_CW_MainTex_Scroll ("    ", Vector) = (0,0,0,0)
[CurvedWorldLargeLabel]  V_CW_Label_Cutoff ("Cutout", Float) = 0
 _Cutoff ("  Alpha cutoff", Range(0,1)) = 0.5
}
SubShader { 
 Pass {
  Name "SHADOWCASTER"
  Tags { "LIGHTMODE"="SHADOWCASTER" "SHADOWSUPPORT"="true" }
  GpuProgramID 22928
Program "vp" {
SubProgram "gles " {
Keywords { "SHADOWS_DEPTH" }
					"!!GLES
					#version 100
					
					#ifdef VERTEX
					attribute vec4 _glesVertex;
					attribute vec3 _glesNormal;
					attribute vec4 _glesMultiTexCoord0;
					uniform highp vec4 _WorldSpaceLightPos0;
					uniform highp vec4 unity_LightShadowBias;
					uniform highp mat4 glstate_matrix_mvp;
					uniform highp mat4 _Object2World;
					uniform highp mat4 _World2Object;
					uniform highp mat4 unity_MatrixVP;
					uniform highp vec3 _V_CW_Bend;
					uniform highp vec3 _V_CW_Bias;
					uniform highp vec4 _V_CW_PivotPoint_Position;
					uniform highp vec4 _MainTex_ST;
					varying highp vec2 xlv_TEXCOORD1;
					void main ()
					{
					  highp vec2 xzOff_1;
					  highp vec4 worldPos_2;
					  highp vec4 tmpvar_3;
					  tmpvar_3 = (_Object2World * _glesVertex);
					  worldPos_2.w = tmpvar_3.w;
					  worldPos_2.xyz = (tmpvar_3.xyz - _V_CW_PivotPoint_Position.xyz);
					  highp vec2 tmpvar_4;
					  tmpvar_4.x = float((worldPos_2.z >= 0.0));
					  tmpvar_4.y = float((worldPos_2.x >= 0.0));
					  xzOff_1 = (max (vec2(0.0, 0.0), (
					    abs(worldPos_2.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_4 * 2.0) - 1.0));
					  xzOff_1 = (xzOff_1 * xzOff_1);
					  highp vec4 tmpvar_5;
					  tmpvar_5.xzw = vec3(0.0, 0.0, 0.0);
					  tmpvar_5.y = (((_V_CW_Bend.x * xzOff_1.x) + (_V_CW_Bend.z * xzOff_1.y)) * 0.001);
					  worldPos_2 = tmpvar_5;
					  highp vec3 vertex_6;
					  vertex_6 = (_glesVertex + (_World2Object * tmpvar_5)).xyz;
					  highp vec4 clipPos_7;
					  if ((unity_LightShadowBias.z != 0.0)) {
					    highp vec4 tmpvar_8;
					    tmpvar_8.w = 1.0;
					    tmpvar_8.xyz = vertex_6;
					    highp vec3 tmpvar_9;
					    tmpvar_9 = (_Object2World * tmpvar_8).xyz;
					    highp vec4 v_10;
					    v_10.x = _World2Object[0].x;
					    v_10.y = _World2Object[1].x;
					    v_10.z = _World2Object[2].x;
					    v_10.w = _World2Object[3].x;
					    highp vec4 v_11;
					    v_11.x = _World2Object[0].y;
					    v_11.y = _World2Object[1].y;
					    v_11.z = _World2Object[2].y;
					    v_11.w = _World2Object[3].y;
					    highp vec4 v_12;
					    v_12.x = _World2Object[0].z;
					    v_12.y = _World2Object[1].z;
					    v_12.z = _World2Object[2].z;
					    v_12.w = _World2Object[3].z;
					    highp vec3 tmpvar_13;
					    tmpvar_13 = normalize(((
					      (v_10.xyz * _glesNormal.x)
					     + 
					      (v_11.xyz * _glesNormal.y)
					    ) + (v_12.xyz * _glesNormal.z)));
					    highp float tmpvar_14;
					    tmpvar_14 = dot (tmpvar_13, normalize((_WorldSpaceLightPos0.xyz - 
					      (tmpvar_9 * _WorldSpaceLightPos0.w)
					    )));
					    highp vec4 tmpvar_15;
					    tmpvar_15.w = 1.0;
					    tmpvar_15.xyz = (tmpvar_9 - (tmpvar_13 * (unity_LightShadowBias.z * 
					      sqrt((1.0 - (tmpvar_14 * tmpvar_14)))
					    )));
					    clipPos_7 = (unity_MatrixVP * tmpvar_15);
					  } else {
					    highp vec4 tmpvar_16;
					    tmpvar_16.w = 1.0;
					    tmpvar_16.xyz = vertex_6;
					    clipPos_7 = (glstate_matrix_mvp * tmpvar_16);
					  };
					  highp vec4 clipPos_17;
					  clipPos_17.xyw = clipPos_7.xyw;
					  clipPos_17.z = (clipPos_7.z + clamp ((unity_LightShadowBias.x / clipPos_7.w), 0.0, 1.0));
					  clipPos_17.z = mix (clipPos_17.z, max (clipPos_17.z, -(clipPos_7.w)), unity_LightShadowBias.y);
					  gl_Position = clipPos_17;
					  xlv_TEXCOORD1 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform sampler2D _MainTex;
					uniform lowp float _Cutoff;
					uniform lowp vec4 _Color;
					varying highp vec2 xlv_TEXCOORD1;
					void main ()
					{
					  lowp float x_1;
					  x_1 = ((texture2D (_MainTex, xlv_TEXCOORD1).w * _Color.w) - _Cutoff);
					  if ((x_1 < 0.0)) {
					    discard;
					  };
					  gl_FragData[0] = vec4(0.0, 0.0, 0.0, 0.0);
					}
					
					
					#endif"
}
SubProgram "gles " {
Keywords { "SHADOWS_CUBE" }
					"!!GLES
					#version 100
					
					#ifdef VERTEX
					attribute vec4 _glesVertex;
					attribute vec4 _glesMultiTexCoord0;
					uniform highp vec4 _LightPositionRange;
					uniform highp mat4 glstate_matrix_mvp;
					uniform highp mat4 _Object2World;
					uniform highp mat4 _World2Object;
					uniform highp vec3 _V_CW_Bend;
					uniform highp vec3 _V_CW_Bias;
					uniform highp vec4 _V_CW_PivotPoint_Position;
					uniform highp vec4 _MainTex_ST;
					varying highp vec3 xlv_TEXCOORD0;
					varying highp vec2 xlv_TEXCOORD1;
					void main ()
					{
					  highp vec4 vertex_1;
					  highp vec2 xzOff_2;
					  highp vec4 worldPos_3;
					  highp vec4 tmpvar_4;
					  tmpvar_4 = (_Object2World * _glesVertex);
					  worldPos_3.w = tmpvar_4.w;
					  worldPos_3.xyz = (tmpvar_4.xyz - _V_CW_PivotPoint_Position.xyz);
					  highp vec2 tmpvar_5;
					  tmpvar_5.x = float((worldPos_3.z >= 0.0));
					  tmpvar_5.y = float((worldPos_3.x >= 0.0));
					  xzOff_2 = (max (vec2(0.0, 0.0), (
					    abs(worldPos_3.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_5 * 2.0) - 1.0));
					  xzOff_2 = (xzOff_2 * xzOff_2);
					  highp vec4 tmpvar_6;
					  tmpvar_6.xzw = vec3(0.0, 0.0, 0.0);
					  tmpvar_6.y = (((_V_CW_Bend.x * xzOff_2.x) + (_V_CW_Bend.z * xzOff_2.y)) * 0.001);
					  worldPos_3 = tmpvar_6;
					  vertex_1 = (_glesVertex + (_World2Object * tmpvar_6));
					  xlv_TEXCOORD0 = ((_Object2World * vertex_1).xyz - _LightPositionRange.xyz);
					  gl_Position = (glstate_matrix_mvp * vertex_1);
					  xlv_TEXCOORD1 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform highp vec4 _LightPositionRange;
					uniform highp vec4 unity_LightShadowBias;
					uniform sampler2D _MainTex;
					uniform lowp float _Cutoff;
					uniform lowp vec4 _Color;
					varying highp vec3 xlv_TEXCOORD0;
					varying highp vec2 xlv_TEXCOORD1;
					void main ()
					{
					  lowp float x_1;
					  x_1 = ((texture2D (_MainTex, xlv_TEXCOORD1).w * _Color.w) - _Cutoff);
					  if ((x_1 < 0.0)) {
					    discard;
					  };
					  highp vec4 tmpvar_2;
					  tmpvar_2 = fract((vec4(1.0, 255.0, 65025.0, 1.658138e+07) * min (
					    ((sqrt(dot (xlv_TEXCOORD0, xlv_TEXCOORD0)) + unity_LightShadowBias.x) * _LightPositionRange.w)
					  , 0.999)));
					  highp vec4 tmpvar_3;
					  tmpvar_3 = (tmpvar_2 - (tmpvar_2.yzww * 0.003921569));
					  gl_FragData[0] = tmpvar_3;
					}
					
					
					#endif"
}
}
Program "fp" {
SubProgram "gles " {
Keywords { "SHADOWS_DEPTH" }
					"!!GLES"
}
SubProgram "gles " {
Keywords { "SHADOWS_CUBE" }
					"!!GLES"
}
}
 }
}
}