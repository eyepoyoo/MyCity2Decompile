Shader "Hidden/VacuumShaders/Curved World/Legacy Shader/Transparent/Vertex Alpha" {
Properties {
[CurvedWorldGearMenu]  V_CW_Label_Tag ("", Float) = 0
[CurvedWorldLabel]  V_CW_Label_UnityDefaults ("Default Visual Options", Float) = 0
[CurvedWorldLargeLabel]  V_CW_Label_Modes ("Modes", Float) = 0
[CurvedWorldRenderingMode]  V_CW_Rendering_Mode ("  Rendering", Float) = 0
[CurvedWorldTextureMixMode]  V_CW_Texture_Mix_Mode ("  Texture Mix", Float) = 0
[CurvedWorldLargeLabel]  V_CW_Label_Albedo ("Albedo", Float) = 0
 _Color ("  Color", Color) = (1,1,1,1)
 _MainTex ("  Map (RGB) RefStr, Gloss & Trans (A)", 2D) = "white" { }
[CurvedWorldUVScroll]  _V_CW_MainTex_Scroll ("    ", Vector) = (0,0,0,0)
 _V_CW_SecondaryTex_Blend ("  Blend Alpha Offset", Range(-1,1)) = 0
 _V_CW_SecondaryTex ("  Blend Map", 2D) = "gray" { }
[CurvedWorldUVScroll]  _V_CW_SecondaryTex_Scroll ("    ", Vector) = (0,0,0,0)
[CurvedWorldLabel]  V_CW_Label_UnityDefaults ("Curved World Optionals", Float) = 0
[HideInInspector]  _V_CW_Rim_Color ("", Color) = (1,1,1,1)
[HideInInspector]  _V_CW_Rim_Bias ("", Range(-1,1)) = 0.2
[HideInInspector]  _V_CW_Rim_Power ("", Range(0.5,8)) = 3
[HideInInspector]  _EmissionMap ("", 2D) = "white" { }
[HideInInspector]  _EmissionColor ("", Color) = (1,1,1,1)
[HideInInspector]  _SpecColor ("", Color) = (0.5,0.5,0.5,1)
[HideInInspector]  _Shininess ("", Range(0.01,1)) = 0.078125
[HideInInspector]  _V_CW_ReflectColor ("", Color) = (1,1,1,1)
[HideInInspector]  _V_CW_ReflectStrengthAlphaOffset ("", Range(-1,1)) = 0
[HideInInspector]  _V_CW_Cube ("", CUBE) = "_Skybox" { }
[HideInInspector]  _V_CW_Fresnel_Power ("", Range(0.5,8)) = 1
[HideInInspector]  _V_CW_Fresnel_Bias ("", Range(-1,1)) = 0
[HideInInspector]  _V_CW_NormalMapStrength ("", Float) = 1
[HideInInspector]  _V_CW_NormalMap ("", 2D) = "bump" { }
[HideInInspector]  _V_CW_NormalMap_UV_Scale ("", Float) = 1
[HideInInspector]  _V_CW_SecondaryNormalMap ("", 2D) = "" { }
[HideInInspector]  _V_CW_SecondaryNormalMap_UV_Scale ("", Float) = 1
}
SubShader { 
 LOD 200
 Tags { "QUEUE"="Transparent" "IGNOREPROJECTOR"="true" "RenderType"="Transparent" "CurvedWorldTag"="Legacy Shader/Transparent/Vertex Alpha" "CurvedWorldNoneRemoveableKeywords"="" "CurvedWorldAvailableOptions"="V_CW_REFLECTIVE;V_CW_VERTEX_COLOR;_EMISSION;V_CW_RIM;_NORMALMAP;V_CW_SPECULAR_HD;" }
 Pass {
  Name "FORWARD"
  Tags { "LIGHTMODE"="ForwardBase" "QUEUE"="Transparent" "IGNOREPROJECTOR"="true" "RenderType"="Transparent" "CurvedWorldTag"="Legacy Shader/Transparent/Vertex Alpha" "CurvedWorldNoneRemoveableKeywords"="" "CurvedWorldAvailableOptions"="V_CW_REFLECTIVE;V_CW_VERTEX_COLOR;_EMISSION;V_CW_RIM;_NORMALMAP;V_CW_SPECULAR_HD;" }
  ZWrite Off
  Blend SrcAlpha OneMinusSrcAlpha
  ColorMask RGB
  GpuProgramID 50992
Program "vp" {
SubProgram "gles " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "V_CW_REFLECTIVE_OFF" "V_CW_VERTEX_COLOR_OFF" "_EMISSION_OFF" "V_CW_RIM_OFF" "_NORMALMAP_OFF" "V_CW_SPECULAR_OFF" }
					"!!GLES
					#version 100
					
					#ifdef VERTEX
					attribute vec4 _glesTANGENT;
					attribute vec4 _glesVertex;
					attribute vec4 _glesColor;
					attribute vec3 _glesNormal;
					attribute vec4 _glesMultiTexCoord0;
					uniform mediump vec4 unity_SHBr;
					uniform mediump vec4 unity_SHBg;
					uniform mediump vec4 unity_SHBb;
					uniform mediump vec4 unity_SHC;
					uniform highp mat4 glstate_matrix_mvp;
					uniform highp mat4 _Object2World;
					uniform highp mat4 _World2Object;
					uniform highp vec3 _V_CW_Bend;
					uniform highp vec3 _V_CW_Bias;
					uniform highp vec4 _V_CW_PivotPoint_Position;
					uniform highp vec4 _MainTex_ST;
					uniform highp vec4 _V_CW_SecondaryTex_ST;
					varying highp vec4 xlv_TEXCOORD0;
					varying mediump vec3 xlv_TEXCOORD1;
					varying highp vec3 xlv_TEXCOORD2;
					varying lowp vec4 xlv_COLOR0;
					varying mediump vec3 xlv_TEXCOORD3;
					void main ()
					{
					  lowp vec3 worldNormal_1;
					  highp vec4 tmpvar_2;
					  mediump vec3 tmpvar_3;
					  highp vec4 v_4;
					  highp float tmpvar_5;
					  tmpvar_5 = _World2Object[0].x;
					  v_4.x = tmpvar_5;
					  highp float tmpvar_6;
					  tmpvar_6 = _World2Object[1].x;
					  v_4.y = tmpvar_6;
					  highp float tmpvar_7;
					  tmpvar_7 = _World2Object[2].x;
					  v_4.z = tmpvar_7;
					  highp float tmpvar_8;
					  tmpvar_8 = _World2Object[3].x;
					  v_4.w = tmpvar_8;
					  highp vec4 v_9;
					  highp float tmpvar_10;
					  tmpvar_10 = _World2Object[0].y;
					  v_9.x = tmpvar_10;
					  highp float tmpvar_11;
					  tmpvar_11 = _World2Object[1].y;
					  v_9.y = tmpvar_11;
					  highp float tmpvar_12;
					  tmpvar_12 = _World2Object[2].y;
					  v_9.z = tmpvar_12;
					  highp float tmpvar_13;
					  tmpvar_13 = _World2Object[3].y;
					  v_9.w = tmpvar_13;
					  highp vec4 v_14;
					  highp float tmpvar_15;
					  tmpvar_15 = _World2Object[0].z;
					  v_14.x = tmpvar_15;
					  highp float tmpvar_16;
					  tmpvar_16 = _World2Object[1].z;
					  v_14.y = tmpvar_16;
					  highp float tmpvar_17;
					  tmpvar_17 = _World2Object[2].z;
					  v_14.z = tmpvar_17;
					  highp float tmpvar_18;
					  tmpvar_18 = _World2Object[3].z;
					  v_14.w = tmpvar_18;
					  highp vec3 tmpvar_19;
					  tmpvar_19 = normalize(((
					    (v_4.xyz * _glesNormal.x)
					   + 
					    (v_9.xyz * _glesNormal.y)
					  ) + (v_14.xyz * _glesNormal.z)));
					  highp mat3 tmpvar_20;
					  tmpvar_20[0] = _Object2World[0].xyz;
					  tmpvar_20[1] = _Object2World[1].xyz;
					  tmpvar_20[2] = _Object2World[2].xyz;
					  highp vec3 tmpvar_21;
					  tmpvar_21 = normalize((tmpvar_20 * _glesTANGENT.xyz));
					  highp vec4 vertex_22;
					  vertex_22.w = _glesVertex.w;
					  highp vec2 xzOff_23;
					  highp vec3 v2_24;
					  highp vec3 v1_25;
					  highp vec3 v0_26;
					  highp vec3 tmpvar_27;
					  tmpvar_27 = ((_Object2World * _glesVertex).xyz - _V_CW_PivotPoint_Position.xyz);
					  highp vec3 tmpvar_28;
					  tmpvar_28 = (tmpvar_27 + tmpvar_21);
					  v1_25.xz = tmpvar_28.xz;
					  highp vec3 tmpvar_29;
					  tmpvar_29 = (tmpvar_27 - ((tmpvar_19.yzx * tmpvar_21.zxy) - (tmpvar_19.zxy * tmpvar_21.yzx)));
					  v2_24.xz = tmpvar_29.xz;
					  highp vec2 tmpvar_30;
					  tmpvar_30.x = float((tmpvar_27.z >= 0.0));
					  tmpvar_30.y = float((tmpvar_27.x >= 0.0));
					  xzOff_23 = (max (vec2(0.0, 0.0), (
					    abs(tmpvar_27.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_30 * 2.0) - 1.0));
					  xzOff_23 = (xzOff_23 * xzOff_23);
					  highp vec3 tmpvar_31;
					  tmpvar_31.xz = vec2(0.0, 0.0);
					  tmpvar_31.y = (((_V_CW_Bend.x * xzOff_23.x) + (_V_CW_Bend.z * xzOff_23.y)) * 0.001);
					  v0_26 = (tmpvar_27 + tmpvar_31);
					  highp vec2 tmpvar_32;
					  tmpvar_32.x = float((tmpvar_28.z >= 0.0));
					  tmpvar_32.y = float((tmpvar_28.x >= 0.0));
					  xzOff_23 = (max (vec2(0.0, 0.0), (
					    abs(tmpvar_28.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_32 * 2.0) - 1.0));
					  xzOff_23 = (xzOff_23 * xzOff_23);
					  v1_25.y = (tmpvar_28.y + ((
					    (_V_CW_Bend.x * xzOff_23.x)
					   + 
					    (_V_CW_Bend.z * xzOff_23.y)
					  ) * 0.001));
					  highp vec2 tmpvar_33;
					  tmpvar_33.x = float((tmpvar_29.z >= 0.0));
					  tmpvar_33.y = float((tmpvar_29.x >= 0.0));
					  xzOff_23 = (max (vec2(0.0, 0.0), (
					    abs(tmpvar_29.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_33 * 2.0) - 1.0));
					  xzOff_23 = (xzOff_23 * xzOff_23);
					  v2_24.y = (tmpvar_29.y + ((
					    (_V_CW_Bend.x * xzOff_23.x)
					   + 
					    (_V_CW_Bend.z * xzOff_23.y)
					  ) * 0.001));
					  highp mat3 tmpvar_34;
					  tmpvar_34[0] = _World2Object[0].xyz;
					  tmpvar_34[1] = _World2Object[1].xyz;
					  tmpvar_34[2] = _World2Object[2].xyz;
					  vertex_22.xyz = (_glesVertex.xyz + (tmpvar_34 * tmpvar_31));
					  highp mat3 tmpvar_35;
					  tmpvar_35[0] = _World2Object[0].xyz;
					  tmpvar_35[1] = _World2Object[1].xyz;
					  tmpvar_35[2] = _World2Object[2].xyz;
					  highp vec3 a_36;
					  a_36 = (v2_24 - v0_26);
					  highp vec3 b_37;
					  b_37 = (v1_25 - v0_26);
					  highp vec3 tmpvar_38;
					  tmpvar_38 = normalize((tmpvar_35 * normalize(
					    ((a_36.yzx * b_37.zxy) - (a_36.zxy * b_37.yzx))
					  )));
					  tmpvar_2.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
					  tmpvar_2.zw = ((_glesMultiTexCoord0.xy * _V_CW_SecondaryTex_ST.xy) + _V_CW_SecondaryTex_ST.zw);
					  highp vec4 v_39;
					  v_39.x = tmpvar_5;
					  v_39.y = tmpvar_6;
					  v_39.z = tmpvar_7;
					  v_39.w = tmpvar_8;
					  highp vec4 v_40;
					  v_40.x = tmpvar_10;
					  v_40.y = tmpvar_11;
					  v_40.z = tmpvar_12;
					  v_40.w = tmpvar_13;
					  highp vec4 v_41;
					  v_41.x = tmpvar_15;
					  v_41.y = tmpvar_16;
					  v_41.z = tmpvar_17;
					  v_41.w = tmpvar_18;
					  highp vec3 tmpvar_42;
					  tmpvar_42 = normalize(((
					    (v_39.xyz * tmpvar_38.x)
					   + 
					    (v_40.xyz * tmpvar_38.y)
					  ) + (v_41.xyz * tmpvar_38.z)));
					  worldNormal_1 = tmpvar_42;
					  tmpvar_3 = worldNormal_1;
					  mediump vec3 normal_43;
					  normal_43 = worldNormal_1;
					  mediump vec3 x1_44;
					  mediump vec4 tmpvar_45;
					  tmpvar_45 = (normal_43.xyzz * normal_43.yzzx);
					  x1_44.x = dot (unity_SHBr, tmpvar_45);
					  x1_44.y = dot (unity_SHBg, tmpvar_45);
					  x1_44.z = dot (unity_SHBb, tmpvar_45);
					  gl_Position = (glstate_matrix_mvp * vertex_22);
					  xlv_TEXCOORD0 = tmpvar_2;
					  xlv_TEXCOORD1 = tmpvar_3;
					  xlv_TEXCOORD2 = (_Object2World * vertex_22).xyz;
					  xlv_COLOR0 = _glesColor;
					  xlv_TEXCOORD3 = (x1_44 + (unity_SHC.xyz * (
					    (normal_43.x * normal_43.x)
					   - 
					    (normal_43.y * normal_43.y)
					  )));
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform highp vec4 _Time;
					uniform mediump vec4 _WorldSpaceLightPos0;
					uniform mediump vec4 unity_SHAr;
					uniform mediump vec4 unity_SHAg;
					uniform mediump vec4 unity_SHAb;
					uniform lowp vec4 _LightColor0;
					uniform sampler2D _MainTex;
					uniform lowp vec4 _Color;
					uniform lowp vec2 _V_CW_MainTex_Scroll;
					uniform sampler2D _V_CW_SecondaryTex;
					uniform lowp vec2 _V_CW_SecondaryTex_Scroll;
					uniform lowp float _V_CW_SecondaryTex_Blend;
					varying highp vec4 xlv_TEXCOORD0;
					varying mediump vec3 xlv_TEXCOORD1;
					varying lowp vec4 xlv_COLOR0;
					varying mediump vec3 xlv_TEXCOORD3;
					void main ()
					{
					  mediump vec3 tmpvar_1;
					  mediump vec3 tmpvar_2;
					  lowp vec3 tmpvar_3;
					  lowp vec3 lightDir_4;
					  highp vec4 tmpvar_5;
					  mediump vec3 tmpvar_6;
					  tmpvar_6 = _WorldSpaceLightPos0.xyz;
					  lightDir_4 = tmpvar_6;
					  tmpvar_5 = xlv_COLOR0;
					  tmpvar_3 = xlv_TEXCOORD1;
					  lowp vec3 tmpvar_7;
					  lowp float vBlend_8;
					  highp vec2 P_9;
					  P_9 = (xlv_TEXCOORD0.xy + (_V_CW_MainTex_Scroll * _Time.x));
					  highp float tmpvar_10;
					  tmpvar_10 = clamp ((_V_CW_SecondaryTex_Blend + tmpvar_5.w), 0.0, 1.0);
					  vBlend_8 = tmpvar_10;
					  highp vec2 P_11;
					  P_11 = (xlv_TEXCOORD0.zw + (_V_CW_SecondaryTex_Scroll * _Time.x));
					  lowp vec4 tmpvar_12;
					  tmpvar_12 = mix (texture2D (_MainTex, P_9), texture2D (_V_CW_SecondaryTex, P_11), vec4(vBlend_8));
					  tmpvar_7 = (tmpvar_12.xyz * _Color.xyz);
					  tmpvar_1 = _LightColor0.xyz;
					  tmpvar_2 = lightDir_4;
					  mediump vec3 normalWorld_13;
					  normalWorld_13 = tmpvar_3;
					  mediump vec3 ambient_14;
					  mediump vec4 tmpvar_15;
					  tmpvar_15.w = 1.0;
					  tmpvar_15.xyz = normalWorld_13;
					  mediump vec3 x_16;
					  x_16.x = dot (unity_SHAr, tmpvar_15);
					  x_16.y = dot (unity_SHAg, tmpvar_15);
					  x_16.z = dot (unity_SHAb, tmpvar_15);
					  ambient_14 = max (((1.055 * 
					    pow (max (vec3(0.0, 0.0, 0.0), (xlv_TEXCOORD3 + x_16)), vec3(0.4166667, 0.4166667, 0.4166667))
					  ) - 0.055), vec3(0.0, 0.0, 0.0));
					  lowp vec4 c_17;
					  lowp vec4 c_18;
					  lowp float diff_19;
					  mediump float tmpvar_20;
					  tmpvar_20 = max (0.0, dot (tmpvar_3, tmpvar_2));
					  diff_19 = tmpvar_20;
					  c_18.xyz = ((tmpvar_7 * tmpvar_1) * diff_19);
					  c_18.w = (tmpvar_12.w * _Color.w);
					  c_17.w = c_18.w;
					  c_17.xyz = (c_18.xyz + (tmpvar_7 * ambient_14));
					  gl_FragData[0] = c_17;
					}
					
					
					#endif"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "VERTEXLIGHT_ON" "V_CW_REFLECTIVE_OFF" "V_CW_VERTEX_COLOR_OFF" "_EMISSION_OFF" "V_CW_RIM_OFF" "_NORMALMAP_OFF" "V_CW_SPECULAR_OFF" }
					"!!GLES
					#version 100
					
					#ifdef VERTEX
					attribute vec4 _glesTANGENT;
					attribute vec4 _glesVertex;
					attribute vec4 _glesColor;
					attribute vec3 _glesNormal;
					attribute vec4 _glesMultiTexCoord0;
					uniform highp vec4 unity_4LightPosX0;
					uniform highp vec4 unity_4LightPosY0;
					uniform highp vec4 unity_4LightPosZ0;
					uniform mediump vec4 unity_4LightAtten0;
					uniform mediump vec4 unity_LightColor[8];
					uniform mediump vec4 unity_SHBr;
					uniform mediump vec4 unity_SHBg;
					uniform mediump vec4 unity_SHBb;
					uniform mediump vec4 unity_SHC;
					uniform highp mat4 glstate_matrix_mvp;
					uniform highp mat4 _Object2World;
					uniform highp mat4 _World2Object;
					uniform highp vec3 _V_CW_Bend;
					uniform highp vec3 _V_CW_Bias;
					uniform highp vec4 _V_CW_PivotPoint_Position;
					uniform highp vec4 _MainTex_ST;
					uniform highp vec4 _V_CW_SecondaryTex_ST;
					varying highp vec4 xlv_TEXCOORD0;
					varying mediump vec3 xlv_TEXCOORD1;
					varying highp vec3 xlv_TEXCOORD2;
					varying lowp vec4 xlv_COLOR0;
					varying mediump vec3 xlv_TEXCOORD3;
					void main ()
					{
					  lowp vec3 worldNormal_1;
					  highp vec4 tmpvar_2;
					  mediump vec3 tmpvar_3;
					  mediump vec3 tmpvar_4;
					  highp vec4 v_5;
					  highp float tmpvar_6;
					  tmpvar_6 = _World2Object[0].x;
					  v_5.x = tmpvar_6;
					  highp float tmpvar_7;
					  tmpvar_7 = _World2Object[1].x;
					  v_5.y = tmpvar_7;
					  highp float tmpvar_8;
					  tmpvar_8 = _World2Object[2].x;
					  v_5.z = tmpvar_8;
					  highp float tmpvar_9;
					  tmpvar_9 = _World2Object[3].x;
					  v_5.w = tmpvar_9;
					  highp vec4 v_10;
					  highp float tmpvar_11;
					  tmpvar_11 = _World2Object[0].y;
					  v_10.x = tmpvar_11;
					  highp float tmpvar_12;
					  tmpvar_12 = _World2Object[1].y;
					  v_10.y = tmpvar_12;
					  highp float tmpvar_13;
					  tmpvar_13 = _World2Object[2].y;
					  v_10.z = tmpvar_13;
					  highp float tmpvar_14;
					  tmpvar_14 = _World2Object[3].y;
					  v_10.w = tmpvar_14;
					  highp vec4 v_15;
					  highp float tmpvar_16;
					  tmpvar_16 = _World2Object[0].z;
					  v_15.x = tmpvar_16;
					  highp float tmpvar_17;
					  tmpvar_17 = _World2Object[1].z;
					  v_15.y = tmpvar_17;
					  highp float tmpvar_18;
					  tmpvar_18 = _World2Object[2].z;
					  v_15.z = tmpvar_18;
					  highp float tmpvar_19;
					  tmpvar_19 = _World2Object[3].z;
					  v_15.w = tmpvar_19;
					  highp vec3 tmpvar_20;
					  tmpvar_20 = normalize(((
					    (v_5.xyz * _glesNormal.x)
					   + 
					    (v_10.xyz * _glesNormal.y)
					  ) + (v_15.xyz * _glesNormal.z)));
					  highp mat3 tmpvar_21;
					  tmpvar_21[0] = _Object2World[0].xyz;
					  tmpvar_21[1] = _Object2World[1].xyz;
					  tmpvar_21[2] = _Object2World[2].xyz;
					  highp vec3 tmpvar_22;
					  tmpvar_22 = normalize((tmpvar_21 * _glesTANGENT.xyz));
					  highp vec4 vertex_23;
					  vertex_23.w = _glesVertex.w;
					  highp vec2 xzOff_24;
					  highp vec3 v2_25;
					  highp vec3 v1_26;
					  highp vec3 v0_27;
					  highp vec3 tmpvar_28;
					  tmpvar_28 = ((_Object2World * _glesVertex).xyz - _V_CW_PivotPoint_Position.xyz);
					  highp vec3 tmpvar_29;
					  tmpvar_29 = (tmpvar_28 + tmpvar_22);
					  v1_26.xz = tmpvar_29.xz;
					  highp vec3 tmpvar_30;
					  tmpvar_30 = (tmpvar_28 - ((tmpvar_20.yzx * tmpvar_22.zxy) - (tmpvar_20.zxy * tmpvar_22.yzx)));
					  v2_25.xz = tmpvar_30.xz;
					  highp vec2 tmpvar_31;
					  tmpvar_31.x = float((tmpvar_28.z >= 0.0));
					  tmpvar_31.y = float((tmpvar_28.x >= 0.0));
					  xzOff_24 = (max (vec2(0.0, 0.0), (
					    abs(tmpvar_28.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_31 * 2.0) - 1.0));
					  xzOff_24 = (xzOff_24 * xzOff_24);
					  highp vec3 tmpvar_32;
					  tmpvar_32.xz = vec2(0.0, 0.0);
					  tmpvar_32.y = (((_V_CW_Bend.x * xzOff_24.x) + (_V_CW_Bend.z * xzOff_24.y)) * 0.001);
					  v0_27 = (tmpvar_28 + tmpvar_32);
					  highp vec2 tmpvar_33;
					  tmpvar_33.x = float((tmpvar_29.z >= 0.0));
					  tmpvar_33.y = float((tmpvar_29.x >= 0.0));
					  xzOff_24 = (max (vec2(0.0, 0.0), (
					    abs(tmpvar_29.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_33 * 2.0) - 1.0));
					  xzOff_24 = (xzOff_24 * xzOff_24);
					  v1_26.y = (tmpvar_29.y + ((
					    (_V_CW_Bend.x * xzOff_24.x)
					   + 
					    (_V_CW_Bend.z * xzOff_24.y)
					  ) * 0.001));
					  highp vec2 tmpvar_34;
					  tmpvar_34.x = float((tmpvar_30.z >= 0.0));
					  tmpvar_34.y = float((tmpvar_30.x >= 0.0));
					  xzOff_24 = (max (vec2(0.0, 0.0), (
					    abs(tmpvar_30.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_34 * 2.0) - 1.0));
					  xzOff_24 = (xzOff_24 * xzOff_24);
					  v2_25.y = (tmpvar_30.y + ((
					    (_V_CW_Bend.x * xzOff_24.x)
					   + 
					    (_V_CW_Bend.z * xzOff_24.y)
					  ) * 0.001));
					  highp mat3 tmpvar_35;
					  tmpvar_35[0] = _World2Object[0].xyz;
					  tmpvar_35[1] = _World2Object[1].xyz;
					  tmpvar_35[2] = _World2Object[2].xyz;
					  vertex_23.xyz = (_glesVertex.xyz + (tmpvar_35 * tmpvar_32));
					  highp mat3 tmpvar_36;
					  tmpvar_36[0] = _World2Object[0].xyz;
					  tmpvar_36[1] = _World2Object[1].xyz;
					  tmpvar_36[2] = _World2Object[2].xyz;
					  highp vec3 a_37;
					  a_37 = (v2_25 - v0_27);
					  highp vec3 b_38;
					  b_38 = (v1_26 - v0_27);
					  highp vec3 tmpvar_39;
					  tmpvar_39 = normalize((tmpvar_36 * normalize(
					    ((a_37.yzx * b_38.zxy) - (a_37.zxy * b_38.yzx))
					  )));
					  tmpvar_2.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
					  tmpvar_2.zw = ((_glesMultiTexCoord0.xy * _V_CW_SecondaryTex_ST.xy) + _V_CW_SecondaryTex_ST.zw);
					  highp vec3 tmpvar_40;
					  tmpvar_40 = (_Object2World * vertex_23).xyz;
					  highp vec4 v_41;
					  v_41.x = tmpvar_6;
					  v_41.y = tmpvar_7;
					  v_41.z = tmpvar_8;
					  v_41.w = tmpvar_9;
					  highp vec4 v_42;
					  v_42.x = tmpvar_11;
					  v_42.y = tmpvar_12;
					  v_42.z = tmpvar_13;
					  v_42.w = tmpvar_14;
					  highp vec4 v_43;
					  v_43.x = tmpvar_16;
					  v_43.y = tmpvar_17;
					  v_43.z = tmpvar_18;
					  v_43.w = tmpvar_19;
					  highp vec3 tmpvar_44;
					  tmpvar_44 = normalize(((
					    (v_41.xyz * tmpvar_39.x)
					   + 
					    (v_42.xyz * tmpvar_39.y)
					  ) + (v_43.xyz * tmpvar_39.z)));
					  worldNormal_1 = tmpvar_44;
					  tmpvar_3 = worldNormal_1;
					  highp vec3 lightColor0_45;
					  lightColor0_45 = unity_LightColor[0].xyz;
					  highp vec3 lightColor1_46;
					  lightColor1_46 = unity_LightColor[1].xyz;
					  highp vec3 lightColor2_47;
					  lightColor2_47 = unity_LightColor[2].xyz;
					  highp vec3 lightColor3_48;
					  lightColor3_48 = unity_LightColor[3].xyz;
					  highp vec4 lightAttenSq_49;
					  lightAttenSq_49 = unity_4LightAtten0;
					  highp vec3 normal_50;
					  normal_50 = worldNormal_1;
					  highp vec3 col_51;
					  highp vec4 ndotl_52;
					  highp vec4 lengthSq_53;
					  highp vec4 tmpvar_54;
					  tmpvar_54 = (unity_4LightPosX0 - tmpvar_40.x);
					  highp vec4 tmpvar_55;
					  tmpvar_55 = (unity_4LightPosY0 - tmpvar_40.y);
					  highp vec4 tmpvar_56;
					  tmpvar_56 = (unity_4LightPosZ0 - tmpvar_40.z);
					  lengthSq_53 = (tmpvar_54 * tmpvar_54);
					  lengthSq_53 = (lengthSq_53 + (tmpvar_55 * tmpvar_55));
					  lengthSq_53 = (lengthSq_53 + (tmpvar_56 * tmpvar_56));
					  ndotl_52 = (tmpvar_54 * normal_50.x);
					  ndotl_52 = (ndotl_52 + (tmpvar_55 * normal_50.y));
					  ndotl_52 = (ndotl_52 + (tmpvar_56 * normal_50.z));
					  highp vec4 tmpvar_57;
					  tmpvar_57 = max (vec4(0.0, 0.0, 0.0, 0.0), (ndotl_52 * inversesqrt(lengthSq_53)));
					  ndotl_52 = tmpvar_57;
					  highp vec4 tmpvar_58;
					  tmpvar_58 = (tmpvar_57 * (1.0/((1.0 + 
					    (lengthSq_53 * lightAttenSq_49)
					  ))));
					  col_51 = (lightColor0_45 * tmpvar_58.x);
					  col_51 = (col_51 + (lightColor1_46 * tmpvar_58.y));
					  col_51 = (col_51 + (lightColor2_47 * tmpvar_58.z));
					  col_51 = (col_51 + (lightColor3_48 * tmpvar_58.w));
					  tmpvar_4 = col_51;
					  mediump vec3 normal_59;
					  normal_59 = worldNormal_1;
					  mediump vec3 ambient_60;
					  ambient_60 = (tmpvar_4 * ((tmpvar_4 * 
					    ((tmpvar_4 * 0.305306) + 0.6821711)
					  ) + 0.01252288));
					  mediump vec3 x1_61;
					  mediump vec4 tmpvar_62;
					  tmpvar_62 = (normal_59.xyzz * normal_59.yzzx);
					  x1_61.x = dot (unity_SHBr, tmpvar_62);
					  x1_61.y = dot (unity_SHBg, tmpvar_62);
					  x1_61.z = dot (unity_SHBb, tmpvar_62);
					  ambient_60 = (ambient_60 + (x1_61 + (unity_SHC.xyz * 
					    ((normal_59.x * normal_59.x) - (normal_59.y * normal_59.y))
					  )));
					  tmpvar_4 = ambient_60;
					  gl_Position = (glstate_matrix_mvp * vertex_23);
					  xlv_TEXCOORD0 = tmpvar_2;
					  xlv_TEXCOORD1 = tmpvar_3;
					  xlv_TEXCOORD2 = tmpvar_40;
					  xlv_COLOR0 = _glesColor;
					  xlv_TEXCOORD3 = ambient_60;
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform highp vec4 _Time;
					uniform mediump vec4 _WorldSpaceLightPos0;
					uniform mediump vec4 unity_SHAr;
					uniform mediump vec4 unity_SHAg;
					uniform mediump vec4 unity_SHAb;
					uniform lowp vec4 _LightColor0;
					uniform sampler2D _MainTex;
					uniform lowp vec4 _Color;
					uniform lowp vec2 _V_CW_MainTex_Scroll;
					uniform sampler2D _V_CW_SecondaryTex;
					uniform lowp vec2 _V_CW_SecondaryTex_Scroll;
					uniform lowp float _V_CW_SecondaryTex_Blend;
					varying highp vec4 xlv_TEXCOORD0;
					varying mediump vec3 xlv_TEXCOORD1;
					varying lowp vec4 xlv_COLOR0;
					varying mediump vec3 xlv_TEXCOORD3;
					void main ()
					{
					  mediump vec3 tmpvar_1;
					  mediump vec3 tmpvar_2;
					  lowp vec3 tmpvar_3;
					  lowp vec3 lightDir_4;
					  highp vec4 tmpvar_5;
					  mediump vec3 tmpvar_6;
					  tmpvar_6 = _WorldSpaceLightPos0.xyz;
					  lightDir_4 = tmpvar_6;
					  tmpvar_5 = xlv_COLOR0;
					  tmpvar_3 = xlv_TEXCOORD1;
					  lowp vec3 tmpvar_7;
					  lowp float vBlend_8;
					  highp vec2 P_9;
					  P_9 = (xlv_TEXCOORD0.xy + (_V_CW_MainTex_Scroll * _Time.x));
					  highp float tmpvar_10;
					  tmpvar_10 = clamp ((_V_CW_SecondaryTex_Blend + tmpvar_5.w), 0.0, 1.0);
					  vBlend_8 = tmpvar_10;
					  highp vec2 P_11;
					  P_11 = (xlv_TEXCOORD0.zw + (_V_CW_SecondaryTex_Scroll * _Time.x));
					  lowp vec4 tmpvar_12;
					  tmpvar_12 = mix (texture2D (_MainTex, P_9), texture2D (_V_CW_SecondaryTex, P_11), vec4(vBlend_8));
					  tmpvar_7 = (tmpvar_12.xyz * _Color.xyz);
					  tmpvar_1 = _LightColor0.xyz;
					  tmpvar_2 = lightDir_4;
					  mediump vec3 normalWorld_13;
					  normalWorld_13 = tmpvar_3;
					  mediump vec3 ambient_14;
					  mediump vec4 tmpvar_15;
					  tmpvar_15.w = 1.0;
					  tmpvar_15.xyz = normalWorld_13;
					  mediump vec3 x_16;
					  x_16.x = dot (unity_SHAr, tmpvar_15);
					  x_16.y = dot (unity_SHAg, tmpvar_15);
					  x_16.z = dot (unity_SHAb, tmpvar_15);
					  ambient_14 = max (((1.055 * 
					    pow (max (vec3(0.0, 0.0, 0.0), (xlv_TEXCOORD3 + x_16)), vec3(0.4166667, 0.4166667, 0.4166667))
					  ) - 0.055), vec3(0.0, 0.0, 0.0));
					  lowp vec4 c_17;
					  lowp vec4 c_18;
					  lowp float diff_19;
					  mediump float tmpvar_20;
					  tmpvar_20 = max (0.0, dot (tmpvar_3, tmpvar_2));
					  diff_19 = tmpvar_20;
					  c_18.xyz = ((tmpvar_7 * tmpvar_1) * diff_19);
					  c_18.w = (tmpvar_12.w * _Color.w);
					  c_17.w = c_18.w;
					  c_17.xyz = (c_18.xyz + (tmpvar_7 * ambient_14));
					  gl_FragData[0] = c_17;
					}
					
					
					#endif"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "FOG_LINEAR" "V_CW_REFLECTIVE_OFF" "V_CW_VERTEX_COLOR_OFF" "_EMISSION_OFF" "V_CW_RIM_OFF" "_NORMALMAP_OFF" "V_CW_SPECULAR_OFF" }
					"!!GLES
					#version 100
					
					#ifdef VERTEX
					attribute vec4 _glesTANGENT;
					attribute vec4 _glesVertex;
					attribute vec4 _glesColor;
					attribute vec3 _glesNormal;
					attribute vec4 _glesMultiTexCoord0;
					uniform mediump vec4 unity_SHBr;
					uniform mediump vec4 unity_SHBg;
					uniform mediump vec4 unity_SHBb;
					uniform mediump vec4 unity_SHC;
					uniform highp mat4 glstate_matrix_mvp;
					uniform highp mat4 _Object2World;
					uniform highp mat4 _World2Object;
					uniform highp vec4 unity_FogParams;
					uniform highp vec3 _V_CW_Bend;
					uniform highp vec3 _V_CW_Bias;
					uniform highp vec4 _V_CW_PivotPoint_Position;
					uniform highp vec4 _MainTex_ST;
					uniform highp vec4 _V_CW_SecondaryTex_ST;
					varying highp vec4 xlv_TEXCOORD0;
					varying mediump vec3 xlv_TEXCOORD1;
					varying highp vec3 xlv_TEXCOORD2;
					varying lowp vec4 xlv_COLOR0;
					varying mediump vec3 xlv_TEXCOORD3;
					varying highp float xlv_TEXCOORD4;
					void main ()
					{
					  lowp vec3 worldNormal_1;
					  highp vec4 tmpvar_2;
					  highp vec4 tmpvar_3;
					  mediump vec3 tmpvar_4;
					  highp vec4 v_5;
					  highp float tmpvar_6;
					  tmpvar_6 = _World2Object[0].x;
					  v_5.x = tmpvar_6;
					  highp float tmpvar_7;
					  tmpvar_7 = _World2Object[1].x;
					  v_5.y = tmpvar_7;
					  highp float tmpvar_8;
					  tmpvar_8 = _World2Object[2].x;
					  v_5.z = tmpvar_8;
					  highp float tmpvar_9;
					  tmpvar_9 = _World2Object[3].x;
					  v_5.w = tmpvar_9;
					  highp vec4 v_10;
					  highp float tmpvar_11;
					  tmpvar_11 = _World2Object[0].y;
					  v_10.x = tmpvar_11;
					  highp float tmpvar_12;
					  tmpvar_12 = _World2Object[1].y;
					  v_10.y = tmpvar_12;
					  highp float tmpvar_13;
					  tmpvar_13 = _World2Object[2].y;
					  v_10.z = tmpvar_13;
					  highp float tmpvar_14;
					  tmpvar_14 = _World2Object[3].y;
					  v_10.w = tmpvar_14;
					  highp vec4 v_15;
					  highp float tmpvar_16;
					  tmpvar_16 = _World2Object[0].z;
					  v_15.x = tmpvar_16;
					  highp float tmpvar_17;
					  tmpvar_17 = _World2Object[1].z;
					  v_15.y = tmpvar_17;
					  highp float tmpvar_18;
					  tmpvar_18 = _World2Object[2].z;
					  v_15.z = tmpvar_18;
					  highp float tmpvar_19;
					  tmpvar_19 = _World2Object[3].z;
					  v_15.w = tmpvar_19;
					  highp vec3 tmpvar_20;
					  tmpvar_20 = normalize(((
					    (v_5.xyz * _glesNormal.x)
					   + 
					    (v_10.xyz * _glesNormal.y)
					  ) + (v_15.xyz * _glesNormal.z)));
					  highp mat3 tmpvar_21;
					  tmpvar_21[0] = _Object2World[0].xyz;
					  tmpvar_21[1] = _Object2World[1].xyz;
					  tmpvar_21[2] = _Object2World[2].xyz;
					  highp vec3 tmpvar_22;
					  tmpvar_22 = normalize((tmpvar_21 * _glesTANGENT.xyz));
					  highp vec4 vertex_23;
					  vertex_23.w = _glesVertex.w;
					  highp vec2 xzOff_24;
					  highp vec3 v2_25;
					  highp vec3 v1_26;
					  highp vec3 v0_27;
					  highp vec3 tmpvar_28;
					  tmpvar_28 = ((_Object2World * _glesVertex).xyz - _V_CW_PivotPoint_Position.xyz);
					  highp vec3 tmpvar_29;
					  tmpvar_29 = (tmpvar_28 + tmpvar_22);
					  v1_26.xz = tmpvar_29.xz;
					  highp vec3 tmpvar_30;
					  tmpvar_30 = (tmpvar_28 - ((tmpvar_20.yzx * tmpvar_22.zxy) - (tmpvar_20.zxy * tmpvar_22.yzx)));
					  v2_25.xz = tmpvar_30.xz;
					  highp vec2 tmpvar_31;
					  tmpvar_31.x = float((tmpvar_28.z >= 0.0));
					  tmpvar_31.y = float((tmpvar_28.x >= 0.0));
					  xzOff_24 = (max (vec2(0.0, 0.0), (
					    abs(tmpvar_28.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_31 * 2.0) - 1.0));
					  xzOff_24 = (xzOff_24 * xzOff_24);
					  highp vec3 tmpvar_32;
					  tmpvar_32.xz = vec2(0.0, 0.0);
					  tmpvar_32.y = (((_V_CW_Bend.x * xzOff_24.x) + (_V_CW_Bend.z * xzOff_24.y)) * 0.001);
					  v0_27 = (tmpvar_28 + tmpvar_32);
					  highp vec2 tmpvar_33;
					  tmpvar_33.x = float((tmpvar_29.z >= 0.0));
					  tmpvar_33.y = float((tmpvar_29.x >= 0.0));
					  xzOff_24 = (max (vec2(0.0, 0.0), (
					    abs(tmpvar_29.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_33 * 2.0) - 1.0));
					  xzOff_24 = (xzOff_24 * xzOff_24);
					  v1_26.y = (tmpvar_29.y + ((
					    (_V_CW_Bend.x * xzOff_24.x)
					   + 
					    (_V_CW_Bend.z * xzOff_24.y)
					  ) * 0.001));
					  highp vec2 tmpvar_34;
					  tmpvar_34.x = float((tmpvar_30.z >= 0.0));
					  tmpvar_34.y = float((tmpvar_30.x >= 0.0));
					  xzOff_24 = (max (vec2(0.0, 0.0), (
					    abs(tmpvar_30.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_34 * 2.0) - 1.0));
					  xzOff_24 = (xzOff_24 * xzOff_24);
					  v2_25.y = (tmpvar_30.y + ((
					    (_V_CW_Bend.x * xzOff_24.x)
					   + 
					    (_V_CW_Bend.z * xzOff_24.y)
					  ) * 0.001));
					  highp mat3 tmpvar_35;
					  tmpvar_35[0] = _World2Object[0].xyz;
					  tmpvar_35[1] = _World2Object[1].xyz;
					  tmpvar_35[2] = _World2Object[2].xyz;
					  vertex_23.xyz = (_glesVertex.xyz + (tmpvar_35 * tmpvar_32));
					  highp mat3 tmpvar_36;
					  tmpvar_36[0] = _World2Object[0].xyz;
					  tmpvar_36[1] = _World2Object[1].xyz;
					  tmpvar_36[2] = _World2Object[2].xyz;
					  highp vec3 a_37;
					  a_37 = (v2_25 - v0_27);
					  highp vec3 b_38;
					  b_38 = (v1_26 - v0_27);
					  highp vec3 tmpvar_39;
					  tmpvar_39 = normalize((tmpvar_36 * normalize(
					    ((a_37.yzx * b_38.zxy) - (a_37.zxy * b_38.yzx))
					  )));
					  tmpvar_2 = (glstate_matrix_mvp * vertex_23);
					  tmpvar_3.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
					  tmpvar_3.zw = ((_glesMultiTexCoord0.xy * _V_CW_SecondaryTex_ST.xy) + _V_CW_SecondaryTex_ST.zw);
					  highp vec4 v_40;
					  v_40.x = tmpvar_6;
					  v_40.y = tmpvar_7;
					  v_40.z = tmpvar_8;
					  v_40.w = tmpvar_9;
					  highp vec4 v_41;
					  v_41.x = tmpvar_11;
					  v_41.y = tmpvar_12;
					  v_41.z = tmpvar_13;
					  v_41.w = tmpvar_14;
					  highp vec4 v_42;
					  v_42.x = tmpvar_16;
					  v_42.y = tmpvar_17;
					  v_42.z = tmpvar_18;
					  v_42.w = tmpvar_19;
					  highp vec3 tmpvar_43;
					  tmpvar_43 = normalize(((
					    (v_40.xyz * tmpvar_39.x)
					   + 
					    (v_41.xyz * tmpvar_39.y)
					  ) + (v_42.xyz * tmpvar_39.z)));
					  worldNormal_1 = tmpvar_43;
					  tmpvar_4 = worldNormal_1;
					  mediump vec3 normal_44;
					  normal_44 = worldNormal_1;
					  mediump vec3 x1_45;
					  mediump vec4 tmpvar_46;
					  tmpvar_46 = (normal_44.xyzz * normal_44.yzzx);
					  x1_45.x = dot (unity_SHBr, tmpvar_46);
					  x1_45.y = dot (unity_SHBg, tmpvar_46);
					  x1_45.z = dot (unity_SHBb, tmpvar_46);
					  gl_Position = tmpvar_2;
					  xlv_TEXCOORD0 = tmpvar_3;
					  xlv_TEXCOORD1 = tmpvar_4;
					  xlv_TEXCOORD2 = (_Object2World * vertex_23).xyz;
					  xlv_COLOR0 = _glesColor;
					  xlv_TEXCOORD3 = (x1_45 + (unity_SHC.xyz * (
					    (normal_44.x * normal_44.x)
					   - 
					    (normal_44.y * normal_44.y)
					  )));
					  xlv_TEXCOORD4 = ((tmpvar_2.z * unity_FogParams.z) + unity_FogParams.w);
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform highp vec4 _Time;
					uniform mediump vec4 _WorldSpaceLightPos0;
					uniform mediump vec4 unity_SHAr;
					uniform mediump vec4 unity_SHAg;
					uniform mediump vec4 unity_SHAb;
					uniform lowp vec4 unity_FogColor;
					uniform lowp vec4 _LightColor0;
					uniform sampler2D _MainTex;
					uniform lowp vec4 _Color;
					uniform lowp vec2 _V_CW_MainTex_Scroll;
					uniform sampler2D _V_CW_SecondaryTex;
					uniform lowp vec2 _V_CW_SecondaryTex_Scroll;
					uniform lowp float _V_CW_SecondaryTex_Blend;
					varying highp vec4 xlv_TEXCOORD0;
					varying mediump vec3 xlv_TEXCOORD1;
					varying lowp vec4 xlv_COLOR0;
					varying mediump vec3 xlv_TEXCOORD3;
					varying highp float xlv_TEXCOORD4;
					void main ()
					{
					  mediump vec3 tmpvar_1;
					  mediump vec3 tmpvar_2;
					  lowp vec4 c_3;
					  lowp vec3 tmpvar_4;
					  lowp vec3 lightDir_5;
					  highp vec4 tmpvar_6;
					  mediump vec3 tmpvar_7;
					  tmpvar_7 = _WorldSpaceLightPos0.xyz;
					  lightDir_5 = tmpvar_7;
					  tmpvar_6 = xlv_COLOR0;
					  tmpvar_4 = xlv_TEXCOORD1;
					  lowp vec3 tmpvar_8;
					  lowp float vBlend_9;
					  highp vec2 P_10;
					  P_10 = (xlv_TEXCOORD0.xy + (_V_CW_MainTex_Scroll * _Time.x));
					  highp float tmpvar_11;
					  tmpvar_11 = clamp ((_V_CW_SecondaryTex_Blend + tmpvar_6.w), 0.0, 1.0);
					  vBlend_9 = tmpvar_11;
					  highp vec2 P_12;
					  P_12 = (xlv_TEXCOORD0.zw + (_V_CW_SecondaryTex_Scroll * _Time.x));
					  lowp vec4 tmpvar_13;
					  tmpvar_13 = mix (texture2D (_MainTex, P_10), texture2D (_V_CW_SecondaryTex, P_12), vec4(vBlend_9));
					  tmpvar_8 = (tmpvar_13.xyz * _Color.xyz);
					  tmpvar_1 = _LightColor0.xyz;
					  tmpvar_2 = lightDir_5;
					  mediump vec3 normalWorld_14;
					  normalWorld_14 = tmpvar_4;
					  mediump vec3 ambient_15;
					  mediump vec4 tmpvar_16;
					  tmpvar_16.w = 1.0;
					  tmpvar_16.xyz = normalWorld_14;
					  mediump vec3 x_17;
					  x_17.x = dot (unity_SHAr, tmpvar_16);
					  x_17.y = dot (unity_SHAg, tmpvar_16);
					  x_17.z = dot (unity_SHAb, tmpvar_16);
					  ambient_15 = max (((1.055 * 
					    pow (max (vec3(0.0, 0.0, 0.0), (xlv_TEXCOORD3 + x_17)), vec3(0.4166667, 0.4166667, 0.4166667))
					  ) - 0.055), vec3(0.0, 0.0, 0.0));
					  lowp vec4 c_18;
					  lowp vec4 c_19;
					  lowp float diff_20;
					  mediump float tmpvar_21;
					  tmpvar_21 = max (0.0, dot (tmpvar_4, tmpvar_2));
					  diff_20 = tmpvar_21;
					  c_19.xyz = ((tmpvar_8 * tmpvar_1) * diff_20);
					  c_19.w = (tmpvar_13.w * _Color.w);
					  c_18.w = c_19.w;
					  c_18.xyz = (c_19.xyz + (tmpvar_8 * ambient_15));
					  c_3.w = c_18.w;
					  highp float tmpvar_22;
					  tmpvar_22 = clamp (xlv_TEXCOORD4, 0.0, 1.0);
					  c_3.xyz = mix (unity_FogColor.xyz, c_18.xyz, vec3(tmpvar_22));
					  gl_FragData[0] = c_3;
					}
					
					
					#endif"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "FOG_LINEAR" "VERTEXLIGHT_ON" "V_CW_REFLECTIVE_OFF" "V_CW_VERTEX_COLOR_OFF" "_EMISSION_OFF" "V_CW_RIM_OFF" "_NORMALMAP_OFF" "V_CW_SPECULAR_OFF" }
					"!!GLES
					#version 100
					
					#ifdef VERTEX
					attribute vec4 _glesTANGENT;
					attribute vec4 _glesVertex;
					attribute vec4 _glesColor;
					attribute vec3 _glesNormal;
					attribute vec4 _glesMultiTexCoord0;
					uniform highp vec4 unity_4LightPosX0;
					uniform highp vec4 unity_4LightPosY0;
					uniform highp vec4 unity_4LightPosZ0;
					uniform mediump vec4 unity_4LightAtten0;
					uniform mediump vec4 unity_LightColor[8];
					uniform mediump vec4 unity_SHBr;
					uniform mediump vec4 unity_SHBg;
					uniform mediump vec4 unity_SHBb;
					uniform mediump vec4 unity_SHC;
					uniform highp mat4 glstate_matrix_mvp;
					uniform highp mat4 _Object2World;
					uniform highp mat4 _World2Object;
					uniform highp vec4 unity_FogParams;
					uniform highp vec3 _V_CW_Bend;
					uniform highp vec3 _V_CW_Bias;
					uniform highp vec4 _V_CW_PivotPoint_Position;
					uniform highp vec4 _MainTex_ST;
					uniform highp vec4 _V_CW_SecondaryTex_ST;
					varying highp vec4 xlv_TEXCOORD0;
					varying mediump vec3 xlv_TEXCOORD1;
					varying highp vec3 xlv_TEXCOORD2;
					varying lowp vec4 xlv_COLOR0;
					varying mediump vec3 xlv_TEXCOORD3;
					varying highp float xlv_TEXCOORD4;
					void main ()
					{
					  lowp vec3 worldNormal_1;
					  highp vec4 tmpvar_2;
					  highp vec4 tmpvar_3;
					  mediump vec3 tmpvar_4;
					  mediump vec3 tmpvar_5;
					  highp vec4 v_6;
					  highp float tmpvar_7;
					  tmpvar_7 = _World2Object[0].x;
					  v_6.x = tmpvar_7;
					  highp float tmpvar_8;
					  tmpvar_8 = _World2Object[1].x;
					  v_6.y = tmpvar_8;
					  highp float tmpvar_9;
					  tmpvar_9 = _World2Object[2].x;
					  v_6.z = tmpvar_9;
					  highp float tmpvar_10;
					  tmpvar_10 = _World2Object[3].x;
					  v_6.w = tmpvar_10;
					  highp vec4 v_11;
					  highp float tmpvar_12;
					  tmpvar_12 = _World2Object[0].y;
					  v_11.x = tmpvar_12;
					  highp float tmpvar_13;
					  tmpvar_13 = _World2Object[1].y;
					  v_11.y = tmpvar_13;
					  highp float tmpvar_14;
					  tmpvar_14 = _World2Object[2].y;
					  v_11.z = tmpvar_14;
					  highp float tmpvar_15;
					  tmpvar_15 = _World2Object[3].y;
					  v_11.w = tmpvar_15;
					  highp vec4 v_16;
					  highp float tmpvar_17;
					  tmpvar_17 = _World2Object[0].z;
					  v_16.x = tmpvar_17;
					  highp float tmpvar_18;
					  tmpvar_18 = _World2Object[1].z;
					  v_16.y = tmpvar_18;
					  highp float tmpvar_19;
					  tmpvar_19 = _World2Object[2].z;
					  v_16.z = tmpvar_19;
					  highp float tmpvar_20;
					  tmpvar_20 = _World2Object[3].z;
					  v_16.w = tmpvar_20;
					  highp vec3 tmpvar_21;
					  tmpvar_21 = normalize(((
					    (v_6.xyz * _glesNormal.x)
					   + 
					    (v_11.xyz * _glesNormal.y)
					  ) + (v_16.xyz * _glesNormal.z)));
					  highp mat3 tmpvar_22;
					  tmpvar_22[0] = _Object2World[0].xyz;
					  tmpvar_22[1] = _Object2World[1].xyz;
					  tmpvar_22[2] = _Object2World[2].xyz;
					  highp vec3 tmpvar_23;
					  tmpvar_23 = normalize((tmpvar_22 * _glesTANGENT.xyz));
					  highp vec4 vertex_24;
					  vertex_24.w = _glesVertex.w;
					  highp vec2 xzOff_25;
					  highp vec3 v2_26;
					  highp vec3 v1_27;
					  highp vec3 v0_28;
					  highp vec3 tmpvar_29;
					  tmpvar_29 = ((_Object2World * _glesVertex).xyz - _V_CW_PivotPoint_Position.xyz);
					  highp vec3 tmpvar_30;
					  tmpvar_30 = (tmpvar_29 + tmpvar_23);
					  v1_27.xz = tmpvar_30.xz;
					  highp vec3 tmpvar_31;
					  tmpvar_31 = (tmpvar_29 - ((tmpvar_21.yzx * tmpvar_23.zxy) - (tmpvar_21.zxy * tmpvar_23.yzx)));
					  v2_26.xz = tmpvar_31.xz;
					  highp vec2 tmpvar_32;
					  tmpvar_32.x = float((tmpvar_29.z >= 0.0));
					  tmpvar_32.y = float((tmpvar_29.x >= 0.0));
					  xzOff_25 = (max (vec2(0.0, 0.0), (
					    abs(tmpvar_29.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_32 * 2.0) - 1.0));
					  xzOff_25 = (xzOff_25 * xzOff_25);
					  highp vec3 tmpvar_33;
					  tmpvar_33.xz = vec2(0.0, 0.0);
					  tmpvar_33.y = (((_V_CW_Bend.x * xzOff_25.x) + (_V_CW_Bend.z * xzOff_25.y)) * 0.001);
					  v0_28 = (tmpvar_29 + tmpvar_33);
					  highp vec2 tmpvar_34;
					  tmpvar_34.x = float((tmpvar_30.z >= 0.0));
					  tmpvar_34.y = float((tmpvar_30.x >= 0.0));
					  xzOff_25 = (max (vec2(0.0, 0.0), (
					    abs(tmpvar_30.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_34 * 2.0) - 1.0));
					  xzOff_25 = (xzOff_25 * xzOff_25);
					  v1_27.y = (tmpvar_30.y + ((
					    (_V_CW_Bend.x * xzOff_25.x)
					   + 
					    (_V_CW_Bend.z * xzOff_25.y)
					  ) * 0.001));
					  highp vec2 tmpvar_35;
					  tmpvar_35.x = float((tmpvar_31.z >= 0.0));
					  tmpvar_35.y = float((tmpvar_31.x >= 0.0));
					  xzOff_25 = (max (vec2(0.0, 0.0), (
					    abs(tmpvar_31.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_35 * 2.0) - 1.0));
					  xzOff_25 = (xzOff_25 * xzOff_25);
					  v2_26.y = (tmpvar_31.y + ((
					    (_V_CW_Bend.x * xzOff_25.x)
					   + 
					    (_V_CW_Bend.z * xzOff_25.y)
					  ) * 0.001));
					  highp mat3 tmpvar_36;
					  tmpvar_36[0] = _World2Object[0].xyz;
					  tmpvar_36[1] = _World2Object[1].xyz;
					  tmpvar_36[2] = _World2Object[2].xyz;
					  vertex_24.xyz = (_glesVertex.xyz + (tmpvar_36 * tmpvar_33));
					  highp mat3 tmpvar_37;
					  tmpvar_37[0] = _World2Object[0].xyz;
					  tmpvar_37[1] = _World2Object[1].xyz;
					  tmpvar_37[2] = _World2Object[2].xyz;
					  highp vec3 a_38;
					  a_38 = (v2_26 - v0_28);
					  highp vec3 b_39;
					  b_39 = (v1_27 - v0_28);
					  highp vec3 tmpvar_40;
					  tmpvar_40 = normalize((tmpvar_37 * normalize(
					    ((a_38.yzx * b_39.zxy) - (a_38.zxy * b_39.yzx))
					  )));
					  tmpvar_2 = (glstate_matrix_mvp * vertex_24);
					  tmpvar_3.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
					  tmpvar_3.zw = ((_glesMultiTexCoord0.xy * _V_CW_SecondaryTex_ST.xy) + _V_CW_SecondaryTex_ST.zw);
					  highp vec3 tmpvar_41;
					  tmpvar_41 = (_Object2World * vertex_24).xyz;
					  highp vec4 v_42;
					  v_42.x = tmpvar_7;
					  v_42.y = tmpvar_8;
					  v_42.z = tmpvar_9;
					  v_42.w = tmpvar_10;
					  highp vec4 v_43;
					  v_43.x = tmpvar_12;
					  v_43.y = tmpvar_13;
					  v_43.z = tmpvar_14;
					  v_43.w = tmpvar_15;
					  highp vec4 v_44;
					  v_44.x = tmpvar_17;
					  v_44.y = tmpvar_18;
					  v_44.z = tmpvar_19;
					  v_44.w = tmpvar_20;
					  highp vec3 tmpvar_45;
					  tmpvar_45 = normalize(((
					    (v_42.xyz * tmpvar_40.x)
					   + 
					    (v_43.xyz * tmpvar_40.y)
					  ) + (v_44.xyz * tmpvar_40.z)));
					  worldNormal_1 = tmpvar_45;
					  tmpvar_4 = worldNormal_1;
					  highp vec3 lightColor0_46;
					  lightColor0_46 = unity_LightColor[0].xyz;
					  highp vec3 lightColor1_47;
					  lightColor1_47 = unity_LightColor[1].xyz;
					  highp vec3 lightColor2_48;
					  lightColor2_48 = unity_LightColor[2].xyz;
					  highp vec3 lightColor3_49;
					  lightColor3_49 = unity_LightColor[3].xyz;
					  highp vec4 lightAttenSq_50;
					  lightAttenSq_50 = unity_4LightAtten0;
					  highp vec3 normal_51;
					  normal_51 = worldNormal_1;
					  highp vec3 col_52;
					  highp vec4 ndotl_53;
					  highp vec4 lengthSq_54;
					  highp vec4 tmpvar_55;
					  tmpvar_55 = (unity_4LightPosX0 - tmpvar_41.x);
					  highp vec4 tmpvar_56;
					  tmpvar_56 = (unity_4LightPosY0 - tmpvar_41.y);
					  highp vec4 tmpvar_57;
					  tmpvar_57 = (unity_4LightPosZ0 - tmpvar_41.z);
					  lengthSq_54 = (tmpvar_55 * tmpvar_55);
					  lengthSq_54 = (lengthSq_54 + (tmpvar_56 * tmpvar_56));
					  lengthSq_54 = (lengthSq_54 + (tmpvar_57 * tmpvar_57));
					  ndotl_53 = (tmpvar_55 * normal_51.x);
					  ndotl_53 = (ndotl_53 + (tmpvar_56 * normal_51.y));
					  ndotl_53 = (ndotl_53 + (tmpvar_57 * normal_51.z));
					  highp vec4 tmpvar_58;
					  tmpvar_58 = max (vec4(0.0, 0.0, 0.0, 0.0), (ndotl_53 * inversesqrt(lengthSq_54)));
					  ndotl_53 = tmpvar_58;
					  highp vec4 tmpvar_59;
					  tmpvar_59 = (tmpvar_58 * (1.0/((1.0 + 
					    (lengthSq_54 * lightAttenSq_50)
					  ))));
					  col_52 = (lightColor0_46 * tmpvar_59.x);
					  col_52 = (col_52 + (lightColor1_47 * tmpvar_59.y));
					  col_52 = (col_52 + (lightColor2_48 * tmpvar_59.z));
					  col_52 = (col_52 + (lightColor3_49 * tmpvar_59.w));
					  tmpvar_5 = col_52;
					  mediump vec3 normal_60;
					  normal_60 = worldNormal_1;
					  mediump vec3 ambient_61;
					  ambient_61 = (tmpvar_5 * ((tmpvar_5 * 
					    ((tmpvar_5 * 0.305306) + 0.6821711)
					  ) + 0.01252288));
					  mediump vec3 x1_62;
					  mediump vec4 tmpvar_63;
					  tmpvar_63 = (normal_60.xyzz * normal_60.yzzx);
					  x1_62.x = dot (unity_SHBr, tmpvar_63);
					  x1_62.y = dot (unity_SHBg, tmpvar_63);
					  x1_62.z = dot (unity_SHBb, tmpvar_63);
					  ambient_61 = (ambient_61 + (x1_62 + (unity_SHC.xyz * 
					    ((normal_60.x * normal_60.x) - (normal_60.y * normal_60.y))
					  )));
					  tmpvar_5 = ambient_61;
					  gl_Position = tmpvar_2;
					  xlv_TEXCOORD0 = tmpvar_3;
					  xlv_TEXCOORD1 = tmpvar_4;
					  xlv_TEXCOORD2 = tmpvar_41;
					  xlv_COLOR0 = _glesColor;
					  xlv_TEXCOORD3 = ambient_61;
					  xlv_TEXCOORD4 = ((tmpvar_2.z * unity_FogParams.z) + unity_FogParams.w);
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform highp vec4 _Time;
					uniform mediump vec4 _WorldSpaceLightPos0;
					uniform mediump vec4 unity_SHAr;
					uniform mediump vec4 unity_SHAg;
					uniform mediump vec4 unity_SHAb;
					uniform lowp vec4 unity_FogColor;
					uniform lowp vec4 _LightColor0;
					uniform sampler2D _MainTex;
					uniform lowp vec4 _Color;
					uniform lowp vec2 _V_CW_MainTex_Scroll;
					uniform sampler2D _V_CW_SecondaryTex;
					uniform lowp vec2 _V_CW_SecondaryTex_Scroll;
					uniform lowp float _V_CW_SecondaryTex_Blend;
					varying highp vec4 xlv_TEXCOORD0;
					varying mediump vec3 xlv_TEXCOORD1;
					varying lowp vec4 xlv_COLOR0;
					varying mediump vec3 xlv_TEXCOORD3;
					varying highp float xlv_TEXCOORD4;
					void main ()
					{
					  mediump vec3 tmpvar_1;
					  mediump vec3 tmpvar_2;
					  lowp vec4 c_3;
					  lowp vec3 tmpvar_4;
					  lowp vec3 lightDir_5;
					  highp vec4 tmpvar_6;
					  mediump vec3 tmpvar_7;
					  tmpvar_7 = _WorldSpaceLightPos0.xyz;
					  lightDir_5 = tmpvar_7;
					  tmpvar_6 = xlv_COLOR0;
					  tmpvar_4 = xlv_TEXCOORD1;
					  lowp vec3 tmpvar_8;
					  lowp float vBlend_9;
					  highp vec2 P_10;
					  P_10 = (xlv_TEXCOORD0.xy + (_V_CW_MainTex_Scroll * _Time.x));
					  highp float tmpvar_11;
					  tmpvar_11 = clamp ((_V_CW_SecondaryTex_Blend + tmpvar_6.w), 0.0, 1.0);
					  vBlend_9 = tmpvar_11;
					  highp vec2 P_12;
					  P_12 = (xlv_TEXCOORD0.zw + (_V_CW_SecondaryTex_Scroll * _Time.x));
					  lowp vec4 tmpvar_13;
					  tmpvar_13 = mix (texture2D (_MainTex, P_10), texture2D (_V_CW_SecondaryTex, P_12), vec4(vBlend_9));
					  tmpvar_8 = (tmpvar_13.xyz * _Color.xyz);
					  tmpvar_1 = _LightColor0.xyz;
					  tmpvar_2 = lightDir_5;
					  mediump vec3 normalWorld_14;
					  normalWorld_14 = tmpvar_4;
					  mediump vec3 ambient_15;
					  mediump vec4 tmpvar_16;
					  tmpvar_16.w = 1.0;
					  tmpvar_16.xyz = normalWorld_14;
					  mediump vec3 x_17;
					  x_17.x = dot (unity_SHAr, tmpvar_16);
					  x_17.y = dot (unity_SHAg, tmpvar_16);
					  x_17.z = dot (unity_SHAb, tmpvar_16);
					  ambient_15 = max (((1.055 * 
					    pow (max (vec3(0.0, 0.0, 0.0), (xlv_TEXCOORD3 + x_17)), vec3(0.4166667, 0.4166667, 0.4166667))
					  ) - 0.055), vec3(0.0, 0.0, 0.0));
					  lowp vec4 c_18;
					  lowp vec4 c_19;
					  lowp float diff_20;
					  mediump float tmpvar_21;
					  tmpvar_21 = max (0.0, dot (tmpvar_4, tmpvar_2));
					  diff_20 = tmpvar_21;
					  c_19.xyz = ((tmpvar_8 * tmpvar_1) * diff_20);
					  c_19.w = (tmpvar_13.w * _Color.w);
					  c_18.w = c_19.w;
					  c_18.xyz = (c_19.xyz + (tmpvar_8 * ambient_15));
					  c_3.w = c_18.w;
					  highp float tmpvar_22;
					  tmpvar_22 = clamp (xlv_TEXCOORD4, 0.0, 1.0);
					  c_3.xyz = mix (unity_FogColor.xyz, c_18.xyz, vec3(tmpvar_22));
					  gl_FragData[0] = c_3;
					}
					
					
					#endif"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "V_CW_VERTEX_COLOR" "V_CW_REFLECTIVE_OFF" "_EMISSION_OFF" "V_CW_RIM_OFF" "_NORMALMAP_OFF" "V_CW_SPECULAR_OFF" }
					"!!GLES
					#version 100
					
					#ifdef VERTEX
					attribute vec4 _glesTANGENT;
					attribute vec4 _glesVertex;
					attribute vec4 _glesColor;
					attribute vec3 _glesNormal;
					attribute vec4 _glesMultiTexCoord0;
					uniform mediump vec4 unity_SHBr;
					uniform mediump vec4 unity_SHBg;
					uniform mediump vec4 unity_SHBb;
					uniform mediump vec4 unity_SHC;
					uniform highp mat4 glstate_matrix_mvp;
					uniform highp mat4 _Object2World;
					uniform highp mat4 _World2Object;
					uniform highp vec3 _V_CW_Bend;
					uniform highp vec3 _V_CW_Bias;
					uniform highp vec4 _V_CW_PivotPoint_Position;
					uniform highp vec4 _MainTex_ST;
					uniform highp vec4 _V_CW_SecondaryTex_ST;
					varying highp vec4 xlv_TEXCOORD0;
					varying mediump vec3 xlv_TEXCOORD1;
					varying highp vec3 xlv_TEXCOORD2;
					varying lowp vec4 xlv_COLOR0;
					varying mediump vec3 xlv_TEXCOORD3;
					void main ()
					{
					  lowp vec3 worldNormal_1;
					  highp vec4 tmpvar_2;
					  mediump vec3 tmpvar_3;
					  highp vec4 v_4;
					  highp float tmpvar_5;
					  tmpvar_5 = _World2Object[0].x;
					  v_4.x = tmpvar_5;
					  highp float tmpvar_6;
					  tmpvar_6 = _World2Object[1].x;
					  v_4.y = tmpvar_6;
					  highp float tmpvar_7;
					  tmpvar_7 = _World2Object[2].x;
					  v_4.z = tmpvar_7;
					  highp float tmpvar_8;
					  tmpvar_8 = _World2Object[3].x;
					  v_4.w = tmpvar_8;
					  highp vec4 v_9;
					  highp float tmpvar_10;
					  tmpvar_10 = _World2Object[0].y;
					  v_9.x = tmpvar_10;
					  highp float tmpvar_11;
					  tmpvar_11 = _World2Object[1].y;
					  v_9.y = tmpvar_11;
					  highp float tmpvar_12;
					  tmpvar_12 = _World2Object[2].y;
					  v_9.z = tmpvar_12;
					  highp float tmpvar_13;
					  tmpvar_13 = _World2Object[3].y;
					  v_9.w = tmpvar_13;
					  highp vec4 v_14;
					  highp float tmpvar_15;
					  tmpvar_15 = _World2Object[0].z;
					  v_14.x = tmpvar_15;
					  highp float tmpvar_16;
					  tmpvar_16 = _World2Object[1].z;
					  v_14.y = tmpvar_16;
					  highp float tmpvar_17;
					  tmpvar_17 = _World2Object[2].z;
					  v_14.z = tmpvar_17;
					  highp float tmpvar_18;
					  tmpvar_18 = _World2Object[3].z;
					  v_14.w = tmpvar_18;
					  highp vec3 tmpvar_19;
					  tmpvar_19 = normalize(((
					    (v_4.xyz * _glesNormal.x)
					   + 
					    (v_9.xyz * _glesNormal.y)
					  ) + (v_14.xyz * _glesNormal.z)));
					  highp mat3 tmpvar_20;
					  tmpvar_20[0] = _Object2World[0].xyz;
					  tmpvar_20[1] = _Object2World[1].xyz;
					  tmpvar_20[2] = _Object2World[2].xyz;
					  highp vec3 tmpvar_21;
					  tmpvar_21 = normalize((tmpvar_20 * _glesTANGENT.xyz));
					  highp vec4 vertex_22;
					  vertex_22.w = _glesVertex.w;
					  highp vec2 xzOff_23;
					  highp vec3 v2_24;
					  highp vec3 v1_25;
					  highp vec3 v0_26;
					  highp vec3 tmpvar_27;
					  tmpvar_27 = ((_Object2World * _glesVertex).xyz - _V_CW_PivotPoint_Position.xyz);
					  highp vec3 tmpvar_28;
					  tmpvar_28 = (tmpvar_27 + tmpvar_21);
					  v1_25.xz = tmpvar_28.xz;
					  highp vec3 tmpvar_29;
					  tmpvar_29 = (tmpvar_27 - ((tmpvar_19.yzx * tmpvar_21.zxy) - (tmpvar_19.zxy * tmpvar_21.yzx)));
					  v2_24.xz = tmpvar_29.xz;
					  highp vec2 tmpvar_30;
					  tmpvar_30.x = float((tmpvar_27.z >= 0.0));
					  tmpvar_30.y = float((tmpvar_27.x >= 0.0));
					  xzOff_23 = (max (vec2(0.0, 0.0), (
					    abs(tmpvar_27.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_30 * 2.0) - 1.0));
					  xzOff_23 = (xzOff_23 * xzOff_23);
					  highp vec3 tmpvar_31;
					  tmpvar_31.xz = vec2(0.0, 0.0);
					  tmpvar_31.y = (((_V_CW_Bend.x * xzOff_23.x) + (_V_CW_Bend.z * xzOff_23.y)) * 0.001);
					  v0_26 = (tmpvar_27 + tmpvar_31);
					  highp vec2 tmpvar_32;
					  tmpvar_32.x = float((tmpvar_28.z >= 0.0));
					  tmpvar_32.y = float((tmpvar_28.x >= 0.0));
					  xzOff_23 = (max (vec2(0.0, 0.0), (
					    abs(tmpvar_28.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_32 * 2.0) - 1.0));
					  xzOff_23 = (xzOff_23 * xzOff_23);
					  v1_25.y = (tmpvar_28.y + ((
					    (_V_CW_Bend.x * xzOff_23.x)
					   + 
					    (_V_CW_Bend.z * xzOff_23.y)
					  ) * 0.001));
					  highp vec2 tmpvar_33;
					  tmpvar_33.x = float((tmpvar_29.z >= 0.0));
					  tmpvar_33.y = float((tmpvar_29.x >= 0.0));
					  xzOff_23 = (max (vec2(0.0, 0.0), (
					    abs(tmpvar_29.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_33 * 2.0) - 1.0));
					  xzOff_23 = (xzOff_23 * xzOff_23);
					  v2_24.y = (tmpvar_29.y + ((
					    (_V_CW_Bend.x * xzOff_23.x)
					   + 
					    (_V_CW_Bend.z * xzOff_23.y)
					  ) * 0.001));
					  highp mat3 tmpvar_34;
					  tmpvar_34[0] = _World2Object[0].xyz;
					  tmpvar_34[1] = _World2Object[1].xyz;
					  tmpvar_34[2] = _World2Object[2].xyz;
					  vertex_22.xyz = (_glesVertex.xyz + (tmpvar_34 * tmpvar_31));
					  highp mat3 tmpvar_35;
					  tmpvar_35[0] = _World2Object[0].xyz;
					  tmpvar_35[1] = _World2Object[1].xyz;
					  tmpvar_35[2] = _World2Object[2].xyz;
					  highp vec3 a_36;
					  a_36 = (v2_24 - v0_26);
					  highp vec3 b_37;
					  b_37 = (v1_25 - v0_26);
					  highp vec3 tmpvar_38;
					  tmpvar_38 = normalize((tmpvar_35 * normalize(
					    ((a_36.yzx * b_37.zxy) - (a_36.zxy * b_37.yzx))
					  )));
					  tmpvar_2.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
					  tmpvar_2.zw = ((_glesMultiTexCoord0.xy * _V_CW_SecondaryTex_ST.xy) + _V_CW_SecondaryTex_ST.zw);
					  highp vec4 v_39;
					  v_39.x = tmpvar_5;
					  v_39.y = tmpvar_6;
					  v_39.z = tmpvar_7;
					  v_39.w = tmpvar_8;
					  highp vec4 v_40;
					  v_40.x = tmpvar_10;
					  v_40.y = tmpvar_11;
					  v_40.z = tmpvar_12;
					  v_40.w = tmpvar_13;
					  highp vec4 v_41;
					  v_41.x = tmpvar_15;
					  v_41.y = tmpvar_16;
					  v_41.z = tmpvar_17;
					  v_41.w = tmpvar_18;
					  highp vec3 tmpvar_42;
					  tmpvar_42 = normalize(((
					    (v_39.xyz * tmpvar_38.x)
					   + 
					    (v_40.xyz * tmpvar_38.y)
					  ) + (v_41.xyz * tmpvar_38.z)));
					  worldNormal_1 = tmpvar_42;
					  tmpvar_3 = worldNormal_1;
					  mediump vec3 normal_43;
					  normal_43 = worldNormal_1;
					  mediump vec3 x1_44;
					  mediump vec4 tmpvar_45;
					  tmpvar_45 = (normal_43.xyzz * normal_43.yzzx);
					  x1_44.x = dot (unity_SHBr, tmpvar_45);
					  x1_44.y = dot (unity_SHBg, tmpvar_45);
					  x1_44.z = dot (unity_SHBb, tmpvar_45);
					  gl_Position = (glstate_matrix_mvp * vertex_22);
					  xlv_TEXCOORD0 = tmpvar_2;
					  xlv_TEXCOORD1 = tmpvar_3;
					  xlv_TEXCOORD2 = (_Object2World * vertex_22).xyz;
					  xlv_COLOR0 = _glesColor;
					  xlv_TEXCOORD3 = (x1_44 + (unity_SHC.xyz * (
					    (normal_43.x * normal_43.x)
					   - 
					    (normal_43.y * normal_43.y)
					  )));
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform highp vec4 _Time;
					uniform mediump vec4 _WorldSpaceLightPos0;
					uniform mediump vec4 unity_SHAr;
					uniform mediump vec4 unity_SHAg;
					uniform mediump vec4 unity_SHAb;
					uniform lowp vec4 _LightColor0;
					uniform sampler2D _MainTex;
					uniform lowp vec4 _Color;
					uniform lowp vec2 _V_CW_MainTex_Scroll;
					uniform sampler2D _V_CW_SecondaryTex;
					uniform lowp vec2 _V_CW_SecondaryTex_Scroll;
					uniform lowp float _V_CW_SecondaryTex_Blend;
					varying highp vec4 xlv_TEXCOORD0;
					varying mediump vec3 xlv_TEXCOORD1;
					varying lowp vec4 xlv_COLOR0;
					varying mediump vec3 xlv_TEXCOORD3;
					void main ()
					{
					  mediump vec3 tmpvar_1;
					  mediump vec3 tmpvar_2;
					  lowp vec3 tmpvar_3;
					  lowp vec3 lightDir_4;
					  highp vec4 tmpvar_5;
					  mediump vec3 tmpvar_6;
					  tmpvar_6 = _WorldSpaceLightPos0.xyz;
					  lightDir_4 = tmpvar_6;
					  tmpvar_5 = xlv_COLOR0;
					  tmpvar_3 = xlv_TEXCOORD1;
					  lowp vec3 tmpvar_7;
					  lowp float vBlend_8;
					  highp vec2 P_9;
					  P_9 = (xlv_TEXCOORD0.xy + (_V_CW_MainTex_Scroll * _Time.x));
					  highp float tmpvar_10;
					  tmpvar_10 = clamp ((_V_CW_SecondaryTex_Blend + tmpvar_5.w), 0.0, 1.0);
					  vBlend_8 = tmpvar_10;
					  highp vec2 P_11;
					  P_11 = (xlv_TEXCOORD0.zw + (_V_CW_SecondaryTex_Scroll * _Time.x));
					  lowp vec4 tmpvar_12;
					  tmpvar_12 = mix (texture2D (_MainTex, P_9), texture2D (_V_CW_SecondaryTex, P_11), vec4(vBlend_8));
					  tmpvar_7 = (tmpvar_12.xyz * _Color.xyz);
					  tmpvar_7 = (tmpvar_7 * tmpvar_5.xyz);
					  tmpvar_1 = _LightColor0.xyz;
					  tmpvar_2 = lightDir_4;
					  mediump vec3 normalWorld_13;
					  normalWorld_13 = tmpvar_3;
					  mediump vec3 ambient_14;
					  mediump vec4 tmpvar_15;
					  tmpvar_15.w = 1.0;
					  tmpvar_15.xyz = normalWorld_13;
					  mediump vec3 x_16;
					  x_16.x = dot (unity_SHAr, tmpvar_15);
					  x_16.y = dot (unity_SHAg, tmpvar_15);
					  x_16.z = dot (unity_SHAb, tmpvar_15);
					  ambient_14 = max (((1.055 * 
					    pow (max (vec3(0.0, 0.0, 0.0), (xlv_TEXCOORD3 + x_16)), vec3(0.4166667, 0.4166667, 0.4166667))
					  ) - 0.055), vec3(0.0, 0.0, 0.0));
					  lowp vec4 c_17;
					  lowp vec4 c_18;
					  lowp float diff_19;
					  mediump float tmpvar_20;
					  tmpvar_20 = max (0.0, dot (tmpvar_3, tmpvar_2));
					  diff_19 = tmpvar_20;
					  c_18.xyz = ((tmpvar_7 * tmpvar_1) * diff_19);
					  c_18.w = (tmpvar_12.w * _Color.w);
					  c_17.w = c_18.w;
					  c_17.xyz = (c_18.xyz + (tmpvar_7 * ambient_14));
					  gl_FragData[0] = c_17;
					}
					
					
					#endif"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "VERTEXLIGHT_ON" "V_CW_VERTEX_COLOR" "V_CW_REFLECTIVE_OFF" "_EMISSION_OFF" "V_CW_RIM_OFF" "_NORMALMAP_OFF" "V_CW_SPECULAR_OFF" }
					"!!GLES
					#version 100
					
					#ifdef VERTEX
					attribute vec4 _glesTANGENT;
					attribute vec4 _glesVertex;
					attribute vec4 _glesColor;
					attribute vec3 _glesNormal;
					attribute vec4 _glesMultiTexCoord0;
					uniform highp vec4 unity_4LightPosX0;
					uniform highp vec4 unity_4LightPosY0;
					uniform highp vec4 unity_4LightPosZ0;
					uniform mediump vec4 unity_4LightAtten0;
					uniform mediump vec4 unity_LightColor[8];
					uniform mediump vec4 unity_SHBr;
					uniform mediump vec4 unity_SHBg;
					uniform mediump vec4 unity_SHBb;
					uniform mediump vec4 unity_SHC;
					uniform highp mat4 glstate_matrix_mvp;
					uniform highp mat4 _Object2World;
					uniform highp mat4 _World2Object;
					uniform highp vec3 _V_CW_Bend;
					uniform highp vec3 _V_CW_Bias;
					uniform highp vec4 _V_CW_PivotPoint_Position;
					uniform highp vec4 _MainTex_ST;
					uniform highp vec4 _V_CW_SecondaryTex_ST;
					varying highp vec4 xlv_TEXCOORD0;
					varying mediump vec3 xlv_TEXCOORD1;
					varying highp vec3 xlv_TEXCOORD2;
					varying lowp vec4 xlv_COLOR0;
					varying mediump vec3 xlv_TEXCOORD3;
					void main ()
					{
					  lowp vec3 worldNormal_1;
					  highp vec4 tmpvar_2;
					  mediump vec3 tmpvar_3;
					  mediump vec3 tmpvar_4;
					  highp vec4 v_5;
					  highp float tmpvar_6;
					  tmpvar_6 = _World2Object[0].x;
					  v_5.x = tmpvar_6;
					  highp float tmpvar_7;
					  tmpvar_7 = _World2Object[1].x;
					  v_5.y = tmpvar_7;
					  highp float tmpvar_8;
					  tmpvar_8 = _World2Object[2].x;
					  v_5.z = tmpvar_8;
					  highp float tmpvar_9;
					  tmpvar_9 = _World2Object[3].x;
					  v_5.w = tmpvar_9;
					  highp vec4 v_10;
					  highp float tmpvar_11;
					  tmpvar_11 = _World2Object[0].y;
					  v_10.x = tmpvar_11;
					  highp float tmpvar_12;
					  tmpvar_12 = _World2Object[1].y;
					  v_10.y = tmpvar_12;
					  highp float tmpvar_13;
					  tmpvar_13 = _World2Object[2].y;
					  v_10.z = tmpvar_13;
					  highp float tmpvar_14;
					  tmpvar_14 = _World2Object[3].y;
					  v_10.w = tmpvar_14;
					  highp vec4 v_15;
					  highp float tmpvar_16;
					  tmpvar_16 = _World2Object[0].z;
					  v_15.x = tmpvar_16;
					  highp float tmpvar_17;
					  tmpvar_17 = _World2Object[1].z;
					  v_15.y = tmpvar_17;
					  highp float tmpvar_18;
					  tmpvar_18 = _World2Object[2].z;
					  v_15.z = tmpvar_18;
					  highp float tmpvar_19;
					  tmpvar_19 = _World2Object[3].z;
					  v_15.w = tmpvar_19;
					  highp vec3 tmpvar_20;
					  tmpvar_20 = normalize(((
					    (v_5.xyz * _glesNormal.x)
					   + 
					    (v_10.xyz * _glesNormal.y)
					  ) + (v_15.xyz * _glesNormal.z)));
					  highp mat3 tmpvar_21;
					  tmpvar_21[0] = _Object2World[0].xyz;
					  tmpvar_21[1] = _Object2World[1].xyz;
					  tmpvar_21[2] = _Object2World[2].xyz;
					  highp vec3 tmpvar_22;
					  tmpvar_22 = normalize((tmpvar_21 * _glesTANGENT.xyz));
					  highp vec4 vertex_23;
					  vertex_23.w = _glesVertex.w;
					  highp vec2 xzOff_24;
					  highp vec3 v2_25;
					  highp vec3 v1_26;
					  highp vec3 v0_27;
					  highp vec3 tmpvar_28;
					  tmpvar_28 = ((_Object2World * _glesVertex).xyz - _V_CW_PivotPoint_Position.xyz);
					  highp vec3 tmpvar_29;
					  tmpvar_29 = (tmpvar_28 + tmpvar_22);
					  v1_26.xz = tmpvar_29.xz;
					  highp vec3 tmpvar_30;
					  tmpvar_30 = (tmpvar_28 - ((tmpvar_20.yzx * tmpvar_22.zxy) - (tmpvar_20.zxy * tmpvar_22.yzx)));
					  v2_25.xz = tmpvar_30.xz;
					  highp vec2 tmpvar_31;
					  tmpvar_31.x = float((tmpvar_28.z >= 0.0));
					  tmpvar_31.y = float((tmpvar_28.x >= 0.0));
					  xzOff_24 = (max (vec2(0.0, 0.0), (
					    abs(tmpvar_28.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_31 * 2.0) - 1.0));
					  xzOff_24 = (xzOff_24 * xzOff_24);
					  highp vec3 tmpvar_32;
					  tmpvar_32.xz = vec2(0.0, 0.0);
					  tmpvar_32.y = (((_V_CW_Bend.x * xzOff_24.x) + (_V_CW_Bend.z * xzOff_24.y)) * 0.001);
					  v0_27 = (tmpvar_28 + tmpvar_32);
					  highp vec2 tmpvar_33;
					  tmpvar_33.x = float((tmpvar_29.z >= 0.0));
					  tmpvar_33.y = float((tmpvar_29.x >= 0.0));
					  xzOff_24 = (max (vec2(0.0, 0.0), (
					    abs(tmpvar_29.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_33 * 2.0) - 1.0));
					  xzOff_24 = (xzOff_24 * xzOff_24);
					  v1_26.y = (tmpvar_29.y + ((
					    (_V_CW_Bend.x * xzOff_24.x)
					   + 
					    (_V_CW_Bend.z * xzOff_24.y)
					  ) * 0.001));
					  highp vec2 tmpvar_34;
					  tmpvar_34.x = float((tmpvar_30.z >= 0.0));
					  tmpvar_34.y = float((tmpvar_30.x >= 0.0));
					  xzOff_24 = (max (vec2(0.0, 0.0), (
					    abs(tmpvar_30.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_34 * 2.0) - 1.0));
					  xzOff_24 = (xzOff_24 * xzOff_24);
					  v2_25.y = (tmpvar_30.y + ((
					    (_V_CW_Bend.x * xzOff_24.x)
					   + 
					    (_V_CW_Bend.z * xzOff_24.y)
					  ) * 0.001));
					  highp mat3 tmpvar_35;
					  tmpvar_35[0] = _World2Object[0].xyz;
					  tmpvar_35[1] = _World2Object[1].xyz;
					  tmpvar_35[2] = _World2Object[2].xyz;
					  vertex_23.xyz = (_glesVertex.xyz + (tmpvar_35 * tmpvar_32));
					  highp mat3 tmpvar_36;
					  tmpvar_36[0] = _World2Object[0].xyz;
					  tmpvar_36[1] = _World2Object[1].xyz;
					  tmpvar_36[2] = _World2Object[2].xyz;
					  highp vec3 a_37;
					  a_37 = (v2_25 - v0_27);
					  highp vec3 b_38;
					  b_38 = (v1_26 - v0_27);
					  highp vec3 tmpvar_39;
					  tmpvar_39 = normalize((tmpvar_36 * normalize(
					    ((a_37.yzx * b_38.zxy) - (a_37.zxy * b_38.yzx))
					  )));
					  tmpvar_2.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
					  tmpvar_2.zw = ((_glesMultiTexCoord0.xy * _V_CW_SecondaryTex_ST.xy) + _V_CW_SecondaryTex_ST.zw);
					  highp vec3 tmpvar_40;
					  tmpvar_40 = (_Object2World * vertex_23).xyz;
					  highp vec4 v_41;
					  v_41.x = tmpvar_6;
					  v_41.y = tmpvar_7;
					  v_41.z = tmpvar_8;
					  v_41.w = tmpvar_9;
					  highp vec4 v_42;
					  v_42.x = tmpvar_11;
					  v_42.y = tmpvar_12;
					  v_42.z = tmpvar_13;
					  v_42.w = tmpvar_14;
					  highp vec4 v_43;
					  v_43.x = tmpvar_16;
					  v_43.y = tmpvar_17;
					  v_43.z = tmpvar_18;
					  v_43.w = tmpvar_19;
					  highp vec3 tmpvar_44;
					  tmpvar_44 = normalize(((
					    (v_41.xyz * tmpvar_39.x)
					   + 
					    (v_42.xyz * tmpvar_39.y)
					  ) + (v_43.xyz * tmpvar_39.z)));
					  worldNormal_1 = tmpvar_44;
					  tmpvar_3 = worldNormal_1;
					  highp vec3 lightColor0_45;
					  lightColor0_45 = unity_LightColor[0].xyz;
					  highp vec3 lightColor1_46;
					  lightColor1_46 = unity_LightColor[1].xyz;
					  highp vec3 lightColor2_47;
					  lightColor2_47 = unity_LightColor[2].xyz;
					  highp vec3 lightColor3_48;
					  lightColor3_48 = unity_LightColor[3].xyz;
					  highp vec4 lightAttenSq_49;
					  lightAttenSq_49 = unity_4LightAtten0;
					  highp vec3 normal_50;
					  normal_50 = worldNormal_1;
					  highp vec3 col_51;
					  highp vec4 ndotl_52;
					  highp vec4 lengthSq_53;
					  highp vec4 tmpvar_54;
					  tmpvar_54 = (unity_4LightPosX0 - tmpvar_40.x);
					  highp vec4 tmpvar_55;
					  tmpvar_55 = (unity_4LightPosY0 - tmpvar_40.y);
					  highp vec4 tmpvar_56;
					  tmpvar_56 = (unity_4LightPosZ0 - tmpvar_40.z);
					  lengthSq_53 = (tmpvar_54 * tmpvar_54);
					  lengthSq_53 = (lengthSq_53 + (tmpvar_55 * tmpvar_55));
					  lengthSq_53 = (lengthSq_53 + (tmpvar_56 * tmpvar_56));
					  ndotl_52 = (tmpvar_54 * normal_50.x);
					  ndotl_52 = (ndotl_52 + (tmpvar_55 * normal_50.y));
					  ndotl_52 = (ndotl_52 + (tmpvar_56 * normal_50.z));
					  highp vec4 tmpvar_57;
					  tmpvar_57 = max (vec4(0.0, 0.0, 0.0, 0.0), (ndotl_52 * inversesqrt(lengthSq_53)));
					  ndotl_52 = tmpvar_57;
					  highp vec4 tmpvar_58;
					  tmpvar_58 = (tmpvar_57 * (1.0/((1.0 + 
					    (lengthSq_53 * lightAttenSq_49)
					  ))));
					  col_51 = (lightColor0_45 * tmpvar_58.x);
					  col_51 = (col_51 + (lightColor1_46 * tmpvar_58.y));
					  col_51 = (col_51 + (lightColor2_47 * tmpvar_58.z));
					  col_51 = (col_51 + (lightColor3_48 * tmpvar_58.w));
					  tmpvar_4 = col_51;
					  mediump vec3 normal_59;
					  normal_59 = worldNormal_1;
					  mediump vec3 ambient_60;
					  ambient_60 = (tmpvar_4 * ((tmpvar_4 * 
					    ((tmpvar_4 * 0.305306) + 0.6821711)
					  ) + 0.01252288));
					  mediump vec3 x1_61;
					  mediump vec4 tmpvar_62;
					  tmpvar_62 = (normal_59.xyzz * normal_59.yzzx);
					  x1_61.x = dot (unity_SHBr, tmpvar_62);
					  x1_61.y = dot (unity_SHBg, tmpvar_62);
					  x1_61.z = dot (unity_SHBb, tmpvar_62);
					  ambient_60 = (ambient_60 + (x1_61 + (unity_SHC.xyz * 
					    ((normal_59.x * normal_59.x) - (normal_59.y * normal_59.y))
					  )));
					  tmpvar_4 = ambient_60;
					  gl_Position = (glstate_matrix_mvp * vertex_23);
					  xlv_TEXCOORD0 = tmpvar_2;
					  xlv_TEXCOORD1 = tmpvar_3;
					  xlv_TEXCOORD2 = tmpvar_40;
					  xlv_COLOR0 = _glesColor;
					  xlv_TEXCOORD3 = ambient_60;
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform highp vec4 _Time;
					uniform mediump vec4 _WorldSpaceLightPos0;
					uniform mediump vec4 unity_SHAr;
					uniform mediump vec4 unity_SHAg;
					uniform mediump vec4 unity_SHAb;
					uniform lowp vec4 _LightColor0;
					uniform sampler2D _MainTex;
					uniform lowp vec4 _Color;
					uniform lowp vec2 _V_CW_MainTex_Scroll;
					uniform sampler2D _V_CW_SecondaryTex;
					uniform lowp vec2 _V_CW_SecondaryTex_Scroll;
					uniform lowp float _V_CW_SecondaryTex_Blend;
					varying highp vec4 xlv_TEXCOORD0;
					varying mediump vec3 xlv_TEXCOORD1;
					varying lowp vec4 xlv_COLOR0;
					varying mediump vec3 xlv_TEXCOORD3;
					void main ()
					{
					  mediump vec3 tmpvar_1;
					  mediump vec3 tmpvar_2;
					  lowp vec3 tmpvar_3;
					  lowp vec3 lightDir_4;
					  highp vec4 tmpvar_5;
					  mediump vec3 tmpvar_6;
					  tmpvar_6 = _WorldSpaceLightPos0.xyz;
					  lightDir_4 = tmpvar_6;
					  tmpvar_5 = xlv_COLOR0;
					  tmpvar_3 = xlv_TEXCOORD1;
					  lowp vec3 tmpvar_7;
					  lowp float vBlend_8;
					  highp vec2 P_9;
					  P_9 = (xlv_TEXCOORD0.xy + (_V_CW_MainTex_Scroll * _Time.x));
					  highp float tmpvar_10;
					  tmpvar_10 = clamp ((_V_CW_SecondaryTex_Blend + tmpvar_5.w), 0.0, 1.0);
					  vBlend_8 = tmpvar_10;
					  highp vec2 P_11;
					  P_11 = (xlv_TEXCOORD0.zw + (_V_CW_SecondaryTex_Scroll * _Time.x));
					  lowp vec4 tmpvar_12;
					  tmpvar_12 = mix (texture2D (_MainTex, P_9), texture2D (_V_CW_SecondaryTex, P_11), vec4(vBlend_8));
					  tmpvar_7 = (tmpvar_12.xyz * _Color.xyz);
					  tmpvar_7 = (tmpvar_7 * tmpvar_5.xyz);
					  tmpvar_1 = _LightColor0.xyz;
					  tmpvar_2 = lightDir_4;
					  mediump vec3 normalWorld_13;
					  normalWorld_13 = tmpvar_3;
					  mediump vec3 ambient_14;
					  mediump vec4 tmpvar_15;
					  tmpvar_15.w = 1.0;
					  tmpvar_15.xyz = normalWorld_13;
					  mediump vec3 x_16;
					  x_16.x = dot (unity_SHAr, tmpvar_15);
					  x_16.y = dot (unity_SHAg, tmpvar_15);
					  x_16.z = dot (unity_SHAb, tmpvar_15);
					  ambient_14 = max (((1.055 * 
					    pow (max (vec3(0.0, 0.0, 0.0), (xlv_TEXCOORD3 + x_16)), vec3(0.4166667, 0.4166667, 0.4166667))
					  ) - 0.055), vec3(0.0, 0.0, 0.0));
					  lowp vec4 c_17;
					  lowp vec4 c_18;
					  lowp float diff_19;
					  mediump float tmpvar_20;
					  tmpvar_20 = max (0.0, dot (tmpvar_3, tmpvar_2));
					  diff_19 = tmpvar_20;
					  c_18.xyz = ((tmpvar_7 * tmpvar_1) * diff_19);
					  c_18.w = (tmpvar_12.w * _Color.w);
					  c_17.w = c_18.w;
					  c_17.xyz = (c_18.xyz + (tmpvar_7 * ambient_14));
					  gl_FragData[0] = c_17;
					}
					
					
					#endif"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "FOG_LINEAR" "V_CW_VERTEX_COLOR" "V_CW_REFLECTIVE_OFF" "_EMISSION_OFF" "V_CW_RIM_OFF" "_NORMALMAP_OFF" "V_CW_SPECULAR_OFF" }
					"!!GLES
					#version 100
					
					#ifdef VERTEX
					attribute vec4 _glesTANGENT;
					attribute vec4 _glesVertex;
					attribute vec4 _glesColor;
					attribute vec3 _glesNormal;
					attribute vec4 _glesMultiTexCoord0;
					uniform mediump vec4 unity_SHBr;
					uniform mediump vec4 unity_SHBg;
					uniform mediump vec4 unity_SHBb;
					uniform mediump vec4 unity_SHC;
					uniform highp mat4 glstate_matrix_mvp;
					uniform highp mat4 _Object2World;
					uniform highp mat4 _World2Object;
					uniform highp vec4 unity_FogParams;
					uniform highp vec3 _V_CW_Bend;
					uniform highp vec3 _V_CW_Bias;
					uniform highp vec4 _V_CW_PivotPoint_Position;
					uniform highp vec4 _MainTex_ST;
					uniform highp vec4 _V_CW_SecondaryTex_ST;
					varying highp vec4 xlv_TEXCOORD0;
					varying mediump vec3 xlv_TEXCOORD1;
					varying highp vec3 xlv_TEXCOORD2;
					varying lowp vec4 xlv_COLOR0;
					varying mediump vec3 xlv_TEXCOORD3;
					varying highp float xlv_TEXCOORD4;
					void main ()
					{
					  lowp vec3 worldNormal_1;
					  highp vec4 tmpvar_2;
					  highp vec4 tmpvar_3;
					  mediump vec3 tmpvar_4;
					  highp vec4 v_5;
					  highp float tmpvar_6;
					  tmpvar_6 = _World2Object[0].x;
					  v_5.x = tmpvar_6;
					  highp float tmpvar_7;
					  tmpvar_7 = _World2Object[1].x;
					  v_5.y = tmpvar_7;
					  highp float tmpvar_8;
					  tmpvar_8 = _World2Object[2].x;
					  v_5.z = tmpvar_8;
					  highp float tmpvar_9;
					  tmpvar_9 = _World2Object[3].x;
					  v_5.w = tmpvar_9;
					  highp vec4 v_10;
					  highp float tmpvar_11;
					  tmpvar_11 = _World2Object[0].y;
					  v_10.x = tmpvar_11;
					  highp float tmpvar_12;
					  tmpvar_12 = _World2Object[1].y;
					  v_10.y = tmpvar_12;
					  highp float tmpvar_13;
					  tmpvar_13 = _World2Object[2].y;
					  v_10.z = tmpvar_13;
					  highp float tmpvar_14;
					  tmpvar_14 = _World2Object[3].y;
					  v_10.w = tmpvar_14;
					  highp vec4 v_15;
					  highp float tmpvar_16;
					  tmpvar_16 = _World2Object[0].z;
					  v_15.x = tmpvar_16;
					  highp float tmpvar_17;
					  tmpvar_17 = _World2Object[1].z;
					  v_15.y = tmpvar_17;
					  highp float tmpvar_18;
					  tmpvar_18 = _World2Object[2].z;
					  v_15.z = tmpvar_18;
					  highp float tmpvar_19;
					  tmpvar_19 = _World2Object[3].z;
					  v_15.w = tmpvar_19;
					  highp vec3 tmpvar_20;
					  tmpvar_20 = normalize(((
					    (v_5.xyz * _glesNormal.x)
					   + 
					    (v_10.xyz * _glesNormal.y)
					  ) + (v_15.xyz * _glesNormal.z)));
					  highp mat3 tmpvar_21;
					  tmpvar_21[0] = _Object2World[0].xyz;
					  tmpvar_21[1] = _Object2World[1].xyz;
					  tmpvar_21[2] = _Object2World[2].xyz;
					  highp vec3 tmpvar_22;
					  tmpvar_22 = normalize((tmpvar_21 * _glesTANGENT.xyz));
					  highp vec4 vertex_23;
					  vertex_23.w = _glesVertex.w;
					  highp vec2 xzOff_24;
					  highp vec3 v2_25;
					  highp vec3 v1_26;
					  highp vec3 v0_27;
					  highp vec3 tmpvar_28;
					  tmpvar_28 = ((_Object2World * _glesVertex).xyz - _V_CW_PivotPoint_Position.xyz);
					  highp vec3 tmpvar_29;
					  tmpvar_29 = (tmpvar_28 + tmpvar_22);
					  v1_26.xz = tmpvar_29.xz;
					  highp vec3 tmpvar_30;
					  tmpvar_30 = (tmpvar_28 - ((tmpvar_20.yzx * tmpvar_22.zxy) - (tmpvar_20.zxy * tmpvar_22.yzx)));
					  v2_25.xz = tmpvar_30.xz;
					  highp vec2 tmpvar_31;
					  tmpvar_31.x = float((tmpvar_28.z >= 0.0));
					  tmpvar_31.y = float((tmpvar_28.x >= 0.0));
					  xzOff_24 = (max (vec2(0.0, 0.0), (
					    abs(tmpvar_28.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_31 * 2.0) - 1.0));
					  xzOff_24 = (xzOff_24 * xzOff_24);
					  highp vec3 tmpvar_32;
					  tmpvar_32.xz = vec2(0.0, 0.0);
					  tmpvar_32.y = (((_V_CW_Bend.x * xzOff_24.x) + (_V_CW_Bend.z * xzOff_24.y)) * 0.001);
					  v0_27 = (tmpvar_28 + tmpvar_32);
					  highp vec2 tmpvar_33;
					  tmpvar_33.x = float((tmpvar_29.z >= 0.0));
					  tmpvar_33.y = float((tmpvar_29.x >= 0.0));
					  xzOff_24 = (max (vec2(0.0, 0.0), (
					    abs(tmpvar_29.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_33 * 2.0) - 1.0));
					  xzOff_24 = (xzOff_24 * xzOff_24);
					  v1_26.y = (tmpvar_29.y + ((
					    (_V_CW_Bend.x * xzOff_24.x)
					   + 
					    (_V_CW_Bend.z * xzOff_24.y)
					  ) * 0.001));
					  highp vec2 tmpvar_34;
					  tmpvar_34.x = float((tmpvar_30.z >= 0.0));
					  tmpvar_34.y = float((tmpvar_30.x >= 0.0));
					  xzOff_24 = (max (vec2(0.0, 0.0), (
					    abs(tmpvar_30.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_34 * 2.0) - 1.0));
					  xzOff_24 = (xzOff_24 * xzOff_24);
					  v2_25.y = (tmpvar_30.y + ((
					    (_V_CW_Bend.x * xzOff_24.x)
					   + 
					    (_V_CW_Bend.z * xzOff_24.y)
					  ) * 0.001));
					  highp mat3 tmpvar_35;
					  tmpvar_35[0] = _World2Object[0].xyz;
					  tmpvar_35[1] = _World2Object[1].xyz;
					  tmpvar_35[2] = _World2Object[2].xyz;
					  vertex_23.xyz = (_glesVertex.xyz + (tmpvar_35 * tmpvar_32));
					  highp mat3 tmpvar_36;
					  tmpvar_36[0] = _World2Object[0].xyz;
					  tmpvar_36[1] = _World2Object[1].xyz;
					  tmpvar_36[2] = _World2Object[2].xyz;
					  highp vec3 a_37;
					  a_37 = (v2_25 - v0_27);
					  highp vec3 b_38;
					  b_38 = (v1_26 - v0_27);
					  highp vec3 tmpvar_39;
					  tmpvar_39 = normalize((tmpvar_36 * normalize(
					    ((a_37.yzx * b_38.zxy) - (a_37.zxy * b_38.yzx))
					  )));
					  tmpvar_2 = (glstate_matrix_mvp * vertex_23);
					  tmpvar_3.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
					  tmpvar_3.zw = ((_glesMultiTexCoord0.xy * _V_CW_SecondaryTex_ST.xy) + _V_CW_SecondaryTex_ST.zw);
					  highp vec4 v_40;
					  v_40.x = tmpvar_6;
					  v_40.y = tmpvar_7;
					  v_40.z = tmpvar_8;
					  v_40.w = tmpvar_9;
					  highp vec4 v_41;
					  v_41.x = tmpvar_11;
					  v_41.y = tmpvar_12;
					  v_41.z = tmpvar_13;
					  v_41.w = tmpvar_14;
					  highp vec4 v_42;
					  v_42.x = tmpvar_16;
					  v_42.y = tmpvar_17;
					  v_42.z = tmpvar_18;
					  v_42.w = tmpvar_19;
					  highp vec3 tmpvar_43;
					  tmpvar_43 = normalize(((
					    (v_40.xyz * tmpvar_39.x)
					   + 
					    (v_41.xyz * tmpvar_39.y)
					  ) + (v_42.xyz * tmpvar_39.z)));
					  worldNormal_1 = tmpvar_43;
					  tmpvar_4 = worldNormal_1;
					  mediump vec3 normal_44;
					  normal_44 = worldNormal_1;
					  mediump vec3 x1_45;
					  mediump vec4 tmpvar_46;
					  tmpvar_46 = (normal_44.xyzz * normal_44.yzzx);
					  x1_45.x = dot (unity_SHBr, tmpvar_46);
					  x1_45.y = dot (unity_SHBg, tmpvar_46);
					  x1_45.z = dot (unity_SHBb, tmpvar_46);
					  gl_Position = tmpvar_2;
					  xlv_TEXCOORD0 = tmpvar_3;
					  xlv_TEXCOORD1 = tmpvar_4;
					  xlv_TEXCOORD2 = (_Object2World * vertex_23).xyz;
					  xlv_COLOR0 = _glesColor;
					  xlv_TEXCOORD3 = (x1_45 + (unity_SHC.xyz * (
					    (normal_44.x * normal_44.x)
					   - 
					    (normal_44.y * normal_44.y)
					  )));
					  xlv_TEXCOORD4 = ((tmpvar_2.z * unity_FogParams.z) + unity_FogParams.w);
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform highp vec4 _Time;
					uniform mediump vec4 _WorldSpaceLightPos0;
					uniform mediump vec4 unity_SHAr;
					uniform mediump vec4 unity_SHAg;
					uniform mediump vec4 unity_SHAb;
					uniform lowp vec4 unity_FogColor;
					uniform lowp vec4 _LightColor0;
					uniform sampler2D _MainTex;
					uniform lowp vec4 _Color;
					uniform lowp vec2 _V_CW_MainTex_Scroll;
					uniform sampler2D _V_CW_SecondaryTex;
					uniform lowp vec2 _V_CW_SecondaryTex_Scroll;
					uniform lowp float _V_CW_SecondaryTex_Blend;
					varying highp vec4 xlv_TEXCOORD0;
					varying mediump vec3 xlv_TEXCOORD1;
					varying lowp vec4 xlv_COLOR0;
					varying mediump vec3 xlv_TEXCOORD3;
					varying highp float xlv_TEXCOORD4;
					void main ()
					{
					  mediump vec3 tmpvar_1;
					  mediump vec3 tmpvar_2;
					  lowp vec4 c_3;
					  lowp vec3 tmpvar_4;
					  lowp vec3 lightDir_5;
					  highp vec4 tmpvar_6;
					  mediump vec3 tmpvar_7;
					  tmpvar_7 = _WorldSpaceLightPos0.xyz;
					  lightDir_5 = tmpvar_7;
					  tmpvar_6 = xlv_COLOR0;
					  tmpvar_4 = xlv_TEXCOORD1;
					  lowp vec3 tmpvar_8;
					  lowp float vBlend_9;
					  highp vec2 P_10;
					  P_10 = (xlv_TEXCOORD0.xy + (_V_CW_MainTex_Scroll * _Time.x));
					  highp float tmpvar_11;
					  tmpvar_11 = clamp ((_V_CW_SecondaryTex_Blend + tmpvar_6.w), 0.0, 1.0);
					  vBlend_9 = tmpvar_11;
					  highp vec2 P_12;
					  P_12 = (xlv_TEXCOORD0.zw + (_V_CW_SecondaryTex_Scroll * _Time.x));
					  lowp vec4 tmpvar_13;
					  tmpvar_13 = mix (texture2D (_MainTex, P_10), texture2D (_V_CW_SecondaryTex, P_12), vec4(vBlend_9));
					  tmpvar_8 = (tmpvar_13.xyz * _Color.xyz);
					  tmpvar_8 = (tmpvar_8 * tmpvar_6.xyz);
					  tmpvar_1 = _LightColor0.xyz;
					  tmpvar_2 = lightDir_5;
					  mediump vec3 normalWorld_14;
					  normalWorld_14 = tmpvar_4;
					  mediump vec3 ambient_15;
					  mediump vec4 tmpvar_16;
					  tmpvar_16.w = 1.0;
					  tmpvar_16.xyz = normalWorld_14;
					  mediump vec3 x_17;
					  x_17.x = dot (unity_SHAr, tmpvar_16);
					  x_17.y = dot (unity_SHAg, tmpvar_16);
					  x_17.z = dot (unity_SHAb, tmpvar_16);
					  ambient_15 = max (((1.055 * 
					    pow (max (vec3(0.0, 0.0, 0.0), (xlv_TEXCOORD3 + x_17)), vec3(0.4166667, 0.4166667, 0.4166667))
					  ) - 0.055), vec3(0.0, 0.0, 0.0));
					  lowp vec4 c_18;
					  lowp vec4 c_19;
					  lowp float diff_20;
					  mediump float tmpvar_21;
					  tmpvar_21 = max (0.0, dot (tmpvar_4, tmpvar_2));
					  diff_20 = tmpvar_21;
					  c_19.xyz = ((tmpvar_8 * tmpvar_1) * diff_20);
					  c_19.w = (tmpvar_13.w * _Color.w);
					  c_18.w = c_19.w;
					  c_18.xyz = (c_19.xyz + (tmpvar_8 * ambient_15));
					  c_3.w = c_18.w;
					  highp float tmpvar_22;
					  tmpvar_22 = clamp (xlv_TEXCOORD4, 0.0, 1.0);
					  c_3.xyz = mix (unity_FogColor.xyz, c_18.xyz, vec3(tmpvar_22));
					  gl_FragData[0] = c_3;
					}
					
					
					#endif"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "FOG_LINEAR" "VERTEXLIGHT_ON" "V_CW_VERTEX_COLOR" "V_CW_REFLECTIVE_OFF" "_EMISSION_OFF" "V_CW_RIM_OFF" "_NORMALMAP_OFF" "V_CW_SPECULAR_OFF" }
					"!!GLES
					#version 100
					
					#ifdef VERTEX
					attribute vec4 _glesTANGENT;
					attribute vec4 _glesVertex;
					attribute vec4 _glesColor;
					attribute vec3 _glesNormal;
					attribute vec4 _glesMultiTexCoord0;
					uniform highp vec4 unity_4LightPosX0;
					uniform highp vec4 unity_4LightPosY0;
					uniform highp vec4 unity_4LightPosZ0;
					uniform mediump vec4 unity_4LightAtten0;
					uniform mediump vec4 unity_LightColor[8];
					uniform mediump vec4 unity_SHBr;
					uniform mediump vec4 unity_SHBg;
					uniform mediump vec4 unity_SHBb;
					uniform mediump vec4 unity_SHC;
					uniform highp mat4 glstate_matrix_mvp;
					uniform highp mat4 _Object2World;
					uniform highp mat4 _World2Object;
					uniform highp vec4 unity_FogParams;
					uniform highp vec3 _V_CW_Bend;
					uniform highp vec3 _V_CW_Bias;
					uniform highp vec4 _V_CW_PivotPoint_Position;
					uniform highp vec4 _MainTex_ST;
					uniform highp vec4 _V_CW_SecondaryTex_ST;
					varying highp vec4 xlv_TEXCOORD0;
					varying mediump vec3 xlv_TEXCOORD1;
					varying highp vec3 xlv_TEXCOORD2;
					varying lowp vec4 xlv_COLOR0;
					varying mediump vec3 xlv_TEXCOORD3;
					varying highp float xlv_TEXCOORD4;
					void main ()
					{
					  lowp vec3 worldNormal_1;
					  highp vec4 tmpvar_2;
					  highp vec4 tmpvar_3;
					  mediump vec3 tmpvar_4;
					  mediump vec3 tmpvar_5;
					  highp vec4 v_6;
					  highp float tmpvar_7;
					  tmpvar_7 = _World2Object[0].x;
					  v_6.x = tmpvar_7;
					  highp float tmpvar_8;
					  tmpvar_8 = _World2Object[1].x;
					  v_6.y = tmpvar_8;
					  highp float tmpvar_9;
					  tmpvar_9 = _World2Object[2].x;
					  v_6.z = tmpvar_9;
					  highp float tmpvar_10;
					  tmpvar_10 = _World2Object[3].x;
					  v_6.w = tmpvar_10;
					  highp vec4 v_11;
					  highp float tmpvar_12;
					  tmpvar_12 = _World2Object[0].y;
					  v_11.x = tmpvar_12;
					  highp float tmpvar_13;
					  tmpvar_13 = _World2Object[1].y;
					  v_11.y = tmpvar_13;
					  highp float tmpvar_14;
					  tmpvar_14 = _World2Object[2].y;
					  v_11.z = tmpvar_14;
					  highp float tmpvar_15;
					  tmpvar_15 = _World2Object[3].y;
					  v_11.w = tmpvar_15;
					  highp vec4 v_16;
					  highp float tmpvar_17;
					  tmpvar_17 = _World2Object[0].z;
					  v_16.x = tmpvar_17;
					  highp float tmpvar_18;
					  tmpvar_18 = _World2Object[1].z;
					  v_16.y = tmpvar_18;
					  highp float tmpvar_19;
					  tmpvar_19 = _World2Object[2].z;
					  v_16.z = tmpvar_19;
					  highp float tmpvar_20;
					  tmpvar_20 = _World2Object[3].z;
					  v_16.w = tmpvar_20;
					  highp vec3 tmpvar_21;
					  tmpvar_21 = normalize(((
					    (v_6.xyz * _glesNormal.x)
					   + 
					    (v_11.xyz * _glesNormal.y)
					  ) + (v_16.xyz * _glesNormal.z)));
					  highp mat3 tmpvar_22;
					  tmpvar_22[0] = _Object2World[0].xyz;
					  tmpvar_22[1] = _Object2World[1].xyz;
					  tmpvar_22[2] = _Object2World[2].xyz;
					  highp vec3 tmpvar_23;
					  tmpvar_23 = normalize((tmpvar_22 * _glesTANGENT.xyz));
					  highp vec4 vertex_24;
					  vertex_24.w = _glesVertex.w;
					  highp vec2 xzOff_25;
					  highp vec3 v2_26;
					  highp vec3 v1_27;
					  highp vec3 v0_28;
					  highp vec3 tmpvar_29;
					  tmpvar_29 = ((_Object2World * _glesVertex).xyz - _V_CW_PivotPoint_Position.xyz);
					  highp vec3 tmpvar_30;
					  tmpvar_30 = (tmpvar_29 + tmpvar_23);
					  v1_27.xz = tmpvar_30.xz;
					  highp vec3 tmpvar_31;
					  tmpvar_31 = (tmpvar_29 - ((tmpvar_21.yzx * tmpvar_23.zxy) - (tmpvar_21.zxy * tmpvar_23.yzx)));
					  v2_26.xz = tmpvar_31.xz;
					  highp vec2 tmpvar_32;
					  tmpvar_32.x = float((tmpvar_29.z >= 0.0));
					  tmpvar_32.y = float((tmpvar_29.x >= 0.0));
					  xzOff_25 = (max (vec2(0.0, 0.0), (
					    abs(tmpvar_29.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_32 * 2.0) - 1.0));
					  xzOff_25 = (xzOff_25 * xzOff_25);
					  highp vec3 tmpvar_33;
					  tmpvar_33.xz = vec2(0.0, 0.0);
					  tmpvar_33.y = (((_V_CW_Bend.x * xzOff_25.x) + (_V_CW_Bend.z * xzOff_25.y)) * 0.001);
					  v0_28 = (tmpvar_29 + tmpvar_33);
					  highp vec2 tmpvar_34;
					  tmpvar_34.x = float((tmpvar_30.z >= 0.0));
					  tmpvar_34.y = float((tmpvar_30.x >= 0.0));
					  xzOff_25 = (max (vec2(0.0, 0.0), (
					    abs(tmpvar_30.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_34 * 2.0) - 1.0));
					  xzOff_25 = (xzOff_25 * xzOff_25);
					  v1_27.y = (tmpvar_30.y + ((
					    (_V_CW_Bend.x * xzOff_25.x)
					   + 
					    (_V_CW_Bend.z * xzOff_25.y)
					  ) * 0.001));
					  highp vec2 tmpvar_35;
					  tmpvar_35.x = float((tmpvar_31.z >= 0.0));
					  tmpvar_35.y = float((tmpvar_31.x >= 0.0));
					  xzOff_25 = (max (vec2(0.0, 0.0), (
					    abs(tmpvar_31.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_35 * 2.0) - 1.0));
					  xzOff_25 = (xzOff_25 * xzOff_25);
					  v2_26.y = (tmpvar_31.y + ((
					    (_V_CW_Bend.x * xzOff_25.x)
					   + 
					    (_V_CW_Bend.z * xzOff_25.y)
					  ) * 0.001));
					  highp mat3 tmpvar_36;
					  tmpvar_36[0] = _World2Object[0].xyz;
					  tmpvar_36[1] = _World2Object[1].xyz;
					  tmpvar_36[2] = _World2Object[2].xyz;
					  vertex_24.xyz = (_glesVertex.xyz + (tmpvar_36 * tmpvar_33));
					  highp mat3 tmpvar_37;
					  tmpvar_37[0] = _World2Object[0].xyz;
					  tmpvar_37[1] = _World2Object[1].xyz;
					  tmpvar_37[2] = _World2Object[2].xyz;
					  highp vec3 a_38;
					  a_38 = (v2_26 - v0_28);
					  highp vec3 b_39;
					  b_39 = (v1_27 - v0_28);
					  highp vec3 tmpvar_40;
					  tmpvar_40 = normalize((tmpvar_37 * normalize(
					    ((a_38.yzx * b_39.zxy) - (a_38.zxy * b_39.yzx))
					  )));
					  tmpvar_2 = (glstate_matrix_mvp * vertex_24);
					  tmpvar_3.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
					  tmpvar_3.zw = ((_glesMultiTexCoord0.xy * _V_CW_SecondaryTex_ST.xy) + _V_CW_SecondaryTex_ST.zw);
					  highp vec3 tmpvar_41;
					  tmpvar_41 = (_Object2World * vertex_24).xyz;
					  highp vec4 v_42;
					  v_42.x = tmpvar_7;
					  v_42.y = tmpvar_8;
					  v_42.z = tmpvar_9;
					  v_42.w = tmpvar_10;
					  highp vec4 v_43;
					  v_43.x = tmpvar_12;
					  v_43.y = tmpvar_13;
					  v_43.z = tmpvar_14;
					  v_43.w = tmpvar_15;
					  highp vec4 v_44;
					  v_44.x = tmpvar_17;
					  v_44.y = tmpvar_18;
					  v_44.z = tmpvar_19;
					  v_44.w = tmpvar_20;
					  highp vec3 tmpvar_45;
					  tmpvar_45 = normalize(((
					    (v_42.xyz * tmpvar_40.x)
					   + 
					    (v_43.xyz * tmpvar_40.y)
					  ) + (v_44.xyz * tmpvar_40.z)));
					  worldNormal_1 = tmpvar_45;
					  tmpvar_4 = worldNormal_1;
					  highp vec3 lightColor0_46;
					  lightColor0_46 = unity_LightColor[0].xyz;
					  highp vec3 lightColor1_47;
					  lightColor1_47 = unity_LightColor[1].xyz;
					  highp vec3 lightColor2_48;
					  lightColor2_48 = unity_LightColor[2].xyz;
					  highp vec3 lightColor3_49;
					  lightColor3_49 = unity_LightColor[3].xyz;
					  highp vec4 lightAttenSq_50;
					  lightAttenSq_50 = unity_4LightAtten0;
					  highp vec3 normal_51;
					  normal_51 = worldNormal_1;
					  highp vec3 col_52;
					  highp vec4 ndotl_53;
					  highp vec4 lengthSq_54;
					  highp vec4 tmpvar_55;
					  tmpvar_55 = (unity_4LightPosX0 - tmpvar_41.x);
					  highp vec4 tmpvar_56;
					  tmpvar_56 = (unity_4LightPosY0 - tmpvar_41.y);
					  highp vec4 tmpvar_57;
					  tmpvar_57 = (unity_4LightPosZ0 - tmpvar_41.z);
					  lengthSq_54 = (tmpvar_55 * tmpvar_55);
					  lengthSq_54 = (lengthSq_54 + (tmpvar_56 * tmpvar_56));
					  lengthSq_54 = (lengthSq_54 + (tmpvar_57 * tmpvar_57));
					  ndotl_53 = (tmpvar_55 * normal_51.x);
					  ndotl_53 = (ndotl_53 + (tmpvar_56 * normal_51.y));
					  ndotl_53 = (ndotl_53 + (tmpvar_57 * normal_51.z));
					  highp vec4 tmpvar_58;
					  tmpvar_58 = max (vec4(0.0, 0.0, 0.0, 0.0), (ndotl_53 * inversesqrt(lengthSq_54)));
					  ndotl_53 = tmpvar_58;
					  highp vec4 tmpvar_59;
					  tmpvar_59 = (tmpvar_58 * (1.0/((1.0 + 
					    (lengthSq_54 * lightAttenSq_50)
					  ))));
					  col_52 = (lightColor0_46 * tmpvar_59.x);
					  col_52 = (col_52 + (lightColor1_47 * tmpvar_59.y));
					  col_52 = (col_52 + (lightColor2_48 * tmpvar_59.z));
					  col_52 = (col_52 + (lightColor3_49 * tmpvar_59.w));
					  tmpvar_5 = col_52;
					  mediump vec3 normal_60;
					  normal_60 = worldNormal_1;
					  mediump vec3 ambient_61;
					  ambient_61 = (tmpvar_5 * ((tmpvar_5 * 
					    ((tmpvar_5 * 0.305306) + 0.6821711)
					  ) + 0.01252288));
					  mediump vec3 x1_62;
					  mediump vec4 tmpvar_63;
					  tmpvar_63 = (normal_60.xyzz * normal_60.yzzx);
					  x1_62.x = dot (unity_SHBr, tmpvar_63);
					  x1_62.y = dot (unity_SHBg, tmpvar_63);
					  x1_62.z = dot (unity_SHBb, tmpvar_63);
					  ambient_61 = (ambient_61 + (x1_62 + (unity_SHC.xyz * 
					    ((normal_60.x * normal_60.x) - (normal_60.y * normal_60.y))
					  )));
					  tmpvar_5 = ambient_61;
					  gl_Position = tmpvar_2;
					  xlv_TEXCOORD0 = tmpvar_3;
					  xlv_TEXCOORD1 = tmpvar_4;
					  xlv_TEXCOORD2 = tmpvar_41;
					  xlv_COLOR0 = _glesColor;
					  xlv_TEXCOORD3 = ambient_61;
					  xlv_TEXCOORD4 = ((tmpvar_2.z * unity_FogParams.z) + unity_FogParams.w);
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform highp vec4 _Time;
					uniform mediump vec4 _WorldSpaceLightPos0;
					uniform mediump vec4 unity_SHAr;
					uniform mediump vec4 unity_SHAg;
					uniform mediump vec4 unity_SHAb;
					uniform lowp vec4 unity_FogColor;
					uniform lowp vec4 _LightColor0;
					uniform sampler2D _MainTex;
					uniform lowp vec4 _Color;
					uniform lowp vec2 _V_CW_MainTex_Scroll;
					uniform sampler2D _V_CW_SecondaryTex;
					uniform lowp vec2 _V_CW_SecondaryTex_Scroll;
					uniform lowp float _V_CW_SecondaryTex_Blend;
					varying highp vec4 xlv_TEXCOORD0;
					varying mediump vec3 xlv_TEXCOORD1;
					varying lowp vec4 xlv_COLOR0;
					varying mediump vec3 xlv_TEXCOORD3;
					varying highp float xlv_TEXCOORD4;
					void main ()
					{
					  mediump vec3 tmpvar_1;
					  mediump vec3 tmpvar_2;
					  lowp vec4 c_3;
					  lowp vec3 tmpvar_4;
					  lowp vec3 lightDir_5;
					  highp vec4 tmpvar_6;
					  mediump vec3 tmpvar_7;
					  tmpvar_7 = _WorldSpaceLightPos0.xyz;
					  lightDir_5 = tmpvar_7;
					  tmpvar_6 = xlv_COLOR0;
					  tmpvar_4 = xlv_TEXCOORD1;
					  lowp vec3 tmpvar_8;
					  lowp float vBlend_9;
					  highp vec2 P_10;
					  P_10 = (xlv_TEXCOORD0.xy + (_V_CW_MainTex_Scroll * _Time.x));
					  highp float tmpvar_11;
					  tmpvar_11 = clamp ((_V_CW_SecondaryTex_Blend + tmpvar_6.w), 0.0, 1.0);
					  vBlend_9 = tmpvar_11;
					  highp vec2 P_12;
					  P_12 = (xlv_TEXCOORD0.zw + (_V_CW_SecondaryTex_Scroll * _Time.x));
					  lowp vec4 tmpvar_13;
					  tmpvar_13 = mix (texture2D (_MainTex, P_10), texture2D (_V_CW_SecondaryTex, P_12), vec4(vBlend_9));
					  tmpvar_8 = (tmpvar_13.xyz * _Color.xyz);
					  tmpvar_8 = (tmpvar_8 * tmpvar_6.xyz);
					  tmpvar_1 = _LightColor0.xyz;
					  tmpvar_2 = lightDir_5;
					  mediump vec3 normalWorld_14;
					  normalWorld_14 = tmpvar_4;
					  mediump vec3 ambient_15;
					  mediump vec4 tmpvar_16;
					  tmpvar_16.w = 1.0;
					  tmpvar_16.xyz = normalWorld_14;
					  mediump vec3 x_17;
					  x_17.x = dot (unity_SHAr, tmpvar_16);
					  x_17.y = dot (unity_SHAg, tmpvar_16);
					  x_17.z = dot (unity_SHAb, tmpvar_16);
					  ambient_15 = max (((1.055 * 
					    pow (max (vec3(0.0, 0.0, 0.0), (xlv_TEXCOORD3 + x_17)), vec3(0.4166667, 0.4166667, 0.4166667))
					  ) - 0.055), vec3(0.0, 0.0, 0.0));
					  lowp vec4 c_18;
					  lowp vec4 c_19;
					  lowp float diff_20;
					  mediump float tmpvar_21;
					  tmpvar_21 = max (0.0, dot (tmpvar_4, tmpvar_2));
					  diff_20 = tmpvar_21;
					  c_19.xyz = ((tmpvar_8 * tmpvar_1) * diff_20);
					  c_19.w = (tmpvar_13.w * _Color.w);
					  c_18.w = c_19.w;
					  c_18.xyz = (c_19.xyz + (tmpvar_8 * ambient_15));
					  c_3.w = c_18.w;
					  highp float tmpvar_22;
					  tmpvar_22 = clamp (xlv_TEXCOORD4, 0.0, 1.0);
					  c_3.xyz = mix (unity_FogColor.xyz, c_18.xyz, vec3(tmpvar_22));
					  gl_FragData[0] = c_3;
					}
					
					
					#endif"
}
}
Program "fp" {
SubProgram "gles " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "V_CW_REFLECTIVE_OFF" "V_CW_VERTEX_COLOR_OFF" "_EMISSION_OFF" "V_CW_RIM_OFF" "_NORMALMAP_OFF" "V_CW_SPECULAR_OFF" }
					"!!GLES"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "FOG_LINEAR" "V_CW_REFLECTIVE_OFF" "V_CW_VERTEX_COLOR_OFF" "_EMISSION_OFF" "V_CW_RIM_OFF" "_NORMALMAP_OFF" "V_CW_SPECULAR_OFF" }
					"!!GLES"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "V_CW_VERTEX_COLOR" "V_CW_REFLECTIVE_OFF" "_EMISSION_OFF" "V_CW_RIM_OFF" "_NORMALMAP_OFF" "V_CW_SPECULAR_OFF" }
					"!!GLES"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "FOG_LINEAR" "V_CW_VERTEX_COLOR" "V_CW_REFLECTIVE_OFF" "_EMISSION_OFF" "V_CW_RIM_OFF" "_NORMALMAP_OFF" "V_CW_SPECULAR_OFF" }
					"!!GLES"
}
}
 }
 Pass {
  Name "FORWARD"
  Tags { "LIGHTMODE"="ForwardAdd" "QUEUE"="Transparent" "IGNOREPROJECTOR"="true" "RenderType"="Transparent" "CurvedWorldTag"="Legacy Shader/Transparent/Vertex Alpha" "CurvedWorldNoneRemoveableKeywords"="" "CurvedWorldAvailableOptions"="V_CW_REFLECTIVE;V_CW_VERTEX_COLOR;_EMISSION;V_CW_RIM;_NORMALMAP;V_CW_SPECULAR_HD;" }
  ZWrite Off
  Blend SrcAlpha One
  ColorMask RGB
  GpuProgramID 78962
Program "vp" {
SubProgram "gles " {
Keywords { "POINT" "V_CW_REFLECTIVE_OFF" "V_CW_VERTEX_COLOR_OFF" "_EMISSION_OFF" "V_CW_RIM_OFF" "_NORMALMAP_OFF" "V_CW_SPECULAR_OFF" }
					"!!GLES
					#version 100
					
					#ifdef VERTEX
					attribute vec4 _glesTANGENT;
					attribute vec4 _glesVertex;
					attribute vec4 _glesColor;
					attribute vec3 _glesNormal;
					attribute vec4 _glesMultiTexCoord0;
					uniform highp mat4 glstate_matrix_mvp;
					uniform highp mat4 _Object2World;
					uniform highp mat4 _World2Object;
					uniform highp vec3 _V_CW_Bend;
					uniform highp vec3 _V_CW_Bias;
					uniform highp vec4 _V_CW_PivotPoint_Position;
					uniform highp vec4 _MainTex_ST;
					uniform highp vec4 _V_CW_SecondaryTex_ST;
					varying highp vec4 xlv_TEXCOORD0;
					varying mediump vec3 xlv_TEXCOORD1;
					varying highp vec3 xlv_TEXCOORD2;
					varying lowp vec4 xlv_COLOR0;
					void main ()
					{
					  lowp vec3 worldNormal_1;
					  highp vec4 tmpvar_2;
					  mediump vec3 tmpvar_3;
					  highp vec4 v_4;
					  highp float tmpvar_5;
					  tmpvar_5 = _World2Object[0].x;
					  v_4.x = tmpvar_5;
					  highp float tmpvar_6;
					  tmpvar_6 = _World2Object[1].x;
					  v_4.y = tmpvar_6;
					  highp float tmpvar_7;
					  tmpvar_7 = _World2Object[2].x;
					  v_4.z = tmpvar_7;
					  highp float tmpvar_8;
					  tmpvar_8 = _World2Object[3].x;
					  v_4.w = tmpvar_8;
					  highp vec4 v_9;
					  highp float tmpvar_10;
					  tmpvar_10 = _World2Object[0].y;
					  v_9.x = tmpvar_10;
					  highp float tmpvar_11;
					  tmpvar_11 = _World2Object[1].y;
					  v_9.y = tmpvar_11;
					  highp float tmpvar_12;
					  tmpvar_12 = _World2Object[2].y;
					  v_9.z = tmpvar_12;
					  highp float tmpvar_13;
					  tmpvar_13 = _World2Object[3].y;
					  v_9.w = tmpvar_13;
					  highp vec4 v_14;
					  highp float tmpvar_15;
					  tmpvar_15 = _World2Object[0].z;
					  v_14.x = tmpvar_15;
					  highp float tmpvar_16;
					  tmpvar_16 = _World2Object[1].z;
					  v_14.y = tmpvar_16;
					  highp float tmpvar_17;
					  tmpvar_17 = _World2Object[2].z;
					  v_14.z = tmpvar_17;
					  highp float tmpvar_18;
					  tmpvar_18 = _World2Object[3].z;
					  v_14.w = tmpvar_18;
					  highp vec3 tmpvar_19;
					  tmpvar_19 = normalize(((
					    (v_4.xyz * _glesNormal.x)
					   + 
					    (v_9.xyz * _glesNormal.y)
					  ) + (v_14.xyz * _glesNormal.z)));
					  highp mat3 tmpvar_20;
					  tmpvar_20[0] = _Object2World[0].xyz;
					  tmpvar_20[1] = _Object2World[1].xyz;
					  tmpvar_20[2] = _Object2World[2].xyz;
					  highp vec3 tmpvar_21;
					  tmpvar_21 = normalize((tmpvar_20 * _glesTANGENT.xyz));
					  highp vec4 vertex_22;
					  vertex_22.w = _glesVertex.w;
					  highp vec2 xzOff_23;
					  highp vec3 v2_24;
					  highp vec3 v1_25;
					  highp vec3 v0_26;
					  highp vec3 tmpvar_27;
					  tmpvar_27 = ((_Object2World * _glesVertex).xyz - _V_CW_PivotPoint_Position.xyz);
					  highp vec3 tmpvar_28;
					  tmpvar_28 = (tmpvar_27 + tmpvar_21);
					  v1_25.xz = tmpvar_28.xz;
					  highp vec3 tmpvar_29;
					  tmpvar_29 = (tmpvar_27 - ((tmpvar_19.yzx * tmpvar_21.zxy) - (tmpvar_19.zxy * tmpvar_21.yzx)));
					  v2_24.xz = tmpvar_29.xz;
					  highp vec2 tmpvar_30;
					  tmpvar_30.x = float((tmpvar_27.z >= 0.0));
					  tmpvar_30.y = float((tmpvar_27.x >= 0.0));
					  xzOff_23 = (max (vec2(0.0, 0.0), (
					    abs(tmpvar_27.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_30 * 2.0) - 1.0));
					  xzOff_23 = (xzOff_23 * xzOff_23);
					  highp vec3 tmpvar_31;
					  tmpvar_31.xz = vec2(0.0, 0.0);
					  tmpvar_31.y = (((_V_CW_Bend.x * xzOff_23.x) + (_V_CW_Bend.z * xzOff_23.y)) * 0.001);
					  v0_26 = (tmpvar_27 + tmpvar_31);
					  highp vec2 tmpvar_32;
					  tmpvar_32.x = float((tmpvar_28.z >= 0.0));
					  tmpvar_32.y = float((tmpvar_28.x >= 0.0));
					  xzOff_23 = (max (vec2(0.0, 0.0), (
					    abs(tmpvar_28.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_32 * 2.0) - 1.0));
					  xzOff_23 = (xzOff_23 * xzOff_23);
					  v1_25.y = (tmpvar_28.y + ((
					    (_V_CW_Bend.x * xzOff_23.x)
					   + 
					    (_V_CW_Bend.z * xzOff_23.y)
					  ) * 0.001));
					  highp vec2 tmpvar_33;
					  tmpvar_33.x = float((tmpvar_29.z >= 0.0));
					  tmpvar_33.y = float((tmpvar_29.x >= 0.0));
					  xzOff_23 = (max (vec2(0.0, 0.0), (
					    abs(tmpvar_29.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_33 * 2.0) - 1.0));
					  xzOff_23 = (xzOff_23 * xzOff_23);
					  v2_24.y = (tmpvar_29.y + ((
					    (_V_CW_Bend.x * xzOff_23.x)
					   + 
					    (_V_CW_Bend.z * xzOff_23.y)
					  ) * 0.001));
					  highp mat3 tmpvar_34;
					  tmpvar_34[0] = _World2Object[0].xyz;
					  tmpvar_34[1] = _World2Object[1].xyz;
					  tmpvar_34[2] = _World2Object[2].xyz;
					  vertex_22.xyz = (_glesVertex.xyz + (tmpvar_34 * tmpvar_31));
					  highp mat3 tmpvar_35;
					  tmpvar_35[0] = _World2Object[0].xyz;
					  tmpvar_35[1] = _World2Object[1].xyz;
					  tmpvar_35[2] = _World2Object[2].xyz;
					  highp vec3 a_36;
					  a_36 = (v2_24 - v0_26);
					  highp vec3 b_37;
					  b_37 = (v1_25 - v0_26);
					  highp vec3 tmpvar_38;
					  tmpvar_38 = normalize((tmpvar_35 * normalize(
					    ((a_36.yzx * b_37.zxy) - (a_36.zxy * b_37.yzx))
					  )));
					  tmpvar_2.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
					  tmpvar_2.zw = ((_glesMultiTexCoord0.xy * _V_CW_SecondaryTex_ST.xy) + _V_CW_SecondaryTex_ST.zw);
					  highp vec4 v_39;
					  v_39.x = tmpvar_5;
					  v_39.y = tmpvar_6;
					  v_39.z = tmpvar_7;
					  v_39.w = tmpvar_8;
					  highp vec4 v_40;
					  v_40.x = tmpvar_10;
					  v_40.y = tmpvar_11;
					  v_40.z = tmpvar_12;
					  v_40.w = tmpvar_13;
					  highp vec4 v_41;
					  v_41.x = tmpvar_15;
					  v_41.y = tmpvar_16;
					  v_41.z = tmpvar_17;
					  v_41.w = tmpvar_18;
					  highp vec3 tmpvar_42;
					  tmpvar_42 = normalize(((
					    (v_39.xyz * tmpvar_38.x)
					   + 
					    (v_40.xyz * tmpvar_38.y)
					  ) + (v_41.xyz * tmpvar_38.z)));
					  worldNormal_1 = tmpvar_42;
					  tmpvar_3 = worldNormal_1;
					  gl_Position = (glstate_matrix_mvp * vertex_22);
					  xlv_TEXCOORD0 = tmpvar_2;
					  xlv_TEXCOORD1 = tmpvar_3;
					  xlv_TEXCOORD2 = (_Object2World * vertex_22).xyz;
					  xlv_COLOR0 = _glesColor;
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform highp vec4 _Time;
					uniform highp vec4 _WorldSpaceLightPos0;
					uniform lowp vec4 _LightColor0;
					uniform sampler2D _LightTexture0;
					uniform mediump mat4 _LightMatrix0;
					uniform sampler2D _MainTex;
					uniform lowp vec4 _Color;
					uniform lowp vec2 _V_CW_MainTex_Scroll;
					uniform sampler2D _V_CW_SecondaryTex;
					uniform lowp vec2 _V_CW_SecondaryTex_Scroll;
					uniform lowp float _V_CW_SecondaryTex_Blend;
					varying highp vec4 xlv_TEXCOORD0;
					varying mediump vec3 xlv_TEXCOORD1;
					varying highp vec3 xlv_TEXCOORD2;
					varying lowp vec4 xlv_COLOR0;
					void main ()
					{
					  mediump vec3 tmpvar_1;
					  mediump vec3 tmpvar_2;
					  lowp vec3 tmpvar_3;
					  lowp vec3 lightDir_4;
					  highp vec4 tmpvar_5;
					  highp vec3 tmpvar_6;
					  tmpvar_6 = normalize((_WorldSpaceLightPos0.xyz - xlv_TEXCOORD2));
					  lightDir_4 = tmpvar_6;
					  tmpvar_5 = xlv_COLOR0;
					  tmpvar_3 = xlv_TEXCOORD1;
					  lowp float vBlend_7;
					  highp vec2 P_8;
					  P_8 = (xlv_TEXCOORD0.xy + (_V_CW_MainTex_Scroll * _Time.x));
					  highp float tmpvar_9;
					  tmpvar_9 = clamp ((_V_CW_SecondaryTex_Blend + tmpvar_5.w), 0.0, 1.0);
					  vBlend_7 = tmpvar_9;
					  highp vec2 P_10;
					  P_10 = (xlv_TEXCOORD0.zw + (_V_CW_SecondaryTex_Scroll * _Time.x));
					  lowp vec4 tmpvar_11;
					  tmpvar_11 = mix (texture2D (_MainTex, P_8), texture2D (_V_CW_SecondaryTex, P_10), vec4(vBlend_7));
					  highp vec4 tmpvar_12;
					  tmpvar_12.w = 1.0;
					  tmpvar_12.xyz = xlv_TEXCOORD2;
					  highp vec3 tmpvar_13;
					  tmpvar_13 = (_LightMatrix0 * tmpvar_12).xyz;
					  highp float tmpvar_14;
					  tmpvar_14 = dot (tmpvar_13, tmpvar_13);
					  lowp float tmpvar_15;
					  tmpvar_15 = texture2D (_LightTexture0, vec2(tmpvar_14)).w;
					  tmpvar_1 = _LightColor0.xyz;
					  tmpvar_2 = lightDir_4;
					  tmpvar_1 = (tmpvar_1 * tmpvar_15);
					  lowp vec4 c_16;
					  lowp vec4 c_17;
					  lowp float diff_18;
					  mediump float tmpvar_19;
					  tmpvar_19 = max (0.0, dot (tmpvar_3, tmpvar_2));
					  diff_18 = tmpvar_19;
					  c_17.xyz = ((tmpvar_11.xyz * _Color.xyz) * (tmpvar_1 * diff_18));
					  c_17.w = (tmpvar_11.w * _Color.w);
					  c_16.w = c_17.w;
					  c_16.xyz = c_17.xyz;
					  gl_FragData[0] = c_16;
					}
					
					
					#endif"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "V_CW_REFLECTIVE_OFF" "V_CW_VERTEX_COLOR_OFF" "_EMISSION_OFF" "V_CW_RIM_OFF" "_NORMALMAP_OFF" "V_CW_SPECULAR_OFF" }
					"!!GLES
					#version 100
					
					#ifdef VERTEX
					attribute vec4 _glesTANGENT;
					attribute vec4 _glesVertex;
					attribute vec4 _glesColor;
					attribute vec3 _glesNormal;
					attribute vec4 _glesMultiTexCoord0;
					uniform highp mat4 glstate_matrix_mvp;
					uniform highp mat4 _Object2World;
					uniform highp mat4 _World2Object;
					uniform highp vec3 _V_CW_Bend;
					uniform highp vec3 _V_CW_Bias;
					uniform highp vec4 _V_CW_PivotPoint_Position;
					uniform highp vec4 _MainTex_ST;
					uniform highp vec4 _V_CW_SecondaryTex_ST;
					varying highp vec4 xlv_TEXCOORD0;
					varying mediump vec3 xlv_TEXCOORD1;
					varying highp vec3 xlv_TEXCOORD2;
					varying lowp vec4 xlv_COLOR0;
					void main ()
					{
					  lowp vec3 worldNormal_1;
					  highp vec4 tmpvar_2;
					  mediump vec3 tmpvar_3;
					  highp vec4 v_4;
					  highp float tmpvar_5;
					  tmpvar_5 = _World2Object[0].x;
					  v_4.x = tmpvar_5;
					  highp float tmpvar_6;
					  tmpvar_6 = _World2Object[1].x;
					  v_4.y = tmpvar_6;
					  highp float tmpvar_7;
					  tmpvar_7 = _World2Object[2].x;
					  v_4.z = tmpvar_7;
					  highp float tmpvar_8;
					  tmpvar_8 = _World2Object[3].x;
					  v_4.w = tmpvar_8;
					  highp vec4 v_9;
					  highp float tmpvar_10;
					  tmpvar_10 = _World2Object[0].y;
					  v_9.x = tmpvar_10;
					  highp float tmpvar_11;
					  tmpvar_11 = _World2Object[1].y;
					  v_9.y = tmpvar_11;
					  highp float tmpvar_12;
					  tmpvar_12 = _World2Object[2].y;
					  v_9.z = tmpvar_12;
					  highp float tmpvar_13;
					  tmpvar_13 = _World2Object[3].y;
					  v_9.w = tmpvar_13;
					  highp vec4 v_14;
					  highp float tmpvar_15;
					  tmpvar_15 = _World2Object[0].z;
					  v_14.x = tmpvar_15;
					  highp float tmpvar_16;
					  tmpvar_16 = _World2Object[1].z;
					  v_14.y = tmpvar_16;
					  highp float tmpvar_17;
					  tmpvar_17 = _World2Object[2].z;
					  v_14.z = tmpvar_17;
					  highp float tmpvar_18;
					  tmpvar_18 = _World2Object[3].z;
					  v_14.w = tmpvar_18;
					  highp vec3 tmpvar_19;
					  tmpvar_19 = normalize(((
					    (v_4.xyz * _glesNormal.x)
					   + 
					    (v_9.xyz * _glesNormal.y)
					  ) + (v_14.xyz * _glesNormal.z)));
					  highp mat3 tmpvar_20;
					  tmpvar_20[0] = _Object2World[0].xyz;
					  tmpvar_20[1] = _Object2World[1].xyz;
					  tmpvar_20[2] = _Object2World[2].xyz;
					  highp vec3 tmpvar_21;
					  tmpvar_21 = normalize((tmpvar_20 * _glesTANGENT.xyz));
					  highp vec4 vertex_22;
					  vertex_22.w = _glesVertex.w;
					  highp vec2 xzOff_23;
					  highp vec3 v2_24;
					  highp vec3 v1_25;
					  highp vec3 v0_26;
					  highp vec3 tmpvar_27;
					  tmpvar_27 = ((_Object2World * _glesVertex).xyz - _V_CW_PivotPoint_Position.xyz);
					  highp vec3 tmpvar_28;
					  tmpvar_28 = (tmpvar_27 + tmpvar_21);
					  v1_25.xz = tmpvar_28.xz;
					  highp vec3 tmpvar_29;
					  tmpvar_29 = (tmpvar_27 - ((tmpvar_19.yzx * tmpvar_21.zxy) - (tmpvar_19.zxy * tmpvar_21.yzx)));
					  v2_24.xz = tmpvar_29.xz;
					  highp vec2 tmpvar_30;
					  tmpvar_30.x = float((tmpvar_27.z >= 0.0));
					  tmpvar_30.y = float((tmpvar_27.x >= 0.0));
					  xzOff_23 = (max (vec2(0.0, 0.0), (
					    abs(tmpvar_27.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_30 * 2.0) - 1.0));
					  xzOff_23 = (xzOff_23 * xzOff_23);
					  highp vec3 tmpvar_31;
					  tmpvar_31.xz = vec2(0.0, 0.0);
					  tmpvar_31.y = (((_V_CW_Bend.x * xzOff_23.x) + (_V_CW_Bend.z * xzOff_23.y)) * 0.001);
					  v0_26 = (tmpvar_27 + tmpvar_31);
					  highp vec2 tmpvar_32;
					  tmpvar_32.x = float((tmpvar_28.z >= 0.0));
					  tmpvar_32.y = float((tmpvar_28.x >= 0.0));
					  xzOff_23 = (max (vec2(0.0, 0.0), (
					    abs(tmpvar_28.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_32 * 2.0) - 1.0));
					  xzOff_23 = (xzOff_23 * xzOff_23);
					  v1_25.y = (tmpvar_28.y + ((
					    (_V_CW_Bend.x * xzOff_23.x)
					   + 
					    (_V_CW_Bend.z * xzOff_23.y)
					  ) * 0.001));
					  highp vec2 tmpvar_33;
					  tmpvar_33.x = float((tmpvar_29.z >= 0.0));
					  tmpvar_33.y = float((tmpvar_29.x >= 0.0));
					  xzOff_23 = (max (vec2(0.0, 0.0), (
					    abs(tmpvar_29.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_33 * 2.0) - 1.0));
					  xzOff_23 = (xzOff_23 * xzOff_23);
					  v2_24.y = (tmpvar_29.y + ((
					    (_V_CW_Bend.x * xzOff_23.x)
					   + 
					    (_V_CW_Bend.z * xzOff_23.y)
					  ) * 0.001));
					  highp mat3 tmpvar_34;
					  tmpvar_34[0] = _World2Object[0].xyz;
					  tmpvar_34[1] = _World2Object[1].xyz;
					  tmpvar_34[2] = _World2Object[2].xyz;
					  vertex_22.xyz = (_glesVertex.xyz + (tmpvar_34 * tmpvar_31));
					  highp mat3 tmpvar_35;
					  tmpvar_35[0] = _World2Object[0].xyz;
					  tmpvar_35[1] = _World2Object[1].xyz;
					  tmpvar_35[2] = _World2Object[2].xyz;
					  highp vec3 a_36;
					  a_36 = (v2_24 - v0_26);
					  highp vec3 b_37;
					  b_37 = (v1_25 - v0_26);
					  highp vec3 tmpvar_38;
					  tmpvar_38 = normalize((tmpvar_35 * normalize(
					    ((a_36.yzx * b_37.zxy) - (a_36.zxy * b_37.yzx))
					  )));
					  tmpvar_2.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
					  tmpvar_2.zw = ((_glesMultiTexCoord0.xy * _V_CW_SecondaryTex_ST.xy) + _V_CW_SecondaryTex_ST.zw);
					  highp vec4 v_39;
					  v_39.x = tmpvar_5;
					  v_39.y = tmpvar_6;
					  v_39.z = tmpvar_7;
					  v_39.w = tmpvar_8;
					  highp vec4 v_40;
					  v_40.x = tmpvar_10;
					  v_40.y = tmpvar_11;
					  v_40.z = tmpvar_12;
					  v_40.w = tmpvar_13;
					  highp vec4 v_41;
					  v_41.x = tmpvar_15;
					  v_41.y = tmpvar_16;
					  v_41.z = tmpvar_17;
					  v_41.w = tmpvar_18;
					  highp vec3 tmpvar_42;
					  tmpvar_42 = normalize(((
					    (v_39.xyz * tmpvar_38.x)
					   + 
					    (v_40.xyz * tmpvar_38.y)
					  ) + (v_41.xyz * tmpvar_38.z)));
					  worldNormal_1 = tmpvar_42;
					  tmpvar_3 = worldNormal_1;
					  gl_Position = (glstate_matrix_mvp * vertex_22);
					  xlv_TEXCOORD0 = tmpvar_2;
					  xlv_TEXCOORD1 = tmpvar_3;
					  xlv_TEXCOORD2 = (_Object2World * vertex_22).xyz;
					  xlv_COLOR0 = _glesColor;
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform highp vec4 _Time;
					uniform mediump vec4 _WorldSpaceLightPos0;
					uniform lowp vec4 _LightColor0;
					uniform sampler2D _MainTex;
					uniform lowp vec4 _Color;
					uniform lowp vec2 _V_CW_MainTex_Scroll;
					uniform sampler2D _V_CW_SecondaryTex;
					uniform lowp vec2 _V_CW_SecondaryTex_Scroll;
					uniform lowp float _V_CW_SecondaryTex_Blend;
					varying highp vec4 xlv_TEXCOORD0;
					varying mediump vec3 xlv_TEXCOORD1;
					varying lowp vec4 xlv_COLOR0;
					void main ()
					{
					  mediump vec3 tmpvar_1;
					  mediump vec3 tmpvar_2;
					  lowp vec3 tmpvar_3;
					  lowp vec3 lightDir_4;
					  highp vec4 tmpvar_5;
					  mediump vec3 tmpvar_6;
					  tmpvar_6 = _WorldSpaceLightPos0.xyz;
					  lightDir_4 = tmpvar_6;
					  tmpvar_5 = xlv_COLOR0;
					  tmpvar_3 = xlv_TEXCOORD1;
					  lowp float vBlend_7;
					  highp vec2 P_8;
					  P_8 = (xlv_TEXCOORD0.xy + (_V_CW_MainTex_Scroll * _Time.x));
					  highp float tmpvar_9;
					  tmpvar_9 = clamp ((_V_CW_SecondaryTex_Blend + tmpvar_5.w), 0.0, 1.0);
					  vBlend_7 = tmpvar_9;
					  highp vec2 P_10;
					  P_10 = (xlv_TEXCOORD0.zw + (_V_CW_SecondaryTex_Scroll * _Time.x));
					  lowp vec4 tmpvar_11;
					  tmpvar_11 = mix (texture2D (_MainTex, P_8), texture2D (_V_CW_SecondaryTex, P_10), vec4(vBlend_7));
					  tmpvar_1 = _LightColor0.xyz;
					  tmpvar_2 = lightDir_4;
					  lowp vec4 c_12;
					  lowp vec4 c_13;
					  lowp float diff_14;
					  mediump float tmpvar_15;
					  tmpvar_15 = max (0.0, dot (tmpvar_3, tmpvar_2));
					  diff_14 = tmpvar_15;
					  c_13.xyz = ((tmpvar_11.xyz * _Color.xyz) * (tmpvar_1 * diff_14));
					  c_13.w = (tmpvar_11.w * _Color.w);
					  c_12.w = c_13.w;
					  c_12.xyz = c_13.xyz;
					  gl_FragData[0] = c_12;
					}
					
					
					#endif"
}
SubProgram "gles " {
Keywords { "SPOT" "V_CW_REFLECTIVE_OFF" "V_CW_VERTEX_COLOR_OFF" "_EMISSION_OFF" "V_CW_RIM_OFF" "_NORMALMAP_OFF" "V_CW_SPECULAR_OFF" }
					"!!GLES
					#version 100
					
					#ifdef VERTEX
					attribute vec4 _glesTANGENT;
					attribute vec4 _glesVertex;
					attribute vec4 _glesColor;
					attribute vec3 _glesNormal;
					attribute vec4 _glesMultiTexCoord0;
					uniform highp mat4 glstate_matrix_mvp;
					uniform highp mat4 _Object2World;
					uniform highp mat4 _World2Object;
					uniform highp vec3 _V_CW_Bend;
					uniform highp vec3 _V_CW_Bias;
					uniform highp vec4 _V_CW_PivotPoint_Position;
					uniform highp vec4 _MainTex_ST;
					uniform highp vec4 _V_CW_SecondaryTex_ST;
					varying highp vec4 xlv_TEXCOORD0;
					varying mediump vec3 xlv_TEXCOORD1;
					varying highp vec3 xlv_TEXCOORD2;
					varying lowp vec4 xlv_COLOR0;
					void main ()
					{
					  lowp vec3 worldNormal_1;
					  highp vec4 tmpvar_2;
					  mediump vec3 tmpvar_3;
					  highp vec4 v_4;
					  highp float tmpvar_5;
					  tmpvar_5 = _World2Object[0].x;
					  v_4.x = tmpvar_5;
					  highp float tmpvar_6;
					  tmpvar_6 = _World2Object[1].x;
					  v_4.y = tmpvar_6;
					  highp float tmpvar_7;
					  tmpvar_7 = _World2Object[2].x;
					  v_4.z = tmpvar_7;
					  highp float tmpvar_8;
					  tmpvar_8 = _World2Object[3].x;
					  v_4.w = tmpvar_8;
					  highp vec4 v_9;
					  highp float tmpvar_10;
					  tmpvar_10 = _World2Object[0].y;
					  v_9.x = tmpvar_10;
					  highp float tmpvar_11;
					  tmpvar_11 = _World2Object[1].y;
					  v_9.y = tmpvar_11;
					  highp float tmpvar_12;
					  tmpvar_12 = _World2Object[2].y;
					  v_9.z = tmpvar_12;
					  highp float tmpvar_13;
					  tmpvar_13 = _World2Object[3].y;
					  v_9.w = tmpvar_13;
					  highp vec4 v_14;
					  highp float tmpvar_15;
					  tmpvar_15 = _World2Object[0].z;
					  v_14.x = tmpvar_15;
					  highp float tmpvar_16;
					  tmpvar_16 = _World2Object[1].z;
					  v_14.y = tmpvar_16;
					  highp float tmpvar_17;
					  tmpvar_17 = _World2Object[2].z;
					  v_14.z = tmpvar_17;
					  highp float tmpvar_18;
					  tmpvar_18 = _World2Object[3].z;
					  v_14.w = tmpvar_18;
					  highp vec3 tmpvar_19;
					  tmpvar_19 = normalize(((
					    (v_4.xyz * _glesNormal.x)
					   + 
					    (v_9.xyz * _glesNormal.y)
					  ) + (v_14.xyz * _glesNormal.z)));
					  highp mat3 tmpvar_20;
					  tmpvar_20[0] = _Object2World[0].xyz;
					  tmpvar_20[1] = _Object2World[1].xyz;
					  tmpvar_20[2] = _Object2World[2].xyz;
					  highp vec3 tmpvar_21;
					  tmpvar_21 = normalize((tmpvar_20 * _glesTANGENT.xyz));
					  highp vec4 vertex_22;
					  vertex_22.w = _glesVertex.w;
					  highp vec2 xzOff_23;
					  highp vec3 v2_24;
					  highp vec3 v1_25;
					  highp vec3 v0_26;
					  highp vec3 tmpvar_27;
					  tmpvar_27 = ((_Object2World * _glesVertex).xyz - _V_CW_PivotPoint_Position.xyz);
					  highp vec3 tmpvar_28;
					  tmpvar_28 = (tmpvar_27 + tmpvar_21);
					  v1_25.xz = tmpvar_28.xz;
					  highp vec3 tmpvar_29;
					  tmpvar_29 = (tmpvar_27 - ((tmpvar_19.yzx * tmpvar_21.zxy) - (tmpvar_19.zxy * tmpvar_21.yzx)));
					  v2_24.xz = tmpvar_29.xz;
					  highp vec2 tmpvar_30;
					  tmpvar_30.x = float((tmpvar_27.z >= 0.0));
					  tmpvar_30.y = float((tmpvar_27.x >= 0.0));
					  xzOff_23 = (max (vec2(0.0, 0.0), (
					    abs(tmpvar_27.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_30 * 2.0) - 1.0));
					  xzOff_23 = (xzOff_23 * xzOff_23);
					  highp vec3 tmpvar_31;
					  tmpvar_31.xz = vec2(0.0, 0.0);
					  tmpvar_31.y = (((_V_CW_Bend.x * xzOff_23.x) + (_V_CW_Bend.z * xzOff_23.y)) * 0.001);
					  v0_26 = (tmpvar_27 + tmpvar_31);
					  highp vec2 tmpvar_32;
					  tmpvar_32.x = float((tmpvar_28.z >= 0.0));
					  tmpvar_32.y = float((tmpvar_28.x >= 0.0));
					  xzOff_23 = (max (vec2(0.0, 0.0), (
					    abs(tmpvar_28.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_32 * 2.0) - 1.0));
					  xzOff_23 = (xzOff_23 * xzOff_23);
					  v1_25.y = (tmpvar_28.y + ((
					    (_V_CW_Bend.x * xzOff_23.x)
					   + 
					    (_V_CW_Bend.z * xzOff_23.y)
					  ) * 0.001));
					  highp vec2 tmpvar_33;
					  tmpvar_33.x = float((tmpvar_29.z >= 0.0));
					  tmpvar_33.y = float((tmpvar_29.x >= 0.0));
					  xzOff_23 = (max (vec2(0.0, 0.0), (
					    abs(tmpvar_29.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_33 * 2.0) - 1.0));
					  xzOff_23 = (xzOff_23 * xzOff_23);
					  v2_24.y = (tmpvar_29.y + ((
					    (_V_CW_Bend.x * xzOff_23.x)
					   + 
					    (_V_CW_Bend.z * xzOff_23.y)
					  ) * 0.001));
					  highp mat3 tmpvar_34;
					  tmpvar_34[0] = _World2Object[0].xyz;
					  tmpvar_34[1] = _World2Object[1].xyz;
					  tmpvar_34[2] = _World2Object[2].xyz;
					  vertex_22.xyz = (_glesVertex.xyz + (tmpvar_34 * tmpvar_31));
					  highp mat3 tmpvar_35;
					  tmpvar_35[0] = _World2Object[0].xyz;
					  tmpvar_35[1] = _World2Object[1].xyz;
					  tmpvar_35[2] = _World2Object[2].xyz;
					  highp vec3 a_36;
					  a_36 = (v2_24 - v0_26);
					  highp vec3 b_37;
					  b_37 = (v1_25 - v0_26);
					  highp vec3 tmpvar_38;
					  tmpvar_38 = normalize((tmpvar_35 * normalize(
					    ((a_36.yzx * b_37.zxy) - (a_36.zxy * b_37.yzx))
					  )));
					  tmpvar_2.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
					  tmpvar_2.zw = ((_glesMultiTexCoord0.xy * _V_CW_SecondaryTex_ST.xy) + _V_CW_SecondaryTex_ST.zw);
					  highp vec4 v_39;
					  v_39.x = tmpvar_5;
					  v_39.y = tmpvar_6;
					  v_39.z = tmpvar_7;
					  v_39.w = tmpvar_8;
					  highp vec4 v_40;
					  v_40.x = tmpvar_10;
					  v_40.y = tmpvar_11;
					  v_40.z = tmpvar_12;
					  v_40.w = tmpvar_13;
					  highp vec4 v_41;
					  v_41.x = tmpvar_15;
					  v_41.y = tmpvar_16;
					  v_41.z = tmpvar_17;
					  v_41.w = tmpvar_18;
					  highp vec3 tmpvar_42;
					  tmpvar_42 = normalize(((
					    (v_39.xyz * tmpvar_38.x)
					   + 
					    (v_40.xyz * tmpvar_38.y)
					  ) + (v_41.xyz * tmpvar_38.z)));
					  worldNormal_1 = tmpvar_42;
					  tmpvar_3 = worldNormal_1;
					  gl_Position = (glstate_matrix_mvp * vertex_22);
					  xlv_TEXCOORD0 = tmpvar_2;
					  xlv_TEXCOORD1 = tmpvar_3;
					  xlv_TEXCOORD2 = (_Object2World * vertex_22).xyz;
					  xlv_COLOR0 = _glesColor;
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform highp vec4 _Time;
					uniform highp vec4 _WorldSpaceLightPos0;
					uniform lowp vec4 _LightColor0;
					uniform sampler2D _LightTexture0;
					uniform mediump mat4 _LightMatrix0;
					uniform sampler2D _LightTextureB0;
					uniform sampler2D _MainTex;
					uniform lowp vec4 _Color;
					uniform lowp vec2 _V_CW_MainTex_Scroll;
					uniform sampler2D _V_CW_SecondaryTex;
					uniform lowp vec2 _V_CW_SecondaryTex_Scroll;
					uniform lowp float _V_CW_SecondaryTex_Blend;
					varying highp vec4 xlv_TEXCOORD0;
					varying mediump vec3 xlv_TEXCOORD1;
					varying highp vec3 xlv_TEXCOORD2;
					varying lowp vec4 xlv_COLOR0;
					void main ()
					{
					  mediump vec3 tmpvar_1;
					  mediump vec3 tmpvar_2;
					  lowp float atten_3;
					  mediump vec4 lightCoord_4;
					  lowp vec3 tmpvar_5;
					  lowp vec3 lightDir_6;
					  highp vec4 tmpvar_7;
					  highp vec3 tmpvar_8;
					  tmpvar_8 = normalize((_WorldSpaceLightPos0.xyz - xlv_TEXCOORD2));
					  lightDir_6 = tmpvar_8;
					  tmpvar_7 = xlv_COLOR0;
					  tmpvar_5 = xlv_TEXCOORD1;
					  lowp float vBlend_9;
					  highp vec2 P_10;
					  P_10 = (xlv_TEXCOORD0.xy + (_V_CW_MainTex_Scroll * _Time.x));
					  highp float tmpvar_11;
					  tmpvar_11 = clamp ((_V_CW_SecondaryTex_Blend + tmpvar_7.w), 0.0, 1.0);
					  vBlend_9 = tmpvar_11;
					  highp vec2 P_12;
					  P_12 = (xlv_TEXCOORD0.zw + (_V_CW_SecondaryTex_Scroll * _Time.x));
					  lowp vec4 tmpvar_13;
					  tmpvar_13 = mix (texture2D (_MainTex, P_10), texture2D (_V_CW_SecondaryTex, P_12), vec4(vBlend_9));
					  highp vec4 tmpvar_14;
					  tmpvar_14.w = 1.0;
					  tmpvar_14.xyz = xlv_TEXCOORD2;
					  highp vec4 tmpvar_15;
					  tmpvar_15 = (_LightMatrix0 * tmpvar_14);
					  lightCoord_4 = tmpvar_15;
					  lowp vec4 tmpvar_16;
					  mediump vec2 P_17;
					  P_17 = ((lightCoord_4.xy / lightCoord_4.w) + 0.5);
					  tmpvar_16 = texture2D (_LightTexture0, P_17);
					  highp vec3 LightCoord_18;
					  LightCoord_18 = lightCoord_4.xyz;
					  highp float tmpvar_19;
					  tmpvar_19 = dot (LightCoord_18, LightCoord_18);
					  lowp vec4 tmpvar_20;
					  tmpvar_20 = texture2D (_LightTextureB0, vec2(tmpvar_19));
					  mediump float tmpvar_21;
					  tmpvar_21 = ((float(
					    (lightCoord_4.z > 0.0)
					  ) * tmpvar_16.w) * tmpvar_20.w);
					  atten_3 = tmpvar_21;
					  tmpvar_1 = _LightColor0.xyz;
					  tmpvar_2 = lightDir_6;
					  tmpvar_1 = (tmpvar_1 * atten_3);
					  lowp vec4 c_22;
					  lowp vec4 c_23;
					  lowp float diff_24;
					  mediump float tmpvar_25;
					  tmpvar_25 = max (0.0, dot (tmpvar_5, tmpvar_2));
					  diff_24 = tmpvar_25;
					  c_23.xyz = ((tmpvar_13.xyz * _Color.xyz) * (tmpvar_1 * diff_24));
					  c_23.w = (tmpvar_13.w * _Color.w);
					  c_22.w = c_23.w;
					  c_22.xyz = c_23.xyz;
					  gl_FragData[0] = c_22;
					}
					
					
					#endif"
}
SubProgram "gles " {
Keywords { "POINT_COOKIE" "V_CW_REFLECTIVE_OFF" "V_CW_VERTEX_COLOR_OFF" "_EMISSION_OFF" "V_CW_RIM_OFF" "_NORMALMAP_OFF" "V_CW_SPECULAR_OFF" }
					"!!GLES
					#version 100
					
					#ifdef VERTEX
					attribute vec4 _glesTANGENT;
					attribute vec4 _glesVertex;
					attribute vec4 _glesColor;
					attribute vec3 _glesNormal;
					attribute vec4 _glesMultiTexCoord0;
					uniform highp mat4 glstate_matrix_mvp;
					uniform highp mat4 _Object2World;
					uniform highp mat4 _World2Object;
					uniform highp vec3 _V_CW_Bend;
					uniform highp vec3 _V_CW_Bias;
					uniform highp vec4 _V_CW_PivotPoint_Position;
					uniform highp vec4 _MainTex_ST;
					uniform highp vec4 _V_CW_SecondaryTex_ST;
					varying highp vec4 xlv_TEXCOORD0;
					varying mediump vec3 xlv_TEXCOORD1;
					varying highp vec3 xlv_TEXCOORD2;
					varying lowp vec4 xlv_COLOR0;
					void main ()
					{
					  lowp vec3 worldNormal_1;
					  highp vec4 tmpvar_2;
					  mediump vec3 tmpvar_3;
					  highp vec4 v_4;
					  highp float tmpvar_5;
					  tmpvar_5 = _World2Object[0].x;
					  v_4.x = tmpvar_5;
					  highp float tmpvar_6;
					  tmpvar_6 = _World2Object[1].x;
					  v_4.y = tmpvar_6;
					  highp float tmpvar_7;
					  tmpvar_7 = _World2Object[2].x;
					  v_4.z = tmpvar_7;
					  highp float tmpvar_8;
					  tmpvar_8 = _World2Object[3].x;
					  v_4.w = tmpvar_8;
					  highp vec4 v_9;
					  highp float tmpvar_10;
					  tmpvar_10 = _World2Object[0].y;
					  v_9.x = tmpvar_10;
					  highp float tmpvar_11;
					  tmpvar_11 = _World2Object[1].y;
					  v_9.y = tmpvar_11;
					  highp float tmpvar_12;
					  tmpvar_12 = _World2Object[2].y;
					  v_9.z = tmpvar_12;
					  highp float tmpvar_13;
					  tmpvar_13 = _World2Object[3].y;
					  v_9.w = tmpvar_13;
					  highp vec4 v_14;
					  highp float tmpvar_15;
					  tmpvar_15 = _World2Object[0].z;
					  v_14.x = tmpvar_15;
					  highp float tmpvar_16;
					  tmpvar_16 = _World2Object[1].z;
					  v_14.y = tmpvar_16;
					  highp float tmpvar_17;
					  tmpvar_17 = _World2Object[2].z;
					  v_14.z = tmpvar_17;
					  highp float tmpvar_18;
					  tmpvar_18 = _World2Object[3].z;
					  v_14.w = tmpvar_18;
					  highp vec3 tmpvar_19;
					  tmpvar_19 = normalize(((
					    (v_4.xyz * _glesNormal.x)
					   + 
					    (v_9.xyz * _glesNormal.y)
					  ) + (v_14.xyz * _glesNormal.z)));
					  highp mat3 tmpvar_20;
					  tmpvar_20[0] = _Object2World[0].xyz;
					  tmpvar_20[1] = _Object2World[1].xyz;
					  tmpvar_20[2] = _Object2World[2].xyz;
					  highp vec3 tmpvar_21;
					  tmpvar_21 = normalize((tmpvar_20 * _glesTANGENT.xyz));
					  highp vec4 vertex_22;
					  vertex_22.w = _glesVertex.w;
					  highp vec2 xzOff_23;
					  highp vec3 v2_24;
					  highp vec3 v1_25;
					  highp vec3 v0_26;
					  highp vec3 tmpvar_27;
					  tmpvar_27 = ((_Object2World * _glesVertex).xyz - _V_CW_PivotPoint_Position.xyz);
					  highp vec3 tmpvar_28;
					  tmpvar_28 = (tmpvar_27 + tmpvar_21);
					  v1_25.xz = tmpvar_28.xz;
					  highp vec3 tmpvar_29;
					  tmpvar_29 = (tmpvar_27 - ((tmpvar_19.yzx * tmpvar_21.zxy) - (tmpvar_19.zxy * tmpvar_21.yzx)));
					  v2_24.xz = tmpvar_29.xz;
					  highp vec2 tmpvar_30;
					  tmpvar_30.x = float((tmpvar_27.z >= 0.0));
					  tmpvar_30.y = float((tmpvar_27.x >= 0.0));
					  xzOff_23 = (max (vec2(0.0, 0.0), (
					    abs(tmpvar_27.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_30 * 2.0) - 1.0));
					  xzOff_23 = (xzOff_23 * xzOff_23);
					  highp vec3 tmpvar_31;
					  tmpvar_31.xz = vec2(0.0, 0.0);
					  tmpvar_31.y = (((_V_CW_Bend.x * xzOff_23.x) + (_V_CW_Bend.z * xzOff_23.y)) * 0.001);
					  v0_26 = (tmpvar_27 + tmpvar_31);
					  highp vec2 tmpvar_32;
					  tmpvar_32.x = float((tmpvar_28.z >= 0.0));
					  tmpvar_32.y = float((tmpvar_28.x >= 0.0));
					  xzOff_23 = (max (vec2(0.0, 0.0), (
					    abs(tmpvar_28.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_32 * 2.0) - 1.0));
					  xzOff_23 = (xzOff_23 * xzOff_23);
					  v1_25.y = (tmpvar_28.y + ((
					    (_V_CW_Bend.x * xzOff_23.x)
					   + 
					    (_V_CW_Bend.z * xzOff_23.y)
					  ) * 0.001));
					  highp vec2 tmpvar_33;
					  tmpvar_33.x = float((tmpvar_29.z >= 0.0));
					  tmpvar_33.y = float((tmpvar_29.x >= 0.0));
					  xzOff_23 = (max (vec2(0.0, 0.0), (
					    abs(tmpvar_29.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_33 * 2.0) - 1.0));
					  xzOff_23 = (xzOff_23 * xzOff_23);
					  v2_24.y = (tmpvar_29.y + ((
					    (_V_CW_Bend.x * xzOff_23.x)
					   + 
					    (_V_CW_Bend.z * xzOff_23.y)
					  ) * 0.001));
					  highp mat3 tmpvar_34;
					  tmpvar_34[0] = _World2Object[0].xyz;
					  tmpvar_34[1] = _World2Object[1].xyz;
					  tmpvar_34[2] = _World2Object[2].xyz;
					  vertex_22.xyz = (_glesVertex.xyz + (tmpvar_34 * tmpvar_31));
					  highp mat3 tmpvar_35;
					  tmpvar_35[0] = _World2Object[0].xyz;
					  tmpvar_35[1] = _World2Object[1].xyz;
					  tmpvar_35[2] = _World2Object[2].xyz;
					  highp vec3 a_36;
					  a_36 = (v2_24 - v0_26);
					  highp vec3 b_37;
					  b_37 = (v1_25 - v0_26);
					  highp vec3 tmpvar_38;
					  tmpvar_38 = normalize((tmpvar_35 * normalize(
					    ((a_36.yzx * b_37.zxy) - (a_36.zxy * b_37.yzx))
					  )));
					  tmpvar_2.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
					  tmpvar_2.zw = ((_glesMultiTexCoord0.xy * _V_CW_SecondaryTex_ST.xy) + _V_CW_SecondaryTex_ST.zw);
					  highp vec4 v_39;
					  v_39.x = tmpvar_5;
					  v_39.y = tmpvar_6;
					  v_39.z = tmpvar_7;
					  v_39.w = tmpvar_8;
					  highp vec4 v_40;
					  v_40.x = tmpvar_10;
					  v_40.y = tmpvar_11;
					  v_40.z = tmpvar_12;
					  v_40.w = tmpvar_13;
					  highp vec4 v_41;
					  v_41.x = tmpvar_15;
					  v_41.y = tmpvar_16;
					  v_41.z = tmpvar_17;
					  v_41.w = tmpvar_18;
					  highp vec3 tmpvar_42;
					  tmpvar_42 = normalize(((
					    (v_39.xyz * tmpvar_38.x)
					   + 
					    (v_40.xyz * tmpvar_38.y)
					  ) + (v_41.xyz * tmpvar_38.z)));
					  worldNormal_1 = tmpvar_42;
					  tmpvar_3 = worldNormal_1;
					  gl_Position = (glstate_matrix_mvp * vertex_22);
					  xlv_TEXCOORD0 = tmpvar_2;
					  xlv_TEXCOORD1 = tmpvar_3;
					  xlv_TEXCOORD2 = (_Object2World * vertex_22).xyz;
					  xlv_COLOR0 = _glesColor;
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform highp vec4 _Time;
					uniform highp vec4 _WorldSpaceLightPos0;
					uniform lowp vec4 _LightColor0;
					uniform lowp samplerCube _LightTexture0;
					uniform mediump mat4 _LightMatrix0;
					uniform sampler2D _LightTextureB0;
					uniform sampler2D _MainTex;
					uniform lowp vec4 _Color;
					uniform lowp vec2 _V_CW_MainTex_Scroll;
					uniform sampler2D _V_CW_SecondaryTex;
					uniform lowp vec2 _V_CW_SecondaryTex_Scroll;
					uniform lowp float _V_CW_SecondaryTex_Blend;
					varying highp vec4 xlv_TEXCOORD0;
					varying mediump vec3 xlv_TEXCOORD1;
					varying highp vec3 xlv_TEXCOORD2;
					varying lowp vec4 xlv_COLOR0;
					void main ()
					{
					  mediump vec3 tmpvar_1;
					  mediump vec3 tmpvar_2;
					  lowp vec3 tmpvar_3;
					  lowp vec3 lightDir_4;
					  highp vec4 tmpvar_5;
					  highp vec3 tmpvar_6;
					  tmpvar_6 = normalize((_WorldSpaceLightPos0.xyz - xlv_TEXCOORD2));
					  lightDir_4 = tmpvar_6;
					  tmpvar_5 = xlv_COLOR0;
					  tmpvar_3 = xlv_TEXCOORD1;
					  lowp float vBlend_7;
					  highp vec2 P_8;
					  P_8 = (xlv_TEXCOORD0.xy + (_V_CW_MainTex_Scroll * _Time.x));
					  highp float tmpvar_9;
					  tmpvar_9 = clamp ((_V_CW_SecondaryTex_Blend + tmpvar_5.w), 0.0, 1.0);
					  vBlend_7 = tmpvar_9;
					  highp vec2 P_10;
					  P_10 = (xlv_TEXCOORD0.zw + (_V_CW_SecondaryTex_Scroll * _Time.x));
					  lowp vec4 tmpvar_11;
					  tmpvar_11 = mix (texture2D (_MainTex, P_8), texture2D (_V_CW_SecondaryTex, P_10), vec4(vBlend_7));
					  highp vec4 tmpvar_12;
					  tmpvar_12.w = 1.0;
					  tmpvar_12.xyz = xlv_TEXCOORD2;
					  highp vec3 tmpvar_13;
					  tmpvar_13 = (_LightMatrix0 * tmpvar_12).xyz;
					  highp float tmpvar_14;
					  tmpvar_14 = dot (tmpvar_13, tmpvar_13);
					  lowp float tmpvar_15;
					  tmpvar_15 = (texture2D (_LightTextureB0, vec2(tmpvar_14)).w * textureCube (_LightTexture0, tmpvar_13).w);
					  tmpvar_1 = _LightColor0.xyz;
					  tmpvar_2 = lightDir_4;
					  tmpvar_1 = (tmpvar_1 * tmpvar_15);
					  lowp vec4 c_16;
					  lowp vec4 c_17;
					  lowp float diff_18;
					  mediump float tmpvar_19;
					  tmpvar_19 = max (0.0, dot (tmpvar_3, tmpvar_2));
					  diff_18 = tmpvar_19;
					  c_17.xyz = ((tmpvar_11.xyz * _Color.xyz) * (tmpvar_1 * diff_18));
					  c_17.w = (tmpvar_11.w * _Color.w);
					  c_16.w = c_17.w;
					  c_16.xyz = c_17.xyz;
					  gl_FragData[0] = c_16;
					}
					
					
					#endif"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL_COOKIE" "V_CW_REFLECTIVE_OFF" "V_CW_VERTEX_COLOR_OFF" "_EMISSION_OFF" "V_CW_RIM_OFF" "_NORMALMAP_OFF" "V_CW_SPECULAR_OFF" }
					"!!GLES
					#version 100
					
					#ifdef VERTEX
					attribute vec4 _glesTANGENT;
					attribute vec4 _glesVertex;
					attribute vec4 _glesColor;
					attribute vec3 _glesNormal;
					attribute vec4 _glesMultiTexCoord0;
					uniform highp mat4 glstate_matrix_mvp;
					uniform highp mat4 _Object2World;
					uniform highp mat4 _World2Object;
					uniform highp vec3 _V_CW_Bend;
					uniform highp vec3 _V_CW_Bias;
					uniform highp vec4 _V_CW_PivotPoint_Position;
					uniform highp vec4 _MainTex_ST;
					uniform highp vec4 _V_CW_SecondaryTex_ST;
					varying highp vec4 xlv_TEXCOORD0;
					varying mediump vec3 xlv_TEXCOORD1;
					varying highp vec3 xlv_TEXCOORD2;
					varying lowp vec4 xlv_COLOR0;
					void main ()
					{
					  lowp vec3 worldNormal_1;
					  highp vec4 tmpvar_2;
					  mediump vec3 tmpvar_3;
					  highp vec4 v_4;
					  highp float tmpvar_5;
					  tmpvar_5 = _World2Object[0].x;
					  v_4.x = tmpvar_5;
					  highp float tmpvar_6;
					  tmpvar_6 = _World2Object[1].x;
					  v_4.y = tmpvar_6;
					  highp float tmpvar_7;
					  tmpvar_7 = _World2Object[2].x;
					  v_4.z = tmpvar_7;
					  highp float tmpvar_8;
					  tmpvar_8 = _World2Object[3].x;
					  v_4.w = tmpvar_8;
					  highp vec4 v_9;
					  highp float tmpvar_10;
					  tmpvar_10 = _World2Object[0].y;
					  v_9.x = tmpvar_10;
					  highp float tmpvar_11;
					  tmpvar_11 = _World2Object[1].y;
					  v_9.y = tmpvar_11;
					  highp float tmpvar_12;
					  tmpvar_12 = _World2Object[2].y;
					  v_9.z = tmpvar_12;
					  highp float tmpvar_13;
					  tmpvar_13 = _World2Object[3].y;
					  v_9.w = tmpvar_13;
					  highp vec4 v_14;
					  highp float tmpvar_15;
					  tmpvar_15 = _World2Object[0].z;
					  v_14.x = tmpvar_15;
					  highp float tmpvar_16;
					  tmpvar_16 = _World2Object[1].z;
					  v_14.y = tmpvar_16;
					  highp float tmpvar_17;
					  tmpvar_17 = _World2Object[2].z;
					  v_14.z = tmpvar_17;
					  highp float tmpvar_18;
					  tmpvar_18 = _World2Object[3].z;
					  v_14.w = tmpvar_18;
					  highp vec3 tmpvar_19;
					  tmpvar_19 = normalize(((
					    (v_4.xyz * _glesNormal.x)
					   + 
					    (v_9.xyz * _glesNormal.y)
					  ) + (v_14.xyz * _glesNormal.z)));
					  highp mat3 tmpvar_20;
					  tmpvar_20[0] = _Object2World[0].xyz;
					  tmpvar_20[1] = _Object2World[1].xyz;
					  tmpvar_20[2] = _Object2World[2].xyz;
					  highp vec3 tmpvar_21;
					  tmpvar_21 = normalize((tmpvar_20 * _glesTANGENT.xyz));
					  highp vec4 vertex_22;
					  vertex_22.w = _glesVertex.w;
					  highp vec2 xzOff_23;
					  highp vec3 v2_24;
					  highp vec3 v1_25;
					  highp vec3 v0_26;
					  highp vec3 tmpvar_27;
					  tmpvar_27 = ((_Object2World * _glesVertex).xyz - _V_CW_PivotPoint_Position.xyz);
					  highp vec3 tmpvar_28;
					  tmpvar_28 = (tmpvar_27 + tmpvar_21);
					  v1_25.xz = tmpvar_28.xz;
					  highp vec3 tmpvar_29;
					  tmpvar_29 = (tmpvar_27 - ((tmpvar_19.yzx * tmpvar_21.zxy) - (tmpvar_19.zxy * tmpvar_21.yzx)));
					  v2_24.xz = tmpvar_29.xz;
					  highp vec2 tmpvar_30;
					  tmpvar_30.x = float((tmpvar_27.z >= 0.0));
					  tmpvar_30.y = float((tmpvar_27.x >= 0.0));
					  xzOff_23 = (max (vec2(0.0, 0.0), (
					    abs(tmpvar_27.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_30 * 2.0) - 1.0));
					  xzOff_23 = (xzOff_23 * xzOff_23);
					  highp vec3 tmpvar_31;
					  tmpvar_31.xz = vec2(0.0, 0.0);
					  tmpvar_31.y = (((_V_CW_Bend.x * xzOff_23.x) + (_V_CW_Bend.z * xzOff_23.y)) * 0.001);
					  v0_26 = (tmpvar_27 + tmpvar_31);
					  highp vec2 tmpvar_32;
					  tmpvar_32.x = float((tmpvar_28.z >= 0.0));
					  tmpvar_32.y = float((tmpvar_28.x >= 0.0));
					  xzOff_23 = (max (vec2(0.0, 0.0), (
					    abs(tmpvar_28.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_32 * 2.0) - 1.0));
					  xzOff_23 = (xzOff_23 * xzOff_23);
					  v1_25.y = (tmpvar_28.y + ((
					    (_V_CW_Bend.x * xzOff_23.x)
					   + 
					    (_V_CW_Bend.z * xzOff_23.y)
					  ) * 0.001));
					  highp vec2 tmpvar_33;
					  tmpvar_33.x = float((tmpvar_29.z >= 0.0));
					  tmpvar_33.y = float((tmpvar_29.x >= 0.0));
					  xzOff_23 = (max (vec2(0.0, 0.0), (
					    abs(tmpvar_29.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_33 * 2.0) - 1.0));
					  xzOff_23 = (xzOff_23 * xzOff_23);
					  v2_24.y = (tmpvar_29.y + ((
					    (_V_CW_Bend.x * xzOff_23.x)
					   + 
					    (_V_CW_Bend.z * xzOff_23.y)
					  ) * 0.001));
					  highp mat3 tmpvar_34;
					  tmpvar_34[0] = _World2Object[0].xyz;
					  tmpvar_34[1] = _World2Object[1].xyz;
					  tmpvar_34[2] = _World2Object[2].xyz;
					  vertex_22.xyz = (_glesVertex.xyz + (tmpvar_34 * tmpvar_31));
					  highp mat3 tmpvar_35;
					  tmpvar_35[0] = _World2Object[0].xyz;
					  tmpvar_35[1] = _World2Object[1].xyz;
					  tmpvar_35[2] = _World2Object[2].xyz;
					  highp vec3 a_36;
					  a_36 = (v2_24 - v0_26);
					  highp vec3 b_37;
					  b_37 = (v1_25 - v0_26);
					  highp vec3 tmpvar_38;
					  tmpvar_38 = normalize((tmpvar_35 * normalize(
					    ((a_36.yzx * b_37.zxy) - (a_36.zxy * b_37.yzx))
					  )));
					  tmpvar_2.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
					  tmpvar_2.zw = ((_glesMultiTexCoord0.xy * _V_CW_SecondaryTex_ST.xy) + _V_CW_SecondaryTex_ST.zw);
					  highp vec4 v_39;
					  v_39.x = tmpvar_5;
					  v_39.y = tmpvar_6;
					  v_39.z = tmpvar_7;
					  v_39.w = tmpvar_8;
					  highp vec4 v_40;
					  v_40.x = tmpvar_10;
					  v_40.y = tmpvar_11;
					  v_40.z = tmpvar_12;
					  v_40.w = tmpvar_13;
					  highp vec4 v_41;
					  v_41.x = tmpvar_15;
					  v_41.y = tmpvar_16;
					  v_41.z = tmpvar_17;
					  v_41.w = tmpvar_18;
					  highp vec3 tmpvar_42;
					  tmpvar_42 = normalize(((
					    (v_39.xyz * tmpvar_38.x)
					   + 
					    (v_40.xyz * tmpvar_38.y)
					  ) + (v_41.xyz * tmpvar_38.z)));
					  worldNormal_1 = tmpvar_42;
					  tmpvar_3 = worldNormal_1;
					  gl_Position = (glstate_matrix_mvp * vertex_22);
					  xlv_TEXCOORD0 = tmpvar_2;
					  xlv_TEXCOORD1 = tmpvar_3;
					  xlv_TEXCOORD2 = (_Object2World * vertex_22).xyz;
					  xlv_COLOR0 = _glesColor;
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform highp vec4 _Time;
					uniform mediump vec4 _WorldSpaceLightPos0;
					uniform lowp vec4 _LightColor0;
					uniform sampler2D _LightTexture0;
					uniform mediump mat4 _LightMatrix0;
					uniform sampler2D _MainTex;
					uniform lowp vec4 _Color;
					uniform lowp vec2 _V_CW_MainTex_Scroll;
					uniform sampler2D _V_CW_SecondaryTex;
					uniform lowp vec2 _V_CW_SecondaryTex_Scroll;
					uniform lowp float _V_CW_SecondaryTex_Blend;
					varying highp vec4 xlv_TEXCOORD0;
					varying mediump vec3 xlv_TEXCOORD1;
					varying highp vec3 xlv_TEXCOORD2;
					varying lowp vec4 xlv_COLOR0;
					void main ()
					{
					  mediump vec3 tmpvar_1;
					  mediump vec3 tmpvar_2;
					  lowp vec3 tmpvar_3;
					  lowp vec3 lightDir_4;
					  highp vec4 tmpvar_5;
					  mediump vec3 tmpvar_6;
					  tmpvar_6 = _WorldSpaceLightPos0.xyz;
					  lightDir_4 = tmpvar_6;
					  tmpvar_5 = xlv_COLOR0;
					  tmpvar_3 = xlv_TEXCOORD1;
					  lowp float vBlend_7;
					  highp vec2 P_8;
					  P_8 = (xlv_TEXCOORD0.xy + (_V_CW_MainTex_Scroll * _Time.x));
					  highp float tmpvar_9;
					  tmpvar_9 = clamp ((_V_CW_SecondaryTex_Blend + tmpvar_5.w), 0.0, 1.0);
					  vBlend_7 = tmpvar_9;
					  highp vec2 P_10;
					  P_10 = (xlv_TEXCOORD0.zw + (_V_CW_SecondaryTex_Scroll * _Time.x));
					  lowp vec4 tmpvar_11;
					  tmpvar_11 = mix (texture2D (_MainTex, P_8), texture2D (_V_CW_SecondaryTex, P_10), vec4(vBlend_7));
					  highp vec4 tmpvar_12;
					  tmpvar_12.w = 1.0;
					  tmpvar_12.xyz = xlv_TEXCOORD2;
					  highp vec2 tmpvar_13;
					  tmpvar_13 = (_LightMatrix0 * tmpvar_12).xy;
					  lowp float tmpvar_14;
					  tmpvar_14 = texture2D (_LightTexture0, tmpvar_13).w;
					  tmpvar_1 = _LightColor0.xyz;
					  tmpvar_2 = lightDir_4;
					  tmpvar_1 = (tmpvar_1 * tmpvar_14);
					  lowp vec4 c_15;
					  lowp vec4 c_16;
					  lowp float diff_17;
					  mediump float tmpvar_18;
					  tmpvar_18 = max (0.0, dot (tmpvar_3, tmpvar_2));
					  diff_17 = tmpvar_18;
					  c_16.xyz = ((tmpvar_11.xyz * _Color.xyz) * (tmpvar_1 * diff_17));
					  c_16.w = (tmpvar_11.w * _Color.w);
					  c_15.w = c_16.w;
					  c_15.xyz = c_16.xyz;
					  gl_FragData[0] = c_15;
					}
					
					
					#endif"
}
SubProgram "gles " {
Keywords { "POINT" "FOG_LINEAR" "V_CW_REFLECTIVE_OFF" "V_CW_VERTEX_COLOR_OFF" "_EMISSION_OFF" "V_CW_RIM_OFF" "_NORMALMAP_OFF" "V_CW_SPECULAR_OFF" }
					"!!GLES
					#version 100
					
					#ifdef VERTEX
					attribute vec4 _glesTANGENT;
					attribute vec4 _glesVertex;
					attribute vec4 _glesColor;
					attribute vec3 _glesNormal;
					attribute vec4 _glesMultiTexCoord0;
					uniform highp mat4 glstate_matrix_mvp;
					uniform highp mat4 _Object2World;
					uniform highp mat4 _World2Object;
					uniform highp vec4 unity_FogParams;
					uniform highp vec3 _V_CW_Bend;
					uniform highp vec3 _V_CW_Bias;
					uniform highp vec4 _V_CW_PivotPoint_Position;
					uniform highp vec4 _MainTex_ST;
					uniform highp vec4 _V_CW_SecondaryTex_ST;
					varying highp vec4 xlv_TEXCOORD0;
					varying mediump vec3 xlv_TEXCOORD1;
					varying highp vec3 xlv_TEXCOORD2;
					varying lowp vec4 xlv_COLOR0;
					varying highp float xlv_TEXCOORD3;
					void main ()
					{
					  lowp vec3 worldNormal_1;
					  highp vec4 tmpvar_2;
					  highp vec4 tmpvar_3;
					  mediump vec3 tmpvar_4;
					  highp vec4 v_5;
					  highp float tmpvar_6;
					  tmpvar_6 = _World2Object[0].x;
					  v_5.x = tmpvar_6;
					  highp float tmpvar_7;
					  tmpvar_7 = _World2Object[1].x;
					  v_5.y = tmpvar_7;
					  highp float tmpvar_8;
					  tmpvar_8 = _World2Object[2].x;
					  v_5.z = tmpvar_8;
					  highp float tmpvar_9;
					  tmpvar_9 = _World2Object[3].x;
					  v_5.w = tmpvar_9;
					  highp vec4 v_10;
					  highp float tmpvar_11;
					  tmpvar_11 = _World2Object[0].y;
					  v_10.x = tmpvar_11;
					  highp float tmpvar_12;
					  tmpvar_12 = _World2Object[1].y;
					  v_10.y = tmpvar_12;
					  highp float tmpvar_13;
					  tmpvar_13 = _World2Object[2].y;
					  v_10.z = tmpvar_13;
					  highp float tmpvar_14;
					  tmpvar_14 = _World2Object[3].y;
					  v_10.w = tmpvar_14;
					  highp vec4 v_15;
					  highp float tmpvar_16;
					  tmpvar_16 = _World2Object[0].z;
					  v_15.x = tmpvar_16;
					  highp float tmpvar_17;
					  tmpvar_17 = _World2Object[1].z;
					  v_15.y = tmpvar_17;
					  highp float tmpvar_18;
					  tmpvar_18 = _World2Object[2].z;
					  v_15.z = tmpvar_18;
					  highp float tmpvar_19;
					  tmpvar_19 = _World2Object[3].z;
					  v_15.w = tmpvar_19;
					  highp vec3 tmpvar_20;
					  tmpvar_20 = normalize(((
					    (v_5.xyz * _glesNormal.x)
					   + 
					    (v_10.xyz * _glesNormal.y)
					  ) + (v_15.xyz * _glesNormal.z)));
					  highp mat3 tmpvar_21;
					  tmpvar_21[0] = _Object2World[0].xyz;
					  tmpvar_21[1] = _Object2World[1].xyz;
					  tmpvar_21[2] = _Object2World[2].xyz;
					  highp vec3 tmpvar_22;
					  tmpvar_22 = normalize((tmpvar_21 * _glesTANGENT.xyz));
					  highp vec4 vertex_23;
					  vertex_23.w = _glesVertex.w;
					  highp vec2 xzOff_24;
					  highp vec3 v2_25;
					  highp vec3 v1_26;
					  highp vec3 v0_27;
					  highp vec3 tmpvar_28;
					  tmpvar_28 = ((_Object2World * _glesVertex).xyz - _V_CW_PivotPoint_Position.xyz);
					  highp vec3 tmpvar_29;
					  tmpvar_29 = (tmpvar_28 + tmpvar_22);
					  v1_26.xz = tmpvar_29.xz;
					  highp vec3 tmpvar_30;
					  tmpvar_30 = (tmpvar_28 - ((tmpvar_20.yzx * tmpvar_22.zxy) - (tmpvar_20.zxy * tmpvar_22.yzx)));
					  v2_25.xz = tmpvar_30.xz;
					  highp vec2 tmpvar_31;
					  tmpvar_31.x = float((tmpvar_28.z >= 0.0));
					  tmpvar_31.y = float((tmpvar_28.x >= 0.0));
					  xzOff_24 = (max (vec2(0.0, 0.0), (
					    abs(tmpvar_28.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_31 * 2.0) - 1.0));
					  xzOff_24 = (xzOff_24 * xzOff_24);
					  highp vec3 tmpvar_32;
					  tmpvar_32.xz = vec2(0.0, 0.0);
					  tmpvar_32.y = (((_V_CW_Bend.x * xzOff_24.x) + (_V_CW_Bend.z * xzOff_24.y)) * 0.001);
					  v0_27 = (tmpvar_28 + tmpvar_32);
					  highp vec2 tmpvar_33;
					  tmpvar_33.x = float((tmpvar_29.z >= 0.0));
					  tmpvar_33.y = float((tmpvar_29.x >= 0.0));
					  xzOff_24 = (max (vec2(0.0, 0.0), (
					    abs(tmpvar_29.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_33 * 2.0) - 1.0));
					  xzOff_24 = (xzOff_24 * xzOff_24);
					  v1_26.y = (tmpvar_29.y + ((
					    (_V_CW_Bend.x * xzOff_24.x)
					   + 
					    (_V_CW_Bend.z * xzOff_24.y)
					  ) * 0.001));
					  highp vec2 tmpvar_34;
					  tmpvar_34.x = float((tmpvar_30.z >= 0.0));
					  tmpvar_34.y = float((tmpvar_30.x >= 0.0));
					  xzOff_24 = (max (vec2(0.0, 0.0), (
					    abs(tmpvar_30.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_34 * 2.0) - 1.0));
					  xzOff_24 = (xzOff_24 * xzOff_24);
					  v2_25.y = (tmpvar_30.y + ((
					    (_V_CW_Bend.x * xzOff_24.x)
					   + 
					    (_V_CW_Bend.z * xzOff_24.y)
					  ) * 0.001));
					  highp mat3 tmpvar_35;
					  tmpvar_35[0] = _World2Object[0].xyz;
					  tmpvar_35[1] = _World2Object[1].xyz;
					  tmpvar_35[2] = _World2Object[2].xyz;
					  vertex_23.xyz = (_glesVertex.xyz + (tmpvar_35 * tmpvar_32));
					  highp mat3 tmpvar_36;
					  tmpvar_36[0] = _World2Object[0].xyz;
					  tmpvar_36[1] = _World2Object[1].xyz;
					  tmpvar_36[2] = _World2Object[2].xyz;
					  highp vec3 a_37;
					  a_37 = (v2_25 - v0_27);
					  highp vec3 b_38;
					  b_38 = (v1_26 - v0_27);
					  highp vec3 tmpvar_39;
					  tmpvar_39 = normalize((tmpvar_36 * normalize(
					    ((a_37.yzx * b_38.zxy) - (a_37.zxy * b_38.yzx))
					  )));
					  tmpvar_2 = (glstate_matrix_mvp * vertex_23);
					  tmpvar_3.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
					  tmpvar_3.zw = ((_glesMultiTexCoord0.xy * _V_CW_SecondaryTex_ST.xy) + _V_CW_SecondaryTex_ST.zw);
					  highp vec4 v_40;
					  v_40.x = tmpvar_6;
					  v_40.y = tmpvar_7;
					  v_40.z = tmpvar_8;
					  v_40.w = tmpvar_9;
					  highp vec4 v_41;
					  v_41.x = tmpvar_11;
					  v_41.y = tmpvar_12;
					  v_41.z = tmpvar_13;
					  v_41.w = tmpvar_14;
					  highp vec4 v_42;
					  v_42.x = tmpvar_16;
					  v_42.y = tmpvar_17;
					  v_42.z = tmpvar_18;
					  v_42.w = tmpvar_19;
					  highp vec3 tmpvar_43;
					  tmpvar_43 = normalize(((
					    (v_40.xyz * tmpvar_39.x)
					   + 
					    (v_41.xyz * tmpvar_39.y)
					  ) + (v_42.xyz * tmpvar_39.z)));
					  worldNormal_1 = tmpvar_43;
					  tmpvar_4 = worldNormal_1;
					  gl_Position = tmpvar_2;
					  xlv_TEXCOORD0 = tmpvar_3;
					  xlv_TEXCOORD1 = tmpvar_4;
					  xlv_TEXCOORD2 = (_Object2World * vertex_23).xyz;
					  xlv_COLOR0 = _glesColor;
					  xlv_TEXCOORD3 = ((tmpvar_2.z * unity_FogParams.z) + unity_FogParams.w);
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform highp vec4 _Time;
					uniform highp vec4 _WorldSpaceLightPos0;
					uniform lowp vec4 _LightColor0;
					uniform sampler2D _LightTexture0;
					uniform mediump mat4 _LightMatrix0;
					uniform sampler2D _MainTex;
					uniform lowp vec4 _Color;
					uniform lowp vec2 _V_CW_MainTex_Scroll;
					uniform sampler2D _V_CW_SecondaryTex;
					uniform lowp vec2 _V_CW_SecondaryTex_Scroll;
					uniform lowp float _V_CW_SecondaryTex_Blend;
					varying highp vec4 xlv_TEXCOORD0;
					varying mediump vec3 xlv_TEXCOORD1;
					varying highp vec3 xlv_TEXCOORD2;
					varying lowp vec4 xlv_COLOR0;
					varying highp float xlv_TEXCOORD3;
					void main ()
					{
					  mediump vec3 tmpvar_1;
					  mediump vec3 tmpvar_2;
					  lowp vec4 c_3;
					  lowp vec3 tmpvar_4;
					  lowp vec3 lightDir_5;
					  highp vec4 tmpvar_6;
					  highp vec3 tmpvar_7;
					  tmpvar_7 = normalize((_WorldSpaceLightPos0.xyz - xlv_TEXCOORD2));
					  lightDir_5 = tmpvar_7;
					  tmpvar_6 = xlv_COLOR0;
					  tmpvar_4 = xlv_TEXCOORD1;
					  lowp float vBlend_8;
					  highp vec2 P_9;
					  P_9 = (xlv_TEXCOORD0.xy + (_V_CW_MainTex_Scroll * _Time.x));
					  highp float tmpvar_10;
					  tmpvar_10 = clamp ((_V_CW_SecondaryTex_Blend + tmpvar_6.w), 0.0, 1.0);
					  vBlend_8 = tmpvar_10;
					  highp vec2 P_11;
					  P_11 = (xlv_TEXCOORD0.zw + (_V_CW_SecondaryTex_Scroll * _Time.x));
					  lowp vec4 tmpvar_12;
					  tmpvar_12 = mix (texture2D (_MainTex, P_9), texture2D (_V_CW_SecondaryTex, P_11), vec4(vBlend_8));
					  highp vec4 tmpvar_13;
					  tmpvar_13.w = 1.0;
					  tmpvar_13.xyz = xlv_TEXCOORD2;
					  highp vec3 tmpvar_14;
					  tmpvar_14 = (_LightMatrix0 * tmpvar_13).xyz;
					  highp float tmpvar_15;
					  tmpvar_15 = dot (tmpvar_14, tmpvar_14);
					  lowp float tmpvar_16;
					  tmpvar_16 = texture2D (_LightTexture0, vec2(tmpvar_15)).w;
					  tmpvar_1 = _LightColor0.xyz;
					  tmpvar_2 = lightDir_5;
					  tmpvar_1 = (tmpvar_1 * tmpvar_16);
					  lowp vec4 c_17;
					  lowp vec4 c_18;
					  lowp float diff_19;
					  mediump float tmpvar_20;
					  tmpvar_20 = max (0.0, dot (tmpvar_4, tmpvar_2));
					  diff_19 = tmpvar_20;
					  c_18.xyz = ((tmpvar_12.xyz * _Color.xyz) * (tmpvar_1 * diff_19));
					  c_18.w = (tmpvar_12.w * _Color.w);
					  c_17.w = c_18.w;
					  c_17.xyz = c_18.xyz;
					  c_3.w = c_17.w;
					  highp float tmpvar_21;
					  tmpvar_21 = clamp (xlv_TEXCOORD3, 0.0, 1.0);
					  c_3.xyz = (c_18.xyz * vec3(tmpvar_21));
					  gl_FragData[0] = c_3;
					}
					
					
					#endif"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "FOG_LINEAR" "V_CW_REFLECTIVE_OFF" "V_CW_VERTEX_COLOR_OFF" "_EMISSION_OFF" "V_CW_RIM_OFF" "_NORMALMAP_OFF" "V_CW_SPECULAR_OFF" }
					"!!GLES
					#version 100
					
					#ifdef VERTEX
					attribute vec4 _glesTANGENT;
					attribute vec4 _glesVertex;
					attribute vec4 _glesColor;
					attribute vec3 _glesNormal;
					attribute vec4 _glesMultiTexCoord0;
					uniform highp mat4 glstate_matrix_mvp;
					uniform highp mat4 _Object2World;
					uniform highp mat4 _World2Object;
					uniform highp vec4 unity_FogParams;
					uniform highp vec3 _V_CW_Bend;
					uniform highp vec3 _V_CW_Bias;
					uniform highp vec4 _V_CW_PivotPoint_Position;
					uniform highp vec4 _MainTex_ST;
					uniform highp vec4 _V_CW_SecondaryTex_ST;
					varying highp vec4 xlv_TEXCOORD0;
					varying mediump vec3 xlv_TEXCOORD1;
					varying highp vec3 xlv_TEXCOORD2;
					varying lowp vec4 xlv_COLOR0;
					varying highp float xlv_TEXCOORD3;
					void main ()
					{
					  lowp vec3 worldNormal_1;
					  highp vec4 tmpvar_2;
					  highp vec4 tmpvar_3;
					  mediump vec3 tmpvar_4;
					  highp vec4 v_5;
					  highp float tmpvar_6;
					  tmpvar_6 = _World2Object[0].x;
					  v_5.x = tmpvar_6;
					  highp float tmpvar_7;
					  tmpvar_7 = _World2Object[1].x;
					  v_5.y = tmpvar_7;
					  highp float tmpvar_8;
					  tmpvar_8 = _World2Object[2].x;
					  v_5.z = tmpvar_8;
					  highp float tmpvar_9;
					  tmpvar_9 = _World2Object[3].x;
					  v_5.w = tmpvar_9;
					  highp vec4 v_10;
					  highp float tmpvar_11;
					  tmpvar_11 = _World2Object[0].y;
					  v_10.x = tmpvar_11;
					  highp float tmpvar_12;
					  tmpvar_12 = _World2Object[1].y;
					  v_10.y = tmpvar_12;
					  highp float tmpvar_13;
					  tmpvar_13 = _World2Object[2].y;
					  v_10.z = tmpvar_13;
					  highp float tmpvar_14;
					  tmpvar_14 = _World2Object[3].y;
					  v_10.w = tmpvar_14;
					  highp vec4 v_15;
					  highp float tmpvar_16;
					  tmpvar_16 = _World2Object[0].z;
					  v_15.x = tmpvar_16;
					  highp float tmpvar_17;
					  tmpvar_17 = _World2Object[1].z;
					  v_15.y = tmpvar_17;
					  highp float tmpvar_18;
					  tmpvar_18 = _World2Object[2].z;
					  v_15.z = tmpvar_18;
					  highp float tmpvar_19;
					  tmpvar_19 = _World2Object[3].z;
					  v_15.w = tmpvar_19;
					  highp vec3 tmpvar_20;
					  tmpvar_20 = normalize(((
					    (v_5.xyz * _glesNormal.x)
					   + 
					    (v_10.xyz * _glesNormal.y)
					  ) + (v_15.xyz * _glesNormal.z)));
					  highp mat3 tmpvar_21;
					  tmpvar_21[0] = _Object2World[0].xyz;
					  tmpvar_21[1] = _Object2World[1].xyz;
					  tmpvar_21[2] = _Object2World[2].xyz;
					  highp vec3 tmpvar_22;
					  tmpvar_22 = normalize((tmpvar_21 * _glesTANGENT.xyz));
					  highp vec4 vertex_23;
					  vertex_23.w = _glesVertex.w;
					  highp vec2 xzOff_24;
					  highp vec3 v2_25;
					  highp vec3 v1_26;
					  highp vec3 v0_27;
					  highp vec3 tmpvar_28;
					  tmpvar_28 = ((_Object2World * _glesVertex).xyz - _V_CW_PivotPoint_Position.xyz);
					  highp vec3 tmpvar_29;
					  tmpvar_29 = (tmpvar_28 + tmpvar_22);
					  v1_26.xz = tmpvar_29.xz;
					  highp vec3 tmpvar_30;
					  tmpvar_30 = (tmpvar_28 - ((tmpvar_20.yzx * tmpvar_22.zxy) - (tmpvar_20.zxy * tmpvar_22.yzx)));
					  v2_25.xz = tmpvar_30.xz;
					  highp vec2 tmpvar_31;
					  tmpvar_31.x = float((tmpvar_28.z >= 0.0));
					  tmpvar_31.y = float((tmpvar_28.x >= 0.0));
					  xzOff_24 = (max (vec2(0.0, 0.0), (
					    abs(tmpvar_28.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_31 * 2.0) - 1.0));
					  xzOff_24 = (xzOff_24 * xzOff_24);
					  highp vec3 tmpvar_32;
					  tmpvar_32.xz = vec2(0.0, 0.0);
					  tmpvar_32.y = (((_V_CW_Bend.x * xzOff_24.x) + (_V_CW_Bend.z * xzOff_24.y)) * 0.001);
					  v0_27 = (tmpvar_28 + tmpvar_32);
					  highp vec2 tmpvar_33;
					  tmpvar_33.x = float((tmpvar_29.z >= 0.0));
					  tmpvar_33.y = float((tmpvar_29.x >= 0.0));
					  xzOff_24 = (max (vec2(0.0, 0.0), (
					    abs(tmpvar_29.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_33 * 2.0) - 1.0));
					  xzOff_24 = (xzOff_24 * xzOff_24);
					  v1_26.y = (tmpvar_29.y + ((
					    (_V_CW_Bend.x * xzOff_24.x)
					   + 
					    (_V_CW_Bend.z * xzOff_24.y)
					  ) * 0.001));
					  highp vec2 tmpvar_34;
					  tmpvar_34.x = float((tmpvar_30.z >= 0.0));
					  tmpvar_34.y = float((tmpvar_30.x >= 0.0));
					  xzOff_24 = (max (vec2(0.0, 0.0), (
					    abs(tmpvar_30.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_34 * 2.0) - 1.0));
					  xzOff_24 = (xzOff_24 * xzOff_24);
					  v2_25.y = (tmpvar_30.y + ((
					    (_V_CW_Bend.x * xzOff_24.x)
					   + 
					    (_V_CW_Bend.z * xzOff_24.y)
					  ) * 0.001));
					  highp mat3 tmpvar_35;
					  tmpvar_35[0] = _World2Object[0].xyz;
					  tmpvar_35[1] = _World2Object[1].xyz;
					  tmpvar_35[2] = _World2Object[2].xyz;
					  vertex_23.xyz = (_glesVertex.xyz + (tmpvar_35 * tmpvar_32));
					  highp mat3 tmpvar_36;
					  tmpvar_36[0] = _World2Object[0].xyz;
					  tmpvar_36[1] = _World2Object[1].xyz;
					  tmpvar_36[2] = _World2Object[2].xyz;
					  highp vec3 a_37;
					  a_37 = (v2_25 - v0_27);
					  highp vec3 b_38;
					  b_38 = (v1_26 - v0_27);
					  highp vec3 tmpvar_39;
					  tmpvar_39 = normalize((tmpvar_36 * normalize(
					    ((a_37.yzx * b_38.zxy) - (a_37.zxy * b_38.yzx))
					  )));
					  tmpvar_2 = (glstate_matrix_mvp * vertex_23);
					  tmpvar_3.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
					  tmpvar_3.zw = ((_glesMultiTexCoord0.xy * _V_CW_SecondaryTex_ST.xy) + _V_CW_SecondaryTex_ST.zw);
					  highp vec4 v_40;
					  v_40.x = tmpvar_6;
					  v_40.y = tmpvar_7;
					  v_40.z = tmpvar_8;
					  v_40.w = tmpvar_9;
					  highp vec4 v_41;
					  v_41.x = tmpvar_11;
					  v_41.y = tmpvar_12;
					  v_41.z = tmpvar_13;
					  v_41.w = tmpvar_14;
					  highp vec4 v_42;
					  v_42.x = tmpvar_16;
					  v_42.y = tmpvar_17;
					  v_42.z = tmpvar_18;
					  v_42.w = tmpvar_19;
					  highp vec3 tmpvar_43;
					  tmpvar_43 = normalize(((
					    (v_40.xyz * tmpvar_39.x)
					   + 
					    (v_41.xyz * tmpvar_39.y)
					  ) + (v_42.xyz * tmpvar_39.z)));
					  worldNormal_1 = tmpvar_43;
					  tmpvar_4 = worldNormal_1;
					  gl_Position = tmpvar_2;
					  xlv_TEXCOORD0 = tmpvar_3;
					  xlv_TEXCOORD1 = tmpvar_4;
					  xlv_TEXCOORD2 = (_Object2World * vertex_23).xyz;
					  xlv_COLOR0 = _glesColor;
					  xlv_TEXCOORD3 = ((tmpvar_2.z * unity_FogParams.z) + unity_FogParams.w);
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform highp vec4 _Time;
					uniform mediump vec4 _WorldSpaceLightPos0;
					uniform lowp vec4 _LightColor0;
					uniform sampler2D _MainTex;
					uniform lowp vec4 _Color;
					uniform lowp vec2 _V_CW_MainTex_Scroll;
					uniform sampler2D _V_CW_SecondaryTex;
					uniform lowp vec2 _V_CW_SecondaryTex_Scroll;
					uniform lowp float _V_CW_SecondaryTex_Blend;
					varying highp vec4 xlv_TEXCOORD0;
					varying mediump vec3 xlv_TEXCOORD1;
					varying lowp vec4 xlv_COLOR0;
					varying highp float xlv_TEXCOORD3;
					void main ()
					{
					  mediump vec3 tmpvar_1;
					  mediump vec3 tmpvar_2;
					  lowp vec4 c_3;
					  lowp vec3 tmpvar_4;
					  lowp vec3 lightDir_5;
					  highp vec4 tmpvar_6;
					  mediump vec3 tmpvar_7;
					  tmpvar_7 = _WorldSpaceLightPos0.xyz;
					  lightDir_5 = tmpvar_7;
					  tmpvar_6 = xlv_COLOR0;
					  tmpvar_4 = xlv_TEXCOORD1;
					  lowp float vBlend_8;
					  highp vec2 P_9;
					  P_9 = (xlv_TEXCOORD0.xy + (_V_CW_MainTex_Scroll * _Time.x));
					  highp float tmpvar_10;
					  tmpvar_10 = clamp ((_V_CW_SecondaryTex_Blend + tmpvar_6.w), 0.0, 1.0);
					  vBlend_8 = tmpvar_10;
					  highp vec2 P_11;
					  P_11 = (xlv_TEXCOORD0.zw + (_V_CW_SecondaryTex_Scroll * _Time.x));
					  lowp vec4 tmpvar_12;
					  tmpvar_12 = mix (texture2D (_MainTex, P_9), texture2D (_V_CW_SecondaryTex, P_11), vec4(vBlend_8));
					  tmpvar_1 = _LightColor0.xyz;
					  tmpvar_2 = lightDir_5;
					  lowp vec4 c_13;
					  lowp vec4 c_14;
					  lowp float diff_15;
					  mediump float tmpvar_16;
					  tmpvar_16 = max (0.0, dot (tmpvar_4, tmpvar_2));
					  diff_15 = tmpvar_16;
					  c_14.xyz = ((tmpvar_12.xyz * _Color.xyz) * (tmpvar_1 * diff_15));
					  c_14.w = (tmpvar_12.w * _Color.w);
					  c_13.w = c_14.w;
					  c_13.xyz = c_14.xyz;
					  c_3.w = c_13.w;
					  highp float tmpvar_17;
					  tmpvar_17 = clamp (xlv_TEXCOORD3, 0.0, 1.0);
					  c_3.xyz = (c_14.xyz * vec3(tmpvar_17));
					  gl_FragData[0] = c_3;
					}
					
					
					#endif"
}
SubProgram "gles " {
Keywords { "SPOT" "FOG_LINEAR" "V_CW_REFLECTIVE_OFF" "V_CW_VERTEX_COLOR_OFF" "_EMISSION_OFF" "V_CW_RIM_OFF" "_NORMALMAP_OFF" "V_CW_SPECULAR_OFF" }
					"!!GLES
					#version 100
					
					#ifdef VERTEX
					attribute vec4 _glesTANGENT;
					attribute vec4 _glesVertex;
					attribute vec4 _glesColor;
					attribute vec3 _glesNormal;
					attribute vec4 _glesMultiTexCoord0;
					uniform highp mat4 glstate_matrix_mvp;
					uniform highp mat4 _Object2World;
					uniform highp mat4 _World2Object;
					uniform highp vec4 unity_FogParams;
					uniform highp vec3 _V_CW_Bend;
					uniform highp vec3 _V_CW_Bias;
					uniform highp vec4 _V_CW_PivotPoint_Position;
					uniform highp vec4 _MainTex_ST;
					uniform highp vec4 _V_CW_SecondaryTex_ST;
					varying highp vec4 xlv_TEXCOORD0;
					varying mediump vec3 xlv_TEXCOORD1;
					varying highp vec3 xlv_TEXCOORD2;
					varying lowp vec4 xlv_COLOR0;
					varying highp float xlv_TEXCOORD3;
					void main ()
					{
					  lowp vec3 worldNormal_1;
					  highp vec4 tmpvar_2;
					  highp vec4 tmpvar_3;
					  mediump vec3 tmpvar_4;
					  highp vec4 v_5;
					  highp float tmpvar_6;
					  tmpvar_6 = _World2Object[0].x;
					  v_5.x = tmpvar_6;
					  highp float tmpvar_7;
					  tmpvar_7 = _World2Object[1].x;
					  v_5.y = tmpvar_7;
					  highp float tmpvar_8;
					  tmpvar_8 = _World2Object[2].x;
					  v_5.z = tmpvar_8;
					  highp float tmpvar_9;
					  tmpvar_9 = _World2Object[3].x;
					  v_5.w = tmpvar_9;
					  highp vec4 v_10;
					  highp float tmpvar_11;
					  tmpvar_11 = _World2Object[0].y;
					  v_10.x = tmpvar_11;
					  highp float tmpvar_12;
					  tmpvar_12 = _World2Object[1].y;
					  v_10.y = tmpvar_12;
					  highp float tmpvar_13;
					  tmpvar_13 = _World2Object[2].y;
					  v_10.z = tmpvar_13;
					  highp float tmpvar_14;
					  tmpvar_14 = _World2Object[3].y;
					  v_10.w = tmpvar_14;
					  highp vec4 v_15;
					  highp float tmpvar_16;
					  tmpvar_16 = _World2Object[0].z;
					  v_15.x = tmpvar_16;
					  highp float tmpvar_17;
					  tmpvar_17 = _World2Object[1].z;
					  v_15.y = tmpvar_17;
					  highp float tmpvar_18;
					  tmpvar_18 = _World2Object[2].z;
					  v_15.z = tmpvar_18;
					  highp float tmpvar_19;
					  tmpvar_19 = _World2Object[3].z;
					  v_15.w = tmpvar_19;
					  highp vec3 tmpvar_20;
					  tmpvar_20 = normalize(((
					    (v_5.xyz * _glesNormal.x)
					   + 
					    (v_10.xyz * _glesNormal.y)
					  ) + (v_15.xyz * _glesNormal.z)));
					  highp mat3 tmpvar_21;
					  tmpvar_21[0] = _Object2World[0].xyz;
					  tmpvar_21[1] = _Object2World[1].xyz;
					  tmpvar_21[2] = _Object2World[2].xyz;
					  highp vec3 tmpvar_22;
					  tmpvar_22 = normalize((tmpvar_21 * _glesTANGENT.xyz));
					  highp vec4 vertex_23;
					  vertex_23.w = _glesVertex.w;
					  highp vec2 xzOff_24;
					  highp vec3 v2_25;
					  highp vec3 v1_26;
					  highp vec3 v0_27;
					  highp vec3 tmpvar_28;
					  tmpvar_28 = ((_Object2World * _glesVertex).xyz - _V_CW_PivotPoint_Position.xyz);
					  highp vec3 tmpvar_29;
					  tmpvar_29 = (tmpvar_28 + tmpvar_22);
					  v1_26.xz = tmpvar_29.xz;
					  highp vec3 tmpvar_30;
					  tmpvar_30 = (tmpvar_28 - ((tmpvar_20.yzx * tmpvar_22.zxy) - (tmpvar_20.zxy * tmpvar_22.yzx)));
					  v2_25.xz = tmpvar_30.xz;
					  highp vec2 tmpvar_31;
					  tmpvar_31.x = float((tmpvar_28.z >= 0.0));
					  tmpvar_31.y = float((tmpvar_28.x >= 0.0));
					  xzOff_24 = (max (vec2(0.0, 0.0), (
					    abs(tmpvar_28.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_31 * 2.0) - 1.0));
					  xzOff_24 = (xzOff_24 * xzOff_24);
					  highp vec3 tmpvar_32;
					  tmpvar_32.xz = vec2(0.0, 0.0);
					  tmpvar_32.y = (((_V_CW_Bend.x * xzOff_24.x) + (_V_CW_Bend.z * xzOff_24.y)) * 0.001);
					  v0_27 = (tmpvar_28 + tmpvar_32);
					  highp vec2 tmpvar_33;
					  tmpvar_33.x = float((tmpvar_29.z >= 0.0));
					  tmpvar_33.y = float((tmpvar_29.x >= 0.0));
					  xzOff_24 = (max (vec2(0.0, 0.0), (
					    abs(tmpvar_29.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_33 * 2.0) - 1.0));
					  xzOff_24 = (xzOff_24 * xzOff_24);
					  v1_26.y = (tmpvar_29.y + ((
					    (_V_CW_Bend.x * xzOff_24.x)
					   + 
					    (_V_CW_Bend.z * xzOff_24.y)
					  ) * 0.001));
					  highp vec2 tmpvar_34;
					  tmpvar_34.x = float((tmpvar_30.z >= 0.0));
					  tmpvar_34.y = float((tmpvar_30.x >= 0.0));
					  xzOff_24 = (max (vec2(0.0, 0.0), (
					    abs(tmpvar_30.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_34 * 2.0) - 1.0));
					  xzOff_24 = (xzOff_24 * xzOff_24);
					  v2_25.y = (tmpvar_30.y + ((
					    (_V_CW_Bend.x * xzOff_24.x)
					   + 
					    (_V_CW_Bend.z * xzOff_24.y)
					  ) * 0.001));
					  highp mat3 tmpvar_35;
					  tmpvar_35[0] = _World2Object[0].xyz;
					  tmpvar_35[1] = _World2Object[1].xyz;
					  tmpvar_35[2] = _World2Object[2].xyz;
					  vertex_23.xyz = (_glesVertex.xyz + (tmpvar_35 * tmpvar_32));
					  highp mat3 tmpvar_36;
					  tmpvar_36[0] = _World2Object[0].xyz;
					  tmpvar_36[1] = _World2Object[1].xyz;
					  tmpvar_36[2] = _World2Object[2].xyz;
					  highp vec3 a_37;
					  a_37 = (v2_25 - v0_27);
					  highp vec3 b_38;
					  b_38 = (v1_26 - v0_27);
					  highp vec3 tmpvar_39;
					  tmpvar_39 = normalize((tmpvar_36 * normalize(
					    ((a_37.yzx * b_38.zxy) - (a_37.zxy * b_38.yzx))
					  )));
					  tmpvar_2 = (glstate_matrix_mvp * vertex_23);
					  tmpvar_3.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
					  tmpvar_3.zw = ((_glesMultiTexCoord0.xy * _V_CW_SecondaryTex_ST.xy) + _V_CW_SecondaryTex_ST.zw);
					  highp vec4 v_40;
					  v_40.x = tmpvar_6;
					  v_40.y = tmpvar_7;
					  v_40.z = tmpvar_8;
					  v_40.w = tmpvar_9;
					  highp vec4 v_41;
					  v_41.x = tmpvar_11;
					  v_41.y = tmpvar_12;
					  v_41.z = tmpvar_13;
					  v_41.w = tmpvar_14;
					  highp vec4 v_42;
					  v_42.x = tmpvar_16;
					  v_42.y = tmpvar_17;
					  v_42.z = tmpvar_18;
					  v_42.w = tmpvar_19;
					  highp vec3 tmpvar_43;
					  tmpvar_43 = normalize(((
					    (v_40.xyz * tmpvar_39.x)
					   + 
					    (v_41.xyz * tmpvar_39.y)
					  ) + (v_42.xyz * tmpvar_39.z)));
					  worldNormal_1 = tmpvar_43;
					  tmpvar_4 = worldNormal_1;
					  gl_Position = tmpvar_2;
					  xlv_TEXCOORD0 = tmpvar_3;
					  xlv_TEXCOORD1 = tmpvar_4;
					  xlv_TEXCOORD2 = (_Object2World * vertex_23).xyz;
					  xlv_COLOR0 = _glesColor;
					  xlv_TEXCOORD3 = ((tmpvar_2.z * unity_FogParams.z) + unity_FogParams.w);
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform highp vec4 _Time;
					uniform highp vec4 _WorldSpaceLightPos0;
					uniform lowp vec4 _LightColor0;
					uniform sampler2D _LightTexture0;
					uniform mediump mat4 _LightMatrix0;
					uniform sampler2D _LightTextureB0;
					uniform sampler2D _MainTex;
					uniform lowp vec4 _Color;
					uniform lowp vec2 _V_CW_MainTex_Scroll;
					uniform sampler2D _V_CW_SecondaryTex;
					uniform lowp vec2 _V_CW_SecondaryTex_Scroll;
					uniform lowp float _V_CW_SecondaryTex_Blend;
					varying highp vec4 xlv_TEXCOORD0;
					varying mediump vec3 xlv_TEXCOORD1;
					varying highp vec3 xlv_TEXCOORD2;
					varying lowp vec4 xlv_COLOR0;
					varying highp float xlv_TEXCOORD3;
					void main ()
					{
					  mediump vec3 tmpvar_1;
					  mediump vec3 tmpvar_2;
					  lowp vec4 c_3;
					  lowp float atten_4;
					  mediump vec4 lightCoord_5;
					  lowp vec3 tmpvar_6;
					  lowp vec3 lightDir_7;
					  highp vec4 tmpvar_8;
					  highp vec3 tmpvar_9;
					  tmpvar_9 = normalize((_WorldSpaceLightPos0.xyz - xlv_TEXCOORD2));
					  lightDir_7 = tmpvar_9;
					  tmpvar_8 = xlv_COLOR0;
					  tmpvar_6 = xlv_TEXCOORD1;
					  lowp float vBlend_10;
					  highp vec2 P_11;
					  P_11 = (xlv_TEXCOORD0.xy + (_V_CW_MainTex_Scroll * _Time.x));
					  highp float tmpvar_12;
					  tmpvar_12 = clamp ((_V_CW_SecondaryTex_Blend + tmpvar_8.w), 0.0, 1.0);
					  vBlend_10 = tmpvar_12;
					  highp vec2 P_13;
					  P_13 = (xlv_TEXCOORD0.zw + (_V_CW_SecondaryTex_Scroll * _Time.x));
					  lowp vec4 tmpvar_14;
					  tmpvar_14 = mix (texture2D (_MainTex, P_11), texture2D (_V_CW_SecondaryTex, P_13), vec4(vBlend_10));
					  highp vec4 tmpvar_15;
					  tmpvar_15.w = 1.0;
					  tmpvar_15.xyz = xlv_TEXCOORD2;
					  highp vec4 tmpvar_16;
					  tmpvar_16 = (_LightMatrix0 * tmpvar_15);
					  lightCoord_5 = tmpvar_16;
					  lowp vec4 tmpvar_17;
					  mediump vec2 P_18;
					  P_18 = ((lightCoord_5.xy / lightCoord_5.w) + 0.5);
					  tmpvar_17 = texture2D (_LightTexture0, P_18);
					  highp vec3 LightCoord_19;
					  LightCoord_19 = lightCoord_5.xyz;
					  highp float tmpvar_20;
					  tmpvar_20 = dot (LightCoord_19, LightCoord_19);
					  lowp vec4 tmpvar_21;
					  tmpvar_21 = texture2D (_LightTextureB0, vec2(tmpvar_20));
					  mediump float tmpvar_22;
					  tmpvar_22 = ((float(
					    (lightCoord_5.z > 0.0)
					  ) * tmpvar_17.w) * tmpvar_21.w);
					  atten_4 = tmpvar_22;
					  tmpvar_1 = _LightColor0.xyz;
					  tmpvar_2 = lightDir_7;
					  tmpvar_1 = (tmpvar_1 * atten_4);
					  lowp vec4 c_23;
					  lowp vec4 c_24;
					  lowp float diff_25;
					  mediump float tmpvar_26;
					  tmpvar_26 = max (0.0, dot (tmpvar_6, tmpvar_2));
					  diff_25 = tmpvar_26;
					  c_24.xyz = ((tmpvar_14.xyz * _Color.xyz) * (tmpvar_1 * diff_25));
					  c_24.w = (tmpvar_14.w * _Color.w);
					  c_23.w = c_24.w;
					  c_23.xyz = c_24.xyz;
					  c_3.w = c_23.w;
					  highp float tmpvar_27;
					  tmpvar_27 = clamp (xlv_TEXCOORD3, 0.0, 1.0);
					  c_3.xyz = (c_24.xyz * vec3(tmpvar_27));
					  gl_FragData[0] = c_3;
					}
					
					
					#endif"
}
SubProgram "gles " {
Keywords { "POINT_COOKIE" "FOG_LINEAR" "V_CW_REFLECTIVE_OFF" "V_CW_VERTEX_COLOR_OFF" "_EMISSION_OFF" "V_CW_RIM_OFF" "_NORMALMAP_OFF" "V_CW_SPECULAR_OFF" }
					"!!GLES
					#version 100
					
					#ifdef VERTEX
					attribute vec4 _glesTANGENT;
					attribute vec4 _glesVertex;
					attribute vec4 _glesColor;
					attribute vec3 _glesNormal;
					attribute vec4 _glesMultiTexCoord0;
					uniform highp mat4 glstate_matrix_mvp;
					uniform highp mat4 _Object2World;
					uniform highp mat4 _World2Object;
					uniform highp vec4 unity_FogParams;
					uniform highp vec3 _V_CW_Bend;
					uniform highp vec3 _V_CW_Bias;
					uniform highp vec4 _V_CW_PivotPoint_Position;
					uniform highp vec4 _MainTex_ST;
					uniform highp vec4 _V_CW_SecondaryTex_ST;
					varying highp vec4 xlv_TEXCOORD0;
					varying mediump vec3 xlv_TEXCOORD1;
					varying highp vec3 xlv_TEXCOORD2;
					varying lowp vec4 xlv_COLOR0;
					varying highp float xlv_TEXCOORD3;
					void main ()
					{
					  lowp vec3 worldNormal_1;
					  highp vec4 tmpvar_2;
					  highp vec4 tmpvar_3;
					  mediump vec3 tmpvar_4;
					  highp vec4 v_5;
					  highp float tmpvar_6;
					  tmpvar_6 = _World2Object[0].x;
					  v_5.x = tmpvar_6;
					  highp float tmpvar_7;
					  tmpvar_7 = _World2Object[1].x;
					  v_5.y = tmpvar_7;
					  highp float tmpvar_8;
					  tmpvar_8 = _World2Object[2].x;
					  v_5.z = tmpvar_8;
					  highp float tmpvar_9;
					  tmpvar_9 = _World2Object[3].x;
					  v_5.w = tmpvar_9;
					  highp vec4 v_10;
					  highp float tmpvar_11;
					  tmpvar_11 = _World2Object[0].y;
					  v_10.x = tmpvar_11;
					  highp float tmpvar_12;
					  tmpvar_12 = _World2Object[1].y;
					  v_10.y = tmpvar_12;
					  highp float tmpvar_13;
					  tmpvar_13 = _World2Object[2].y;
					  v_10.z = tmpvar_13;
					  highp float tmpvar_14;
					  tmpvar_14 = _World2Object[3].y;
					  v_10.w = tmpvar_14;
					  highp vec4 v_15;
					  highp float tmpvar_16;
					  tmpvar_16 = _World2Object[0].z;
					  v_15.x = tmpvar_16;
					  highp float tmpvar_17;
					  tmpvar_17 = _World2Object[1].z;
					  v_15.y = tmpvar_17;
					  highp float tmpvar_18;
					  tmpvar_18 = _World2Object[2].z;
					  v_15.z = tmpvar_18;
					  highp float tmpvar_19;
					  tmpvar_19 = _World2Object[3].z;
					  v_15.w = tmpvar_19;
					  highp vec3 tmpvar_20;
					  tmpvar_20 = normalize(((
					    (v_5.xyz * _glesNormal.x)
					   + 
					    (v_10.xyz * _glesNormal.y)
					  ) + (v_15.xyz * _glesNormal.z)));
					  highp mat3 tmpvar_21;
					  tmpvar_21[0] = _Object2World[0].xyz;
					  tmpvar_21[1] = _Object2World[1].xyz;
					  tmpvar_21[2] = _Object2World[2].xyz;
					  highp vec3 tmpvar_22;
					  tmpvar_22 = normalize((tmpvar_21 * _glesTANGENT.xyz));
					  highp vec4 vertex_23;
					  vertex_23.w = _glesVertex.w;
					  highp vec2 xzOff_24;
					  highp vec3 v2_25;
					  highp vec3 v1_26;
					  highp vec3 v0_27;
					  highp vec3 tmpvar_28;
					  tmpvar_28 = ((_Object2World * _glesVertex).xyz - _V_CW_PivotPoint_Position.xyz);
					  highp vec3 tmpvar_29;
					  tmpvar_29 = (tmpvar_28 + tmpvar_22);
					  v1_26.xz = tmpvar_29.xz;
					  highp vec3 tmpvar_30;
					  tmpvar_30 = (tmpvar_28 - ((tmpvar_20.yzx * tmpvar_22.zxy) - (tmpvar_20.zxy * tmpvar_22.yzx)));
					  v2_25.xz = tmpvar_30.xz;
					  highp vec2 tmpvar_31;
					  tmpvar_31.x = float((tmpvar_28.z >= 0.0));
					  tmpvar_31.y = float((tmpvar_28.x >= 0.0));
					  xzOff_24 = (max (vec2(0.0, 0.0), (
					    abs(tmpvar_28.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_31 * 2.0) - 1.0));
					  xzOff_24 = (xzOff_24 * xzOff_24);
					  highp vec3 tmpvar_32;
					  tmpvar_32.xz = vec2(0.0, 0.0);
					  tmpvar_32.y = (((_V_CW_Bend.x * xzOff_24.x) + (_V_CW_Bend.z * xzOff_24.y)) * 0.001);
					  v0_27 = (tmpvar_28 + tmpvar_32);
					  highp vec2 tmpvar_33;
					  tmpvar_33.x = float((tmpvar_29.z >= 0.0));
					  tmpvar_33.y = float((tmpvar_29.x >= 0.0));
					  xzOff_24 = (max (vec2(0.0, 0.0), (
					    abs(tmpvar_29.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_33 * 2.0) - 1.0));
					  xzOff_24 = (xzOff_24 * xzOff_24);
					  v1_26.y = (tmpvar_29.y + ((
					    (_V_CW_Bend.x * xzOff_24.x)
					   + 
					    (_V_CW_Bend.z * xzOff_24.y)
					  ) * 0.001));
					  highp vec2 tmpvar_34;
					  tmpvar_34.x = float((tmpvar_30.z >= 0.0));
					  tmpvar_34.y = float((tmpvar_30.x >= 0.0));
					  xzOff_24 = (max (vec2(0.0, 0.0), (
					    abs(tmpvar_30.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_34 * 2.0) - 1.0));
					  xzOff_24 = (xzOff_24 * xzOff_24);
					  v2_25.y = (tmpvar_30.y + ((
					    (_V_CW_Bend.x * xzOff_24.x)
					   + 
					    (_V_CW_Bend.z * xzOff_24.y)
					  ) * 0.001));
					  highp mat3 tmpvar_35;
					  tmpvar_35[0] = _World2Object[0].xyz;
					  tmpvar_35[1] = _World2Object[1].xyz;
					  tmpvar_35[2] = _World2Object[2].xyz;
					  vertex_23.xyz = (_glesVertex.xyz + (tmpvar_35 * tmpvar_32));
					  highp mat3 tmpvar_36;
					  tmpvar_36[0] = _World2Object[0].xyz;
					  tmpvar_36[1] = _World2Object[1].xyz;
					  tmpvar_36[2] = _World2Object[2].xyz;
					  highp vec3 a_37;
					  a_37 = (v2_25 - v0_27);
					  highp vec3 b_38;
					  b_38 = (v1_26 - v0_27);
					  highp vec3 tmpvar_39;
					  tmpvar_39 = normalize((tmpvar_36 * normalize(
					    ((a_37.yzx * b_38.zxy) - (a_37.zxy * b_38.yzx))
					  )));
					  tmpvar_2 = (glstate_matrix_mvp * vertex_23);
					  tmpvar_3.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
					  tmpvar_3.zw = ((_glesMultiTexCoord0.xy * _V_CW_SecondaryTex_ST.xy) + _V_CW_SecondaryTex_ST.zw);
					  highp vec4 v_40;
					  v_40.x = tmpvar_6;
					  v_40.y = tmpvar_7;
					  v_40.z = tmpvar_8;
					  v_40.w = tmpvar_9;
					  highp vec4 v_41;
					  v_41.x = tmpvar_11;
					  v_41.y = tmpvar_12;
					  v_41.z = tmpvar_13;
					  v_41.w = tmpvar_14;
					  highp vec4 v_42;
					  v_42.x = tmpvar_16;
					  v_42.y = tmpvar_17;
					  v_42.z = tmpvar_18;
					  v_42.w = tmpvar_19;
					  highp vec3 tmpvar_43;
					  tmpvar_43 = normalize(((
					    (v_40.xyz * tmpvar_39.x)
					   + 
					    (v_41.xyz * tmpvar_39.y)
					  ) + (v_42.xyz * tmpvar_39.z)));
					  worldNormal_1 = tmpvar_43;
					  tmpvar_4 = worldNormal_1;
					  gl_Position = tmpvar_2;
					  xlv_TEXCOORD0 = tmpvar_3;
					  xlv_TEXCOORD1 = tmpvar_4;
					  xlv_TEXCOORD2 = (_Object2World * vertex_23).xyz;
					  xlv_COLOR0 = _glesColor;
					  xlv_TEXCOORD3 = ((tmpvar_2.z * unity_FogParams.z) + unity_FogParams.w);
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform highp vec4 _Time;
					uniform highp vec4 _WorldSpaceLightPos0;
					uniform lowp vec4 _LightColor0;
					uniform lowp samplerCube _LightTexture0;
					uniform mediump mat4 _LightMatrix0;
					uniform sampler2D _LightTextureB0;
					uniform sampler2D _MainTex;
					uniform lowp vec4 _Color;
					uniform lowp vec2 _V_CW_MainTex_Scroll;
					uniform sampler2D _V_CW_SecondaryTex;
					uniform lowp vec2 _V_CW_SecondaryTex_Scroll;
					uniform lowp float _V_CW_SecondaryTex_Blend;
					varying highp vec4 xlv_TEXCOORD0;
					varying mediump vec3 xlv_TEXCOORD1;
					varying highp vec3 xlv_TEXCOORD2;
					varying lowp vec4 xlv_COLOR0;
					varying highp float xlv_TEXCOORD3;
					void main ()
					{
					  mediump vec3 tmpvar_1;
					  mediump vec3 tmpvar_2;
					  lowp vec4 c_3;
					  lowp vec3 tmpvar_4;
					  lowp vec3 lightDir_5;
					  highp vec4 tmpvar_6;
					  highp vec3 tmpvar_7;
					  tmpvar_7 = normalize((_WorldSpaceLightPos0.xyz - xlv_TEXCOORD2));
					  lightDir_5 = tmpvar_7;
					  tmpvar_6 = xlv_COLOR0;
					  tmpvar_4 = xlv_TEXCOORD1;
					  lowp float vBlend_8;
					  highp vec2 P_9;
					  P_9 = (xlv_TEXCOORD0.xy + (_V_CW_MainTex_Scroll * _Time.x));
					  highp float tmpvar_10;
					  tmpvar_10 = clamp ((_V_CW_SecondaryTex_Blend + tmpvar_6.w), 0.0, 1.0);
					  vBlend_8 = tmpvar_10;
					  highp vec2 P_11;
					  P_11 = (xlv_TEXCOORD0.zw + (_V_CW_SecondaryTex_Scroll * _Time.x));
					  lowp vec4 tmpvar_12;
					  tmpvar_12 = mix (texture2D (_MainTex, P_9), texture2D (_V_CW_SecondaryTex, P_11), vec4(vBlend_8));
					  highp vec4 tmpvar_13;
					  tmpvar_13.w = 1.0;
					  tmpvar_13.xyz = xlv_TEXCOORD2;
					  highp vec3 tmpvar_14;
					  tmpvar_14 = (_LightMatrix0 * tmpvar_13).xyz;
					  highp float tmpvar_15;
					  tmpvar_15 = dot (tmpvar_14, tmpvar_14);
					  lowp float tmpvar_16;
					  tmpvar_16 = (texture2D (_LightTextureB0, vec2(tmpvar_15)).w * textureCube (_LightTexture0, tmpvar_14).w);
					  tmpvar_1 = _LightColor0.xyz;
					  tmpvar_2 = lightDir_5;
					  tmpvar_1 = (tmpvar_1 * tmpvar_16);
					  lowp vec4 c_17;
					  lowp vec4 c_18;
					  lowp float diff_19;
					  mediump float tmpvar_20;
					  tmpvar_20 = max (0.0, dot (tmpvar_4, tmpvar_2));
					  diff_19 = tmpvar_20;
					  c_18.xyz = ((tmpvar_12.xyz * _Color.xyz) * (tmpvar_1 * diff_19));
					  c_18.w = (tmpvar_12.w * _Color.w);
					  c_17.w = c_18.w;
					  c_17.xyz = c_18.xyz;
					  c_3.w = c_17.w;
					  highp float tmpvar_21;
					  tmpvar_21 = clamp (xlv_TEXCOORD3, 0.0, 1.0);
					  c_3.xyz = (c_18.xyz * vec3(tmpvar_21));
					  gl_FragData[0] = c_3;
					}
					
					
					#endif"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL_COOKIE" "FOG_LINEAR" "V_CW_REFLECTIVE_OFF" "V_CW_VERTEX_COLOR_OFF" "_EMISSION_OFF" "V_CW_RIM_OFF" "_NORMALMAP_OFF" "V_CW_SPECULAR_OFF" }
					"!!GLES
					#version 100
					
					#ifdef VERTEX
					attribute vec4 _glesTANGENT;
					attribute vec4 _glesVertex;
					attribute vec4 _glesColor;
					attribute vec3 _glesNormal;
					attribute vec4 _glesMultiTexCoord0;
					uniform highp mat4 glstate_matrix_mvp;
					uniform highp mat4 _Object2World;
					uniform highp mat4 _World2Object;
					uniform highp vec4 unity_FogParams;
					uniform highp vec3 _V_CW_Bend;
					uniform highp vec3 _V_CW_Bias;
					uniform highp vec4 _V_CW_PivotPoint_Position;
					uniform highp vec4 _MainTex_ST;
					uniform highp vec4 _V_CW_SecondaryTex_ST;
					varying highp vec4 xlv_TEXCOORD0;
					varying mediump vec3 xlv_TEXCOORD1;
					varying highp vec3 xlv_TEXCOORD2;
					varying lowp vec4 xlv_COLOR0;
					varying highp float xlv_TEXCOORD3;
					void main ()
					{
					  lowp vec3 worldNormal_1;
					  highp vec4 tmpvar_2;
					  highp vec4 tmpvar_3;
					  mediump vec3 tmpvar_4;
					  highp vec4 v_5;
					  highp float tmpvar_6;
					  tmpvar_6 = _World2Object[0].x;
					  v_5.x = tmpvar_6;
					  highp float tmpvar_7;
					  tmpvar_7 = _World2Object[1].x;
					  v_5.y = tmpvar_7;
					  highp float tmpvar_8;
					  tmpvar_8 = _World2Object[2].x;
					  v_5.z = tmpvar_8;
					  highp float tmpvar_9;
					  tmpvar_9 = _World2Object[3].x;
					  v_5.w = tmpvar_9;
					  highp vec4 v_10;
					  highp float tmpvar_11;
					  tmpvar_11 = _World2Object[0].y;
					  v_10.x = tmpvar_11;
					  highp float tmpvar_12;
					  tmpvar_12 = _World2Object[1].y;
					  v_10.y = tmpvar_12;
					  highp float tmpvar_13;
					  tmpvar_13 = _World2Object[2].y;
					  v_10.z = tmpvar_13;
					  highp float tmpvar_14;
					  tmpvar_14 = _World2Object[3].y;
					  v_10.w = tmpvar_14;
					  highp vec4 v_15;
					  highp float tmpvar_16;
					  tmpvar_16 = _World2Object[0].z;
					  v_15.x = tmpvar_16;
					  highp float tmpvar_17;
					  tmpvar_17 = _World2Object[1].z;
					  v_15.y = tmpvar_17;
					  highp float tmpvar_18;
					  tmpvar_18 = _World2Object[2].z;
					  v_15.z = tmpvar_18;
					  highp float tmpvar_19;
					  tmpvar_19 = _World2Object[3].z;
					  v_15.w = tmpvar_19;
					  highp vec3 tmpvar_20;
					  tmpvar_20 = normalize(((
					    (v_5.xyz * _glesNormal.x)
					   + 
					    (v_10.xyz * _glesNormal.y)
					  ) + (v_15.xyz * _glesNormal.z)));
					  highp mat3 tmpvar_21;
					  tmpvar_21[0] = _Object2World[0].xyz;
					  tmpvar_21[1] = _Object2World[1].xyz;
					  tmpvar_21[2] = _Object2World[2].xyz;
					  highp vec3 tmpvar_22;
					  tmpvar_22 = normalize((tmpvar_21 * _glesTANGENT.xyz));
					  highp vec4 vertex_23;
					  vertex_23.w = _glesVertex.w;
					  highp vec2 xzOff_24;
					  highp vec3 v2_25;
					  highp vec3 v1_26;
					  highp vec3 v0_27;
					  highp vec3 tmpvar_28;
					  tmpvar_28 = ((_Object2World * _glesVertex).xyz - _V_CW_PivotPoint_Position.xyz);
					  highp vec3 tmpvar_29;
					  tmpvar_29 = (tmpvar_28 + tmpvar_22);
					  v1_26.xz = tmpvar_29.xz;
					  highp vec3 tmpvar_30;
					  tmpvar_30 = (tmpvar_28 - ((tmpvar_20.yzx * tmpvar_22.zxy) - (tmpvar_20.zxy * tmpvar_22.yzx)));
					  v2_25.xz = tmpvar_30.xz;
					  highp vec2 tmpvar_31;
					  tmpvar_31.x = float((tmpvar_28.z >= 0.0));
					  tmpvar_31.y = float((tmpvar_28.x >= 0.0));
					  xzOff_24 = (max (vec2(0.0, 0.0), (
					    abs(tmpvar_28.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_31 * 2.0) - 1.0));
					  xzOff_24 = (xzOff_24 * xzOff_24);
					  highp vec3 tmpvar_32;
					  tmpvar_32.xz = vec2(0.0, 0.0);
					  tmpvar_32.y = (((_V_CW_Bend.x * xzOff_24.x) + (_V_CW_Bend.z * xzOff_24.y)) * 0.001);
					  v0_27 = (tmpvar_28 + tmpvar_32);
					  highp vec2 tmpvar_33;
					  tmpvar_33.x = float((tmpvar_29.z >= 0.0));
					  tmpvar_33.y = float((tmpvar_29.x >= 0.0));
					  xzOff_24 = (max (vec2(0.0, 0.0), (
					    abs(tmpvar_29.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_33 * 2.0) - 1.0));
					  xzOff_24 = (xzOff_24 * xzOff_24);
					  v1_26.y = (tmpvar_29.y + ((
					    (_V_CW_Bend.x * xzOff_24.x)
					   + 
					    (_V_CW_Bend.z * xzOff_24.y)
					  ) * 0.001));
					  highp vec2 tmpvar_34;
					  tmpvar_34.x = float((tmpvar_30.z >= 0.0));
					  tmpvar_34.y = float((tmpvar_30.x >= 0.0));
					  xzOff_24 = (max (vec2(0.0, 0.0), (
					    abs(tmpvar_30.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_34 * 2.0) - 1.0));
					  xzOff_24 = (xzOff_24 * xzOff_24);
					  v2_25.y = (tmpvar_30.y + ((
					    (_V_CW_Bend.x * xzOff_24.x)
					   + 
					    (_V_CW_Bend.z * xzOff_24.y)
					  ) * 0.001));
					  highp mat3 tmpvar_35;
					  tmpvar_35[0] = _World2Object[0].xyz;
					  tmpvar_35[1] = _World2Object[1].xyz;
					  tmpvar_35[2] = _World2Object[2].xyz;
					  vertex_23.xyz = (_glesVertex.xyz + (tmpvar_35 * tmpvar_32));
					  highp mat3 tmpvar_36;
					  tmpvar_36[0] = _World2Object[0].xyz;
					  tmpvar_36[1] = _World2Object[1].xyz;
					  tmpvar_36[2] = _World2Object[2].xyz;
					  highp vec3 a_37;
					  a_37 = (v2_25 - v0_27);
					  highp vec3 b_38;
					  b_38 = (v1_26 - v0_27);
					  highp vec3 tmpvar_39;
					  tmpvar_39 = normalize((tmpvar_36 * normalize(
					    ((a_37.yzx * b_38.zxy) - (a_37.zxy * b_38.yzx))
					  )));
					  tmpvar_2 = (glstate_matrix_mvp * vertex_23);
					  tmpvar_3.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
					  tmpvar_3.zw = ((_glesMultiTexCoord0.xy * _V_CW_SecondaryTex_ST.xy) + _V_CW_SecondaryTex_ST.zw);
					  highp vec4 v_40;
					  v_40.x = tmpvar_6;
					  v_40.y = tmpvar_7;
					  v_40.z = tmpvar_8;
					  v_40.w = tmpvar_9;
					  highp vec4 v_41;
					  v_41.x = tmpvar_11;
					  v_41.y = tmpvar_12;
					  v_41.z = tmpvar_13;
					  v_41.w = tmpvar_14;
					  highp vec4 v_42;
					  v_42.x = tmpvar_16;
					  v_42.y = tmpvar_17;
					  v_42.z = tmpvar_18;
					  v_42.w = tmpvar_19;
					  highp vec3 tmpvar_43;
					  tmpvar_43 = normalize(((
					    (v_40.xyz * tmpvar_39.x)
					   + 
					    (v_41.xyz * tmpvar_39.y)
					  ) + (v_42.xyz * tmpvar_39.z)));
					  worldNormal_1 = tmpvar_43;
					  tmpvar_4 = worldNormal_1;
					  gl_Position = tmpvar_2;
					  xlv_TEXCOORD0 = tmpvar_3;
					  xlv_TEXCOORD1 = tmpvar_4;
					  xlv_TEXCOORD2 = (_Object2World * vertex_23).xyz;
					  xlv_COLOR0 = _glesColor;
					  xlv_TEXCOORD3 = ((tmpvar_2.z * unity_FogParams.z) + unity_FogParams.w);
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform highp vec4 _Time;
					uniform mediump vec4 _WorldSpaceLightPos0;
					uniform lowp vec4 _LightColor0;
					uniform sampler2D _LightTexture0;
					uniform mediump mat4 _LightMatrix0;
					uniform sampler2D _MainTex;
					uniform lowp vec4 _Color;
					uniform lowp vec2 _V_CW_MainTex_Scroll;
					uniform sampler2D _V_CW_SecondaryTex;
					uniform lowp vec2 _V_CW_SecondaryTex_Scroll;
					uniform lowp float _V_CW_SecondaryTex_Blend;
					varying highp vec4 xlv_TEXCOORD0;
					varying mediump vec3 xlv_TEXCOORD1;
					varying highp vec3 xlv_TEXCOORD2;
					varying lowp vec4 xlv_COLOR0;
					varying highp float xlv_TEXCOORD3;
					void main ()
					{
					  mediump vec3 tmpvar_1;
					  mediump vec3 tmpvar_2;
					  lowp vec4 c_3;
					  lowp vec3 tmpvar_4;
					  lowp vec3 lightDir_5;
					  highp vec4 tmpvar_6;
					  mediump vec3 tmpvar_7;
					  tmpvar_7 = _WorldSpaceLightPos0.xyz;
					  lightDir_5 = tmpvar_7;
					  tmpvar_6 = xlv_COLOR0;
					  tmpvar_4 = xlv_TEXCOORD1;
					  lowp float vBlend_8;
					  highp vec2 P_9;
					  P_9 = (xlv_TEXCOORD0.xy + (_V_CW_MainTex_Scroll * _Time.x));
					  highp float tmpvar_10;
					  tmpvar_10 = clamp ((_V_CW_SecondaryTex_Blend + tmpvar_6.w), 0.0, 1.0);
					  vBlend_8 = tmpvar_10;
					  highp vec2 P_11;
					  P_11 = (xlv_TEXCOORD0.zw + (_V_CW_SecondaryTex_Scroll * _Time.x));
					  lowp vec4 tmpvar_12;
					  tmpvar_12 = mix (texture2D (_MainTex, P_9), texture2D (_V_CW_SecondaryTex, P_11), vec4(vBlend_8));
					  highp vec4 tmpvar_13;
					  tmpvar_13.w = 1.0;
					  tmpvar_13.xyz = xlv_TEXCOORD2;
					  highp vec2 tmpvar_14;
					  tmpvar_14 = (_LightMatrix0 * tmpvar_13).xy;
					  lowp float tmpvar_15;
					  tmpvar_15 = texture2D (_LightTexture0, tmpvar_14).w;
					  tmpvar_1 = _LightColor0.xyz;
					  tmpvar_2 = lightDir_5;
					  tmpvar_1 = (tmpvar_1 * tmpvar_15);
					  lowp vec4 c_16;
					  lowp vec4 c_17;
					  lowp float diff_18;
					  mediump float tmpvar_19;
					  tmpvar_19 = max (0.0, dot (tmpvar_4, tmpvar_2));
					  diff_18 = tmpvar_19;
					  c_17.xyz = ((tmpvar_12.xyz * _Color.xyz) * (tmpvar_1 * diff_18));
					  c_17.w = (tmpvar_12.w * _Color.w);
					  c_16.w = c_17.w;
					  c_16.xyz = c_17.xyz;
					  c_3.w = c_16.w;
					  highp float tmpvar_20;
					  tmpvar_20 = clamp (xlv_TEXCOORD3, 0.0, 1.0);
					  c_3.xyz = (c_17.xyz * vec3(tmpvar_20));
					  gl_FragData[0] = c_3;
					}
					
					
					#endif"
}
SubProgram "gles " {
Keywords { "POINT" "V_CW_VERTEX_COLOR" "V_CW_REFLECTIVE_OFF" "_EMISSION_OFF" "V_CW_RIM_OFF" "_NORMALMAP_OFF" "V_CW_SPECULAR_OFF" }
					"!!GLES
					#version 100
					
					#ifdef VERTEX
					attribute vec4 _glesTANGENT;
					attribute vec4 _glesVertex;
					attribute vec4 _glesColor;
					attribute vec3 _glesNormal;
					attribute vec4 _glesMultiTexCoord0;
					uniform highp mat4 glstate_matrix_mvp;
					uniform highp mat4 _Object2World;
					uniform highp mat4 _World2Object;
					uniform highp vec3 _V_CW_Bend;
					uniform highp vec3 _V_CW_Bias;
					uniform highp vec4 _V_CW_PivotPoint_Position;
					uniform highp vec4 _MainTex_ST;
					uniform highp vec4 _V_CW_SecondaryTex_ST;
					varying highp vec4 xlv_TEXCOORD0;
					varying mediump vec3 xlv_TEXCOORD1;
					varying highp vec3 xlv_TEXCOORD2;
					varying lowp vec4 xlv_COLOR0;
					void main ()
					{
					  lowp vec3 worldNormal_1;
					  highp vec4 tmpvar_2;
					  mediump vec3 tmpvar_3;
					  highp vec4 v_4;
					  highp float tmpvar_5;
					  tmpvar_5 = _World2Object[0].x;
					  v_4.x = tmpvar_5;
					  highp float tmpvar_6;
					  tmpvar_6 = _World2Object[1].x;
					  v_4.y = tmpvar_6;
					  highp float tmpvar_7;
					  tmpvar_7 = _World2Object[2].x;
					  v_4.z = tmpvar_7;
					  highp float tmpvar_8;
					  tmpvar_8 = _World2Object[3].x;
					  v_4.w = tmpvar_8;
					  highp vec4 v_9;
					  highp float tmpvar_10;
					  tmpvar_10 = _World2Object[0].y;
					  v_9.x = tmpvar_10;
					  highp float tmpvar_11;
					  tmpvar_11 = _World2Object[1].y;
					  v_9.y = tmpvar_11;
					  highp float tmpvar_12;
					  tmpvar_12 = _World2Object[2].y;
					  v_9.z = tmpvar_12;
					  highp float tmpvar_13;
					  tmpvar_13 = _World2Object[3].y;
					  v_9.w = tmpvar_13;
					  highp vec4 v_14;
					  highp float tmpvar_15;
					  tmpvar_15 = _World2Object[0].z;
					  v_14.x = tmpvar_15;
					  highp float tmpvar_16;
					  tmpvar_16 = _World2Object[1].z;
					  v_14.y = tmpvar_16;
					  highp float tmpvar_17;
					  tmpvar_17 = _World2Object[2].z;
					  v_14.z = tmpvar_17;
					  highp float tmpvar_18;
					  tmpvar_18 = _World2Object[3].z;
					  v_14.w = tmpvar_18;
					  highp vec3 tmpvar_19;
					  tmpvar_19 = normalize(((
					    (v_4.xyz * _glesNormal.x)
					   + 
					    (v_9.xyz * _glesNormal.y)
					  ) + (v_14.xyz * _glesNormal.z)));
					  highp mat3 tmpvar_20;
					  tmpvar_20[0] = _Object2World[0].xyz;
					  tmpvar_20[1] = _Object2World[1].xyz;
					  tmpvar_20[2] = _Object2World[2].xyz;
					  highp vec3 tmpvar_21;
					  tmpvar_21 = normalize((tmpvar_20 * _glesTANGENT.xyz));
					  highp vec4 vertex_22;
					  vertex_22.w = _glesVertex.w;
					  highp vec2 xzOff_23;
					  highp vec3 v2_24;
					  highp vec3 v1_25;
					  highp vec3 v0_26;
					  highp vec3 tmpvar_27;
					  tmpvar_27 = ((_Object2World * _glesVertex).xyz - _V_CW_PivotPoint_Position.xyz);
					  highp vec3 tmpvar_28;
					  tmpvar_28 = (tmpvar_27 + tmpvar_21);
					  v1_25.xz = tmpvar_28.xz;
					  highp vec3 tmpvar_29;
					  tmpvar_29 = (tmpvar_27 - ((tmpvar_19.yzx * tmpvar_21.zxy) - (tmpvar_19.zxy * tmpvar_21.yzx)));
					  v2_24.xz = tmpvar_29.xz;
					  highp vec2 tmpvar_30;
					  tmpvar_30.x = float((tmpvar_27.z >= 0.0));
					  tmpvar_30.y = float((tmpvar_27.x >= 0.0));
					  xzOff_23 = (max (vec2(0.0, 0.0), (
					    abs(tmpvar_27.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_30 * 2.0) - 1.0));
					  xzOff_23 = (xzOff_23 * xzOff_23);
					  highp vec3 tmpvar_31;
					  tmpvar_31.xz = vec2(0.0, 0.0);
					  tmpvar_31.y = (((_V_CW_Bend.x * xzOff_23.x) + (_V_CW_Bend.z * xzOff_23.y)) * 0.001);
					  v0_26 = (tmpvar_27 + tmpvar_31);
					  highp vec2 tmpvar_32;
					  tmpvar_32.x = float((tmpvar_28.z >= 0.0));
					  tmpvar_32.y = float((tmpvar_28.x >= 0.0));
					  xzOff_23 = (max (vec2(0.0, 0.0), (
					    abs(tmpvar_28.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_32 * 2.0) - 1.0));
					  xzOff_23 = (xzOff_23 * xzOff_23);
					  v1_25.y = (tmpvar_28.y + ((
					    (_V_CW_Bend.x * xzOff_23.x)
					   + 
					    (_V_CW_Bend.z * xzOff_23.y)
					  ) * 0.001));
					  highp vec2 tmpvar_33;
					  tmpvar_33.x = float((tmpvar_29.z >= 0.0));
					  tmpvar_33.y = float((tmpvar_29.x >= 0.0));
					  xzOff_23 = (max (vec2(0.0, 0.0), (
					    abs(tmpvar_29.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_33 * 2.0) - 1.0));
					  xzOff_23 = (xzOff_23 * xzOff_23);
					  v2_24.y = (tmpvar_29.y + ((
					    (_V_CW_Bend.x * xzOff_23.x)
					   + 
					    (_V_CW_Bend.z * xzOff_23.y)
					  ) * 0.001));
					  highp mat3 tmpvar_34;
					  tmpvar_34[0] = _World2Object[0].xyz;
					  tmpvar_34[1] = _World2Object[1].xyz;
					  tmpvar_34[2] = _World2Object[2].xyz;
					  vertex_22.xyz = (_glesVertex.xyz + (tmpvar_34 * tmpvar_31));
					  highp mat3 tmpvar_35;
					  tmpvar_35[0] = _World2Object[0].xyz;
					  tmpvar_35[1] = _World2Object[1].xyz;
					  tmpvar_35[2] = _World2Object[2].xyz;
					  highp vec3 a_36;
					  a_36 = (v2_24 - v0_26);
					  highp vec3 b_37;
					  b_37 = (v1_25 - v0_26);
					  highp vec3 tmpvar_38;
					  tmpvar_38 = normalize((tmpvar_35 * normalize(
					    ((a_36.yzx * b_37.zxy) - (a_36.zxy * b_37.yzx))
					  )));
					  tmpvar_2.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
					  tmpvar_2.zw = ((_glesMultiTexCoord0.xy * _V_CW_SecondaryTex_ST.xy) + _V_CW_SecondaryTex_ST.zw);
					  highp vec4 v_39;
					  v_39.x = tmpvar_5;
					  v_39.y = tmpvar_6;
					  v_39.z = tmpvar_7;
					  v_39.w = tmpvar_8;
					  highp vec4 v_40;
					  v_40.x = tmpvar_10;
					  v_40.y = tmpvar_11;
					  v_40.z = tmpvar_12;
					  v_40.w = tmpvar_13;
					  highp vec4 v_41;
					  v_41.x = tmpvar_15;
					  v_41.y = tmpvar_16;
					  v_41.z = tmpvar_17;
					  v_41.w = tmpvar_18;
					  highp vec3 tmpvar_42;
					  tmpvar_42 = normalize(((
					    (v_39.xyz * tmpvar_38.x)
					   + 
					    (v_40.xyz * tmpvar_38.y)
					  ) + (v_41.xyz * tmpvar_38.z)));
					  worldNormal_1 = tmpvar_42;
					  tmpvar_3 = worldNormal_1;
					  gl_Position = (glstate_matrix_mvp * vertex_22);
					  xlv_TEXCOORD0 = tmpvar_2;
					  xlv_TEXCOORD1 = tmpvar_3;
					  xlv_TEXCOORD2 = (_Object2World * vertex_22).xyz;
					  xlv_COLOR0 = _glesColor;
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform highp vec4 _Time;
					uniform highp vec4 _WorldSpaceLightPos0;
					uniform lowp vec4 _LightColor0;
					uniform sampler2D _LightTexture0;
					uniform mediump mat4 _LightMatrix0;
					uniform sampler2D _MainTex;
					uniform lowp vec4 _Color;
					uniform lowp vec2 _V_CW_MainTex_Scroll;
					uniform sampler2D _V_CW_SecondaryTex;
					uniform lowp vec2 _V_CW_SecondaryTex_Scroll;
					uniform lowp float _V_CW_SecondaryTex_Blend;
					varying highp vec4 xlv_TEXCOORD0;
					varying mediump vec3 xlv_TEXCOORD1;
					varying highp vec3 xlv_TEXCOORD2;
					varying lowp vec4 xlv_COLOR0;
					void main ()
					{
					  mediump vec3 tmpvar_1;
					  mediump vec3 tmpvar_2;
					  lowp vec3 tmpvar_3;
					  lowp vec3 lightDir_4;
					  highp vec4 tmpvar_5;
					  highp vec3 tmpvar_6;
					  tmpvar_6 = normalize((_WorldSpaceLightPos0.xyz - xlv_TEXCOORD2));
					  lightDir_4 = tmpvar_6;
					  tmpvar_5 = xlv_COLOR0;
					  tmpvar_3 = xlv_TEXCOORD1;
					  lowp vec3 tmpvar_7;
					  lowp float vBlend_8;
					  highp vec2 P_9;
					  P_9 = (xlv_TEXCOORD0.xy + (_V_CW_MainTex_Scroll * _Time.x));
					  highp float tmpvar_10;
					  tmpvar_10 = clamp ((_V_CW_SecondaryTex_Blend + tmpvar_5.w), 0.0, 1.0);
					  vBlend_8 = tmpvar_10;
					  highp vec2 P_11;
					  P_11 = (xlv_TEXCOORD0.zw + (_V_CW_SecondaryTex_Scroll * _Time.x));
					  lowp vec4 tmpvar_12;
					  tmpvar_12 = mix (texture2D (_MainTex, P_9), texture2D (_V_CW_SecondaryTex, P_11), vec4(vBlend_8));
					  tmpvar_7 = (tmpvar_12.xyz * _Color.xyz);
					  tmpvar_7 = (tmpvar_7 * tmpvar_5.xyz);
					  highp vec4 tmpvar_13;
					  tmpvar_13.w = 1.0;
					  tmpvar_13.xyz = xlv_TEXCOORD2;
					  highp vec3 tmpvar_14;
					  tmpvar_14 = (_LightMatrix0 * tmpvar_13).xyz;
					  highp float tmpvar_15;
					  tmpvar_15 = dot (tmpvar_14, tmpvar_14);
					  lowp float tmpvar_16;
					  tmpvar_16 = texture2D (_LightTexture0, vec2(tmpvar_15)).w;
					  tmpvar_1 = _LightColor0.xyz;
					  tmpvar_2 = lightDir_4;
					  tmpvar_1 = (tmpvar_1 * tmpvar_16);
					  lowp vec4 c_17;
					  lowp vec4 c_18;
					  lowp float diff_19;
					  mediump float tmpvar_20;
					  tmpvar_20 = max (0.0, dot (tmpvar_3, tmpvar_2));
					  diff_19 = tmpvar_20;
					  c_18.xyz = ((tmpvar_7 * tmpvar_1) * diff_19);
					  c_18.w = (tmpvar_12.w * _Color.w);
					  c_17.w = c_18.w;
					  c_17.xyz = c_18.xyz;
					  gl_FragData[0] = c_17;
					}
					
					
					#endif"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "V_CW_VERTEX_COLOR" "V_CW_REFLECTIVE_OFF" "_EMISSION_OFF" "V_CW_RIM_OFF" "_NORMALMAP_OFF" "V_CW_SPECULAR_OFF" }
					"!!GLES
					#version 100
					
					#ifdef VERTEX
					attribute vec4 _glesTANGENT;
					attribute vec4 _glesVertex;
					attribute vec4 _glesColor;
					attribute vec3 _glesNormal;
					attribute vec4 _glesMultiTexCoord0;
					uniform highp mat4 glstate_matrix_mvp;
					uniform highp mat4 _Object2World;
					uniform highp mat4 _World2Object;
					uniform highp vec3 _V_CW_Bend;
					uniform highp vec3 _V_CW_Bias;
					uniform highp vec4 _V_CW_PivotPoint_Position;
					uniform highp vec4 _MainTex_ST;
					uniform highp vec4 _V_CW_SecondaryTex_ST;
					varying highp vec4 xlv_TEXCOORD0;
					varying mediump vec3 xlv_TEXCOORD1;
					varying highp vec3 xlv_TEXCOORD2;
					varying lowp vec4 xlv_COLOR0;
					void main ()
					{
					  lowp vec3 worldNormal_1;
					  highp vec4 tmpvar_2;
					  mediump vec3 tmpvar_3;
					  highp vec4 v_4;
					  highp float tmpvar_5;
					  tmpvar_5 = _World2Object[0].x;
					  v_4.x = tmpvar_5;
					  highp float tmpvar_6;
					  tmpvar_6 = _World2Object[1].x;
					  v_4.y = tmpvar_6;
					  highp float tmpvar_7;
					  tmpvar_7 = _World2Object[2].x;
					  v_4.z = tmpvar_7;
					  highp float tmpvar_8;
					  tmpvar_8 = _World2Object[3].x;
					  v_4.w = tmpvar_8;
					  highp vec4 v_9;
					  highp float tmpvar_10;
					  tmpvar_10 = _World2Object[0].y;
					  v_9.x = tmpvar_10;
					  highp float tmpvar_11;
					  tmpvar_11 = _World2Object[1].y;
					  v_9.y = tmpvar_11;
					  highp float tmpvar_12;
					  tmpvar_12 = _World2Object[2].y;
					  v_9.z = tmpvar_12;
					  highp float tmpvar_13;
					  tmpvar_13 = _World2Object[3].y;
					  v_9.w = tmpvar_13;
					  highp vec4 v_14;
					  highp float tmpvar_15;
					  tmpvar_15 = _World2Object[0].z;
					  v_14.x = tmpvar_15;
					  highp float tmpvar_16;
					  tmpvar_16 = _World2Object[1].z;
					  v_14.y = tmpvar_16;
					  highp float tmpvar_17;
					  tmpvar_17 = _World2Object[2].z;
					  v_14.z = tmpvar_17;
					  highp float tmpvar_18;
					  tmpvar_18 = _World2Object[3].z;
					  v_14.w = tmpvar_18;
					  highp vec3 tmpvar_19;
					  tmpvar_19 = normalize(((
					    (v_4.xyz * _glesNormal.x)
					   + 
					    (v_9.xyz * _glesNormal.y)
					  ) + (v_14.xyz * _glesNormal.z)));
					  highp mat3 tmpvar_20;
					  tmpvar_20[0] = _Object2World[0].xyz;
					  tmpvar_20[1] = _Object2World[1].xyz;
					  tmpvar_20[2] = _Object2World[2].xyz;
					  highp vec3 tmpvar_21;
					  tmpvar_21 = normalize((tmpvar_20 * _glesTANGENT.xyz));
					  highp vec4 vertex_22;
					  vertex_22.w = _glesVertex.w;
					  highp vec2 xzOff_23;
					  highp vec3 v2_24;
					  highp vec3 v1_25;
					  highp vec3 v0_26;
					  highp vec3 tmpvar_27;
					  tmpvar_27 = ((_Object2World * _glesVertex).xyz - _V_CW_PivotPoint_Position.xyz);
					  highp vec3 tmpvar_28;
					  tmpvar_28 = (tmpvar_27 + tmpvar_21);
					  v1_25.xz = tmpvar_28.xz;
					  highp vec3 tmpvar_29;
					  tmpvar_29 = (tmpvar_27 - ((tmpvar_19.yzx * tmpvar_21.zxy) - (tmpvar_19.zxy * tmpvar_21.yzx)));
					  v2_24.xz = tmpvar_29.xz;
					  highp vec2 tmpvar_30;
					  tmpvar_30.x = float((tmpvar_27.z >= 0.0));
					  tmpvar_30.y = float((tmpvar_27.x >= 0.0));
					  xzOff_23 = (max (vec2(0.0, 0.0), (
					    abs(tmpvar_27.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_30 * 2.0) - 1.0));
					  xzOff_23 = (xzOff_23 * xzOff_23);
					  highp vec3 tmpvar_31;
					  tmpvar_31.xz = vec2(0.0, 0.0);
					  tmpvar_31.y = (((_V_CW_Bend.x * xzOff_23.x) + (_V_CW_Bend.z * xzOff_23.y)) * 0.001);
					  v0_26 = (tmpvar_27 + tmpvar_31);
					  highp vec2 tmpvar_32;
					  tmpvar_32.x = float((tmpvar_28.z >= 0.0));
					  tmpvar_32.y = float((tmpvar_28.x >= 0.0));
					  xzOff_23 = (max (vec2(0.0, 0.0), (
					    abs(tmpvar_28.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_32 * 2.0) - 1.0));
					  xzOff_23 = (xzOff_23 * xzOff_23);
					  v1_25.y = (tmpvar_28.y + ((
					    (_V_CW_Bend.x * xzOff_23.x)
					   + 
					    (_V_CW_Bend.z * xzOff_23.y)
					  ) * 0.001));
					  highp vec2 tmpvar_33;
					  tmpvar_33.x = float((tmpvar_29.z >= 0.0));
					  tmpvar_33.y = float((tmpvar_29.x >= 0.0));
					  xzOff_23 = (max (vec2(0.0, 0.0), (
					    abs(tmpvar_29.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_33 * 2.0) - 1.0));
					  xzOff_23 = (xzOff_23 * xzOff_23);
					  v2_24.y = (tmpvar_29.y + ((
					    (_V_CW_Bend.x * xzOff_23.x)
					   + 
					    (_V_CW_Bend.z * xzOff_23.y)
					  ) * 0.001));
					  highp mat3 tmpvar_34;
					  tmpvar_34[0] = _World2Object[0].xyz;
					  tmpvar_34[1] = _World2Object[1].xyz;
					  tmpvar_34[2] = _World2Object[2].xyz;
					  vertex_22.xyz = (_glesVertex.xyz + (tmpvar_34 * tmpvar_31));
					  highp mat3 tmpvar_35;
					  tmpvar_35[0] = _World2Object[0].xyz;
					  tmpvar_35[1] = _World2Object[1].xyz;
					  tmpvar_35[2] = _World2Object[2].xyz;
					  highp vec3 a_36;
					  a_36 = (v2_24 - v0_26);
					  highp vec3 b_37;
					  b_37 = (v1_25 - v0_26);
					  highp vec3 tmpvar_38;
					  tmpvar_38 = normalize((tmpvar_35 * normalize(
					    ((a_36.yzx * b_37.zxy) - (a_36.zxy * b_37.yzx))
					  )));
					  tmpvar_2.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
					  tmpvar_2.zw = ((_glesMultiTexCoord0.xy * _V_CW_SecondaryTex_ST.xy) + _V_CW_SecondaryTex_ST.zw);
					  highp vec4 v_39;
					  v_39.x = tmpvar_5;
					  v_39.y = tmpvar_6;
					  v_39.z = tmpvar_7;
					  v_39.w = tmpvar_8;
					  highp vec4 v_40;
					  v_40.x = tmpvar_10;
					  v_40.y = tmpvar_11;
					  v_40.z = tmpvar_12;
					  v_40.w = tmpvar_13;
					  highp vec4 v_41;
					  v_41.x = tmpvar_15;
					  v_41.y = tmpvar_16;
					  v_41.z = tmpvar_17;
					  v_41.w = tmpvar_18;
					  highp vec3 tmpvar_42;
					  tmpvar_42 = normalize(((
					    (v_39.xyz * tmpvar_38.x)
					   + 
					    (v_40.xyz * tmpvar_38.y)
					  ) + (v_41.xyz * tmpvar_38.z)));
					  worldNormal_1 = tmpvar_42;
					  tmpvar_3 = worldNormal_1;
					  gl_Position = (glstate_matrix_mvp * vertex_22);
					  xlv_TEXCOORD0 = tmpvar_2;
					  xlv_TEXCOORD1 = tmpvar_3;
					  xlv_TEXCOORD2 = (_Object2World * vertex_22).xyz;
					  xlv_COLOR0 = _glesColor;
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform highp vec4 _Time;
					uniform mediump vec4 _WorldSpaceLightPos0;
					uniform lowp vec4 _LightColor0;
					uniform sampler2D _MainTex;
					uniform lowp vec4 _Color;
					uniform lowp vec2 _V_CW_MainTex_Scroll;
					uniform sampler2D _V_CW_SecondaryTex;
					uniform lowp vec2 _V_CW_SecondaryTex_Scroll;
					uniform lowp float _V_CW_SecondaryTex_Blend;
					varying highp vec4 xlv_TEXCOORD0;
					varying mediump vec3 xlv_TEXCOORD1;
					varying lowp vec4 xlv_COLOR0;
					void main ()
					{
					  mediump vec3 tmpvar_1;
					  mediump vec3 tmpvar_2;
					  lowp vec3 tmpvar_3;
					  lowp vec3 lightDir_4;
					  highp vec4 tmpvar_5;
					  mediump vec3 tmpvar_6;
					  tmpvar_6 = _WorldSpaceLightPos0.xyz;
					  lightDir_4 = tmpvar_6;
					  tmpvar_5 = xlv_COLOR0;
					  tmpvar_3 = xlv_TEXCOORD1;
					  lowp vec3 tmpvar_7;
					  lowp float vBlend_8;
					  highp vec2 P_9;
					  P_9 = (xlv_TEXCOORD0.xy + (_V_CW_MainTex_Scroll * _Time.x));
					  highp float tmpvar_10;
					  tmpvar_10 = clamp ((_V_CW_SecondaryTex_Blend + tmpvar_5.w), 0.0, 1.0);
					  vBlend_8 = tmpvar_10;
					  highp vec2 P_11;
					  P_11 = (xlv_TEXCOORD0.zw + (_V_CW_SecondaryTex_Scroll * _Time.x));
					  lowp vec4 tmpvar_12;
					  tmpvar_12 = mix (texture2D (_MainTex, P_9), texture2D (_V_CW_SecondaryTex, P_11), vec4(vBlend_8));
					  tmpvar_7 = (tmpvar_12.xyz * _Color.xyz);
					  tmpvar_7 = (tmpvar_7 * tmpvar_5.xyz);
					  tmpvar_1 = _LightColor0.xyz;
					  tmpvar_2 = lightDir_4;
					  lowp vec4 c_13;
					  lowp vec4 c_14;
					  lowp float diff_15;
					  mediump float tmpvar_16;
					  tmpvar_16 = max (0.0, dot (tmpvar_3, tmpvar_2));
					  diff_15 = tmpvar_16;
					  c_14.xyz = ((tmpvar_7 * tmpvar_1) * diff_15);
					  c_14.w = (tmpvar_12.w * _Color.w);
					  c_13.w = c_14.w;
					  c_13.xyz = c_14.xyz;
					  gl_FragData[0] = c_13;
					}
					
					
					#endif"
}
SubProgram "gles " {
Keywords { "SPOT" "V_CW_VERTEX_COLOR" "V_CW_REFLECTIVE_OFF" "_EMISSION_OFF" "V_CW_RIM_OFF" "_NORMALMAP_OFF" "V_CW_SPECULAR_OFF" }
					"!!GLES
					#version 100
					
					#ifdef VERTEX
					attribute vec4 _glesTANGENT;
					attribute vec4 _glesVertex;
					attribute vec4 _glesColor;
					attribute vec3 _glesNormal;
					attribute vec4 _glesMultiTexCoord0;
					uniform highp mat4 glstate_matrix_mvp;
					uniform highp mat4 _Object2World;
					uniform highp mat4 _World2Object;
					uniform highp vec3 _V_CW_Bend;
					uniform highp vec3 _V_CW_Bias;
					uniform highp vec4 _V_CW_PivotPoint_Position;
					uniform highp vec4 _MainTex_ST;
					uniform highp vec4 _V_CW_SecondaryTex_ST;
					varying highp vec4 xlv_TEXCOORD0;
					varying mediump vec3 xlv_TEXCOORD1;
					varying highp vec3 xlv_TEXCOORD2;
					varying lowp vec4 xlv_COLOR0;
					void main ()
					{
					  lowp vec3 worldNormal_1;
					  highp vec4 tmpvar_2;
					  mediump vec3 tmpvar_3;
					  highp vec4 v_4;
					  highp float tmpvar_5;
					  tmpvar_5 = _World2Object[0].x;
					  v_4.x = tmpvar_5;
					  highp float tmpvar_6;
					  tmpvar_6 = _World2Object[1].x;
					  v_4.y = tmpvar_6;
					  highp float tmpvar_7;
					  tmpvar_7 = _World2Object[2].x;
					  v_4.z = tmpvar_7;
					  highp float tmpvar_8;
					  tmpvar_8 = _World2Object[3].x;
					  v_4.w = tmpvar_8;
					  highp vec4 v_9;
					  highp float tmpvar_10;
					  tmpvar_10 = _World2Object[0].y;
					  v_9.x = tmpvar_10;
					  highp float tmpvar_11;
					  tmpvar_11 = _World2Object[1].y;
					  v_9.y = tmpvar_11;
					  highp float tmpvar_12;
					  tmpvar_12 = _World2Object[2].y;
					  v_9.z = tmpvar_12;
					  highp float tmpvar_13;
					  tmpvar_13 = _World2Object[3].y;
					  v_9.w = tmpvar_13;
					  highp vec4 v_14;
					  highp float tmpvar_15;
					  tmpvar_15 = _World2Object[0].z;
					  v_14.x = tmpvar_15;
					  highp float tmpvar_16;
					  tmpvar_16 = _World2Object[1].z;
					  v_14.y = tmpvar_16;
					  highp float tmpvar_17;
					  tmpvar_17 = _World2Object[2].z;
					  v_14.z = tmpvar_17;
					  highp float tmpvar_18;
					  tmpvar_18 = _World2Object[3].z;
					  v_14.w = tmpvar_18;
					  highp vec3 tmpvar_19;
					  tmpvar_19 = normalize(((
					    (v_4.xyz * _glesNormal.x)
					   + 
					    (v_9.xyz * _glesNormal.y)
					  ) + (v_14.xyz * _glesNormal.z)));
					  highp mat3 tmpvar_20;
					  tmpvar_20[0] = _Object2World[0].xyz;
					  tmpvar_20[1] = _Object2World[1].xyz;
					  tmpvar_20[2] = _Object2World[2].xyz;
					  highp vec3 tmpvar_21;
					  tmpvar_21 = normalize((tmpvar_20 * _glesTANGENT.xyz));
					  highp vec4 vertex_22;
					  vertex_22.w = _glesVertex.w;
					  highp vec2 xzOff_23;
					  highp vec3 v2_24;
					  highp vec3 v1_25;
					  highp vec3 v0_26;
					  highp vec3 tmpvar_27;
					  tmpvar_27 = ((_Object2World * _glesVertex).xyz - _V_CW_PivotPoint_Position.xyz);
					  highp vec3 tmpvar_28;
					  tmpvar_28 = (tmpvar_27 + tmpvar_21);
					  v1_25.xz = tmpvar_28.xz;
					  highp vec3 tmpvar_29;
					  tmpvar_29 = (tmpvar_27 - ((tmpvar_19.yzx * tmpvar_21.zxy) - (tmpvar_19.zxy * tmpvar_21.yzx)));
					  v2_24.xz = tmpvar_29.xz;
					  highp vec2 tmpvar_30;
					  tmpvar_30.x = float((tmpvar_27.z >= 0.0));
					  tmpvar_30.y = float((tmpvar_27.x >= 0.0));
					  xzOff_23 = (max (vec2(0.0, 0.0), (
					    abs(tmpvar_27.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_30 * 2.0) - 1.0));
					  xzOff_23 = (xzOff_23 * xzOff_23);
					  highp vec3 tmpvar_31;
					  tmpvar_31.xz = vec2(0.0, 0.0);
					  tmpvar_31.y = (((_V_CW_Bend.x * xzOff_23.x) + (_V_CW_Bend.z * xzOff_23.y)) * 0.001);
					  v0_26 = (tmpvar_27 + tmpvar_31);
					  highp vec2 tmpvar_32;
					  tmpvar_32.x = float((tmpvar_28.z >= 0.0));
					  tmpvar_32.y = float((tmpvar_28.x >= 0.0));
					  xzOff_23 = (max (vec2(0.0, 0.0), (
					    abs(tmpvar_28.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_32 * 2.0) - 1.0));
					  xzOff_23 = (xzOff_23 * xzOff_23);
					  v1_25.y = (tmpvar_28.y + ((
					    (_V_CW_Bend.x * xzOff_23.x)
					   + 
					    (_V_CW_Bend.z * xzOff_23.y)
					  ) * 0.001));
					  highp vec2 tmpvar_33;
					  tmpvar_33.x = float((tmpvar_29.z >= 0.0));
					  tmpvar_33.y = float((tmpvar_29.x >= 0.0));
					  xzOff_23 = (max (vec2(0.0, 0.0), (
					    abs(tmpvar_29.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_33 * 2.0) - 1.0));
					  xzOff_23 = (xzOff_23 * xzOff_23);
					  v2_24.y = (tmpvar_29.y + ((
					    (_V_CW_Bend.x * xzOff_23.x)
					   + 
					    (_V_CW_Bend.z * xzOff_23.y)
					  ) * 0.001));
					  highp mat3 tmpvar_34;
					  tmpvar_34[0] = _World2Object[0].xyz;
					  tmpvar_34[1] = _World2Object[1].xyz;
					  tmpvar_34[2] = _World2Object[2].xyz;
					  vertex_22.xyz = (_glesVertex.xyz + (tmpvar_34 * tmpvar_31));
					  highp mat3 tmpvar_35;
					  tmpvar_35[0] = _World2Object[0].xyz;
					  tmpvar_35[1] = _World2Object[1].xyz;
					  tmpvar_35[2] = _World2Object[2].xyz;
					  highp vec3 a_36;
					  a_36 = (v2_24 - v0_26);
					  highp vec3 b_37;
					  b_37 = (v1_25 - v0_26);
					  highp vec3 tmpvar_38;
					  tmpvar_38 = normalize((tmpvar_35 * normalize(
					    ((a_36.yzx * b_37.zxy) - (a_36.zxy * b_37.yzx))
					  )));
					  tmpvar_2.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
					  tmpvar_2.zw = ((_glesMultiTexCoord0.xy * _V_CW_SecondaryTex_ST.xy) + _V_CW_SecondaryTex_ST.zw);
					  highp vec4 v_39;
					  v_39.x = tmpvar_5;
					  v_39.y = tmpvar_6;
					  v_39.z = tmpvar_7;
					  v_39.w = tmpvar_8;
					  highp vec4 v_40;
					  v_40.x = tmpvar_10;
					  v_40.y = tmpvar_11;
					  v_40.z = tmpvar_12;
					  v_40.w = tmpvar_13;
					  highp vec4 v_41;
					  v_41.x = tmpvar_15;
					  v_41.y = tmpvar_16;
					  v_41.z = tmpvar_17;
					  v_41.w = tmpvar_18;
					  highp vec3 tmpvar_42;
					  tmpvar_42 = normalize(((
					    (v_39.xyz * tmpvar_38.x)
					   + 
					    (v_40.xyz * tmpvar_38.y)
					  ) + (v_41.xyz * tmpvar_38.z)));
					  worldNormal_1 = tmpvar_42;
					  tmpvar_3 = worldNormal_1;
					  gl_Position = (glstate_matrix_mvp * vertex_22);
					  xlv_TEXCOORD0 = tmpvar_2;
					  xlv_TEXCOORD1 = tmpvar_3;
					  xlv_TEXCOORD2 = (_Object2World * vertex_22).xyz;
					  xlv_COLOR0 = _glesColor;
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform highp vec4 _Time;
					uniform highp vec4 _WorldSpaceLightPos0;
					uniform lowp vec4 _LightColor0;
					uniform sampler2D _LightTexture0;
					uniform mediump mat4 _LightMatrix0;
					uniform sampler2D _LightTextureB0;
					uniform sampler2D _MainTex;
					uniform lowp vec4 _Color;
					uniform lowp vec2 _V_CW_MainTex_Scroll;
					uniform sampler2D _V_CW_SecondaryTex;
					uniform lowp vec2 _V_CW_SecondaryTex_Scroll;
					uniform lowp float _V_CW_SecondaryTex_Blend;
					varying highp vec4 xlv_TEXCOORD0;
					varying mediump vec3 xlv_TEXCOORD1;
					varying highp vec3 xlv_TEXCOORD2;
					varying lowp vec4 xlv_COLOR0;
					void main ()
					{
					  mediump vec3 tmpvar_1;
					  mediump vec3 tmpvar_2;
					  lowp float atten_3;
					  mediump vec4 lightCoord_4;
					  lowp vec3 tmpvar_5;
					  lowp vec3 lightDir_6;
					  highp vec4 tmpvar_7;
					  highp vec3 tmpvar_8;
					  tmpvar_8 = normalize((_WorldSpaceLightPos0.xyz - xlv_TEXCOORD2));
					  lightDir_6 = tmpvar_8;
					  tmpvar_7 = xlv_COLOR0;
					  tmpvar_5 = xlv_TEXCOORD1;
					  lowp vec3 tmpvar_9;
					  lowp float vBlend_10;
					  highp vec2 P_11;
					  P_11 = (xlv_TEXCOORD0.xy + (_V_CW_MainTex_Scroll * _Time.x));
					  highp float tmpvar_12;
					  tmpvar_12 = clamp ((_V_CW_SecondaryTex_Blend + tmpvar_7.w), 0.0, 1.0);
					  vBlend_10 = tmpvar_12;
					  highp vec2 P_13;
					  P_13 = (xlv_TEXCOORD0.zw + (_V_CW_SecondaryTex_Scroll * _Time.x));
					  lowp vec4 tmpvar_14;
					  tmpvar_14 = mix (texture2D (_MainTex, P_11), texture2D (_V_CW_SecondaryTex, P_13), vec4(vBlend_10));
					  tmpvar_9 = (tmpvar_14.xyz * _Color.xyz);
					  tmpvar_9 = (tmpvar_9 * tmpvar_7.xyz);
					  highp vec4 tmpvar_15;
					  tmpvar_15.w = 1.0;
					  tmpvar_15.xyz = xlv_TEXCOORD2;
					  highp vec4 tmpvar_16;
					  tmpvar_16 = (_LightMatrix0 * tmpvar_15);
					  lightCoord_4 = tmpvar_16;
					  lowp vec4 tmpvar_17;
					  mediump vec2 P_18;
					  P_18 = ((lightCoord_4.xy / lightCoord_4.w) + 0.5);
					  tmpvar_17 = texture2D (_LightTexture0, P_18);
					  highp vec3 LightCoord_19;
					  LightCoord_19 = lightCoord_4.xyz;
					  highp float tmpvar_20;
					  tmpvar_20 = dot (LightCoord_19, LightCoord_19);
					  lowp vec4 tmpvar_21;
					  tmpvar_21 = texture2D (_LightTextureB0, vec2(tmpvar_20));
					  mediump float tmpvar_22;
					  tmpvar_22 = ((float(
					    (lightCoord_4.z > 0.0)
					  ) * tmpvar_17.w) * tmpvar_21.w);
					  atten_3 = tmpvar_22;
					  tmpvar_1 = _LightColor0.xyz;
					  tmpvar_2 = lightDir_6;
					  tmpvar_1 = (tmpvar_1 * atten_3);
					  lowp vec4 c_23;
					  lowp vec4 c_24;
					  lowp float diff_25;
					  mediump float tmpvar_26;
					  tmpvar_26 = max (0.0, dot (tmpvar_5, tmpvar_2));
					  diff_25 = tmpvar_26;
					  c_24.xyz = ((tmpvar_9 * tmpvar_1) * diff_25);
					  c_24.w = (tmpvar_14.w * _Color.w);
					  c_23.w = c_24.w;
					  c_23.xyz = c_24.xyz;
					  gl_FragData[0] = c_23;
					}
					
					
					#endif"
}
SubProgram "gles " {
Keywords { "POINT_COOKIE" "V_CW_VERTEX_COLOR" "V_CW_REFLECTIVE_OFF" "_EMISSION_OFF" "V_CW_RIM_OFF" "_NORMALMAP_OFF" "V_CW_SPECULAR_OFF" }
					"!!GLES
					#version 100
					
					#ifdef VERTEX
					attribute vec4 _glesTANGENT;
					attribute vec4 _glesVertex;
					attribute vec4 _glesColor;
					attribute vec3 _glesNormal;
					attribute vec4 _glesMultiTexCoord0;
					uniform highp mat4 glstate_matrix_mvp;
					uniform highp mat4 _Object2World;
					uniform highp mat4 _World2Object;
					uniform highp vec3 _V_CW_Bend;
					uniform highp vec3 _V_CW_Bias;
					uniform highp vec4 _V_CW_PivotPoint_Position;
					uniform highp vec4 _MainTex_ST;
					uniform highp vec4 _V_CW_SecondaryTex_ST;
					varying highp vec4 xlv_TEXCOORD0;
					varying mediump vec3 xlv_TEXCOORD1;
					varying highp vec3 xlv_TEXCOORD2;
					varying lowp vec4 xlv_COLOR0;
					void main ()
					{
					  lowp vec3 worldNormal_1;
					  highp vec4 tmpvar_2;
					  mediump vec3 tmpvar_3;
					  highp vec4 v_4;
					  highp float tmpvar_5;
					  tmpvar_5 = _World2Object[0].x;
					  v_4.x = tmpvar_5;
					  highp float tmpvar_6;
					  tmpvar_6 = _World2Object[1].x;
					  v_4.y = tmpvar_6;
					  highp float tmpvar_7;
					  tmpvar_7 = _World2Object[2].x;
					  v_4.z = tmpvar_7;
					  highp float tmpvar_8;
					  tmpvar_8 = _World2Object[3].x;
					  v_4.w = tmpvar_8;
					  highp vec4 v_9;
					  highp float tmpvar_10;
					  tmpvar_10 = _World2Object[0].y;
					  v_9.x = tmpvar_10;
					  highp float tmpvar_11;
					  tmpvar_11 = _World2Object[1].y;
					  v_9.y = tmpvar_11;
					  highp float tmpvar_12;
					  tmpvar_12 = _World2Object[2].y;
					  v_9.z = tmpvar_12;
					  highp float tmpvar_13;
					  tmpvar_13 = _World2Object[3].y;
					  v_9.w = tmpvar_13;
					  highp vec4 v_14;
					  highp float tmpvar_15;
					  tmpvar_15 = _World2Object[0].z;
					  v_14.x = tmpvar_15;
					  highp float tmpvar_16;
					  tmpvar_16 = _World2Object[1].z;
					  v_14.y = tmpvar_16;
					  highp float tmpvar_17;
					  tmpvar_17 = _World2Object[2].z;
					  v_14.z = tmpvar_17;
					  highp float tmpvar_18;
					  tmpvar_18 = _World2Object[3].z;
					  v_14.w = tmpvar_18;
					  highp vec3 tmpvar_19;
					  tmpvar_19 = normalize(((
					    (v_4.xyz * _glesNormal.x)
					   + 
					    (v_9.xyz * _glesNormal.y)
					  ) + (v_14.xyz * _glesNormal.z)));
					  highp mat3 tmpvar_20;
					  tmpvar_20[0] = _Object2World[0].xyz;
					  tmpvar_20[1] = _Object2World[1].xyz;
					  tmpvar_20[2] = _Object2World[2].xyz;
					  highp vec3 tmpvar_21;
					  tmpvar_21 = normalize((tmpvar_20 * _glesTANGENT.xyz));
					  highp vec4 vertex_22;
					  vertex_22.w = _glesVertex.w;
					  highp vec2 xzOff_23;
					  highp vec3 v2_24;
					  highp vec3 v1_25;
					  highp vec3 v0_26;
					  highp vec3 tmpvar_27;
					  tmpvar_27 = ((_Object2World * _glesVertex).xyz - _V_CW_PivotPoint_Position.xyz);
					  highp vec3 tmpvar_28;
					  tmpvar_28 = (tmpvar_27 + tmpvar_21);
					  v1_25.xz = tmpvar_28.xz;
					  highp vec3 tmpvar_29;
					  tmpvar_29 = (tmpvar_27 - ((tmpvar_19.yzx * tmpvar_21.zxy) - (tmpvar_19.zxy * tmpvar_21.yzx)));
					  v2_24.xz = tmpvar_29.xz;
					  highp vec2 tmpvar_30;
					  tmpvar_30.x = float((tmpvar_27.z >= 0.0));
					  tmpvar_30.y = float((tmpvar_27.x >= 0.0));
					  xzOff_23 = (max (vec2(0.0, 0.0), (
					    abs(tmpvar_27.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_30 * 2.0) - 1.0));
					  xzOff_23 = (xzOff_23 * xzOff_23);
					  highp vec3 tmpvar_31;
					  tmpvar_31.xz = vec2(0.0, 0.0);
					  tmpvar_31.y = (((_V_CW_Bend.x * xzOff_23.x) + (_V_CW_Bend.z * xzOff_23.y)) * 0.001);
					  v0_26 = (tmpvar_27 + tmpvar_31);
					  highp vec2 tmpvar_32;
					  tmpvar_32.x = float((tmpvar_28.z >= 0.0));
					  tmpvar_32.y = float((tmpvar_28.x >= 0.0));
					  xzOff_23 = (max (vec2(0.0, 0.0), (
					    abs(tmpvar_28.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_32 * 2.0) - 1.0));
					  xzOff_23 = (xzOff_23 * xzOff_23);
					  v1_25.y = (tmpvar_28.y + ((
					    (_V_CW_Bend.x * xzOff_23.x)
					   + 
					    (_V_CW_Bend.z * xzOff_23.y)
					  ) * 0.001));
					  highp vec2 tmpvar_33;
					  tmpvar_33.x = float((tmpvar_29.z >= 0.0));
					  tmpvar_33.y = float((tmpvar_29.x >= 0.0));
					  xzOff_23 = (max (vec2(0.0, 0.0), (
					    abs(tmpvar_29.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_33 * 2.0) - 1.0));
					  xzOff_23 = (xzOff_23 * xzOff_23);
					  v2_24.y = (tmpvar_29.y + ((
					    (_V_CW_Bend.x * xzOff_23.x)
					   + 
					    (_V_CW_Bend.z * xzOff_23.y)
					  ) * 0.001));
					  highp mat3 tmpvar_34;
					  tmpvar_34[0] = _World2Object[0].xyz;
					  tmpvar_34[1] = _World2Object[1].xyz;
					  tmpvar_34[2] = _World2Object[2].xyz;
					  vertex_22.xyz = (_glesVertex.xyz + (tmpvar_34 * tmpvar_31));
					  highp mat3 tmpvar_35;
					  tmpvar_35[0] = _World2Object[0].xyz;
					  tmpvar_35[1] = _World2Object[1].xyz;
					  tmpvar_35[2] = _World2Object[2].xyz;
					  highp vec3 a_36;
					  a_36 = (v2_24 - v0_26);
					  highp vec3 b_37;
					  b_37 = (v1_25 - v0_26);
					  highp vec3 tmpvar_38;
					  tmpvar_38 = normalize((tmpvar_35 * normalize(
					    ((a_36.yzx * b_37.zxy) - (a_36.zxy * b_37.yzx))
					  )));
					  tmpvar_2.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
					  tmpvar_2.zw = ((_glesMultiTexCoord0.xy * _V_CW_SecondaryTex_ST.xy) + _V_CW_SecondaryTex_ST.zw);
					  highp vec4 v_39;
					  v_39.x = tmpvar_5;
					  v_39.y = tmpvar_6;
					  v_39.z = tmpvar_7;
					  v_39.w = tmpvar_8;
					  highp vec4 v_40;
					  v_40.x = tmpvar_10;
					  v_40.y = tmpvar_11;
					  v_40.z = tmpvar_12;
					  v_40.w = tmpvar_13;
					  highp vec4 v_41;
					  v_41.x = tmpvar_15;
					  v_41.y = tmpvar_16;
					  v_41.z = tmpvar_17;
					  v_41.w = tmpvar_18;
					  highp vec3 tmpvar_42;
					  tmpvar_42 = normalize(((
					    (v_39.xyz * tmpvar_38.x)
					   + 
					    (v_40.xyz * tmpvar_38.y)
					  ) + (v_41.xyz * tmpvar_38.z)));
					  worldNormal_1 = tmpvar_42;
					  tmpvar_3 = worldNormal_1;
					  gl_Position = (glstate_matrix_mvp * vertex_22);
					  xlv_TEXCOORD0 = tmpvar_2;
					  xlv_TEXCOORD1 = tmpvar_3;
					  xlv_TEXCOORD2 = (_Object2World * vertex_22).xyz;
					  xlv_COLOR0 = _glesColor;
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform highp vec4 _Time;
					uniform highp vec4 _WorldSpaceLightPos0;
					uniform lowp vec4 _LightColor0;
					uniform lowp samplerCube _LightTexture0;
					uniform mediump mat4 _LightMatrix0;
					uniform sampler2D _LightTextureB0;
					uniform sampler2D _MainTex;
					uniform lowp vec4 _Color;
					uniform lowp vec2 _V_CW_MainTex_Scroll;
					uniform sampler2D _V_CW_SecondaryTex;
					uniform lowp vec2 _V_CW_SecondaryTex_Scroll;
					uniform lowp float _V_CW_SecondaryTex_Blend;
					varying highp vec4 xlv_TEXCOORD0;
					varying mediump vec3 xlv_TEXCOORD1;
					varying highp vec3 xlv_TEXCOORD2;
					varying lowp vec4 xlv_COLOR0;
					void main ()
					{
					  mediump vec3 tmpvar_1;
					  mediump vec3 tmpvar_2;
					  lowp vec3 tmpvar_3;
					  lowp vec3 lightDir_4;
					  highp vec4 tmpvar_5;
					  highp vec3 tmpvar_6;
					  tmpvar_6 = normalize((_WorldSpaceLightPos0.xyz - xlv_TEXCOORD2));
					  lightDir_4 = tmpvar_6;
					  tmpvar_5 = xlv_COLOR0;
					  tmpvar_3 = xlv_TEXCOORD1;
					  lowp vec3 tmpvar_7;
					  lowp float vBlend_8;
					  highp vec2 P_9;
					  P_9 = (xlv_TEXCOORD0.xy + (_V_CW_MainTex_Scroll * _Time.x));
					  highp float tmpvar_10;
					  tmpvar_10 = clamp ((_V_CW_SecondaryTex_Blend + tmpvar_5.w), 0.0, 1.0);
					  vBlend_8 = tmpvar_10;
					  highp vec2 P_11;
					  P_11 = (xlv_TEXCOORD0.zw + (_V_CW_SecondaryTex_Scroll * _Time.x));
					  lowp vec4 tmpvar_12;
					  tmpvar_12 = mix (texture2D (_MainTex, P_9), texture2D (_V_CW_SecondaryTex, P_11), vec4(vBlend_8));
					  tmpvar_7 = (tmpvar_12.xyz * _Color.xyz);
					  tmpvar_7 = (tmpvar_7 * tmpvar_5.xyz);
					  highp vec4 tmpvar_13;
					  tmpvar_13.w = 1.0;
					  tmpvar_13.xyz = xlv_TEXCOORD2;
					  highp vec3 tmpvar_14;
					  tmpvar_14 = (_LightMatrix0 * tmpvar_13).xyz;
					  highp float tmpvar_15;
					  tmpvar_15 = dot (tmpvar_14, tmpvar_14);
					  lowp float tmpvar_16;
					  tmpvar_16 = (texture2D (_LightTextureB0, vec2(tmpvar_15)).w * textureCube (_LightTexture0, tmpvar_14).w);
					  tmpvar_1 = _LightColor0.xyz;
					  tmpvar_2 = lightDir_4;
					  tmpvar_1 = (tmpvar_1 * tmpvar_16);
					  lowp vec4 c_17;
					  lowp vec4 c_18;
					  lowp float diff_19;
					  mediump float tmpvar_20;
					  tmpvar_20 = max (0.0, dot (tmpvar_3, tmpvar_2));
					  diff_19 = tmpvar_20;
					  c_18.xyz = ((tmpvar_7 * tmpvar_1) * diff_19);
					  c_18.w = (tmpvar_12.w * _Color.w);
					  c_17.w = c_18.w;
					  c_17.xyz = c_18.xyz;
					  gl_FragData[0] = c_17;
					}
					
					
					#endif"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL_COOKIE" "V_CW_VERTEX_COLOR" "V_CW_REFLECTIVE_OFF" "_EMISSION_OFF" "V_CW_RIM_OFF" "_NORMALMAP_OFF" "V_CW_SPECULAR_OFF" }
					"!!GLES
					#version 100
					
					#ifdef VERTEX
					attribute vec4 _glesTANGENT;
					attribute vec4 _glesVertex;
					attribute vec4 _glesColor;
					attribute vec3 _glesNormal;
					attribute vec4 _glesMultiTexCoord0;
					uniform highp mat4 glstate_matrix_mvp;
					uniform highp mat4 _Object2World;
					uniform highp mat4 _World2Object;
					uniform highp vec3 _V_CW_Bend;
					uniform highp vec3 _V_CW_Bias;
					uniform highp vec4 _V_CW_PivotPoint_Position;
					uniform highp vec4 _MainTex_ST;
					uniform highp vec4 _V_CW_SecondaryTex_ST;
					varying highp vec4 xlv_TEXCOORD0;
					varying mediump vec3 xlv_TEXCOORD1;
					varying highp vec3 xlv_TEXCOORD2;
					varying lowp vec4 xlv_COLOR0;
					void main ()
					{
					  lowp vec3 worldNormal_1;
					  highp vec4 tmpvar_2;
					  mediump vec3 tmpvar_3;
					  highp vec4 v_4;
					  highp float tmpvar_5;
					  tmpvar_5 = _World2Object[0].x;
					  v_4.x = tmpvar_5;
					  highp float tmpvar_6;
					  tmpvar_6 = _World2Object[1].x;
					  v_4.y = tmpvar_6;
					  highp float tmpvar_7;
					  tmpvar_7 = _World2Object[2].x;
					  v_4.z = tmpvar_7;
					  highp float tmpvar_8;
					  tmpvar_8 = _World2Object[3].x;
					  v_4.w = tmpvar_8;
					  highp vec4 v_9;
					  highp float tmpvar_10;
					  tmpvar_10 = _World2Object[0].y;
					  v_9.x = tmpvar_10;
					  highp float tmpvar_11;
					  tmpvar_11 = _World2Object[1].y;
					  v_9.y = tmpvar_11;
					  highp float tmpvar_12;
					  tmpvar_12 = _World2Object[2].y;
					  v_9.z = tmpvar_12;
					  highp float tmpvar_13;
					  tmpvar_13 = _World2Object[3].y;
					  v_9.w = tmpvar_13;
					  highp vec4 v_14;
					  highp float tmpvar_15;
					  tmpvar_15 = _World2Object[0].z;
					  v_14.x = tmpvar_15;
					  highp float tmpvar_16;
					  tmpvar_16 = _World2Object[1].z;
					  v_14.y = tmpvar_16;
					  highp float tmpvar_17;
					  tmpvar_17 = _World2Object[2].z;
					  v_14.z = tmpvar_17;
					  highp float tmpvar_18;
					  tmpvar_18 = _World2Object[3].z;
					  v_14.w = tmpvar_18;
					  highp vec3 tmpvar_19;
					  tmpvar_19 = normalize(((
					    (v_4.xyz * _glesNormal.x)
					   + 
					    (v_9.xyz * _glesNormal.y)
					  ) + (v_14.xyz * _glesNormal.z)));
					  highp mat3 tmpvar_20;
					  tmpvar_20[0] = _Object2World[0].xyz;
					  tmpvar_20[1] = _Object2World[1].xyz;
					  tmpvar_20[2] = _Object2World[2].xyz;
					  highp vec3 tmpvar_21;
					  tmpvar_21 = normalize((tmpvar_20 * _glesTANGENT.xyz));
					  highp vec4 vertex_22;
					  vertex_22.w = _glesVertex.w;
					  highp vec2 xzOff_23;
					  highp vec3 v2_24;
					  highp vec3 v1_25;
					  highp vec3 v0_26;
					  highp vec3 tmpvar_27;
					  tmpvar_27 = ((_Object2World * _glesVertex).xyz - _V_CW_PivotPoint_Position.xyz);
					  highp vec3 tmpvar_28;
					  tmpvar_28 = (tmpvar_27 + tmpvar_21);
					  v1_25.xz = tmpvar_28.xz;
					  highp vec3 tmpvar_29;
					  tmpvar_29 = (tmpvar_27 - ((tmpvar_19.yzx * tmpvar_21.zxy) - (tmpvar_19.zxy * tmpvar_21.yzx)));
					  v2_24.xz = tmpvar_29.xz;
					  highp vec2 tmpvar_30;
					  tmpvar_30.x = float((tmpvar_27.z >= 0.0));
					  tmpvar_30.y = float((tmpvar_27.x >= 0.0));
					  xzOff_23 = (max (vec2(0.0, 0.0), (
					    abs(tmpvar_27.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_30 * 2.0) - 1.0));
					  xzOff_23 = (xzOff_23 * xzOff_23);
					  highp vec3 tmpvar_31;
					  tmpvar_31.xz = vec2(0.0, 0.0);
					  tmpvar_31.y = (((_V_CW_Bend.x * xzOff_23.x) + (_V_CW_Bend.z * xzOff_23.y)) * 0.001);
					  v0_26 = (tmpvar_27 + tmpvar_31);
					  highp vec2 tmpvar_32;
					  tmpvar_32.x = float((tmpvar_28.z >= 0.0));
					  tmpvar_32.y = float((tmpvar_28.x >= 0.0));
					  xzOff_23 = (max (vec2(0.0, 0.0), (
					    abs(tmpvar_28.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_32 * 2.0) - 1.0));
					  xzOff_23 = (xzOff_23 * xzOff_23);
					  v1_25.y = (tmpvar_28.y + ((
					    (_V_CW_Bend.x * xzOff_23.x)
					   + 
					    (_V_CW_Bend.z * xzOff_23.y)
					  ) * 0.001));
					  highp vec2 tmpvar_33;
					  tmpvar_33.x = float((tmpvar_29.z >= 0.0));
					  tmpvar_33.y = float((tmpvar_29.x >= 0.0));
					  xzOff_23 = (max (vec2(0.0, 0.0), (
					    abs(tmpvar_29.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_33 * 2.0) - 1.0));
					  xzOff_23 = (xzOff_23 * xzOff_23);
					  v2_24.y = (tmpvar_29.y + ((
					    (_V_CW_Bend.x * xzOff_23.x)
					   + 
					    (_V_CW_Bend.z * xzOff_23.y)
					  ) * 0.001));
					  highp mat3 tmpvar_34;
					  tmpvar_34[0] = _World2Object[0].xyz;
					  tmpvar_34[1] = _World2Object[1].xyz;
					  tmpvar_34[2] = _World2Object[2].xyz;
					  vertex_22.xyz = (_glesVertex.xyz + (tmpvar_34 * tmpvar_31));
					  highp mat3 tmpvar_35;
					  tmpvar_35[0] = _World2Object[0].xyz;
					  tmpvar_35[1] = _World2Object[1].xyz;
					  tmpvar_35[2] = _World2Object[2].xyz;
					  highp vec3 a_36;
					  a_36 = (v2_24 - v0_26);
					  highp vec3 b_37;
					  b_37 = (v1_25 - v0_26);
					  highp vec3 tmpvar_38;
					  tmpvar_38 = normalize((tmpvar_35 * normalize(
					    ((a_36.yzx * b_37.zxy) - (a_36.zxy * b_37.yzx))
					  )));
					  tmpvar_2.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
					  tmpvar_2.zw = ((_glesMultiTexCoord0.xy * _V_CW_SecondaryTex_ST.xy) + _V_CW_SecondaryTex_ST.zw);
					  highp vec4 v_39;
					  v_39.x = tmpvar_5;
					  v_39.y = tmpvar_6;
					  v_39.z = tmpvar_7;
					  v_39.w = tmpvar_8;
					  highp vec4 v_40;
					  v_40.x = tmpvar_10;
					  v_40.y = tmpvar_11;
					  v_40.z = tmpvar_12;
					  v_40.w = tmpvar_13;
					  highp vec4 v_41;
					  v_41.x = tmpvar_15;
					  v_41.y = tmpvar_16;
					  v_41.z = tmpvar_17;
					  v_41.w = tmpvar_18;
					  highp vec3 tmpvar_42;
					  tmpvar_42 = normalize(((
					    (v_39.xyz * tmpvar_38.x)
					   + 
					    (v_40.xyz * tmpvar_38.y)
					  ) + (v_41.xyz * tmpvar_38.z)));
					  worldNormal_1 = tmpvar_42;
					  tmpvar_3 = worldNormal_1;
					  gl_Position = (glstate_matrix_mvp * vertex_22);
					  xlv_TEXCOORD0 = tmpvar_2;
					  xlv_TEXCOORD1 = tmpvar_3;
					  xlv_TEXCOORD2 = (_Object2World * vertex_22).xyz;
					  xlv_COLOR0 = _glesColor;
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform highp vec4 _Time;
					uniform mediump vec4 _WorldSpaceLightPos0;
					uniform lowp vec4 _LightColor0;
					uniform sampler2D _LightTexture0;
					uniform mediump mat4 _LightMatrix0;
					uniform sampler2D _MainTex;
					uniform lowp vec4 _Color;
					uniform lowp vec2 _V_CW_MainTex_Scroll;
					uniform sampler2D _V_CW_SecondaryTex;
					uniform lowp vec2 _V_CW_SecondaryTex_Scroll;
					uniform lowp float _V_CW_SecondaryTex_Blend;
					varying highp vec4 xlv_TEXCOORD0;
					varying mediump vec3 xlv_TEXCOORD1;
					varying highp vec3 xlv_TEXCOORD2;
					varying lowp vec4 xlv_COLOR0;
					void main ()
					{
					  mediump vec3 tmpvar_1;
					  mediump vec3 tmpvar_2;
					  lowp vec3 tmpvar_3;
					  lowp vec3 lightDir_4;
					  highp vec4 tmpvar_5;
					  mediump vec3 tmpvar_6;
					  tmpvar_6 = _WorldSpaceLightPos0.xyz;
					  lightDir_4 = tmpvar_6;
					  tmpvar_5 = xlv_COLOR0;
					  tmpvar_3 = xlv_TEXCOORD1;
					  lowp vec3 tmpvar_7;
					  lowp float vBlend_8;
					  highp vec2 P_9;
					  P_9 = (xlv_TEXCOORD0.xy + (_V_CW_MainTex_Scroll * _Time.x));
					  highp float tmpvar_10;
					  tmpvar_10 = clamp ((_V_CW_SecondaryTex_Blend + tmpvar_5.w), 0.0, 1.0);
					  vBlend_8 = tmpvar_10;
					  highp vec2 P_11;
					  P_11 = (xlv_TEXCOORD0.zw + (_V_CW_SecondaryTex_Scroll * _Time.x));
					  lowp vec4 tmpvar_12;
					  tmpvar_12 = mix (texture2D (_MainTex, P_9), texture2D (_V_CW_SecondaryTex, P_11), vec4(vBlend_8));
					  tmpvar_7 = (tmpvar_12.xyz * _Color.xyz);
					  tmpvar_7 = (tmpvar_7 * tmpvar_5.xyz);
					  highp vec4 tmpvar_13;
					  tmpvar_13.w = 1.0;
					  tmpvar_13.xyz = xlv_TEXCOORD2;
					  highp vec2 tmpvar_14;
					  tmpvar_14 = (_LightMatrix0 * tmpvar_13).xy;
					  lowp float tmpvar_15;
					  tmpvar_15 = texture2D (_LightTexture0, tmpvar_14).w;
					  tmpvar_1 = _LightColor0.xyz;
					  tmpvar_2 = lightDir_4;
					  tmpvar_1 = (tmpvar_1 * tmpvar_15);
					  lowp vec4 c_16;
					  lowp vec4 c_17;
					  lowp float diff_18;
					  mediump float tmpvar_19;
					  tmpvar_19 = max (0.0, dot (tmpvar_3, tmpvar_2));
					  diff_18 = tmpvar_19;
					  c_17.xyz = ((tmpvar_7 * tmpvar_1) * diff_18);
					  c_17.w = (tmpvar_12.w * _Color.w);
					  c_16.w = c_17.w;
					  c_16.xyz = c_17.xyz;
					  gl_FragData[0] = c_16;
					}
					
					
					#endif"
}
SubProgram "gles " {
Keywords { "POINT" "FOG_LINEAR" "V_CW_VERTEX_COLOR" "V_CW_REFLECTIVE_OFF" "_EMISSION_OFF" "V_CW_RIM_OFF" "_NORMALMAP_OFF" "V_CW_SPECULAR_OFF" }
					"!!GLES
					#version 100
					
					#ifdef VERTEX
					attribute vec4 _glesTANGENT;
					attribute vec4 _glesVertex;
					attribute vec4 _glesColor;
					attribute vec3 _glesNormal;
					attribute vec4 _glesMultiTexCoord0;
					uniform highp mat4 glstate_matrix_mvp;
					uniform highp mat4 _Object2World;
					uniform highp mat4 _World2Object;
					uniform highp vec4 unity_FogParams;
					uniform highp vec3 _V_CW_Bend;
					uniform highp vec3 _V_CW_Bias;
					uniform highp vec4 _V_CW_PivotPoint_Position;
					uniform highp vec4 _MainTex_ST;
					uniform highp vec4 _V_CW_SecondaryTex_ST;
					varying highp vec4 xlv_TEXCOORD0;
					varying mediump vec3 xlv_TEXCOORD1;
					varying highp vec3 xlv_TEXCOORD2;
					varying lowp vec4 xlv_COLOR0;
					varying highp float xlv_TEXCOORD3;
					void main ()
					{
					  lowp vec3 worldNormal_1;
					  highp vec4 tmpvar_2;
					  highp vec4 tmpvar_3;
					  mediump vec3 tmpvar_4;
					  highp vec4 v_5;
					  highp float tmpvar_6;
					  tmpvar_6 = _World2Object[0].x;
					  v_5.x = tmpvar_6;
					  highp float tmpvar_7;
					  tmpvar_7 = _World2Object[1].x;
					  v_5.y = tmpvar_7;
					  highp float tmpvar_8;
					  tmpvar_8 = _World2Object[2].x;
					  v_5.z = tmpvar_8;
					  highp float tmpvar_9;
					  tmpvar_9 = _World2Object[3].x;
					  v_5.w = tmpvar_9;
					  highp vec4 v_10;
					  highp float tmpvar_11;
					  tmpvar_11 = _World2Object[0].y;
					  v_10.x = tmpvar_11;
					  highp float tmpvar_12;
					  tmpvar_12 = _World2Object[1].y;
					  v_10.y = tmpvar_12;
					  highp float tmpvar_13;
					  tmpvar_13 = _World2Object[2].y;
					  v_10.z = tmpvar_13;
					  highp float tmpvar_14;
					  tmpvar_14 = _World2Object[3].y;
					  v_10.w = tmpvar_14;
					  highp vec4 v_15;
					  highp float tmpvar_16;
					  tmpvar_16 = _World2Object[0].z;
					  v_15.x = tmpvar_16;
					  highp float tmpvar_17;
					  tmpvar_17 = _World2Object[1].z;
					  v_15.y = tmpvar_17;
					  highp float tmpvar_18;
					  tmpvar_18 = _World2Object[2].z;
					  v_15.z = tmpvar_18;
					  highp float tmpvar_19;
					  tmpvar_19 = _World2Object[3].z;
					  v_15.w = tmpvar_19;
					  highp vec3 tmpvar_20;
					  tmpvar_20 = normalize(((
					    (v_5.xyz * _glesNormal.x)
					   + 
					    (v_10.xyz * _glesNormal.y)
					  ) + (v_15.xyz * _glesNormal.z)));
					  highp mat3 tmpvar_21;
					  tmpvar_21[0] = _Object2World[0].xyz;
					  tmpvar_21[1] = _Object2World[1].xyz;
					  tmpvar_21[2] = _Object2World[2].xyz;
					  highp vec3 tmpvar_22;
					  tmpvar_22 = normalize((tmpvar_21 * _glesTANGENT.xyz));
					  highp vec4 vertex_23;
					  vertex_23.w = _glesVertex.w;
					  highp vec2 xzOff_24;
					  highp vec3 v2_25;
					  highp vec3 v1_26;
					  highp vec3 v0_27;
					  highp vec3 tmpvar_28;
					  tmpvar_28 = ((_Object2World * _glesVertex).xyz - _V_CW_PivotPoint_Position.xyz);
					  highp vec3 tmpvar_29;
					  tmpvar_29 = (tmpvar_28 + tmpvar_22);
					  v1_26.xz = tmpvar_29.xz;
					  highp vec3 tmpvar_30;
					  tmpvar_30 = (tmpvar_28 - ((tmpvar_20.yzx * tmpvar_22.zxy) - (tmpvar_20.zxy * tmpvar_22.yzx)));
					  v2_25.xz = tmpvar_30.xz;
					  highp vec2 tmpvar_31;
					  tmpvar_31.x = float((tmpvar_28.z >= 0.0));
					  tmpvar_31.y = float((tmpvar_28.x >= 0.0));
					  xzOff_24 = (max (vec2(0.0, 0.0), (
					    abs(tmpvar_28.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_31 * 2.0) - 1.0));
					  xzOff_24 = (xzOff_24 * xzOff_24);
					  highp vec3 tmpvar_32;
					  tmpvar_32.xz = vec2(0.0, 0.0);
					  tmpvar_32.y = (((_V_CW_Bend.x * xzOff_24.x) + (_V_CW_Bend.z * xzOff_24.y)) * 0.001);
					  v0_27 = (tmpvar_28 + tmpvar_32);
					  highp vec2 tmpvar_33;
					  tmpvar_33.x = float((tmpvar_29.z >= 0.0));
					  tmpvar_33.y = float((tmpvar_29.x >= 0.0));
					  xzOff_24 = (max (vec2(0.0, 0.0), (
					    abs(tmpvar_29.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_33 * 2.0) - 1.0));
					  xzOff_24 = (xzOff_24 * xzOff_24);
					  v1_26.y = (tmpvar_29.y + ((
					    (_V_CW_Bend.x * xzOff_24.x)
					   + 
					    (_V_CW_Bend.z * xzOff_24.y)
					  ) * 0.001));
					  highp vec2 tmpvar_34;
					  tmpvar_34.x = float((tmpvar_30.z >= 0.0));
					  tmpvar_34.y = float((tmpvar_30.x >= 0.0));
					  xzOff_24 = (max (vec2(0.0, 0.0), (
					    abs(tmpvar_30.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_34 * 2.0) - 1.0));
					  xzOff_24 = (xzOff_24 * xzOff_24);
					  v2_25.y = (tmpvar_30.y + ((
					    (_V_CW_Bend.x * xzOff_24.x)
					   + 
					    (_V_CW_Bend.z * xzOff_24.y)
					  ) * 0.001));
					  highp mat3 tmpvar_35;
					  tmpvar_35[0] = _World2Object[0].xyz;
					  tmpvar_35[1] = _World2Object[1].xyz;
					  tmpvar_35[2] = _World2Object[2].xyz;
					  vertex_23.xyz = (_glesVertex.xyz + (tmpvar_35 * tmpvar_32));
					  highp mat3 tmpvar_36;
					  tmpvar_36[0] = _World2Object[0].xyz;
					  tmpvar_36[1] = _World2Object[1].xyz;
					  tmpvar_36[2] = _World2Object[2].xyz;
					  highp vec3 a_37;
					  a_37 = (v2_25 - v0_27);
					  highp vec3 b_38;
					  b_38 = (v1_26 - v0_27);
					  highp vec3 tmpvar_39;
					  tmpvar_39 = normalize((tmpvar_36 * normalize(
					    ((a_37.yzx * b_38.zxy) - (a_37.zxy * b_38.yzx))
					  )));
					  tmpvar_2 = (glstate_matrix_mvp * vertex_23);
					  tmpvar_3.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
					  tmpvar_3.zw = ((_glesMultiTexCoord0.xy * _V_CW_SecondaryTex_ST.xy) + _V_CW_SecondaryTex_ST.zw);
					  highp vec4 v_40;
					  v_40.x = tmpvar_6;
					  v_40.y = tmpvar_7;
					  v_40.z = tmpvar_8;
					  v_40.w = tmpvar_9;
					  highp vec4 v_41;
					  v_41.x = tmpvar_11;
					  v_41.y = tmpvar_12;
					  v_41.z = tmpvar_13;
					  v_41.w = tmpvar_14;
					  highp vec4 v_42;
					  v_42.x = tmpvar_16;
					  v_42.y = tmpvar_17;
					  v_42.z = tmpvar_18;
					  v_42.w = tmpvar_19;
					  highp vec3 tmpvar_43;
					  tmpvar_43 = normalize(((
					    (v_40.xyz * tmpvar_39.x)
					   + 
					    (v_41.xyz * tmpvar_39.y)
					  ) + (v_42.xyz * tmpvar_39.z)));
					  worldNormal_1 = tmpvar_43;
					  tmpvar_4 = worldNormal_1;
					  gl_Position = tmpvar_2;
					  xlv_TEXCOORD0 = tmpvar_3;
					  xlv_TEXCOORD1 = tmpvar_4;
					  xlv_TEXCOORD2 = (_Object2World * vertex_23).xyz;
					  xlv_COLOR0 = _glesColor;
					  xlv_TEXCOORD3 = ((tmpvar_2.z * unity_FogParams.z) + unity_FogParams.w);
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform highp vec4 _Time;
					uniform highp vec4 _WorldSpaceLightPos0;
					uniform lowp vec4 _LightColor0;
					uniform sampler2D _LightTexture0;
					uniform mediump mat4 _LightMatrix0;
					uniform sampler2D _MainTex;
					uniform lowp vec4 _Color;
					uniform lowp vec2 _V_CW_MainTex_Scroll;
					uniform sampler2D _V_CW_SecondaryTex;
					uniform lowp vec2 _V_CW_SecondaryTex_Scroll;
					uniform lowp float _V_CW_SecondaryTex_Blend;
					varying highp vec4 xlv_TEXCOORD0;
					varying mediump vec3 xlv_TEXCOORD1;
					varying highp vec3 xlv_TEXCOORD2;
					varying lowp vec4 xlv_COLOR0;
					varying highp float xlv_TEXCOORD3;
					void main ()
					{
					  mediump vec3 tmpvar_1;
					  mediump vec3 tmpvar_2;
					  lowp vec4 c_3;
					  lowp vec3 tmpvar_4;
					  lowp vec3 lightDir_5;
					  highp vec4 tmpvar_6;
					  highp vec3 tmpvar_7;
					  tmpvar_7 = normalize((_WorldSpaceLightPos0.xyz - xlv_TEXCOORD2));
					  lightDir_5 = tmpvar_7;
					  tmpvar_6 = xlv_COLOR0;
					  tmpvar_4 = xlv_TEXCOORD1;
					  lowp vec3 tmpvar_8;
					  lowp float vBlend_9;
					  highp vec2 P_10;
					  P_10 = (xlv_TEXCOORD0.xy + (_V_CW_MainTex_Scroll * _Time.x));
					  highp float tmpvar_11;
					  tmpvar_11 = clamp ((_V_CW_SecondaryTex_Blend + tmpvar_6.w), 0.0, 1.0);
					  vBlend_9 = tmpvar_11;
					  highp vec2 P_12;
					  P_12 = (xlv_TEXCOORD0.zw + (_V_CW_SecondaryTex_Scroll * _Time.x));
					  lowp vec4 tmpvar_13;
					  tmpvar_13 = mix (texture2D (_MainTex, P_10), texture2D (_V_CW_SecondaryTex, P_12), vec4(vBlend_9));
					  tmpvar_8 = (tmpvar_13.xyz * _Color.xyz);
					  tmpvar_8 = (tmpvar_8 * tmpvar_6.xyz);
					  highp vec4 tmpvar_14;
					  tmpvar_14.w = 1.0;
					  tmpvar_14.xyz = xlv_TEXCOORD2;
					  highp vec3 tmpvar_15;
					  tmpvar_15 = (_LightMatrix0 * tmpvar_14).xyz;
					  highp float tmpvar_16;
					  tmpvar_16 = dot (tmpvar_15, tmpvar_15);
					  lowp float tmpvar_17;
					  tmpvar_17 = texture2D (_LightTexture0, vec2(tmpvar_16)).w;
					  tmpvar_1 = _LightColor0.xyz;
					  tmpvar_2 = lightDir_5;
					  tmpvar_1 = (tmpvar_1 * tmpvar_17);
					  lowp vec4 c_18;
					  lowp vec4 c_19;
					  lowp float diff_20;
					  mediump float tmpvar_21;
					  tmpvar_21 = max (0.0, dot (tmpvar_4, tmpvar_2));
					  diff_20 = tmpvar_21;
					  c_19.xyz = ((tmpvar_8 * tmpvar_1) * diff_20);
					  c_19.w = (tmpvar_13.w * _Color.w);
					  c_18.w = c_19.w;
					  c_18.xyz = c_19.xyz;
					  c_3.w = c_18.w;
					  highp float tmpvar_22;
					  tmpvar_22 = clamp (xlv_TEXCOORD3, 0.0, 1.0);
					  c_3.xyz = (c_19.xyz * vec3(tmpvar_22));
					  gl_FragData[0] = c_3;
					}
					
					
					#endif"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "FOG_LINEAR" "V_CW_VERTEX_COLOR" "V_CW_REFLECTIVE_OFF" "_EMISSION_OFF" "V_CW_RIM_OFF" "_NORMALMAP_OFF" "V_CW_SPECULAR_OFF" }
					"!!GLES
					#version 100
					
					#ifdef VERTEX
					attribute vec4 _glesTANGENT;
					attribute vec4 _glesVertex;
					attribute vec4 _glesColor;
					attribute vec3 _glesNormal;
					attribute vec4 _glesMultiTexCoord0;
					uniform highp mat4 glstate_matrix_mvp;
					uniform highp mat4 _Object2World;
					uniform highp mat4 _World2Object;
					uniform highp vec4 unity_FogParams;
					uniform highp vec3 _V_CW_Bend;
					uniform highp vec3 _V_CW_Bias;
					uniform highp vec4 _V_CW_PivotPoint_Position;
					uniform highp vec4 _MainTex_ST;
					uniform highp vec4 _V_CW_SecondaryTex_ST;
					varying highp vec4 xlv_TEXCOORD0;
					varying mediump vec3 xlv_TEXCOORD1;
					varying highp vec3 xlv_TEXCOORD2;
					varying lowp vec4 xlv_COLOR0;
					varying highp float xlv_TEXCOORD3;
					void main ()
					{
					  lowp vec3 worldNormal_1;
					  highp vec4 tmpvar_2;
					  highp vec4 tmpvar_3;
					  mediump vec3 tmpvar_4;
					  highp vec4 v_5;
					  highp float tmpvar_6;
					  tmpvar_6 = _World2Object[0].x;
					  v_5.x = tmpvar_6;
					  highp float tmpvar_7;
					  tmpvar_7 = _World2Object[1].x;
					  v_5.y = tmpvar_7;
					  highp float tmpvar_8;
					  tmpvar_8 = _World2Object[2].x;
					  v_5.z = tmpvar_8;
					  highp float tmpvar_9;
					  tmpvar_9 = _World2Object[3].x;
					  v_5.w = tmpvar_9;
					  highp vec4 v_10;
					  highp float tmpvar_11;
					  tmpvar_11 = _World2Object[0].y;
					  v_10.x = tmpvar_11;
					  highp float tmpvar_12;
					  tmpvar_12 = _World2Object[1].y;
					  v_10.y = tmpvar_12;
					  highp float tmpvar_13;
					  tmpvar_13 = _World2Object[2].y;
					  v_10.z = tmpvar_13;
					  highp float tmpvar_14;
					  tmpvar_14 = _World2Object[3].y;
					  v_10.w = tmpvar_14;
					  highp vec4 v_15;
					  highp float tmpvar_16;
					  tmpvar_16 = _World2Object[0].z;
					  v_15.x = tmpvar_16;
					  highp float tmpvar_17;
					  tmpvar_17 = _World2Object[1].z;
					  v_15.y = tmpvar_17;
					  highp float tmpvar_18;
					  tmpvar_18 = _World2Object[2].z;
					  v_15.z = tmpvar_18;
					  highp float tmpvar_19;
					  tmpvar_19 = _World2Object[3].z;
					  v_15.w = tmpvar_19;
					  highp vec3 tmpvar_20;
					  tmpvar_20 = normalize(((
					    (v_5.xyz * _glesNormal.x)
					   + 
					    (v_10.xyz * _glesNormal.y)
					  ) + (v_15.xyz * _glesNormal.z)));
					  highp mat3 tmpvar_21;
					  tmpvar_21[0] = _Object2World[0].xyz;
					  tmpvar_21[1] = _Object2World[1].xyz;
					  tmpvar_21[2] = _Object2World[2].xyz;
					  highp vec3 tmpvar_22;
					  tmpvar_22 = normalize((tmpvar_21 * _glesTANGENT.xyz));
					  highp vec4 vertex_23;
					  vertex_23.w = _glesVertex.w;
					  highp vec2 xzOff_24;
					  highp vec3 v2_25;
					  highp vec3 v1_26;
					  highp vec3 v0_27;
					  highp vec3 tmpvar_28;
					  tmpvar_28 = ((_Object2World * _glesVertex).xyz - _V_CW_PivotPoint_Position.xyz);
					  highp vec3 tmpvar_29;
					  tmpvar_29 = (tmpvar_28 + tmpvar_22);
					  v1_26.xz = tmpvar_29.xz;
					  highp vec3 tmpvar_30;
					  tmpvar_30 = (tmpvar_28 - ((tmpvar_20.yzx * tmpvar_22.zxy) - (tmpvar_20.zxy * tmpvar_22.yzx)));
					  v2_25.xz = tmpvar_30.xz;
					  highp vec2 tmpvar_31;
					  tmpvar_31.x = float((tmpvar_28.z >= 0.0));
					  tmpvar_31.y = float((tmpvar_28.x >= 0.0));
					  xzOff_24 = (max (vec2(0.0, 0.0), (
					    abs(tmpvar_28.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_31 * 2.0) - 1.0));
					  xzOff_24 = (xzOff_24 * xzOff_24);
					  highp vec3 tmpvar_32;
					  tmpvar_32.xz = vec2(0.0, 0.0);
					  tmpvar_32.y = (((_V_CW_Bend.x * xzOff_24.x) + (_V_CW_Bend.z * xzOff_24.y)) * 0.001);
					  v0_27 = (tmpvar_28 + tmpvar_32);
					  highp vec2 tmpvar_33;
					  tmpvar_33.x = float((tmpvar_29.z >= 0.0));
					  tmpvar_33.y = float((tmpvar_29.x >= 0.0));
					  xzOff_24 = (max (vec2(0.0, 0.0), (
					    abs(tmpvar_29.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_33 * 2.0) - 1.0));
					  xzOff_24 = (xzOff_24 * xzOff_24);
					  v1_26.y = (tmpvar_29.y + ((
					    (_V_CW_Bend.x * xzOff_24.x)
					   + 
					    (_V_CW_Bend.z * xzOff_24.y)
					  ) * 0.001));
					  highp vec2 tmpvar_34;
					  tmpvar_34.x = float((tmpvar_30.z >= 0.0));
					  tmpvar_34.y = float((tmpvar_30.x >= 0.0));
					  xzOff_24 = (max (vec2(0.0, 0.0), (
					    abs(tmpvar_30.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_34 * 2.0) - 1.0));
					  xzOff_24 = (xzOff_24 * xzOff_24);
					  v2_25.y = (tmpvar_30.y + ((
					    (_V_CW_Bend.x * xzOff_24.x)
					   + 
					    (_V_CW_Bend.z * xzOff_24.y)
					  ) * 0.001));
					  highp mat3 tmpvar_35;
					  tmpvar_35[0] = _World2Object[0].xyz;
					  tmpvar_35[1] = _World2Object[1].xyz;
					  tmpvar_35[2] = _World2Object[2].xyz;
					  vertex_23.xyz = (_glesVertex.xyz + (tmpvar_35 * tmpvar_32));
					  highp mat3 tmpvar_36;
					  tmpvar_36[0] = _World2Object[0].xyz;
					  tmpvar_36[1] = _World2Object[1].xyz;
					  tmpvar_36[2] = _World2Object[2].xyz;
					  highp vec3 a_37;
					  a_37 = (v2_25 - v0_27);
					  highp vec3 b_38;
					  b_38 = (v1_26 - v0_27);
					  highp vec3 tmpvar_39;
					  tmpvar_39 = normalize((tmpvar_36 * normalize(
					    ((a_37.yzx * b_38.zxy) - (a_37.zxy * b_38.yzx))
					  )));
					  tmpvar_2 = (glstate_matrix_mvp * vertex_23);
					  tmpvar_3.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
					  tmpvar_3.zw = ((_glesMultiTexCoord0.xy * _V_CW_SecondaryTex_ST.xy) + _V_CW_SecondaryTex_ST.zw);
					  highp vec4 v_40;
					  v_40.x = tmpvar_6;
					  v_40.y = tmpvar_7;
					  v_40.z = tmpvar_8;
					  v_40.w = tmpvar_9;
					  highp vec4 v_41;
					  v_41.x = tmpvar_11;
					  v_41.y = tmpvar_12;
					  v_41.z = tmpvar_13;
					  v_41.w = tmpvar_14;
					  highp vec4 v_42;
					  v_42.x = tmpvar_16;
					  v_42.y = tmpvar_17;
					  v_42.z = tmpvar_18;
					  v_42.w = tmpvar_19;
					  highp vec3 tmpvar_43;
					  tmpvar_43 = normalize(((
					    (v_40.xyz * tmpvar_39.x)
					   + 
					    (v_41.xyz * tmpvar_39.y)
					  ) + (v_42.xyz * tmpvar_39.z)));
					  worldNormal_1 = tmpvar_43;
					  tmpvar_4 = worldNormal_1;
					  gl_Position = tmpvar_2;
					  xlv_TEXCOORD0 = tmpvar_3;
					  xlv_TEXCOORD1 = tmpvar_4;
					  xlv_TEXCOORD2 = (_Object2World * vertex_23).xyz;
					  xlv_COLOR0 = _glesColor;
					  xlv_TEXCOORD3 = ((tmpvar_2.z * unity_FogParams.z) + unity_FogParams.w);
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform highp vec4 _Time;
					uniform mediump vec4 _WorldSpaceLightPos0;
					uniform lowp vec4 _LightColor0;
					uniform sampler2D _MainTex;
					uniform lowp vec4 _Color;
					uniform lowp vec2 _V_CW_MainTex_Scroll;
					uniform sampler2D _V_CW_SecondaryTex;
					uniform lowp vec2 _V_CW_SecondaryTex_Scroll;
					uniform lowp float _V_CW_SecondaryTex_Blend;
					varying highp vec4 xlv_TEXCOORD0;
					varying mediump vec3 xlv_TEXCOORD1;
					varying lowp vec4 xlv_COLOR0;
					varying highp float xlv_TEXCOORD3;
					void main ()
					{
					  mediump vec3 tmpvar_1;
					  mediump vec3 tmpvar_2;
					  lowp vec4 c_3;
					  lowp vec3 tmpvar_4;
					  lowp vec3 lightDir_5;
					  highp vec4 tmpvar_6;
					  mediump vec3 tmpvar_7;
					  tmpvar_7 = _WorldSpaceLightPos0.xyz;
					  lightDir_5 = tmpvar_7;
					  tmpvar_6 = xlv_COLOR0;
					  tmpvar_4 = xlv_TEXCOORD1;
					  lowp vec3 tmpvar_8;
					  lowp float vBlend_9;
					  highp vec2 P_10;
					  P_10 = (xlv_TEXCOORD0.xy + (_V_CW_MainTex_Scroll * _Time.x));
					  highp float tmpvar_11;
					  tmpvar_11 = clamp ((_V_CW_SecondaryTex_Blend + tmpvar_6.w), 0.0, 1.0);
					  vBlend_9 = tmpvar_11;
					  highp vec2 P_12;
					  P_12 = (xlv_TEXCOORD0.zw + (_V_CW_SecondaryTex_Scroll * _Time.x));
					  lowp vec4 tmpvar_13;
					  tmpvar_13 = mix (texture2D (_MainTex, P_10), texture2D (_V_CW_SecondaryTex, P_12), vec4(vBlend_9));
					  tmpvar_8 = (tmpvar_13.xyz * _Color.xyz);
					  tmpvar_8 = (tmpvar_8 * tmpvar_6.xyz);
					  tmpvar_1 = _LightColor0.xyz;
					  tmpvar_2 = lightDir_5;
					  lowp vec4 c_14;
					  lowp vec4 c_15;
					  lowp float diff_16;
					  mediump float tmpvar_17;
					  tmpvar_17 = max (0.0, dot (tmpvar_4, tmpvar_2));
					  diff_16 = tmpvar_17;
					  c_15.xyz = ((tmpvar_8 * tmpvar_1) * diff_16);
					  c_15.w = (tmpvar_13.w * _Color.w);
					  c_14.w = c_15.w;
					  c_14.xyz = c_15.xyz;
					  c_3.w = c_14.w;
					  highp float tmpvar_18;
					  tmpvar_18 = clamp (xlv_TEXCOORD3, 0.0, 1.0);
					  c_3.xyz = (c_15.xyz * vec3(tmpvar_18));
					  gl_FragData[0] = c_3;
					}
					
					
					#endif"
}
SubProgram "gles " {
Keywords { "SPOT" "FOG_LINEAR" "V_CW_VERTEX_COLOR" "V_CW_REFLECTIVE_OFF" "_EMISSION_OFF" "V_CW_RIM_OFF" "_NORMALMAP_OFF" "V_CW_SPECULAR_OFF" }
					"!!GLES
					#version 100
					
					#ifdef VERTEX
					attribute vec4 _glesTANGENT;
					attribute vec4 _glesVertex;
					attribute vec4 _glesColor;
					attribute vec3 _glesNormal;
					attribute vec4 _glesMultiTexCoord0;
					uniform highp mat4 glstate_matrix_mvp;
					uniform highp mat4 _Object2World;
					uniform highp mat4 _World2Object;
					uniform highp vec4 unity_FogParams;
					uniform highp vec3 _V_CW_Bend;
					uniform highp vec3 _V_CW_Bias;
					uniform highp vec4 _V_CW_PivotPoint_Position;
					uniform highp vec4 _MainTex_ST;
					uniform highp vec4 _V_CW_SecondaryTex_ST;
					varying highp vec4 xlv_TEXCOORD0;
					varying mediump vec3 xlv_TEXCOORD1;
					varying highp vec3 xlv_TEXCOORD2;
					varying lowp vec4 xlv_COLOR0;
					varying highp float xlv_TEXCOORD3;
					void main ()
					{
					  lowp vec3 worldNormal_1;
					  highp vec4 tmpvar_2;
					  highp vec4 tmpvar_3;
					  mediump vec3 tmpvar_4;
					  highp vec4 v_5;
					  highp float tmpvar_6;
					  tmpvar_6 = _World2Object[0].x;
					  v_5.x = tmpvar_6;
					  highp float tmpvar_7;
					  tmpvar_7 = _World2Object[1].x;
					  v_5.y = tmpvar_7;
					  highp float tmpvar_8;
					  tmpvar_8 = _World2Object[2].x;
					  v_5.z = tmpvar_8;
					  highp float tmpvar_9;
					  tmpvar_9 = _World2Object[3].x;
					  v_5.w = tmpvar_9;
					  highp vec4 v_10;
					  highp float tmpvar_11;
					  tmpvar_11 = _World2Object[0].y;
					  v_10.x = tmpvar_11;
					  highp float tmpvar_12;
					  tmpvar_12 = _World2Object[1].y;
					  v_10.y = tmpvar_12;
					  highp float tmpvar_13;
					  tmpvar_13 = _World2Object[2].y;
					  v_10.z = tmpvar_13;
					  highp float tmpvar_14;
					  tmpvar_14 = _World2Object[3].y;
					  v_10.w = tmpvar_14;
					  highp vec4 v_15;
					  highp float tmpvar_16;
					  tmpvar_16 = _World2Object[0].z;
					  v_15.x = tmpvar_16;
					  highp float tmpvar_17;
					  tmpvar_17 = _World2Object[1].z;
					  v_15.y = tmpvar_17;
					  highp float tmpvar_18;
					  tmpvar_18 = _World2Object[2].z;
					  v_15.z = tmpvar_18;
					  highp float tmpvar_19;
					  tmpvar_19 = _World2Object[3].z;
					  v_15.w = tmpvar_19;
					  highp vec3 tmpvar_20;
					  tmpvar_20 = normalize(((
					    (v_5.xyz * _glesNormal.x)
					   + 
					    (v_10.xyz * _glesNormal.y)
					  ) + (v_15.xyz * _glesNormal.z)));
					  highp mat3 tmpvar_21;
					  tmpvar_21[0] = _Object2World[0].xyz;
					  tmpvar_21[1] = _Object2World[1].xyz;
					  tmpvar_21[2] = _Object2World[2].xyz;
					  highp vec3 tmpvar_22;
					  tmpvar_22 = normalize((tmpvar_21 * _glesTANGENT.xyz));
					  highp vec4 vertex_23;
					  vertex_23.w = _glesVertex.w;
					  highp vec2 xzOff_24;
					  highp vec3 v2_25;
					  highp vec3 v1_26;
					  highp vec3 v0_27;
					  highp vec3 tmpvar_28;
					  tmpvar_28 = ((_Object2World * _glesVertex).xyz - _V_CW_PivotPoint_Position.xyz);
					  highp vec3 tmpvar_29;
					  tmpvar_29 = (tmpvar_28 + tmpvar_22);
					  v1_26.xz = tmpvar_29.xz;
					  highp vec3 tmpvar_30;
					  tmpvar_30 = (tmpvar_28 - ((tmpvar_20.yzx * tmpvar_22.zxy) - (tmpvar_20.zxy * tmpvar_22.yzx)));
					  v2_25.xz = tmpvar_30.xz;
					  highp vec2 tmpvar_31;
					  tmpvar_31.x = float((tmpvar_28.z >= 0.0));
					  tmpvar_31.y = float((tmpvar_28.x >= 0.0));
					  xzOff_24 = (max (vec2(0.0, 0.0), (
					    abs(tmpvar_28.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_31 * 2.0) - 1.0));
					  xzOff_24 = (xzOff_24 * xzOff_24);
					  highp vec3 tmpvar_32;
					  tmpvar_32.xz = vec2(0.0, 0.0);
					  tmpvar_32.y = (((_V_CW_Bend.x * xzOff_24.x) + (_V_CW_Bend.z * xzOff_24.y)) * 0.001);
					  v0_27 = (tmpvar_28 + tmpvar_32);
					  highp vec2 tmpvar_33;
					  tmpvar_33.x = float((tmpvar_29.z >= 0.0));
					  tmpvar_33.y = float((tmpvar_29.x >= 0.0));
					  xzOff_24 = (max (vec2(0.0, 0.0), (
					    abs(tmpvar_29.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_33 * 2.0) - 1.0));
					  xzOff_24 = (xzOff_24 * xzOff_24);
					  v1_26.y = (tmpvar_29.y + ((
					    (_V_CW_Bend.x * xzOff_24.x)
					   + 
					    (_V_CW_Bend.z * xzOff_24.y)
					  ) * 0.001));
					  highp vec2 tmpvar_34;
					  tmpvar_34.x = float((tmpvar_30.z >= 0.0));
					  tmpvar_34.y = float((tmpvar_30.x >= 0.0));
					  xzOff_24 = (max (vec2(0.0, 0.0), (
					    abs(tmpvar_30.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_34 * 2.0) - 1.0));
					  xzOff_24 = (xzOff_24 * xzOff_24);
					  v2_25.y = (tmpvar_30.y + ((
					    (_V_CW_Bend.x * xzOff_24.x)
					   + 
					    (_V_CW_Bend.z * xzOff_24.y)
					  ) * 0.001));
					  highp mat3 tmpvar_35;
					  tmpvar_35[0] = _World2Object[0].xyz;
					  tmpvar_35[1] = _World2Object[1].xyz;
					  tmpvar_35[2] = _World2Object[2].xyz;
					  vertex_23.xyz = (_glesVertex.xyz + (tmpvar_35 * tmpvar_32));
					  highp mat3 tmpvar_36;
					  tmpvar_36[0] = _World2Object[0].xyz;
					  tmpvar_36[1] = _World2Object[1].xyz;
					  tmpvar_36[2] = _World2Object[2].xyz;
					  highp vec3 a_37;
					  a_37 = (v2_25 - v0_27);
					  highp vec3 b_38;
					  b_38 = (v1_26 - v0_27);
					  highp vec3 tmpvar_39;
					  tmpvar_39 = normalize((tmpvar_36 * normalize(
					    ((a_37.yzx * b_38.zxy) - (a_37.zxy * b_38.yzx))
					  )));
					  tmpvar_2 = (glstate_matrix_mvp * vertex_23);
					  tmpvar_3.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
					  tmpvar_3.zw = ((_glesMultiTexCoord0.xy * _V_CW_SecondaryTex_ST.xy) + _V_CW_SecondaryTex_ST.zw);
					  highp vec4 v_40;
					  v_40.x = tmpvar_6;
					  v_40.y = tmpvar_7;
					  v_40.z = tmpvar_8;
					  v_40.w = tmpvar_9;
					  highp vec4 v_41;
					  v_41.x = tmpvar_11;
					  v_41.y = tmpvar_12;
					  v_41.z = tmpvar_13;
					  v_41.w = tmpvar_14;
					  highp vec4 v_42;
					  v_42.x = tmpvar_16;
					  v_42.y = tmpvar_17;
					  v_42.z = tmpvar_18;
					  v_42.w = tmpvar_19;
					  highp vec3 tmpvar_43;
					  tmpvar_43 = normalize(((
					    (v_40.xyz * tmpvar_39.x)
					   + 
					    (v_41.xyz * tmpvar_39.y)
					  ) + (v_42.xyz * tmpvar_39.z)));
					  worldNormal_1 = tmpvar_43;
					  tmpvar_4 = worldNormal_1;
					  gl_Position = tmpvar_2;
					  xlv_TEXCOORD0 = tmpvar_3;
					  xlv_TEXCOORD1 = tmpvar_4;
					  xlv_TEXCOORD2 = (_Object2World * vertex_23).xyz;
					  xlv_COLOR0 = _glesColor;
					  xlv_TEXCOORD3 = ((tmpvar_2.z * unity_FogParams.z) + unity_FogParams.w);
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform highp vec4 _Time;
					uniform highp vec4 _WorldSpaceLightPos0;
					uniform lowp vec4 _LightColor0;
					uniform sampler2D _LightTexture0;
					uniform mediump mat4 _LightMatrix0;
					uniform sampler2D _LightTextureB0;
					uniform sampler2D _MainTex;
					uniform lowp vec4 _Color;
					uniform lowp vec2 _V_CW_MainTex_Scroll;
					uniform sampler2D _V_CW_SecondaryTex;
					uniform lowp vec2 _V_CW_SecondaryTex_Scroll;
					uniform lowp float _V_CW_SecondaryTex_Blend;
					varying highp vec4 xlv_TEXCOORD0;
					varying mediump vec3 xlv_TEXCOORD1;
					varying highp vec3 xlv_TEXCOORD2;
					varying lowp vec4 xlv_COLOR0;
					varying highp float xlv_TEXCOORD3;
					void main ()
					{
					  mediump vec3 tmpvar_1;
					  mediump vec3 tmpvar_2;
					  lowp vec4 c_3;
					  lowp float atten_4;
					  mediump vec4 lightCoord_5;
					  lowp vec3 tmpvar_6;
					  lowp vec3 lightDir_7;
					  highp vec4 tmpvar_8;
					  highp vec3 tmpvar_9;
					  tmpvar_9 = normalize((_WorldSpaceLightPos0.xyz - xlv_TEXCOORD2));
					  lightDir_7 = tmpvar_9;
					  tmpvar_8 = xlv_COLOR0;
					  tmpvar_6 = xlv_TEXCOORD1;
					  lowp vec3 tmpvar_10;
					  lowp float vBlend_11;
					  highp vec2 P_12;
					  P_12 = (xlv_TEXCOORD0.xy + (_V_CW_MainTex_Scroll * _Time.x));
					  highp float tmpvar_13;
					  tmpvar_13 = clamp ((_V_CW_SecondaryTex_Blend + tmpvar_8.w), 0.0, 1.0);
					  vBlend_11 = tmpvar_13;
					  highp vec2 P_14;
					  P_14 = (xlv_TEXCOORD0.zw + (_V_CW_SecondaryTex_Scroll * _Time.x));
					  lowp vec4 tmpvar_15;
					  tmpvar_15 = mix (texture2D (_MainTex, P_12), texture2D (_V_CW_SecondaryTex, P_14), vec4(vBlend_11));
					  tmpvar_10 = (tmpvar_15.xyz * _Color.xyz);
					  tmpvar_10 = (tmpvar_10 * tmpvar_8.xyz);
					  highp vec4 tmpvar_16;
					  tmpvar_16.w = 1.0;
					  tmpvar_16.xyz = xlv_TEXCOORD2;
					  highp vec4 tmpvar_17;
					  tmpvar_17 = (_LightMatrix0 * tmpvar_16);
					  lightCoord_5 = tmpvar_17;
					  lowp vec4 tmpvar_18;
					  mediump vec2 P_19;
					  P_19 = ((lightCoord_5.xy / lightCoord_5.w) + 0.5);
					  tmpvar_18 = texture2D (_LightTexture0, P_19);
					  highp vec3 LightCoord_20;
					  LightCoord_20 = lightCoord_5.xyz;
					  highp float tmpvar_21;
					  tmpvar_21 = dot (LightCoord_20, LightCoord_20);
					  lowp vec4 tmpvar_22;
					  tmpvar_22 = texture2D (_LightTextureB0, vec2(tmpvar_21));
					  mediump float tmpvar_23;
					  tmpvar_23 = ((float(
					    (lightCoord_5.z > 0.0)
					  ) * tmpvar_18.w) * tmpvar_22.w);
					  atten_4 = tmpvar_23;
					  tmpvar_1 = _LightColor0.xyz;
					  tmpvar_2 = lightDir_7;
					  tmpvar_1 = (tmpvar_1 * atten_4);
					  lowp vec4 c_24;
					  lowp vec4 c_25;
					  lowp float diff_26;
					  mediump float tmpvar_27;
					  tmpvar_27 = max (0.0, dot (tmpvar_6, tmpvar_2));
					  diff_26 = tmpvar_27;
					  c_25.xyz = ((tmpvar_10 * tmpvar_1) * diff_26);
					  c_25.w = (tmpvar_15.w * _Color.w);
					  c_24.w = c_25.w;
					  c_24.xyz = c_25.xyz;
					  c_3.w = c_24.w;
					  highp float tmpvar_28;
					  tmpvar_28 = clamp (xlv_TEXCOORD3, 0.0, 1.0);
					  c_3.xyz = (c_25.xyz * vec3(tmpvar_28));
					  gl_FragData[0] = c_3;
					}
					
					
					#endif"
}
SubProgram "gles " {
Keywords { "POINT_COOKIE" "FOG_LINEAR" "V_CW_VERTEX_COLOR" "V_CW_REFLECTIVE_OFF" "_EMISSION_OFF" "V_CW_RIM_OFF" "_NORMALMAP_OFF" "V_CW_SPECULAR_OFF" }
					"!!GLES
					#version 100
					
					#ifdef VERTEX
					attribute vec4 _glesTANGENT;
					attribute vec4 _glesVertex;
					attribute vec4 _glesColor;
					attribute vec3 _glesNormal;
					attribute vec4 _glesMultiTexCoord0;
					uniform highp mat4 glstate_matrix_mvp;
					uniform highp mat4 _Object2World;
					uniform highp mat4 _World2Object;
					uniform highp vec4 unity_FogParams;
					uniform highp vec3 _V_CW_Bend;
					uniform highp vec3 _V_CW_Bias;
					uniform highp vec4 _V_CW_PivotPoint_Position;
					uniform highp vec4 _MainTex_ST;
					uniform highp vec4 _V_CW_SecondaryTex_ST;
					varying highp vec4 xlv_TEXCOORD0;
					varying mediump vec3 xlv_TEXCOORD1;
					varying highp vec3 xlv_TEXCOORD2;
					varying lowp vec4 xlv_COLOR0;
					varying highp float xlv_TEXCOORD3;
					void main ()
					{
					  lowp vec3 worldNormal_1;
					  highp vec4 tmpvar_2;
					  highp vec4 tmpvar_3;
					  mediump vec3 tmpvar_4;
					  highp vec4 v_5;
					  highp float tmpvar_6;
					  tmpvar_6 = _World2Object[0].x;
					  v_5.x = tmpvar_6;
					  highp float tmpvar_7;
					  tmpvar_7 = _World2Object[1].x;
					  v_5.y = tmpvar_7;
					  highp float tmpvar_8;
					  tmpvar_8 = _World2Object[2].x;
					  v_5.z = tmpvar_8;
					  highp float tmpvar_9;
					  tmpvar_9 = _World2Object[3].x;
					  v_5.w = tmpvar_9;
					  highp vec4 v_10;
					  highp float tmpvar_11;
					  tmpvar_11 = _World2Object[0].y;
					  v_10.x = tmpvar_11;
					  highp float tmpvar_12;
					  tmpvar_12 = _World2Object[1].y;
					  v_10.y = tmpvar_12;
					  highp float tmpvar_13;
					  tmpvar_13 = _World2Object[2].y;
					  v_10.z = tmpvar_13;
					  highp float tmpvar_14;
					  tmpvar_14 = _World2Object[3].y;
					  v_10.w = tmpvar_14;
					  highp vec4 v_15;
					  highp float tmpvar_16;
					  tmpvar_16 = _World2Object[0].z;
					  v_15.x = tmpvar_16;
					  highp float tmpvar_17;
					  tmpvar_17 = _World2Object[1].z;
					  v_15.y = tmpvar_17;
					  highp float tmpvar_18;
					  tmpvar_18 = _World2Object[2].z;
					  v_15.z = tmpvar_18;
					  highp float tmpvar_19;
					  tmpvar_19 = _World2Object[3].z;
					  v_15.w = tmpvar_19;
					  highp vec3 tmpvar_20;
					  tmpvar_20 = normalize(((
					    (v_5.xyz * _glesNormal.x)
					   + 
					    (v_10.xyz * _glesNormal.y)
					  ) + (v_15.xyz * _glesNormal.z)));
					  highp mat3 tmpvar_21;
					  tmpvar_21[0] = _Object2World[0].xyz;
					  tmpvar_21[1] = _Object2World[1].xyz;
					  tmpvar_21[2] = _Object2World[2].xyz;
					  highp vec3 tmpvar_22;
					  tmpvar_22 = normalize((tmpvar_21 * _glesTANGENT.xyz));
					  highp vec4 vertex_23;
					  vertex_23.w = _glesVertex.w;
					  highp vec2 xzOff_24;
					  highp vec3 v2_25;
					  highp vec3 v1_26;
					  highp vec3 v0_27;
					  highp vec3 tmpvar_28;
					  tmpvar_28 = ((_Object2World * _glesVertex).xyz - _V_CW_PivotPoint_Position.xyz);
					  highp vec3 tmpvar_29;
					  tmpvar_29 = (tmpvar_28 + tmpvar_22);
					  v1_26.xz = tmpvar_29.xz;
					  highp vec3 tmpvar_30;
					  tmpvar_30 = (tmpvar_28 - ((tmpvar_20.yzx * tmpvar_22.zxy) - (tmpvar_20.zxy * tmpvar_22.yzx)));
					  v2_25.xz = tmpvar_30.xz;
					  highp vec2 tmpvar_31;
					  tmpvar_31.x = float((tmpvar_28.z >= 0.0));
					  tmpvar_31.y = float((tmpvar_28.x >= 0.0));
					  xzOff_24 = (max (vec2(0.0, 0.0), (
					    abs(tmpvar_28.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_31 * 2.0) - 1.0));
					  xzOff_24 = (xzOff_24 * xzOff_24);
					  highp vec3 tmpvar_32;
					  tmpvar_32.xz = vec2(0.0, 0.0);
					  tmpvar_32.y = (((_V_CW_Bend.x * xzOff_24.x) + (_V_CW_Bend.z * xzOff_24.y)) * 0.001);
					  v0_27 = (tmpvar_28 + tmpvar_32);
					  highp vec2 tmpvar_33;
					  tmpvar_33.x = float((tmpvar_29.z >= 0.0));
					  tmpvar_33.y = float((tmpvar_29.x >= 0.0));
					  xzOff_24 = (max (vec2(0.0, 0.0), (
					    abs(tmpvar_29.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_33 * 2.0) - 1.0));
					  xzOff_24 = (xzOff_24 * xzOff_24);
					  v1_26.y = (tmpvar_29.y + ((
					    (_V_CW_Bend.x * xzOff_24.x)
					   + 
					    (_V_CW_Bend.z * xzOff_24.y)
					  ) * 0.001));
					  highp vec2 tmpvar_34;
					  tmpvar_34.x = float((tmpvar_30.z >= 0.0));
					  tmpvar_34.y = float((tmpvar_30.x >= 0.0));
					  xzOff_24 = (max (vec2(0.0, 0.0), (
					    abs(tmpvar_30.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_34 * 2.0) - 1.0));
					  xzOff_24 = (xzOff_24 * xzOff_24);
					  v2_25.y = (tmpvar_30.y + ((
					    (_V_CW_Bend.x * xzOff_24.x)
					   + 
					    (_V_CW_Bend.z * xzOff_24.y)
					  ) * 0.001));
					  highp mat3 tmpvar_35;
					  tmpvar_35[0] = _World2Object[0].xyz;
					  tmpvar_35[1] = _World2Object[1].xyz;
					  tmpvar_35[2] = _World2Object[2].xyz;
					  vertex_23.xyz = (_glesVertex.xyz + (tmpvar_35 * tmpvar_32));
					  highp mat3 tmpvar_36;
					  tmpvar_36[0] = _World2Object[0].xyz;
					  tmpvar_36[1] = _World2Object[1].xyz;
					  tmpvar_36[2] = _World2Object[2].xyz;
					  highp vec3 a_37;
					  a_37 = (v2_25 - v0_27);
					  highp vec3 b_38;
					  b_38 = (v1_26 - v0_27);
					  highp vec3 tmpvar_39;
					  tmpvar_39 = normalize((tmpvar_36 * normalize(
					    ((a_37.yzx * b_38.zxy) - (a_37.zxy * b_38.yzx))
					  )));
					  tmpvar_2 = (glstate_matrix_mvp * vertex_23);
					  tmpvar_3.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
					  tmpvar_3.zw = ((_glesMultiTexCoord0.xy * _V_CW_SecondaryTex_ST.xy) + _V_CW_SecondaryTex_ST.zw);
					  highp vec4 v_40;
					  v_40.x = tmpvar_6;
					  v_40.y = tmpvar_7;
					  v_40.z = tmpvar_8;
					  v_40.w = tmpvar_9;
					  highp vec4 v_41;
					  v_41.x = tmpvar_11;
					  v_41.y = tmpvar_12;
					  v_41.z = tmpvar_13;
					  v_41.w = tmpvar_14;
					  highp vec4 v_42;
					  v_42.x = tmpvar_16;
					  v_42.y = tmpvar_17;
					  v_42.z = tmpvar_18;
					  v_42.w = tmpvar_19;
					  highp vec3 tmpvar_43;
					  tmpvar_43 = normalize(((
					    (v_40.xyz * tmpvar_39.x)
					   + 
					    (v_41.xyz * tmpvar_39.y)
					  ) + (v_42.xyz * tmpvar_39.z)));
					  worldNormal_1 = tmpvar_43;
					  tmpvar_4 = worldNormal_1;
					  gl_Position = tmpvar_2;
					  xlv_TEXCOORD0 = tmpvar_3;
					  xlv_TEXCOORD1 = tmpvar_4;
					  xlv_TEXCOORD2 = (_Object2World * vertex_23).xyz;
					  xlv_COLOR0 = _glesColor;
					  xlv_TEXCOORD3 = ((tmpvar_2.z * unity_FogParams.z) + unity_FogParams.w);
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform highp vec4 _Time;
					uniform highp vec4 _WorldSpaceLightPos0;
					uniform lowp vec4 _LightColor0;
					uniform lowp samplerCube _LightTexture0;
					uniform mediump mat4 _LightMatrix0;
					uniform sampler2D _LightTextureB0;
					uniform sampler2D _MainTex;
					uniform lowp vec4 _Color;
					uniform lowp vec2 _V_CW_MainTex_Scroll;
					uniform sampler2D _V_CW_SecondaryTex;
					uniform lowp vec2 _V_CW_SecondaryTex_Scroll;
					uniform lowp float _V_CW_SecondaryTex_Blend;
					varying highp vec4 xlv_TEXCOORD0;
					varying mediump vec3 xlv_TEXCOORD1;
					varying highp vec3 xlv_TEXCOORD2;
					varying lowp vec4 xlv_COLOR0;
					varying highp float xlv_TEXCOORD3;
					void main ()
					{
					  mediump vec3 tmpvar_1;
					  mediump vec3 tmpvar_2;
					  lowp vec4 c_3;
					  lowp vec3 tmpvar_4;
					  lowp vec3 lightDir_5;
					  highp vec4 tmpvar_6;
					  highp vec3 tmpvar_7;
					  tmpvar_7 = normalize((_WorldSpaceLightPos0.xyz - xlv_TEXCOORD2));
					  lightDir_5 = tmpvar_7;
					  tmpvar_6 = xlv_COLOR0;
					  tmpvar_4 = xlv_TEXCOORD1;
					  lowp vec3 tmpvar_8;
					  lowp float vBlend_9;
					  highp vec2 P_10;
					  P_10 = (xlv_TEXCOORD0.xy + (_V_CW_MainTex_Scroll * _Time.x));
					  highp float tmpvar_11;
					  tmpvar_11 = clamp ((_V_CW_SecondaryTex_Blend + tmpvar_6.w), 0.0, 1.0);
					  vBlend_9 = tmpvar_11;
					  highp vec2 P_12;
					  P_12 = (xlv_TEXCOORD0.zw + (_V_CW_SecondaryTex_Scroll * _Time.x));
					  lowp vec4 tmpvar_13;
					  tmpvar_13 = mix (texture2D (_MainTex, P_10), texture2D (_V_CW_SecondaryTex, P_12), vec4(vBlend_9));
					  tmpvar_8 = (tmpvar_13.xyz * _Color.xyz);
					  tmpvar_8 = (tmpvar_8 * tmpvar_6.xyz);
					  highp vec4 tmpvar_14;
					  tmpvar_14.w = 1.0;
					  tmpvar_14.xyz = xlv_TEXCOORD2;
					  highp vec3 tmpvar_15;
					  tmpvar_15 = (_LightMatrix0 * tmpvar_14).xyz;
					  highp float tmpvar_16;
					  tmpvar_16 = dot (tmpvar_15, tmpvar_15);
					  lowp float tmpvar_17;
					  tmpvar_17 = (texture2D (_LightTextureB0, vec2(tmpvar_16)).w * textureCube (_LightTexture0, tmpvar_15).w);
					  tmpvar_1 = _LightColor0.xyz;
					  tmpvar_2 = lightDir_5;
					  tmpvar_1 = (tmpvar_1 * tmpvar_17);
					  lowp vec4 c_18;
					  lowp vec4 c_19;
					  lowp float diff_20;
					  mediump float tmpvar_21;
					  tmpvar_21 = max (0.0, dot (tmpvar_4, tmpvar_2));
					  diff_20 = tmpvar_21;
					  c_19.xyz = ((tmpvar_8 * tmpvar_1) * diff_20);
					  c_19.w = (tmpvar_13.w * _Color.w);
					  c_18.w = c_19.w;
					  c_18.xyz = c_19.xyz;
					  c_3.w = c_18.w;
					  highp float tmpvar_22;
					  tmpvar_22 = clamp (xlv_TEXCOORD3, 0.0, 1.0);
					  c_3.xyz = (c_19.xyz * vec3(tmpvar_22));
					  gl_FragData[0] = c_3;
					}
					
					
					#endif"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL_COOKIE" "FOG_LINEAR" "V_CW_VERTEX_COLOR" "V_CW_REFLECTIVE_OFF" "_EMISSION_OFF" "V_CW_RIM_OFF" "_NORMALMAP_OFF" "V_CW_SPECULAR_OFF" }
					"!!GLES
					#version 100
					
					#ifdef VERTEX
					attribute vec4 _glesTANGENT;
					attribute vec4 _glesVertex;
					attribute vec4 _glesColor;
					attribute vec3 _glesNormal;
					attribute vec4 _glesMultiTexCoord0;
					uniform highp mat4 glstate_matrix_mvp;
					uniform highp mat4 _Object2World;
					uniform highp mat4 _World2Object;
					uniform highp vec4 unity_FogParams;
					uniform highp vec3 _V_CW_Bend;
					uniform highp vec3 _V_CW_Bias;
					uniform highp vec4 _V_CW_PivotPoint_Position;
					uniform highp vec4 _MainTex_ST;
					uniform highp vec4 _V_CW_SecondaryTex_ST;
					varying highp vec4 xlv_TEXCOORD0;
					varying mediump vec3 xlv_TEXCOORD1;
					varying highp vec3 xlv_TEXCOORD2;
					varying lowp vec4 xlv_COLOR0;
					varying highp float xlv_TEXCOORD3;
					void main ()
					{
					  lowp vec3 worldNormal_1;
					  highp vec4 tmpvar_2;
					  highp vec4 tmpvar_3;
					  mediump vec3 tmpvar_4;
					  highp vec4 v_5;
					  highp float tmpvar_6;
					  tmpvar_6 = _World2Object[0].x;
					  v_5.x = tmpvar_6;
					  highp float tmpvar_7;
					  tmpvar_7 = _World2Object[1].x;
					  v_5.y = tmpvar_7;
					  highp float tmpvar_8;
					  tmpvar_8 = _World2Object[2].x;
					  v_5.z = tmpvar_8;
					  highp float tmpvar_9;
					  tmpvar_9 = _World2Object[3].x;
					  v_5.w = tmpvar_9;
					  highp vec4 v_10;
					  highp float tmpvar_11;
					  tmpvar_11 = _World2Object[0].y;
					  v_10.x = tmpvar_11;
					  highp float tmpvar_12;
					  tmpvar_12 = _World2Object[1].y;
					  v_10.y = tmpvar_12;
					  highp float tmpvar_13;
					  tmpvar_13 = _World2Object[2].y;
					  v_10.z = tmpvar_13;
					  highp float tmpvar_14;
					  tmpvar_14 = _World2Object[3].y;
					  v_10.w = tmpvar_14;
					  highp vec4 v_15;
					  highp float tmpvar_16;
					  tmpvar_16 = _World2Object[0].z;
					  v_15.x = tmpvar_16;
					  highp float tmpvar_17;
					  tmpvar_17 = _World2Object[1].z;
					  v_15.y = tmpvar_17;
					  highp float tmpvar_18;
					  tmpvar_18 = _World2Object[2].z;
					  v_15.z = tmpvar_18;
					  highp float tmpvar_19;
					  tmpvar_19 = _World2Object[3].z;
					  v_15.w = tmpvar_19;
					  highp vec3 tmpvar_20;
					  tmpvar_20 = normalize(((
					    (v_5.xyz * _glesNormal.x)
					   + 
					    (v_10.xyz * _glesNormal.y)
					  ) + (v_15.xyz * _glesNormal.z)));
					  highp mat3 tmpvar_21;
					  tmpvar_21[0] = _Object2World[0].xyz;
					  tmpvar_21[1] = _Object2World[1].xyz;
					  tmpvar_21[2] = _Object2World[2].xyz;
					  highp vec3 tmpvar_22;
					  tmpvar_22 = normalize((tmpvar_21 * _glesTANGENT.xyz));
					  highp vec4 vertex_23;
					  vertex_23.w = _glesVertex.w;
					  highp vec2 xzOff_24;
					  highp vec3 v2_25;
					  highp vec3 v1_26;
					  highp vec3 v0_27;
					  highp vec3 tmpvar_28;
					  tmpvar_28 = ((_Object2World * _glesVertex).xyz - _V_CW_PivotPoint_Position.xyz);
					  highp vec3 tmpvar_29;
					  tmpvar_29 = (tmpvar_28 + tmpvar_22);
					  v1_26.xz = tmpvar_29.xz;
					  highp vec3 tmpvar_30;
					  tmpvar_30 = (tmpvar_28 - ((tmpvar_20.yzx * tmpvar_22.zxy) - (tmpvar_20.zxy * tmpvar_22.yzx)));
					  v2_25.xz = tmpvar_30.xz;
					  highp vec2 tmpvar_31;
					  tmpvar_31.x = float((tmpvar_28.z >= 0.0));
					  tmpvar_31.y = float((tmpvar_28.x >= 0.0));
					  xzOff_24 = (max (vec2(0.0, 0.0), (
					    abs(tmpvar_28.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_31 * 2.0) - 1.0));
					  xzOff_24 = (xzOff_24 * xzOff_24);
					  highp vec3 tmpvar_32;
					  tmpvar_32.xz = vec2(0.0, 0.0);
					  tmpvar_32.y = (((_V_CW_Bend.x * xzOff_24.x) + (_V_CW_Bend.z * xzOff_24.y)) * 0.001);
					  v0_27 = (tmpvar_28 + tmpvar_32);
					  highp vec2 tmpvar_33;
					  tmpvar_33.x = float((tmpvar_29.z >= 0.0));
					  tmpvar_33.y = float((tmpvar_29.x >= 0.0));
					  xzOff_24 = (max (vec2(0.0, 0.0), (
					    abs(tmpvar_29.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_33 * 2.0) - 1.0));
					  xzOff_24 = (xzOff_24 * xzOff_24);
					  v1_26.y = (tmpvar_29.y + ((
					    (_V_CW_Bend.x * xzOff_24.x)
					   + 
					    (_V_CW_Bend.z * xzOff_24.y)
					  ) * 0.001));
					  highp vec2 tmpvar_34;
					  tmpvar_34.x = float((tmpvar_30.z >= 0.0));
					  tmpvar_34.y = float((tmpvar_30.x >= 0.0));
					  xzOff_24 = (max (vec2(0.0, 0.0), (
					    abs(tmpvar_30.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_34 * 2.0) - 1.0));
					  xzOff_24 = (xzOff_24 * xzOff_24);
					  v2_25.y = (tmpvar_30.y + ((
					    (_V_CW_Bend.x * xzOff_24.x)
					   + 
					    (_V_CW_Bend.z * xzOff_24.y)
					  ) * 0.001));
					  highp mat3 tmpvar_35;
					  tmpvar_35[0] = _World2Object[0].xyz;
					  tmpvar_35[1] = _World2Object[1].xyz;
					  tmpvar_35[2] = _World2Object[2].xyz;
					  vertex_23.xyz = (_glesVertex.xyz + (tmpvar_35 * tmpvar_32));
					  highp mat3 tmpvar_36;
					  tmpvar_36[0] = _World2Object[0].xyz;
					  tmpvar_36[1] = _World2Object[1].xyz;
					  tmpvar_36[2] = _World2Object[2].xyz;
					  highp vec3 a_37;
					  a_37 = (v2_25 - v0_27);
					  highp vec3 b_38;
					  b_38 = (v1_26 - v0_27);
					  highp vec3 tmpvar_39;
					  tmpvar_39 = normalize((tmpvar_36 * normalize(
					    ((a_37.yzx * b_38.zxy) - (a_37.zxy * b_38.yzx))
					  )));
					  tmpvar_2 = (glstate_matrix_mvp * vertex_23);
					  tmpvar_3.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
					  tmpvar_3.zw = ((_glesMultiTexCoord0.xy * _V_CW_SecondaryTex_ST.xy) + _V_CW_SecondaryTex_ST.zw);
					  highp vec4 v_40;
					  v_40.x = tmpvar_6;
					  v_40.y = tmpvar_7;
					  v_40.z = tmpvar_8;
					  v_40.w = tmpvar_9;
					  highp vec4 v_41;
					  v_41.x = tmpvar_11;
					  v_41.y = tmpvar_12;
					  v_41.z = tmpvar_13;
					  v_41.w = tmpvar_14;
					  highp vec4 v_42;
					  v_42.x = tmpvar_16;
					  v_42.y = tmpvar_17;
					  v_42.z = tmpvar_18;
					  v_42.w = tmpvar_19;
					  highp vec3 tmpvar_43;
					  tmpvar_43 = normalize(((
					    (v_40.xyz * tmpvar_39.x)
					   + 
					    (v_41.xyz * tmpvar_39.y)
					  ) + (v_42.xyz * tmpvar_39.z)));
					  worldNormal_1 = tmpvar_43;
					  tmpvar_4 = worldNormal_1;
					  gl_Position = tmpvar_2;
					  xlv_TEXCOORD0 = tmpvar_3;
					  xlv_TEXCOORD1 = tmpvar_4;
					  xlv_TEXCOORD2 = (_Object2World * vertex_23).xyz;
					  xlv_COLOR0 = _glesColor;
					  xlv_TEXCOORD3 = ((tmpvar_2.z * unity_FogParams.z) + unity_FogParams.w);
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform highp vec4 _Time;
					uniform mediump vec4 _WorldSpaceLightPos0;
					uniform lowp vec4 _LightColor0;
					uniform sampler2D _LightTexture0;
					uniform mediump mat4 _LightMatrix0;
					uniform sampler2D _MainTex;
					uniform lowp vec4 _Color;
					uniform lowp vec2 _V_CW_MainTex_Scroll;
					uniform sampler2D _V_CW_SecondaryTex;
					uniform lowp vec2 _V_CW_SecondaryTex_Scroll;
					uniform lowp float _V_CW_SecondaryTex_Blend;
					varying highp vec4 xlv_TEXCOORD0;
					varying mediump vec3 xlv_TEXCOORD1;
					varying highp vec3 xlv_TEXCOORD2;
					varying lowp vec4 xlv_COLOR0;
					varying highp float xlv_TEXCOORD3;
					void main ()
					{
					  mediump vec3 tmpvar_1;
					  mediump vec3 tmpvar_2;
					  lowp vec4 c_3;
					  lowp vec3 tmpvar_4;
					  lowp vec3 lightDir_5;
					  highp vec4 tmpvar_6;
					  mediump vec3 tmpvar_7;
					  tmpvar_7 = _WorldSpaceLightPos0.xyz;
					  lightDir_5 = tmpvar_7;
					  tmpvar_6 = xlv_COLOR0;
					  tmpvar_4 = xlv_TEXCOORD1;
					  lowp vec3 tmpvar_8;
					  lowp float vBlend_9;
					  highp vec2 P_10;
					  P_10 = (xlv_TEXCOORD0.xy + (_V_CW_MainTex_Scroll * _Time.x));
					  highp float tmpvar_11;
					  tmpvar_11 = clamp ((_V_CW_SecondaryTex_Blend + tmpvar_6.w), 0.0, 1.0);
					  vBlend_9 = tmpvar_11;
					  highp vec2 P_12;
					  P_12 = (xlv_TEXCOORD0.zw + (_V_CW_SecondaryTex_Scroll * _Time.x));
					  lowp vec4 tmpvar_13;
					  tmpvar_13 = mix (texture2D (_MainTex, P_10), texture2D (_V_CW_SecondaryTex, P_12), vec4(vBlend_9));
					  tmpvar_8 = (tmpvar_13.xyz * _Color.xyz);
					  tmpvar_8 = (tmpvar_8 * tmpvar_6.xyz);
					  highp vec4 tmpvar_14;
					  tmpvar_14.w = 1.0;
					  tmpvar_14.xyz = xlv_TEXCOORD2;
					  highp vec2 tmpvar_15;
					  tmpvar_15 = (_LightMatrix0 * tmpvar_14).xy;
					  lowp float tmpvar_16;
					  tmpvar_16 = texture2D (_LightTexture0, tmpvar_15).w;
					  tmpvar_1 = _LightColor0.xyz;
					  tmpvar_2 = lightDir_5;
					  tmpvar_1 = (tmpvar_1 * tmpvar_16);
					  lowp vec4 c_17;
					  lowp vec4 c_18;
					  lowp float diff_19;
					  mediump float tmpvar_20;
					  tmpvar_20 = max (0.0, dot (tmpvar_4, tmpvar_2));
					  diff_19 = tmpvar_20;
					  c_18.xyz = ((tmpvar_8 * tmpvar_1) * diff_19);
					  c_18.w = (tmpvar_13.w * _Color.w);
					  c_17.w = c_18.w;
					  c_17.xyz = c_18.xyz;
					  c_3.w = c_17.w;
					  highp float tmpvar_21;
					  tmpvar_21 = clamp (xlv_TEXCOORD3, 0.0, 1.0);
					  c_3.xyz = (c_18.xyz * vec3(tmpvar_21));
					  gl_FragData[0] = c_3;
					}
					
					
					#endif"
}
}
Program "fp" {
SubProgram "gles " {
Keywords { "POINT" "V_CW_REFLECTIVE_OFF" "V_CW_VERTEX_COLOR_OFF" "_EMISSION_OFF" "V_CW_RIM_OFF" "_NORMALMAP_OFF" "V_CW_SPECULAR_OFF" }
					"!!GLES"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "V_CW_REFLECTIVE_OFF" "V_CW_VERTEX_COLOR_OFF" "_EMISSION_OFF" "V_CW_RIM_OFF" "_NORMALMAP_OFF" "V_CW_SPECULAR_OFF" }
					"!!GLES"
}
SubProgram "gles " {
Keywords { "SPOT" "V_CW_REFLECTIVE_OFF" "V_CW_VERTEX_COLOR_OFF" "_EMISSION_OFF" "V_CW_RIM_OFF" "_NORMALMAP_OFF" "V_CW_SPECULAR_OFF" }
					"!!GLES"
}
SubProgram "gles " {
Keywords { "POINT_COOKIE" "V_CW_REFLECTIVE_OFF" "V_CW_VERTEX_COLOR_OFF" "_EMISSION_OFF" "V_CW_RIM_OFF" "_NORMALMAP_OFF" "V_CW_SPECULAR_OFF" }
					"!!GLES"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL_COOKIE" "V_CW_REFLECTIVE_OFF" "V_CW_VERTEX_COLOR_OFF" "_EMISSION_OFF" "V_CW_RIM_OFF" "_NORMALMAP_OFF" "V_CW_SPECULAR_OFF" }
					"!!GLES"
}
SubProgram "gles " {
Keywords { "POINT" "FOG_LINEAR" "V_CW_REFLECTIVE_OFF" "V_CW_VERTEX_COLOR_OFF" "_EMISSION_OFF" "V_CW_RIM_OFF" "_NORMALMAP_OFF" "V_CW_SPECULAR_OFF" }
					"!!GLES"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "FOG_LINEAR" "V_CW_REFLECTIVE_OFF" "V_CW_VERTEX_COLOR_OFF" "_EMISSION_OFF" "V_CW_RIM_OFF" "_NORMALMAP_OFF" "V_CW_SPECULAR_OFF" }
					"!!GLES"
}
SubProgram "gles " {
Keywords { "SPOT" "FOG_LINEAR" "V_CW_REFLECTIVE_OFF" "V_CW_VERTEX_COLOR_OFF" "_EMISSION_OFF" "V_CW_RIM_OFF" "_NORMALMAP_OFF" "V_CW_SPECULAR_OFF" }
					"!!GLES"
}
SubProgram "gles " {
Keywords { "POINT_COOKIE" "FOG_LINEAR" "V_CW_REFLECTIVE_OFF" "V_CW_VERTEX_COLOR_OFF" "_EMISSION_OFF" "V_CW_RIM_OFF" "_NORMALMAP_OFF" "V_CW_SPECULAR_OFF" }
					"!!GLES"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL_COOKIE" "FOG_LINEAR" "V_CW_REFLECTIVE_OFF" "V_CW_VERTEX_COLOR_OFF" "_EMISSION_OFF" "V_CW_RIM_OFF" "_NORMALMAP_OFF" "V_CW_SPECULAR_OFF" }
					"!!GLES"
}
SubProgram "gles " {
Keywords { "POINT" "V_CW_VERTEX_COLOR" "V_CW_REFLECTIVE_OFF" "_EMISSION_OFF" "V_CW_RIM_OFF" "_NORMALMAP_OFF" "V_CW_SPECULAR_OFF" }
					"!!GLES"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "V_CW_VERTEX_COLOR" "V_CW_REFLECTIVE_OFF" "_EMISSION_OFF" "V_CW_RIM_OFF" "_NORMALMAP_OFF" "V_CW_SPECULAR_OFF" }
					"!!GLES"
}
SubProgram "gles " {
Keywords { "SPOT" "V_CW_VERTEX_COLOR" "V_CW_REFLECTIVE_OFF" "_EMISSION_OFF" "V_CW_RIM_OFF" "_NORMALMAP_OFF" "V_CW_SPECULAR_OFF" }
					"!!GLES"
}
SubProgram "gles " {
Keywords { "POINT_COOKIE" "V_CW_VERTEX_COLOR" "V_CW_REFLECTIVE_OFF" "_EMISSION_OFF" "V_CW_RIM_OFF" "_NORMALMAP_OFF" "V_CW_SPECULAR_OFF" }
					"!!GLES"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL_COOKIE" "V_CW_VERTEX_COLOR" "V_CW_REFLECTIVE_OFF" "_EMISSION_OFF" "V_CW_RIM_OFF" "_NORMALMAP_OFF" "V_CW_SPECULAR_OFF" }
					"!!GLES"
}
SubProgram "gles " {
Keywords { "POINT" "FOG_LINEAR" "V_CW_VERTEX_COLOR" "V_CW_REFLECTIVE_OFF" "_EMISSION_OFF" "V_CW_RIM_OFF" "_NORMALMAP_OFF" "V_CW_SPECULAR_OFF" }
					"!!GLES"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "FOG_LINEAR" "V_CW_VERTEX_COLOR" "V_CW_REFLECTIVE_OFF" "_EMISSION_OFF" "V_CW_RIM_OFF" "_NORMALMAP_OFF" "V_CW_SPECULAR_OFF" }
					"!!GLES"
}
SubProgram "gles " {
Keywords { "SPOT" "FOG_LINEAR" "V_CW_VERTEX_COLOR" "V_CW_REFLECTIVE_OFF" "_EMISSION_OFF" "V_CW_RIM_OFF" "_NORMALMAP_OFF" "V_CW_SPECULAR_OFF" }
					"!!GLES"
}
SubProgram "gles " {
Keywords { "POINT_COOKIE" "FOG_LINEAR" "V_CW_VERTEX_COLOR" "V_CW_REFLECTIVE_OFF" "_EMISSION_OFF" "V_CW_RIM_OFF" "_NORMALMAP_OFF" "V_CW_SPECULAR_OFF" }
					"!!GLES"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL_COOKIE" "FOG_LINEAR" "V_CW_VERTEX_COLOR" "V_CW_REFLECTIVE_OFF" "_EMISSION_OFF" "V_CW_RIM_OFF" "_NORMALMAP_OFF" "V_CW_SPECULAR_OFF" }
					"!!GLES"
}
}
 }
}
Fallback "Hidden/VacuumShaders/Curved World/VertexLit/Transparent"
}