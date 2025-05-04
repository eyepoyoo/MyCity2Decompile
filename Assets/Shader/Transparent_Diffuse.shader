Shader "Hidden/VacuumShaders/Curved World/Legacy Shader/Transparent/Diffuse" {
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
[CurvedWorldLabel]  V_CW_CW_OPTIONS ("Curved World Optionals", Float) = 0
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
 Tags { "QUEUE"="Transparent" "IGNOREPROJECTOR"="true" "RenderType"="Transparent" "CurvedWorldTag"="Legacy Shader/Transparent/Diffuse" "CurvedWorldNoneRemoveableKeywords"="" "CurvedWorldAvailableOptions"="V_CW_REFLECTIVE;V_CW_VERTEX_COLOR;_EMISSION;V_CW_RIM;_NORMALMAP;V_CW_SPECULAR_HD;" }
 Pass {
  Name "FORWARD"
  Tags { "LIGHTMODE"="ForwardBase" "QUEUE"="Transparent" "IGNOREPROJECTOR"="true" "RenderType"="Transparent" "CurvedWorldTag"="Legacy Shader/Transparent/Diffuse" "CurvedWorldNoneRemoveableKeywords"="" "CurvedWorldAvailableOptions"="V_CW_REFLECTIVE;V_CW_VERTEX_COLOR;_EMISSION;V_CW_RIM;_NORMALMAP;V_CW_SPECULAR_HD;" }
  ZWrite Off
  Blend SrcAlpha OneMinusSrcAlpha
  ColorMask RGB
  GpuProgramID 8943
Program "vp" {
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
					varying highp vec2 xlv_TEXCOORD0;
					varying mediump vec3 xlv_TEXCOORD1;
					varying highp vec3 xlv_TEXCOORD2;
					varying lowp vec4 xlv_COLOR0;
					varying mediump vec3 xlv_TEXCOORD3;
					void main ()
					{
					  lowp vec3 worldNormal_1;
					  mediump vec3 tmpvar_2;
					  highp vec4 v_3;
					  highp float tmpvar_4;
					  tmpvar_4 = _World2Object[0].x;
					  v_3.x = tmpvar_4;
					  highp float tmpvar_5;
					  tmpvar_5 = _World2Object[1].x;
					  v_3.y = tmpvar_5;
					  highp float tmpvar_6;
					  tmpvar_6 = _World2Object[2].x;
					  v_3.z = tmpvar_6;
					  highp float tmpvar_7;
					  tmpvar_7 = _World2Object[3].x;
					  v_3.w = tmpvar_7;
					  highp vec4 v_8;
					  highp float tmpvar_9;
					  tmpvar_9 = _World2Object[0].y;
					  v_8.x = tmpvar_9;
					  highp float tmpvar_10;
					  tmpvar_10 = _World2Object[1].y;
					  v_8.y = tmpvar_10;
					  highp float tmpvar_11;
					  tmpvar_11 = _World2Object[2].y;
					  v_8.z = tmpvar_11;
					  highp float tmpvar_12;
					  tmpvar_12 = _World2Object[3].y;
					  v_8.w = tmpvar_12;
					  highp vec4 v_13;
					  highp float tmpvar_14;
					  tmpvar_14 = _World2Object[0].z;
					  v_13.x = tmpvar_14;
					  highp float tmpvar_15;
					  tmpvar_15 = _World2Object[1].z;
					  v_13.y = tmpvar_15;
					  highp float tmpvar_16;
					  tmpvar_16 = _World2Object[2].z;
					  v_13.z = tmpvar_16;
					  highp float tmpvar_17;
					  tmpvar_17 = _World2Object[3].z;
					  v_13.w = tmpvar_17;
					  highp vec3 tmpvar_18;
					  tmpvar_18 = normalize(((
					    (v_3.xyz * _glesNormal.x)
					   + 
					    (v_8.xyz * _glesNormal.y)
					  ) + (v_13.xyz * _glesNormal.z)));
					  highp mat3 tmpvar_19;
					  tmpvar_19[0] = _Object2World[0].xyz;
					  tmpvar_19[1] = _Object2World[1].xyz;
					  tmpvar_19[2] = _Object2World[2].xyz;
					  highp vec3 tmpvar_20;
					  tmpvar_20 = normalize((tmpvar_19 * _glesTANGENT.xyz));
					  highp vec4 vertex_21;
					  vertex_21.w = _glesVertex.w;
					  highp vec2 xzOff_22;
					  highp vec3 v2_23;
					  highp vec3 v1_24;
					  highp vec3 v0_25;
					  highp vec3 tmpvar_26;
					  tmpvar_26 = ((_Object2World * _glesVertex).xyz - _V_CW_PivotPoint_Position.xyz);
					  highp vec3 tmpvar_27;
					  tmpvar_27 = (tmpvar_26 + tmpvar_20);
					  v1_24.xz = tmpvar_27.xz;
					  highp vec3 tmpvar_28;
					  tmpvar_28 = (tmpvar_26 - ((tmpvar_18.yzx * tmpvar_20.zxy) - (tmpvar_18.zxy * tmpvar_20.yzx)));
					  v2_23.xz = tmpvar_28.xz;
					  highp vec2 tmpvar_29;
					  tmpvar_29.x = float((tmpvar_26.z >= 0.0));
					  tmpvar_29.y = float((tmpvar_26.x >= 0.0));
					  xzOff_22 = (max (vec2(0.0, 0.0), (
					    abs(tmpvar_26.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_29 * 2.0) - 1.0));
					  xzOff_22 = (xzOff_22 * xzOff_22);
					  highp vec3 tmpvar_30;
					  tmpvar_30.xz = vec2(0.0, 0.0);
					  tmpvar_30.y = (((_V_CW_Bend.x * xzOff_22.x) + (_V_CW_Bend.z * xzOff_22.y)) * 0.001);
					  v0_25 = (tmpvar_26 + tmpvar_30);
					  highp vec2 tmpvar_31;
					  tmpvar_31.x = float((tmpvar_27.z >= 0.0));
					  tmpvar_31.y = float((tmpvar_27.x >= 0.0));
					  xzOff_22 = (max (vec2(0.0, 0.0), (
					    abs(tmpvar_27.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_31 * 2.0) - 1.0));
					  xzOff_22 = (xzOff_22 * xzOff_22);
					  v1_24.y = (tmpvar_27.y + ((
					    (_V_CW_Bend.x * xzOff_22.x)
					   + 
					    (_V_CW_Bend.z * xzOff_22.y)
					  ) * 0.001));
					  highp vec2 tmpvar_32;
					  tmpvar_32.x = float((tmpvar_28.z >= 0.0));
					  tmpvar_32.y = float((tmpvar_28.x >= 0.0));
					  xzOff_22 = (max (vec2(0.0, 0.0), (
					    abs(tmpvar_28.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_32 * 2.0) - 1.0));
					  xzOff_22 = (xzOff_22 * xzOff_22);
					  v2_23.y = (tmpvar_28.y + ((
					    (_V_CW_Bend.x * xzOff_22.x)
					   + 
					    (_V_CW_Bend.z * xzOff_22.y)
					  ) * 0.001));
					  highp mat3 tmpvar_33;
					  tmpvar_33[0] = _World2Object[0].xyz;
					  tmpvar_33[1] = _World2Object[1].xyz;
					  tmpvar_33[2] = _World2Object[2].xyz;
					  vertex_21.xyz = (_glesVertex.xyz + (tmpvar_33 * tmpvar_30));
					  highp mat3 tmpvar_34;
					  tmpvar_34[0] = _World2Object[0].xyz;
					  tmpvar_34[1] = _World2Object[1].xyz;
					  tmpvar_34[2] = _World2Object[2].xyz;
					  highp vec3 a_35;
					  a_35 = (v2_23 - v0_25);
					  highp vec3 b_36;
					  b_36 = (v1_24 - v0_25);
					  highp vec3 tmpvar_37;
					  tmpvar_37 = normalize((tmpvar_34 * normalize(
					    ((a_35.yzx * b_36.zxy) - (a_35.zxy * b_36.yzx))
					  )));
					  highp vec4 v_38;
					  v_38.x = tmpvar_4;
					  v_38.y = tmpvar_5;
					  v_38.z = tmpvar_6;
					  v_38.w = tmpvar_7;
					  highp vec4 v_39;
					  v_39.x = tmpvar_9;
					  v_39.y = tmpvar_10;
					  v_39.z = tmpvar_11;
					  v_39.w = tmpvar_12;
					  highp vec4 v_40;
					  v_40.x = tmpvar_14;
					  v_40.y = tmpvar_15;
					  v_40.z = tmpvar_16;
					  v_40.w = tmpvar_17;
					  highp vec3 tmpvar_41;
					  tmpvar_41 = normalize(((
					    (v_38.xyz * tmpvar_37.x)
					   + 
					    (v_39.xyz * tmpvar_37.y)
					  ) + (v_40.xyz * tmpvar_37.z)));
					  worldNormal_1 = tmpvar_41;
					  tmpvar_2 = worldNormal_1;
					  mediump vec3 normal_42;
					  normal_42 = worldNormal_1;
					  mediump vec3 x1_43;
					  mediump vec4 tmpvar_44;
					  tmpvar_44 = (normal_42.xyzz * normal_42.yzzx);
					  x1_43.x = dot (unity_SHBr, tmpvar_44);
					  x1_43.y = dot (unity_SHBg, tmpvar_44);
					  x1_43.z = dot (unity_SHBb, tmpvar_44);
					  gl_Position = (glstate_matrix_mvp * vertex_21);
					  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
					  xlv_TEXCOORD1 = tmpvar_2;
					  xlv_TEXCOORD2 = (_Object2World * vertex_21).xyz;
					  xlv_COLOR0 = _glesColor;
					  xlv_TEXCOORD3 = (x1_43 + (unity_SHC.xyz * (
					    (normal_42.x * normal_42.x)
					   - 
					    (normal_42.y * normal_42.y)
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
					varying highp vec2 xlv_TEXCOORD0;
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
					  lowp vec4 tmpvar_8;
					  highp vec2 P_9;
					  P_9 = (xlv_TEXCOORD0 + (_V_CW_MainTex_Scroll * _Time.x));
					  tmpvar_8 = texture2D (_MainTex, P_9);
					  tmpvar_7 = (tmpvar_8.xyz * _Color.xyz);
					  tmpvar_7 = (tmpvar_7 * tmpvar_5.xyz);
					  tmpvar_1 = _LightColor0.xyz;
					  tmpvar_2 = lightDir_4;
					  mediump vec3 normalWorld_10;
					  normalWorld_10 = tmpvar_3;
					  mediump vec3 ambient_11;
					  mediump vec4 tmpvar_12;
					  tmpvar_12.w = 1.0;
					  tmpvar_12.xyz = normalWorld_10;
					  mediump vec3 x_13;
					  x_13.x = dot (unity_SHAr, tmpvar_12);
					  x_13.y = dot (unity_SHAg, tmpvar_12);
					  x_13.z = dot (unity_SHAb, tmpvar_12);
					  ambient_11 = max (((1.055 * 
					    pow (max (vec3(0.0, 0.0, 0.0), (xlv_TEXCOORD3 + x_13)), vec3(0.4166667, 0.4166667, 0.4166667))
					  ) - 0.055), vec3(0.0, 0.0, 0.0));
					  lowp vec4 c_14;
					  lowp vec4 c_15;
					  lowp float diff_16;
					  mediump float tmpvar_17;
					  tmpvar_17 = max (0.0, dot (tmpvar_3, tmpvar_2));
					  diff_16 = tmpvar_17;
					  c_15.xyz = ((tmpvar_7 * tmpvar_1) * diff_16);
					  c_15.w = (tmpvar_8.w * _Color.w);
					  c_14.w = c_15.w;
					  c_14.xyz = (c_15.xyz + (tmpvar_7 * ambient_11));
					  gl_FragData[0] = c_14;
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
					varying highp vec2 xlv_TEXCOORD0;
					varying mediump vec3 xlv_TEXCOORD1;
					varying highp vec3 xlv_TEXCOORD2;
					varying lowp vec4 xlv_COLOR0;
					varying mediump vec3 xlv_TEXCOORD3;
					void main ()
					{
					  lowp vec3 worldNormal_1;
					  mediump vec3 tmpvar_2;
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
					  highp vec3 tmpvar_39;
					  tmpvar_39 = (_Object2World * vertex_22).xyz;
					  highp vec4 v_40;
					  v_40.x = tmpvar_5;
					  v_40.y = tmpvar_6;
					  v_40.z = tmpvar_7;
					  v_40.w = tmpvar_8;
					  highp vec4 v_41;
					  v_41.x = tmpvar_10;
					  v_41.y = tmpvar_11;
					  v_41.z = tmpvar_12;
					  v_41.w = tmpvar_13;
					  highp vec4 v_42;
					  v_42.x = tmpvar_15;
					  v_42.y = tmpvar_16;
					  v_42.z = tmpvar_17;
					  v_42.w = tmpvar_18;
					  highp vec3 tmpvar_43;
					  tmpvar_43 = normalize(((
					    (v_40.xyz * tmpvar_38.x)
					   + 
					    (v_41.xyz * tmpvar_38.y)
					  ) + (v_42.xyz * tmpvar_38.z)));
					  worldNormal_1 = tmpvar_43;
					  tmpvar_2 = worldNormal_1;
					  highp vec3 lightColor0_44;
					  lightColor0_44 = unity_LightColor[0].xyz;
					  highp vec3 lightColor1_45;
					  lightColor1_45 = unity_LightColor[1].xyz;
					  highp vec3 lightColor2_46;
					  lightColor2_46 = unity_LightColor[2].xyz;
					  highp vec3 lightColor3_47;
					  lightColor3_47 = unity_LightColor[3].xyz;
					  highp vec4 lightAttenSq_48;
					  lightAttenSq_48 = unity_4LightAtten0;
					  highp vec3 normal_49;
					  normal_49 = worldNormal_1;
					  highp vec3 col_50;
					  highp vec4 ndotl_51;
					  highp vec4 lengthSq_52;
					  highp vec4 tmpvar_53;
					  tmpvar_53 = (unity_4LightPosX0 - tmpvar_39.x);
					  highp vec4 tmpvar_54;
					  tmpvar_54 = (unity_4LightPosY0 - tmpvar_39.y);
					  highp vec4 tmpvar_55;
					  tmpvar_55 = (unity_4LightPosZ0 - tmpvar_39.z);
					  lengthSq_52 = (tmpvar_53 * tmpvar_53);
					  lengthSq_52 = (lengthSq_52 + (tmpvar_54 * tmpvar_54));
					  lengthSq_52 = (lengthSq_52 + (tmpvar_55 * tmpvar_55));
					  ndotl_51 = (tmpvar_53 * normal_49.x);
					  ndotl_51 = (ndotl_51 + (tmpvar_54 * normal_49.y));
					  ndotl_51 = (ndotl_51 + (tmpvar_55 * normal_49.z));
					  highp vec4 tmpvar_56;
					  tmpvar_56 = max (vec4(0.0, 0.0, 0.0, 0.0), (ndotl_51 * inversesqrt(lengthSq_52)));
					  ndotl_51 = tmpvar_56;
					  highp vec4 tmpvar_57;
					  tmpvar_57 = (tmpvar_56 * (1.0/((1.0 + 
					    (lengthSq_52 * lightAttenSq_48)
					  ))));
					  col_50 = (lightColor0_44 * tmpvar_57.x);
					  col_50 = (col_50 + (lightColor1_45 * tmpvar_57.y));
					  col_50 = (col_50 + (lightColor2_46 * tmpvar_57.z));
					  col_50 = (col_50 + (lightColor3_47 * tmpvar_57.w));
					  tmpvar_3 = col_50;
					  mediump vec3 normal_58;
					  normal_58 = worldNormal_1;
					  mediump vec3 ambient_59;
					  ambient_59 = (tmpvar_3 * ((tmpvar_3 * 
					    ((tmpvar_3 * 0.305306) + 0.6821711)
					  ) + 0.01252288));
					  mediump vec3 x1_60;
					  mediump vec4 tmpvar_61;
					  tmpvar_61 = (normal_58.xyzz * normal_58.yzzx);
					  x1_60.x = dot (unity_SHBr, tmpvar_61);
					  x1_60.y = dot (unity_SHBg, tmpvar_61);
					  x1_60.z = dot (unity_SHBb, tmpvar_61);
					  ambient_59 = (ambient_59 + (x1_60 + (unity_SHC.xyz * 
					    ((normal_58.x * normal_58.x) - (normal_58.y * normal_58.y))
					  )));
					  tmpvar_3 = ambient_59;
					  gl_Position = (glstate_matrix_mvp * vertex_22);
					  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
					  xlv_TEXCOORD1 = tmpvar_2;
					  xlv_TEXCOORD2 = tmpvar_39;
					  xlv_COLOR0 = _glesColor;
					  xlv_TEXCOORD3 = ambient_59;
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
					varying highp vec2 xlv_TEXCOORD0;
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
					  lowp vec4 tmpvar_8;
					  highp vec2 P_9;
					  P_9 = (xlv_TEXCOORD0 + (_V_CW_MainTex_Scroll * _Time.x));
					  tmpvar_8 = texture2D (_MainTex, P_9);
					  tmpvar_7 = (tmpvar_8.xyz * _Color.xyz);
					  tmpvar_7 = (tmpvar_7 * tmpvar_5.xyz);
					  tmpvar_1 = _LightColor0.xyz;
					  tmpvar_2 = lightDir_4;
					  mediump vec3 normalWorld_10;
					  normalWorld_10 = tmpvar_3;
					  mediump vec3 ambient_11;
					  mediump vec4 tmpvar_12;
					  tmpvar_12.w = 1.0;
					  tmpvar_12.xyz = normalWorld_10;
					  mediump vec3 x_13;
					  x_13.x = dot (unity_SHAr, tmpvar_12);
					  x_13.y = dot (unity_SHAg, tmpvar_12);
					  x_13.z = dot (unity_SHAb, tmpvar_12);
					  ambient_11 = max (((1.055 * 
					    pow (max (vec3(0.0, 0.0, 0.0), (xlv_TEXCOORD3 + x_13)), vec3(0.4166667, 0.4166667, 0.4166667))
					  ) - 0.055), vec3(0.0, 0.0, 0.0));
					  lowp vec4 c_14;
					  lowp vec4 c_15;
					  lowp float diff_16;
					  mediump float tmpvar_17;
					  tmpvar_17 = max (0.0, dot (tmpvar_3, tmpvar_2));
					  diff_16 = tmpvar_17;
					  c_15.xyz = ((tmpvar_7 * tmpvar_1) * diff_16);
					  c_15.w = (tmpvar_8.w * _Color.w);
					  c_14.w = c_15.w;
					  c_14.xyz = (c_15.xyz + (tmpvar_7 * ambient_11));
					  gl_FragData[0] = c_14;
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
					varying highp vec2 xlv_TEXCOORD0;
					varying mediump vec3 xlv_TEXCOORD1;
					varying highp vec3 xlv_TEXCOORD2;
					varying lowp vec4 xlv_COLOR0;
					varying mediump vec3 xlv_TEXCOORD3;
					varying highp float xlv_TEXCOORD4;
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
					  tmpvar_2 = (glstate_matrix_mvp * vertex_22);
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
					  gl_Position = tmpvar_2;
					  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
					  xlv_TEXCOORD1 = tmpvar_3;
					  xlv_TEXCOORD2 = (_Object2World * vertex_22).xyz;
					  xlv_COLOR0 = _glesColor;
					  xlv_TEXCOORD3 = (x1_44 + (unity_SHC.xyz * (
					    (normal_43.x * normal_43.x)
					   - 
					    (normal_43.y * normal_43.y)
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
					varying highp vec2 xlv_TEXCOORD0;
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
					  lowp vec4 tmpvar_9;
					  highp vec2 P_10;
					  P_10 = (xlv_TEXCOORD0 + (_V_CW_MainTex_Scroll * _Time.x));
					  tmpvar_9 = texture2D (_MainTex, P_10);
					  tmpvar_8 = (tmpvar_9.xyz * _Color.xyz);
					  tmpvar_8 = (tmpvar_8 * tmpvar_6.xyz);
					  tmpvar_1 = _LightColor0.xyz;
					  tmpvar_2 = lightDir_5;
					  mediump vec3 normalWorld_11;
					  normalWorld_11 = tmpvar_4;
					  mediump vec3 ambient_12;
					  mediump vec4 tmpvar_13;
					  tmpvar_13.w = 1.0;
					  tmpvar_13.xyz = normalWorld_11;
					  mediump vec3 x_14;
					  x_14.x = dot (unity_SHAr, tmpvar_13);
					  x_14.y = dot (unity_SHAg, tmpvar_13);
					  x_14.z = dot (unity_SHAb, tmpvar_13);
					  ambient_12 = max (((1.055 * 
					    pow (max (vec3(0.0, 0.0, 0.0), (xlv_TEXCOORD3 + x_14)), vec3(0.4166667, 0.4166667, 0.4166667))
					  ) - 0.055), vec3(0.0, 0.0, 0.0));
					  lowp vec4 c_15;
					  lowp vec4 c_16;
					  lowp float diff_17;
					  mediump float tmpvar_18;
					  tmpvar_18 = max (0.0, dot (tmpvar_4, tmpvar_2));
					  diff_17 = tmpvar_18;
					  c_16.xyz = ((tmpvar_8 * tmpvar_1) * diff_17);
					  c_16.w = (tmpvar_9.w * _Color.w);
					  c_15.w = c_16.w;
					  c_15.xyz = (c_16.xyz + (tmpvar_8 * ambient_12));
					  c_3.w = c_15.w;
					  highp float tmpvar_19;
					  tmpvar_19 = clamp (xlv_TEXCOORD4, 0.0, 1.0);
					  c_3.xyz = mix (unity_FogColor.xyz, c_15.xyz, vec3(tmpvar_19));
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
					varying highp vec2 xlv_TEXCOORD0;
					varying mediump vec3 xlv_TEXCOORD1;
					varying highp vec3 xlv_TEXCOORD2;
					varying lowp vec4 xlv_COLOR0;
					varying mediump vec3 xlv_TEXCOORD3;
					varying highp float xlv_TEXCOORD4;
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
					  tmpvar_2 = (glstate_matrix_mvp * vertex_23);
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
					  gl_Position = tmpvar_2;
					  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
					  xlv_TEXCOORD1 = tmpvar_3;
					  xlv_TEXCOORD2 = tmpvar_40;
					  xlv_COLOR0 = _glesColor;
					  xlv_TEXCOORD3 = ambient_60;
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
					varying highp vec2 xlv_TEXCOORD0;
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
					  lowp vec4 tmpvar_9;
					  highp vec2 P_10;
					  P_10 = (xlv_TEXCOORD0 + (_V_CW_MainTex_Scroll * _Time.x));
					  tmpvar_9 = texture2D (_MainTex, P_10);
					  tmpvar_8 = (tmpvar_9.xyz * _Color.xyz);
					  tmpvar_8 = (tmpvar_8 * tmpvar_6.xyz);
					  tmpvar_1 = _LightColor0.xyz;
					  tmpvar_2 = lightDir_5;
					  mediump vec3 normalWorld_11;
					  normalWorld_11 = tmpvar_4;
					  mediump vec3 ambient_12;
					  mediump vec4 tmpvar_13;
					  tmpvar_13.w = 1.0;
					  tmpvar_13.xyz = normalWorld_11;
					  mediump vec3 x_14;
					  x_14.x = dot (unity_SHAr, tmpvar_13);
					  x_14.y = dot (unity_SHAg, tmpvar_13);
					  x_14.z = dot (unity_SHAb, tmpvar_13);
					  ambient_12 = max (((1.055 * 
					    pow (max (vec3(0.0, 0.0, 0.0), (xlv_TEXCOORD3 + x_14)), vec3(0.4166667, 0.4166667, 0.4166667))
					  ) - 0.055), vec3(0.0, 0.0, 0.0));
					  lowp vec4 c_15;
					  lowp vec4 c_16;
					  lowp float diff_17;
					  mediump float tmpvar_18;
					  tmpvar_18 = max (0.0, dot (tmpvar_4, tmpvar_2));
					  diff_17 = tmpvar_18;
					  c_16.xyz = ((tmpvar_8 * tmpvar_1) * diff_17);
					  c_16.w = (tmpvar_9.w * _Color.w);
					  c_15.w = c_16.w;
					  c_15.xyz = (c_16.xyz + (tmpvar_8 * ambient_12));
					  c_3.w = c_15.w;
					  highp float tmpvar_19;
					  tmpvar_19 = clamp (xlv_TEXCOORD4, 0.0, 1.0);
					  c_3.xyz = mix (unity_FogColor.xyz, c_15.xyz, vec3(tmpvar_19));
					  gl_FragData[0] = c_3;
					}
					
					
					#endif"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "V_CW_SPECULAR" "V_CW_VERTEX_COLOR" "V_CW_REFLECTIVE_OFF" "_EMISSION_OFF" "V_CW_RIM_OFF" "_NORMALMAP_OFF" }
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
					varying highp vec2 xlv_TEXCOORD0;
					varying mediump vec3 xlv_TEXCOORD1;
					varying highp vec3 xlv_TEXCOORD2;
					varying lowp vec4 xlv_COLOR0;
					varying mediump vec3 xlv_TEXCOORD3;
					void main ()
					{
					  lowp vec3 worldNormal_1;
					  mediump vec3 tmpvar_2;
					  highp vec4 v_3;
					  highp float tmpvar_4;
					  tmpvar_4 = _World2Object[0].x;
					  v_3.x = tmpvar_4;
					  highp float tmpvar_5;
					  tmpvar_5 = _World2Object[1].x;
					  v_3.y = tmpvar_5;
					  highp float tmpvar_6;
					  tmpvar_6 = _World2Object[2].x;
					  v_3.z = tmpvar_6;
					  highp float tmpvar_7;
					  tmpvar_7 = _World2Object[3].x;
					  v_3.w = tmpvar_7;
					  highp vec4 v_8;
					  highp float tmpvar_9;
					  tmpvar_9 = _World2Object[0].y;
					  v_8.x = tmpvar_9;
					  highp float tmpvar_10;
					  tmpvar_10 = _World2Object[1].y;
					  v_8.y = tmpvar_10;
					  highp float tmpvar_11;
					  tmpvar_11 = _World2Object[2].y;
					  v_8.z = tmpvar_11;
					  highp float tmpvar_12;
					  tmpvar_12 = _World2Object[3].y;
					  v_8.w = tmpvar_12;
					  highp vec4 v_13;
					  highp float tmpvar_14;
					  tmpvar_14 = _World2Object[0].z;
					  v_13.x = tmpvar_14;
					  highp float tmpvar_15;
					  tmpvar_15 = _World2Object[1].z;
					  v_13.y = tmpvar_15;
					  highp float tmpvar_16;
					  tmpvar_16 = _World2Object[2].z;
					  v_13.z = tmpvar_16;
					  highp float tmpvar_17;
					  tmpvar_17 = _World2Object[3].z;
					  v_13.w = tmpvar_17;
					  highp vec3 tmpvar_18;
					  tmpvar_18 = normalize(((
					    (v_3.xyz * _glesNormal.x)
					   + 
					    (v_8.xyz * _glesNormal.y)
					  ) + (v_13.xyz * _glesNormal.z)));
					  highp mat3 tmpvar_19;
					  tmpvar_19[0] = _Object2World[0].xyz;
					  tmpvar_19[1] = _Object2World[1].xyz;
					  tmpvar_19[2] = _Object2World[2].xyz;
					  highp vec3 tmpvar_20;
					  tmpvar_20 = normalize((tmpvar_19 * _glesTANGENT.xyz));
					  highp vec4 vertex_21;
					  vertex_21.w = _glesVertex.w;
					  highp vec2 xzOff_22;
					  highp vec3 v2_23;
					  highp vec3 v1_24;
					  highp vec3 v0_25;
					  highp vec3 tmpvar_26;
					  tmpvar_26 = ((_Object2World * _glesVertex).xyz - _V_CW_PivotPoint_Position.xyz);
					  highp vec3 tmpvar_27;
					  tmpvar_27 = (tmpvar_26 + tmpvar_20);
					  v1_24.xz = tmpvar_27.xz;
					  highp vec3 tmpvar_28;
					  tmpvar_28 = (tmpvar_26 - ((tmpvar_18.yzx * tmpvar_20.zxy) - (tmpvar_18.zxy * tmpvar_20.yzx)));
					  v2_23.xz = tmpvar_28.xz;
					  highp vec2 tmpvar_29;
					  tmpvar_29.x = float((tmpvar_26.z >= 0.0));
					  tmpvar_29.y = float((tmpvar_26.x >= 0.0));
					  xzOff_22 = (max (vec2(0.0, 0.0), (
					    abs(tmpvar_26.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_29 * 2.0) - 1.0));
					  xzOff_22 = (xzOff_22 * xzOff_22);
					  highp vec3 tmpvar_30;
					  tmpvar_30.xz = vec2(0.0, 0.0);
					  tmpvar_30.y = (((_V_CW_Bend.x * xzOff_22.x) + (_V_CW_Bend.z * xzOff_22.y)) * 0.001);
					  v0_25 = (tmpvar_26 + tmpvar_30);
					  highp vec2 tmpvar_31;
					  tmpvar_31.x = float((tmpvar_27.z >= 0.0));
					  tmpvar_31.y = float((tmpvar_27.x >= 0.0));
					  xzOff_22 = (max (vec2(0.0, 0.0), (
					    abs(tmpvar_27.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_31 * 2.0) - 1.0));
					  xzOff_22 = (xzOff_22 * xzOff_22);
					  v1_24.y = (tmpvar_27.y + ((
					    (_V_CW_Bend.x * xzOff_22.x)
					   + 
					    (_V_CW_Bend.z * xzOff_22.y)
					  ) * 0.001));
					  highp vec2 tmpvar_32;
					  tmpvar_32.x = float((tmpvar_28.z >= 0.0));
					  tmpvar_32.y = float((tmpvar_28.x >= 0.0));
					  xzOff_22 = (max (vec2(0.0, 0.0), (
					    abs(tmpvar_28.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_32 * 2.0) - 1.0));
					  xzOff_22 = (xzOff_22 * xzOff_22);
					  v2_23.y = (tmpvar_28.y + ((
					    (_V_CW_Bend.x * xzOff_22.x)
					   + 
					    (_V_CW_Bend.z * xzOff_22.y)
					  ) * 0.001));
					  highp mat3 tmpvar_33;
					  tmpvar_33[0] = _World2Object[0].xyz;
					  tmpvar_33[1] = _World2Object[1].xyz;
					  tmpvar_33[2] = _World2Object[2].xyz;
					  vertex_21.xyz = (_glesVertex.xyz + (tmpvar_33 * tmpvar_30));
					  highp mat3 tmpvar_34;
					  tmpvar_34[0] = _World2Object[0].xyz;
					  tmpvar_34[1] = _World2Object[1].xyz;
					  tmpvar_34[2] = _World2Object[2].xyz;
					  highp vec3 a_35;
					  a_35 = (v2_23 - v0_25);
					  highp vec3 b_36;
					  b_36 = (v1_24 - v0_25);
					  highp vec3 tmpvar_37;
					  tmpvar_37 = normalize((tmpvar_34 * normalize(
					    ((a_35.yzx * b_36.zxy) - (a_35.zxy * b_36.yzx))
					  )));
					  highp vec4 v_38;
					  v_38.x = tmpvar_4;
					  v_38.y = tmpvar_5;
					  v_38.z = tmpvar_6;
					  v_38.w = tmpvar_7;
					  highp vec4 v_39;
					  v_39.x = tmpvar_9;
					  v_39.y = tmpvar_10;
					  v_39.z = tmpvar_11;
					  v_39.w = tmpvar_12;
					  highp vec4 v_40;
					  v_40.x = tmpvar_14;
					  v_40.y = tmpvar_15;
					  v_40.z = tmpvar_16;
					  v_40.w = tmpvar_17;
					  highp vec3 tmpvar_41;
					  tmpvar_41 = normalize(((
					    (v_38.xyz * tmpvar_37.x)
					   + 
					    (v_39.xyz * tmpvar_37.y)
					  ) + (v_40.xyz * tmpvar_37.z)));
					  worldNormal_1 = tmpvar_41;
					  tmpvar_2 = worldNormal_1;
					  mediump vec3 normal_42;
					  normal_42 = worldNormal_1;
					  mediump vec3 x1_43;
					  mediump vec4 tmpvar_44;
					  tmpvar_44 = (normal_42.xyzz * normal_42.yzzx);
					  x1_43.x = dot (unity_SHBr, tmpvar_44);
					  x1_43.y = dot (unity_SHBg, tmpvar_44);
					  x1_43.z = dot (unity_SHBb, tmpvar_44);
					  gl_Position = (glstate_matrix_mvp * vertex_21);
					  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
					  xlv_TEXCOORD1 = tmpvar_2;
					  xlv_TEXCOORD2 = (_Object2World * vertex_21).xyz;
					  xlv_COLOR0 = _glesColor;
					  xlv_TEXCOORD3 = (x1_43 + (unity_SHC.xyz * (
					    (normal_42.x * normal_42.x)
					   - 
					    (normal_42.y * normal_42.y)
					  )));
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform highp vec4 _Time;
					uniform highp vec3 _WorldSpaceCameraPos;
					uniform mediump vec4 _WorldSpaceLightPos0;
					uniform mediump vec4 unity_SHAr;
					uniform mediump vec4 unity_SHAg;
					uniform mediump vec4 unity_SHAb;
					uniform lowp vec4 _LightColor0;
					uniform lowp vec4 _SpecColor;
					uniform sampler2D _MainTex;
					uniform lowp vec4 _Color;
					uniform lowp vec2 _V_CW_MainTex_Scroll;
					uniform mediump float _Shininess;
					varying highp vec2 xlv_TEXCOORD0;
					varying mediump vec3 xlv_TEXCOORD1;
					varying highp vec3 xlv_TEXCOORD2;
					varying lowp vec4 xlv_COLOR0;
					varying mediump vec3 xlv_TEXCOORD3;
					void main ()
					{
					  mediump vec3 tmpvar_1;
					  mediump vec3 tmpvar_2;
					  lowp vec3 tmpvar_3;
					  lowp vec3 worldViewDir_4;
					  lowp vec3 lightDir_5;
					  highp vec4 tmpvar_6;
					  mediump vec3 tmpvar_7;
					  tmpvar_7 = _WorldSpaceLightPos0.xyz;
					  lightDir_5 = tmpvar_7;
					  highp vec3 tmpvar_8;
					  tmpvar_8 = normalize((_WorldSpaceCameraPos - xlv_TEXCOORD2));
					  worldViewDir_4 = tmpvar_8;
					  tmpvar_6 = xlv_COLOR0;
					  tmpvar_3 = xlv_TEXCOORD1;
					  lowp vec3 tmpvar_9;
					  lowp vec4 tmpvar_10;
					  highp vec2 P_11;
					  P_11 = (xlv_TEXCOORD0 + (_V_CW_MainTex_Scroll * _Time.x));
					  tmpvar_10 = texture2D (_MainTex, P_11);
					  tmpvar_9 = (tmpvar_10.xyz * _Color.xyz);
					  tmpvar_9 = (tmpvar_9 * tmpvar_6.xyz);
					  tmpvar_1 = _LightColor0.xyz;
					  tmpvar_2 = lightDir_5;
					  mediump vec3 normalWorld_12;
					  normalWorld_12 = tmpvar_3;
					  mediump vec3 ambient_13;
					  mediump vec4 tmpvar_14;
					  tmpvar_14.w = 1.0;
					  tmpvar_14.xyz = normalWorld_12;
					  mediump vec3 x_15;
					  x_15.x = dot (unity_SHAr, tmpvar_14);
					  x_15.y = dot (unity_SHAg, tmpvar_14);
					  x_15.z = dot (unity_SHAb, tmpvar_14);
					  ambient_13 = max (((1.055 * 
					    pow (max (vec3(0.0, 0.0, 0.0), (xlv_TEXCOORD3 + x_15)), vec3(0.4166667, 0.4166667, 0.4166667))
					  ) - 0.055), vec3(0.0, 0.0, 0.0));
					  mediump vec3 viewDir_16;
					  viewDir_16 = worldViewDir_4;
					  lowp vec4 c_17;
					  lowp vec4 c_18;
					  highp float nh_19;
					  lowp float diff_20;
					  mediump float tmpvar_21;
					  tmpvar_21 = max (0.0, dot (tmpvar_3, tmpvar_2));
					  diff_20 = tmpvar_21;
					  mediump float tmpvar_22;
					  tmpvar_22 = max (0.0, dot (tmpvar_3, normalize(
					    (tmpvar_2 + viewDir_16)
					  )));
					  nh_19 = tmpvar_22;
					  mediump float y_23;
					  y_23 = (_Shininess * 128.0);
					  highp float tmpvar_24;
					  tmpvar_24 = (pow (nh_19, y_23) * tmpvar_10.w);
					  c_18.xyz = (((tmpvar_9 * tmpvar_1) * diff_20) + ((tmpvar_1 * _SpecColor.xyz) * tmpvar_24));
					  c_18.w = (tmpvar_10.w * _Color.w);
					  c_17.w = c_18.w;
					  c_17.xyz = (c_18.xyz + (tmpvar_9 * ambient_13));
					  gl_FragData[0] = c_17;
					}
					
					
					#endif"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "VERTEXLIGHT_ON" "V_CW_SPECULAR" "V_CW_VERTEX_COLOR" "V_CW_REFLECTIVE_OFF" "_EMISSION_OFF" "V_CW_RIM_OFF" "_NORMALMAP_OFF" }
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
					varying highp vec2 xlv_TEXCOORD0;
					varying mediump vec3 xlv_TEXCOORD1;
					varying highp vec3 xlv_TEXCOORD2;
					varying lowp vec4 xlv_COLOR0;
					varying mediump vec3 xlv_TEXCOORD3;
					void main ()
					{
					  lowp vec3 worldNormal_1;
					  mediump vec3 tmpvar_2;
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
					  highp vec3 tmpvar_39;
					  tmpvar_39 = (_Object2World * vertex_22).xyz;
					  highp vec4 v_40;
					  v_40.x = tmpvar_5;
					  v_40.y = tmpvar_6;
					  v_40.z = tmpvar_7;
					  v_40.w = tmpvar_8;
					  highp vec4 v_41;
					  v_41.x = tmpvar_10;
					  v_41.y = tmpvar_11;
					  v_41.z = tmpvar_12;
					  v_41.w = tmpvar_13;
					  highp vec4 v_42;
					  v_42.x = tmpvar_15;
					  v_42.y = tmpvar_16;
					  v_42.z = tmpvar_17;
					  v_42.w = tmpvar_18;
					  highp vec3 tmpvar_43;
					  tmpvar_43 = normalize(((
					    (v_40.xyz * tmpvar_38.x)
					   + 
					    (v_41.xyz * tmpvar_38.y)
					  ) + (v_42.xyz * tmpvar_38.z)));
					  worldNormal_1 = tmpvar_43;
					  tmpvar_2 = worldNormal_1;
					  highp vec3 lightColor0_44;
					  lightColor0_44 = unity_LightColor[0].xyz;
					  highp vec3 lightColor1_45;
					  lightColor1_45 = unity_LightColor[1].xyz;
					  highp vec3 lightColor2_46;
					  lightColor2_46 = unity_LightColor[2].xyz;
					  highp vec3 lightColor3_47;
					  lightColor3_47 = unity_LightColor[3].xyz;
					  highp vec4 lightAttenSq_48;
					  lightAttenSq_48 = unity_4LightAtten0;
					  highp vec3 normal_49;
					  normal_49 = worldNormal_1;
					  highp vec3 col_50;
					  highp vec4 ndotl_51;
					  highp vec4 lengthSq_52;
					  highp vec4 tmpvar_53;
					  tmpvar_53 = (unity_4LightPosX0 - tmpvar_39.x);
					  highp vec4 tmpvar_54;
					  tmpvar_54 = (unity_4LightPosY0 - tmpvar_39.y);
					  highp vec4 tmpvar_55;
					  tmpvar_55 = (unity_4LightPosZ0 - tmpvar_39.z);
					  lengthSq_52 = (tmpvar_53 * tmpvar_53);
					  lengthSq_52 = (lengthSq_52 + (tmpvar_54 * tmpvar_54));
					  lengthSq_52 = (lengthSq_52 + (tmpvar_55 * tmpvar_55));
					  ndotl_51 = (tmpvar_53 * normal_49.x);
					  ndotl_51 = (ndotl_51 + (tmpvar_54 * normal_49.y));
					  ndotl_51 = (ndotl_51 + (tmpvar_55 * normal_49.z));
					  highp vec4 tmpvar_56;
					  tmpvar_56 = max (vec4(0.0, 0.0, 0.0, 0.0), (ndotl_51 * inversesqrt(lengthSq_52)));
					  ndotl_51 = tmpvar_56;
					  highp vec4 tmpvar_57;
					  tmpvar_57 = (tmpvar_56 * (1.0/((1.0 + 
					    (lengthSq_52 * lightAttenSq_48)
					  ))));
					  col_50 = (lightColor0_44 * tmpvar_57.x);
					  col_50 = (col_50 + (lightColor1_45 * tmpvar_57.y));
					  col_50 = (col_50 + (lightColor2_46 * tmpvar_57.z));
					  col_50 = (col_50 + (lightColor3_47 * tmpvar_57.w));
					  tmpvar_3 = col_50;
					  mediump vec3 normal_58;
					  normal_58 = worldNormal_1;
					  mediump vec3 ambient_59;
					  ambient_59 = (tmpvar_3 * ((tmpvar_3 * 
					    ((tmpvar_3 * 0.305306) + 0.6821711)
					  ) + 0.01252288));
					  mediump vec3 x1_60;
					  mediump vec4 tmpvar_61;
					  tmpvar_61 = (normal_58.xyzz * normal_58.yzzx);
					  x1_60.x = dot (unity_SHBr, tmpvar_61);
					  x1_60.y = dot (unity_SHBg, tmpvar_61);
					  x1_60.z = dot (unity_SHBb, tmpvar_61);
					  ambient_59 = (ambient_59 + (x1_60 + (unity_SHC.xyz * 
					    ((normal_58.x * normal_58.x) - (normal_58.y * normal_58.y))
					  )));
					  tmpvar_3 = ambient_59;
					  gl_Position = (glstate_matrix_mvp * vertex_22);
					  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
					  xlv_TEXCOORD1 = tmpvar_2;
					  xlv_TEXCOORD2 = tmpvar_39;
					  xlv_COLOR0 = _glesColor;
					  xlv_TEXCOORD3 = ambient_59;
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform highp vec4 _Time;
					uniform highp vec3 _WorldSpaceCameraPos;
					uniform mediump vec4 _WorldSpaceLightPos0;
					uniform mediump vec4 unity_SHAr;
					uniform mediump vec4 unity_SHAg;
					uniform mediump vec4 unity_SHAb;
					uniform lowp vec4 _LightColor0;
					uniform lowp vec4 _SpecColor;
					uniform sampler2D _MainTex;
					uniform lowp vec4 _Color;
					uniform lowp vec2 _V_CW_MainTex_Scroll;
					uniform mediump float _Shininess;
					varying highp vec2 xlv_TEXCOORD0;
					varying mediump vec3 xlv_TEXCOORD1;
					varying highp vec3 xlv_TEXCOORD2;
					varying lowp vec4 xlv_COLOR0;
					varying mediump vec3 xlv_TEXCOORD3;
					void main ()
					{
					  mediump vec3 tmpvar_1;
					  mediump vec3 tmpvar_2;
					  lowp vec3 tmpvar_3;
					  lowp vec3 worldViewDir_4;
					  lowp vec3 lightDir_5;
					  highp vec4 tmpvar_6;
					  mediump vec3 tmpvar_7;
					  tmpvar_7 = _WorldSpaceLightPos0.xyz;
					  lightDir_5 = tmpvar_7;
					  highp vec3 tmpvar_8;
					  tmpvar_8 = normalize((_WorldSpaceCameraPos - xlv_TEXCOORD2));
					  worldViewDir_4 = tmpvar_8;
					  tmpvar_6 = xlv_COLOR0;
					  tmpvar_3 = xlv_TEXCOORD1;
					  lowp vec3 tmpvar_9;
					  lowp vec4 tmpvar_10;
					  highp vec2 P_11;
					  P_11 = (xlv_TEXCOORD0 + (_V_CW_MainTex_Scroll * _Time.x));
					  tmpvar_10 = texture2D (_MainTex, P_11);
					  tmpvar_9 = (tmpvar_10.xyz * _Color.xyz);
					  tmpvar_9 = (tmpvar_9 * tmpvar_6.xyz);
					  tmpvar_1 = _LightColor0.xyz;
					  tmpvar_2 = lightDir_5;
					  mediump vec3 normalWorld_12;
					  normalWorld_12 = tmpvar_3;
					  mediump vec3 ambient_13;
					  mediump vec4 tmpvar_14;
					  tmpvar_14.w = 1.0;
					  tmpvar_14.xyz = normalWorld_12;
					  mediump vec3 x_15;
					  x_15.x = dot (unity_SHAr, tmpvar_14);
					  x_15.y = dot (unity_SHAg, tmpvar_14);
					  x_15.z = dot (unity_SHAb, tmpvar_14);
					  ambient_13 = max (((1.055 * 
					    pow (max (vec3(0.0, 0.0, 0.0), (xlv_TEXCOORD3 + x_15)), vec3(0.4166667, 0.4166667, 0.4166667))
					  ) - 0.055), vec3(0.0, 0.0, 0.0));
					  mediump vec3 viewDir_16;
					  viewDir_16 = worldViewDir_4;
					  lowp vec4 c_17;
					  lowp vec4 c_18;
					  highp float nh_19;
					  lowp float diff_20;
					  mediump float tmpvar_21;
					  tmpvar_21 = max (0.0, dot (tmpvar_3, tmpvar_2));
					  diff_20 = tmpvar_21;
					  mediump float tmpvar_22;
					  tmpvar_22 = max (0.0, dot (tmpvar_3, normalize(
					    (tmpvar_2 + viewDir_16)
					  )));
					  nh_19 = tmpvar_22;
					  mediump float y_23;
					  y_23 = (_Shininess * 128.0);
					  highp float tmpvar_24;
					  tmpvar_24 = (pow (nh_19, y_23) * tmpvar_10.w);
					  c_18.xyz = (((tmpvar_9 * tmpvar_1) * diff_20) + ((tmpvar_1 * _SpecColor.xyz) * tmpvar_24));
					  c_18.w = (tmpvar_10.w * _Color.w);
					  c_17.w = c_18.w;
					  c_17.xyz = (c_18.xyz + (tmpvar_9 * ambient_13));
					  gl_FragData[0] = c_17;
					}
					
					
					#endif"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "FOG_LINEAR" "V_CW_SPECULAR" "V_CW_VERTEX_COLOR" "V_CW_REFLECTIVE_OFF" "_EMISSION_OFF" "V_CW_RIM_OFF" "_NORMALMAP_OFF" }
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
					varying highp vec2 xlv_TEXCOORD0;
					varying mediump vec3 xlv_TEXCOORD1;
					varying highp vec3 xlv_TEXCOORD2;
					varying lowp vec4 xlv_COLOR0;
					varying mediump vec3 xlv_TEXCOORD3;
					varying highp float xlv_TEXCOORD4;
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
					  tmpvar_2 = (glstate_matrix_mvp * vertex_22);
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
					  gl_Position = tmpvar_2;
					  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
					  xlv_TEXCOORD1 = tmpvar_3;
					  xlv_TEXCOORD2 = (_Object2World * vertex_22).xyz;
					  xlv_COLOR0 = _glesColor;
					  xlv_TEXCOORD3 = (x1_44 + (unity_SHC.xyz * (
					    (normal_43.x * normal_43.x)
					   - 
					    (normal_43.y * normal_43.y)
					  )));
					  xlv_TEXCOORD4 = ((tmpvar_2.z * unity_FogParams.z) + unity_FogParams.w);
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform highp vec4 _Time;
					uniform highp vec3 _WorldSpaceCameraPos;
					uniform mediump vec4 _WorldSpaceLightPos0;
					uniform mediump vec4 unity_SHAr;
					uniform mediump vec4 unity_SHAg;
					uniform mediump vec4 unity_SHAb;
					uniform lowp vec4 unity_FogColor;
					uniform lowp vec4 _LightColor0;
					uniform lowp vec4 _SpecColor;
					uniform sampler2D _MainTex;
					uniform lowp vec4 _Color;
					uniform lowp vec2 _V_CW_MainTex_Scroll;
					uniform mediump float _Shininess;
					varying highp vec2 xlv_TEXCOORD0;
					varying mediump vec3 xlv_TEXCOORD1;
					varying highp vec3 xlv_TEXCOORD2;
					varying lowp vec4 xlv_COLOR0;
					varying mediump vec3 xlv_TEXCOORD3;
					varying highp float xlv_TEXCOORD4;
					void main ()
					{
					  mediump vec3 tmpvar_1;
					  mediump vec3 tmpvar_2;
					  lowp vec4 c_3;
					  lowp vec3 tmpvar_4;
					  lowp vec3 worldViewDir_5;
					  lowp vec3 lightDir_6;
					  highp vec4 tmpvar_7;
					  mediump vec3 tmpvar_8;
					  tmpvar_8 = _WorldSpaceLightPos0.xyz;
					  lightDir_6 = tmpvar_8;
					  highp vec3 tmpvar_9;
					  tmpvar_9 = normalize((_WorldSpaceCameraPos - xlv_TEXCOORD2));
					  worldViewDir_5 = tmpvar_9;
					  tmpvar_7 = xlv_COLOR0;
					  tmpvar_4 = xlv_TEXCOORD1;
					  lowp vec3 tmpvar_10;
					  lowp vec4 tmpvar_11;
					  highp vec2 P_12;
					  P_12 = (xlv_TEXCOORD0 + (_V_CW_MainTex_Scroll * _Time.x));
					  tmpvar_11 = texture2D (_MainTex, P_12);
					  tmpvar_10 = (tmpvar_11.xyz * _Color.xyz);
					  tmpvar_10 = (tmpvar_10 * tmpvar_7.xyz);
					  tmpvar_1 = _LightColor0.xyz;
					  tmpvar_2 = lightDir_6;
					  mediump vec3 normalWorld_13;
					  normalWorld_13 = tmpvar_4;
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
					  mediump vec3 viewDir_17;
					  viewDir_17 = worldViewDir_5;
					  lowp vec4 c_18;
					  lowp vec4 c_19;
					  highp float nh_20;
					  lowp float diff_21;
					  mediump float tmpvar_22;
					  tmpvar_22 = max (0.0, dot (tmpvar_4, tmpvar_2));
					  diff_21 = tmpvar_22;
					  mediump float tmpvar_23;
					  tmpvar_23 = max (0.0, dot (tmpvar_4, normalize(
					    (tmpvar_2 + viewDir_17)
					  )));
					  nh_20 = tmpvar_23;
					  mediump float y_24;
					  y_24 = (_Shininess * 128.0);
					  highp float tmpvar_25;
					  tmpvar_25 = (pow (nh_20, y_24) * tmpvar_11.w);
					  c_19.xyz = (((tmpvar_10 * tmpvar_1) * diff_21) + ((tmpvar_1 * _SpecColor.xyz) * tmpvar_25));
					  c_19.w = (tmpvar_11.w * _Color.w);
					  c_18.w = c_19.w;
					  c_18.xyz = (c_19.xyz + (tmpvar_10 * ambient_14));
					  c_3.w = c_18.w;
					  highp float tmpvar_26;
					  tmpvar_26 = clamp (xlv_TEXCOORD4, 0.0, 1.0);
					  c_3.xyz = mix (unity_FogColor.xyz, c_18.xyz, vec3(tmpvar_26));
					  gl_FragData[0] = c_3;
					}
					
					
					#endif"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "FOG_LINEAR" "VERTEXLIGHT_ON" "V_CW_SPECULAR" "V_CW_VERTEX_COLOR" "V_CW_REFLECTIVE_OFF" "_EMISSION_OFF" "V_CW_RIM_OFF" "_NORMALMAP_OFF" }
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
					varying highp vec2 xlv_TEXCOORD0;
					varying mediump vec3 xlv_TEXCOORD1;
					varying highp vec3 xlv_TEXCOORD2;
					varying lowp vec4 xlv_COLOR0;
					varying mediump vec3 xlv_TEXCOORD3;
					varying highp float xlv_TEXCOORD4;
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
					  tmpvar_2 = (glstate_matrix_mvp * vertex_23);
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
					  gl_Position = tmpvar_2;
					  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
					  xlv_TEXCOORD1 = tmpvar_3;
					  xlv_TEXCOORD2 = tmpvar_40;
					  xlv_COLOR0 = _glesColor;
					  xlv_TEXCOORD3 = ambient_60;
					  xlv_TEXCOORD4 = ((tmpvar_2.z * unity_FogParams.z) + unity_FogParams.w);
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform highp vec4 _Time;
					uniform highp vec3 _WorldSpaceCameraPos;
					uniform mediump vec4 _WorldSpaceLightPos0;
					uniform mediump vec4 unity_SHAr;
					uniform mediump vec4 unity_SHAg;
					uniform mediump vec4 unity_SHAb;
					uniform lowp vec4 unity_FogColor;
					uniform lowp vec4 _LightColor0;
					uniform lowp vec4 _SpecColor;
					uniform sampler2D _MainTex;
					uniform lowp vec4 _Color;
					uniform lowp vec2 _V_CW_MainTex_Scroll;
					uniform mediump float _Shininess;
					varying highp vec2 xlv_TEXCOORD0;
					varying mediump vec3 xlv_TEXCOORD1;
					varying highp vec3 xlv_TEXCOORD2;
					varying lowp vec4 xlv_COLOR0;
					varying mediump vec3 xlv_TEXCOORD3;
					varying highp float xlv_TEXCOORD4;
					void main ()
					{
					  mediump vec3 tmpvar_1;
					  mediump vec3 tmpvar_2;
					  lowp vec4 c_3;
					  lowp vec3 tmpvar_4;
					  lowp vec3 worldViewDir_5;
					  lowp vec3 lightDir_6;
					  highp vec4 tmpvar_7;
					  mediump vec3 tmpvar_8;
					  tmpvar_8 = _WorldSpaceLightPos0.xyz;
					  lightDir_6 = tmpvar_8;
					  highp vec3 tmpvar_9;
					  tmpvar_9 = normalize((_WorldSpaceCameraPos - xlv_TEXCOORD2));
					  worldViewDir_5 = tmpvar_9;
					  tmpvar_7 = xlv_COLOR0;
					  tmpvar_4 = xlv_TEXCOORD1;
					  lowp vec3 tmpvar_10;
					  lowp vec4 tmpvar_11;
					  highp vec2 P_12;
					  P_12 = (xlv_TEXCOORD0 + (_V_CW_MainTex_Scroll * _Time.x));
					  tmpvar_11 = texture2D (_MainTex, P_12);
					  tmpvar_10 = (tmpvar_11.xyz * _Color.xyz);
					  tmpvar_10 = (tmpvar_10 * tmpvar_7.xyz);
					  tmpvar_1 = _LightColor0.xyz;
					  tmpvar_2 = lightDir_6;
					  mediump vec3 normalWorld_13;
					  normalWorld_13 = tmpvar_4;
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
					  mediump vec3 viewDir_17;
					  viewDir_17 = worldViewDir_5;
					  lowp vec4 c_18;
					  lowp vec4 c_19;
					  highp float nh_20;
					  lowp float diff_21;
					  mediump float tmpvar_22;
					  tmpvar_22 = max (0.0, dot (tmpvar_4, tmpvar_2));
					  diff_21 = tmpvar_22;
					  mediump float tmpvar_23;
					  tmpvar_23 = max (0.0, dot (tmpvar_4, normalize(
					    (tmpvar_2 + viewDir_17)
					  )));
					  nh_20 = tmpvar_23;
					  mediump float y_24;
					  y_24 = (_Shininess * 128.0);
					  highp float tmpvar_25;
					  tmpvar_25 = (pow (nh_20, y_24) * tmpvar_11.w);
					  c_19.xyz = (((tmpvar_10 * tmpvar_1) * diff_21) + ((tmpvar_1 * _SpecColor.xyz) * tmpvar_25));
					  c_19.w = (tmpvar_11.w * _Color.w);
					  c_18.w = c_19.w;
					  c_18.xyz = (c_19.xyz + (tmpvar_10 * ambient_14));
					  c_3.w = c_18.w;
					  highp float tmpvar_26;
					  tmpvar_26 = clamp (xlv_TEXCOORD4, 0.0, 1.0);
					  c_3.xyz = mix (unity_FogColor.xyz, c_18.xyz, vec3(tmpvar_26));
					  gl_FragData[0] = c_3;
					}
					
					
					#endif"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "_EMISSION" "V_CW_VERTEX_COLOR" "_NORMALMAP" "V_CW_REFLECTIVE_OFF" "V_CW_RIM_OFF" "V_CW_SPECULAR_OFF" }
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
					uniform highp vec4 unity_WorldTransformParams;
					uniform highp vec3 _V_CW_Bend;
					uniform highp vec3 _V_CW_Bias;
					uniform highp vec4 _V_CW_PivotPoint_Position;
					uniform highp vec4 _MainTex_ST;
					varying highp vec2 xlv_TEXCOORD0;
					varying highp vec4 xlv_TEXCOORD1;
					varying highp vec4 xlv_TEXCOORD2;
					varying highp vec4 xlv_TEXCOORD3;
					varying lowp vec4 xlv_COLOR0;
					varying mediump vec3 xlv_TEXCOORD4;
					void main ()
					{
					  lowp float tangentSign_1;
					  lowp vec3 worldTangent_2;
					  lowp vec3 worldNormal_3;
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
					  highp vec3 tmpvar_39;
					  tmpvar_39 = (_Object2World * vertex_22).xyz;
					  highp vec4 v_40;
					  v_40.x = tmpvar_5;
					  v_40.y = tmpvar_6;
					  v_40.z = tmpvar_7;
					  v_40.w = tmpvar_8;
					  highp vec4 v_41;
					  v_41.x = tmpvar_10;
					  v_41.y = tmpvar_11;
					  v_41.z = tmpvar_12;
					  v_41.w = tmpvar_13;
					  highp vec4 v_42;
					  v_42.x = tmpvar_15;
					  v_42.y = tmpvar_16;
					  v_42.z = tmpvar_17;
					  v_42.w = tmpvar_18;
					  highp vec3 tmpvar_43;
					  tmpvar_43 = normalize(((
					    (v_40.xyz * tmpvar_38.x)
					   + 
					    (v_41.xyz * tmpvar_38.y)
					  ) + (v_42.xyz * tmpvar_38.z)));
					  worldNormal_3 = tmpvar_43;
					  highp mat3 tmpvar_44;
					  tmpvar_44[0] = _Object2World[0].xyz;
					  tmpvar_44[1] = _Object2World[1].xyz;
					  tmpvar_44[2] = _Object2World[2].xyz;
					  highp vec3 tmpvar_45;
					  tmpvar_45 = normalize((tmpvar_44 * _glesTANGENT.xyz));
					  worldTangent_2 = tmpvar_45;
					  highp float tmpvar_46;
					  tmpvar_46 = (_glesTANGENT.w * unity_WorldTransformParams.w);
					  tangentSign_1 = tmpvar_46;
					  lowp vec3 tmpvar_47;
					  tmpvar_47 = (((worldNormal_3.yzx * worldTangent_2.zxy) - (worldNormal_3.zxy * worldTangent_2.yzx)) * tangentSign_1);
					  highp vec4 tmpvar_48;
					  tmpvar_48.x = worldTangent_2.x;
					  tmpvar_48.y = tmpvar_47.x;
					  tmpvar_48.z = worldNormal_3.x;
					  tmpvar_48.w = tmpvar_39.x;
					  highp vec4 tmpvar_49;
					  tmpvar_49.x = worldTangent_2.y;
					  tmpvar_49.y = tmpvar_47.y;
					  tmpvar_49.z = worldNormal_3.y;
					  tmpvar_49.w = tmpvar_39.y;
					  highp vec4 tmpvar_50;
					  tmpvar_50.x = worldTangent_2.z;
					  tmpvar_50.y = tmpvar_47.z;
					  tmpvar_50.z = worldNormal_3.z;
					  tmpvar_50.w = tmpvar_39.z;
					  mediump vec3 normal_51;
					  normal_51 = worldNormal_3;
					  mediump vec3 x1_52;
					  mediump vec4 tmpvar_53;
					  tmpvar_53 = (normal_51.xyzz * normal_51.yzzx);
					  x1_52.x = dot (unity_SHBr, tmpvar_53);
					  x1_52.y = dot (unity_SHBg, tmpvar_53);
					  x1_52.z = dot (unity_SHBb, tmpvar_53);
					  gl_Position = (glstate_matrix_mvp * vertex_22);
					  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
					  xlv_TEXCOORD1 = tmpvar_48;
					  xlv_TEXCOORD2 = tmpvar_49;
					  xlv_TEXCOORD3 = tmpvar_50;
					  xlv_COLOR0 = _glesColor;
					  xlv_TEXCOORD4 = (x1_52 + (unity_SHC.xyz * (
					    (normal_51.x * normal_51.x)
					   - 
					    (normal_51.y * normal_51.y)
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
					uniform sampler2D _V_CW_NormalMap;
					uniform mediump float _V_CW_NormalMap_UV_Scale;
					uniform mediump float _V_CW_NormalMapStrength;
					uniform sampler2D _EmissionMap;
					uniform mediump vec4 _EmissionColor;
					varying highp vec2 xlv_TEXCOORD0;
					varying highp vec4 xlv_TEXCOORD1;
					varying highp vec4 xlv_TEXCOORD2;
					varying highp vec4 xlv_TEXCOORD3;
					varying lowp vec4 xlv_COLOR0;
					varying mediump vec3 xlv_TEXCOORD4;
					void main ()
					{
					  mediump vec3 tmpvar_1;
					  mediump vec3 tmpvar_2;
					  lowp vec3 worldN_3;
					  lowp vec4 c_4;
					  lowp vec3 lightDir_5;
					  highp vec4 tmpvar_6;
					  mediump vec3 tmpvar_7;
					  tmpvar_7 = _WorldSpaceLightPos0.xyz;
					  lightDir_5 = tmpvar_7;
					  tmpvar_6 = xlv_COLOR0;
					  lowp vec3 tmpvar_8;
					  lowp vec3 tmpvar_9;
					  lowp vec3 tmpvar_10;
					  lowp vec4 tmpvar_11;
					  highp vec2 P_12;
					  highp vec2 tmpvar_13;
					  tmpvar_13 = (_V_CW_MainTex_Scroll * _Time.x);
					  P_12 = (xlv_TEXCOORD0 + tmpvar_13);
					  tmpvar_11 = texture2D (_MainTex, P_12);
					  tmpvar_8 = (tmpvar_11.xyz * _Color.xyz);
					  tmpvar_8 = (tmpvar_8 * tmpvar_6.xyz);
					  highp vec2 P_14;
					  P_14 = ((xlv_TEXCOORD0 * _V_CW_NormalMap_UV_Scale) + tmpvar_13);
					  lowp vec3 normal_15;
					  normal_15.xy = ((texture2D (_V_CW_NormalMap, P_14).wy * 2.0) - 1.0);
					  normal_15.z = sqrt((1.0 - clamp (
					    dot (normal_15.xy, normal_15.xy)
					  , 0.0, 1.0)));
					  mediump vec3 tmpvar_16;
					  tmpvar_16.xy = (normal_15.xy * vec2(_V_CW_NormalMapStrength));
					  tmpvar_16.z = normal_15.z;
					  mediump vec3 tmpvar_17;
					  tmpvar_17 = normalize(tmpvar_16);
					  tmpvar_9 = tmpvar_17;
					  tmpvar_10 = (texture2D (_EmissionMap, xlv_TEXCOORD0).xyz * _EmissionColor.xyz);
					  highp float tmpvar_18;
					  tmpvar_18 = dot (xlv_TEXCOORD1.xyz, tmpvar_9);
					  worldN_3.x = tmpvar_18;
					  highp float tmpvar_19;
					  tmpvar_19 = dot (xlv_TEXCOORD2.xyz, tmpvar_9);
					  worldN_3.y = tmpvar_19;
					  highp float tmpvar_20;
					  tmpvar_20 = dot (xlv_TEXCOORD3.xyz, tmpvar_9);
					  worldN_3.z = tmpvar_20;
					  tmpvar_1 = _LightColor0.xyz;
					  tmpvar_2 = lightDir_5;
					  mediump vec3 normalWorld_21;
					  normalWorld_21 = worldN_3;
					  mediump vec3 ambient_22;
					  mediump vec4 tmpvar_23;
					  tmpvar_23.w = 1.0;
					  tmpvar_23.xyz = normalWorld_21;
					  mediump vec3 x_24;
					  x_24.x = dot (unity_SHAr, tmpvar_23);
					  x_24.y = dot (unity_SHAg, tmpvar_23);
					  x_24.z = dot (unity_SHAb, tmpvar_23);
					  ambient_22 = max (((1.055 * 
					    pow (max (vec3(0.0, 0.0, 0.0), (xlv_TEXCOORD4 + x_24)), vec3(0.4166667, 0.4166667, 0.4166667))
					  ) - 0.055), vec3(0.0, 0.0, 0.0));
					  lowp vec4 c_25;
					  lowp vec4 c_26;
					  lowp float diff_27;
					  mediump float tmpvar_28;
					  tmpvar_28 = max (0.0, dot (worldN_3, tmpvar_2));
					  diff_27 = tmpvar_28;
					  c_26.xyz = ((tmpvar_8 * tmpvar_1) * diff_27);
					  c_26.w = (tmpvar_11.w * _Color.w);
					  c_25.w = c_26.w;
					  c_25.xyz = (c_26.xyz + (tmpvar_8 * ambient_22));
					  c_4.w = c_25.w;
					  c_4.xyz = (c_25.xyz + tmpvar_10);
					  gl_FragData[0] = c_4;
					}
					
					
					#endif"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "_EMISSION" "VERTEXLIGHT_ON" "V_CW_VERTEX_COLOR" "_NORMALMAP" "V_CW_REFLECTIVE_OFF" "V_CW_RIM_OFF" "V_CW_SPECULAR_OFF" }
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
					uniform highp vec4 unity_WorldTransformParams;
					uniform highp vec3 _V_CW_Bend;
					uniform highp vec3 _V_CW_Bias;
					uniform highp vec4 _V_CW_PivotPoint_Position;
					uniform highp vec4 _MainTex_ST;
					varying highp vec2 xlv_TEXCOORD0;
					varying highp vec4 xlv_TEXCOORD1;
					varying highp vec4 xlv_TEXCOORD2;
					varying highp vec4 xlv_TEXCOORD3;
					varying lowp vec4 xlv_COLOR0;
					varying mediump vec3 xlv_TEXCOORD4;
					void main ()
					{
					  lowp float tangentSign_1;
					  lowp vec3 worldTangent_2;
					  lowp vec3 worldNormal_3;
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
					  worldNormal_3 = tmpvar_44;
					  highp mat3 tmpvar_45;
					  tmpvar_45[0] = _Object2World[0].xyz;
					  tmpvar_45[1] = _Object2World[1].xyz;
					  tmpvar_45[2] = _Object2World[2].xyz;
					  highp vec3 tmpvar_46;
					  tmpvar_46 = normalize((tmpvar_45 * _glesTANGENT.xyz));
					  worldTangent_2 = tmpvar_46;
					  highp float tmpvar_47;
					  tmpvar_47 = (_glesTANGENT.w * unity_WorldTransformParams.w);
					  tangentSign_1 = tmpvar_47;
					  lowp vec3 tmpvar_48;
					  tmpvar_48 = (((worldNormal_3.yzx * worldTangent_2.zxy) - (worldNormal_3.zxy * worldTangent_2.yzx)) * tangentSign_1);
					  highp vec4 tmpvar_49;
					  tmpvar_49.x = worldTangent_2.x;
					  tmpvar_49.y = tmpvar_48.x;
					  tmpvar_49.z = worldNormal_3.x;
					  tmpvar_49.w = tmpvar_40.x;
					  highp vec4 tmpvar_50;
					  tmpvar_50.x = worldTangent_2.y;
					  tmpvar_50.y = tmpvar_48.y;
					  tmpvar_50.z = worldNormal_3.y;
					  tmpvar_50.w = tmpvar_40.y;
					  highp vec4 tmpvar_51;
					  tmpvar_51.x = worldTangent_2.z;
					  tmpvar_51.y = tmpvar_48.z;
					  tmpvar_51.z = worldNormal_3.z;
					  tmpvar_51.w = tmpvar_40.z;
					  highp vec3 lightColor0_52;
					  lightColor0_52 = unity_LightColor[0].xyz;
					  highp vec3 lightColor1_53;
					  lightColor1_53 = unity_LightColor[1].xyz;
					  highp vec3 lightColor2_54;
					  lightColor2_54 = unity_LightColor[2].xyz;
					  highp vec3 lightColor3_55;
					  lightColor3_55 = unity_LightColor[3].xyz;
					  highp vec4 lightAttenSq_56;
					  lightAttenSq_56 = unity_4LightAtten0;
					  highp vec3 normal_57;
					  normal_57 = worldNormal_3;
					  highp vec3 col_58;
					  highp vec4 ndotl_59;
					  highp vec4 lengthSq_60;
					  highp vec4 tmpvar_61;
					  tmpvar_61 = (unity_4LightPosX0 - tmpvar_40.x);
					  highp vec4 tmpvar_62;
					  tmpvar_62 = (unity_4LightPosY0 - tmpvar_40.y);
					  highp vec4 tmpvar_63;
					  tmpvar_63 = (unity_4LightPosZ0 - tmpvar_40.z);
					  lengthSq_60 = (tmpvar_61 * tmpvar_61);
					  lengthSq_60 = (lengthSq_60 + (tmpvar_62 * tmpvar_62));
					  lengthSq_60 = (lengthSq_60 + (tmpvar_63 * tmpvar_63));
					  ndotl_59 = (tmpvar_61 * normal_57.x);
					  ndotl_59 = (ndotl_59 + (tmpvar_62 * normal_57.y));
					  ndotl_59 = (ndotl_59 + (tmpvar_63 * normal_57.z));
					  highp vec4 tmpvar_64;
					  tmpvar_64 = max (vec4(0.0, 0.0, 0.0, 0.0), (ndotl_59 * inversesqrt(lengthSq_60)));
					  ndotl_59 = tmpvar_64;
					  highp vec4 tmpvar_65;
					  tmpvar_65 = (tmpvar_64 * (1.0/((1.0 + 
					    (lengthSq_60 * lightAttenSq_56)
					  ))));
					  col_58 = (lightColor0_52 * tmpvar_65.x);
					  col_58 = (col_58 + (lightColor1_53 * tmpvar_65.y));
					  col_58 = (col_58 + (lightColor2_54 * tmpvar_65.z));
					  col_58 = (col_58 + (lightColor3_55 * tmpvar_65.w));
					  tmpvar_4 = col_58;
					  mediump vec3 normal_66;
					  normal_66 = worldNormal_3;
					  mediump vec3 ambient_67;
					  ambient_67 = (tmpvar_4 * ((tmpvar_4 * 
					    ((tmpvar_4 * 0.305306) + 0.6821711)
					  ) + 0.01252288));
					  mediump vec3 x1_68;
					  mediump vec4 tmpvar_69;
					  tmpvar_69 = (normal_66.xyzz * normal_66.yzzx);
					  x1_68.x = dot (unity_SHBr, tmpvar_69);
					  x1_68.y = dot (unity_SHBg, tmpvar_69);
					  x1_68.z = dot (unity_SHBb, tmpvar_69);
					  ambient_67 = (ambient_67 + (x1_68 + (unity_SHC.xyz * 
					    ((normal_66.x * normal_66.x) - (normal_66.y * normal_66.y))
					  )));
					  tmpvar_4 = ambient_67;
					  gl_Position = (glstate_matrix_mvp * vertex_23);
					  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
					  xlv_TEXCOORD1 = tmpvar_49;
					  xlv_TEXCOORD2 = tmpvar_50;
					  xlv_TEXCOORD3 = tmpvar_51;
					  xlv_COLOR0 = _glesColor;
					  xlv_TEXCOORD4 = ambient_67;
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
					uniform sampler2D _V_CW_NormalMap;
					uniform mediump float _V_CW_NormalMap_UV_Scale;
					uniform mediump float _V_CW_NormalMapStrength;
					uniform sampler2D _EmissionMap;
					uniform mediump vec4 _EmissionColor;
					varying highp vec2 xlv_TEXCOORD0;
					varying highp vec4 xlv_TEXCOORD1;
					varying highp vec4 xlv_TEXCOORD2;
					varying highp vec4 xlv_TEXCOORD3;
					varying lowp vec4 xlv_COLOR0;
					varying mediump vec3 xlv_TEXCOORD4;
					void main ()
					{
					  mediump vec3 tmpvar_1;
					  mediump vec3 tmpvar_2;
					  lowp vec3 worldN_3;
					  lowp vec4 c_4;
					  lowp vec3 lightDir_5;
					  highp vec4 tmpvar_6;
					  mediump vec3 tmpvar_7;
					  tmpvar_7 = _WorldSpaceLightPos0.xyz;
					  lightDir_5 = tmpvar_7;
					  tmpvar_6 = xlv_COLOR0;
					  lowp vec3 tmpvar_8;
					  lowp vec3 tmpvar_9;
					  lowp vec3 tmpvar_10;
					  lowp vec4 tmpvar_11;
					  highp vec2 P_12;
					  highp vec2 tmpvar_13;
					  tmpvar_13 = (_V_CW_MainTex_Scroll * _Time.x);
					  P_12 = (xlv_TEXCOORD0 + tmpvar_13);
					  tmpvar_11 = texture2D (_MainTex, P_12);
					  tmpvar_8 = (tmpvar_11.xyz * _Color.xyz);
					  tmpvar_8 = (tmpvar_8 * tmpvar_6.xyz);
					  highp vec2 P_14;
					  P_14 = ((xlv_TEXCOORD0 * _V_CW_NormalMap_UV_Scale) + tmpvar_13);
					  lowp vec3 normal_15;
					  normal_15.xy = ((texture2D (_V_CW_NormalMap, P_14).wy * 2.0) - 1.0);
					  normal_15.z = sqrt((1.0 - clamp (
					    dot (normal_15.xy, normal_15.xy)
					  , 0.0, 1.0)));
					  mediump vec3 tmpvar_16;
					  tmpvar_16.xy = (normal_15.xy * vec2(_V_CW_NormalMapStrength));
					  tmpvar_16.z = normal_15.z;
					  mediump vec3 tmpvar_17;
					  tmpvar_17 = normalize(tmpvar_16);
					  tmpvar_9 = tmpvar_17;
					  tmpvar_10 = (texture2D (_EmissionMap, xlv_TEXCOORD0).xyz * _EmissionColor.xyz);
					  highp float tmpvar_18;
					  tmpvar_18 = dot (xlv_TEXCOORD1.xyz, tmpvar_9);
					  worldN_3.x = tmpvar_18;
					  highp float tmpvar_19;
					  tmpvar_19 = dot (xlv_TEXCOORD2.xyz, tmpvar_9);
					  worldN_3.y = tmpvar_19;
					  highp float tmpvar_20;
					  tmpvar_20 = dot (xlv_TEXCOORD3.xyz, tmpvar_9);
					  worldN_3.z = tmpvar_20;
					  tmpvar_1 = _LightColor0.xyz;
					  tmpvar_2 = lightDir_5;
					  mediump vec3 normalWorld_21;
					  normalWorld_21 = worldN_3;
					  mediump vec3 ambient_22;
					  mediump vec4 tmpvar_23;
					  tmpvar_23.w = 1.0;
					  tmpvar_23.xyz = normalWorld_21;
					  mediump vec3 x_24;
					  x_24.x = dot (unity_SHAr, tmpvar_23);
					  x_24.y = dot (unity_SHAg, tmpvar_23);
					  x_24.z = dot (unity_SHAb, tmpvar_23);
					  ambient_22 = max (((1.055 * 
					    pow (max (vec3(0.0, 0.0, 0.0), (xlv_TEXCOORD4 + x_24)), vec3(0.4166667, 0.4166667, 0.4166667))
					  ) - 0.055), vec3(0.0, 0.0, 0.0));
					  lowp vec4 c_25;
					  lowp vec4 c_26;
					  lowp float diff_27;
					  mediump float tmpvar_28;
					  tmpvar_28 = max (0.0, dot (worldN_3, tmpvar_2));
					  diff_27 = tmpvar_28;
					  c_26.xyz = ((tmpvar_8 * tmpvar_1) * diff_27);
					  c_26.w = (tmpvar_11.w * _Color.w);
					  c_25.w = c_26.w;
					  c_25.xyz = (c_26.xyz + (tmpvar_8 * ambient_22));
					  c_4.w = c_25.w;
					  c_4.xyz = (c_25.xyz + tmpvar_10);
					  gl_FragData[0] = c_4;
					}
					
					
					#endif"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "FOG_LINEAR" "_EMISSION" "V_CW_VERTEX_COLOR" "_NORMALMAP" "V_CW_REFLECTIVE_OFF" "V_CW_RIM_OFF" "V_CW_SPECULAR_OFF" }
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
					uniform highp vec4 unity_WorldTransformParams;
					uniform highp vec4 unity_FogParams;
					uniform highp vec3 _V_CW_Bend;
					uniform highp vec3 _V_CW_Bias;
					uniform highp vec4 _V_CW_PivotPoint_Position;
					uniform highp vec4 _MainTex_ST;
					varying highp vec2 xlv_TEXCOORD0;
					varying highp vec4 xlv_TEXCOORD1;
					varying highp vec4 xlv_TEXCOORD2;
					varying highp vec4 xlv_TEXCOORD3;
					varying lowp vec4 xlv_COLOR0;
					varying mediump vec3 xlv_TEXCOORD4;
					varying highp float xlv_TEXCOORD5;
					void main ()
					{
					  lowp float tangentSign_1;
					  lowp vec3 worldTangent_2;
					  lowp vec3 worldNormal_3;
					  highp vec4 tmpvar_4;
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
					  tmpvar_4 = (glstate_matrix_mvp * vertex_23);
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
					  worldNormal_3 = tmpvar_44;
					  highp mat3 tmpvar_45;
					  tmpvar_45[0] = _Object2World[0].xyz;
					  tmpvar_45[1] = _Object2World[1].xyz;
					  tmpvar_45[2] = _Object2World[2].xyz;
					  highp vec3 tmpvar_46;
					  tmpvar_46 = normalize((tmpvar_45 * _glesTANGENT.xyz));
					  worldTangent_2 = tmpvar_46;
					  highp float tmpvar_47;
					  tmpvar_47 = (_glesTANGENT.w * unity_WorldTransformParams.w);
					  tangentSign_1 = tmpvar_47;
					  lowp vec3 tmpvar_48;
					  tmpvar_48 = (((worldNormal_3.yzx * worldTangent_2.zxy) - (worldNormal_3.zxy * worldTangent_2.yzx)) * tangentSign_1);
					  highp vec4 tmpvar_49;
					  tmpvar_49.x = worldTangent_2.x;
					  tmpvar_49.y = tmpvar_48.x;
					  tmpvar_49.z = worldNormal_3.x;
					  tmpvar_49.w = tmpvar_40.x;
					  highp vec4 tmpvar_50;
					  tmpvar_50.x = worldTangent_2.y;
					  tmpvar_50.y = tmpvar_48.y;
					  tmpvar_50.z = worldNormal_3.y;
					  tmpvar_50.w = tmpvar_40.y;
					  highp vec4 tmpvar_51;
					  tmpvar_51.x = worldTangent_2.z;
					  tmpvar_51.y = tmpvar_48.z;
					  tmpvar_51.z = worldNormal_3.z;
					  tmpvar_51.w = tmpvar_40.z;
					  mediump vec3 normal_52;
					  normal_52 = worldNormal_3;
					  mediump vec3 x1_53;
					  mediump vec4 tmpvar_54;
					  tmpvar_54 = (normal_52.xyzz * normal_52.yzzx);
					  x1_53.x = dot (unity_SHBr, tmpvar_54);
					  x1_53.y = dot (unity_SHBg, tmpvar_54);
					  x1_53.z = dot (unity_SHBb, tmpvar_54);
					  gl_Position = tmpvar_4;
					  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
					  xlv_TEXCOORD1 = tmpvar_49;
					  xlv_TEXCOORD2 = tmpvar_50;
					  xlv_TEXCOORD3 = tmpvar_51;
					  xlv_COLOR0 = _glesColor;
					  xlv_TEXCOORD4 = (x1_53 + (unity_SHC.xyz * (
					    (normal_52.x * normal_52.x)
					   - 
					    (normal_52.y * normal_52.y)
					  )));
					  xlv_TEXCOORD5 = ((tmpvar_4.z * unity_FogParams.z) + unity_FogParams.w);
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
					uniform sampler2D _V_CW_NormalMap;
					uniform mediump float _V_CW_NormalMap_UV_Scale;
					uniform mediump float _V_CW_NormalMapStrength;
					uniform sampler2D _EmissionMap;
					uniform mediump vec4 _EmissionColor;
					varying highp vec2 xlv_TEXCOORD0;
					varying highp vec4 xlv_TEXCOORD1;
					varying highp vec4 xlv_TEXCOORD2;
					varying highp vec4 xlv_TEXCOORD3;
					varying lowp vec4 xlv_COLOR0;
					varying mediump vec3 xlv_TEXCOORD4;
					varying highp float xlv_TEXCOORD5;
					void main ()
					{
					  mediump vec3 tmpvar_1;
					  mediump vec3 tmpvar_2;
					  lowp vec3 worldN_3;
					  lowp vec4 c_4;
					  lowp vec3 lightDir_5;
					  highp vec4 tmpvar_6;
					  mediump vec3 tmpvar_7;
					  tmpvar_7 = _WorldSpaceLightPos0.xyz;
					  lightDir_5 = tmpvar_7;
					  tmpvar_6 = xlv_COLOR0;
					  lowp vec3 tmpvar_8;
					  lowp vec3 tmpvar_9;
					  lowp vec3 tmpvar_10;
					  lowp vec4 tmpvar_11;
					  highp vec2 P_12;
					  highp vec2 tmpvar_13;
					  tmpvar_13 = (_V_CW_MainTex_Scroll * _Time.x);
					  P_12 = (xlv_TEXCOORD0 + tmpvar_13);
					  tmpvar_11 = texture2D (_MainTex, P_12);
					  tmpvar_8 = (tmpvar_11.xyz * _Color.xyz);
					  tmpvar_8 = (tmpvar_8 * tmpvar_6.xyz);
					  highp vec2 P_14;
					  P_14 = ((xlv_TEXCOORD0 * _V_CW_NormalMap_UV_Scale) + tmpvar_13);
					  lowp vec3 normal_15;
					  normal_15.xy = ((texture2D (_V_CW_NormalMap, P_14).wy * 2.0) - 1.0);
					  normal_15.z = sqrt((1.0 - clamp (
					    dot (normal_15.xy, normal_15.xy)
					  , 0.0, 1.0)));
					  mediump vec3 tmpvar_16;
					  tmpvar_16.xy = (normal_15.xy * vec2(_V_CW_NormalMapStrength));
					  tmpvar_16.z = normal_15.z;
					  mediump vec3 tmpvar_17;
					  tmpvar_17 = normalize(tmpvar_16);
					  tmpvar_9 = tmpvar_17;
					  tmpvar_10 = (texture2D (_EmissionMap, xlv_TEXCOORD0).xyz * _EmissionColor.xyz);
					  highp float tmpvar_18;
					  tmpvar_18 = dot (xlv_TEXCOORD1.xyz, tmpvar_9);
					  worldN_3.x = tmpvar_18;
					  highp float tmpvar_19;
					  tmpvar_19 = dot (xlv_TEXCOORD2.xyz, tmpvar_9);
					  worldN_3.y = tmpvar_19;
					  highp float tmpvar_20;
					  tmpvar_20 = dot (xlv_TEXCOORD3.xyz, tmpvar_9);
					  worldN_3.z = tmpvar_20;
					  tmpvar_1 = _LightColor0.xyz;
					  tmpvar_2 = lightDir_5;
					  mediump vec3 normalWorld_21;
					  normalWorld_21 = worldN_3;
					  mediump vec3 ambient_22;
					  mediump vec4 tmpvar_23;
					  tmpvar_23.w = 1.0;
					  tmpvar_23.xyz = normalWorld_21;
					  mediump vec3 x_24;
					  x_24.x = dot (unity_SHAr, tmpvar_23);
					  x_24.y = dot (unity_SHAg, tmpvar_23);
					  x_24.z = dot (unity_SHAb, tmpvar_23);
					  ambient_22 = max (((1.055 * 
					    pow (max (vec3(0.0, 0.0, 0.0), (xlv_TEXCOORD4 + x_24)), vec3(0.4166667, 0.4166667, 0.4166667))
					  ) - 0.055), vec3(0.0, 0.0, 0.0));
					  lowp vec4 c_25;
					  lowp vec4 c_26;
					  lowp float diff_27;
					  mediump float tmpvar_28;
					  tmpvar_28 = max (0.0, dot (worldN_3, tmpvar_2));
					  diff_27 = tmpvar_28;
					  c_26.xyz = ((tmpvar_8 * tmpvar_1) * diff_27);
					  c_26.w = (tmpvar_11.w * _Color.w);
					  c_25.w = c_26.w;
					  c_25.xyz = (c_26.xyz + (tmpvar_8 * ambient_22));
					  c_4.w = c_25.w;
					  c_4.xyz = (c_25.xyz + tmpvar_10);
					  highp float tmpvar_29;
					  tmpvar_29 = clamp (xlv_TEXCOORD5, 0.0, 1.0);
					  c_4.xyz = mix (unity_FogColor.xyz, c_4.xyz, vec3(tmpvar_29));
					  gl_FragData[0] = c_4;
					}
					
					
					#endif"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "FOG_LINEAR" "_EMISSION" "VERTEXLIGHT_ON" "V_CW_VERTEX_COLOR" "_NORMALMAP" "V_CW_REFLECTIVE_OFF" "V_CW_RIM_OFF" "V_CW_SPECULAR_OFF" }
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
					uniform highp vec4 unity_WorldTransformParams;
					uniform highp vec4 unity_FogParams;
					uniform highp vec3 _V_CW_Bend;
					uniform highp vec3 _V_CW_Bias;
					uniform highp vec4 _V_CW_PivotPoint_Position;
					uniform highp vec4 _MainTex_ST;
					varying highp vec2 xlv_TEXCOORD0;
					varying highp vec4 xlv_TEXCOORD1;
					varying highp vec4 xlv_TEXCOORD2;
					varying highp vec4 xlv_TEXCOORD3;
					varying lowp vec4 xlv_COLOR0;
					varying mediump vec3 xlv_TEXCOORD4;
					varying highp float xlv_TEXCOORD5;
					void main ()
					{
					  lowp float tangentSign_1;
					  lowp vec3 worldTangent_2;
					  lowp vec3 worldNormal_3;
					  highp vec4 tmpvar_4;
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
					  tmpvar_4 = (glstate_matrix_mvp * vertex_24);
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
					  worldNormal_3 = tmpvar_45;
					  highp mat3 tmpvar_46;
					  tmpvar_46[0] = _Object2World[0].xyz;
					  tmpvar_46[1] = _Object2World[1].xyz;
					  tmpvar_46[2] = _Object2World[2].xyz;
					  highp vec3 tmpvar_47;
					  tmpvar_47 = normalize((tmpvar_46 * _glesTANGENT.xyz));
					  worldTangent_2 = tmpvar_47;
					  highp float tmpvar_48;
					  tmpvar_48 = (_glesTANGENT.w * unity_WorldTransformParams.w);
					  tangentSign_1 = tmpvar_48;
					  lowp vec3 tmpvar_49;
					  tmpvar_49 = (((worldNormal_3.yzx * worldTangent_2.zxy) - (worldNormal_3.zxy * worldTangent_2.yzx)) * tangentSign_1);
					  highp vec4 tmpvar_50;
					  tmpvar_50.x = worldTangent_2.x;
					  tmpvar_50.y = tmpvar_49.x;
					  tmpvar_50.z = worldNormal_3.x;
					  tmpvar_50.w = tmpvar_41.x;
					  highp vec4 tmpvar_51;
					  tmpvar_51.x = worldTangent_2.y;
					  tmpvar_51.y = tmpvar_49.y;
					  tmpvar_51.z = worldNormal_3.y;
					  tmpvar_51.w = tmpvar_41.y;
					  highp vec4 tmpvar_52;
					  tmpvar_52.x = worldTangent_2.z;
					  tmpvar_52.y = tmpvar_49.z;
					  tmpvar_52.z = worldNormal_3.z;
					  tmpvar_52.w = tmpvar_41.z;
					  highp vec3 lightColor0_53;
					  lightColor0_53 = unity_LightColor[0].xyz;
					  highp vec3 lightColor1_54;
					  lightColor1_54 = unity_LightColor[1].xyz;
					  highp vec3 lightColor2_55;
					  lightColor2_55 = unity_LightColor[2].xyz;
					  highp vec3 lightColor3_56;
					  lightColor3_56 = unity_LightColor[3].xyz;
					  highp vec4 lightAttenSq_57;
					  lightAttenSq_57 = unity_4LightAtten0;
					  highp vec3 normal_58;
					  normal_58 = worldNormal_3;
					  highp vec3 col_59;
					  highp vec4 ndotl_60;
					  highp vec4 lengthSq_61;
					  highp vec4 tmpvar_62;
					  tmpvar_62 = (unity_4LightPosX0 - tmpvar_41.x);
					  highp vec4 tmpvar_63;
					  tmpvar_63 = (unity_4LightPosY0 - tmpvar_41.y);
					  highp vec4 tmpvar_64;
					  tmpvar_64 = (unity_4LightPosZ0 - tmpvar_41.z);
					  lengthSq_61 = (tmpvar_62 * tmpvar_62);
					  lengthSq_61 = (lengthSq_61 + (tmpvar_63 * tmpvar_63));
					  lengthSq_61 = (lengthSq_61 + (tmpvar_64 * tmpvar_64));
					  ndotl_60 = (tmpvar_62 * normal_58.x);
					  ndotl_60 = (ndotl_60 + (tmpvar_63 * normal_58.y));
					  ndotl_60 = (ndotl_60 + (tmpvar_64 * normal_58.z));
					  highp vec4 tmpvar_65;
					  tmpvar_65 = max (vec4(0.0, 0.0, 0.0, 0.0), (ndotl_60 * inversesqrt(lengthSq_61)));
					  ndotl_60 = tmpvar_65;
					  highp vec4 tmpvar_66;
					  tmpvar_66 = (tmpvar_65 * (1.0/((1.0 + 
					    (lengthSq_61 * lightAttenSq_57)
					  ))));
					  col_59 = (lightColor0_53 * tmpvar_66.x);
					  col_59 = (col_59 + (lightColor1_54 * tmpvar_66.y));
					  col_59 = (col_59 + (lightColor2_55 * tmpvar_66.z));
					  col_59 = (col_59 + (lightColor3_56 * tmpvar_66.w));
					  tmpvar_5 = col_59;
					  mediump vec3 normal_67;
					  normal_67 = worldNormal_3;
					  mediump vec3 ambient_68;
					  ambient_68 = (tmpvar_5 * ((tmpvar_5 * 
					    ((tmpvar_5 * 0.305306) + 0.6821711)
					  ) + 0.01252288));
					  mediump vec3 x1_69;
					  mediump vec4 tmpvar_70;
					  tmpvar_70 = (normal_67.xyzz * normal_67.yzzx);
					  x1_69.x = dot (unity_SHBr, tmpvar_70);
					  x1_69.y = dot (unity_SHBg, tmpvar_70);
					  x1_69.z = dot (unity_SHBb, tmpvar_70);
					  ambient_68 = (ambient_68 + (x1_69 + (unity_SHC.xyz * 
					    ((normal_67.x * normal_67.x) - (normal_67.y * normal_67.y))
					  )));
					  tmpvar_5 = ambient_68;
					  gl_Position = tmpvar_4;
					  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
					  xlv_TEXCOORD1 = tmpvar_50;
					  xlv_TEXCOORD2 = tmpvar_51;
					  xlv_TEXCOORD3 = tmpvar_52;
					  xlv_COLOR0 = _glesColor;
					  xlv_TEXCOORD4 = ambient_68;
					  xlv_TEXCOORD5 = ((tmpvar_4.z * unity_FogParams.z) + unity_FogParams.w);
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
					uniform sampler2D _V_CW_NormalMap;
					uniform mediump float _V_CW_NormalMap_UV_Scale;
					uniform mediump float _V_CW_NormalMapStrength;
					uniform sampler2D _EmissionMap;
					uniform mediump vec4 _EmissionColor;
					varying highp vec2 xlv_TEXCOORD0;
					varying highp vec4 xlv_TEXCOORD1;
					varying highp vec4 xlv_TEXCOORD2;
					varying highp vec4 xlv_TEXCOORD3;
					varying lowp vec4 xlv_COLOR0;
					varying mediump vec3 xlv_TEXCOORD4;
					varying highp float xlv_TEXCOORD5;
					void main ()
					{
					  mediump vec3 tmpvar_1;
					  mediump vec3 tmpvar_2;
					  lowp vec3 worldN_3;
					  lowp vec4 c_4;
					  lowp vec3 lightDir_5;
					  highp vec4 tmpvar_6;
					  mediump vec3 tmpvar_7;
					  tmpvar_7 = _WorldSpaceLightPos0.xyz;
					  lightDir_5 = tmpvar_7;
					  tmpvar_6 = xlv_COLOR0;
					  lowp vec3 tmpvar_8;
					  lowp vec3 tmpvar_9;
					  lowp vec3 tmpvar_10;
					  lowp vec4 tmpvar_11;
					  highp vec2 P_12;
					  highp vec2 tmpvar_13;
					  tmpvar_13 = (_V_CW_MainTex_Scroll * _Time.x);
					  P_12 = (xlv_TEXCOORD0 + tmpvar_13);
					  tmpvar_11 = texture2D (_MainTex, P_12);
					  tmpvar_8 = (tmpvar_11.xyz * _Color.xyz);
					  tmpvar_8 = (tmpvar_8 * tmpvar_6.xyz);
					  highp vec2 P_14;
					  P_14 = ((xlv_TEXCOORD0 * _V_CW_NormalMap_UV_Scale) + tmpvar_13);
					  lowp vec3 normal_15;
					  normal_15.xy = ((texture2D (_V_CW_NormalMap, P_14).wy * 2.0) - 1.0);
					  normal_15.z = sqrt((1.0 - clamp (
					    dot (normal_15.xy, normal_15.xy)
					  , 0.0, 1.0)));
					  mediump vec3 tmpvar_16;
					  tmpvar_16.xy = (normal_15.xy * vec2(_V_CW_NormalMapStrength));
					  tmpvar_16.z = normal_15.z;
					  mediump vec3 tmpvar_17;
					  tmpvar_17 = normalize(tmpvar_16);
					  tmpvar_9 = tmpvar_17;
					  tmpvar_10 = (texture2D (_EmissionMap, xlv_TEXCOORD0).xyz * _EmissionColor.xyz);
					  highp float tmpvar_18;
					  tmpvar_18 = dot (xlv_TEXCOORD1.xyz, tmpvar_9);
					  worldN_3.x = tmpvar_18;
					  highp float tmpvar_19;
					  tmpvar_19 = dot (xlv_TEXCOORD2.xyz, tmpvar_9);
					  worldN_3.y = tmpvar_19;
					  highp float tmpvar_20;
					  tmpvar_20 = dot (xlv_TEXCOORD3.xyz, tmpvar_9);
					  worldN_3.z = tmpvar_20;
					  tmpvar_1 = _LightColor0.xyz;
					  tmpvar_2 = lightDir_5;
					  mediump vec3 normalWorld_21;
					  normalWorld_21 = worldN_3;
					  mediump vec3 ambient_22;
					  mediump vec4 tmpvar_23;
					  tmpvar_23.w = 1.0;
					  tmpvar_23.xyz = normalWorld_21;
					  mediump vec3 x_24;
					  x_24.x = dot (unity_SHAr, tmpvar_23);
					  x_24.y = dot (unity_SHAg, tmpvar_23);
					  x_24.z = dot (unity_SHAb, tmpvar_23);
					  ambient_22 = max (((1.055 * 
					    pow (max (vec3(0.0, 0.0, 0.0), (xlv_TEXCOORD4 + x_24)), vec3(0.4166667, 0.4166667, 0.4166667))
					  ) - 0.055), vec3(0.0, 0.0, 0.0));
					  lowp vec4 c_25;
					  lowp vec4 c_26;
					  lowp float diff_27;
					  mediump float tmpvar_28;
					  tmpvar_28 = max (0.0, dot (worldN_3, tmpvar_2));
					  diff_27 = tmpvar_28;
					  c_26.xyz = ((tmpvar_8 * tmpvar_1) * diff_27);
					  c_26.w = (tmpvar_11.w * _Color.w);
					  c_25.w = c_26.w;
					  c_25.xyz = (c_26.xyz + (tmpvar_8 * ambient_22));
					  c_4.w = c_25.w;
					  c_4.xyz = (c_25.xyz + tmpvar_10);
					  highp float tmpvar_29;
					  tmpvar_29 = clamp (xlv_TEXCOORD5, 0.0, 1.0);
					  c_4.xyz = mix (unity_FogColor.xyz, c_4.xyz, vec3(tmpvar_29));
					  gl_FragData[0] = c_4;
					}
					
					
					#endif"
}
}
Program "fp" {
SubProgram "gles " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "V_CW_VERTEX_COLOR" "V_CW_REFLECTIVE_OFF" "_EMISSION_OFF" "V_CW_RIM_OFF" "_NORMALMAP_OFF" "V_CW_SPECULAR_OFF" }
					"!!GLES"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "FOG_LINEAR" "V_CW_VERTEX_COLOR" "V_CW_REFLECTIVE_OFF" "_EMISSION_OFF" "V_CW_RIM_OFF" "_NORMALMAP_OFF" "V_CW_SPECULAR_OFF" }
					"!!GLES"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "V_CW_SPECULAR" "V_CW_VERTEX_COLOR" "V_CW_REFLECTIVE_OFF" "_EMISSION_OFF" "V_CW_RIM_OFF" "_NORMALMAP_OFF" }
					"!!GLES"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "FOG_LINEAR" "V_CW_SPECULAR" "V_CW_VERTEX_COLOR" "V_CW_REFLECTIVE_OFF" "_EMISSION_OFF" "V_CW_RIM_OFF" "_NORMALMAP_OFF" }
					"!!GLES"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "_EMISSION" "V_CW_VERTEX_COLOR" "_NORMALMAP" "V_CW_REFLECTIVE_OFF" "V_CW_RIM_OFF" "V_CW_SPECULAR_OFF" }
					"!!GLES"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "FOG_LINEAR" "_EMISSION" "V_CW_VERTEX_COLOR" "_NORMALMAP" "V_CW_REFLECTIVE_OFF" "V_CW_RIM_OFF" "V_CW_SPECULAR_OFF" }
					"!!GLES"
}
}
 }
 Pass {
  Name "FORWARD"
  Tags { "LIGHTMODE"="ForwardAdd" "QUEUE"="Transparent" "IGNOREPROJECTOR"="true" "RenderType"="Transparent" "CurvedWorldTag"="Legacy Shader/Transparent/Diffuse" "CurvedWorldNoneRemoveableKeywords"="" "CurvedWorldAvailableOptions"="V_CW_REFLECTIVE;V_CW_VERTEX_COLOR;_EMISSION;V_CW_RIM;_NORMALMAP;V_CW_SPECULAR_HD;" }
  ZWrite Off
  Blend SrcAlpha One
  ColorMask RGB
  GpuProgramID 92889
Program "vp" {
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
					varying highp vec2 xlv_TEXCOORD0;
					varying mediump vec3 xlv_TEXCOORD1;
					varying highp vec3 xlv_TEXCOORD2;
					varying lowp vec4 xlv_COLOR0;
					void main ()
					{
					  lowp vec3 worldNormal_1;
					  mediump vec3 tmpvar_2;
					  highp vec4 v_3;
					  highp float tmpvar_4;
					  tmpvar_4 = _World2Object[0].x;
					  v_3.x = tmpvar_4;
					  highp float tmpvar_5;
					  tmpvar_5 = _World2Object[1].x;
					  v_3.y = tmpvar_5;
					  highp float tmpvar_6;
					  tmpvar_6 = _World2Object[2].x;
					  v_3.z = tmpvar_6;
					  highp float tmpvar_7;
					  tmpvar_7 = _World2Object[3].x;
					  v_3.w = tmpvar_7;
					  highp vec4 v_8;
					  highp float tmpvar_9;
					  tmpvar_9 = _World2Object[0].y;
					  v_8.x = tmpvar_9;
					  highp float tmpvar_10;
					  tmpvar_10 = _World2Object[1].y;
					  v_8.y = tmpvar_10;
					  highp float tmpvar_11;
					  tmpvar_11 = _World2Object[2].y;
					  v_8.z = tmpvar_11;
					  highp float tmpvar_12;
					  tmpvar_12 = _World2Object[3].y;
					  v_8.w = tmpvar_12;
					  highp vec4 v_13;
					  highp float tmpvar_14;
					  tmpvar_14 = _World2Object[0].z;
					  v_13.x = tmpvar_14;
					  highp float tmpvar_15;
					  tmpvar_15 = _World2Object[1].z;
					  v_13.y = tmpvar_15;
					  highp float tmpvar_16;
					  tmpvar_16 = _World2Object[2].z;
					  v_13.z = tmpvar_16;
					  highp float tmpvar_17;
					  tmpvar_17 = _World2Object[3].z;
					  v_13.w = tmpvar_17;
					  highp vec3 tmpvar_18;
					  tmpvar_18 = normalize(((
					    (v_3.xyz * _glesNormal.x)
					   + 
					    (v_8.xyz * _glesNormal.y)
					  ) + (v_13.xyz * _glesNormal.z)));
					  highp mat3 tmpvar_19;
					  tmpvar_19[0] = _Object2World[0].xyz;
					  tmpvar_19[1] = _Object2World[1].xyz;
					  tmpvar_19[2] = _Object2World[2].xyz;
					  highp vec3 tmpvar_20;
					  tmpvar_20 = normalize((tmpvar_19 * _glesTANGENT.xyz));
					  highp vec4 vertex_21;
					  vertex_21.w = _glesVertex.w;
					  highp vec2 xzOff_22;
					  highp vec3 v2_23;
					  highp vec3 v1_24;
					  highp vec3 v0_25;
					  highp vec3 tmpvar_26;
					  tmpvar_26 = ((_Object2World * _glesVertex).xyz - _V_CW_PivotPoint_Position.xyz);
					  highp vec3 tmpvar_27;
					  tmpvar_27 = (tmpvar_26 + tmpvar_20);
					  v1_24.xz = tmpvar_27.xz;
					  highp vec3 tmpvar_28;
					  tmpvar_28 = (tmpvar_26 - ((tmpvar_18.yzx * tmpvar_20.zxy) - (tmpvar_18.zxy * tmpvar_20.yzx)));
					  v2_23.xz = tmpvar_28.xz;
					  highp vec2 tmpvar_29;
					  tmpvar_29.x = float((tmpvar_26.z >= 0.0));
					  tmpvar_29.y = float((tmpvar_26.x >= 0.0));
					  xzOff_22 = (max (vec2(0.0, 0.0), (
					    abs(tmpvar_26.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_29 * 2.0) - 1.0));
					  xzOff_22 = (xzOff_22 * xzOff_22);
					  highp vec3 tmpvar_30;
					  tmpvar_30.xz = vec2(0.0, 0.0);
					  tmpvar_30.y = (((_V_CW_Bend.x * xzOff_22.x) + (_V_CW_Bend.z * xzOff_22.y)) * 0.001);
					  v0_25 = (tmpvar_26 + tmpvar_30);
					  highp vec2 tmpvar_31;
					  tmpvar_31.x = float((tmpvar_27.z >= 0.0));
					  tmpvar_31.y = float((tmpvar_27.x >= 0.0));
					  xzOff_22 = (max (vec2(0.0, 0.0), (
					    abs(tmpvar_27.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_31 * 2.0) - 1.0));
					  xzOff_22 = (xzOff_22 * xzOff_22);
					  v1_24.y = (tmpvar_27.y + ((
					    (_V_CW_Bend.x * xzOff_22.x)
					   + 
					    (_V_CW_Bend.z * xzOff_22.y)
					  ) * 0.001));
					  highp vec2 tmpvar_32;
					  tmpvar_32.x = float((tmpvar_28.z >= 0.0));
					  tmpvar_32.y = float((tmpvar_28.x >= 0.0));
					  xzOff_22 = (max (vec2(0.0, 0.0), (
					    abs(tmpvar_28.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_32 * 2.0) - 1.0));
					  xzOff_22 = (xzOff_22 * xzOff_22);
					  v2_23.y = (tmpvar_28.y + ((
					    (_V_CW_Bend.x * xzOff_22.x)
					   + 
					    (_V_CW_Bend.z * xzOff_22.y)
					  ) * 0.001));
					  highp mat3 tmpvar_33;
					  tmpvar_33[0] = _World2Object[0].xyz;
					  tmpvar_33[1] = _World2Object[1].xyz;
					  tmpvar_33[2] = _World2Object[2].xyz;
					  vertex_21.xyz = (_glesVertex.xyz + (tmpvar_33 * tmpvar_30));
					  highp mat3 tmpvar_34;
					  tmpvar_34[0] = _World2Object[0].xyz;
					  tmpvar_34[1] = _World2Object[1].xyz;
					  tmpvar_34[2] = _World2Object[2].xyz;
					  highp vec3 a_35;
					  a_35 = (v2_23 - v0_25);
					  highp vec3 b_36;
					  b_36 = (v1_24 - v0_25);
					  highp vec3 tmpvar_37;
					  tmpvar_37 = normalize((tmpvar_34 * normalize(
					    ((a_35.yzx * b_36.zxy) - (a_35.zxy * b_36.yzx))
					  )));
					  highp vec4 v_38;
					  v_38.x = tmpvar_4;
					  v_38.y = tmpvar_5;
					  v_38.z = tmpvar_6;
					  v_38.w = tmpvar_7;
					  highp vec4 v_39;
					  v_39.x = tmpvar_9;
					  v_39.y = tmpvar_10;
					  v_39.z = tmpvar_11;
					  v_39.w = tmpvar_12;
					  highp vec4 v_40;
					  v_40.x = tmpvar_14;
					  v_40.y = tmpvar_15;
					  v_40.z = tmpvar_16;
					  v_40.w = tmpvar_17;
					  highp vec3 tmpvar_41;
					  tmpvar_41 = normalize(((
					    (v_38.xyz * tmpvar_37.x)
					   + 
					    (v_39.xyz * tmpvar_37.y)
					  ) + (v_40.xyz * tmpvar_37.z)));
					  worldNormal_1 = tmpvar_41;
					  tmpvar_2 = worldNormal_1;
					  gl_Position = (glstate_matrix_mvp * vertex_21);
					  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
					  xlv_TEXCOORD1 = tmpvar_2;
					  xlv_TEXCOORD2 = (_Object2World * vertex_21).xyz;
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
					varying highp vec2 xlv_TEXCOORD0;
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
					  lowp vec4 tmpvar_8;
					  highp vec2 P_9;
					  P_9 = (xlv_TEXCOORD0 + (_V_CW_MainTex_Scroll * _Time.x));
					  tmpvar_8 = texture2D (_MainTex, P_9);
					  tmpvar_7 = (tmpvar_8.xyz * _Color.xyz);
					  tmpvar_7 = (tmpvar_7 * tmpvar_5.xyz);
					  highp vec4 tmpvar_10;
					  tmpvar_10.w = 1.0;
					  tmpvar_10.xyz = xlv_TEXCOORD2;
					  highp vec3 tmpvar_11;
					  tmpvar_11 = (_LightMatrix0 * tmpvar_10).xyz;
					  highp float tmpvar_12;
					  tmpvar_12 = dot (tmpvar_11, tmpvar_11);
					  lowp float tmpvar_13;
					  tmpvar_13 = texture2D (_LightTexture0, vec2(tmpvar_12)).w;
					  tmpvar_1 = _LightColor0.xyz;
					  tmpvar_2 = lightDir_4;
					  tmpvar_1 = (tmpvar_1 * tmpvar_13);
					  lowp vec4 c_14;
					  lowp vec4 c_15;
					  lowp float diff_16;
					  mediump float tmpvar_17;
					  tmpvar_17 = max (0.0, dot (tmpvar_3, tmpvar_2));
					  diff_16 = tmpvar_17;
					  c_15.xyz = ((tmpvar_7 * tmpvar_1) * diff_16);
					  c_15.w = (tmpvar_8.w * _Color.w);
					  c_14.w = c_15.w;
					  c_14.xyz = c_15.xyz;
					  gl_FragData[0] = c_14;
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
					varying highp vec2 xlv_TEXCOORD0;
					varying mediump vec3 xlv_TEXCOORD1;
					varying highp vec3 xlv_TEXCOORD2;
					varying lowp vec4 xlv_COLOR0;
					void main ()
					{
					  lowp vec3 worldNormal_1;
					  mediump vec3 tmpvar_2;
					  highp vec4 v_3;
					  highp float tmpvar_4;
					  tmpvar_4 = _World2Object[0].x;
					  v_3.x = tmpvar_4;
					  highp float tmpvar_5;
					  tmpvar_5 = _World2Object[1].x;
					  v_3.y = tmpvar_5;
					  highp float tmpvar_6;
					  tmpvar_6 = _World2Object[2].x;
					  v_3.z = tmpvar_6;
					  highp float tmpvar_7;
					  tmpvar_7 = _World2Object[3].x;
					  v_3.w = tmpvar_7;
					  highp vec4 v_8;
					  highp float tmpvar_9;
					  tmpvar_9 = _World2Object[0].y;
					  v_8.x = tmpvar_9;
					  highp float tmpvar_10;
					  tmpvar_10 = _World2Object[1].y;
					  v_8.y = tmpvar_10;
					  highp float tmpvar_11;
					  tmpvar_11 = _World2Object[2].y;
					  v_8.z = tmpvar_11;
					  highp float tmpvar_12;
					  tmpvar_12 = _World2Object[3].y;
					  v_8.w = tmpvar_12;
					  highp vec4 v_13;
					  highp float tmpvar_14;
					  tmpvar_14 = _World2Object[0].z;
					  v_13.x = tmpvar_14;
					  highp float tmpvar_15;
					  tmpvar_15 = _World2Object[1].z;
					  v_13.y = tmpvar_15;
					  highp float tmpvar_16;
					  tmpvar_16 = _World2Object[2].z;
					  v_13.z = tmpvar_16;
					  highp float tmpvar_17;
					  tmpvar_17 = _World2Object[3].z;
					  v_13.w = tmpvar_17;
					  highp vec3 tmpvar_18;
					  tmpvar_18 = normalize(((
					    (v_3.xyz * _glesNormal.x)
					   + 
					    (v_8.xyz * _glesNormal.y)
					  ) + (v_13.xyz * _glesNormal.z)));
					  highp mat3 tmpvar_19;
					  tmpvar_19[0] = _Object2World[0].xyz;
					  tmpvar_19[1] = _Object2World[1].xyz;
					  tmpvar_19[2] = _Object2World[2].xyz;
					  highp vec3 tmpvar_20;
					  tmpvar_20 = normalize((tmpvar_19 * _glesTANGENT.xyz));
					  highp vec4 vertex_21;
					  vertex_21.w = _glesVertex.w;
					  highp vec2 xzOff_22;
					  highp vec3 v2_23;
					  highp vec3 v1_24;
					  highp vec3 v0_25;
					  highp vec3 tmpvar_26;
					  tmpvar_26 = ((_Object2World * _glesVertex).xyz - _V_CW_PivotPoint_Position.xyz);
					  highp vec3 tmpvar_27;
					  tmpvar_27 = (tmpvar_26 + tmpvar_20);
					  v1_24.xz = tmpvar_27.xz;
					  highp vec3 tmpvar_28;
					  tmpvar_28 = (tmpvar_26 - ((tmpvar_18.yzx * tmpvar_20.zxy) - (tmpvar_18.zxy * tmpvar_20.yzx)));
					  v2_23.xz = tmpvar_28.xz;
					  highp vec2 tmpvar_29;
					  tmpvar_29.x = float((tmpvar_26.z >= 0.0));
					  tmpvar_29.y = float((tmpvar_26.x >= 0.0));
					  xzOff_22 = (max (vec2(0.0, 0.0), (
					    abs(tmpvar_26.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_29 * 2.0) - 1.0));
					  xzOff_22 = (xzOff_22 * xzOff_22);
					  highp vec3 tmpvar_30;
					  tmpvar_30.xz = vec2(0.0, 0.0);
					  tmpvar_30.y = (((_V_CW_Bend.x * xzOff_22.x) + (_V_CW_Bend.z * xzOff_22.y)) * 0.001);
					  v0_25 = (tmpvar_26 + tmpvar_30);
					  highp vec2 tmpvar_31;
					  tmpvar_31.x = float((tmpvar_27.z >= 0.0));
					  tmpvar_31.y = float((tmpvar_27.x >= 0.0));
					  xzOff_22 = (max (vec2(0.0, 0.0), (
					    abs(tmpvar_27.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_31 * 2.0) - 1.0));
					  xzOff_22 = (xzOff_22 * xzOff_22);
					  v1_24.y = (tmpvar_27.y + ((
					    (_V_CW_Bend.x * xzOff_22.x)
					   + 
					    (_V_CW_Bend.z * xzOff_22.y)
					  ) * 0.001));
					  highp vec2 tmpvar_32;
					  tmpvar_32.x = float((tmpvar_28.z >= 0.0));
					  tmpvar_32.y = float((tmpvar_28.x >= 0.0));
					  xzOff_22 = (max (vec2(0.0, 0.0), (
					    abs(tmpvar_28.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_32 * 2.0) - 1.0));
					  xzOff_22 = (xzOff_22 * xzOff_22);
					  v2_23.y = (tmpvar_28.y + ((
					    (_V_CW_Bend.x * xzOff_22.x)
					   + 
					    (_V_CW_Bend.z * xzOff_22.y)
					  ) * 0.001));
					  highp mat3 tmpvar_33;
					  tmpvar_33[0] = _World2Object[0].xyz;
					  tmpvar_33[1] = _World2Object[1].xyz;
					  tmpvar_33[2] = _World2Object[2].xyz;
					  vertex_21.xyz = (_glesVertex.xyz + (tmpvar_33 * tmpvar_30));
					  highp mat3 tmpvar_34;
					  tmpvar_34[0] = _World2Object[0].xyz;
					  tmpvar_34[1] = _World2Object[1].xyz;
					  tmpvar_34[2] = _World2Object[2].xyz;
					  highp vec3 a_35;
					  a_35 = (v2_23 - v0_25);
					  highp vec3 b_36;
					  b_36 = (v1_24 - v0_25);
					  highp vec3 tmpvar_37;
					  tmpvar_37 = normalize((tmpvar_34 * normalize(
					    ((a_35.yzx * b_36.zxy) - (a_35.zxy * b_36.yzx))
					  )));
					  highp vec4 v_38;
					  v_38.x = tmpvar_4;
					  v_38.y = tmpvar_5;
					  v_38.z = tmpvar_6;
					  v_38.w = tmpvar_7;
					  highp vec4 v_39;
					  v_39.x = tmpvar_9;
					  v_39.y = tmpvar_10;
					  v_39.z = tmpvar_11;
					  v_39.w = tmpvar_12;
					  highp vec4 v_40;
					  v_40.x = tmpvar_14;
					  v_40.y = tmpvar_15;
					  v_40.z = tmpvar_16;
					  v_40.w = tmpvar_17;
					  highp vec3 tmpvar_41;
					  tmpvar_41 = normalize(((
					    (v_38.xyz * tmpvar_37.x)
					   + 
					    (v_39.xyz * tmpvar_37.y)
					  ) + (v_40.xyz * tmpvar_37.z)));
					  worldNormal_1 = tmpvar_41;
					  tmpvar_2 = worldNormal_1;
					  gl_Position = (glstate_matrix_mvp * vertex_21);
					  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
					  xlv_TEXCOORD1 = tmpvar_2;
					  xlv_TEXCOORD2 = (_Object2World * vertex_21).xyz;
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
					varying highp vec2 xlv_TEXCOORD0;
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
					  lowp vec4 tmpvar_8;
					  highp vec2 P_9;
					  P_9 = (xlv_TEXCOORD0 + (_V_CW_MainTex_Scroll * _Time.x));
					  tmpvar_8 = texture2D (_MainTex, P_9);
					  tmpvar_7 = (tmpvar_8.xyz * _Color.xyz);
					  tmpvar_7 = (tmpvar_7 * tmpvar_5.xyz);
					  tmpvar_1 = _LightColor0.xyz;
					  tmpvar_2 = lightDir_4;
					  lowp vec4 c_10;
					  lowp vec4 c_11;
					  lowp float diff_12;
					  mediump float tmpvar_13;
					  tmpvar_13 = max (0.0, dot (tmpvar_3, tmpvar_2));
					  diff_12 = tmpvar_13;
					  c_11.xyz = ((tmpvar_7 * tmpvar_1) * diff_12);
					  c_11.w = (tmpvar_8.w * _Color.w);
					  c_10.w = c_11.w;
					  c_10.xyz = c_11.xyz;
					  gl_FragData[0] = c_10;
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
					varying highp vec2 xlv_TEXCOORD0;
					varying mediump vec3 xlv_TEXCOORD1;
					varying highp vec3 xlv_TEXCOORD2;
					varying lowp vec4 xlv_COLOR0;
					void main ()
					{
					  lowp vec3 worldNormal_1;
					  mediump vec3 tmpvar_2;
					  highp vec4 v_3;
					  highp float tmpvar_4;
					  tmpvar_4 = _World2Object[0].x;
					  v_3.x = tmpvar_4;
					  highp float tmpvar_5;
					  tmpvar_5 = _World2Object[1].x;
					  v_3.y = tmpvar_5;
					  highp float tmpvar_6;
					  tmpvar_6 = _World2Object[2].x;
					  v_3.z = tmpvar_6;
					  highp float tmpvar_7;
					  tmpvar_7 = _World2Object[3].x;
					  v_3.w = tmpvar_7;
					  highp vec4 v_8;
					  highp float tmpvar_9;
					  tmpvar_9 = _World2Object[0].y;
					  v_8.x = tmpvar_9;
					  highp float tmpvar_10;
					  tmpvar_10 = _World2Object[1].y;
					  v_8.y = tmpvar_10;
					  highp float tmpvar_11;
					  tmpvar_11 = _World2Object[2].y;
					  v_8.z = tmpvar_11;
					  highp float tmpvar_12;
					  tmpvar_12 = _World2Object[3].y;
					  v_8.w = tmpvar_12;
					  highp vec4 v_13;
					  highp float tmpvar_14;
					  tmpvar_14 = _World2Object[0].z;
					  v_13.x = tmpvar_14;
					  highp float tmpvar_15;
					  tmpvar_15 = _World2Object[1].z;
					  v_13.y = tmpvar_15;
					  highp float tmpvar_16;
					  tmpvar_16 = _World2Object[2].z;
					  v_13.z = tmpvar_16;
					  highp float tmpvar_17;
					  tmpvar_17 = _World2Object[3].z;
					  v_13.w = tmpvar_17;
					  highp vec3 tmpvar_18;
					  tmpvar_18 = normalize(((
					    (v_3.xyz * _glesNormal.x)
					   + 
					    (v_8.xyz * _glesNormal.y)
					  ) + (v_13.xyz * _glesNormal.z)));
					  highp mat3 tmpvar_19;
					  tmpvar_19[0] = _Object2World[0].xyz;
					  tmpvar_19[1] = _Object2World[1].xyz;
					  tmpvar_19[2] = _Object2World[2].xyz;
					  highp vec3 tmpvar_20;
					  tmpvar_20 = normalize((tmpvar_19 * _glesTANGENT.xyz));
					  highp vec4 vertex_21;
					  vertex_21.w = _glesVertex.w;
					  highp vec2 xzOff_22;
					  highp vec3 v2_23;
					  highp vec3 v1_24;
					  highp vec3 v0_25;
					  highp vec3 tmpvar_26;
					  tmpvar_26 = ((_Object2World * _glesVertex).xyz - _V_CW_PivotPoint_Position.xyz);
					  highp vec3 tmpvar_27;
					  tmpvar_27 = (tmpvar_26 + tmpvar_20);
					  v1_24.xz = tmpvar_27.xz;
					  highp vec3 tmpvar_28;
					  tmpvar_28 = (tmpvar_26 - ((tmpvar_18.yzx * tmpvar_20.zxy) - (tmpvar_18.zxy * tmpvar_20.yzx)));
					  v2_23.xz = tmpvar_28.xz;
					  highp vec2 tmpvar_29;
					  tmpvar_29.x = float((tmpvar_26.z >= 0.0));
					  tmpvar_29.y = float((tmpvar_26.x >= 0.0));
					  xzOff_22 = (max (vec2(0.0, 0.0), (
					    abs(tmpvar_26.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_29 * 2.0) - 1.0));
					  xzOff_22 = (xzOff_22 * xzOff_22);
					  highp vec3 tmpvar_30;
					  tmpvar_30.xz = vec2(0.0, 0.0);
					  tmpvar_30.y = (((_V_CW_Bend.x * xzOff_22.x) + (_V_CW_Bend.z * xzOff_22.y)) * 0.001);
					  v0_25 = (tmpvar_26 + tmpvar_30);
					  highp vec2 tmpvar_31;
					  tmpvar_31.x = float((tmpvar_27.z >= 0.0));
					  tmpvar_31.y = float((tmpvar_27.x >= 0.0));
					  xzOff_22 = (max (vec2(0.0, 0.0), (
					    abs(tmpvar_27.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_31 * 2.0) - 1.0));
					  xzOff_22 = (xzOff_22 * xzOff_22);
					  v1_24.y = (tmpvar_27.y + ((
					    (_V_CW_Bend.x * xzOff_22.x)
					   + 
					    (_V_CW_Bend.z * xzOff_22.y)
					  ) * 0.001));
					  highp vec2 tmpvar_32;
					  tmpvar_32.x = float((tmpvar_28.z >= 0.0));
					  tmpvar_32.y = float((tmpvar_28.x >= 0.0));
					  xzOff_22 = (max (vec2(0.0, 0.0), (
					    abs(tmpvar_28.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_32 * 2.0) - 1.0));
					  xzOff_22 = (xzOff_22 * xzOff_22);
					  v2_23.y = (tmpvar_28.y + ((
					    (_V_CW_Bend.x * xzOff_22.x)
					   + 
					    (_V_CW_Bend.z * xzOff_22.y)
					  ) * 0.001));
					  highp mat3 tmpvar_33;
					  tmpvar_33[0] = _World2Object[0].xyz;
					  tmpvar_33[1] = _World2Object[1].xyz;
					  tmpvar_33[2] = _World2Object[2].xyz;
					  vertex_21.xyz = (_glesVertex.xyz + (tmpvar_33 * tmpvar_30));
					  highp mat3 tmpvar_34;
					  tmpvar_34[0] = _World2Object[0].xyz;
					  tmpvar_34[1] = _World2Object[1].xyz;
					  tmpvar_34[2] = _World2Object[2].xyz;
					  highp vec3 a_35;
					  a_35 = (v2_23 - v0_25);
					  highp vec3 b_36;
					  b_36 = (v1_24 - v0_25);
					  highp vec3 tmpvar_37;
					  tmpvar_37 = normalize((tmpvar_34 * normalize(
					    ((a_35.yzx * b_36.zxy) - (a_35.zxy * b_36.yzx))
					  )));
					  highp vec4 v_38;
					  v_38.x = tmpvar_4;
					  v_38.y = tmpvar_5;
					  v_38.z = tmpvar_6;
					  v_38.w = tmpvar_7;
					  highp vec4 v_39;
					  v_39.x = tmpvar_9;
					  v_39.y = tmpvar_10;
					  v_39.z = tmpvar_11;
					  v_39.w = tmpvar_12;
					  highp vec4 v_40;
					  v_40.x = tmpvar_14;
					  v_40.y = tmpvar_15;
					  v_40.z = tmpvar_16;
					  v_40.w = tmpvar_17;
					  highp vec3 tmpvar_41;
					  tmpvar_41 = normalize(((
					    (v_38.xyz * tmpvar_37.x)
					   + 
					    (v_39.xyz * tmpvar_37.y)
					  ) + (v_40.xyz * tmpvar_37.z)));
					  worldNormal_1 = tmpvar_41;
					  tmpvar_2 = worldNormal_1;
					  gl_Position = (glstate_matrix_mvp * vertex_21);
					  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
					  xlv_TEXCOORD1 = tmpvar_2;
					  xlv_TEXCOORD2 = (_Object2World * vertex_21).xyz;
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
					varying highp vec2 xlv_TEXCOORD0;
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
					  lowp vec4 tmpvar_10;
					  highp vec2 P_11;
					  P_11 = (xlv_TEXCOORD0 + (_V_CW_MainTex_Scroll * _Time.x));
					  tmpvar_10 = texture2D (_MainTex, P_11);
					  tmpvar_9 = (tmpvar_10.xyz * _Color.xyz);
					  tmpvar_9 = (tmpvar_9 * tmpvar_7.xyz);
					  highp vec4 tmpvar_12;
					  tmpvar_12.w = 1.0;
					  tmpvar_12.xyz = xlv_TEXCOORD2;
					  highp vec4 tmpvar_13;
					  tmpvar_13 = (_LightMatrix0 * tmpvar_12);
					  lightCoord_4 = tmpvar_13;
					  lowp vec4 tmpvar_14;
					  mediump vec2 P_15;
					  P_15 = ((lightCoord_4.xy / lightCoord_4.w) + 0.5);
					  tmpvar_14 = texture2D (_LightTexture0, P_15);
					  highp vec3 LightCoord_16;
					  LightCoord_16 = lightCoord_4.xyz;
					  highp float tmpvar_17;
					  tmpvar_17 = dot (LightCoord_16, LightCoord_16);
					  lowp vec4 tmpvar_18;
					  tmpvar_18 = texture2D (_LightTextureB0, vec2(tmpvar_17));
					  mediump float tmpvar_19;
					  tmpvar_19 = ((float(
					    (lightCoord_4.z > 0.0)
					  ) * tmpvar_14.w) * tmpvar_18.w);
					  atten_3 = tmpvar_19;
					  tmpvar_1 = _LightColor0.xyz;
					  tmpvar_2 = lightDir_6;
					  tmpvar_1 = (tmpvar_1 * atten_3);
					  lowp vec4 c_20;
					  lowp vec4 c_21;
					  lowp float diff_22;
					  mediump float tmpvar_23;
					  tmpvar_23 = max (0.0, dot (tmpvar_5, tmpvar_2));
					  diff_22 = tmpvar_23;
					  c_21.xyz = ((tmpvar_9 * tmpvar_1) * diff_22);
					  c_21.w = (tmpvar_10.w * _Color.w);
					  c_20.w = c_21.w;
					  c_20.xyz = c_21.xyz;
					  gl_FragData[0] = c_20;
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
					varying highp vec2 xlv_TEXCOORD0;
					varying mediump vec3 xlv_TEXCOORD1;
					varying highp vec3 xlv_TEXCOORD2;
					varying lowp vec4 xlv_COLOR0;
					void main ()
					{
					  lowp vec3 worldNormal_1;
					  mediump vec3 tmpvar_2;
					  highp vec4 v_3;
					  highp float tmpvar_4;
					  tmpvar_4 = _World2Object[0].x;
					  v_3.x = tmpvar_4;
					  highp float tmpvar_5;
					  tmpvar_5 = _World2Object[1].x;
					  v_3.y = tmpvar_5;
					  highp float tmpvar_6;
					  tmpvar_6 = _World2Object[2].x;
					  v_3.z = tmpvar_6;
					  highp float tmpvar_7;
					  tmpvar_7 = _World2Object[3].x;
					  v_3.w = tmpvar_7;
					  highp vec4 v_8;
					  highp float tmpvar_9;
					  tmpvar_9 = _World2Object[0].y;
					  v_8.x = tmpvar_9;
					  highp float tmpvar_10;
					  tmpvar_10 = _World2Object[1].y;
					  v_8.y = tmpvar_10;
					  highp float tmpvar_11;
					  tmpvar_11 = _World2Object[2].y;
					  v_8.z = tmpvar_11;
					  highp float tmpvar_12;
					  tmpvar_12 = _World2Object[3].y;
					  v_8.w = tmpvar_12;
					  highp vec4 v_13;
					  highp float tmpvar_14;
					  tmpvar_14 = _World2Object[0].z;
					  v_13.x = tmpvar_14;
					  highp float tmpvar_15;
					  tmpvar_15 = _World2Object[1].z;
					  v_13.y = tmpvar_15;
					  highp float tmpvar_16;
					  tmpvar_16 = _World2Object[2].z;
					  v_13.z = tmpvar_16;
					  highp float tmpvar_17;
					  tmpvar_17 = _World2Object[3].z;
					  v_13.w = tmpvar_17;
					  highp vec3 tmpvar_18;
					  tmpvar_18 = normalize(((
					    (v_3.xyz * _glesNormal.x)
					   + 
					    (v_8.xyz * _glesNormal.y)
					  ) + (v_13.xyz * _glesNormal.z)));
					  highp mat3 tmpvar_19;
					  tmpvar_19[0] = _Object2World[0].xyz;
					  tmpvar_19[1] = _Object2World[1].xyz;
					  tmpvar_19[2] = _Object2World[2].xyz;
					  highp vec3 tmpvar_20;
					  tmpvar_20 = normalize((tmpvar_19 * _glesTANGENT.xyz));
					  highp vec4 vertex_21;
					  vertex_21.w = _glesVertex.w;
					  highp vec2 xzOff_22;
					  highp vec3 v2_23;
					  highp vec3 v1_24;
					  highp vec3 v0_25;
					  highp vec3 tmpvar_26;
					  tmpvar_26 = ((_Object2World * _glesVertex).xyz - _V_CW_PivotPoint_Position.xyz);
					  highp vec3 tmpvar_27;
					  tmpvar_27 = (tmpvar_26 + tmpvar_20);
					  v1_24.xz = tmpvar_27.xz;
					  highp vec3 tmpvar_28;
					  tmpvar_28 = (tmpvar_26 - ((tmpvar_18.yzx * tmpvar_20.zxy) - (tmpvar_18.zxy * tmpvar_20.yzx)));
					  v2_23.xz = tmpvar_28.xz;
					  highp vec2 tmpvar_29;
					  tmpvar_29.x = float((tmpvar_26.z >= 0.0));
					  tmpvar_29.y = float((tmpvar_26.x >= 0.0));
					  xzOff_22 = (max (vec2(0.0, 0.0), (
					    abs(tmpvar_26.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_29 * 2.0) - 1.0));
					  xzOff_22 = (xzOff_22 * xzOff_22);
					  highp vec3 tmpvar_30;
					  tmpvar_30.xz = vec2(0.0, 0.0);
					  tmpvar_30.y = (((_V_CW_Bend.x * xzOff_22.x) + (_V_CW_Bend.z * xzOff_22.y)) * 0.001);
					  v0_25 = (tmpvar_26 + tmpvar_30);
					  highp vec2 tmpvar_31;
					  tmpvar_31.x = float((tmpvar_27.z >= 0.0));
					  tmpvar_31.y = float((tmpvar_27.x >= 0.0));
					  xzOff_22 = (max (vec2(0.0, 0.0), (
					    abs(tmpvar_27.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_31 * 2.0) - 1.0));
					  xzOff_22 = (xzOff_22 * xzOff_22);
					  v1_24.y = (tmpvar_27.y + ((
					    (_V_CW_Bend.x * xzOff_22.x)
					   + 
					    (_V_CW_Bend.z * xzOff_22.y)
					  ) * 0.001));
					  highp vec2 tmpvar_32;
					  tmpvar_32.x = float((tmpvar_28.z >= 0.0));
					  tmpvar_32.y = float((tmpvar_28.x >= 0.0));
					  xzOff_22 = (max (vec2(0.0, 0.0), (
					    abs(tmpvar_28.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_32 * 2.0) - 1.0));
					  xzOff_22 = (xzOff_22 * xzOff_22);
					  v2_23.y = (tmpvar_28.y + ((
					    (_V_CW_Bend.x * xzOff_22.x)
					   + 
					    (_V_CW_Bend.z * xzOff_22.y)
					  ) * 0.001));
					  highp mat3 tmpvar_33;
					  tmpvar_33[0] = _World2Object[0].xyz;
					  tmpvar_33[1] = _World2Object[1].xyz;
					  tmpvar_33[2] = _World2Object[2].xyz;
					  vertex_21.xyz = (_glesVertex.xyz + (tmpvar_33 * tmpvar_30));
					  highp mat3 tmpvar_34;
					  tmpvar_34[0] = _World2Object[0].xyz;
					  tmpvar_34[1] = _World2Object[1].xyz;
					  tmpvar_34[2] = _World2Object[2].xyz;
					  highp vec3 a_35;
					  a_35 = (v2_23 - v0_25);
					  highp vec3 b_36;
					  b_36 = (v1_24 - v0_25);
					  highp vec3 tmpvar_37;
					  tmpvar_37 = normalize((tmpvar_34 * normalize(
					    ((a_35.yzx * b_36.zxy) - (a_35.zxy * b_36.yzx))
					  )));
					  highp vec4 v_38;
					  v_38.x = tmpvar_4;
					  v_38.y = tmpvar_5;
					  v_38.z = tmpvar_6;
					  v_38.w = tmpvar_7;
					  highp vec4 v_39;
					  v_39.x = tmpvar_9;
					  v_39.y = tmpvar_10;
					  v_39.z = tmpvar_11;
					  v_39.w = tmpvar_12;
					  highp vec4 v_40;
					  v_40.x = tmpvar_14;
					  v_40.y = tmpvar_15;
					  v_40.z = tmpvar_16;
					  v_40.w = tmpvar_17;
					  highp vec3 tmpvar_41;
					  tmpvar_41 = normalize(((
					    (v_38.xyz * tmpvar_37.x)
					   + 
					    (v_39.xyz * tmpvar_37.y)
					  ) + (v_40.xyz * tmpvar_37.z)));
					  worldNormal_1 = tmpvar_41;
					  tmpvar_2 = worldNormal_1;
					  gl_Position = (glstate_matrix_mvp * vertex_21);
					  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
					  xlv_TEXCOORD1 = tmpvar_2;
					  xlv_TEXCOORD2 = (_Object2World * vertex_21).xyz;
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
					varying highp vec2 xlv_TEXCOORD0;
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
					  lowp vec4 tmpvar_8;
					  highp vec2 P_9;
					  P_9 = (xlv_TEXCOORD0 + (_V_CW_MainTex_Scroll * _Time.x));
					  tmpvar_8 = texture2D (_MainTex, P_9);
					  tmpvar_7 = (tmpvar_8.xyz * _Color.xyz);
					  tmpvar_7 = (tmpvar_7 * tmpvar_5.xyz);
					  highp vec4 tmpvar_10;
					  tmpvar_10.w = 1.0;
					  tmpvar_10.xyz = xlv_TEXCOORD2;
					  highp vec3 tmpvar_11;
					  tmpvar_11 = (_LightMatrix0 * tmpvar_10).xyz;
					  highp float tmpvar_12;
					  tmpvar_12 = dot (tmpvar_11, tmpvar_11);
					  lowp float tmpvar_13;
					  tmpvar_13 = (texture2D (_LightTextureB0, vec2(tmpvar_12)).w * textureCube (_LightTexture0, tmpvar_11).w);
					  tmpvar_1 = _LightColor0.xyz;
					  tmpvar_2 = lightDir_4;
					  tmpvar_1 = (tmpvar_1 * tmpvar_13);
					  lowp vec4 c_14;
					  lowp vec4 c_15;
					  lowp float diff_16;
					  mediump float tmpvar_17;
					  tmpvar_17 = max (0.0, dot (tmpvar_3, tmpvar_2));
					  diff_16 = tmpvar_17;
					  c_15.xyz = ((tmpvar_7 * tmpvar_1) * diff_16);
					  c_15.w = (tmpvar_8.w * _Color.w);
					  c_14.w = c_15.w;
					  c_14.xyz = c_15.xyz;
					  gl_FragData[0] = c_14;
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
					varying highp vec2 xlv_TEXCOORD0;
					varying mediump vec3 xlv_TEXCOORD1;
					varying highp vec3 xlv_TEXCOORD2;
					varying lowp vec4 xlv_COLOR0;
					void main ()
					{
					  lowp vec3 worldNormal_1;
					  mediump vec3 tmpvar_2;
					  highp vec4 v_3;
					  highp float tmpvar_4;
					  tmpvar_4 = _World2Object[0].x;
					  v_3.x = tmpvar_4;
					  highp float tmpvar_5;
					  tmpvar_5 = _World2Object[1].x;
					  v_3.y = tmpvar_5;
					  highp float tmpvar_6;
					  tmpvar_6 = _World2Object[2].x;
					  v_3.z = tmpvar_6;
					  highp float tmpvar_7;
					  tmpvar_7 = _World2Object[3].x;
					  v_3.w = tmpvar_7;
					  highp vec4 v_8;
					  highp float tmpvar_9;
					  tmpvar_9 = _World2Object[0].y;
					  v_8.x = tmpvar_9;
					  highp float tmpvar_10;
					  tmpvar_10 = _World2Object[1].y;
					  v_8.y = tmpvar_10;
					  highp float tmpvar_11;
					  tmpvar_11 = _World2Object[2].y;
					  v_8.z = tmpvar_11;
					  highp float tmpvar_12;
					  tmpvar_12 = _World2Object[3].y;
					  v_8.w = tmpvar_12;
					  highp vec4 v_13;
					  highp float tmpvar_14;
					  tmpvar_14 = _World2Object[0].z;
					  v_13.x = tmpvar_14;
					  highp float tmpvar_15;
					  tmpvar_15 = _World2Object[1].z;
					  v_13.y = tmpvar_15;
					  highp float tmpvar_16;
					  tmpvar_16 = _World2Object[2].z;
					  v_13.z = tmpvar_16;
					  highp float tmpvar_17;
					  tmpvar_17 = _World2Object[3].z;
					  v_13.w = tmpvar_17;
					  highp vec3 tmpvar_18;
					  tmpvar_18 = normalize(((
					    (v_3.xyz * _glesNormal.x)
					   + 
					    (v_8.xyz * _glesNormal.y)
					  ) + (v_13.xyz * _glesNormal.z)));
					  highp mat3 tmpvar_19;
					  tmpvar_19[0] = _Object2World[0].xyz;
					  tmpvar_19[1] = _Object2World[1].xyz;
					  tmpvar_19[2] = _Object2World[2].xyz;
					  highp vec3 tmpvar_20;
					  tmpvar_20 = normalize((tmpvar_19 * _glesTANGENT.xyz));
					  highp vec4 vertex_21;
					  vertex_21.w = _glesVertex.w;
					  highp vec2 xzOff_22;
					  highp vec3 v2_23;
					  highp vec3 v1_24;
					  highp vec3 v0_25;
					  highp vec3 tmpvar_26;
					  tmpvar_26 = ((_Object2World * _glesVertex).xyz - _V_CW_PivotPoint_Position.xyz);
					  highp vec3 tmpvar_27;
					  tmpvar_27 = (tmpvar_26 + tmpvar_20);
					  v1_24.xz = tmpvar_27.xz;
					  highp vec3 tmpvar_28;
					  tmpvar_28 = (tmpvar_26 - ((tmpvar_18.yzx * tmpvar_20.zxy) - (tmpvar_18.zxy * tmpvar_20.yzx)));
					  v2_23.xz = tmpvar_28.xz;
					  highp vec2 tmpvar_29;
					  tmpvar_29.x = float((tmpvar_26.z >= 0.0));
					  tmpvar_29.y = float((tmpvar_26.x >= 0.0));
					  xzOff_22 = (max (vec2(0.0, 0.0), (
					    abs(tmpvar_26.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_29 * 2.0) - 1.0));
					  xzOff_22 = (xzOff_22 * xzOff_22);
					  highp vec3 tmpvar_30;
					  tmpvar_30.xz = vec2(0.0, 0.0);
					  tmpvar_30.y = (((_V_CW_Bend.x * xzOff_22.x) + (_V_CW_Bend.z * xzOff_22.y)) * 0.001);
					  v0_25 = (tmpvar_26 + tmpvar_30);
					  highp vec2 tmpvar_31;
					  tmpvar_31.x = float((tmpvar_27.z >= 0.0));
					  tmpvar_31.y = float((tmpvar_27.x >= 0.0));
					  xzOff_22 = (max (vec2(0.0, 0.0), (
					    abs(tmpvar_27.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_31 * 2.0) - 1.0));
					  xzOff_22 = (xzOff_22 * xzOff_22);
					  v1_24.y = (tmpvar_27.y + ((
					    (_V_CW_Bend.x * xzOff_22.x)
					   + 
					    (_V_CW_Bend.z * xzOff_22.y)
					  ) * 0.001));
					  highp vec2 tmpvar_32;
					  tmpvar_32.x = float((tmpvar_28.z >= 0.0));
					  tmpvar_32.y = float((tmpvar_28.x >= 0.0));
					  xzOff_22 = (max (vec2(0.0, 0.0), (
					    abs(tmpvar_28.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_32 * 2.0) - 1.0));
					  xzOff_22 = (xzOff_22 * xzOff_22);
					  v2_23.y = (tmpvar_28.y + ((
					    (_V_CW_Bend.x * xzOff_22.x)
					   + 
					    (_V_CW_Bend.z * xzOff_22.y)
					  ) * 0.001));
					  highp mat3 tmpvar_33;
					  tmpvar_33[0] = _World2Object[0].xyz;
					  tmpvar_33[1] = _World2Object[1].xyz;
					  tmpvar_33[2] = _World2Object[2].xyz;
					  vertex_21.xyz = (_glesVertex.xyz + (tmpvar_33 * tmpvar_30));
					  highp mat3 tmpvar_34;
					  tmpvar_34[0] = _World2Object[0].xyz;
					  tmpvar_34[1] = _World2Object[1].xyz;
					  tmpvar_34[2] = _World2Object[2].xyz;
					  highp vec3 a_35;
					  a_35 = (v2_23 - v0_25);
					  highp vec3 b_36;
					  b_36 = (v1_24 - v0_25);
					  highp vec3 tmpvar_37;
					  tmpvar_37 = normalize((tmpvar_34 * normalize(
					    ((a_35.yzx * b_36.zxy) - (a_35.zxy * b_36.yzx))
					  )));
					  highp vec4 v_38;
					  v_38.x = tmpvar_4;
					  v_38.y = tmpvar_5;
					  v_38.z = tmpvar_6;
					  v_38.w = tmpvar_7;
					  highp vec4 v_39;
					  v_39.x = tmpvar_9;
					  v_39.y = tmpvar_10;
					  v_39.z = tmpvar_11;
					  v_39.w = tmpvar_12;
					  highp vec4 v_40;
					  v_40.x = tmpvar_14;
					  v_40.y = tmpvar_15;
					  v_40.z = tmpvar_16;
					  v_40.w = tmpvar_17;
					  highp vec3 tmpvar_41;
					  tmpvar_41 = normalize(((
					    (v_38.xyz * tmpvar_37.x)
					   + 
					    (v_39.xyz * tmpvar_37.y)
					  ) + (v_40.xyz * tmpvar_37.z)));
					  worldNormal_1 = tmpvar_41;
					  tmpvar_2 = worldNormal_1;
					  gl_Position = (glstate_matrix_mvp * vertex_21);
					  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
					  xlv_TEXCOORD1 = tmpvar_2;
					  xlv_TEXCOORD2 = (_Object2World * vertex_21).xyz;
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
					varying highp vec2 xlv_TEXCOORD0;
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
					  lowp vec4 tmpvar_8;
					  highp vec2 P_9;
					  P_9 = (xlv_TEXCOORD0 + (_V_CW_MainTex_Scroll * _Time.x));
					  tmpvar_8 = texture2D (_MainTex, P_9);
					  tmpvar_7 = (tmpvar_8.xyz * _Color.xyz);
					  tmpvar_7 = (tmpvar_7 * tmpvar_5.xyz);
					  highp vec4 tmpvar_10;
					  tmpvar_10.w = 1.0;
					  tmpvar_10.xyz = xlv_TEXCOORD2;
					  highp vec2 tmpvar_11;
					  tmpvar_11 = (_LightMatrix0 * tmpvar_10).xy;
					  lowp float tmpvar_12;
					  tmpvar_12 = texture2D (_LightTexture0, tmpvar_11).w;
					  tmpvar_1 = _LightColor0.xyz;
					  tmpvar_2 = lightDir_4;
					  tmpvar_1 = (tmpvar_1 * tmpvar_12);
					  lowp vec4 c_13;
					  lowp vec4 c_14;
					  lowp float diff_15;
					  mediump float tmpvar_16;
					  tmpvar_16 = max (0.0, dot (tmpvar_3, tmpvar_2));
					  diff_15 = tmpvar_16;
					  c_14.xyz = ((tmpvar_7 * tmpvar_1) * diff_15);
					  c_14.w = (tmpvar_8.w * _Color.w);
					  c_13.w = c_14.w;
					  c_13.xyz = c_14.xyz;
					  gl_FragData[0] = c_13;
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
					varying highp vec2 xlv_TEXCOORD0;
					varying mediump vec3 xlv_TEXCOORD1;
					varying highp vec3 xlv_TEXCOORD2;
					varying lowp vec4 xlv_COLOR0;
					varying highp float xlv_TEXCOORD3;
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
					  tmpvar_2 = (glstate_matrix_mvp * vertex_22);
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
					  gl_Position = tmpvar_2;
					  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
					  xlv_TEXCOORD1 = tmpvar_3;
					  xlv_TEXCOORD2 = (_Object2World * vertex_22).xyz;
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
					varying highp vec2 xlv_TEXCOORD0;
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
					  lowp vec4 tmpvar_9;
					  highp vec2 P_10;
					  P_10 = (xlv_TEXCOORD0 + (_V_CW_MainTex_Scroll * _Time.x));
					  tmpvar_9 = texture2D (_MainTex, P_10);
					  tmpvar_8 = (tmpvar_9.xyz * _Color.xyz);
					  tmpvar_8 = (tmpvar_8 * tmpvar_6.xyz);
					  highp vec4 tmpvar_11;
					  tmpvar_11.w = 1.0;
					  tmpvar_11.xyz = xlv_TEXCOORD2;
					  highp vec3 tmpvar_12;
					  tmpvar_12 = (_LightMatrix0 * tmpvar_11).xyz;
					  highp float tmpvar_13;
					  tmpvar_13 = dot (tmpvar_12, tmpvar_12);
					  lowp float tmpvar_14;
					  tmpvar_14 = texture2D (_LightTexture0, vec2(tmpvar_13)).w;
					  tmpvar_1 = _LightColor0.xyz;
					  tmpvar_2 = lightDir_5;
					  tmpvar_1 = (tmpvar_1 * tmpvar_14);
					  lowp vec4 c_15;
					  lowp vec4 c_16;
					  lowp float diff_17;
					  mediump float tmpvar_18;
					  tmpvar_18 = max (0.0, dot (tmpvar_4, tmpvar_2));
					  diff_17 = tmpvar_18;
					  c_16.xyz = ((tmpvar_8 * tmpvar_1) * diff_17);
					  c_16.w = (tmpvar_9.w * _Color.w);
					  c_15.w = c_16.w;
					  c_15.xyz = c_16.xyz;
					  c_3.w = c_15.w;
					  highp float tmpvar_19;
					  tmpvar_19 = clamp (xlv_TEXCOORD3, 0.0, 1.0);
					  c_3.xyz = (c_16.xyz * vec3(tmpvar_19));
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
					varying highp vec2 xlv_TEXCOORD0;
					varying mediump vec3 xlv_TEXCOORD1;
					varying highp vec3 xlv_TEXCOORD2;
					varying lowp vec4 xlv_COLOR0;
					varying highp float xlv_TEXCOORD3;
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
					  tmpvar_2 = (glstate_matrix_mvp * vertex_22);
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
					  gl_Position = tmpvar_2;
					  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
					  xlv_TEXCOORD1 = tmpvar_3;
					  xlv_TEXCOORD2 = (_Object2World * vertex_22).xyz;
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
					varying highp vec2 xlv_TEXCOORD0;
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
					  lowp vec4 tmpvar_9;
					  highp vec2 P_10;
					  P_10 = (xlv_TEXCOORD0 + (_V_CW_MainTex_Scroll * _Time.x));
					  tmpvar_9 = texture2D (_MainTex, P_10);
					  tmpvar_8 = (tmpvar_9.xyz * _Color.xyz);
					  tmpvar_8 = (tmpvar_8 * tmpvar_6.xyz);
					  tmpvar_1 = _LightColor0.xyz;
					  tmpvar_2 = lightDir_5;
					  lowp vec4 c_11;
					  lowp vec4 c_12;
					  lowp float diff_13;
					  mediump float tmpvar_14;
					  tmpvar_14 = max (0.0, dot (tmpvar_4, tmpvar_2));
					  diff_13 = tmpvar_14;
					  c_12.xyz = ((tmpvar_8 * tmpvar_1) * diff_13);
					  c_12.w = (tmpvar_9.w * _Color.w);
					  c_11.w = c_12.w;
					  c_11.xyz = c_12.xyz;
					  c_3.w = c_11.w;
					  highp float tmpvar_15;
					  tmpvar_15 = clamp (xlv_TEXCOORD3, 0.0, 1.0);
					  c_3.xyz = (c_12.xyz * vec3(tmpvar_15));
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
					varying highp vec2 xlv_TEXCOORD0;
					varying mediump vec3 xlv_TEXCOORD1;
					varying highp vec3 xlv_TEXCOORD2;
					varying lowp vec4 xlv_COLOR0;
					varying highp float xlv_TEXCOORD3;
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
					  tmpvar_2 = (glstate_matrix_mvp * vertex_22);
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
					  gl_Position = tmpvar_2;
					  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
					  xlv_TEXCOORD1 = tmpvar_3;
					  xlv_TEXCOORD2 = (_Object2World * vertex_22).xyz;
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
					varying highp vec2 xlv_TEXCOORD0;
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
					  lowp vec4 tmpvar_11;
					  highp vec2 P_12;
					  P_12 = (xlv_TEXCOORD0 + (_V_CW_MainTex_Scroll * _Time.x));
					  tmpvar_11 = texture2D (_MainTex, P_12);
					  tmpvar_10 = (tmpvar_11.xyz * _Color.xyz);
					  tmpvar_10 = (tmpvar_10 * tmpvar_8.xyz);
					  highp vec4 tmpvar_13;
					  tmpvar_13.w = 1.0;
					  tmpvar_13.xyz = xlv_TEXCOORD2;
					  highp vec4 tmpvar_14;
					  tmpvar_14 = (_LightMatrix0 * tmpvar_13);
					  lightCoord_5 = tmpvar_14;
					  lowp vec4 tmpvar_15;
					  mediump vec2 P_16;
					  P_16 = ((lightCoord_5.xy / lightCoord_5.w) + 0.5);
					  tmpvar_15 = texture2D (_LightTexture0, P_16);
					  highp vec3 LightCoord_17;
					  LightCoord_17 = lightCoord_5.xyz;
					  highp float tmpvar_18;
					  tmpvar_18 = dot (LightCoord_17, LightCoord_17);
					  lowp vec4 tmpvar_19;
					  tmpvar_19 = texture2D (_LightTextureB0, vec2(tmpvar_18));
					  mediump float tmpvar_20;
					  tmpvar_20 = ((float(
					    (lightCoord_5.z > 0.0)
					  ) * tmpvar_15.w) * tmpvar_19.w);
					  atten_4 = tmpvar_20;
					  tmpvar_1 = _LightColor0.xyz;
					  tmpvar_2 = lightDir_7;
					  tmpvar_1 = (tmpvar_1 * atten_4);
					  lowp vec4 c_21;
					  lowp vec4 c_22;
					  lowp float diff_23;
					  mediump float tmpvar_24;
					  tmpvar_24 = max (0.0, dot (tmpvar_6, tmpvar_2));
					  diff_23 = tmpvar_24;
					  c_22.xyz = ((tmpvar_10 * tmpvar_1) * diff_23);
					  c_22.w = (tmpvar_11.w * _Color.w);
					  c_21.w = c_22.w;
					  c_21.xyz = c_22.xyz;
					  c_3.w = c_21.w;
					  highp float tmpvar_25;
					  tmpvar_25 = clamp (xlv_TEXCOORD3, 0.0, 1.0);
					  c_3.xyz = (c_22.xyz * vec3(tmpvar_25));
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
					varying highp vec2 xlv_TEXCOORD0;
					varying mediump vec3 xlv_TEXCOORD1;
					varying highp vec3 xlv_TEXCOORD2;
					varying lowp vec4 xlv_COLOR0;
					varying highp float xlv_TEXCOORD3;
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
					  tmpvar_2 = (glstate_matrix_mvp * vertex_22);
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
					  gl_Position = tmpvar_2;
					  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
					  xlv_TEXCOORD1 = tmpvar_3;
					  xlv_TEXCOORD2 = (_Object2World * vertex_22).xyz;
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
					varying highp vec2 xlv_TEXCOORD0;
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
					  lowp vec4 tmpvar_9;
					  highp vec2 P_10;
					  P_10 = (xlv_TEXCOORD0 + (_V_CW_MainTex_Scroll * _Time.x));
					  tmpvar_9 = texture2D (_MainTex, P_10);
					  tmpvar_8 = (tmpvar_9.xyz * _Color.xyz);
					  tmpvar_8 = (tmpvar_8 * tmpvar_6.xyz);
					  highp vec4 tmpvar_11;
					  tmpvar_11.w = 1.0;
					  tmpvar_11.xyz = xlv_TEXCOORD2;
					  highp vec3 tmpvar_12;
					  tmpvar_12 = (_LightMatrix0 * tmpvar_11).xyz;
					  highp float tmpvar_13;
					  tmpvar_13 = dot (tmpvar_12, tmpvar_12);
					  lowp float tmpvar_14;
					  tmpvar_14 = (texture2D (_LightTextureB0, vec2(tmpvar_13)).w * textureCube (_LightTexture0, tmpvar_12).w);
					  tmpvar_1 = _LightColor0.xyz;
					  tmpvar_2 = lightDir_5;
					  tmpvar_1 = (tmpvar_1 * tmpvar_14);
					  lowp vec4 c_15;
					  lowp vec4 c_16;
					  lowp float diff_17;
					  mediump float tmpvar_18;
					  tmpvar_18 = max (0.0, dot (tmpvar_4, tmpvar_2));
					  diff_17 = tmpvar_18;
					  c_16.xyz = ((tmpvar_8 * tmpvar_1) * diff_17);
					  c_16.w = (tmpvar_9.w * _Color.w);
					  c_15.w = c_16.w;
					  c_15.xyz = c_16.xyz;
					  c_3.w = c_15.w;
					  highp float tmpvar_19;
					  tmpvar_19 = clamp (xlv_TEXCOORD3, 0.0, 1.0);
					  c_3.xyz = (c_16.xyz * vec3(tmpvar_19));
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
					varying highp vec2 xlv_TEXCOORD0;
					varying mediump vec3 xlv_TEXCOORD1;
					varying highp vec3 xlv_TEXCOORD2;
					varying lowp vec4 xlv_COLOR0;
					varying highp float xlv_TEXCOORD3;
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
					  tmpvar_2 = (glstate_matrix_mvp * vertex_22);
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
					  gl_Position = tmpvar_2;
					  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
					  xlv_TEXCOORD1 = tmpvar_3;
					  xlv_TEXCOORD2 = (_Object2World * vertex_22).xyz;
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
					varying highp vec2 xlv_TEXCOORD0;
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
					  lowp vec4 tmpvar_9;
					  highp vec2 P_10;
					  P_10 = (xlv_TEXCOORD0 + (_V_CW_MainTex_Scroll * _Time.x));
					  tmpvar_9 = texture2D (_MainTex, P_10);
					  tmpvar_8 = (tmpvar_9.xyz * _Color.xyz);
					  tmpvar_8 = (tmpvar_8 * tmpvar_6.xyz);
					  highp vec4 tmpvar_11;
					  tmpvar_11.w = 1.0;
					  tmpvar_11.xyz = xlv_TEXCOORD2;
					  highp vec2 tmpvar_12;
					  tmpvar_12 = (_LightMatrix0 * tmpvar_11).xy;
					  lowp float tmpvar_13;
					  tmpvar_13 = texture2D (_LightTexture0, tmpvar_12).w;
					  tmpvar_1 = _LightColor0.xyz;
					  tmpvar_2 = lightDir_5;
					  tmpvar_1 = (tmpvar_1 * tmpvar_13);
					  lowp vec4 c_14;
					  lowp vec4 c_15;
					  lowp float diff_16;
					  mediump float tmpvar_17;
					  tmpvar_17 = max (0.0, dot (tmpvar_4, tmpvar_2));
					  diff_16 = tmpvar_17;
					  c_15.xyz = ((tmpvar_8 * tmpvar_1) * diff_16);
					  c_15.w = (tmpvar_9.w * _Color.w);
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
Keywords { "POINT" "V_CW_SPECULAR" "V_CW_VERTEX_COLOR" "V_CW_REFLECTIVE_OFF" "_EMISSION_OFF" "V_CW_RIM_OFF" "_NORMALMAP_OFF" }
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
					varying highp vec2 xlv_TEXCOORD0;
					varying mediump vec3 xlv_TEXCOORD1;
					varying highp vec3 xlv_TEXCOORD2;
					varying lowp vec4 xlv_COLOR0;
					void main ()
					{
					  lowp vec3 worldNormal_1;
					  mediump vec3 tmpvar_2;
					  highp vec4 v_3;
					  highp float tmpvar_4;
					  tmpvar_4 = _World2Object[0].x;
					  v_3.x = tmpvar_4;
					  highp float tmpvar_5;
					  tmpvar_5 = _World2Object[1].x;
					  v_3.y = tmpvar_5;
					  highp float tmpvar_6;
					  tmpvar_6 = _World2Object[2].x;
					  v_3.z = tmpvar_6;
					  highp float tmpvar_7;
					  tmpvar_7 = _World2Object[3].x;
					  v_3.w = tmpvar_7;
					  highp vec4 v_8;
					  highp float tmpvar_9;
					  tmpvar_9 = _World2Object[0].y;
					  v_8.x = tmpvar_9;
					  highp float tmpvar_10;
					  tmpvar_10 = _World2Object[1].y;
					  v_8.y = tmpvar_10;
					  highp float tmpvar_11;
					  tmpvar_11 = _World2Object[2].y;
					  v_8.z = tmpvar_11;
					  highp float tmpvar_12;
					  tmpvar_12 = _World2Object[3].y;
					  v_8.w = tmpvar_12;
					  highp vec4 v_13;
					  highp float tmpvar_14;
					  tmpvar_14 = _World2Object[0].z;
					  v_13.x = tmpvar_14;
					  highp float tmpvar_15;
					  tmpvar_15 = _World2Object[1].z;
					  v_13.y = tmpvar_15;
					  highp float tmpvar_16;
					  tmpvar_16 = _World2Object[2].z;
					  v_13.z = tmpvar_16;
					  highp float tmpvar_17;
					  tmpvar_17 = _World2Object[3].z;
					  v_13.w = tmpvar_17;
					  highp vec3 tmpvar_18;
					  tmpvar_18 = normalize(((
					    (v_3.xyz * _glesNormal.x)
					   + 
					    (v_8.xyz * _glesNormal.y)
					  ) + (v_13.xyz * _glesNormal.z)));
					  highp mat3 tmpvar_19;
					  tmpvar_19[0] = _Object2World[0].xyz;
					  tmpvar_19[1] = _Object2World[1].xyz;
					  tmpvar_19[2] = _Object2World[2].xyz;
					  highp vec3 tmpvar_20;
					  tmpvar_20 = normalize((tmpvar_19 * _glesTANGENT.xyz));
					  highp vec4 vertex_21;
					  vertex_21.w = _glesVertex.w;
					  highp vec2 xzOff_22;
					  highp vec3 v2_23;
					  highp vec3 v1_24;
					  highp vec3 v0_25;
					  highp vec3 tmpvar_26;
					  tmpvar_26 = ((_Object2World * _glesVertex).xyz - _V_CW_PivotPoint_Position.xyz);
					  highp vec3 tmpvar_27;
					  tmpvar_27 = (tmpvar_26 + tmpvar_20);
					  v1_24.xz = tmpvar_27.xz;
					  highp vec3 tmpvar_28;
					  tmpvar_28 = (tmpvar_26 - ((tmpvar_18.yzx * tmpvar_20.zxy) - (tmpvar_18.zxy * tmpvar_20.yzx)));
					  v2_23.xz = tmpvar_28.xz;
					  highp vec2 tmpvar_29;
					  tmpvar_29.x = float((tmpvar_26.z >= 0.0));
					  tmpvar_29.y = float((tmpvar_26.x >= 0.0));
					  xzOff_22 = (max (vec2(0.0, 0.0), (
					    abs(tmpvar_26.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_29 * 2.0) - 1.0));
					  xzOff_22 = (xzOff_22 * xzOff_22);
					  highp vec3 tmpvar_30;
					  tmpvar_30.xz = vec2(0.0, 0.0);
					  tmpvar_30.y = (((_V_CW_Bend.x * xzOff_22.x) + (_V_CW_Bend.z * xzOff_22.y)) * 0.001);
					  v0_25 = (tmpvar_26 + tmpvar_30);
					  highp vec2 tmpvar_31;
					  tmpvar_31.x = float((tmpvar_27.z >= 0.0));
					  tmpvar_31.y = float((tmpvar_27.x >= 0.0));
					  xzOff_22 = (max (vec2(0.0, 0.0), (
					    abs(tmpvar_27.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_31 * 2.0) - 1.0));
					  xzOff_22 = (xzOff_22 * xzOff_22);
					  v1_24.y = (tmpvar_27.y + ((
					    (_V_CW_Bend.x * xzOff_22.x)
					   + 
					    (_V_CW_Bend.z * xzOff_22.y)
					  ) * 0.001));
					  highp vec2 tmpvar_32;
					  tmpvar_32.x = float((tmpvar_28.z >= 0.0));
					  tmpvar_32.y = float((tmpvar_28.x >= 0.0));
					  xzOff_22 = (max (vec2(0.0, 0.0), (
					    abs(tmpvar_28.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_32 * 2.0) - 1.0));
					  xzOff_22 = (xzOff_22 * xzOff_22);
					  v2_23.y = (tmpvar_28.y + ((
					    (_V_CW_Bend.x * xzOff_22.x)
					   + 
					    (_V_CW_Bend.z * xzOff_22.y)
					  ) * 0.001));
					  highp mat3 tmpvar_33;
					  tmpvar_33[0] = _World2Object[0].xyz;
					  tmpvar_33[1] = _World2Object[1].xyz;
					  tmpvar_33[2] = _World2Object[2].xyz;
					  vertex_21.xyz = (_glesVertex.xyz + (tmpvar_33 * tmpvar_30));
					  highp mat3 tmpvar_34;
					  tmpvar_34[0] = _World2Object[0].xyz;
					  tmpvar_34[1] = _World2Object[1].xyz;
					  tmpvar_34[2] = _World2Object[2].xyz;
					  highp vec3 a_35;
					  a_35 = (v2_23 - v0_25);
					  highp vec3 b_36;
					  b_36 = (v1_24 - v0_25);
					  highp vec3 tmpvar_37;
					  tmpvar_37 = normalize((tmpvar_34 * normalize(
					    ((a_35.yzx * b_36.zxy) - (a_35.zxy * b_36.yzx))
					  )));
					  highp vec4 v_38;
					  v_38.x = tmpvar_4;
					  v_38.y = tmpvar_5;
					  v_38.z = tmpvar_6;
					  v_38.w = tmpvar_7;
					  highp vec4 v_39;
					  v_39.x = tmpvar_9;
					  v_39.y = tmpvar_10;
					  v_39.z = tmpvar_11;
					  v_39.w = tmpvar_12;
					  highp vec4 v_40;
					  v_40.x = tmpvar_14;
					  v_40.y = tmpvar_15;
					  v_40.z = tmpvar_16;
					  v_40.w = tmpvar_17;
					  highp vec3 tmpvar_41;
					  tmpvar_41 = normalize(((
					    (v_38.xyz * tmpvar_37.x)
					   + 
					    (v_39.xyz * tmpvar_37.y)
					  ) + (v_40.xyz * tmpvar_37.z)));
					  worldNormal_1 = tmpvar_41;
					  tmpvar_2 = worldNormal_1;
					  gl_Position = (glstate_matrix_mvp * vertex_21);
					  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
					  xlv_TEXCOORD1 = tmpvar_2;
					  xlv_TEXCOORD2 = (_Object2World * vertex_21).xyz;
					  xlv_COLOR0 = _glesColor;
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform highp vec4 _Time;
					uniform highp vec3 _WorldSpaceCameraPos;
					uniform highp vec4 _WorldSpaceLightPos0;
					uniform lowp vec4 _LightColor0;
					uniform lowp vec4 _SpecColor;
					uniform sampler2D _LightTexture0;
					uniform mediump mat4 _LightMatrix0;
					uniform sampler2D _MainTex;
					uniform lowp vec4 _Color;
					uniform lowp vec2 _V_CW_MainTex_Scroll;
					uniform mediump float _Shininess;
					varying highp vec2 xlv_TEXCOORD0;
					varying mediump vec3 xlv_TEXCOORD1;
					varying highp vec3 xlv_TEXCOORD2;
					varying lowp vec4 xlv_COLOR0;
					void main ()
					{
					  mediump vec3 tmpvar_1;
					  mediump vec3 tmpvar_2;
					  lowp vec3 tmpvar_3;
					  lowp vec3 worldViewDir_4;
					  lowp vec3 lightDir_5;
					  highp vec4 tmpvar_6;
					  highp vec3 tmpvar_7;
					  tmpvar_7 = normalize((_WorldSpaceLightPos0.xyz - xlv_TEXCOORD2));
					  lightDir_5 = tmpvar_7;
					  highp vec3 tmpvar_8;
					  tmpvar_8 = normalize((_WorldSpaceCameraPos - xlv_TEXCOORD2));
					  worldViewDir_4 = tmpvar_8;
					  tmpvar_6 = xlv_COLOR0;
					  tmpvar_3 = xlv_TEXCOORD1;
					  lowp vec3 tmpvar_9;
					  lowp vec4 tmpvar_10;
					  highp vec2 P_11;
					  P_11 = (xlv_TEXCOORD0 + (_V_CW_MainTex_Scroll * _Time.x));
					  tmpvar_10 = texture2D (_MainTex, P_11);
					  tmpvar_9 = (tmpvar_10.xyz * _Color.xyz);
					  tmpvar_9 = (tmpvar_9 * tmpvar_6.xyz);
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
					  tmpvar_2 = lightDir_5;
					  tmpvar_1 = (tmpvar_1 * tmpvar_15);
					  mediump vec3 viewDir_16;
					  viewDir_16 = worldViewDir_4;
					  lowp vec4 c_17;
					  lowp vec4 c_18;
					  highp float nh_19;
					  lowp float diff_20;
					  mediump float tmpvar_21;
					  tmpvar_21 = max (0.0, dot (tmpvar_3, tmpvar_2));
					  diff_20 = tmpvar_21;
					  mediump float tmpvar_22;
					  tmpvar_22 = max (0.0, dot (tmpvar_3, normalize(
					    (tmpvar_2 + viewDir_16)
					  )));
					  nh_19 = tmpvar_22;
					  mediump float y_23;
					  y_23 = (_Shininess * 128.0);
					  highp float tmpvar_24;
					  tmpvar_24 = (pow (nh_19, y_23) * tmpvar_10.w);
					  c_18.xyz = (((tmpvar_9 * tmpvar_1) * diff_20) + ((tmpvar_1 * _SpecColor.xyz) * tmpvar_24));
					  c_18.w = (tmpvar_10.w * _Color.w);
					  c_17.w = c_18.w;
					  c_17.xyz = c_18.xyz;
					  gl_FragData[0] = c_17;
					}
					
					
					#endif"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "V_CW_SPECULAR" "V_CW_VERTEX_COLOR" "V_CW_REFLECTIVE_OFF" "_EMISSION_OFF" "V_CW_RIM_OFF" "_NORMALMAP_OFF" }
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
					varying highp vec2 xlv_TEXCOORD0;
					varying mediump vec3 xlv_TEXCOORD1;
					varying highp vec3 xlv_TEXCOORD2;
					varying lowp vec4 xlv_COLOR0;
					void main ()
					{
					  lowp vec3 worldNormal_1;
					  mediump vec3 tmpvar_2;
					  highp vec4 v_3;
					  highp float tmpvar_4;
					  tmpvar_4 = _World2Object[0].x;
					  v_3.x = tmpvar_4;
					  highp float tmpvar_5;
					  tmpvar_5 = _World2Object[1].x;
					  v_3.y = tmpvar_5;
					  highp float tmpvar_6;
					  tmpvar_6 = _World2Object[2].x;
					  v_3.z = tmpvar_6;
					  highp float tmpvar_7;
					  tmpvar_7 = _World2Object[3].x;
					  v_3.w = tmpvar_7;
					  highp vec4 v_8;
					  highp float tmpvar_9;
					  tmpvar_9 = _World2Object[0].y;
					  v_8.x = tmpvar_9;
					  highp float tmpvar_10;
					  tmpvar_10 = _World2Object[1].y;
					  v_8.y = tmpvar_10;
					  highp float tmpvar_11;
					  tmpvar_11 = _World2Object[2].y;
					  v_8.z = tmpvar_11;
					  highp float tmpvar_12;
					  tmpvar_12 = _World2Object[3].y;
					  v_8.w = tmpvar_12;
					  highp vec4 v_13;
					  highp float tmpvar_14;
					  tmpvar_14 = _World2Object[0].z;
					  v_13.x = tmpvar_14;
					  highp float tmpvar_15;
					  tmpvar_15 = _World2Object[1].z;
					  v_13.y = tmpvar_15;
					  highp float tmpvar_16;
					  tmpvar_16 = _World2Object[2].z;
					  v_13.z = tmpvar_16;
					  highp float tmpvar_17;
					  tmpvar_17 = _World2Object[3].z;
					  v_13.w = tmpvar_17;
					  highp vec3 tmpvar_18;
					  tmpvar_18 = normalize(((
					    (v_3.xyz * _glesNormal.x)
					   + 
					    (v_8.xyz * _glesNormal.y)
					  ) + (v_13.xyz * _glesNormal.z)));
					  highp mat3 tmpvar_19;
					  tmpvar_19[0] = _Object2World[0].xyz;
					  tmpvar_19[1] = _Object2World[1].xyz;
					  tmpvar_19[2] = _Object2World[2].xyz;
					  highp vec3 tmpvar_20;
					  tmpvar_20 = normalize((tmpvar_19 * _glesTANGENT.xyz));
					  highp vec4 vertex_21;
					  vertex_21.w = _glesVertex.w;
					  highp vec2 xzOff_22;
					  highp vec3 v2_23;
					  highp vec3 v1_24;
					  highp vec3 v0_25;
					  highp vec3 tmpvar_26;
					  tmpvar_26 = ((_Object2World * _glesVertex).xyz - _V_CW_PivotPoint_Position.xyz);
					  highp vec3 tmpvar_27;
					  tmpvar_27 = (tmpvar_26 + tmpvar_20);
					  v1_24.xz = tmpvar_27.xz;
					  highp vec3 tmpvar_28;
					  tmpvar_28 = (tmpvar_26 - ((tmpvar_18.yzx * tmpvar_20.zxy) - (tmpvar_18.zxy * tmpvar_20.yzx)));
					  v2_23.xz = tmpvar_28.xz;
					  highp vec2 tmpvar_29;
					  tmpvar_29.x = float((tmpvar_26.z >= 0.0));
					  tmpvar_29.y = float((tmpvar_26.x >= 0.0));
					  xzOff_22 = (max (vec2(0.0, 0.0), (
					    abs(tmpvar_26.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_29 * 2.0) - 1.0));
					  xzOff_22 = (xzOff_22 * xzOff_22);
					  highp vec3 tmpvar_30;
					  tmpvar_30.xz = vec2(0.0, 0.0);
					  tmpvar_30.y = (((_V_CW_Bend.x * xzOff_22.x) + (_V_CW_Bend.z * xzOff_22.y)) * 0.001);
					  v0_25 = (tmpvar_26 + tmpvar_30);
					  highp vec2 tmpvar_31;
					  tmpvar_31.x = float((tmpvar_27.z >= 0.0));
					  tmpvar_31.y = float((tmpvar_27.x >= 0.0));
					  xzOff_22 = (max (vec2(0.0, 0.0), (
					    abs(tmpvar_27.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_31 * 2.0) - 1.0));
					  xzOff_22 = (xzOff_22 * xzOff_22);
					  v1_24.y = (tmpvar_27.y + ((
					    (_V_CW_Bend.x * xzOff_22.x)
					   + 
					    (_V_CW_Bend.z * xzOff_22.y)
					  ) * 0.001));
					  highp vec2 tmpvar_32;
					  tmpvar_32.x = float((tmpvar_28.z >= 0.0));
					  tmpvar_32.y = float((tmpvar_28.x >= 0.0));
					  xzOff_22 = (max (vec2(0.0, 0.0), (
					    abs(tmpvar_28.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_32 * 2.0) - 1.0));
					  xzOff_22 = (xzOff_22 * xzOff_22);
					  v2_23.y = (tmpvar_28.y + ((
					    (_V_CW_Bend.x * xzOff_22.x)
					   + 
					    (_V_CW_Bend.z * xzOff_22.y)
					  ) * 0.001));
					  highp mat3 tmpvar_33;
					  tmpvar_33[0] = _World2Object[0].xyz;
					  tmpvar_33[1] = _World2Object[1].xyz;
					  tmpvar_33[2] = _World2Object[2].xyz;
					  vertex_21.xyz = (_glesVertex.xyz + (tmpvar_33 * tmpvar_30));
					  highp mat3 tmpvar_34;
					  tmpvar_34[0] = _World2Object[0].xyz;
					  tmpvar_34[1] = _World2Object[1].xyz;
					  tmpvar_34[2] = _World2Object[2].xyz;
					  highp vec3 a_35;
					  a_35 = (v2_23 - v0_25);
					  highp vec3 b_36;
					  b_36 = (v1_24 - v0_25);
					  highp vec3 tmpvar_37;
					  tmpvar_37 = normalize((tmpvar_34 * normalize(
					    ((a_35.yzx * b_36.zxy) - (a_35.zxy * b_36.yzx))
					  )));
					  highp vec4 v_38;
					  v_38.x = tmpvar_4;
					  v_38.y = tmpvar_5;
					  v_38.z = tmpvar_6;
					  v_38.w = tmpvar_7;
					  highp vec4 v_39;
					  v_39.x = tmpvar_9;
					  v_39.y = tmpvar_10;
					  v_39.z = tmpvar_11;
					  v_39.w = tmpvar_12;
					  highp vec4 v_40;
					  v_40.x = tmpvar_14;
					  v_40.y = tmpvar_15;
					  v_40.z = tmpvar_16;
					  v_40.w = tmpvar_17;
					  highp vec3 tmpvar_41;
					  tmpvar_41 = normalize(((
					    (v_38.xyz * tmpvar_37.x)
					   + 
					    (v_39.xyz * tmpvar_37.y)
					  ) + (v_40.xyz * tmpvar_37.z)));
					  worldNormal_1 = tmpvar_41;
					  tmpvar_2 = worldNormal_1;
					  gl_Position = (glstate_matrix_mvp * vertex_21);
					  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
					  xlv_TEXCOORD1 = tmpvar_2;
					  xlv_TEXCOORD2 = (_Object2World * vertex_21).xyz;
					  xlv_COLOR0 = _glesColor;
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform highp vec4 _Time;
					uniform highp vec3 _WorldSpaceCameraPos;
					uniform mediump vec4 _WorldSpaceLightPos0;
					uniform lowp vec4 _LightColor0;
					uniform lowp vec4 _SpecColor;
					uniform sampler2D _MainTex;
					uniform lowp vec4 _Color;
					uniform lowp vec2 _V_CW_MainTex_Scroll;
					uniform mediump float _Shininess;
					varying highp vec2 xlv_TEXCOORD0;
					varying mediump vec3 xlv_TEXCOORD1;
					varying highp vec3 xlv_TEXCOORD2;
					varying lowp vec4 xlv_COLOR0;
					void main ()
					{
					  mediump vec3 tmpvar_1;
					  mediump vec3 tmpvar_2;
					  lowp vec3 tmpvar_3;
					  lowp vec3 worldViewDir_4;
					  lowp vec3 lightDir_5;
					  highp vec4 tmpvar_6;
					  mediump vec3 tmpvar_7;
					  tmpvar_7 = _WorldSpaceLightPos0.xyz;
					  lightDir_5 = tmpvar_7;
					  highp vec3 tmpvar_8;
					  tmpvar_8 = normalize((_WorldSpaceCameraPos - xlv_TEXCOORD2));
					  worldViewDir_4 = tmpvar_8;
					  tmpvar_6 = xlv_COLOR0;
					  tmpvar_3 = xlv_TEXCOORD1;
					  lowp vec3 tmpvar_9;
					  lowp vec4 tmpvar_10;
					  highp vec2 P_11;
					  P_11 = (xlv_TEXCOORD0 + (_V_CW_MainTex_Scroll * _Time.x));
					  tmpvar_10 = texture2D (_MainTex, P_11);
					  tmpvar_9 = (tmpvar_10.xyz * _Color.xyz);
					  tmpvar_9 = (tmpvar_9 * tmpvar_6.xyz);
					  tmpvar_1 = _LightColor0.xyz;
					  tmpvar_2 = lightDir_5;
					  mediump vec3 viewDir_12;
					  viewDir_12 = worldViewDir_4;
					  lowp vec4 c_13;
					  lowp vec4 c_14;
					  highp float nh_15;
					  lowp float diff_16;
					  mediump float tmpvar_17;
					  tmpvar_17 = max (0.0, dot (tmpvar_3, tmpvar_2));
					  diff_16 = tmpvar_17;
					  mediump float tmpvar_18;
					  tmpvar_18 = max (0.0, dot (tmpvar_3, normalize(
					    (tmpvar_2 + viewDir_12)
					  )));
					  nh_15 = tmpvar_18;
					  mediump float y_19;
					  y_19 = (_Shininess * 128.0);
					  highp float tmpvar_20;
					  tmpvar_20 = (pow (nh_15, y_19) * tmpvar_10.w);
					  c_14.xyz = (((tmpvar_9 * tmpvar_1) * diff_16) + ((tmpvar_1 * _SpecColor.xyz) * tmpvar_20));
					  c_14.w = (tmpvar_10.w * _Color.w);
					  c_13.w = c_14.w;
					  c_13.xyz = c_14.xyz;
					  gl_FragData[0] = c_13;
					}
					
					
					#endif"
}
SubProgram "gles " {
Keywords { "SPOT" "V_CW_SPECULAR" "V_CW_VERTEX_COLOR" "V_CW_REFLECTIVE_OFF" "_EMISSION_OFF" "V_CW_RIM_OFF" "_NORMALMAP_OFF" }
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
					varying highp vec2 xlv_TEXCOORD0;
					varying mediump vec3 xlv_TEXCOORD1;
					varying highp vec3 xlv_TEXCOORD2;
					varying lowp vec4 xlv_COLOR0;
					void main ()
					{
					  lowp vec3 worldNormal_1;
					  mediump vec3 tmpvar_2;
					  highp vec4 v_3;
					  highp float tmpvar_4;
					  tmpvar_4 = _World2Object[0].x;
					  v_3.x = tmpvar_4;
					  highp float tmpvar_5;
					  tmpvar_5 = _World2Object[1].x;
					  v_3.y = tmpvar_5;
					  highp float tmpvar_6;
					  tmpvar_6 = _World2Object[2].x;
					  v_3.z = tmpvar_6;
					  highp float tmpvar_7;
					  tmpvar_7 = _World2Object[3].x;
					  v_3.w = tmpvar_7;
					  highp vec4 v_8;
					  highp float tmpvar_9;
					  tmpvar_9 = _World2Object[0].y;
					  v_8.x = tmpvar_9;
					  highp float tmpvar_10;
					  tmpvar_10 = _World2Object[1].y;
					  v_8.y = tmpvar_10;
					  highp float tmpvar_11;
					  tmpvar_11 = _World2Object[2].y;
					  v_8.z = tmpvar_11;
					  highp float tmpvar_12;
					  tmpvar_12 = _World2Object[3].y;
					  v_8.w = tmpvar_12;
					  highp vec4 v_13;
					  highp float tmpvar_14;
					  tmpvar_14 = _World2Object[0].z;
					  v_13.x = tmpvar_14;
					  highp float tmpvar_15;
					  tmpvar_15 = _World2Object[1].z;
					  v_13.y = tmpvar_15;
					  highp float tmpvar_16;
					  tmpvar_16 = _World2Object[2].z;
					  v_13.z = tmpvar_16;
					  highp float tmpvar_17;
					  tmpvar_17 = _World2Object[3].z;
					  v_13.w = tmpvar_17;
					  highp vec3 tmpvar_18;
					  tmpvar_18 = normalize(((
					    (v_3.xyz * _glesNormal.x)
					   + 
					    (v_8.xyz * _glesNormal.y)
					  ) + (v_13.xyz * _glesNormal.z)));
					  highp mat3 tmpvar_19;
					  tmpvar_19[0] = _Object2World[0].xyz;
					  tmpvar_19[1] = _Object2World[1].xyz;
					  tmpvar_19[2] = _Object2World[2].xyz;
					  highp vec3 tmpvar_20;
					  tmpvar_20 = normalize((tmpvar_19 * _glesTANGENT.xyz));
					  highp vec4 vertex_21;
					  vertex_21.w = _glesVertex.w;
					  highp vec2 xzOff_22;
					  highp vec3 v2_23;
					  highp vec3 v1_24;
					  highp vec3 v0_25;
					  highp vec3 tmpvar_26;
					  tmpvar_26 = ((_Object2World * _glesVertex).xyz - _V_CW_PivotPoint_Position.xyz);
					  highp vec3 tmpvar_27;
					  tmpvar_27 = (tmpvar_26 + tmpvar_20);
					  v1_24.xz = tmpvar_27.xz;
					  highp vec3 tmpvar_28;
					  tmpvar_28 = (tmpvar_26 - ((tmpvar_18.yzx * tmpvar_20.zxy) - (tmpvar_18.zxy * tmpvar_20.yzx)));
					  v2_23.xz = tmpvar_28.xz;
					  highp vec2 tmpvar_29;
					  tmpvar_29.x = float((tmpvar_26.z >= 0.0));
					  tmpvar_29.y = float((tmpvar_26.x >= 0.0));
					  xzOff_22 = (max (vec2(0.0, 0.0), (
					    abs(tmpvar_26.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_29 * 2.0) - 1.0));
					  xzOff_22 = (xzOff_22 * xzOff_22);
					  highp vec3 tmpvar_30;
					  tmpvar_30.xz = vec2(0.0, 0.0);
					  tmpvar_30.y = (((_V_CW_Bend.x * xzOff_22.x) + (_V_CW_Bend.z * xzOff_22.y)) * 0.001);
					  v0_25 = (tmpvar_26 + tmpvar_30);
					  highp vec2 tmpvar_31;
					  tmpvar_31.x = float((tmpvar_27.z >= 0.0));
					  tmpvar_31.y = float((tmpvar_27.x >= 0.0));
					  xzOff_22 = (max (vec2(0.0, 0.0), (
					    abs(tmpvar_27.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_31 * 2.0) - 1.0));
					  xzOff_22 = (xzOff_22 * xzOff_22);
					  v1_24.y = (tmpvar_27.y + ((
					    (_V_CW_Bend.x * xzOff_22.x)
					   + 
					    (_V_CW_Bend.z * xzOff_22.y)
					  ) * 0.001));
					  highp vec2 tmpvar_32;
					  tmpvar_32.x = float((tmpvar_28.z >= 0.0));
					  tmpvar_32.y = float((tmpvar_28.x >= 0.0));
					  xzOff_22 = (max (vec2(0.0, 0.0), (
					    abs(tmpvar_28.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_32 * 2.0) - 1.0));
					  xzOff_22 = (xzOff_22 * xzOff_22);
					  v2_23.y = (tmpvar_28.y + ((
					    (_V_CW_Bend.x * xzOff_22.x)
					   + 
					    (_V_CW_Bend.z * xzOff_22.y)
					  ) * 0.001));
					  highp mat3 tmpvar_33;
					  tmpvar_33[0] = _World2Object[0].xyz;
					  tmpvar_33[1] = _World2Object[1].xyz;
					  tmpvar_33[2] = _World2Object[2].xyz;
					  vertex_21.xyz = (_glesVertex.xyz + (tmpvar_33 * tmpvar_30));
					  highp mat3 tmpvar_34;
					  tmpvar_34[0] = _World2Object[0].xyz;
					  tmpvar_34[1] = _World2Object[1].xyz;
					  tmpvar_34[2] = _World2Object[2].xyz;
					  highp vec3 a_35;
					  a_35 = (v2_23 - v0_25);
					  highp vec3 b_36;
					  b_36 = (v1_24 - v0_25);
					  highp vec3 tmpvar_37;
					  tmpvar_37 = normalize((tmpvar_34 * normalize(
					    ((a_35.yzx * b_36.zxy) - (a_35.zxy * b_36.yzx))
					  )));
					  highp vec4 v_38;
					  v_38.x = tmpvar_4;
					  v_38.y = tmpvar_5;
					  v_38.z = tmpvar_6;
					  v_38.w = tmpvar_7;
					  highp vec4 v_39;
					  v_39.x = tmpvar_9;
					  v_39.y = tmpvar_10;
					  v_39.z = tmpvar_11;
					  v_39.w = tmpvar_12;
					  highp vec4 v_40;
					  v_40.x = tmpvar_14;
					  v_40.y = tmpvar_15;
					  v_40.z = tmpvar_16;
					  v_40.w = tmpvar_17;
					  highp vec3 tmpvar_41;
					  tmpvar_41 = normalize(((
					    (v_38.xyz * tmpvar_37.x)
					   + 
					    (v_39.xyz * tmpvar_37.y)
					  ) + (v_40.xyz * tmpvar_37.z)));
					  worldNormal_1 = tmpvar_41;
					  tmpvar_2 = worldNormal_1;
					  gl_Position = (glstate_matrix_mvp * vertex_21);
					  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
					  xlv_TEXCOORD1 = tmpvar_2;
					  xlv_TEXCOORD2 = (_Object2World * vertex_21).xyz;
					  xlv_COLOR0 = _glesColor;
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform highp vec4 _Time;
					uniform highp vec3 _WorldSpaceCameraPos;
					uniform highp vec4 _WorldSpaceLightPos0;
					uniform lowp vec4 _LightColor0;
					uniform lowp vec4 _SpecColor;
					uniform sampler2D _LightTexture0;
					uniform mediump mat4 _LightMatrix0;
					uniform sampler2D _LightTextureB0;
					uniform sampler2D _MainTex;
					uniform lowp vec4 _Color;
					uniform lowp vec2 _V_CW_MainTex_Scroll;
					uniform mediump float _Shininess;
					varying highp vec2 xlv_TEXCOORD0;
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
					  lowp vec3 worldViewDir_6;
					  lowp vec3 lightDir_7;
					  highp vec4 tmpvar_8;
					  highp vec3 tmpvar_9;
					  tmpvar_9 = normalize((_WorldSpaceLightPos0.xyz - xlv_TEXCOORD2));
					  lightDir_7 = tmpvar_9;
					  highp vec3 tmpvar_10;
					  tmpvar_10 = normalize((_WorldSpaceCameraPos - xlv_TEXCOORD2));
					  worldViewDir_6 = tmpvar_10;
					  tmpvar_8 = xlv_COLOR0;
					  tmpvar_5 = xlv_TEXCOORD1;
					  lowp vec3 tmpvar_11;
					  lowp vec4 tmpvar_12;
					  highp vec2 P_13;
					  P_13 = (xlv_TEXCOORD0 + (_V_CW_MainTex_Scroll * _Time.x));
					  tmpvar_12 = texture2D (_MainTex, P_13);
					  tmpvar_11 = (tmpvar_12.xyz * _Color.xyz);
					  tmpvar_11 = (tmpvar_11 * tmpvar_8.xyz);
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
					  tmpvar_2 = lightDir_7;
					  tmpvar_1 = (tmpvar_1 * atten_3);
					  mediump vec3 viewDir_22;
					  viewDir_22 = worldViewDir_6;
					  lowp vec4 c_23;
					  lowp vec4 c_24;
					  highp float nh_25;
					  lowp float diff_26;
					  mediump float tmpvar_27;
					  tmpvar_27 = max (0.0, dot (tmpvar_5, tmpvar_2));
					  diff_26 = tmpvar_27;
					  mediump float tmpvar_28;
					  tmpvar_28 = max (0.0, dot (tmpvar_5, normalize(
					    (tmpvar_2 + viewDir_22)
					  )));
					  nh_25 = tmpvar_28;
					  mediump float y_29;
					  y_29 = (_Shininess * 128.0);
					  highp float tmpvar_30;
					  tmpvar_30 = (pow (nh_25, y_29) * tmpvar_12.w);
					  c_24.xyz = (((tmpvar_11 * tmpvar_1) * diff_26) + ((tmpvar_1 * _SpecColor.xyz) * tmpvar_30));
					  c_24.w = (tmpvar_12.w * _Color.w);
					  c_23.w = c_24.w;
					  c_23.xyz = c_24.xyz;
					  gl_FragData[0] = c_23;
					}
					
					
					#endif"
}
SubProgram "gles " {
Keywords { "POINT_COOKIE" "V_CW_SPECULAR" "V_CW_VERTEX_COLOR" "V_CW_REFLECTIVE_OFF" "_EMISSION_OFF" "V_CW_RIM_OFF" "_NORMALMAP_OFF" }
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
					varying highp vec2 xlv_TEXCOORD0;
					varying mediump vec3 xlv_TEXCOORD1;
					varying highp vec3 xlv_TEXCOORD2;
					varying lowp vec4 xlv_COLOR0;
					void main ()
					{
					  lowp vec3 worldNormal_1;
					  mediump vec3 tmpvar_2;
					  highp vec4 v_3;
					  highp float tmpvar_4;
					  tmpvar_4 = _World2Object[0].x;
					  v_3.x = tmpvar_4;
					  highp float tmpvar_5;
					  tmpvar_5 = _World2Object[1].x;
					  v_3.y = tmpvar_5;
					  highp float tmpvar_6;
					  tmpvar_6 = _World2Object[2].x;
					  v_3.z = tmpvar_6;
					  highp float tmpvar_7;
					  tmpvar_7 = _World2Object[3].x;
					  v_3.w = tmpvar_7;
					  highp vec4 v_8;
					  highp float tmpvar_9;
					  tmpvar_9 = _World2Object[0].y;
					  v_8.x = tmpvar_9;
					  highp float tmpvar_10;
					  tmpvar_10 = _World2Object[1].y;
					  v_8.y = tmpvar_10;
					  highp float tmpvar_11;
					  tmpvar_11 = _World2Object[2].y;
					  v_8.z = tmpvar_11;
					  highp float tmpvar_12;
					  tmpvar_12 = _World2Object[3].y;
					  v_8.w = tmpvar_12;
					  highp vec4 v_13;
					  highp float tmpvar_14;
					  tmpvar_14 = _World2Object[0].z;
					  v_13.x = tmpvar_14;
					  highp float tmpvar_15;
					  tmpvar_15 = _World2Object[1].z;
					  v_13.y = tmpvar_15;
					  highp float tmpvar_16;
					  tmpvar_16 = _World2Object[2].z;
					  v_13.z = tmpvar_16;
					  highp float tmpvar_17;
					  tmpvar_17 = _World2Object[3].z;
					  v_13.w = tmpvar_17;
					  highp vec3 tmpvar_18;
					  tmpvar_18 = normalize(((
					    (v_3.xyz * _glesNormal.x)
					   + 
					    (v_8.xyz * _glesNormal.y)
					  ) + (v_13.xyz * _glesNormal.z)));
					  highp mat3 tmpvar_19;
					  tmpvar_19[0] = _Object2World[0].xyz;
					  tmpvar_19[1] = _Object2World[1].xyz;
					  tmpvar_19[2] = _Object2World[2].xyz;
					  highp vec3 tmpvar_20;
					  tmpvar_20 = normalize((tmpvar_19 * _glesTANGENT.xyz));
					  highp vec4 vertex_21;
					  vertex_21.w = _glesVertex.w;
					  highp vec2 xzOff_22;
					  highp vec3 v2_23;
					  highp vec3 v1_24;
					  highp vec3 v0_25;
					  highp vec3 tmpvar_26;
					  tmpvar_26 = ((_Object2World * _glesVertex).xyz - _V_CW_PivotPoint_Position.xyz);
					  highp vec3 tmpvar_27;
					  tmpvar_27 = (tmpvar_26 + tmpvar_20);
					  v1_24.xz = tmpvar_27.xz;
					  highp vec3 tmpvar_28;
					  tmpvar_28 = (tmpvar_26 - ((tmpvar_18.yzx * tmpvar_20.zxy) - (tmpvar_18.zxy * tmpvar_20.yzx)));
					  v2_23.xz = tmpvar_28.xz;
					  highp vec2 tmpvar_29;
					  tmpvar_29.x = float((tmpvar_26.z >= 0.0));
					  tmpvar_29.y = float((tmpvar_26.x >= 0.0));
					  xzOff_22 = (max (vec2(0.0, 0.0), (
					    abs(tmpvar_26.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_29 * 2.0) - 1.0));
					  xzOff_22 = (xzOff_22 * xzOff_22);
					  highp vec3 tmpvar_30;
					  tmpvar_30.xz = vec2(0.0, 0.0);
					  tmpvar_30.y = (((_V_CW_Bend.x * xzOff_22.x) + (_V_CW_Bend.z * xzOff_22.y)) * 0.001);
					  v0_25 = (tmpvar_26 + tmpvar_30);
					  highp vec2 tmpvar_31;
					  tmpvar_31.x = float((tmpvar_27.z >= 0.0));
					  tmpvar_31.y = float((tmpvar_27.x >= 0.0));
					  xzOff_22 = (max (vec2(0.0, 0.0), (
					    abs(tmpvar_27.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_31 * 2.0) - 1.0));
					  xzOff_22 = (xzOff_22 * xzOff_22);
					  v1_24.y = (tmpvar_27.y + ((
					    (_V_CW_Bend.x * xzOff_22.x)
					   + 
					    (_V_CW_Bend.z * xzOff_22.y)
					  ) * 0.001));
					  highp vec2 tmpvar_32;
					  tmpvar_32.x = float((tmpvar_28.z >= 0.0));
					  tmpvar_32.y = float((tmpvar_28.x >= 0.0));
					  xzOff_22 = (max (vec2(0.0, 0.0), (
					    abs(tmpvar_28.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_32 * 2.0) - 1.0));
					  xzOff_22 = (xzOff_22 * xzOff_22);
					  v2_23.y = (tmpvar_28.y + ((
					    (_V_CW_Bend.x * xzOff_22.x)
					   + 
					    (_V_CW_Bend.z * xzOff_22.y)
					  ) * 0.001));
					  highp mat3 tmpvar_33;
					  tmpvar_33[0] = _World2Object[0].xyz;
					  tmpvar_33[1] = _World2Object[1].xyz;
					  tmpvar_33[2] = _World2Object[2].xyz;
					  vertex_21.xyz = (_glesVertex.xyz + (tmpvar_33 * tmpvar_30));
					  highp mat3 tmpvar_34;
					  tmpvar_34[0] = _World2Object[0].xyz;
					  tmpvar_34[1] = _World2Object[1].xyz;
					  tmpvar_34[2] = _World2Object[2].xyz;
					  highp vec3 a_35;
					  a_35 = (v2_23 - v0_25);
					  highp vec3 b_36;
					  b_36 = (v1_24 - v0_25);
					  highp vec3 tmpvar_37;
					  tmpvar_37 = normalize((tmpvar_34 * normalize(
					    ((a_35.yzx * b_36.zxy) - (a_35.zxy * b_36.yzx))
					  )));
					  highp vec4 v_38;
					  v_38.x = tmpvar_4;
					  v_38.y = tmpvar_5;
					  v_38.z = tmpvar_6;
					  v_38.w = tmpvar_7;
					  highp vec4 v_39;
					  v_39.x = tmpvar_9;
					  v_39.y = tmpvar_10;
					  v_39.z = tmpvar_11;
					  v_39.w = tmpvar_12;
					  highp vec4 v_40;
					  v_40.x = tmpvar_14;
					  v_40.y = tmpvar_15;
					  v_40.z = tmpvar_16;
					  v_40.w = tmpvar_17;
					  highp vec3 tmpvar_41;
					  tmpvar_41 = normalize(((
					    (v_38.xyz * tmpvar_37.x)
					   + 
					    (v_39.xyz * tmpvar_37.y)
					  ) + (v_40.xyz * tmpvar_37.z)));
					  worldNormal_1 = tmpvar_41;
					  tmpvar_2 = worldNormal_1;
					  gl_Position = (glstate_matrix_mvp * vertex_21);
					  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
					  xlv_TEXCOORD1 = tmpvar_2;
					  xlv_TEXCOORD2 = (_Object2World * vertex_21).xyz;
					  xlv_COLOR0 = _glesColor;
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform highp vec4 _Time;
					uniform highp vec3 _WorldSpaceCameraPos;
					uniform highp vec4 _WorldSpaceLightPos0;
					uniform lowp vec4 _LightColor0;
					uniform lowp vec4 _SpecColor;
					uniform lowp samplerCube _LightTexture0;
					uniform mediump mat4 _LightMatrix0;
					uniform sampler2D _LightTextureB0;
					uniform sampler2D _MainTex;
					uniform lowp vec4 _Color;
					uniform lowp vec2 _V_CW_MainTex_Scroll;
					uniform mediump float _Shininess;
					varying highp vec2 xlv_TEXCOORD0;
					varying mediump vec3 xlv_TEXCOORD1;
					varying highp vec3 xlv_TEXCOORD2;
					varying lowp vec4 xlv_COLOR0;
					void main ()
					{
					  mediump vec3 tmpvar_1;
					  mediump vec3 tmpvar_2;
					  lowp vec3 tmpvar_3;
					  lowp vec3 worldViewDir_4;
					  lowp vec3 lightDir_5;
					  highp vec4 tmpvar_6;
					  highp vec3 tmpvar_7;
					  tmpvar_7 = normalize((_WorldSpaceLightPos0.xyz - xlv_TEXCOORD2));
					  lightDir_5 = tmpvar_7;
					  highp vec3 tmpvar_8;
					  tmpvar_8 = normalize((_WorldSpaceCameraPos - xlv_TEXCOORD2));
					  worldViewDir_4 = tmpvar_8;
					  tmpvar_6 = xlv_COLOR0;
					  tmpvar_3 = xlv_TEXCOORD1;
					  lowp vec3 tmpvar_9;
					  lowp vec4 tmpvar_10;
					  highp vec2 P_11;
					  P_11 = (xlv_TEXCOORD0 + (_V_CW_MainTex_Scroll * _Time.x));
					  tmpvar_10 = texture2D (_MainTex, P_11);
					  tmpvar_9 = (tmpvar_10.xyz * _Color.xyz);
					  tmpvar_9 = (tmpvar_9 * tmpvar_6.xyz);
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
					  tmpvar_2 = lightDir_5;
					  tmpvar_1 = (tmpvar_1 * tmpvar_15);
					  mediump vec3 viewDir_16;
					  viewDir_16 = worldViewDir_4;
					  lowp vec4 c_17;
					  lowp vec4 c_18;
					  highp float nh_19;
					  lowp float diff_20;
					  mediump float tmpvar_21;
					  tmpvar_21 = max (0.0, dot (tmpvar_3, tmpvar_2));
					  diff_20 = tmpvar_21;
					  mediump float tmpvar_22;
					  tmpvar_22 = max (0.0, dot (tmpvar_3, normalize(
					    (tmpvar_2 + viewDir_16)
					  )));
					  nh_19 = tmpvar_22;
					  mediump float y_23;
					  y_23 = (_Shininess * 128.0);
					  highp float tmpvar_24;
					  tmpvar_24 = (pow (nh_19, y_23) * tmpvar_10.w);
					  c_18.xyz = (((tmpvar_9 * tmpvar_1) * diff_20) + ((tmpvar_1 * _SpecColor.xyz) * tmpvar_24));
					  c_18.w = (tmpvar_10.w * _Color.w);
					  c_17.w = c_18.w;
					  c_17.xyz = c_18.xyz;
					  gl_FragData[0] = c_17;
					}
					
					
					#endif"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL_COOKIE" "V_CW_SPECULAR" "V_CW_VERTEX_COLOR" "V_CW_REFLECTIVE_OFF" "_EMISSION_OFF" "V_CW_RIM_OFF" "_NORMALMAP_OFF" }
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
					varying highp vec2 xlv_TEXCOORD0;
					varying mediump vec3 xlv_TEXCOORD1;
					varying highp vec3 xlv_TEXCOORD2;
					varying lowp vec4 xlv_COLOR0;
					void main ()
					{
					  lowp vec3 worldNormal_1;
					  mediump vec3 tmpvar_2;
					  highp vec4 v_3;
					  highp float tmpvar_4;
					  tmpvar_4 = _World2Object[0].x;
					  v_3.x = tmpvar_4;
					  highp float tmpvar_5;
					  tmpvar_5 = _World2Object[1].x;
					  v_3.y = tmpvar_5;
					  highp float tmpvar_6;
					  tmpvar_6 = _World2Object[2].x;
					  v_3.z = tmpvar_6;
					  highp float tmpvar_7;
					  tmpvar_7 = _World2Object[3].x;
					  v_3.w = tmpvar_7;
					  highp vec4 v_8;
					  highp float tmpvar_9;
					  tmpvar_9 = _World2Object[0].y;
					  v_8.x = tmpvar_9;
					  highp float tmpvar_10;
					  tmpvar_10 = _World2Object[1].y;
					  v_8.y = tmpvar_10;
					  highp float tmpvar_11;
					  tmpvar_11 = _World2Object[2].y;
					  v_8.z = tmpvar_11;
					  highp float tmpvar_12;
					  tmpvar_12 = _World2Object[3].y;
					  v_8.w = tmpvar_12;
					  highp vec4 v_13;
					  highp float tmpvar_14;
					  tmpvar_14 = _World2Object[0].z;
					  v_13.x = tmpvar_14;
					  highp float tmpvar_15;
					  tmpvar_15 = _World2Object[1].z;
					  v_13.y = tmpvar_15;
					  highp float tmpvar_16;
					  tmpvar_16 = _World2Object[2].z;
					  v_13.z = tmpvar_16;
					  highp float tmpvar_17;
					  tmpvar_17 = _World2Object[3].z;
					  v_13.w = tmpvar_17;
					  highp vec3 tmpvar_18;
					  tmpvar_18 = normalize(((
					    (v_3.xyz * _glesNormal.x)
					   + 
					    (v_8.xyz * _glesNormal.y)
					  ) + (v_13.xyz * _glesNormal.z)));
					  highp mat3 tmpvar_19;
					  tmpvar_19[0] = _Object2World[0].xyz;
					  tmpvar_19[1] = _Object2World[1].xyz;
					  tmpvar_19[2] = _Object2World[2].xyz;
					  highp vec3 tmpvar_20;
					  tmpvar_20 = normalize((tmpvar_19 * _glesTANGENT.xyz));
					  highp vec4 vertex_21;
					  vertex_21.w = _glesVertex.w;
					  highp vec2 xzOff_22;
					  highp vec3 v2_23;
					  highp vec3 v1_24;
					  highp vec3 v0_25;
					  highp vec3 tmpvar_26;
					  tmpvar_26 = ((_Object2World * _glesVertex).xyz - _V_CW_PivotPoint_Position.xyz);
					  highp vec3 tmpvar_27;
					  tmpvar_27 = (tmpvar_26 + tmpvar_20);
					  v1_24.xz = tmpvar_27.xz;
					  highp vec3 tmpvar_28;
					  tmpvar_28 = (tmpvar_26 - ((tmpvar_18.yzx * tmpvar_20.zxy) - (tmpvar_18.zxy * tmpvar_20.yzx)));
					  v2_23.xz = tmpvar_28.xz;
					  highp vec2 tmpvar_29;
					  tmpvar_29.x = float((tmpvar_26.z >= 0.0));
					  tmpvar_29.y = float((tmpvar_26.x >= 0.0));
					  xzOff_22 = (max (vec2(0.0, 0.0), (
					    abs(tmpvar_26.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_29 * 2.0) - 1.0));
					  xzOff_22 = (xzOff_22 * xzOff_22);
					  highp vec3 tmpvar_30;
					  tmpvar_30.xz = vec2(0.0, 0.0);
					  tmpvar_30.y = (((_V_CW_Bend.x * xzOff_22.x) + (_V_CW_Bend.z * xzOff_22.y)) * 0.001);
					  v0_25 = (tmpvar_26 + tmpvar_30);
					  highp vec2 tmpvar_31;
					  tmpvar_31.x = float((tmpvar_27.z >= 0.0));
					  tmpvar_31.y = float((tmpvar_27.x >= 0.0));
					  xzOff_22 = (max (vec2(0.0, 0.0), (
					    abs(tmpvar_27.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_31 * 2.0) - 1.0));
					  xzOff_22 = (xzOff_22 * xzOff_22);
					  v1_24.y = (tmpvar_27.y + ((
					    (_V_CW_Bend.x * xzOff_22.x)
					   + 
					    (_V_CW_Bend.z * xzOff_22.y)
					  ) * 0.001));
					  highp vec2 tmpvar_32;
					  tmpvar_32.x = float((tmpvar_28.z >= 0.0));
					  tmpvar_32.y = float((tmpvar_28.x >= 0.0));
					  xzOff_22 = (max (vec2(0.0, 0.0), (
					    abs(tmpvar_28.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_32 * 2.0) - 1.0));
					  xzOff_22 = (xzOff_22 * xzOff_22);
					  v2_23.y = (tmpvar_28.y + ((
					    (_V_CW_Bend.x * xzOff_22.x)
					   + 
					    (_V_CW_Bend.z * xzOff_22.y)
					  ) * 0.001));
					  highp mat3 tmpvar_33;
					  tmpvar_33[0] = _World2Object[0].xyz;
					  tmpvar_33[1] = _World2Object[1].xyz;
					  tmpvar_33[2] = _World2Object[2].xyz;
					  vertex_21.xyz = (_glesVertex.xyz + (tmpvar_33 * tmpvar_30));
					  highp mat3 tmpvar_34;
					  tmpvar_34[0] = _World2Object[0].xyz;
					  tmpvar_34[1] = _World2Object[1].xyz;
					  tmpvar_34[2] = _World2Object[2].xyz;
					  highp vec3 a_35;
					  a_35 = (v2_23 - v0_25);
					  highp vec3 b_36;
					  b_36 = (v1_24 - v0_25);
					  highp vec3 tmpvar_37;
					  tmpvar_37 = normalize((tmpvar_34 * normalize(
					    ((a_35.yzx * b_36.zxy) - (a_35.zxy * b_36.yzx))
					  )));
					  highp vec4 v_38;
					  v_38.x = tmpvar_4;
					  v_38.y = tmpvar_5;
					  v_38.z = tmpvar_6;
					  v_38.w = tmpvar_7;
					  highp vec4 v_39;
					  v_39.x = tmpvar_9;
					  v_39.y = tmpvar_10;
					  v_39.z = tmpvar_11;
					  v_39.w = tmpvar_12;
					  highp vec4 v_40;
					  v_40.x = tmpvar_14;
					  v_40.y = tmpvar_15;
					  v_40.z = tmpvar_16;
					  v_40.w = tmpvar_17;
					  highp vec3 tmpvar_41;
					  tmpvar_41 = normalize(((
					    (v_38.xyz * tmpvar_37.x)
					   + 
					    (v_39.xyz * tmpvar_37.y)
					  ) + (v_40.xyz * tmpvar_37.z)));
					  worldNormal_1 = tmpvar_41;
					  tmpvar_2 = worldNormal_1;
					  gl_Position = (glstate_matrix_mvp * vertex_21);
					  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
					  xlv_TEXCOORD1 = tmpvar_2;
					  xlv_TEXCOORD2 = (_Object2World * vertex_21).xyz;
					  xlv_COLOR0 = _glesColor;
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform highp vec4 _Time;
					uniform highp vec3 _WorldSpaceCameraPos;
					uniform mediump vec4 _WorldSpaceLightPos0;
					uniform lowp vec4 _LightColor0;
					uniform lowp vec4 _SpecColor;
					uniform sampler2D _LightTexture0;
					uniform mediump mat4 _LightMatrix0;
					uniform sampler2D _MainTex;
					uniform lowp vec4 _Color;
					uniform lowp vec2 _V_CW_MainTex_Scroll;
					uniform mediump float _Shininess;
					varying highp vec2 xlv_TEXCOORD0;
					varying mediump vec3 xlv_TEXCOORD1;
					varying highp vec3 xlv_TEXCOORD2;
					varying lowp vec4 xlv_COLOR0;
					void main ()
					{
					  mediump vec3 tmpvar_1;
					  mediump vec3 tmpvar_2;
					  lowp vec3 tmpvar_3;
					  lowp vec3 worldViewDir_4;
					  lowp vec3 lightDir_5;
					  highp vec4 tmpvar_6;
					  mediump vec3 tmpvar_7;
					  tmpvar_7 = _WorldSpaceLightPos0.xyz;
					  lightDir_5 = tmpvar_7;
					  highp vec3 tmpvar_8;
					  tmpvar_8 = normalize((_WorldSpaceCameraPos - xlv_TEXCOORD2));
					  worldViewDir_4 = tmpvar_8;
					  tmpvar_6 = xlv_COLOR0;
					  tmpvar_3 = xlv_TEXCOORD1;
					  lowp vec3 tmpvar_9;
					  lowp vec4 tmpvar_10;
					  highp vec2 P_11;
					  P_11 = (xlv_TEXCOORD0 + (_V_CW_MainTex_Scroll * _Time.x));
					  tmpvar_10 = texture2D (_MainTex, P_11);
					  tmpvar_9 = (tmpvar_10.xyz * _Color.xyz);
					  tmpvar_9 = (tmpvar_9 * tmpvar_6.xyz);
					  highp vec4 tmpvar_12;
					  tmpvar_12.w = 1.0;
					  tmpvar_12.xyz = xlv_TEXCOORD2;
					  highp vec2 tmpvar_13;
					  tmpvar_13 = (_LightMatrix0 * tmpvar_12).xy;
					  lowp float tmpvar_14;
					  tmpvar_14 = texture2D (_LightTexture0, tmpvar_13).w;
					  tmpvar_1 = _LightColor0.xyz;
					  tmpvar_2 = lightDir_5;
					  tmpvar_1 = (tmpvar_1 * tmpvar_14);
					  mediump vec3 viewDir_15;
					  viewDir_15 = worldViewDir_4;
					  lowp vec4 c_16;
					  lowp vec4 c_17;
					  highp float nh_18;
					  lowp float diff_19;
					  mediump float tmpvar_20;
					  tmpvar_20 = max (0.0, dot (tmpvar_3, tmpvar_2));
					  diff_19 = tmpvar_20;
					  mediump float tmpvar_21;
					  tmpvar_21 = max (0.0, dot (tmpvar_3, normalize(
					    (tmpvar_2 + viewDir_15)
					  )));
					  nh_18 = tmpvar_21;
					  mediump float y_22;
					  y_22 = (_Shininess * 128.0);
					  highp float tmpvar_23;
					  tmpvar_23 = (pow (nh_18, y_22) * tmpvar_10.w);
					  c_17.xyz = (((tmpvar_9 * tmpvar_1) * diff_19) + ((tmpvar_1 * _SpecColor.xyz) * tmpvar_23));
					  c_17.w = (tmpvar_10.w * _Color.w);
					  c_16.w = c_17.w;
					  c_16.xyz = c_17.xyz;
					  gl_FragData[0] = c_16;
					}
					
					
					#endif"
}
SubProgram "gles " {
Keywords { "POINT" "FOG_LINEAR" "V_CW_SPECULAR" "V_CW_VERTEX_COLOR" "V_CW_REFLECTIVE_OFF" "_EMISSION_OFF" "V_CW_RIM_OFF" "_NORMALMAP_OFF" }
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
					varying highp vec2 xlv_TEXCOORD0;
					varying mediump vec3 xlv_TEXCOORD1;
					varying highp vec3 xlv_TEXCOORD2;
					varying lowp vec4 xlv_COLOR0;
					varying highp float xlv_TEXCOORD3;
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
					  tmpvar_2 = (glstate_matrix_mvp * vertex_22);
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
					  gl_Position = tmpvar_2;
					  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
					  xlv_TEXCOORD1 = tmpvar_3;
					  xlv_TEXCOORD2 = (_Object2World * vertex_22).xyz;
					  xlv_COLOR0 = _glesColor;
					  xlv_TEXCOORD3 = ((tmpvar_2.z * unity_FogParams.z) + unity_FogParams.w);
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform highp vec4 _Time;
					uniform highp vec3 _WorldSpaceCameraPos;
					uniform highp vec4 _WorldSpaceLightPos0;
					uniform lowp vec4 _LightColor0;
					uniform lowp vec4 _SpecColor;
					uniform sampler2D _LightTexture0;
					uniform mediump mat4 _LightMatrix0;
					uniform sampler2D _MainTex;
					uniform lowp vec4 _Color;
					uniform lowp vec2 _V_CW_MainTex_Scroll;
					uniform mediump float _Shininess;
					varying highp vec2 xlv_TEXCOORD0;
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
					  lowp vec3 worldViewDir_5;
					  lowp vec3 lightDir_6;
					  highp vec4 tmpvar_7;
					  highp vec3 tmpvar_8;
					  tmpvar_8 = normalize((_WorldSpaceLightPos0.xyz - xlv_TEXCOORD2));
					  lightDir_6 = tmpvar_8;
					  highp vec3 tmpvar_9;
					  tmpvar_9 = normalize((_WorldSpaceCameraPos - xlv_TEXCOORD2));
					  worldViewDir_5 = tmpvar_9;
					  tmpvar_7 = xlv_COLOR0;
					  tmpvar_4 = xlv_TEXCOORD1;
					  lowp vec3 tmpvar_10;
					  lowp vec4 tmpvar_11;
					  highp vec2 P_12;
					  P_12 = (xlv_TEXCOORD0 + (_V_CW_MainTex_Scroll * _Time.x));
					  tmpvar_11 = texture2D (_MainTex, P_12);
					  tmpvar_10 = (tmpvar_11.xyz * _Color.xyz);
					  tmpvar_10 = (tmpvar_10 * tmpvar_7.xyz);
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
					  tmpvar_2 = lightDir_6;
					  tmpvar_1 = (tmpvar_1 * tmpvar_16);
					  mediump vec3 viewDir_17;
					  viewDir_17 = worldViewDir_5;
					  lowp vec4 c_18;
					  lowp vec4 c_19;
					  highp float nh_20;
					  lowp float diff_21;
					  mediump float tmpvar_22;
					  tmpvar_22 = max (0.0, dot (tmpvar_4, tmpvar_2));
					  diff_21 = tmpvar_22;
					  mediump float tmpvar_23;
					  tmpvar_23 = max (0.0, dot (tmpvar_4, normalize(
					    (tmpvar_2 + viewDir_17)
					  )));
					  nh_20 = tmpvar_23;
					  mediump float y_24;
					  y_24 = (_Shininess * 128.0);
					  highp float tmpvar_25;
					  tmpvar_25 = (pow (nh_20, y_24) * tmpvar_11.w);
					  c_19.xyz = (((tmpvar_10 * tmpvar_1) * diff_21) + ((tmpvar_1 * _SpecColor.xyz) * tmpvar_25));
					  c_19.w = (tmpvar_11.w * _Color.w);
					  c_18.w = c_19.w;
					  c_18.xyz = c_19.xyz;
					  c_3.w = c_18.w;
					  highp float tmpvar_26;
					  tmpvar_26 = clamp (xlv_TEXCOORD3, 0.0, 1.0);
					  c_3.xyz = (c_19.xyz * vec3(tmpvar_26));
					  gl_FragData[0] = c_3;
					}
					
					
					#endif"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "FOG_LINEAR" "V_CW_SPECULAR" "V_CW_VERTEX_COLOR" "V_CW_REFLECTIVE_OFF" "_EMISSION_OFF" "V_CW_RIM_OFF" "_NORMALMAP_OFF" }
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
					varying highp vec2 xlv_TEXCOORD0;
					varying mediump vec3 xlv_TEXCOORD1;
					varying highp vec3 xlv_TEXCOORD2;
					varying lowp vec4 xlv_COLOR0;
					varying highp float xlv_TEXCOORD3;
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
					  tmpvar_2 = (glstate_matrix_mvp * vertex_22);
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
					  gl_Position = tmpvar_2;
					  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
					  xlv_TEXCOORD1 = tmpvar_3;
					  xlv_TEXCOORD2 = (_Object2World * vertex_22).xyz;
					  xlv_COLOR0 = _glesColor;
					  xlv_TEXCOORD3 = ((tmpvar_2.z * unity_FogParams.z) + unity_FogParams.w);
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform highp vec4 _Time;
					uniform highp vec3 _WorldSpaceCameraPos;
					uniform mediump vec4 _WorldSpaceLightPos0;
					uniform lowp vec4 _LightColor0;
					uniform lowp vec4 _SpecColor;
					uniform sampler2D _MainTex;
					uniform lowp vec4 _Color;
					uniform lowp vec2 _V_CW_MainTex_Scroll;
					uniform mediump float _Shininess;
					varying highp vec2 xlv_TEXCOORD0;
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
					  lowp vec3 worldViewDir_5;
					  lowp vec3 lightDir_6;
					  highp vec4 tmpvar_7;
					  mediump vec3 tmpvar_8;
					  tmpvar_8 = _WorldSpaceLightPos0.xyz;
					  lightDir_6 = tmpvar_8;
					  highp vec3 tmpvar_9;
					  tmpvar_9 = normalize((_WorldSpaceCameraPos - xlv_TEXCOORD2));
					  worldViewDir_5 = tmpvar_9;
					  tmpvar_7 = xlv_COLOR0;
					  tmpvar_4 = xlv_TEXCOORD1;
					  lowp vec3 tmpvar_10;
					  lowp vec4 tmpvar_11;
					  highp vec2 P_12;
					  P_12 = (xlv_TEXCOORD0 + (_V_CW_MainTex_Scroll * _Time.x));
					  tmpvar_11 = texture2D (_MainTex, P_12);
					  tmpvar_10 = (tmpvar_11.xyz * _Color.xyz);
					  tmpvar_10 = (tmpvar_10 * tmpvar_7.xyz);
					  tmpvar_1 = _LightColor0.xyz;
					  tmpvar_2 = lightDir_6;
					  mediump vec3 viewDir_13;
					  viewDir_13 = worldViewDir_5;
					  lowp vec4 c_14;
					  lowp vec4 c_15;
					  highp float nh_16;
					  lowp float diff_17;
					  mediump float tmpvar_18;
					  tmpvar_18 = max (0.0, dot (tmpvar_4, tmpvar_2));
					  diff_17 = tmpvar_18;
					  mediump float tmpvar_19;
					  tmpvar_19 = max (0.0, dot (tmpvar_4, normalize(
					    (tmpvar_2 + viewDir_13)
					  )));
					  nh_16 = tmpvar_19;
					  mediump float y_20;
					  y_20 = (_Shininess * 128.0);
					  highp float tmpvar_21;
					  tmpvar_21 = (pow (nh_16, y_20) * tmpvar_11.w);
					  c_15.xyz = (((tmpvar_10 * tmpvar_1) * diff_17) + ((tmpvar_1 * _SpecColor.xyz) * tmpvar_21));
					  c_15.w = (tmpvar_11.w * _Color.w);
					  c_14.w = c_15.w;
					  c_14.xyz = c_15.xyz;
					  c_3.w = c_14.w;
					  highp float tmpvar_22;
					  tmpvar_22 = clamp (xlv_TEXCOORD3, 0.0, 1.0);
					  c_3.xyz = (c_15.xyz * vec3(tmpvar_22));
					  gl_FragData[0] = c_3;
					}
					
					
					#endif"
}
SubProgram "gles " {
Keywords { "SPOT" "FOG_LINEAR" "V_CW_SPECULAR" "V_CW_VERTEX_COLOR" "V_CW_REFLECTIVE_OFF" "_EMISSION_OFF" "V_CW_RIM_OFF" "_NORMALMAP_OFF" }
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
					varying highp vec2 xlv_TEXCOORD0;
					varying mediump vec3 xlv_TEXCOORD1;
					varying highp vec3 xlv_TEXCOORD2;
					varying lowp vec4 xlv_COLOR0;
					varying highp float xlv_TEXCOORD3;
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
					  tmpvar_2 = (glstate_matrix_mvp * vertex_22);
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
					  gl_Position = tmpvar_2;
					  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
					  xlv_TEXCOORD1 = tmpvar_3;
					  xlv_TEXCOORD2 = (_Object2World * vertex_22).xyz;
					  xlv_COLOR0 = _glesColor;
					  xlv_TEXCOORD3 = ((tmpvar_2.z * unity_FogParams.z) + unity_FogParams.w);
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform highp vec4 _Time;
					uniform highp vec3 _WorldSpaceCameraPos;
					uniform highp vec4 _WorldSpaceLightPos0;
					uniform lowp vec4 _LightColor0;
					uniform lowp vec4 _SpecColor;
					uniform sampler2D _LightTexture0;
					uniform mediump mat4 _LightMatrix0;
					uniform sampler2D _LightTextureB0;
					uniform sampler2D _MainTex;
					uniform lowp vec4 _Color;
					uniform lowp vec2 _V_CW_MainTex_Scroll;
					uniform mediump float _Shininess;
					varying highp vec2 xlv_TEXCOORD0;
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
					  lowp vec3 worldViewDir_7;
					  lowp vec3 lightDir_8;
					  highp vec4 tmpvar_9;
					  highp vec3 tmpvar_10;
					  tmpvar_10 = normalize((_WorldSpaceLightPos0.xyz - xlv_TEXCOORD2));
					  lightDir_8 = tmpvar_10;
					  highp vec3 tmpvar_11;
					  tmpvar_11 = normalize((_WorldSpaceCameraPos - xlv_TEXCOORD2));
					  worldViewDir_7 = tmpvar_11;
					  tmpvar_9 = xlv_COLOR0;
					  tmpvar_6 = xlv_TEXCOORD1;
					  lowp vec3 tmpvar_12;
					  lowp vec4 tmpvar_13;
					  highp vec2 P_14;
					  P_14 = (xlv_TEXCOORD0 + (_V_CW_MainTex_Scroll * _Time.x));
					  tmpvar_13 = texture2D (_MainTex, P_14);
					  tmpvar_12 = (tmpvar_13.xyz * _Color.xyz);
					  tmpvar_12 = (tmpvar_12 * tmpvar_9.xyz);
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
					  tmpvar_2 = lightDir_8;
					  tmpvar_1 = (tmpvar_1 * atten_4);
					  mediump vec3 viewDir_23;
					  viewDir_23 = worldViewDir_7;
					  lowp vec4 c_24;
					  lowp vec4 c_25;
					  highp float nh_26;
					  lowp float diff_27;
					  mediump float tmpvar_28;
					  tmpvar_28 = max (0.0, dot (tmpvar_6, tmpvar_2));
					  diff_27 = tmpvar_28;
					  mediump float tmpvar_29;
					  tmpvar_29 = max (0.0, dot (tmpvar_6, normalize(
					    (tmpvar_2 + viewDir_23)
					  )));
					  nh_26 = tmpvar_29;
					  mediump float y_30;
					  y_30 = (_Shininess * 128.0);
					  highp float tmpvar_31;
					  tmpvar_31 = (pow (nh_26, y_30) * tmpvar_13.w);
					  c_25.xyz = (((tmpvar_12 * tmpvar_1) * diff_27) + ((tmpvar_1 * _SpecColor.xyz) * tmpvar_31));
					  c_25.w = (tmpvar_13.w * _Color.w);
					  c_24.w = c_25.w;
					  c_24.xyz = c_25.xyz;
					  c_3.w = c_24.w;
					  highp float tmpvar_32;
					  tmpvar_32 = clamp (xlv_TEXCOORD3, 0.0, 1.0);
					  c_3.xyz = (c_25.xyz * vec3(tmpvar_32));
					  gl_FragData[0] = c_3;
					}
					
					
					#endif"
}
SubProgram "gles " {
Keywords { "POINT_COOKIE" "FOG_LINEAR" "V_CW_SPECULAR" "V_CW_VERTEX_COLOR" "V_CW_REFLECTIVE_OFF" "_EMISSION_OFF" "V_CW_RIM_OFF" "_NORMALMAP_OFF" }
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
					varying highp vec2 xlv_TEXCOORD0;
					varying mediump vec3 xlv_TEXCOORD1;
					varying highp vec3 xlv_TEXCOORD2;
					varying lowp vec4 xlv_COLOR0;
					varying highp float xlv_TEXCOORD3;
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
					  tmpvar_2 = (glstate_matrix_mvp * vertex_22);
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
					  gl_Position = tmpvar_2;
					  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
					  xlv_TEXCOORD1 = tmpvar_3;
					  xlv_TEXCOORD2 = (_Object2World * vertex_22).xyz;
					  xlv_COLOR0 = _glesColor;
					  xlv_TEXCOORD3 = ((tmpvar_2.z * unity_FogParams.z) + unity_FogParams.w);
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform highp vec4 _Time;
					uniform highp vec3 _WorldSpaceCameraPos;
					uniform highp vec4 _WorldSpaceLightPos0;
					uniform lowp vec4 _LightColor0;
					uniform lowp vec4 _SpecColor;
					uniform lowp samplerCube _LightTexture0;
					uniform mediump mat4 _LightMatrix0;
					uniform sampler2D _LightTextureB0;
					uniform sampler2D _MainTex;
					uniform lowp vec4 _Color;
					uniform lowp vec2 _V_CW_MainTex_Scroll;
					uniform mediump float _Shininess;
					varying highp vec2 xlv_TEXCOORD0;
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
					  lowp vec3 worldViewDir_5;
					  lowp vec3 lightDir_6;
					  highp vec4 tmpvar_7;
					  highp vec3 tmpvar_8;
					  tmpvar_8 = normalize((_WorldSpaceLightPos0.xyz - xlv_TEXCOORD2));
					  lightDir_6 = tmpvar_8;
					  highp vec3 tmpvar_9;
					  tmpvar_9 = normalize((_WorldSpaceCameraPos - xlv_TEXCOORD2));
					  worldViewDir_5 = tmpvar_9;
					  tmpvar_7 = xlv_COLOR0;
					  tmpvar_4 = xlv_TEXCOORD1;
					  lowp vec3 tmpvar_10;
					  lowp vec4 tmpvar_11;
					  highp vec2 P_12;
					  P_12 = (xlv_TEXCOORD0 + (_V_CW_MainTex_Scroll * _Time.x));
					  tmpvar_11 = texture2D (_MainTex, P_12);
					  tmpvar_10 = (tmpvar_11.xyz * _Color.xyz);
					  tmpvar_10 = (tmpvar_10 * tmpvar_7.xyz);
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
					  tmpvar_2 = lightDir_6;
					  tmpvar_1 = (tmpvar_1 * tmpvar_16);
					  mediump vec3 viewDir_17;
					  viewDir_17 = worldViewDir_5;
					  lowp vec4 c_18;
					  lowp vec4 c_19;
					  highp float nh_20;
					  lowp float diff_21;
					  mediump float tmpvar_22;
					  tmpvar_22 = max (0.0, dot (tmpvar_4, tmpvar_2));
					  diff_21 = tmpvar_22;
					  mediump float tmpvar_23;
					  tmpvar_23 = max (0.0, dot (tmpvar_4, normalize(
					    (tmpvar_2 + viewDir_17)
					  )));
					  nh_20 = tmpvar_23;
					  mediump float y_24;
					  y_24 = (_Shininess * 128.0);
					  highp float tmpvar_25;
					  tmpvar_25 = (pow (nh_20, y_24) * tmpvar_11.w);
					  c_19.xyz = (((tmpvar_10 * tmpvar_1) * diff_21) + ((tmpvar_1 * _SpecColor.xyz) * tmpvar_25));
					  c_19.w = (tmpvar_11.w * _Color.w);
					  c_18.w = c_19.w;
					  c_18.xyz = c_19.xyz;
					  c_3.w = c_18.w;
					  highp float tmpvar_26;
					  tmpvar_26 = clamp (xlv_TEXCOORD3, 0.0, 1.0);
					  c_3.xyz = (c_19.xyz * vec3(tmpvar_26));
					  gl_FragData[0] = c_3;
					}
					
					
					#endif"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL_COOKIE" "FOG_LINEAR" "V_CW_SPECULAR" "V_CW_VERTEX_COLOR" "V_CW_REFLECTIVE_OFF" "_EMISSION_OFF" "V_CW_RIM_OFF" "_NORMALMAP_OFF" }
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
					varying highp vec2 xlv_TEXCOORD0;
					varying mediump vec3 xlv_TEXCOORD1;
					varying highp vec3 xlv_TEXCOORD2;
					varying lowp vec4 xlv_COLOR0;
					varying highp float xlv_TEXCOORD3;
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
					  tmpvar_2 = (glstate_matrix_mvp * vertex_22);
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
					  gl_Position = tmpvar_2;
					  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
					  xlv_TEXCOORD1 = tmpvar_3;
					  xlv_TEXCOORD2 = (_Object2World * vertex_22).xyz;
					  xlv_COLOR0 = _glesColor;
					  xlv_TEXCOORD3 = ((tmpvar_2.z * unity_FogParams.z) + unity_FogParams.w);
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform highp vec4 _Time;
					uniform highp vec3 _WorldSpaceCameraPos;
					uniform mediump vec4 _WorldSpaceLightPos0;
					uniform lowp vec4 _LightColor0;
					uniform lowp vec4 _SpecColor;
					uniform sampler2D _LightTexture0;
					uniform mediump mat4 _LightMatrix0;
					uniform sampler2D _MainTex;
					uniform lowp vec4 _Color;
					uniform lowp vec2 _V_CW_MainTex_Scroll;
					uniform mediump float _Shininess;
					varying highp vec2 xlv_TEXCOORD0;
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
					  lowp vec3 worldViewDir_5;
					  lowp vec3 lightDir_6;
					  highp vec4 tmpvar_7;
					  mediump vec3 tmpvar_8;
					  tmpvar_8 = _WorldSpaceLightPos0.xyz;
					  lightDir_6 = tmpvar_8;
					  highp vec3 tmpvar_9;
					  tmpvar_9 = normalize((_WorldSpaceCameraPos - xlv_TEXCOORD2));
					  worldViewDir_5 = tmpvar_9;
					  tmpvar_7 = xlv_COLOR0;
					  tmpvar_4 = xlv_TEXCOORD1;
					  lowp vec3 tmpvar_10;
					  lowp vec4 tmpvar_11;
					  highp vec2 P_12;
					  P_12 = (xlv_TEXCOORD0 + (_V_CW_MainTex_Scroll * _Time.x));
					  tmpvar_11 = texture2D (_MainTex, P_12);
					  tmpvar_10 = (tmpvar_11.xyz * _Color.xyz);
					  tmpvar_10 = (tmpvar_10 * tmpvar_7.xyz);
					  highp vec4 tmpvar_13;
					  tmpvar_13.w = 1.0;
					  tmpvar_13.xyz = xlv_TEXCOORD2;
					  highp vec2 tmpvar_14;
					  tmpvar_14 = (_LightMatrix0 * tmpvar_13).xy;
					  lowp float tmpvar_15;
					  tmpvar_15 = texture2D (_LightTexture0, tmpvar_14).w;
					  tmpvar_1 = _LightColor0.xyz;
					  tmpvar_2 = lightDir_6;
					  tmpvar_1 = (tmpvar_1 * tmpvar_15);
					  mediump vec3 viewDir_16;
					  viewDir_16 = worldViewDir_5;
					  lowp vec4 c_17;
					  lowp vec4 c_18;
					  highp float nh_19;
					  lowp float diff_20;
					  mediump float tmpvar_21;
					  tmpvar_21 = max (0.0, dot (tmpvar_4, tmpvar_2));
					  diff_20 = tmpvar_21;
					  mediump float tmpvar_22;
					  tmpvar_22 = max (0.0, dot (tmpvar_4, normalize(
					    (tmpvar_2 + viewDir_16)
					  )));
					  nh_19 = tmpvar_22;
					  mediump float y_23;
					  y_23 = (_Shininess * 128.0);
					  highp float tmpvar_24;
					  tmpvar_24 = (pow (nh_19, y_23) * tmpvar_11.w);
					  c_18.xyz = (((tmpvar_10 * tmpvar_1) * diff_20) + ((tmpvar_1 * _SpecColor.xyz) * tmpvar_24));
					  c_18.w = (tmpvar_11.w * _Color.w);
					  c_17.w = c_18.w;
					  c_17.xyz = c_18.xyz;
					  c_3.w = c_17.w;
					  highp float tmpvar_25;
					  tmpvar_25 = clamp (xlv_TEXCOORD3, 0.0, 1.0);
					  c_3.xyz = (c_18.xyz * vec3(tmpvar_25));
					  gl_FragData[0] = c_3;
					}
					
					
					#endif"
}
SubProgram "gles " {
Keywords { "POINT" "_EMISSION" "V_CW_VERTEX_COLOR" "_NORMALMAP" "V_CW_REFLECTIVE_OFF" "V_CW_RIM_OFF" "V_CW_SPECULAR_OFF" }
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
					uniform highp vec4 unity_WorldTransformParams;
					uniform highp vec3 _V_CW_Bend;
					uniform highp vec3 _V_CW_Bias;
					uniform highp vec4 _V_CW_PivotPoint_Position;
					uniform highp vec4 _MainTex_ST;
					varying highp vec2 xlv_TEXCOORD0;
					varying lowp vec3 xlv_TEXCOORD1;
					varying lowp vec3 xlv_TEXCOORD2;
					varying lowp vec3 xlv_TEXCOORD3;
					varying highp vec3 xlv_TEXCOORD4;
					varying lowp vec4 xlv_COLOR0;
					void main ()
					{
					  lowp float tangentSign_1;
					  lowp vec3 worldTangent_2;
					  lowp vec3 worldNormal_3;
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
					  worldNormal_3 = tmpvar_42;
					  highp mat3 tmpvar_43;
					  tmpvar_43[0] = _Object2World[0].xyz;
					  tmpvar_43[1] = _Object2World[1].xyz;
					  tmpvar_43[2] = _Object2World[2].xyz;
					  highp vec3 tmpvar_44;
					  tmpvar_44 = normalize((tmpvar_43 * _glesTANGENT.xyz));
					  worldTangent_2 = tmpvar_44;
					  highp float tmpvar_45;
					  tmpvar_45 = (_glesTANGENT.w * unity_WorldTransformParams.w);
					  tangentSign_1 = tmpvar_45;
					  lowp vec3 tmpvar_46;
					  tmpvar_46 = (((worldNormal_3.yzx * worldTangent_2.zxy) - (worldNormal_3.zxy * worldTangent_2.yzx)) * tangentSign_1);
					  lowp vec3 tmpvar_47;
					  tmpvar_47.x = worldTangent_2.x;
					  tmpvar_47.y = tmpvar_46.x;
					  tmpvar_47.z = worldNormal_3.x;
					  lowp vec3 tmpvar_48;
					  tmpvar_48.x = worldTangent_2.y;
					  tmpvar_48.y = tmpvar_46.y;
					  tmpvar_48.z = worldNormal_3.y;
					  lowp vec3 tmpvar_49;
					  tmpvar_49.x = worldTangent_2.z;
					  tmpvar_49.y = tmpvar_46.z;
					  tmpvar_49.z = worldNormal_3.z;
					  gl_Position = (glstate_matrix_mvp * vertex_22);
					  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
					  xlv_TEXCOORD1 = tmpvar_47;
					  xlv_TEXCOORD2 = tmpvar_48;
					  xlv_TEXCOORD3 = tmpvar_49;
					  xlv_TEXCOORD4 = (_Object2World * vertex_22).xyz;
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
					uniform sampler2D _V_CW_NormalMap;
					uniform mediump float _V_CW_NormalMap_UV_Scale;
					uniform mediump float _V_CW_NormalMapStrength;
					varying highp vec2 xlv_TEXCOORD0;
					varying lowp vec3 xlv_TEXCOORD1;
					varying lowp vec3 xlv_TEXCOORD2;
					varying lowp vec3 xlv_TEXCOORD3;
					varying highp vec3 xlv_TEXCOORD4;
					varying lowp vec4 xlv_COLOR0;
					void main ()
					{
					  mediump vec3 tmpvar_1;
					  mediump vec3 tmpvar_2;
					  lowp vec3 worldN_3;
					  lowp vec3 lightDir_4;
					  highp vec4 tmpvar_5;
					  highp vec3 tmpvar_6;
					  tmpvar_6 = normalize((_WorldSpaceLightPos0.xyz - xlv_TEXCOORD4));
					  lightDir_4 = tmpvar_6;
					  tmpvar_5 = xlv_COLOR0;
					  lowp vec3 tmpvar_7;
					  lowp vec3 tmpvar_8;
					  lowp vec4 tmpvar_9;
					  highp vec2 P_10;
					  highp vec2 tmpvar_11;
					  tmpvar_11 = (_V_CW_MainTex_Scroll * _Time.x);
					  P_10 = (xlv_TEXCOORD0 + tmpvar_11);
					  tmpvar_9 = texture2D (_MainTex, P_10);
					  tmpvar_7 = (tmpvar_9.xyz * _Color.xyz);
					  tmpvar_7 = (tmpvar_7 * tmpvar_5.xyz);
					  highp vec2 P_12;
					  P_12 = ((xlv_TEXCOORD0 * _V_CW_NormalMap_UV_Scale) + tmpvar_11);
					  lowp vec3 normal_13;
					  normal_13.xy = ((texture2D (_V_CW_NormalMap, P_12).wy * 2.0) - 1.0);
					  normal_13.z = sqrt((1.0 - clamp (
					    dot (normal_13.xy, normal_13.xy)
					  , 0.0, 1.0)));
					  mediump vec3 tmpvar_14;
					  tmpvar_14.xy = (normal_13.xy * vec2(_V_CW_NormalMapStrength));
					  tmpvar_14.z = normal_13.z;
					  mediump vec3 tmpvar_15;
					  tmpvar_15 = normalize(tmpvar_14);
					  tmpvar_8 = tmpvar_15;
					  highp vec4 tmpvar_16;
					  tmpvar_16.w = 1.0;
					  tmpvar_16.xyz = xlv_TEXCOORD4;
					  highp vec3 tmpvar_17;
					  tmpvar_17 = (_LightMatrix0 * tmpvar_16).xyz;
					  highp float tmpvar_18;
					  tmpvar_18 = dot (tmpvar_17, tmpvar_17);
					  lowp float tmpvar_19;
					  tmpvar_19 = texture2D (_LightTexture0, vec2(tmpvar_18)).w;
					  worldN_3.x = dot (xlv_TEXCOORD1, tmpvar_8);
					  worldN_3.y = dot (xlv_TEXCOORD2, tmpvar_8);
					  worldN_3.z = dot (xlv_TEXCOORD3, tmpvar_8);
					  tmpvar_1 = _LightColor0.xyz;
					  tmpvar_2 = lightDir_4;
					  tmpvar_1 = (tmpvar_1 * tmpvar_19);
					  lowp vec4 c_20;
					  lowp vec4 c_21;
					  lowp float diff_22;
					  mediump float tmpvar_23;
					  tmpvar_23 = max (0.0, dot (worldN_3, tmpvar_2));
					  diff_22 = tmpvar_23;
					  c_21.xyz = ((tmpvar_7 * tmpvar_1) * diff_22);
					  c_21.w = (tmpvar_9.w * _Color.w);
					  c_20.w = c_21.w;
					  c_20.xyz = c_21.xyz;
					  gl_FragData[0] = c_20;
					}
					
					
					#endif"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "_EMISSION" "V_CW_VERTEX_COLOR" "_NORMALMAP" "V_CW_REFLECTIVE_OFF" "V_CW_RIM_OFF" "V_CW_SPECULAR_OFF" }
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
					uniform highp vec4 unity_WorldTransformParams;
					uniform highp vec3 _V_CW_Bend;
					uniform highp vec3 _V_CW_Bias;
					uniform highp vec4 _V_CW_PivotPoint_Position;
					uniform highp vec4 _MainTex_ST;
					varying highp vec2 xlv_TEXCOORD0;
					varying lowp vec3 xlv_TEXCOORD1;
					varying lowp vec3 xlv_TEXCOORD2;
					varying lowp vec3 xlv_TEXCOORD3;
					varying highp vec3 xlv_TEXCOORD4;
					varying lowp vec4 xlv_COLOR0;
					void main ()
					{
					  lowp float tangentSign_1;
					  lowp vec3 worldTangent_2;
					  lowp vec3 worldNormal_3;
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
					  worldNormal_3 = tmpvar_42;
					  highp mat3 tmpvar_43;
					  tmpvar_43[0] = _Object2World[0].xyz;
					  tmpvar_43[1] = _Object2World[1].xyz;
					  tmpvar_43[2] = _Object2World[2].xyz;
					  highp vec3 tmpvar_44;
					  tmpvar_44 = normalize((tmpvar_43 * _glesTANGENT.xyz));
					  worldTangent_2 = tmpvar_44;
					  highp float tmpvar_45;
					  tmpvar_45 = (_glesTANGENT.w * unity_WorldTransformParams.w);
					  tangentSign_1 = tmpvar_45;
					  lowp vec3 tmpvar_46;
					  tmpvar_46 = (((worldNormal_3.yzx * worldTangent_2.zxy) - (worldNormal_3.zxy * worldTangent_2.yzx)) * tangentSign_1);
					  lowp vec3 tmpvar_47;
					  tmpvar_47.x = worldTangent_2.x;
					  tmpvar_47.y = tmpvar_46.x;
					  tmpvar_47.z = worldNormal_3.x;
					  lowp vec3 tmpvar_48;
					  tmpvar_48.x = worldTangent_2.y;
					  tmpvar_48.y = tmpvar_46.y;
					  tmpvar_48.z = worldNormal_3.y;
					  lowp vec3 tmpvar_49;
					  tmpvar_49.x = worldTangent_2.z;
					  tmpvar_49.y = tmpvar_46.z;
					  tmpvar_49.z = worldNormal_3.z;
					  gl_Position = (glstate_matrix_mvp * vertex_22);
					  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
					  xlv_TEXCOORD1 = tmpvar_47;
					  xlv_TEXCOORD2 = tmpvar_48;
					  xlv_TEXCOORD3 = tmpvar_49;
					  xlv_TEXCOORD4 = (_Object2World * vertex_22).xyz;
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
					uniform sampler2D _V_CW_NormalMap;
					uniform mediump float _V_CW_NormalMap_UV_Scale;
					uniform mediump float _V_CW_NormalMapStrength;
					varying highp vec2 xlv_TEXCOORD0;
					varying lowp vec3 xlv_TEXCOORD1;
					varying lowp vec3 xlv_TEXCOORD2;
					varying lowp vec3 xlv_TEXCOORD3;
					varying lowp vec4 xlv_COLOR0;
					void main ()
					{
					  mediump vec3 tmpvar_1;
					  mediump vec3 tmpvar_2;
					  lowp vec3 worldN_3;
					  lowp vec3 lightDir_4;
					  highp vec4 tmpvar_5;
					  mediump vec3 tmpvar_6;
					  tmpvar_6 = _WorldSpaceLightPos0.xyz;
					  lightDir_4 = tmpvar_6;
					  tmpvar_5 = xlv_COLOR0;
					  lowp vec3 tmpvar_7;
					  lowp vec3 tmpvar_8;
					  lowp vec4 tmpvar_9;
					  highp vec2 P_10;
					  highp vec2 tmpvar_11;
					  tmpvar_11 = (_V_CW_MainTex_Scroll * _Time.x);
					  P_10 = (xlv_TEXCOORD0 + tmpvar_11);
					  tmpvar_9 = texture2D (_MainTex, P_10);
					  tmpvar_7 = (tmpvar_9.xyz * _Color.xyz);
					  tmpvar_7 = (tmpvar_7 * tmpvar_5.xyz);
					  highp vec2 P_12;
					  P_12 = ((xlv_TEXCOORD0 * _V_CW_NormalMap_UV_Scale) + tmpvar_11);
					  lowp vec3 normal_13;
					  normal_13.xy = ((texture2D (_V_CW_NormalMap, P_12).wy * 2.0) - 1.0);
					  normal_13.z = sqrt((1.0 - clamp (
					    dot (normal_13.xy, normal_13.xy)
					  , 0.0, 1.0)));
					  mediump vec3 tmpvar_14;
					  tmpvar_14.xy = (normal_13.xy * vec2(_V_CW_NormalMapStrength));
					  tmpvar_14.z = normal_13.z;
					  mediump vec3 tmpvar_15;
					  tmpvar_15 = normalize(tmpvar_14);
					  tmpvar_8 = tmpvar_15;
					  worldN_3.x = dot (xlv_TEXCOORD1, tmpvar_8);
					  worldN_3.y = dot (xlv_TEXCOORD2, tmpvar_8);
					  worldN_3.z = dot (xlv_TEXCOORD3, tmpvar_8);
					  tmpvar_1 = _LightColor0.xyz;
					  tmpvar_2 = lightDir_4;
					  lowp vec4 c_16;
					  lowp vec4 c_17;
					  lowp float diff_18;
					  mediump float tmpvar_19;
					  tmpvar_19 = max (0.0, dot (worldN_3, tmpvar_2));
					  diff_18 = tmpvar_19;
					  c_17.xyz = ((tmpvar_7 * tmpvar_1) * diff_18);
					  c_17.w = (tmpvar_9.w * _Color.w);
					  c_16.w = c_17.w;
					  c_16.xyz = c_17.xyz;
					  gl_FragData[0] = c_16;
					}
					
					
					#endif"
}
SubProgram "gles " {
Keywords { "SPOT" "_EMISSION" "V_CW_VERTEX_COLOR" "_NORMALMAP" "V_CW_REFLECTIVE_OFF" "V_CW_RIM_OFF" "V_CW_SPECULAR_OFF" }
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
					uniform highp vec4 unity_WorldTransformParams;
					uniform highp vec3 _V_CW_Bend;
					uniform highp vec3 _V_CW_Bias;
					uniform highp vec4 _V_CW_PivotPoint_Position;
					uniform highp vec4 _MainTex_ST;
					varying highp vec2 xlv_TEXCOORD0;
					varying lowp vec3 xlv_TEXCOORD1;
					varying lowp vec3 xlv_TEXCOORD2;
					varying lowp vec3 xlv_TEXCOORD3;
					varying highp vec3 xlv_TEXCOORD4;
					varying lowp vec4 xlv_COLOR0;
					void main ()
					{
					  lowp float tangentSign_1;
					  lowp vec3 worldTangent_2;
					  lowp vec3 worldNormal_3;
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
					  worldNormal_3 = tmpvar_42;
					  highp mat3 tmpvar_43;
					  tmpvar_43[0] = _Object2World[0].xyz;
					  tmpvar_43[1] = _Object2World[1].xyz;
					  tmpvar_43[2] = _Object2World[2].xyz;
					  highp vec3 tmpvar_44;
					  tmpvar_44 = normalize((tmpvar_43 * _glesTANGENT.xyz));
					  worldTangent_2 = tmpvar_44;
					  highp float tmpvar_45;
					  tmpvar_45 = (_glesTANGENT.w * unity_WorldTransformParams.w);
					  tangentSign_1 = tmpvar_45;
					  lowp vec3 tmpvar_46;
					  tmpvar_46 = (((worldNormal_3.yzx * worldTangent_2.zxy) - (worldNormal_3.zxy * worldTangent_2.yzx)) * tangentSign_1);
					  lowp vec3 tmpvar_47;
					  tmpvar_47.x = worldTangent_2.x;
					  tmpvar_47.y = tmpvar_46.x;
					  tmpvar_47.z = worldNormal_3.x;
					  lowp vec3 tmpvar_48;
					  tmpvar_48.x = worldTangent_2.y;
					  tmpvar_48.y = tmpvar_46.y;
					  tmpvar_48.z = worldNormal_3.y;
					  lowp vec3 tmpvar_49;
					  tmpvar_49.x = worldTangent_2.z;
					  tmpvar_49.y = tmpvar_46.z;
					  tmpvar_49.z = worldNormal_3.z;
					  gl_Position = (glstate_matrix_mvp * vertex_22);
					  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
					  xlv_TEXCOORD1 = tmpvar_47;
					  xlv_TEXCOORD2 = tmpvar_48;
					  xlv_TEXCOORD3 = tmpvar_49;
					  xlv_TEXCOORD4 = (_Object2World * vertex_22).xyz;
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
					uniform sampler2D _V_CW_NormalMap;
					uniform mediump float _V_CW_NormalMap_UV_Scale;
					uniform mediump float _V_CW_NormalMapStrength;
					varying highp vec2 xlv_TEXCOORD0;
					varying lowp vec3 xlv_TEXCOORD1;
					varying lowp vec3 xlv_TEXCOORD2;
					varying lowp vec3 xlv_TEXCOORD3;
					varying highp vec3 xlv_TEXCOORD4;
					varying lowp vec4 xlv_COLOR0;
					void main ()
					{
					  mediump vec3 tmpvar_1;
					  mediump vec3 tmpvar_2;
					  lowp vec3 worldN_3;
					  lowp float atten_4;
					  mediump vec4 lightCoord_5;
					  lowp vec3 lightDir_6;
					  highp vec4 tmpvar_7;
					  highp vec3 tmpvar_8;
					  tmpvar_8 = normalize((_WorldSpaceLightPos0.xyz - xlv_TEXCOORD4));
					  lightDir_6 = tmpvar_8;
					  tmpvar_7 = xlv_COLOR0;
					  lowp vec3 tmpvar_9;
					  lowp vec3 tmpvar_10;
					  lowp vec4 tmpvar_11;
					  highp vec2 P_12;
					  highp vec2 tmpvar_13;
					  tmpvar_13 = (_V_CW_MainTex_Scroll * _Time.x);
					  P_12 = (xlv_TEXCOORD0 + tmpvar_13);
					  tmpvar_11 = texture2D (_MainTex, P_12);
					  tmpvar_9 = (tmpvar_11.xyz * _Color.xyz);
					  tmpvar_9 = (tmpvar_9 * tmpvar_7.xyz);
					  highp vec2 P_14;
					  P_14 = ((xlv_TEXCOORD0 * _V_CW_NormalMap_UV_Scale) + tmpvar_13);
					  lowp vec3 normal_15;
					  normal_15.xy = ((texture2D (_V_CW_NormalMap, P_14).wy * 2.0) - 1.0);
					  normal_15.z = sqrt((1.0 - clamp (
					    dot (normal_15.xy, normal_15.xy)
					  , 0.0, 1.0)));
					  mediump vec3 tmpvar_16;
					  tmpvar_16.xy = (normal_15.xy * vec2(_V_CW_NormalMapStrength));
					  tmpvar_16.z = normal_15.z;
					  mediump vec3 tmpvar_17;
					  tmpvar_17 = normalize(tmpvar_16);
					  tmpvar_10 = tmpvar_17;
					  highp vec4 tmpvar_18;
					  tmpvar_18.w = 1.0;
					  tmpvar_18.xyz = xlv_TEXCOORD4;
					  highp vec4 tmpvar_19;
					  tmpvar_19 = (_LightMatrix0 * tmpvar_18);
					  lightCoord_5 = tmpvar_19;
					  lowp vec4 tmpvar_20;
					  mediump vec2 P_21;
					  P_21 = ((lightCoord_5.xy / lightCoord_5.w) + 0.5);
					  tmpvar_20 = texture2D (_LightTexture0, P_21);
					  highp vec3 LightCoord_22;
					  LightCoord_22 = lightCoord_5.xyz;
					  highp float tmpvar_23;
					  tmpvar_23 = dot (LightCoord_22, LightCoord_22);
					  lowp vec4 tmpvar_24;
					  tmpvar_24 = texture2D (_LightTextureB0, vec2(tmpvar_23));
					  mediump float tmpvar_25;
					  tmpvar_25 = ((float(
					    (lightCoord_5.z > 0.0)
					  ) * tmpvar_20.w) * tmpvar_24.w);
					  atten_4 = tmpvar_25;
					  worldN_3.x = dot (xlv_TEXCOORD1, tmpvar_10);
					  worldN_3.y = dot (xlv_TEXCOORD2, tmpvar_10);
					  worldN_3.z = dot (xlv_TEXCOORD3, tmpvar_10);
					  tmpvar_1 = _LightColor0.xyz;
					  tmpvar_2 = lightDir_6;
					  tmpvar_1 = (tmpvar_1 * atten_4);
					  lowp vec4 c_26;
					  lowp vec4 c_27;
					  lowp float diff_28;
					  mediump float tmpvar_29;
					  tmpvar_29 = max (0.0, dot (worldN_3, tmpvar_2));
					  diff_28 = tmpvar_29;
					  c_27.xyz = ((tmpvar_9 * tmpvar_1) * diff_28);
					  c_27.w = (tmpvar_11.w * _Color.w);
					  c_26.w = c_27.w;
					  c_26.xyz = c_27.xyz;
					  gl_FragData[0] = c_26;
					}
					
					
					#endif"
}
SubProgram "gles " {
Keywords { "POINT_COOKIE" "_EMISSION" "V_CW_VERTEX_COLOR" "_NORMALMAP" "V_CW_REFLECTIVE_OFF" "V_CW_RIM_OFF" "V_CW_SPECULAR_OFF" }
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
					uniform highp vec4 unity_WorldTransformParams;
					uniform highp vec3 _V_CW_Bend;
					uniform highp vec3 _V_CW_Bias;
					uniform highp vec4 _V_CW_PivotPoint_Position;
					uniform highp vec4 _MainTex_ST;
					varying highp vec2 xlv_TEXCOORD0;
					varying lowp vec3 xlv_TEXCOORD1;
					varying lowp vec3 xlv_TEXCOORD2;
					varying lowp vec3 xlv_TEXCOORD3;
					varying highp vec3 xlv_TEXCOORD4;
					varying lowp vec4 xlv_COLOR0;
					void main ()
					{
					  lowp float tangentSign_1;
					  lowp vec3 worldTangent_2;
					  lowp vec3 worldNormal_3;
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
					  worldNormal_3 = tmpvar_42;
					  highp mat3 tmpvar_43;
					  tmpvar_43[0] = _Object2World[0].xyz;
					  tmpvar_43[1] = _Object2World[1].xyz;
					  tmpvar_43[2] = _Object2World[2].xyz;
					  highp vec3 tmpvar_44;
					  tmpvar_44 = normalize((tmpvar_43 * _glesTANGENT.xyz));
					  worldTangent_2 = tmpvar_44;
					  highp float tmpvar_45;
					  tmpvar_45 = (_glesTANGENT.w * unity_WorldTransformParams.w);
					  tangentSign_1 = tmpvar_45;
					  lowp vec3 tmpvar_46;
					  tmpvar_46 = (((worldNormal_3.yzx * worldTangent_2.zxy) - (worldNormal_3.zxy * worldTangent_2.yzx)) * tangentSign_1);
					  lowp vec3 tmpvar_47;
					  tmpvar_47.x = worldTangent_2.x;
					  tmpvar_47.y = tmpvar_46.x;
					  tmpvar_47.z = worldNormal_3.x;
					  lowp vec3 tmpvar_48;
					  tmpvar_48.x = worldTangent_2.y;
					  tmpvar_48.y = tmpvar_46.y;
					  tmpvar_48.z = worldNormal_3.y;
					  lowp vec3 tmpvar_49;
					  tmpvar_49.x = worldTangent_2.z;
					  tmpvar_49.y = tmpvar_46.z;
					  tmpvar_49.z = worldNormal_3.z;
					  gl_Position = (glstate_matrix_mvp * vertex_22);
					  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
					  xlv_TEXCOORD1 = tmpvar_47;
					  xlv_TEXCOORD2 = tmpvar_48;
					  xlv_TEXCOORD3 = tmpvar_49;
					  xlv_TEXCOORD4 = (_Object2World * vertex_22).xyz;
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
					uniform sampler2D _V_CW_NormalMap;
					uniform mediump float _V_CW_NormalMap_UV_Scale;
					uniform mediump float _V_CW_NormalMapStrength;
					varying highp vec2 xlv_TEXCOORD0;
					varying lowp vec3 xlv_TEXCOORD1;
					varying lowp vec3 xlv_TEXCOORD2;
					varying lowp vec3 xlv_TEXCOORD3;
					varying highp vec3 xlv_TEXCOORD4;
					varying lowp vec4 xlv_COLOR0;
					void main ()
					{
					  mediump vec3 tmpvar_1;
					  mediump vec3 tmpvar_2;
					  lowp vec3 worldN_3;
					  lowp vec3 lightDir_4;
					  highp vec4 tmpvar_5;
					  highp vec3 tmpvar_6;
					  tmpvar_6 = normalize((_WorldSpaceLightPos0.xyz - xlv_TEXCOORD4));
					  lightDir_4 = tmpvar_6;
					  tmpvar_5 = xlv_COLOR0;
					  lowp vec3 tmpvar_7;
					  lowp vec3 tmpvar_8;
					  lowp vec4 tmpvar_9;
					  highp vec2 P_10;
					  highp vec2 tmpvar_11;
					  tmpvar_11 = (_V_CW_MainTex_Scroll * _Time.x);
					  P_10 = (xlv_TEXCOORD0 + tmpvar_11);
					  tmpvar_9 = texture2D (_MainTex, P_10);
					  tmpvar_7 = (tmpvar_9.xyz * _Color.xyz);
					  tmpvar_7 = (tmpvar_7 * tmpvar_5.xyz);
					  highp vec2 P_12;
					  P_12 = ((xlv_TEXCOORD0 * _V_CW_NormalMap_UV_Scale) + tmpvar_11);
					  lowp vec3 normal_13;
					  normal_13.xy = ((texture2D (_V_CW_NormalMap, P_12).wy * 2.0) - 1.0);
					  normal_13.z = sqrt((1.0 - clamp (
					    dot (normal_13.xy, normal_13.xy)
					  , 0.0, 1.0)));
					  mediump vec3 tmpvar_14;
					  tmpvar_14.xy = (normal_13.xy * vec2(_V_CW_NormalMapStrength));
					  tmpvar_14.z = normal_13.z;
					  mediump vec3 tmpvar_15;
					  tmpvar_15 = normalize(tmpvar_14);
					  tmpvar_8 = tmpvar_15;
					  highp vec4 tmpvar_16;
					  tmpvar_16.w = 1.0;
					  tmpvar_16.xyz = xlv_TEXCOORD4;
					  highp vec3 tmpvar_17;
					  tmpvar_17 = (_LightMatrix0 * tmpvar_16).xyz;
					  highp float tmpvar_18;
					  tmpvar_18 = dot (tmpvar_17, tmpvar_17);
					  lowp float tmpvar_19;
					  tmpvar_19 = (texture2D (_LightTextureB0, vec2(tmpvar_18)).w * textureCube (_LightTexture0, tmpvar_17).w);
					  worldN_3.x = dot (xlv_TEXCOORD1, tmpvar_8);
					  worldN_3.y = dot (xlv_TEXCOORD2, tmpvar_8);
					  worldN_3.z = dot (xlv_TEXCOORD3, tmpvar_8);
					  tmpvar_1 = _LightColor0.xyz;
					  tmpvar_2 = lightDir_4;
					  tmpvar_1 = (tmpvar_1 * tmpvar_19);
					  lowp vec4 c_20;
					  lowp vec4 c_21;
					  lowp float diff_22;
					  mediump float tmpvar_23;
					  tmpvar_23 = max (0.0, dot (worldN_3, tmpvar_2));
					  diff_22 = tmpvar_23;
					  c_21.xyz = ((tmpvar_7 * tmpvar_1) * diff_22);
					  c_21.w = (tmpvar_9.w * _Color.w);
					  c_20.w = c_21.w;
					  c_20.xyz = c_21.xyz;
					  gl_FragData[0] = c_20;
					}
					
					
					#endif"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL_COOKIE" "_EMISSION" "V_CW_VERTEX_COLOR" "_NORMALMAP" "V_CW_REFLECTIVE_OFF" "V_CW_RIM_OFF" "V_CW_SPECULAR_OFF" }
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
					uniform highp vec4 unity_WorldTransformParams;
					uniform highp vec3 _V_CW_Bend;
					uniform highp vec3 _V_CW_Bias;
					uniform highp vec4 _V_CW_PivotPoint_Position;
					uniform highp vec4 _MainTex_ST;
					varying highp vec2 xlv_TEXCOORD0;
					varying lowp vec3 xlv_TEXCOORD1;
					varying lowp vec3 xlv_TEXCOORD2;
					varying lowp vec3 xlv_TEXCOORD3;
					varying highp vec3 xlv_TEXCOORD4;
					varying lowp vec4 xlv_COLOR0;
					void main ()
					{
					  lowp float tangentSign_1;
					  lowp vec3 worldTangent_2;
					  lowp vec3 worldNormal_3;
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
					  worldNormal_3 = tmpvar_42;
					  highp mat3 tmpvar_43;
					  tmpvar_43[0] = _Object2World[0].xyz;
					  tmpvar_43[1] = _Object2World[1].xyz;
					  tmpvar_43[2] = _Object2World[2].xyz;
					  highp vec3 tmpvar_44;
					  tmpvar_44 = normalize((tmpvar_43 * _glesTANGENT.xyz));
					  worldTangent_2 = tmpvar_44;
					  highp float tmpvar_45;
					  tmpvar_45 = (_glesTANGENT.w * unity_WorldTransformParams.w);
					  tangentSign_1 = tmpvar_45;
					  lowp vec3 tmpvar_46;
					  tmpvar_46 = (((worldNormal_3.yzx * worldTangent_2.zxy) - (worldNormal_3.zxy * worldTangent_2.yzx)) * tangentSign_1);
					  lowp vec3 tmpvar_47;
					  tmpvar_47.x = worldTangent_2.x;
					  tmpvar_47.y = tmpvar_46.x;
					  tmpvar_47.z = worldNormal_3.x;
					  lowp vec3 tmpvar_48;
					  tmpvar_48.x = worldTangent_2.y;
					  tmpvar_48.y = tmpvar_46.y;
					  tmpvar_48.z = worldNormal_3.y;
					  lowp vec3 tmpvar_49;
					  tmpvar_49.x = worldTangent_2.z;
					  tmpvar_49.y = tmpvar_46.z;
					  tmpvar_49.z = worldNormal_3.z;
					  gl_Position = (glstate_matrix_mvp * vertex_22);
					  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
					  xlv_TEXCOORD1 = tmpvar_47;
					  xlv_TEXCOORD2 = tmpvar_48;
					  xlv_TEXCOORD3 = tmpvar_49;
					  xlv_TEXCOORD4 = (_Object2World * vertex_22).xyz;
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
					uniform sampler2D _V_CW_NormalMap;
					uniform mediump float _V_CW_NormalMap_UV_Scale;
					uniform mediump float _V_CW_NormalMapStrength;
					varying highp vec2 xlv_TEXCOORD0;
					varying lowp vec3 xlv_TEXCOORD1;
					varying lowp vec3 xlv_TEXCOORD2;
					varying lowp vec3 xlv_TEXCOORD3;
					varying highp vec3 xlv_TEXCOORD4;
					varying lowp vec4 xlv_COLOR0;
					void main ()
					{
					  mediump vec3 tmpvar_1;
					  mediump vec3 tmpvar_2;
					  lowp vec3 worldN_3;
					  lowp vec3 lightDir_4;
					  highp vec4 tmpvar_5;
					  mediump vec3 tmpvar_6;
					  tmpvar_6 = _WorldSpaceLightPos0.xyz;
					  lightDir_4 = tmpvar_6;
					  tmpvar_5 = xlv_COLOR0;
					  lowp vec3 tmpvar_7;
					  lowp vec3 tmpvar_8;
					  lowp vec4 tmpvar_9;
					  highp vec2 P_10;
					  highp vec2 tmpvar_11;
					  tmpvar_11 = (_V_CW_MainTex_Scroll * _Time.x);
					  P_10 = (xlv_TEXCOORD0 + tmpvar_11);
					  tmpvar_9 = texture2D (_MainTex, P_10);
					  tmpvar_7 = (tmpvar_9.xyz * _Color.xyz);
					  tmpvar_7 = (tmpvar_7 * tmpvar_5.xyz);
					  highp vec2 P_12;
					  P_12 = ((xlv_TEXCOORD0 * _V_CW_NormalMap_UV_Scale) + tmpvar_11);
					  lowp vec3 normal_13;
					  normal_13.xy = ((texture2D (_V_CW_NormalMap, P_12).wy * 2.0) - 1.0);
					  normal_13.z = sqrt((1.0 - clamp (
					    dot (normal_13.xy, normal_13.xy)
					  , 0.0, 1.0)));
					  mediump vec3 tmpvar_14;
					  tmpvar_14.xy = (normal_13.xy * vec2(_V_CW_NormalMapStrength));
					  tmpvar_14.z = normal_13.z;
					  mediump vec3 tmpvar_15;
					  tmpvar_15 = normalize(tmpvar_14);
					  tmpvar_8 = tmpvar_15;
					  highp vec4 tmpvar_16;
					  tmpvar_16.w = 1.0;
					  tmpvar_16.xyz = xlv_TEXCOORD4;
					  highp vec2 tmpvar_17;
					  tmpvar_17 = (_LightMatrix0 * tmpvar_16).xy;
					  lowp float tmpvar_18;
					  tmpvar_18 = texture2D (_LightTexture0, tmpvar_17).w;
					  worldN_3.x = dot (xlv_TEXCOORD1, tmpvar_8);
					  worldN_3.y = dot (xlv_TEXCOORD2, tmpvar_8);
					  worldN_3.z = dot (xlv_TEXCOORD3, tmpvar_8);
					  tmpvar_1 = _LightColor0.xyz;
					  tmpvar_2 = lightDir_4;
					  tmpvar_1 = (tmpvar_1 * tmpvar_18);
					  lowp vec4 c_19;
					  lowp vec4 c_20;
					  lowp float diff_21;
					  mediump float tmpvar_22;
					  tmpvar_22 = max (0.0, dot (worldN_3, tmpvar_2));
					  diff_21 = tmpvar_22;
					  c_20.xyz = ((tmpvar_7 * tmpvar_1) * diff_21);
					  c_20.w = (tmpvar_9.w * _Color.w);
					  c_19.w = c_20.w;
					  c_19.xyz = c_20.xyz;
					  gl_FragData[0] = c_19;
					}
					
					
					#endif"
}
SubProgram "gles " {
Keywords { "POINT" "FOG_LINEAR" "_EMISSION" "V_CW_VERTEX_COLOR" "_NORMALMAP" "V_CW_REFLECTIVE_OFF" "V_CW_RIM_OFF" "V_CW_SPECULAR_OFF" }
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
					uniform highp vec4 unity_WorldTransformParams;
					uniform highp vec4 unity_FogParams;
					uniform highp vec3 _V_CW_Bend;
					uniform highp vec3 _V_CW_Bias;
					uniform highp vec4 _V_CW_PivotPoint_Position;
					uniform highp vec4 _MainTex_ST;
					varying highp vec2 xlv_TEXCOORD0;
					varying lowp vec3 xlv_TEXCOORD1;
					varying lowp vec3 xlv_TEXCOORD2;
					varying lowp vec3 xlv_TEXCOORD3;
					varying highp vec3 xlv_TEXCOORD4;
					varying lowp vec4 xlv_COLOR0;
					varying highp float xlv_TEXCOORD5;
					void main ()
					{
					  lowp float tangentSign_1;
					  lowp vec3 worldTangent_2;
					  lowp vec3 worldNormal_3;
					  highp vec4 tmpvar_4;
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
					  tmpvar_4 = (glstate_matrix_mvp * vertex_23);
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
					  worldNormal_3 = tmpvar_43;
					  highp mat3 tmpvar_44;
					  tmpvar_44[0] = _Object2World[0].xyz;
					  tmpvar_44[1] = _Object2World[1].xyz;
					  tmpvar_44[2] = _Object2World[2].xyz;
					  highp vec3 tmpvar_45;
					  tmpvar_45 = normalize((tmpvar_44 * _glesTANGENT.xyz));
					  worldTangent_2 = tmpvar_45;
					  highp float tmpvar_46;
					  tmpvar_46 = (_glesTANGENT.w * unity_WorldTransformParams.w);
					  tangentSign_1 = tmpvar_46;
					  lowp vec3 tmpvar_47;
					  tmpvar_47 = (((worldNormal_3.yzx * worldTangent_2.zxy) - (worldNormal_3.zxy * worldTangent_2.yzx)) * tangentSign_1);
					  lowp vec3 tmpvar_48;
					  tmpvar_48.x = worldTangent_2.x;
					  tmpvar_48.y = tmpvar_47.x;
					  tmpvar_48.z = worldNormal_3.x;
					  lowp vec3 tmpvar_49;
					  tmpvar_49.x = worldTangent_2.y;
					  tmpvar_49.y = tmpvar_47.y;
					  tmpvar_49.z = worldNormal_3.y;
					  lowp vec3 tmpvar_50;
					  tmpvar_50.x = worldTangent_2.z;
					  tmpvar_50.y = tmpvar_47.z;
					  tmpvar_50.z = worldNormal_3.z;
					  gl_Position = tmpvar_4;
					  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
					  xlv_TEXCOORD1 = tmpvar_48;
					  xlv_TEXCOORD2 = tmpvar_49;
					  xlv_TEXCOORD3 = tmpvar_50;
					  xlv_TEXCOORD4 = (_Object2World * vertex_23).xyz;
					  xlv_COLOR0 = _glesColor;
					  xlv_TEXCOORD5 = ((tmpvar_4.z * unity_FogParams.z) + unity_FogParams.w);
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
					uniform sampler2D _V_CW_NormalMap;
					uniform mediump float _V_CW_NormalMap_UV_Scale;
					uniform mediump float _V_CW_NormalMapStrength;
					varying highp vec2 xlv_TEXCOORD0;
					varying lowp vec3 xlv_TEXCOORD1;
					varying lowp vec3 xlv_TEXCOORD2;
					varying lowp vec3 xlv_TEXCOORD3;
					varying highp vec3 xlv_TEXCOORD4;
					varying lowp vec4 xlv_COLOR0;
					varying highp float xlv_TEXCOORD5;
					void main ()
					{
					  mediump vec3 tmpvar_1;
					  mediump vec3 tmpvar_2;
					  lowp vec3 worldN_3;
					  lowp vec4 c_4;
					  lowp vec3 lightDir_5;
					  highp vec4 tmpvar_6;
					  highp vec3 tmpvar_7;
					  tmpvar_7 = normalize((_WorldSpaceLightPos0.xyz - xlv_TEXCOORD4));
					  lightDir_5 = tmpvar_7;
					  tmpvar_6 = xlv_COLOR0;
					  lowp vec3 tmpvar_8;
					  lowp vec3 tmpvar_9;
					  lowp vec4 tmpvar_10;
					  highp vec2 P_11;
					  highp vec2 tmpvar_12;
					  tmpvar_12 = (_V_CW_MainTex_Scroll * _Time.x);
					  P_11 = (xlv_TEXCOORD0 + tmpvar_12);
					  tmpvar_10 = texture2D (_MainTex, P_11);
					  tmpvar_8 = (tmpvar_10.xyz * _Color.xyz);
					  tmpvar_8 = (tmpvar_8 * tmpvar_6.xyz);
					  highp vec2 P_13;
					  P_13 = ((xlv_TEXCOORD0 * _V_CW_NormalMap_UV_Scale) + tmpvar_12);
					  lowp vec3 normal_14;
					  normal_14.xy = ((texture2D (_V_CW_NormalMap, P_13).wy * 2.0) - 1.0);
					  normal_14.z = sqrt((1.0 - clamp (
					    dot (normal_14.xy, normal_14.xy)
					  , 0.0, 1.0)));
					  mediump vec3 tmpvar_15;
					  tmpvar_15.xy = (normal_14.xy * vec2(_V_CW_NormalMapStrength));
					  tmpvar_15.z = normal_14.z;
					  mediump vec3 tmpvar_16;
					  tmpvar_16 = normalize(tmpvar_15);
					  tmpvar_9 = tmpvar_16;
					  highp vec4 tmpvar_17;
					  tmpvar_17.w = 1.0;
					  tmpvar_17.xyz = xlv_TEXCOORD4;
					  highp vec3 tmpvar_18;
					  tmpvar_18 = (_LightMatrix0 * tmpvar_17).xyz;
					  highp float tmpvar_19;
					  tmpvar_19 = dot (tmpvar_18, tmpvar_18);
					  lowp float tmpvar_20;
					  tmpvar_20 = texture2D (_LightTexture0, vec2(tmpvar_19)).w;
					  worldN_3.x = dot (xlv_TEXCOORD1, tmpvar_9);
					  worldN_3.y = dot (xlv_TEXCOORD2, tmpvar_9);
					  worldN_3.z = dot (xlv_TEXCOORD3, tmpvar_9);
					  tmpvar_1 = _LightColor0.xyz;
					  tmpvar_2 = lightDir_5;
					  tmpvar_1 = (tmpvar_1 * tmpvar_20);
					  lowp vec4 c_21;
					  lowp vec4 c_22;
					  lowp float diff_23;
					  mediump float tmpvar_24;
					  tmpvar_24 = max (0.0, dot (worldN_3, tmpvar_2));
					  diff_23 = tmpvar_24;
					  c_22.xyz = ((tmpvar_8 * tmpvar_1) * diff_23);
					  c_22.w = (tmpvar_10.w * _Color.w);
					  c_21.w = c_22.w;
					  c_21.xyz = c_22.xyz;
					  c_4.w = c_21.w;
					  highp float tmpvar_25;
					  tmpvar_25 = clamp (xlv_TEXCOORD5, 0.0, 1.0);
					  c_4.xyz = (c_22.xyz * vec3(tmpvar_25));
					  gl_FragData[0] = c_4;
					}
					
					
					#endif"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "FOG_LINEAR" "_EMISSION" "V_CW_VERTEX_COLOR" "_NORMALMAP" "V_CW_REFLECTIVE_OFF" "V_CW_RIM_OFF" "V_CW_SPECULAR_OFF" }
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
					uniform highp vec4 unity_WorldTransformParams;
					uniform highp vec4 unity_FogParams;
					uniform highp vec3 _V_CW_Bend;
					uniform highp vec3 _V_CW_Bias;
					uniform highp vec4 _V_CW_PivotPoint_Position;
					uniform highp vec4 _MainTex_ST;
					varying highp vec2 xlv_TEXCOORD0;
					varying lowp vec3 xlv_TEXCOORD1;
					varying lowp vec3 xlv_TEXCOORD2;
					varying lowp vec3 xlv_TEXCOORD3;
					varying highp vec3 xlv_TEXCOORD4;
					varying lowp vec4 xlv_COLOR0;
					varying highp float xlv_TEXCOORD5;
					void main ()
					{
					  lowp float tangentSign_1;
					  lowp vec3 worldTangent_2;
					  lowp vec3 worldNormal_3;
					  highp vec4 tmpvar_4;
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
					  tmpvar_4 = (glstate_matrix_mvp * vertex_23);
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
					  worldNormal_3 = tmpvar_43;
					  highp mat3 tmpvar_44;
					  tmpvar_44[0] = _Object2World[0].xyz;
					  tmpvar_44[1] = _Object2World[1].xyz;
					  tmpvar_44[2] = _Object2World[2].xyz;
					  highp vec3 tmpvar_45;
					  tmpvar_45 = normalize((tmpvar_44 * _glesTANGENT.xyz));
					  worldTangent_2 = tmpvar_45;
					  highp float tmpvar_46;
					  tmpvar_46 = (_glesTANGENT.w * unity_WorldTransformParams.w);
					  tangentSign_1 = tmpvar_46;
					  lowp vec3 tmpvar_47;
					  tmpvar_47 = (((worldNormal_3.yzx * worldTangent_2.zxy) - (worldNormal_3.zxy * worldTangent_2.yzx)) * tangentSign_1);
					  lowp vec3 tmpvar_48;
					  tmpvar_48.x = worldTangent_2.x;
					  tmpvar_48.y = tmpvar_47.x;
					  tmpvar_48.z = worldNormal_3.x;
					  lowp vec3 tmpvar_49;
					  tmpvar_49.x = worldTangent_2.y;
					  tmpvar_49.y = tmpvar_47.y;
					  tmpvar_49.z = worldNormal_3.y;
					  lowp vec3 tmpvar_50;
					  tmpvar_50.x = worldTangent_2.z;
					  tmpvar_50.y = tmpvar_47.z;
					  tmpvar_50.z = worldNormal_3.z;
					  gl_Position = tmpvar_4;
					  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
					  xlv_TEXCOORD1 = tmpvar_48;
					  xlv_TEXCOORD2 = tmpvar_49;
					  xlv_TEXCOORD3 = tmpvar_50;
					  xlv_TEXCOORD4 = (_Object2World * vertex_23).xyz;
					  xlv_COLOR0 = _glesColor;
					  xlv_TEXCOORD5 = ((tmpvar_4.z * unity_FogParams.z) + unity_FogParams.w);
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform highp vec4 _Time;
					uniform mediump vec4 _WorldSpaceLightPos0;
					uniform lowp vec4 _LightColor0;
					uniform sampler2D _MainTex;
					uniform lowp vec4 _Color;
					uniform lowp vec2 _V_CW_MainTex_Scroll;
					uniform sampler2D _V_CW_NormalMap;
					uniform mediump float _V_CW_NormalMap_UV_Scale;
					uniform mediump float _V_CW_NormalMapStrength;
					varying highp vec2 xlv_TEXCOORD0;
					varying lowp vec3 xlv_TEXCOORD1;
					varying lowp vec3 xlv_TEXCOORD2;
					varying lowp vec3 xlv_TEXCOORD3;
					varying lowp vec4 xlv_COLOR0;
					varying highp float xlv_TEXCOORD5;
					void main ()
					{
					  mediump vec3 tmpvar_1;
					  mediump vec3 tmpvar_2;
					  lowp vec3 worldN_3;
					  lowp vec4 c_4;
					  lowp vec3 lightDir_5;
					  highp vec4 tmpvar_6;
					  mediump vec3 tmpvar_7;
					  tmpvar_7 = _WorldSpaceLightPos0.xyz;
					  lightDir_5 = tmpvar_7;
					  tmpvar_6 = xlv_COLOR0;
					  lowp vec3 tmpvar_8;
					  lowp vec3 tmpvar_9;
					  lowp vec4 tmpvar_10;
					  highp vec2 P_11;
					  highp vec2 tmpvar_12;
					  tmpvar_12 = (_V_CW_MainTex_Scroll * _Time.x);
					  P_11 = (xlv_TEXCOORD0 + tmpvar_12);
					  tmpvar_10 = texture2D (_MainTex, P_11);
					  tmpvar_8 = (tmpvar_10.xyz * _Color.xyz);
					  tmpvar_8 = (tmpvar_8 * tmpvar_6.xyz);
					  highp vec2 P_13;
					  P_13 = ((xlv_TEXCOORD0 * _V_CW_NormalMap_UV_Scale) + tmpvar_12);
					  lowp vec3 normal_14;
					  normal_14.xy = ((texture2D (_V_CW_NormalMap, P_13).wy * 2.0) - 1.0);
					  normal_14.z = sqrt((1.0 - clamp (
					    dot (normal_14.xy, normal_14.xy)
					  , 0.0, 1.0)));
					  mediump vec3 tmpvar_15;
					  tmpvar_15.xy = (normal_14.xy * vec2(_V_CW_NormalMapStrength));
					  tmpvar_15.z = normal_14.z;
					  mediump vec3 tmpvar_16;
					  tmpvar_16 = normalize(tmpvar_15);
					  tmpvar_9 = tmpvar_16;
					  worldN_3.x = dot (xlv_TEXCOORD1, tmpvar_9);
					  worldN_3.y = dot (xlv_TEXCOORD2, tmpvar_9);
					  worldN_3.z = dot (xlv_TEXCOORD3, tmpvar_9);
					  tmpvar_1 = _LightColor0.xyz;
					  tmpvar_2 = lightDir_5;
					  lowp vec4 c_17;
					  lowp vec4 c_18;
					  lowp float diff_19;
					  mediump float tmpvar_20;
					  tmpvar_20 = max (0.0, dot (worldN_3, tmpvar_2));
					  diff_19 = tmpvar_20;
					  c_18.xyz = ((tmpvar_8 * tmpvar_1) * diff_19);
					  c_18.w = (tmpvar_10.w * _Color.w);
					  c_17.w = c_18.w;
					  c_17.xyz = c_18.xyz;
					  c_4.w = c_17.w;
					  highp float tmpvar_21;
					  tmpvar_21 = clamp (xlv_TEXCOORD5, 0.0, 1.0);
					  c_4.xyz = (c_18.xyz * vec3(tmpvar_21));
					  gl_FragData[0] = c_4;
					}
					
					
					#endif"
}
SubProgram "gles " {
Keywords { "SPOT" "FOG_LINEAR" "_EMISSION" "V_CW_VERTEX_COLOR" "_NORMALMAP" "V_CW_REFLECTIVE_OFF" "V_CW_RIM_OFF" "V_CW_SPECULAR_OFF" }
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
					uniform highp vec4 unity_WorldTransformParams;
					uniform highp vec4 unity_FogParams;
					uniform highp vec3 _V_CW_Bend;
					uniform highp vec3 _V_CW_Bias;
					uniform highp vec4 _V_CW_PivotPoint_Position;
					uniform highp vec4 _MainTex_ST;
					varying highp vec2 xlv_TEXCOORD0;
					varying lowp vec3 xlv_TEXCOORD1;
					varying lowp vec3 xlv_TEXCOORD2;
					varying lowp vec3 xlv_TEXCOORD3;
					varying highp vec3 xlv_TEXCOORD4;
					varying lowp vec4 xlv_COLOR0;
					varying highp float xlv_TEXCOORD5;
					void main ()
					{
					  lowp float tangentSign_1;
					  lowp vec3 worldTangent_2;
					  lowp vec3 worldNormal_3;
					  highp vec4 tmpvar_4;
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
					  tmpvar_4 = (glstate_matrix_mvp * vertex_23);
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
					  worldNormal_3 = tmpvar_43;
					  highp mat3 tmpvar_44;
					  tmpvar_44[0] = _Object2World[0].xyz;
					  tmpvar_44[1] = _Object2World[1].xyz;
					  tmpvar_44[2] = _Object2World[2].xyz;
					  highp vec3 tmpvar_45;
					  tmpvar_45 = normalize((tmpvar_44 * _glesTANGENT.xyz));
					  worldTangent_2 = tmpvar_45;
					  highp float tmpvar_46;
					  tmpvar_46 = (_glesTANGENT.w * unity_WorldTransformParams.w);
					  tangentSign_1 = tmpvar_46;
					  lowp vec3 tmpvar_47;
					  tmpvar_47 = (((worldNormal_3.yzx * worldTangent_2.zxy) - (worldNormal_3.zxy * worldTangent_2.yzx)) * tangentSign_1);
					  lowp vec3 tmpvar_48;
					  tmpvar_48.x = worldTangent_2.x;
					  tmpvar_48.y = tmpvar_47.x;
					  tmpvar_48.z = worldNormal_3.x;
					  lowp vec3 tmpvar_49;
					  tmpvar_49.x = worldTangent_2.y;
					  tmpvar_49.y = tmpvar_47.y;
					  tmpvar_49.z = worldNormal_3.y;
					  lowp vec3 tmpvar_50;
					  tmpvar_50.x = worldTangent_2.z;
					  tmpvar_50.y = tmpvar_47.z;
					  tmpvar_50.z = worldNormal_3.z;
					  gl_Position = tmpvar_4;
					  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
					  xlv_TEXCOORD1 = tmpvar_48;
					  xlv_TEXCOORD2 = tmpvar_49;
					  xlv_TEXCOORD3 = tmpvar_50;
					  xlv_TEXCOORD4 = (_Object2World * vertex_23).xyz;
					  xlv_COLOR0 = _glesColor;
					  xlv_TEXCOORD5 = ((tmpvar_4.z * unity_FogParams.z) + unity_FogParams.w);
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
					uniform sampler2D _V_CW_NormalMap;
					uniform mediump float _V_CW_NormalMap_UV_Scale;
					uniform mediump float _V_CW_NormalMapStrength;
					varying highp vec2 xlv_TEXCOORD0;
					varying lowp vec3 xlv_TEXCOORD1;
					varying lowp vec3 xlv_TEXCOORD2;
					varying lowp vec3 xlv_TEXCOORD3;
					varying highp vec3 xlv_TEXCOORD4;
					varying lowp vec4 xlv_COLOR0;
					varying highp float xlv_TEXCOORD5;
					void main ()
					{
					  mediump vec3 tmpvar_1;
					  mediump vec3 tmpvar_2;
					  lowp vec3 worldN_3;
					  lowp vec4 c_4;
					  lowp float atten_5;
					  mediump vec4 lightCoord_6;
					  lowp vec3 lightDir_7;
					  highp vec4 tmpvar_8;
					  highp vec3 tmpvar_9;
					  tmpvar_9 = normalize((_WorldSpaceLightPos0.xyz - xlv_TEXCOORD4));
					  lightDir_7 = tmpvar_9;
					  tmpvar_8 = xlv_COLOR0;
					  lowp vec3 tmpvar_10;
					  lowp vec3 tmpvar_11;
					  lowp vec4 tmpvar_12;
					  highp vec2 P_13;
					  highp vec2 tmpvar_14;
					  tmpvar_14 = (_V_CW_MainTex_Scroll * _Time.x);
					  P_13 = (xlv_TEXCOORD0 + tmpvar_14);
					  tmpvar_12 = texture2D (_MainTex, P_13);
					  tmpvar_10 = (tmpvar_12.xyz * _Color.xyz);
					  tmpvar_10 = (tmpvar_10 * tmpvar_8.xyz);
					  highp vec2 P_15;
					  P_15 = ((xlv_TEXCOORD0 * _V_CW_NormalMap_UV_Scale) + tmpvar_14);
					  lowp vec3 normal_16;
					  normal_16.xy = ((texture2D (_V_CW_NormalMap, P_15).wy * 2.0) - 1.0);
					  normal_16.z = sqrt((1.0 - clamp (
					    dot (normal_16.xy, normal_16.xy)
					  , 0.0, 1.0)));
					  mediump vec3 tmpvar_17;
					  tmpvar_17.xy = (normal_16.xy * vec2(_V_CW_NormalMapStrength));
					  tmpvar_17.z = normal_16.z;
					  mediump vec3 tmpvar_18;
					  tmpvar_18 = normalize(tmpvar_17);
					  tmpvar_11 = tmpvar_18;
					  highp vec4 tmpvar_19;
					  tmpvar_19.w = 1.0;
					  tmpvar_19.xyz = xlv_TEXCOORD4;
					  highp vec4 tmpvar_20;
					  tmpvar_20 = (_LightMatrix0 * tmpvar_19);
					  lightCoord_6 = tmpvar_20;
					  lowp vec4 tmpvar_21;
					  mediump vec2 P_22;
					  P_22 = ((lightCoord_6.xy / lightCoord_6.w) + 0.5);
					  tmpvar_21 = texture2D (_LightTexture0, P_22);
					  highp vec3 LightCoord_23;
					  LightCoord_23 = lightCoord_6.xyz;
					  highp float tmpvar_24;
					  tmpvar_24 = dot (LightCoord_23, LightCoord_23);
					  lowp vec4 tmpvar_25;
					  tmpvar_25 = texture2D (_LightTextureB0, vec2(tmpvar_24));
					  mediump float tmpvar_26;
					  tmpvar_26 = ((float(
					    (lightCoord_6.z > 0.0)
					  ) * tmpvar_21.w) * tmpvar_25.w);
					  atten_5 = tmpvar_26;
					  worldN_3.x = dot (xlv_TEXCOORD1, tmpvar_11);
					  worldN_3.y = dot (xlv_TEXCOORD2, tmpvar_11);
					  worldN_3.z = dot (xlv_TEXCOORD3, tmpvar_11);
					  tmpvar_1 = _LightColor0.xyz;
					  tmpvar_2 = lightDir_7;
					  tmpvar_1 = (tmpvar_1 * atten_5);
					  lowp vec4 c_27;
					  lowp vec4 c_28;
					  lowp float diff_29;
					  mediump float tmpvar_30;
					  tmpvar_30 = max (0.0, dot (worldN_3, tmpvar_2));
					  diff_29 = tmpvar_30;
					  c_28.xyz = ((tmpvar_10 * tmpvar_1) * diff_29);
					  c_28.w = (tmpvar_12.w * _Color.w);
					  c_27.w = c_28.w;
					  c_27.xyz = c_28.xyz;
					  c_4.w = c_27.w;
					  highp float tmpvar_31;
					  tmpvar_31 = clamp (xlv_TEXCOORD5, 0.0, 1.0);
					  c_4.xyz = (c_28.xyz * vec3(tmpvar_31));
					  gl_FragData[0] = c_4;
					}
					
					
					#endif"
}
SubProgram "gles " {
Keywords { "POINT_COOKIE" "FOG_LINEAR" "_EMISSION" "V_CW_VERTEX_COLOR" "_NORMALMAP" "V_CW_REFLECTIVE_OFF" "V_CW_RIM_OFF" "V_CW_SPECULAR_OFF" }
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
					uniform highp vec4 unity_WorldTransformParams;
					uniform highp vec4 unity_FogParams;
					uniform highp vec3 _V_CW_Bend;
					uniform highp vec3 _V_CW_Bias;
					uniform highp vec4 _V_CW_PivotPoint_Position;
					uniform highp vec4 _MainTex_ST;
					varying highp vec2 xlv_TEXCOORD0;
					varying lowp vec3 xlv_TEXCOORD1;
					varying lowp vec3 xlv_TEXCOORD2;
					varying lowp vec3 xlv_TEXCOORD3;
					varying highp vec3 xlv_TEXCOORD4;
					varying lowp vec4 xlv_COLOR0;
					varying highp float xlv_TEXCOORD5;
					void main ()
					{
					  lowp float tangentSign_1;
					  lowp vec3 worldTangent_2;
					  lowp vec3 worldNormal_3;
					  highp vec4 tmpvar_4;
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
					  tmpvar_4 = (glstate_matrix_mvp * vertex_23);
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
					  worldNormal_3 = tmpvar_43;
					  highp mat3 tmpvar_44;
					  tmpvar_44[0] = _Object2World[0].xyz;
					  tmpvar_44[1] = _Object2World[1].xyz;
					  tmpvar_44[2] = _Object2World[2].xyz;
					  highp vec3 tmpvar_45;
					  tmpvar_45 = normalize((tmpvar_44 * _glesTANGENT.xyz));
					  worldTangent_2 = tmpvar_45;
					  highp float tmpvar_46;
					  tmpvar_46 = (_glesTANGENT.w * unity_WorldTransformParams.w);
					  tangentSign_1 = tmpvar_46;
					  lowp vec3 tmpvar_47;
					  tmpvar_47 = (((worldNormal_3.yzx * worldTangent_2.zxy) - (worldNormal_3.zxy * worldTangent_2.yzx)) * tangentSign_1);
					  lowp vec3 tmpvar_48;
					  tmpvar_48.x = worldTangent_2.x;
					  tmpvar_48.y = tmpvar_47.x;
					  tmpvar_48.z = worldNormal_3.x;
					  lowp vec3 tmpvar_49;
					  tmpvar_49.x = worldTangent_2.y;
					  tmpvar_49.y = tmpvar_47.y;
					  tmpvar_49.z = worldNormal_3.y;
					  lowp vec3 tmpvar_50;
					  tmpvar_50.x = worldTangent_2.z;
					  tmpvar_50.y = tmpvar_47.z;
					  tmpvar_50.z = worldNormal_3.z;
					  gl_Position = tmpvar_4;
					  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
					  xlv_TEXCOORD1 = tmpvar_48;
					  xlv_TEXCOORD2 = tmpvar_49;
					  xlv_TEXCOORD3 = tmpvar_50;
					  xlv_TEXCOORD4 = (_Object2World * vertex_23).xyz;
					  xlv_COLOR0 = _glesColor;
					  xlv_TEXCOORD5 = ((tmpvar_4.z * unity_FogParams.z) + unity_FogParams.w);
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
					uniform sampler2D _V_CW_NormalMap;
					uniform mediump float _V_CW_NormalMap_UV_Scale;
					uniform mediump float _V_CW_NormalMapStrength;
					varying highp vec2 xlv_TEXCOORD0;
					varying lowp vec3 xlv_TEXCOORD1;
					varying lowp vec3 xlv_TEXCOORD2;
					varying lowp vec3 xlv_TEXCOORD3;
					varying highp vec3 xlv_TEXCOORD4;
					varying lowp vec4 xlv_COLOR0;
					varying highp float xlv_TEXCOORD5;
					void main ()
					{
					  mediump vec3 tmpvar_1;
					  mediump vec3 tmpvar_2;
					  lowp vec3 worldN_3;
					  lowp vec4 c_4;
					  lowp vec3 lightDir_5;
					  highp vec4 tmpvar_6;
					  highp vec3 tmpvar_7;
					  tmpvar_7 = normalize((_WorldSpaceLightPos0.xyz - xlv_TEXCOORD4));
					  lightDir_5 = tmpvar_7;
					  tmpvar_6 = xlv_COLOR0;
					  lowp vec3 tmpvar_8;
					  lowp vec3 tmpvar_9;
					  lowp vec4 tmpvar_10;
					  highp vec2 P_11;
					  highp vec2 tmpvar_12;
					  tmpvar_12 = (_V_CW_MainTex_Scroll * _Time.x);
					  P_11 = (xlv_TEXCOORD0 + tmpvar_12);
					  tmpvar_10 = texture2D (_MainTex, P_11);
					  tmpvar_8 = (tmpvar_10.xyz * _Color.xyz);
					  tmpvar_8 = (tmpvar_8 * tmpvar_6.xyz);
					  highp vec2 P_13;
					  P_13 = ((xlv_TEXCOORD0 * _V_CW_NormalMap_UV_Scale) + tmpvar_12);
					  lowp vec3 normal_14;
					  normal_14.xy = ((texture2D (_V_CW_NormalMap, P_13).wy * 2.0) - 1.0);
					  normal_14.z = sqrt((1.0 - clamp (
					    dot (normal_14.xy, normal_14.xy)
					  , 0.0, 1.0)));
					  mediump vec3 tmpvar_15;
					  tmpvar_15.xy = (normal_14.xy * vec2(_V_CW_NormalMapStrength));
					  tmpvar_15.z = normal_14.z;
					  mediump vec3 tmpvar_16;
					  tmpvar_16 = normalize(tmpvar_15);
					  tmpvar_9 = tmpvar_16;
					  highp vec4 tmpvar_17;
					  tmpvar_17.w = 1.0;
					  tmpvar_17.xyz = xlv_TEXCOORD4;
					  highp vec3 tmpvar_18;
					  tmpvar_18 = (_LightMatrix0 * tmpvar_17).xyz;
					  highp float tmpvar_19;
					  tmpvar_19 = dot (tmpvar_18, tmpvar_18);
					  lowp float tmpvar_20;
					  tmpvar_20 = (texture2D (_LightTextureB0, vec2(tmpvar_19)).w * textureCube (_LightTexture0, tmpvar_18).w);
					  worldN_3.x = dot (xlv_TEXCOORD1, tmpvar_9);
					  worldN_3.y = dot (xlv_TEXCOORD2, tmpvar_9);
					  worldN_3.z = dot (xlv_TEXCOORD3, tmpvar_9);
					  tmpvar_1 = _LightColor0.xyz;
					  tmpvar_2 = lightDir_5;
					  tmpvar_1 = (tmpvar_1 * tmpvar_20);
					  lowp vec4 c_21;
					  lowp vec4 c_22;
					  lowp float diff_23;
					  mediump float tmpvar_24;
					  tmpvar_24 = max (0.0, dot (worldN_3, tmpvar_2));
					  diff_23 = tmpvar_24;
					  c_22.xyz = ((tmpvar_8 * tmpvar_1) * diff_23);
					  c_22.w = (tmpvar_10.w * _Color.w);
					  c_21.w = c_22.w;
					  c_21.xyz = c_22.xyz;
					  c_4.w = c_21.w;
					  highp float tmpvar_25;
					  tmpvar_25 = clamp (xlv_TEXCOORD5, 0.0, 1.0);
					  c_4.xyz = (c_22.xyz * vec3(tmpvar_25));
					  gl_FragData[0] = c_4;
					}
					
					
					#endif"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL_COOKIE" "FOG_LINEAR" "_EMISSION" "V_CW_VERTEX_COLOR" "_NORMALMAP" "V_CW_REFLECTIVE_OFF" "V_CW_RIM_OFF" "V_CW_SPECULAR_OFF" }
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
					uniform highp vec4 unity_WorldTransformParams;
					uniform highp vec4 unity_FogParams;
					uniform highp vec3 _V_CW_Bend;
					uniform highp vec3 _V_CW_Bias;
					uniform highp vec4 _V_CW_PivotPoint_Position;
					uniform highp vec4 _MainTex_ST;
					varying highp vec2 xlv_TEXCOORD0;
					varying lowp vec3 xlv_TEXCOORD1;
					varying lowp vec3 xlv_TEXCOORD2;
					varying lowp vec3 xlv_TEXCOORD3;
					varying highp vec3 xlv_TEXCOORD4;
					varying lowp vec4 xlv_COLOR0;
					varying highp float xlv_TEXCOORD5;
					void main ()
					{
					  lowp float tangentSign_1;
					  lowp vec3 worldTangent_2;
					  lowp vec3 worldNormal_3;
					  highp vec4 tmpvar_4;
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
					  tmpvar_4 = (glstate_matrix_mvp * vertex_23);
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
					  worldNormal_3 = tmpvar_43;
					  highp mat3 tmpvar_44;
					  tmpvar_44[0] = _Object2World[0].xyz;
					  tmpvar_44[1] = _Object2World[1].xyz;
					  tmpvar_44[2] = _Object2World[2].xyz;
					  highp vec3 tmpvar_45;
					  tmpvar_45 = normalize((tmpvar_44 * _glesTANGENT.xyz));
					  worldTangent_2 = tmpvar_45;
					  highp float tmpvar_46;
					  tmpvar_46 = (_glesTANGENT.w * unity_WorldTransformParams.w);
					  tangentSign_1 = tmpvar_46;
					  lowp vec3 tmpvar_47;
					  tmpvar_47 = (((worldNormal_3.yzx * worldTangent_2.zxy) - (worldNormal_3.zxy * worldTangent_2.yzx)) * tangentSign_1);
					  lowp vec3 tmpvar_48;
					  tmpvar_48.x = worldTangent_2.x;
					  tmpvar_48.y = tmpvar_47.x;
					  tmpvar_48.z = worldNormal_3.x;
					  lowp vec3 tmpvar_49;
					  tmpvar_49.x = worldTangent_2.y;
					  tmpvar_49.y = tmpvar_47.y;
					  tmpvar_49.z = worldNormal_3.y;
					  lowp vec3 tmpvar_50;
					  tmpvar_50.x = worldTangent_2.z;
					  tmpvar_50.y = tmpvar_47.z;
					  tmpvar_50.z = worldNormal_3.z;
					  gl_Position = tmpvar_4;
					  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
					  xlv_TEXCOORD1 = tmpvar_48;
					  xlv_TEXCOORD2 = tmpvar_49;
					  xlv_TEXCOORD3 = tmpvar_50;
					  xlv_TEXCOORD4 = (_Object2World * vertex_23).xyz;
					  xlv_COLOR0 = _glesColor;
					  xlv_TEXCOORD5 = ((tmpvar_4.z * unity_FogParams.z) + unity_FogParams.w);
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
					uniform sampler2D _V_CW_NormalMap;
					uniform mediump float _V_CW_NormalMap_UV_Scale;
					uniform mediump float _V_CW_NormalMapStrength;
					varying highp vec2 xlv_TEXCOORD0;
					varying lowp vec3 xlv_TEXCOORD1;
					varying lowp vec3 xlv_TEXCOORD2;
					varying lowp vec3 xlv_TEXCOORD3;
					varying highp vec3 xlv_TEXCOORD4;
					varying lowp vec4 xlv_COLOR0;
					varying highp float xlv_TEXCOORD5;
					void main ()
					{
					  mediump vec3 tmpvar_1;
					  mediump vec3 tmpvar_2;
					  lowp vec3 worldN_3;
					  lowp vec4 c_4;
					  lowp vec3 lightDir_5;
					  highp vec4 tmpvar_6;
					  mediump vec3 tmpvar_7;
					  tmpvar_7 = _WorldSpaceLightPos0.xyz;
					  lightDir_5 = tmpvar_7;
					  tmpvar_6 = xlv_COLOR0;
					  lowp vec3 tmpvar_8;
					  lowp vec3 tmpvar_9;
					  lowp vec4 tmpvar_10;
					  highp vec2 P_11;
					  highp vec2 tmpvar_12;
					  tmpvar_12 = (_V_CW_MainTex_Scroll * _Time.x);
					  P_11 = (xlv_TEXCOORD0 + tmpvar_12);
					  tmpvar_10 = texture2D (_MainTex, P_11);
					  tmpvar_8 = (tmpvar_10.xyz * _Color.xyz);
					  tmpvar_8 = (tmpvar_8 * tmpvar_6.xyz);
					  highp vec2 P_13;
					  P_13 = ((xlv_TEXCOORD0 * _V_CW_NormalMap_UV_Scale) + tmpvar_12);
					  lowp vec3 normal_14;
					  normal_14.xy = ((texture2D (_V_CW_NormalMap, P_13).wy * 2.0) - 1.0);
					  normal_14.z = sqrt((1.0 - clamp (
					    dot (normal_14.xy, normal_14.xy)
					  , 0.0, 1.0)));
					  mediump vec3 tmpvar_15;
					  tmpvar_15.xy = (normal_14.xy * vec2(_V_CW_NormalMapStrength));
					  tmpvar_15.z = normal_14.z;
					  mediump vec3 tmpvar_16;
					  tmpvar_16 = normalize(tmpvar_15);
					  tmpvar_9 = tmpvar_16;
					  highp vec4 tmpvar_17;
					  tmpvar_17.w = 1.0;
					  tmpvar_17.xyz = xlv_TEXCOORD4;
					  highp vec2 tmpvar_18;
					  tmpvar_18 = (_LightMatrix0 * tmpvar_17).xy;
					  lowp float tmpvar_19;
					  tmpvar_19 = texture2D (_LightTexture0, tmpvar_18).w;
					  worldN_3.x = dot (xlv_TEXCOORD1, tmpvar_9);
					  worldN_3.y = dot (xlv_TEXCOORD2, tmpvar_9);
					  worldN_3.z = dot (xlv_TEXCOORD3, tmpvar_9);
					  tmpvar_1 = _LightColor0.xyz;
					  tmpvar_2 = lightDir_5;
					  tmpvar_1 = (tmpvar_1 * tmpvar_19);
					  lowp vec4 c_20;
					  lowp vec4 c_21;
					  lowp float diff_22;
					  mediump float tmpvar_23;
					  tmpvar_23 = max (0.0, dot (worldN_3, tmpvar_2));
					  diff_22 = tmpvar_23;
					  c_21.xyz = ((tmpvar_8 * tmpvar_1) * diff_22);
					  c_21.w = (tmpvar_10.w * _Color.w);
					  c_20.w = c_21.w;
					  c_20.xyz = c_21.xyz;
					  c_4.w = c_20.w;
					  highp float tmpvar_24;
					  tmpvar_24 = clamp (xlv_TEXCOORD5, 0.0, 1.0);
					  c_4.xyz = (c_21.xyz * vec3(tmpvar_24));
					  gl_FragData[0] = c_4;
					}
					
					
					#endif"
}
}
Program "fp" {
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
SubProgram "gles " {
Keywords { "POINT" "V_CW_SPECULAR" "V_CW_VERTEX_COLOR" "V_CW_REFLECTIVE_OFF" "_EMISSION_OFF" "V_CW_RIM_OFF" "_NORMALMAP_OFF" }
					"!!GLES"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "V_CW_SPECULAR" "V_CW_VERTEX_COLOR" "V_CW_REFLECTIVE_OFF" "_EMISSION_OFF" "V_CW_RIM_OFF" "_NORMALMAP_OFF" }
					"!!GLES"
}
SubProgram "gles " {
Keywords { "SPOT" "V_CW_SPECULAR" "V_CW_VERTEX_COLOR" "V_CW_REFLECTIVE_OFF" "_EMISSION_OFF" "V_CW_RIM_OFF" "_NORMALMAP_OFF" }
					"!!GLES"
}
SubProgram "gles " {
Keywords { "POINT_COOKIE" "V_CW_SPECULAR" "V_CW_VERTEX_COLOR" "V_CW_REFLECTIVE_OFF" "_EMISSION_OFF" "V_CW_RIM_OFF" "_NORMALMAP_OFF" }
					"!!GLES"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL_COOKIE" "V_CW_SPECULAR" "V_CW_VERTEX_COLOR" "V_CW_REFLECTIVE_OFF" "_EMISSION_OFF" "V_CW_RIM_OFF" "_NORMALMAP_OFF" }
					"!!GLES"
}
SubProgram "gles " {
Keywords { "POINT" "FOG_LINEAR" "V_CW_SPECULAR" "V_CW_VERTEX_COLOR" "V_CW_REFLECTIVE_OFF" "_EMISSION_OFF" "V_CW_RIM_OFF" "_NORMALMAP_OFF" }
					"!!GLES"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "FOG_LINEAR" "V_CW_SPECULAR" "V_CW_VERTEX_COLOR" "V_CW_REFLECTIVE_OFF" "_EMISSION_OFF" "V_CW_RIM_OFF" "_NORMALMAP_OFF" }
					"!!GLES"
}
SubProgram "gles " {
Keywords { "SPOT" "FOG_LINEAR" "V_CW_SPECULAR" "V_CW_VERTEX_COLOR" "V_CW_REFLECTIVE_OFF" "_EMISSION_OFF" "V_CW_RIM_OFF" "_NORMALMAP_OFF" }
					"!!GLES"
}
SubProgram "gles " {
Keywords { "POINT_COOKIE" "FOG_LINEAR" "V_CW_SPECULAR" "V_CW_VERTEX_COLOR" "V_CW_REFLECTIVE_OFF" "_EMISSION_OFF" "V_CW_RIM_OFF" "_NORMALMAP_OFF" }
					"!!GLES"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL_COOKIE" "FOG_LINEAR" "V_CW_SPECULAR" "V_CW_VERTEX_COLOR" "V_CW_REFLECTIVE_OFF" "_EMISSION_OFF" "V_CW_RIM_OFF" "_NORMALMAP_OFF" }
					"!!GLES"
}
SubProgram "gles " {
Keywords { "POINT" "_EMISSION" "V_CW_VERTEX_COLOR" "_NORMALMAP" "V_CW_REFLECTIVE_OFF" "V_CW_RIM_OFF" "V_CW_SPECULAR_OFF" }
					"!!GLES"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "_EMISSION" "V_CW_VERTEX_COLOR" "_NORMALMAP" "V_CW_REFLECTIVE_OFF" "V_CW_RIM_OFF" "V_CW_SPECULAR_OFF" }
					"!!GLES"
}
SubProgram "gles " {
Keywords { "SPOT" "_EMISSION" "V_CW_VERTEX_COLOR" "_NORMALMAP" "V_CW_REFLECTIVE_OFF" "V_CW_RIM_OFF" "V_CW_SPECULAR_OFF" }
					"!!GLES"
}
SubProgram "gles " {
Keywords { "POINT_COOKIE" "_EMISSION" "V_CW_VERTEX_COLOR" "_NORMALMAP" "V_CW_REFLECTIVE_OFF" "V_CW_RIM_OFF" "V_CW_SPECULAR_OFF" }
					"!!GLES"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL_COOKIE" "_EMISSION" "V_CW_VERTEX_COLOR" "_NORMALMAP" "V_CW_REFLECTIVE_OFF" "V_CW_RIM_OFF" "V_CW_SPECULAR_OFF" }
					"!!GLES"
}
SubProgram "gles " {
Keywords { "POINT" "FOG_LINEAR" "_EMISSION" "V_CW_VERTEX_COLOR" "_NORMALMAP" "V_CW_REFLECTIVE_OFF" "V_CW_RIM_OFF" "V_CW_SPECULAR_OFF" }
					"!!GLES"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "FOG_LINEAR" "_EMISSION" "V_CW_VERTEX_COLOR" "_NORMALMAP" "V_CW_REFLECTIVE_OFF" "V_CW_RIM_OFF" "V_CW_SPECULAR_OFF" }
					"!!GLES"
}
SubProgram "gles " {
Keywords { "SPOT" "FOG_LINEAR" "_EMISSION" "V_CW_VERTEX_COLOR" "_NORMALMAP" "V_CW_REFLECTIVE_OFF" "V_CW_RIM_OFF" "V_CW_SPECULAR_OFF" }
					"!!GLES"
}
SubProgram "gles " {
Keywords { "POINT_COOKIE" "FOG_LINEAR" "_EMISSION" "V_CW_VERTEX_COLOR" "_NORMALMAP" "V_CW_REFLECTIVE_OFF" "V_CW_RIM_OFF" "V_CW_SPECULAR_OFF" }
					"!!GLES"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL_COOKIE" "FOG_LINEAR" "_EMISSION" "V_CW_VERTEX_COLOR" "_NORMALMAP" "V_CW_REFLECTIVE_OFF" "V_CW_RIM_OFF" "V_CW_SPECULAR_OFF" }
					"!!GLES"
}
}
 }
}
Fallback "Hidden/VacuumShaders/Curved World/VertexLit/Transparent"
}