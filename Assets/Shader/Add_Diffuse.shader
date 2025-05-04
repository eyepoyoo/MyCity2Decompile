Shader "Hidden/VacuumShaders/Curved World/Legacy Shader/Additive/Diffuse" {
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
 Tags { "QUEUE"="Transparent" "IGNOREPROJECTOR"="true" "RenderType"="Transparent" "CurvedWorldTag"="Legacy Shader/Additive/Diffuse" "CurvedWorldNoneRemoveableKeywords"="" "CurvedWorldAvailableOptions"="V_CW_REFLECTIVE;V_CW_VERTEX_COLOR;_EMISSION;V_CW_RIM;_NORMALMAP;V_CW_SPECULAR_HD;" }
 Pass {
  Name "FORWARD"
  Tags { "LIGHTMODE"="ForwardBase" "QUEUE"="Transparent" "IGNOREPROJECTOR"="true" "RenderType"="Transparent" "CurvedWorldTag"="Legacy Shader/Additive/Diffuse" "CurvedWorldNoneRemoveableKeywords"="" "CurvedWorldAvailableOptions"="V_CW_REFLECTIVE;V_CW_VERTEX_COLOR;_EMISSION;V_CW_RIM;_NORMALMAP;V_CW_SPECULAR_HD;" }
  ZWrite Off
  Cull Off
  Blend One OneMinusSrcColor
  GpuProgramID 4793
Program "vp" {
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_OFF" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "V_CW_REFLECTIVE_OFF" "V_CW_VERTEX_COLOR_OFF" "_EMISSION_OFF" "V_CW_RIM_OFF" "_NORMALMAP_OFF" "V_CW_SPECULAR_OFF" }
					"!!GLES
					#version 100
					
					#ifdef VERTEX
					attribute vec4 _glesTANGENT;
					attribute vec4 _glesVertex;
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
					varying mediump vec3 xlv_TEXCOORD3;
					void main ()
					{
					  mediump vec3 tmpvar_1;
					  mediump vec3 tmpvar_2;
					  lowp vec4 c_3;
					  lowp vec3 tmpvar_4;
					  lowp vec3 lightDir_5;
					  mediump vec3 tmpvar_6;
					  tmpvar_6 = _WorldSpaceLightPos0.xyz;
					  lightDir_5 = tmpvar_6;
					  tmpvar_4 = xlv_TEXCOORD1;
					  lowp vec3 tmpvar_7;
					  lowp vec4 tmpvar_8;
					  highp vec2 P_9;
					  P_9 = (xlv_TEXCOORD0 + (_V_CW_MainTex_Scroll * _Time.x));
					  tmpvar_8 = texture2D (_MainTex, P_9);
					  tmpvar_7 = (tmpvar_8.xyz * _Color.xyz);
					  tmpvar_1 = _LightColor0.xyz;
					  tmpvar_2 = lightDir_5;
					  mediump vec3 normalWorld_10;
					  normalWorld_10 = tmpvar_4;
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
					  tmpvar_17 = max (0.0, dot (tmpvar_4, tmpvar_2));
					  diff_16 = tmpvar_17;
					  c_15.xyz = ((tmpvar_7 * tmpvar_1) * diff_16);
					  c_15.w = (tmpvar_8.w * _Color.w);
					  c_14.w = c_15.w;
					  c_14.xyz = (c_15.xyz + (tmpvar_7 * ambient_11));
					  c_3.xyz = c_14.xyz;
					  c_3.w = 1.0;
					  gl_FragData[0] = c_3;
					}
					
					
					#endif"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_OFF" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "VERTEXLIGHT_ON" "V_CW_REFLECTIVE_OFF" "V_CW_VERTEX_COLOR_OFF" "_EMISSION_OFF" "V_CW_RIM_OFF" "_NORMALMAP_OFF" "V_CW_SPECULAR_OFF" }
					"!!GLES
					#version 100
					
					#ifdef VERTEX
					attribute vec4 _glesTANGENT;
					attribute vec4 _glesVertex;
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
					varying mediump vec3 xlv_TEXCOORD3;
					void main ()
					{
					  mediump vec3 tmpvar_1;
					  mediump vec3 tmpvar_2;
					  lowp vec4 c_3;
					  lowp vec3 tmpvar_4;
					  lowp vec3 lightDir_5;
					  mediump vec3 tmpvar_6;
					  tmpvar_6 = _WorldSpaceLightPos0.xyz;
					  lightDir_5 = tmpvar_6;
					  tmpvar_4 = xlv_TEXCOORD1;
					  lowp vec3 tmpvar_7;
					  lowp vec4 tmpvar_8;
					  highp vec2 P_9;
					  P_9 = (xlv_TEXCOORD0 + (_V_CW_MainTex_Scroll * _Time.x));
					  tmpvar_8 = texture2D (_MainTex, P_9);
					  tmpvar_7 = (tmpvar_8.xyz * _Color.xyz);
					  tmpvar_1 = _LightColor0.xyz;
					  tmpvar_2 = lightDir_5;
					  mediump vec3 normalWorld_10;
					  normalWorld_10 = tmpvar_4;
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
					  tmpvar_17 = max (0.0, dot (tmpvar_4, tmpvar_2));
					  diff_16 = tmpvar_17;
					  c_15.xyz = ((tmpvar_7 * tmpvar_1) * diff_16);
					  c_15.w = (tmpvar_8.w * _Color.w);
					  c_14.w = c_15.w;
					  c_14.xyz = (c_15.xyz + (tmpvar_7 * ambient_11));
					  c_3.xyz = c_14.xyz;
					  c_3.w = 1.0;
					  gl_FragData[0] = c_3;
					}
					
					
					#endif"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_OFF" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "FOG_LINEAR" "V_CW_REFLECTIVE_OFF" "V_CW_VERTEX_COLOR_OFF" "_EMISSION_OFF" "V_CW_RIM_OFF" "_NORMALMAP_OFF" "V_CW_SPECULAR_OFF" }
					"!!GLES
					#version 100
					
					#ifdef VERTEX
					attribute vec4 _glesTANGENT;
					attribute vec4 _glesVertex;
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
					varying mediump vec3 xlv_TEXCOORD3;
					varying highp float xlv_TEXCOORD4;
					void main ()
					{
					  mediump vec3 tmpvar_1;
					  mediump vec3 tmpvar_2;
					  lowp vec4 c_3;
					  lowp vec3 tmpvar_4;
					  lowp vec3 lightDir_5;
					  mediump vec3 tmpvar_6;
					  tmpvar_6 = _WorldSpaceLightPos0.xyz;
					  lightDir_5 = tmpvar_6;
					  tmpvar_4 = xlv_TEXCOORD1;
					  lowp vec3 tmpvar_7;
					  lowp vec4 tmpvar_8;
					  highp vec2 P_9;
					  P_9 = (xlv_TEXCOORD0 + (_V_CW_MainTex_Scroll * _Time.x));
					  tmpvar_8 = texture2D (_MainTex, P_9);
					  tmpvar_7 = (tmpvar_8.xyz * _Color.xyz);
					  tmpvar_1 = _LightColor0.xyz;
					  tmpvar_2 = lightDir_5;
					  mediump vec3 normalWorld_10;
					  normalWorld_10 = tmpvar_4;
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
					  tmpvar_17 = max (0.0, dot (tmpvar_4, tmpvar_2));
					  diff_16 = tmpvar_17;
					  c_15.xyz = ((tmpvar_7 * tmpvar_1) * diff_16);
					  c_15.w = (tmpvar_8.w * _Color.w);
					  c_14.w = c_15.w;
					  c_14.xyz = (c_15.xyz + (tmpvar_7 * ambient_11));
					  highp float tmpvar_18;
					  tmpvar_18 = clamp (xlv_TEXCOORD4, 0.0, 1.0);
					  c_3.xyz = mix (unity_FogColor.xyz, c_14.xyz, vec3(tmpvar_18));
					  c_3.w = 1.0;
					  gl_FragData[0] = c_3;
					}
					
					
					#endif"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_OFF" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "FOG_LINEAR" "VERTEXLIGHT_ON" "V_CW_REFLECTIVE_OFF" "V_CW_VERTEX_COLOR_OFF" "_EMISSION_OFF" "V_CW_RIM_OFF" "_NORMALMAP_OFF" "V_CW_SPECULAR_OFF" }
					"!!GLES
					#version 100
					
					#ifdef VERTEX
					attribute vec4 _glesTANGENT;
					attribute vec4 _glesVertex;
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
					varying mediump vec3 xlv_TEXCOORD3;
					varying highp float xlv_TEXCOORD4;
					void main ()
					{
					  mediump vec3 tmpvar_1;
					  mediump vec3 tmpvar_2;
					  lowp vec4 c_3;
					  lowp vec3 tmpvar_4;
					  lowp vec3 lightDir_5;
					  mediump vec3 tmpvar_6;
					  tmpvar_6 = _WorldSpaceLightPos0.xyz;
					  lightDir_5 = tmpvar_6;
					  tmpvar_4 = xlv_TEXCOORD1;
					  lowp vec3 tmpvar_7;
					  lowp vec4 tmpvar_8;
					  highp vec2 P_9;
					  P_9 = (xlv_TEXCOORD0 + (_V_CW_MainTex_Scroll * _Time.x));
					  tmpvar_8 = texture2D (_MainTex, P_9);
					  tmpvar_7 = (tmpvar_8.xyz * _Color.xyz);
					  tmpvar_1 = _LightColor0.xyz;
					  tmpvar_2 = lightDir_5;
					  mediump vec3 normalWorld_10;
					  normalWorld_10 = tmpvar_4;
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
					  tmpvar_17 = max (0.0, dot (tmpvar_4, tmpvar_2));
					  diff_16 = tmpvar_17;
					  c_15.xyz = ((tmpvar_7 * tmpvar_1) * diff_16);
					  c_15.w = (tmpvar_8.w * _Color.w);
					  c_14.w = c_15.w;
					  c_14.xyz = (c_15.xyz + (tmpvar_7 * ambient_11));
					  highp float tmpvar_18;
					  tmpvar_18 = clamp (xlv_TEXCOORD4, 0.0, 1.0);
					  c_3.xyz = mix (unity_FogColor.xyz, c_14.xyz, vec3(tmpvar_18));
					  c_3.w = 1.0;
					  gl_FragData[0] = c_3;
					}
					
					
					#endif"
}
}
Program "fp" {
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_OFF" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "V_CW_REFLECTIVE_OFF" "V_CW_VERTEX_COLOR_OFF" "_EMISSION_OFF" "V_CW_RIM_OFF" "_NORMALMAP_OFF" "V_CW_SPECULAR_OFF" }
					"!!GLES"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_OFF" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "FOG_LINEAR" "V_CW_REFLECTIVE_OFF" "V_CW_VERTEX_COLOR_OFF" "_EMISSION_OFF" "V_CW_RIM_OFF" "_NORMALMAP_OFF" "V_CW_SPECULAR_OFF" }
					"!!GLES"
}
}
 }
 Pass {
  Name "FORWARD"
  Tags { "LIGHTMODE"="ForwardAdd" "QUEUE"="Transparent" "IGNOREPROJECTOR"="true" "RenderType"="Transparent" "CurvedWorldTag"="Legacy Shader/Additive/Diffuse" "CurvedWorldNoneRemoveableKeywords"="" "CurvedWorldAvailableOptions"="V_CW_REFLECTIVE;V_CW_VERTEX_COLOR;_EMISSION;V_CW_RIM;_NORMALMAP;V_CW_SPECULAR_HD;" }
  ZWrite Off
  Cull Off
  Blend One One
  GpuProgramID 112187
Program "vp" {
SubProgram "gles " {
Keywords { "POINT" "V_CW_REFLECTIVE_OFF" "V_CW_VERTEX_COLOR_OFF" "_EMISSION_OFF" "V_CW_RIM_OFF" "_NORMALMAP_OFF" "V_CW_SPECULAR_OFF" }
					"!!GLES
					#version 100
					
					#ifdef VERTEX
					attribute vec4 _glesTANGENT;
					attribute vec4 _glesVertex;
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
					void main ()
					{
					  mediump vec3 tmpvar_1;
					  mediump vec3 tmpvar_2;
					  lowp vec4 c_3;
					  lowp vec3 tmpvar_4;
					  lowp vec3 lightDir_5;
					  highp vec3 tmpvar_6;
					  tmpvar_6 = normalize((_WorldSpaceLightPos0.xyz - xlv_TEXCOORD2));
					  lightDir_5 = tmpvar_6;
					  tmpvar_4 = xlv_TEXCOORD1;
					  lowp vec4 tmpvar_7;
					  highp vec2 P_8;
					  P_8 = (xlv_TEXCOORD0 + (_V_CW_MainTex_Scroll * _Time.x));
					  tmpvar_7 = texture2D (_MainTex, P_8);
					  highp vec4 tmpvar_9;
					  tmpvar_9.w = 1.0;
					  tmpvar_9.xyz = xlv_TEXCOORD2;
					  highp vec3 tmpvar_10;
					  tmpvar_10 = (_LightMatrix0 * tmpvar_9).xyz;
					  highp float tmpvar_11;
					  tmpvar_11 = dot (tmpvar_10, tmpvar_10);
					  lowp float tmpvar_12;
					  tmpvar_12 = texture2D (_LightTexture0, vec2(tmpvar_11)).w;
					  tmpvar_1 = _LightColor0.xyz;
					  tmpvar_2 = lightDir_5;
					  tmpvar_1 = (tmpvar_1 * tmpvar_12);
					  lowp vec4 c_13;
					  lowp vec4 c_14;
					  lowp float diff_15;
					  mediump float tmpvar_16;
					  tmpvar_16 = max (0.0, dot (tmpvar_4, tmpvar_2));
					  diff_15 = tmpvar_16;
					  c_14.xyz = ((tmpvar_7.xyz * _Color.xyz) * (tmpvar_1 * diff_15));
					  c_14.w = (tmpvar_7.w * _Color.w);
					  c_13.w = c_14.w;
					  c_13.xyz = c_14.xyz;
					  c_3.xyz = c_13.xyz;
					  c_3.w = 1.0;
					  gl_FragData[0] = c_3;
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
					void main ()
					{
					  mediump vec3 tmpvar_1;
					  mediump vec3 tmpvar_2;
					  lowp vec4 c_3;
					  lowp vec3 tmpvar_4;
					  lowp vec3 lightDir_5;
					  mediump vec3 tmpvar_6;
					  tmpvar_6 = _WorldSpaceLightPos0.xyz;
					  lightDir_5 = tmpvar_6;
					  tmpvar_4 = xlv_TEXCOORD1;
					  lowp vec4 tmpvar_7;
					  highp vec2 P_8;
					  P_8 = (xlv_TEXCOORD0 + (_V_CW_MainTex_Scroll * _Time.x));
					  tmpvar_7 = texture2D (_MainTex, P_8);
					  tmpvar_1 = _LightColor0.xyz;
					  tmpvar_2 = lightDir_5;
					  lowp vec4 c_9;
					  lowp vec4 c_10;
					  lowp float diff_11;
					  mediump float tmpvar_12;
					  tmpvar_12 = max (0.0, dot (tmpvar_4, tmpvar_2));
					  diff_11 = tmpvar_12;
					  c_10.xyz = ((tmpvar_7.xyz * _Color.xyz) * (tmpvar_1 * diff_11));
					  c_10.w = (tmpvar_7.w * _Color.w);
					  c_9.w = c_10.w;
					  c_9.xyz = c_10.xyz;
					  c_3.xyz = c_9.xyz;
					  c_3.w = 1.0;
					  gl_FragData[0] = c_3;
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
					void main ()
					{
					  mediump vec3 tmpvar_1;
					  mediump vec3 tmpvar_2;
					  lowp vec4 c_3;
					  lowp float atten_4;
					  mediump vec4 lightCoord_5;
					  lowp vec3 tmpvar_6;
					  lowp vec3 lightDir_7;
					  highp vec3 tmpvar_8;
					  tmpvar_8 = normalize((_WorldSpaceLightPos0.xyz - xlv_TEXCOORD2));
					  lightDir_7 = tmpvar_8;
					  tmpvar_6 = xlv_TEXCOORD1;
					  lowp vec4 tmpvar_9;
					  highp vec2 P_10;
					  P_10 = (xlv_TEXCOORD0 + (_V_CW_MainTex_Scroll * _Time.x));
					  tmpvar_9 = texture2D (_MainTex, P_10);
					  highp vec4 tmpvar_11;
					  tmpvar_11.w = 1.0;
					  tmpvar_11.xyz = xlv_TEXCOORD2;
					  highp vec4 tmpvar_12;
					  tmpvar_12 = (_LightMatrix0 * tmpvar_11);
					  lightCoord_5 = tmpvar_12;
					  lowp vec4 tmpvar_13;
					  mediump vec2 P_14;
					  P_14 = ((lightCoord_5.xy / lightCoord_5.w) + 0.5);
					  tmpvar_13 = texture2D (_LightTexture0, P_14);
					  highp vec3 LightCoord_15;
					  LightCoord_15 = lightCoord_5.xyz;
					  highp float tmpvar_16;
					  tmpvar_16 = dot (LightCoord_15, LightCoord_15);
					  lowp vec4 tmpvar_17;
					  tmpvar_17 = texture2D (_LightTextureB0, vec2(tmpvar_16));
					  mediump float tmpvar_18;
					  tmpvar_18 = ((float(
					    (lightCoord_5.z > 0.0)
					  ) * tmpvar_13.w) * tmpvar_17.w);
					  atten_4 = tmpvar_18;
					  tmpvar_1 = _LightColor0.xyz;
					  tmpvar_2 = lightDir_7;
					  tmpvar_1 = (tmpvar_1 * atten_4);
					  lowp vec4 c_19;
					  lowp vec4 c_20;
					  lowp float diff_21;
					  mediump float tmpvar_22;
					  tmpvar_22 = max (0.0, dot (tmpvar_6, tmpvar_2));
					  diff_21 = tmpvar_22;
					  c_20.xyz = ((tmpvar_9.xyz * _Color.xyz) * (tmpvar_1 * diff_21));
					  c_20.w = (tmpvar_9.w * _Color.w);
					  c_19.w = c_20.w;
					  c_19.xyz = c_20.xyz;
					  c_3.xyz = c_19.xyz;
					  c_3.w = 1.0;
					  gl_FragData[0] = c_3;
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
					void main ()
					{
					  mediump vec3 tmpvar_1;
					  mediump vec3 tmpvar_2;
					  lowp vec4 c_3;
					  lowp vec3 tmpvar_4;
					  lowp vec3 lightDir_5;
					  highp vec3 tmpvar_6;
					  tmpvar_6 = normalize((_WorldSpaceLightPos0.xyz - xlv_TEXCOORD2));
					  lightDir_5 = tmpvar_6;
					  tmpvar_4 = xlv_TEXCOORD1;
					  lowp vec4 tmpvar_7;
					  highp vec2 P_8;
					  P_8 = (xlv_TEXCOORD0 + (_V_CW_MainTex_Scroll * _Time.x));
					  tmpvar_7 = texture2D (_MainTex, P_8);
					  highp vec4 tmpvar_9;
					  tmpvar_9.w = 1.0;
					  tmpvar_9.xyz = xlv_TEXCOORD2;
					  highp vec3 tmpvar_10;
					  tmpvar_10 = (_LightMatrix0 * tmpvar_9).xyz;
					  highp float tmpvar_11;
					  tmpvar_11 = dot (tmpvar_10, tmpvar_10);
					  lowp float tmpvar_12;
					  tmpvar_12 = (texture2D (_LightTextureB0, vec2(tmpvar_11)).w * textureCube (_LightTexture0, tmpvar_10).w);
					  tmpvar_1 = _LightColor0.xyz;
					  tmpvar_2 = lightDir_5;
					  tmpvar_1 = (tmpvar_1 * tmpvar_12);
					  lowp vec4 c_13;
					  lowp vec4 c_14;
					  lowp float diff_15;
					  mediump float tmpvar_16;
					  tmpvar_16 = max (0.0, dot (tmpvar_4, tmpvar_2));
					  diff_15 = tmpvar_16;
					  c_14.xyz = ((tmpvar_7.xyz * _Color.xyz) * (tmpvar_1 * diff_15));
					  c_14.w = (tmpvar_7.w * _Color.w);
					  c_13.w = c_14.w;
					  c_13.xyz = c_14.xyz;
					  c_3.xyz = c_13.xyz;
					  c_3.w = 1.0;
					  gl_FragData[0] = c_3;
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
					void main ()
					{
					  mediump vec3 tmpvar_1;
					  mediump vec3 tmpvar_2;
					  lowp vec4 c_3;
					  lowp vec3 tmpvar_4;
					  lowp vec3 lightDir_5;
					  mediump vec3 tmpvar_6;
					  tmpvar_6 = _WorldSpaceLightPos0.xyz;
					  lightDir_5 = tmpvar_6;
					  tmpvar_4 = xlv_TEXCOORD1;
					  lowp vec4 tmpvar_7;
					  highp vec2 P_8;
					  P_8 = (xlv_TEXCOORD0 + (_V_CW_MainTex_Scroll * _Time.x));
					  tmpvar_7 = texture2D (_MainTex, P_8);
					  highp vec4 tmpvar_9;
					  tmpvar_9.w = 1.0;
					  tmpvar_9.xyz = xlv_TEXCOORD2;
					  highp vec2 tmpvar_10;
					  tmpvar_10 = (_LightMatrix0 * tmpvar_9).xy;
					  lowp float tmpvar_11;
					  tmpvar_11 = texture2D (_LightTexture0, tmpvar_10).w;
					  tmpvar_1 = _LightColor0.xyz;
					  tmpvar_2 = lightDir_5;
					  tmpvar_1 = (tmpvar_1 * tmpvar_11);
					  lowp vec4 c_12;
					  lowp vec4 c_13;
					  lowp float diff_14;
					  mediump float tmpvar_15;
					  tmpvar_15 = max (0.0, dot (tmpvar_4, tmpvar_2));
					  diff_14 = tmpvar_15;
					  c_13.xyz = ((tmpvar_7.xyz * _Color.xyz) * (tmpvar_1 * diff_14));
					  c_13.w = (tmpvar_7.w * _Color.w);
					  c_12.w = c_13.w;
					  c_12.xyz = c_13.xyz;
					  c_3.xyz = c_12.xyz;
					  c_3.w = 1.0;
					  gl_FragData[0] = c_3;
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
					varying highp float xlv_TEXCOORD3;
					void main ()
					{
					  mediump vec3 tmpvar_1;
					  mediump vec3 tmpvar_2;
					  lowp vec4 c_3;
					  lowp vec3 tmpvar_4;
					  lowp vec3 lightDir_5;
					  highp vec3 tmpvar_6;
					  tmpvar_6 = normalize((_WorldSpaceLightPos0.xyz - xlv_TEXCOORD2));
					  lightDir_5 = tmpvar_6;
					  tmpvar_4 = xlv_TEXCOORD1;
					  lowp vec4 tmpvar_7;
					  highp vec2 P_8;
					  P_8 = (xlv_TEXCOORD0 + (_V_CW_MainTex_Scroll * _Time.x));
					  tmpvar_7 = texture2D (_MainTex, P_8);
					  highp vec4 tmpvar_9;
					  tmpvar_9.w = 1.0;
					  tmpvar_9.xyz = xlv_TEXCOORD2;
					  highp vec3 tmpvar_10;
					  tmpvar_10 = (_LightMatrix0 * tmpvar_9).xyz;
					  highp float tmpvar_11;
					  tmpvar_11 = dot (tmpvar_10, tmpvar_10);
					  lowp float tmpvar_12;
					  tmpvar_12 = texture2D (_LightTexture0, vec2(tmpvar_11)).w;
					  tmpvar_1 = _LightColor0.xyz;
					  tmpvar_2 = lightDir_5;
					  tmpvar_1 = (tmpvar_1 * tmpvar_12);
					  lowp vec4 c_13;
					  lowp float diff_14;
					  mediump float tmpvar_15;
					  tmpvar_15 = max (0.0, dot (tmpvar_4, tmpvar_2));
					  diff_14 = tmpvar_15;
					  c_13.xyz = ((tmpvar_7.xyz * _Color.xyz) * (tmpvar_1 * diff_14));
					  c_13.w = (tmpvar_7.w * _Color.w);
					  highp float tmpvar_16;
					  tmpvar_16 = clamp (xlv_TEXCOORD3, 0.0, 1.0);
					  c_3.xyz = (c_13.xyz * vec3(tmpvar_16));
					  c_3.w = 1.0;
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
					varying highp float xlv_TEXCOORD3;
					void main ()
					{
					  mediump vec3 tmpvar_1;
					  mediump vec3 tmpvar_2;
					  lowp vec4 c_3;
					  lowp vec3 tmpvar_4;
					  lowp vec3 lightDir_5;
					  mediump vec3 tmpvar_6;
					  tmpvar_6 = _WorldSpaceLightPos0.xyz;
					  lightDir_5 = tmpvar_6;
					  tmpvar_4 = xlv_TEXCOORD1;
					  lowp vec4 tmpvar_7;
					  highp vec2 P_8;
					  P_8 = (xlv_TEXCOORD0 + (_V_CW_MainTex_Scroll * _Time.x));
					  tmpvar_7 = texture2D (_MainTex, P_8);
					  tmpvar_1 = _LightColor0.xyz;
					  tmpvar_2 = lightDir_5;
					  lowp vec4 c_9;
					  lowp float diff_10;
					  mediump float tmpvar_11;
					  tmpvar_11 = max (0.0, dot (tmpvar_4, tmpvar_2));
					  diff_10 = tmpvar_11;
					  c_9.xyz = ((tmpvar_7.xyz * _Color.xyz) * (tmpvar_1 * diff_10));
					  c_9.w = (tmpvar_7.w * _Color.w);
					  highp float tmpvar_12;
					  tmpvar_12 = clamp (xlv_TEXCOORD3, 0.0, 1.0);
					  c_3.xyz = (c_9.xyz * vec3(tmpvar_12));
					  c_3.w = 1.0;
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
					  highp vec3 tmpvar_8;
					  tmpvar_8 = normalize((_WorldSpaceLightPos0.xyz - xlv_TEXCOORD2));
					  lightDir_7 = tmpvar_8;
					  tmpvar_6 = xlv_TEXCOORD1;
					  lowp vec4 tmpvar_9;
					  highp vec2 P_10;
					  P_10 = (xlv_TEXCOORD0 + (_V_CW_MainTex_Scroll * _Time.x));
					  tmpvar_9 = texture2D (_MainTex, P_10);
					  highp vec4 tmpvar_11;
					  tmpvar_11.w = 1.0;
					  tmpvar_11.xyz = xlv_TEXCOORD2;
					  highp vec4 tmpvar_12;
					  tmpvar_12 = (_LightMatrix0 * tmpvar_11);
					  lightCoord_5 = tmpvar_12;
					  lowp vec4 tmpvar_13;
					  mediump vec2 P_14;
					  P_14 = ((lightCoord_5.xy / lightCoord_5.w) + 0.5);
					  tmpvar_13 = texture2D (_LightTexture0, P_14);
					  highp vec3 LightCoord_15;
					  LightCoord_15 = lightCoord_5.xyz;
					  highp float tmpvar_16;
					  tmpvar_16 = dot (LightCoord_15, LightCoord_15);
					  lowp vec4 tmpvar_17;
					  tmpvar_17 = texture2D (_LightTextureB0, vec2(tmpvar_16));
					  mediump float tmpvar_18;
					  tmpvar_18 = ((float(
					    (lightCoord_5.z > 0.0)
					  ) * tmpvar_13.w) * tmpvar_17.w);
					  atten_4 = tmpvar_18;
					  tmpvar_1 = _LightColor0.xyz;
					  tmpvar_2 = lightDir_7;
					  tmpvar_1 = (tmpvar_1 * atten_4);
					  lowp vec4 c_19;
					  lowp float diff_20;
					  mediump float tmpvar_21;
					  tmpvar_21 = max (0.0, dot (tmpvar_6, tmpvar_2));
					  diff_20 = tmpvar_21;
					  c_19.xyz = ((tmpvar_9.xyz * _Color.xyz) * (tmpvar_1 * diff_20));
					  c_19.w = (tmpvar_9.w * _Color.w);
					  highp float tmpvar_22;
					  tmpvar_22 = clamp (xlv_TEXCOORD3, 0.0, 1.0);
					  c_3.xyz = (c_19.xyz * vec3(tmpvar_22));
					  c_3.w = 1.0;
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
					varying highp float xlv_TEXCOORD3;
					void main ()
					{
					  mediump vec3 tmpvar_1;
					  mediump vec3 tmpvar_2;
					  lowp vec4 c_3;
					  lowp vec3 tmpvar_4;
					  lowp vec3 lightDir_5;
					  highp vec3 tmpvar_6;
					  tmpvar_6 = normalize((_WorldSpaceLightPos0.xyz - xlv_TEXCOORD2));
					  lightDir_5 = tmpvar_6;
					  tmpvar_4 = xlv_TEXCOORD1;
					  lowp vec4 tmpvar_7;
					  highp vec2 P_8;
					  P_8 = (xlv_TEXCOORD0 + (_V_CW_MainTex_Scroll * _Time.x));
					  tmpvar_7 = texture2D (_MainTex, P_8);
					  highp vec4 tmpvar_9;
					  tmpvar_9.w = 1.0;
					  tmpvar_9.xyz = xlv_TEXCOORD2;
					  highp vec3 tmpvar_10;
					  tmpvar_10 = (_LightMatrix0 * tmpvar_9).xyz;
					  highp float tmpvar_11;
					  tmpvar_11 = dot (tmpvar_10, tmpvar_10);
					  lowp float tmpvar_12;
					  tmpvar_12 = (texture2D (_LightTextureB0, vec2(tmpvar_11)).w * textureCube (_LightTexture0, tmpvar_10).w);
					  tmpvar_1 = _LightColor0.xyz;
					  tmpvar_2 = lightDir_5;
					  tmpvar_1 = (tmpvar_1 * tmpvar_12);
					  lowp vec4 c_13;
					  lowp float diff_14;
					  mediump float tmpvar_15;
					  tmpvar_15 = max (0.0, dot (tmpvar_4, tmpvar_2));
					  diff_14 = tmpvar_15;
					  c_13.xyz = ((tmpvar_7.xyz * _Color.xyz) * (tmpvar_1 * diff_14));
					  c_13.w = (tmpvar_7.w * _Color.w);
					  highp float tmpvar_16;
					  tmpvar_16 = clamp (xlv_TEXCOORD3, 0.0, 1.0);
					  c_3.xyz = (c_13.xyz * vec3(tmpvar_16));
					  c_3.w = 1.0;
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
					varying highp float xlv_TEXCOORD3;
					void main ()
					{
					  mediump vec3 tmpvar_1;
					  mediump vec3 tmpvar_2;
					  lowp vec4 c_3;
					  lowp vec3 tmpvar_4;
					  lowp vec3 lightDir_5;
					  mediump vec3 tmpvar_6;
					  tmpvar_6 = _WorldSpaceLightPos0.xyz;
					  lightDir_5 = tmpvar_6;
					  tmpvar_4 = xlv_TEXCOORD1;
					  lowp vec4 tmpvar_7;
					  highp vec2 P_8;
					  P_8 = (xlv_TEXCOORD0 + (_V_CW_MainTex_Scroll * _Time.x));
					  tmpvar_7 = texture2D (_MainTex, P_8);
					  highp vec4 tmpvar_9;
					  tmpvar_9.w = 1.0;
					  tmpvar_9.xyz = xlv_TEXCOORD2;
					  highp vec2 tmpvar_10;
					  tmpvar_10 = (_LightMatrix0 * tmpvar_9).xy;
					  lowp float tmpvar_11;
					  tmpvar_11 = texture2D (_LightTexture0, tmpvar_10).w;
					  tmpvar_1 = _LightColor0.xyz;
					  tmpvar_2 = lightDir_5;
					  tmpvar_1 = (tmpvar_1 * tmpvar_11);
					  lowp vec4 c_12;
					  lowp float diff_13;
					  mediump float tmpvar_14;
					  tmpvar_14 = max (0.0, dot (tmpvar_4, tmpvar_2));
					  diff_13 = tmpvar_14;
					  c_12.xyz = ((tmpvar_7.xyz * _Color.xyz) * (tmpvar_1 * diff_13));
					  c_12.w = (tmpvar_7.w * _Color.w);
					  highp float tmpvar_15;
					  tmpvar_15 = clamp (xlv_TEXCOORD3, 0.0, 1.0);
					  c_3.xyz = (c_12.xyz * vec3(tmpvar_15));
					  c_3.w = 1.0;
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
}
 }
 Pass {
  Name "PREPASS"
  Tags { "LIGHTMODE"="PrePassBase" "QUEUE"="Transparent" "IGNOREPROJECTOR"="true" "RenderType"="Transparent" "CurvedWorldTag"="Legacy Shader/Additive/Diffuse" "CurvedWorldNoneRemoveableKeywords"="" "CurvedWorldAvailableOptions"="V_CW_REFLECTIVE;V_CW_VERTEX_COLOR;_EMISSION;V_CW_RIM;_NORMALMAP;V_CW_SPECULAR_HD;" }
  ZWrite Off
  Cull Off
  Blend One OneMinusSrcColor
  GpuProgramID 141213
Program "vp" {
SubProgram "gles " {
Keywords { "V_CW_REFLECTIVE_OFF" "V_CW_VERTEX_COLOR_OFF" "_EMISSION_OFF" "V_CW_RIM_OFF" "_NORMALMAP_OFF" "V_CW_SPECULAR_OFF" }
					"!!GLES
					#version 100
					
					#ifdef VERTEX
					attribute vec4 _glesTANGENT;
					attribute vec4 _glesVertex;
					attribute vec3 _glesNormal;
					uniform highp mat4 glstate_matrix_mvp;
					uniform highp mat4 _Object2World;
					uniform highp mat4 _World2Object;
					uniform highp vec3 _V_CW_Bend;
					uniform highp vec3 _V_CW_Bias;
					uniform highp vec4 _V_CW_PivotPoint_Position;
					varying mediump vec3 xlv_TEXCOORD0;
					varying highp vec3 xlv_TEXCOORD1;
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
					  xlv_TEXCOORD0 = tmpvar_2;
					  xlv_TEXCOORD1 = (_Object2World * vertex_21).xyz;
					}
					
					
					#endif
					#ifdef FRAGMENT
					varying mediump vec3 xlv_TEXCOORD0;
					void main ()
					{
					  lowp vec4 res_1;
					  lowp vec3 tmpvar_2;
					  tmpvar_2 = xlv_TEXCOORD0;
					  res_1.xyz = ((tmpvar_2 * 0.5) + 0.5);
					  res_1.w = 0.0;
					  gl_FragData[0] = res_1;
					}
					
					
					#endif"
}
}
Program "fp" {
SubProgram "gles " {
Keywords { "V_CW_REFLECTIVE_OFF" "V_CW_VERTEX_COLOR_OFF" "_EMISSION_OFF" "V_CW_RIM_OFF" "_NORMALMAP_OFF" "V_CW_SPECULAR_OFF" }
					"!!GLES"
}
}
 }
 Pass {
  Name "PREPASS"
  Tags { "LIGHTMODE"="PrePassFinal" "QUEUE"="Transparent" "IGNOREPROJECTOR"="true" "RenderType"="Transparent" "CurvedWorldTag"="Legacy Shader/Additive/Diffuse" "CurvedWorldNoneRemoveableKeywords"="" "CurvedWorldAvailableOptions"="V_CW_REFLECTIVE;V_CW_VERTEX_COLOR;_EMISSION;V_CW_RIM;_NORMALMAP;V_CW_SPECULAR_HD;" }
  ZWrite Off
  Cull Off
  Blend One OneMinusSrcColor
  GpuProgramID 249252
Program "vp" {
SubProgram "gles " {
Keywords { "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "HDR_LIGHT_PREPASS_OFF" "V_CW_REFLECTIVE_OFF" "V_CW_VERTEX_COLOR_OFF" "_EMISSION_OFF" "V_CW_RIM_OFF" "_NORMALMAP_OFF" "V_CW_SPECULAR_OFF" }
					"!!GLES
					#version 100
					
					#ifdef VERTEX
					attribute vec4 _glesTANGENT;
					attribute vec4 _glesVertex;
					attribute vec3 _glesNormal;
					attribute vec4 _glesMultiTexCoord0;
					uniform highp vec4 _ProjectionParams;
					uniform mediump vec4 unity_SHAr;
					uniform mediump vec4 unity_SHAg;
					uniform mediump vec4 unity_SHAb;
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
					varying highp vec3 xlv_TEXCOORD1;
					varying highp vec4 xlv_TEXCOORD2;
					varying highp vec4 xlv_TEXCOORD3;
					varying highp vec3 xlv_TEXCOORD4;
					void main ()
					{
					  highp vec4 tmpvar_1;
					  highp vec4 tmpvar_2;
					  highp vec3 tmpvar_3;
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
					  tmpvar_1 = (glstate_matrix_mvp * vertex_22);
					  highp vec4 o_39;
					  highp vec4 tmpvar_40;
					  tmpvar_40 = (tmpvar_1 * 0.5);
					  highp vec2 tmpvar_41;
					  tmpvar_41.x = tmpvar_40.x;
					  tmpvar_41.y = (tmpvar_40.y * _ProjectionParams.x);
					  o_39.xy = (tmpvar_41 + tmpvar_40.w);
					  o_39.zw = tmpvar_1.zw;
					  tmpvar_2.zw = vec2(0.0, 0.0);
					  tmpvar_2.xy = vec2(0.0, 0.0);
					  highp vec4 v_42;
					  v_42.x = tmpvar_5;
					  v_42.y = tmpvar_6;
					  v_42.z = tmpvar_7;
					  v_42.w = tmpvar_8;
					  highp vec4 v_43;
					  v_43.x = tmpvar_10;
					  v_43.y = tmpvar_11;
					  v_43.z = tmpvar_12;
					  v_43.w = tmpvar_13;
					  highp vec4 v_44;
					  v_44.x = tmpvar_15;
					  v_44.y = tmpvar_16;
					  v_44.z = tmpvar_17;
					  v_44.w = tmpvar_18;
					  highp vec4 tmpvar_45;
					  tmpvar_45.w = 1.0;
					  tmpvar_45.xyz = normalize(((
					    (v_42.xyz * tmpvar_38.x)
					   + 
					    (v_43.xyz * tmpvar_38.y)
					  ) + (v_44.xyz * tmpvar_38.z)));
					  mediump vec4 normal_46;
					  normal_46 = tmpvar_45;
					  mediump vec3 res_47;
					  mediump vec3 x_48;
					  x_48.x = dot (unity_SHAr, normal_46);
					  x_48.y = dot (unity_SHAg, normal_46);
					  x_48.z = dot (unity_SHAb, normal_46);
					  mediump vec3 x1_49;
					  mediump vec4 tmpvar_50;
					  tmpvar_50 = (normal_46.xyzz * normal_46.yzzx);
					  x1_49.x = dot (unity_SHBr, tmpvar_50);
					  x1_49.y = dot (unity_SHBg, tmpvar_50);
					  x1_49.z = dot (unity_SHBb, tmpvar_50);
					  res_47 = (x_48 + (x1_49 + (unity_SHC.xyz * 
					    ((normal_46.x * normal_46.x) - (normal_46.y * normal_46.y))
					  )));
					  res_47 = max (((1.055 * 
					    pow (max (res_47, vec3(0.0, 0.0, 0.0)), vec3(0.4166667, 0.4166667, 0.4166667))
					  ) - 0.055), vec3(0.0, 0.0, 0.0));
					  tmpvar_3 = res_47;
					  gl_Position = tmpvar_1;
					  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
					  xlv_TEXCOORD1 = (_Object2World * vertex_22).xyz;
					  xlv_TEXCOORD2 = o_39;
					  xlv_TEXCOORD3 = tmpvar_2;
					  xlv_TEXCOORD4 = tmpvar_3;
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform highp vec4 _Time;
					uniform sampler2D _MainTex;
					uniform lowp vec4 _Color;
					uniform lowp vec2 _V_CW_MainTex_Scroll;
					uniform sampler2D _LightBuffer;
					varying highp vec2 xlv_TEXCOORD0;
					varying highp vec4 xlv_TEXCOORD2;
					varying highp vec3 xlv_TEXCOORD4;
					void main ()
					{
					  lowp vec4 tmpvar_1;
					  mediump vec4 c_2;
					  mediump vec4 light_3;
					  lowp vec4 tmpvar_4;
					  highp vec2 P_5;
					  P_5 = (xlv_TEXCOORD0 + (_V_CW_MainTex_Scroll * _Time.x));
					  tmpvar_4 = texture2D (_MainTex, P_5);
					  lowp vec4 tmpvar_6;
					  tmpvar_6 = texture2DProj (_LightBuffer, xlv_TEXCOORD2);
					  light_3 = tmpvar_6;
					  light_3 = -(log2(max (light_3, vec4(0.001, 0.001, 0.001, 0.001))));
					  light_3.xyz = (light_3.xyz + xlv_TEXCOORD4);
					  lowp vec4 c_7;
					  c_7.xyz = ((tmpvar_4.xyz * _Color.xyz) * light_3.xyz);
					  c_7.w = (tmpvar_4.w * _Color.w);
					  c_2.xyz = c_7.xyz;
					  c_2.w = 1.0;
					  tmpvar_1 = c_2;
					  gl_FragData[0] = tmpvar_1;
					}
					
					
					#endif"
}
SubProgram "gles " {
Keywords { "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "UNITY_HDR_ON" "V_CW_REFLECTIVE_OFF" "V_CW_VERTEX_COLOR_OFF" "_EMISSION_OFF" "V_CW_RIM_OFF" "_NORMALMAP_OFF" "V_CW_SPECULAR_OFF" }
					"!!GLES
					#version 100
					
					#ifdef VERTEX
					attribute vec4 _glesTANGENT;
					attribute vec4 _glesVertex;
					attribute vec3 _glesNormal;
					attribute vec4 _glesMultiTexCoord0;
					uniform highp vec4 _ProjectionParams;
					uniform mediump vec4 unity_SHAr;
					uniform mediump vec4 unity_SHAg;
					uniform mediump vec4 unity_SHAb;
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
					varying highp vec3 xlv_TEXCOORD1;
					varying highp vec4 xlv_TEXCOORD2;
					varying highp vec4 xlv_TEXCOORD3;
					varying highp vec3 xlv_TEXCOORD4;
					void main ()
					{
					  highp vec4 tmpvar_1;
					  highp vec4 tmpvar_2;
					  highp vec3 tmpvar_3;
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
					  tmpvar_1 = (glstate_matrix_mvp * vertex_22);
					  highp vec4 o_39;
					  highp vec4 tmpvar_40;
					  tmpvar_40 = (tmpvar_1 * 0.5);
					  highp vec2 tmpvar_41;
					  tmpvar_41.x = tmpvar_40.x;
					  tmpvar_41.y = (tmpvar_40.y * _ProjectionParams.x);
					  o_39.xy = (tmpvar_41 + tmpvar_40.w);
					  o_39.zw = tmpvar_1.zw;
					  tmpvar_2.zw = vec2(0.0, 0.0);
					  tmpvar_2.xy = vec2(0.0, 0.0);
					  highp vec4 v_42;
					  v_42.x = tmpvar_5;
					  v_42.y = tmpvar_6;
					  v_42.z = tmpvar_7;
					  v_42.w = tmpvar_8;
					  highp vec4 v_43;
					  v_43.x = tmpvar_10;
					  v_43.y = tmpvar_11;
					  v_43.z = tmpvar_12;
					  v_43.w = tmpvar_13;
					  highp vec4 v_44;
					  v_44.x = tmpvar_15;
					  v_44.y = tmpvar_16;
					  v_44.z = tmpvar_17;
					  v_44.w = tmpvar_18;
					  highp vec4 tmpvar_45;
					  tmpvar_45.w = 1.0;
					  tmpvar_45.xyz = normalize(((
					    (v_42.xyz * tmpvar_38.x)
					   + 
					    (v_43.xyz * tmpvar_38.y)
					  ) + (v_44.xyz * tmpvar_38.z)));
					  mediump vec4 normal_46;
					  normal_46 = tmpvar_45;
					  mediump vec3 res_47;
					  mediump vec3 x_48;
					  x_48.x = dot (unity_SHAr, normal_46);
					  x_48.y = dot (unity_SHAg, normal_46);
					  x_48.z = dot (unity_SHAb, normal_46);
					  mediump vec3 x1_49;
					  mediump vec4 tmpvar_50;
					  tmpvar_50 = (normal_46.xyzz * normal_46.yzzx);
					  x1_49.x = dot (unity_SHBr, tmpvar_50);
					  x1_49.y = dot (unity_SHBg, tmpvar_50);
					  x1_49.z = dot (unity_SHBb, tmpvar_50);
					  res_47 = (x_48 + (x1_49 + (unity_SHC.xyz * 
					    ((normal_46.x * normal_46.x) - (normal_46.y * normal_46.y))
					  )));
					  res_47 = max (((1.055 * 
					    pow (max (res_47, vec3(0.0, 0.0, 0.0)), vec3(0.4166667, 0.4166667, 0.4166667))
					  ) - 0.055), vec3(0.0, 0.0, 0.0));
					  tmpvar_3 = res_47;
					  gl_Position = tmpvar_1;
					  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
					  xlv_TEXCOORD1 = (_Object2World * vertex_22).xyz;
					  xlv_TEXCOORD2 = o_39;
					  xlv_TEXCOORD3 = tmpvar_2;
					  xlv_TEXCOORD4 = tmpvar_3;
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform highp vec4 _Time;
					uniform sampler2D _MainTex;
					uniform lowp vec4 _Color;
					uniform lowp vec2 _V_CW_MainTex_Scroll;
					uniform sampler2D _LightBuffer;
					varying highp vec2 xlv_TEXCOORD0;
					varying highp vec4 xlv_TEXCOORD2;
					varying highp vec3 xlv_TEXCOORD4;
					void main ()
					{
					  lowp vec4 tmpvar_1;
					  mediump vec4 c_2;
					  mediump vec4 light_3;
					  lowp vec4 tmpvar_4;
					  highp vec2 P_5;
					  P_5 = (xlv_TEXCOORD0 + (_V_CW_MainTex_Scroll * _Time.x));
					  tmpvar_4 = texture2D (_MainTex, P_5);
					  lowp vec4 tmpvar_6;
					  tmpvar_6 = texture2DProj (_LightBuffer, xlv_TEXCOORD2);
					  light_3 = tmpvar_6;
					  mediump vec4 tmpvar_7;
					  tmpvar_7 = max (light_3, vec4(0.001, 0.001, 0.001, 0.001));
					  light_3.w = tmpvar_7.w;
					  light_3.xyz = (tmpvar_7.xyz + xlv_TEXCOORD4);
					  lowp vec4 c_8;
					  c_8.xyz = ((tmpvar_4.xyz * _Color.xyz) * light_3.xyz);
					  c_8.w = (tmpvar_4.w * _Color.w);
					  c_2.xyz = c_8.xyz;
					  c_2.w = 1.0;
					  tmpvar_1 = c_2;
					  gl_FragData[0] = tmpvar_1;
					}
					
					
					#endif"
}
SubProgram "gles " {
Keywords { "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "FOG_LINEAR" "HDR_LIGHT_PREPASS_OFF" "V_CW_REFLECTIVE_OFF" "V_CW_VERTEX_COLOR_OFF" "_EMISSION_OFF" "V_CW_RIM_OFF" "_NORMALMAP_OFF" "V_CW_SPECULAR_OFF" }
					"!!GLES
					#version 100
					
					#ifdef VERTEX
					attribute vec4 _glesTANGENT;
					attribute vec4 _glesVertex;
					attribute vec3 _glesNormal;
					attribute vec4 _glesMultiTexCoord0;
					uniform highp vec4 _ProjectionParams;
					uniform mediump vec4 unity_SHAr;
					uniform mediump vec4 unity_SHAg;
					uniform mediump vec4 unity_SHAb;
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
					varying highp vec3 xlv_TEXCOORD1;
					varying highp vec4 xlv_TEXCOORD2;
					varying highp vec4 xlv_TEXCOORD3;
					varying highp vec3 xlv_TEXCOORD4;
					varying highp float xlv_TEXCOORD5;
					void main ()
					{
					  highp vec4 tmpvar_1;
					  highp vec4 tmpvar_2;
					  highp vec3 tmpvar_3;
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
					  tmpvar_1 = (glstate_matrix_mvp * vertex_22);
					  highp vec4 o_39;
					  highp vec4 tmpvar_40;
					  tmpvar_40 = (tmpvar_1 * 0.5);
					  highp vec2 tmpvar_41;
					  tmpvar_41.x = tmpvar_40.x;
					  tmpvar_41.y = (tmpvar_40.y * _ProjectionParams.x);
					  o_39.xy = (tmpvar_41 + tmpvar_40.w);
					  o_39.zw = tmpvar_1.zw;
					  tmpvar_2.zw = vec2(0.0, 0.0);
					  tmpvar_2.xy = vec2(0.0, 0.0);
					  highp vec4 v_42;
					  v_42.x = tmpvar_5;
					  v_42.y = tmpvar_6;
					  v_42.z = tmpvar_7;
					  v_42.w = tmpvar_8;
					  highp vec4 v_43;
					  v_43.x = tmpvar_10;
					  v_43.y = tmpvar_11;
					  v_43.z = tmpvar_12;
					  v_43.w = tmpvar_13;
					  highp vec4 v_44;
					  v_44.x = tmpvar_15;
					  v_44.y = tmpvar_16;
					  v_44.z = tmpvar_17;
					  v_44.w = tmpvar_18;
					  highp vec4 tmpvar_45;
					  tmpvar_45.w = 1.0;
					  tmpvar_45.xyz = normalize(((
					    (v_42.xyz * tmpvar_38.x)
					   + 
					    (v_43.xyz * tmpvar_38.y)
					  ) + (v_44.xyz * tmpvar_38.z)));
					  mediump vec4 normal_46;
					  normal_46 = tmpvar_45;
					  mediump vec3 res_47;
					  mediump vec3 x_48;
					  x_48.x = dot (unity_SHAr, normal_46);
					  x_48.y = dot (unity_SHAg, normal_46);
					  x_48.z = dot (unity_SHAb, normal_46);
					  mediump vec3 x1_49;
					  mediump vec4 tmpvar_50;
					  tmpvar_50 = (normal_46.xyzz * normal_46.yzzx);
					  x1_49.x = dot (unity_SHBr, tmpvar_50);
					  x1_49.y = dot (unity_SHBg, tmpvar_50);
					  x1_49.z = dot (unity_SHBb, tmpvar_50);
					  res_47 = (x_48 + (x1_49 + (unity_SHC.xyz * 
					    ((normal_46.x * normal_46.x) - (normal_46.y * normal_46.y))
					  )));
					  res_47 = max (((1.055 * 
					    pow (max (res_47, vec3(0.0, 0.0, 0.0)), vec3(0.4166667, 0.4166667, 0.4166667))
					  ) - 0.055), vec3(0.0, 0.0, 0.0));
					  tmpvar_3 = res_47;
					  gl_Position = tmpvar_1;
					  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
					  xlv_TEXCOORD1 = (_Object2World * vertex_22).xyz;
					  xlv_TEXCOORD2 = o_39;
					  xlv_TEXCOORD3 = tmpvar_2;
					  xlv_TEXCOORD4 = tmpvar_3;
					  xlv_TEXCOORD5 = ((tmpvar_1.z * unity_FogParams.z) + unity_FogParams.w);
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform highp vec4 _Time;
					uniform lowp vec4 unity_FogColor;
					uniform sampler2D _MainTex;
					uniform lowp vec4 _Color;
					uniform lowp vec2 _V_CW_MainTex_Scroll;
					uniform sampler2D _LightBuffer;
					varying highp vec2 xlv_TEXCOORD0;
					varying highp vec4 xlv_TEXCOORD2;
					varying highp vec3 xlv_TEXCOORD4;
					varying highp float xlv_TEXCOORD5;
					void main ()
					{
					  lowp vec4 tmpvar_1;
					  mediump vec4 c_2;
					  mediump vec4 light_3;
					  lowp vec4 tmpvar_4;
					  highp vec2 P_5;
					  P_5 = (xlv_TEXCOORD0 + (_V_CW_MainTex_Scroll * _Time.x));
					  tmpvar_4 = texture2D (_MainTex, P_5);
					  lowp vec4 tmpvar_6;
					  tmpvar_6 = texture2DProj (_LightBuffer, xlv_TEXCOORD2);
					  light_3 = tmpvar_6;
					  light_3 = -(log2(max (light_3, vec4(0.001, 0.001, 0.001, 0.001))));
					  light_3.xyz = (light_3.xyz + xlv_TEXCOORD4);
					  lowp vec4 c_7;
					  c_7.xyz = ((tmpvar_4.xyz * _Color.xyz) * light_3.xyz);
					  c_7.w = (tmpvar_4.w * _Color.w);
					  c_2 = c_7;
					  highp float tmpvar_8;
					  tmpvar_8 = clamp (xlv_TEXCOORD5, 0.0, 1.0);
					  c_2.xyz = mix (unity_FogColor.xyz, c_2.xyz, vec3(tmpvar_8));
					  c_2.w = 1.0;
					  tmpvar_1 = c_2;
					  gl_FragData[0] = tmpvar_1;
					}
					
					
					#endif"
}
SubProgram "gles " {
Keywords { "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "FOG_LINEAR" "UNITY_HDR_ON" "V_CW_REFLECTIVE_OFF" "V_CW_VERTEX_COLOR_OFF" "_EMISSION_OFF" "V_CW_RIM_OFF" "_NORMALMAP_OFF" "V_CW_SPECULAR_OFF" }
					"!!GLES
					#version 100
					
					#ifdef VERTEX
					attribute vec4 _glesTANGENT;
					attribute vec4 _glesVertex;
					attribute vec3 _glesNormal;
					attribute vec4 _glesMultiTexCoord0;
					uniform highp vec4 _ProjectionParams;
					uniform mediump vec4 unity_SHAr;
					uniform mediump vec4 unity_SHAg;
					uniform mediump vec4 unity_SHAb;
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
					varying highp vec3 xlv_TEXCOORD1;
					varying highp vec4 xlv_TEXCOORD2;
					varying highp vec4 xlv_TEXCOORD3;
					varying highp vec3 xlv_TEXCOORD4;
					varying highp float xlv_TEXCOORD5;
					void main ()
					{
					  highp vec4 tmpvar_1;
					  highp vec4 tmpvar_2;
					  highp vec3 tmpvar_3;
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
					  tmpvar_1 = (glstate_matrix_mvp * vertex_22);
					  highp vec4 o_39;
					  highp vec4 tmpvar_40;
					  tmpvar_40 = (tmpvar_1 * 0.5);
					  highp vec2 tmpvar_41;
					  tmpvar_41.x = tmpvar_40.x;
					  tmpvar_41.y = (tmpvar_40.y * _ProjectionParams.x);
					  o_39.xy = (tmpvar_41 + tmpvar_40.w);
					  o_39.zw = tmpvar_1.zw;
					  tmpvar_2.zw = vec2(0.0, 0.0);
					  tmpvar_2.xy = vec2(0.0, 0.0);
					  highp vec4 v_42;
					  v_42.x = tmpvar_5;
					  v_42.y = tmpvar_6;
					  v_42.z = tmpvar_7;
					  v_42.w = tmpvar_8;
					  highp vec4 v_43;
					  v_43.x = tmpvar_10;
					  v_43.y = tmpvar_11;
					  v_43.z = tmpvar_12;
					  v_43.w = tmpvar_13;
					  highp vec4 v_44;
					  v_44.x = tmpvar_15;
					  v_44.y = tmpvar_16;
					  v_44.z = tmpvar_17;
					  v_44.w = tmpvar_18;
					  highp vec4 tmpvar_45;
					  tmpvar_45.w = 1.0;
					  tmpvar_45.xyz = normalize(((
					    (v_42.xyz * tmpvar_38.x)
					   + 
					    (v_43.xyz * tmpvar_38.y)
					  ) + (v_44.xyz * tmpvar_38.z)));
					  mediump vec4 normal_46;
					  normal_46 = tmpvar_45;
					  mediump vec3 res_47;
					  mediump vec3 x_48;
					  x_48.x = dot (unity_SHAr, normal_46);
					  x_48.y = dot (unity_SHAg, normal_46);
					  x_48.z = dot (unity_SHAb, normal_46);
					  mediump vec3 x1_49;
					  mediump vec4 tmpvar_50;
					  tmpvar_50 = (normal_46.xyzz * normal_46.yzzx);
					  x1_49.x = dot (unity_SHBr, tmpvar_50);
					  x1_49.y = dot (unity_SHBg, tmpvar_50);
					  x1_49.z = dot (unity_SHBb, tmpvar_50);
					  res_47 = (x_48 + (x1_49 + (unity_SHC.xyz * 
					    ((normal_46.x * normal_46.x) - (normal_46.y * normal_46.y))
					  )));
					  res_47 = max (((1.055 * 
					    pow (max (res_47, vec3(0.0, 0.0, 0.0)), vec3(0.4166667, 0.4166667, 0.4166667))
					  ) - 0.055), vec3(0.0, 0.0, 0.0));
					  tmpvar_3 = res_47;
					  gl_Position = tmpvar_1;
					  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
					  xlv_TEXCOORD1 = (_Object2World * vertex_22).xyz;
					  xlv_TEXCOORD2 = o_39;
					  xlv_TEXCOORD3 = tmpvar_2;
					  xlv_TEXCOORD4 = tmpvar_3;
					  xlv_TEXCOORD5 = ((tmpvar_1.z * unity_FogParams.z) + unity_FogParams.w);
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform highp vec4 _Time;
					uniform lowp vec4 unity_FogColor;
					uniform sampler2D _MainTex;
					uniform lowp vec4 _Color;
					uniform lowp vec2 _V_CW_MainTex_Scroll;
					uniform sampler2D _LightBuffer;
					varying highp vec2 xlv_TEXCOORD0;
					varying highp vec4 xlv_TEXCOORD2;
					varying highp vec3 xlv_TEXCOORD4;
					varying highp float xlv_TEXCOORD5;
					void main ()
					{
					  lowp vec4 tmpvar_1;
					  mediump vec4 c_2;
					  mediump vec4 light_3;
					  lowp vec4 tmpvar_4;
					  highp vec2 P_5;
					  P_5 = (xlv_TEXCOORD0 + (_V_CW_MainTex_Scroll * _Time.x));
					  tmpvar_4 = texture2D (_MainTex, P_5);
					  lowp vec4 tmpvar_6;
					  tmpvar_6 = texture2DProj (_LightBuffer, xlv_TEXCOORD2);
					  light_3 = tmpvar_6;
					  mediump vec4 tmpvar_7;
					  tmpvar_7 = max (light_3, vec4(0.001, 0.001, 0.001, 0.001));
					  light_3.w = tmpvar_7.w;
					  light_3.xyz = (tmpvar_7.xyz + xlv_TEXCOORD4);
					  lowp vec4 c_8;
					  c_8.xyz = ((tmpvar_4.xyz * _Color.xyz) * light_3.xyz);
					  c_8.w = (tmpvar_4.w * _Color.w);
					  c_2 = c_8;
					  highp float tmpvar_9;
					  tmpvar_9 = clamp (xlv_TEXCOORD5, 0.0, 1.0);
					  c_2.xyz = mix (unity_FogColor.xyz, c_2.xyz, vec3(tmpvar_9));
					  c_2.w = 1.0;
					  tmpvar_1 = c_2;
					  gl_FragData[0] = tmpvar_1;
					}
					
					
					#endif"
}
}
Program "fp" {
SubProgram "gles " {
Keywords { "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "HDR_LIGHT_PREPASS_OFF" "V_CW_REFLECTIVE_OFF" "V_CW_VERTEX_COLOR_OFF" "_EMISSION_OFF" "V_CW_RIM_OFF" "_NORMALMAP_OFF" "V_CW_SPECULAR_OFF" }
					"!!GLES"
}
SubProgram "gles " {
Keywords { "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "UNITY_HDR_ON" "V_CW_REFLECTIVE_OFF" "V_CW_VERTEX_COLOR_OFF" "_EMISSION_OFF" "V_CW_RIM_OFF" "_NORMALMAP_OFF" "V_CW_SPECULAR_OFF" }
					"!!GLES"
}
SubProgram "gles " {
Keywords { "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "FOG_LINEAR" "HDR_LIGHT_PREPASS_OFF" "V_CW_REFLECTIVE_OFF" "V_CW_VERTEX_COLOR_OFF" "_EMISSION_OFF" "V_CW_RIM_OFF" "_NORMALMAP_OFF" "V_CW_SPECULAR_OFF" }
					"!!GLES"
}
SubProgram "gles " {
Keywords { "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "FOG_LINEAR" "UNITY_HDR_ON" "V_CW_REFLECTIVE_OFF" "V_CW_VERTEX_COLOR_OFF" "_EMISSION_OFF" "V_CW_RIM_OFF" "_NORMALMAP_OFF" "V_CW_SPECULAR_OFF" }
					"!!GLES"
}
}
 }
 Pass {
  Name "DEFERRED"
  Tags { "LIGHTMODE"="Deferred" "QUEUE"="Transparent" "IGNOREPROJECTOR"="true" "RenderType"="Transparent" "CurvedWorldTag"="Legacy Shader/Additive/Diffuse" "CurvedWorldNoneRemoveableKeywords"="" "CurvedWorldAvailableOptions"="V_CW_REFLECTIVE;V_CW_VERTEX_COLOR;_EMISSION;V_CW_RIM;_NORMALMAP;V_CW_SPECULAR_HD;" }
  ZWrite Off
  Cull Off
  Blend One OneMinusSrcColor
  GpuProgramID 275932
Program "vp" {
SubProgram "gles " {
Keywords { "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "HDR_LIGHT_PREPASS_OFF" "V_CW_REFLECTIVE_OFF" "V_CW_VERTEX_COLOR_OFF" "_EMISSION_OFF" "V_CW_RIM_OFF" "_NORMALMAP_OFF" "V_CW_SPECULAR_OFF" }
					"!!GLES
					#version 100
					
					#ifdef VERTEX
					attribute vec4 _glesTANGENT;
					attribute vec4 _glesVertex;
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
					varying highp vec4 xlv_TEXCOORD3;
					varying mediump vec3 xlv_TEXCOORD4;
					void main ()
					{
					  lowp vec3 worldNormal_1;
					  mediump vec3 tmpvar_2;
					  highp vec4 tmpvar_3;
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
					  worldNormal_1 = tmpvar_42;
					  tmpvar_2 = worldNormal_1;
					  tmpvar_3.zw = vec2(0.0, 0.0);
					  tmpvar_3.xy = vec2(0.0, 0.0);
					  mediump vec3 normal_43;
					  normal_43 = worldNormal_1;
					  mediump vec3 x1_44;
					  mediump vec4 tmpvar_45;
					  tmpvar_45 = (normal_43.xyzz * normal_43.yzzx);
					  x1_44.x = dot (unity_SHBr, tmpvar_45);
					  x1_44.y = dot (unity_SHBg, tmpvar_45);
					  x1_44.z = dot (unity_SHBb, tmpvar_45);
					  gl_Position = (glstate_matrix_mvp * vertex_22);
					  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
					  xlv_TEXCOORD1 = tmpvar_2;
					  xlv_TEXCOORD2 = (_Object2World * vertex_22).xyz;
					  xlv_TEXCOORD3 = tmpvar_3;
					  xlv_TEXCOORD4 = (x1_44 + (unity_SHC.xyz * (
					    (normal_43.x * normal_43.x)
					   - 
					    (normal_43.y * normal_43.y)
					  )));
					}
					
					
					#endif
					#ifdef FRAGMENT
					#extension GL_EXT_draw_buffers : enable
					uniform highp vec4 _Time;
					uniform mediump vec4 unity_SHAr;
					uniform mediump vec4 unity_SHAg;
					uniform mediump vec4 unity_SHAb;
					uniform lowp vec4 _SpecColor;
					uniform sampler2D _MainTex;
					uniform lowp vec4 _Color;
					uniform lowp vec2 _V_CW_MainTex_Scroll;
					varying highp vec2 xlv_TEXCOORD0;
					varying mediump vec3 xlv_TEXCOORD1;
					varying mediump vec3 xlv_TEXCOORD4;
					void main ()
					{
					  mediump vec4 outDiffuse_1;
					  mediump vec4 outEmission_2;
					  lowp vec3 tmpvar_3;
					  tmpvar_3 = xlv_TEXCOORD1;
					  lowp vec3 tmpvar_4;
					  highp vec2 P_5;
					  P_5 = (xlv_TEXCOORD0 + (_V_CW_MainTex_Scroll * _Time.x));
					  tmpvar_4 = (texture2D (_MainTex, P_5).xyz * _Color.xyz);
					  mediump vec3 normalWorld_6;
					  normalWorld_6 = tmpvar_3;
					  mediump vec4 tmpvar_7;
					  tmpvar_7.w = 1.0;
					  tmpvar_7.xyz = normalWorld_6;
					  mediump vec3 x_8;
					  x_8.x = dot (unity_SHAr, tmpvar_7);
					  x_8.y = dot (unity_SHAg, tmpvar_7);
					  x_8.z = dot (unity_SHAb, tmpvar_7);
					  mediump vec4 outDiffuseOcclusion_9;
					  mediump vec4 outNormal_10;
					  mediump vec4 emission_11;
					  lowp vec4 tmpvar_12;
					  tmpvar_12.w = 1.0;
					  tmpvar_12.xyz = tmpvar_4;
					  outDiffuseOcclusion_9 = tmpvar_12;
					  mediump vec4 tmpvar_13;
					  tmpvar_13.xyz = _SpecColor.xyz;
					  tmpvar_13.w = 0.0;
					  lowp vec4 tmpvar_14;
					  tmpvar_14.w = 1.0;
					  tmpvar_14.xyz = ((tmpvar_3 * 0.5) + 0.5);
					  outNormal_10 = tmpvar_14;
					  lowp vec4 tmpvar_15;
					  tmpvar_15.w = 1.0;
					  tmpvar_15.xyz = vec3(0.0, 0.0, 0.0);
					  emission_11 = tmpvar_15;
					  emission_11.xyz = (emission_11.xyz + (tmpvar_4 * max (
					    ((1.055 * pow (max (vec3(0.0, 0.0, 0.0), 
					      (xlv_TEXCOORD4 + x_8)
					    ), vec3(0.4166667, 0.4166667, 0.4166667))) - 0.055)
					  , vec3(0.0, 0.0, 0.0))));
					  outDiffuse_1.xyz = outDiffuseOcclusion_9.xyz;
					  outEmission_2.w = emission_11.w;
					  outEmission_2.xyz = exp2(-(emission_11.xyz));
					  outDiffuse_1.w = 1.0;
					  gl_FragData[0] = outDiffuse_1;
					  gl_FragData[1] = tmpvar_13;
					  gl_FragData[2] = outNormal_10;
					  gl_FragData[3] = outEmission_2;
					}
					
					
					#endif"
}
SubProgram "gles " {
Keywords { "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "UNITY_HDR_ON" "V_CW_REFLECTIVE_OFF" "V_CW_VERTEX_COLOR_OFF" "_EMISSION_OFF" "V_CW_RIM_OFF" "_NORMALMAP_OFF" "V_CW_SPECULAR_OFF" }
					"!!GLES
					#version 100
					
					#ifdef VERTEX
					attribute vec4 _glesTANGENT;
					attribute vec4 _glesVertex;
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
					varying highp vec4 xlv_TEXCOORD3;
					varying mediump vec3 xlv_TEXCOORD4;
					void main ()
					{
					  lowp vec3 worldNormal_1;
					  mediump vec3 tmpvar_2;
					  highp vec4 tmpvar_3;
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
					  worldNormal_1 = tmpvar_42;
					  tmpvar_2 = worldNormal_1;
					  tmpvar_3.zw = vec2(0.0, 0.0);
					  tmpvar_3.xy = vec2(0.0, 0.0);
					  mediump vec3 normal_43;
					  normal_43 = worldNormal_1;
					  mediump vec3 x1_44;
					  mediump vec4 tmpvar_45;
					  tmpvar_45 = (normal_43.xyzz * normal_43.yzzx);
					  x1_44.x = dot (unity_SHBr, tmpvar_45);
					  x1_44.y = dot (unity_SHBg, tmpvar_45);
					  x1_44.z = dot (unity_SHBb, tmpvar_45);
					  gl_Position = (glstate_matrix_mvp * vertex_22);
					  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
					  xlv_TEXCOORD1 = tmpvar_2;
					  xlv_TEXCOORD2 = (_Object2World * vertex_22).xyz;
					  xlv_TEXCOORD3 = tmpvar_3;
					  xlv_TEXCOORD4 = (x1_44 + (unity_SHC.xyz * (
					    (normal_43.x * normal_43.x)
					   - 
					    (normal_43.y * normal_43.y)
					  )));
					}
					
					
					#endif
					#ifdef FRAGMENT
					#extension GL_EXT_draw_buffers : enable
					uniform highp vec4 _Time;
					uniform mediump vec4 unity_SHAr;
					uniform mediump vec4 unity_SHAg;
					uniform mediump vec4 unity_SHAb;
					uniform lowp vec4 _SpecColor;
					uniform sampler2D _MainTex;
					uniform lowp vec4 _Color;
					uniform lowp vec2 _V_CW_MainTex_Scroll;
					varying highp vec2 xlv_TEXCOORD0;
					varying mediump vec3 xlv_TEXCOORD1;
					varying mediump vec3 xlv_TEXCOORD4;
					void main ()
					{
					  mediump vec4 outDiffuse_1;
					  lowp vec3 tmpvar_2;
					  tmpvar_2 = xlv_TEXCOORD1;
					  lowp vec3 tmpvar_3;
					  highp vec2 P_4;
					  P_4 = (xlv_TEXCOORD0 + (_V_CW_MainTex_Scroll * _Time.x));
					  tmpvar_3 = (texture2D (_MainTex, P_4).xyz * _Color.xyz);
					  mediump vec3 normalWorld_5;
					  normalWorld_5 = tmpvar_2;
					  mediump vec4 tmpvar_6;
					  tmpvar_6.w = 1.0;
					  tmpvar_6.xyz = normalWorld_5;
					  mediump vec3 x_7;
					  x_7.x = dot (unity_SHAr, tmpvar_6);
					  x_7.y = dot (unity_SHAg, tmpvar_6);
					  x_7.z = dot (unity_SHAb, tmpvar_6);
					  mediump vec4 outDiffuseOcclusion_8;
					  mediump vec4 outNormal_9;
					  mediump vec4 emission_10;
					  lowp vec4 tmpvar_11;
					  tmpvar_11.w = 1.0;
					  tmpvar_11.xyz = tmpvar_3;
					  outDiffuseOcclusion_8 = tmpvar_11;
					  mediump vec4 tmpvar_12;
					  tmpvar_12.xyz = _SpecColor.xyz;
					  tmpvar_12.w = 0.0;
					  lowp vec4 tmpvar_13;
					  tmpvar_13.w = 1.0;
					  tmpvar_13.xyz = ((tmpvar_2 * 0.5) + 0.5);
					  outNormal_9 = tmpvar_13;
					  lowp vec4 tmpvar_14;
					  tmpvar_14.w = 1.0;
					  tmpvar_14.xyz = vec3(0.0, 0.0, 0.0);
					  emission_10 = tmpvar_14;
					  emission_10.xyz = (emission_10.xyz + (tmpvar_3 * max (
					    ((1.055 * pow (max (vec3(0.0, 0.0, 0.0), 
					      (xlv_TEXCOORD4 + x_7)
					    ), vec3(0.4166667, 0.4166667, 0.4166667))) - 0.055)
					  , vec3(0.0, 0.0, 0.0))));
					  outDiffuse_1.xyz = outDiffuseOcclusion_8.xyz;
					  outDiffuse_1.w = 1.0;
					  gl_FragData[0] = outDiffuse_1;
					  gl_FragData[1] = tmpvar_12;
					  gl_FragData[2] = outNormal_9;
					  gl_FragData[3] = emission_10;
					}
					
					
					#endif"
}
}
Program "fp" {
SubProgram "gles " {
Keywords { "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "HDR_LIGHT_PREPASS_OFF" "V_CW_REFLECTIVE_OFF" "V_CW_VERTEX_COLOR_OFF" "_EMISSION_OFF" "V_CW_RIM_OFF" "_NORMALMAP_OFF" "V_CW_SPECULAR_OFF" }
					"!!GLES"
}
SubProgram "gles " {
Keywords { "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "UNITY_HDR_ON" "V_CW_REFLECTIVE_OFF" "V_CW_VERTEX_COLOR_OFF" "_EMISSION_OFF" "V_CW_RIM_OFF" "_NORMALMAP_OFF" "V_CW_SPECULAR_OFF" }
					"!!GLES"
}
}
 }
}
Fallback "Hidden/VacuumShaders/Curved World/VertexLit/Transparent"
}