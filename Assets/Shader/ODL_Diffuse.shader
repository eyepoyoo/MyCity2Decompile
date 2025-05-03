Shader "VacuumShaders/Curved World/One Directional Light" {
Properties {
[CurvedWorldGearMenu]  V_CW_Label_Tag ("", Float) = 0
[CurvedWorldLabel]  V_CW_Label_UnityDefaults ("Default Visual Options", Float) = 0
[CurvedWorldLargeLabel]  V_CW_Label_Modes ("Modes", Float) = 0
[CurvedWorldRenderingMode]  V_CW_Rendering_Mode ("  Rendering", Float) = 0
[CurvedWorldTextureMixMode]  V_CW_Texture_Mix_Mode ("  Texture Mix", Float) = 0
[CurvedWorldLargeLabel]  V_CW_Label_Albedo ("Albedo", Float) = 0
 _Color ("  Color", Color) = (1,1,1,1)
 _MainTex ("  Map (RGB) RefStr & Gloss (A)", 2D) = "white" { }
[CurvedWorldUVScroll]  _V_CW_MainTex_Scroll ("    ", Vector) = (0,0,0,0)
[CurvedWorldLabel]  V_CW_Label_UnityDefaults ("Curved World Optionals", Float) = 0
[HideInInspector]  _V_CW_Rim_Color ("", Color) = (1,1,1,1)
[HideInInspector]  _V_CW_Rim_Bias ("", Range(-1,1)) = 0.2
[HideInInspector]  _V_CW_Rim_Power ("", Range(0.5,8)) = 3
[HideInInspector]  _EmissionMap ("", 2D) = "white" { }
[HideInInspector]  _EmissionColor ("", Color) = (1,1,1,1)
[HideInInspector]  _V_CW_IBL_Intensity ("", Float) = 1
[HideInInspector]  _V_CW_IBL_Contrast ("", Float) = 1
[HideInInspector]  _V_CW_IBL_Cube ("", CUBE) = "" { }
[HideInInspector]  _V_CW_ReflectColor ("", Color) = (1,1,1,1)
[HideInInspector]  _V_CW_ReflectStrengthAlphaOffset ("", Range(-1,1)) = 0
[HideInInspector]  _V_CW_Cube ("", CUBE) = "_Skybox" { }
[HideInInspector]  _V_CW_Fresnel_Bias ("", Range(-1,1)) = 0
[HideInInspector]  _V_CW_Specular_Intensity ("", Range(0,5)) = 1
[HideInInspector]  _V_CW_SpecularOffset ("", Range(-0.25,0.25)) = 0
[HideInInspector]  _V_CW_Specular_Lookup ("", 2D) = "black" { }
[HideInInspector]  _V_CW_NormalMapStrength ("", Float) = 1
[HideInInspector]  _V_CW_NormalMap ("", 2D) = "bump" { }
[HideInInspector]  _V_CW_NormalMap_UV_Scale ("", Float) = 1
[HideInInspector]  _V_CW_SecondaryNormalMap ("", 2D) = "" { }
[HideInInspector]  _V_CW_SecondaryNormalMap_UV_Scale ("", Float) = 1
[HideInInspector]  _V_CW_LightRampTex ("", 2D) = "grey" { }
}
SubShader { 
 LOD 200
 Tags { "RenderType"="CurvedWorld_Opaque" "CurvedWorldTag"="One Directional Light/Opaque/Texture" "CurvedWorldNoneRemoveableKeywords"="" "CurvedWorldAvailableOptions"="V_CW_USE_LIGHT_RAMP_TEXTURE;V_CW_REFLECTIVE;V_CW_VERTEX_COLOR;_EMISSION;V_CW_RIM;V_CW_FOG;_NORMALMAP;V_CW_SPECULAR_LOOKUP;" }
 Pass {
  Name "FORWARD"
  Tags { "LIGHTMODE"="ForwardBase" "SHADOWSUPPORT"="true" "RenderType"="CurvedWorld_Opaque" "CurvedWorldTag"="One Directional Light/Opaque/Texture" "CurvedWorldNoneRemoveableKeywords"="" "CurvedWorldAvailableOptions"="V_CW_USE_LIGHT_RAMP_TEXTURE;V_CW_REFLECTIVE;V_CW_VERTEX_COLOR;_EMISSION;V_CW_RIM;V_CW_FOG;_NORMALMAP;V_CW_SPECULAR_LOOKUP;" }
  GpuProgramID 2473
Program "vp" {
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_OFF" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "V_CW_VERTEX_COLOR" "_NORMALMAP" "_EMISSION_OFF" "V_CW_RIM_OFF" "V_CW_SPECULAR_OFF" "V_CW_REFLECTIVE" "V_CW_FOG" "V_CW_USE_LIGHT_RAMP_TEXTURE_OFF" }
					"!!GLES
					#version 100
					
					#ifdef VERTEX
					attribute vec4 _glesTANGENT;
					attribute vec4 _glesVertex;
					attribute vec4 _glesColor;
					attribute vec3 _glesNormal;
					attribute vec4 _glesMultiTexCoord0;
					uniform highp vec4 _Time;
					uniform highp vec3 _WorldSpaceCameraPos;
					uniform mediump vec4 _WorldSpaceLightPos0;
					uniform highp mat4 glstate_matrix_mvp;
					uniform highp mat4 _Object2World;
					uniform highp mat4 _World2Object;
					uniform highp vec3 _V_CW_Bend;
					uniform highp vec3 _V_CW_Bias;
					uniform highp vec4 _V_CW_PivotPoint_Position;
					uniform highp vec4 _MainTex_ST;
					uniform lowp vec2 _V_CW_MainTex_Scroll;
					varying mediump vec4 xlv_TEXCOORD0;
					varying mediump vec4 xlv_TEXCOORD1;
					varying mediump vec4 xlv_TEXCOORD2;
					varying highp vec4 xlv_COLOR0;
					varying mediump vec4 xlv_TEXCOORD4;
					varying mediump vec3 xlv_TEXCOORD6;
					void main ()
					{
					  lowp vec4 tmpvar_1;
					  tmpvar_1 = _glesColor;
					  mediump vec4 tmpvar_2;
					  mediump vec4 tmpvar_3;
					  mediump vec4 tmpvar_4;
					  highp vec4 tmpvar_5;
					  mediump vec3 tmpvar_6;
					  highp vec4 v_7;
					  highp float tmpvar_8;
					  tmpvar_8 = _World2Object[0].x;
					  v_7.x = tmpvar_8;
					  highp float tmpvar_9;
					  tmpvar_9 = _World2Object[1].x;
					  v_7.y = tmpvar_9;
					  highp float tmpvar_10;
					  tmpvar_10 = _World2Object[2].x;
					  v_7.z = tmpvar_10;
					  highp float tmpvar_11;
					  tmpvar_11 = _World2Object[3].x;
					  v_7.w = tmpvar_11;
					  highp vec4 v_12;
					  highp float tmpvar_13;
					  tmpvar_13 = _World2Object[0].y;
					  v_12.x = tmpvar_13;
					  highp float tmpvar_14;
					  tmpvar_14 = _World2Object[1].y;
					  v_12.y = tmpvar_14;
					  highp float tmpvar_15;
					  tmpvar_15 = _World2Object[2].y;
					  v_12.z = tmpvar_15;
					  highp float tmpvar_16;
					  tmpvar_16 = _World2Object[3].y;
					  v_12.w = tmpvar_16;
					  highp vec4 v_17;
					  highp float tmpvar_18;
					  tmpvar_18 = _World2Object[0].z;
					  v_17.x = tmpvar_18;
					  highp float tmpvar_19;
					  tmpvar_19 = _World2Object[1].z;
					  v_17.y = tmpvar_19;
					  highp float tmpvar_20;
					  tmpvar_20 = _World2Object[2].z;
					  v_17.z = tmpvar_20;
					  highp float tmpvar_21;
					  tmpvar_21 = _World2Object[3].z;
					  v_17.w = tmpvar_21;
					  highp vec3 tmpvar_22;
					  tmpvar_22 = normalize(((
					    (v_7.xyz * _glesNormal.x)
					   + 
					    (v_12.xyz * _glesNormal.y)
					  ) + (v_17.xyz * _glesNormal.z)));
					  highp mat3 tmpvar_23;
					  tmpvar_23[0] = _Object2World[0].xyz;
					  tmpvar_23[1] = _Object2World[1].xyz;
					  tmpvar_23[2] = _Object2World[2].xyz;
					  highp vec3 tmpvar_24;
					  tmpvar_24 = normalize((tmpvar_23 * _glesTANGENT.xyz));
					  highp vec4 vertex_25;
					  vertex_25.w = _glesVertex.w;
					  highp vec2 xzOff_26;
					  highp vec3 v2_27;
					  highp vec3 v1_28;
					  highp vec3 v0_29;
					  highp vec3 tmpvar_30;
					  tmpvar_30 = ((_Object2World * _glesVertex).xyz - _V_CW_PivotPoint_Position.xyz);
					  highp vec3 tmpvar_31;
					  tmpvar_31 = (tmpvar_30 + tmpvar_24);
					  v1_28.xz = tmpvar_31.xz;
					  highp vec3 tmpvar_32;
					  tmpvar_32 = (tmpvar_30 - ((tmpvar_22.yzx * tmpvar_24.zxy) - (tmpvar_22.zxy * tmpvar_24.yzx)));
					  v2_27.xz = tmpvar_32.xz;
					  highp vec2 tmpvar_33;
					  tmpvar_33.x = float((tmpvar_30.z >= 0.0));
					  tmpvar_33.y = float((tmpvar_30.x >= 0.0));
					  xzOff_26 = (max (vec2(0.0, 0.0), (
					    abs(tmpvar_30.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_33 * 2.0) - 1.0));
					  xzOff_26 = (xzOff_26 * xzOff_26);
					  highp vec3 tmpvar_34;
					  tmpvar_34.xz = vec2(0.0, 0.0);
					  tmpvar_34.y = (((_V_CW_Bend.x * xzOff_26.x) + (_V_CW_Bend.z * xzOff_26.y)) * 0.001);
					  v0_29 = (tmpvar_30 + tmpvar_34);
					  highp vec2 tmpvar_35;
					  tmpvar_35.x = float((tmpvar_31.z >= 0.0));
					  tmpvar_35.y = float((tmpvar_31.x >= 0.0));
					  xzOff_26 = (max (vec2(0.0, 0.0), (
					    abs(tmpvar_31.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_35 * 2.0) - 1.0));
					  xzOff_26 = (xzOff_26 * xzOff_26);
					  v1_28.y = (tmpvar_31.y + ((
					    (_V_CW_Bend.x * xzOff_26.x)
					   + 
					    (_V_CW_Bend.z * xzOff_26.y)
					  ) * 0.001));
					  highp vec2 tmpvar_36;
					  tmpvar_36.x = float((tmpvar_32.z >= 0.0));
					  tmpvar_36.y = float((tmpvar_32.x >= 0.0));
					  xzOff_26 = (max (vec2(0.0, 0.0), (
					    abs(tmpvar_32.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_36 * 2.0) - 1.0));
					  xzOff_26 = (xzOff_26 * xzOff_26);
					  v2_27.y = (tmpvar_32.y + ((
					    (_V_CW_Bend.x * xzOff_26.x)
					   + 
					    (_V_CW_Bend.z * xzOff_26.y)
					  ) * 0.001));
					  highp mat3 tmpvar_37;
					  tmpvar_37[0] = _World2Object[0].xyz;
					  tmpvar_37[1] = _World2Object[1].xyz;
					  tmpvar_37[2] = _World2Object[2].xyz;
					  vertex_25.xyz = (_glesVertex.xyz + (tmpvar_37 * tmpvar_34));
					  highp mat3 tmpvar_38;
					  tmpvar_38[0] = _World2Object[0].xyz;
					  tmpvar_38[1] = _World2Object[1].xyz;
					  tmpvar_38[2] = _World2Object[2].xyz;
					  highp vec3 a_39;
					  a_39 = (v2_27 - v0_29);
					  highp vec3 b_40;
					  b_40 = (v1_28 - v0_29);
					  highp vec3 tmpvar_41;
					  tmpvar_41 = normalize((tmpvar_38 * normalize(
					    ((a_39.yzx * b_40.zxy) - (a_39.zxy * b_40.yzx))
					  )));
					  tmpvar_2.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
					  tmpvar_2.xy = (tmpvar_2.xy + (_V_CW_MainTex_Scroll * _Time.x));
					  tmpvar_5 = tmpvar_1;
					  highp vec4 v_42;
					  v_42.x = tmpvar_8;
					  v_42.y = tmpvar_9;
					  v_42.z = tmpvar_10;
					  v_42.w = tmpvar_11;
					  highp vec4 v_43;
					  v_43.x = tmpvar_13;
					  v_43.y = tmpvar_14;
					  v_43.z = tmpvar_15;
					  v_43.w = tmpvar_16;
					  highp vec4 v_44;
					  v_44.x = tmpvar_18;
					  v_44.y = tmpvar_19;
					  v_44.z = tmpvar_20;
					  v_44.w = tmpvar_21;
					  highp vec3 tmpvar_45;
					  tmpvar_45 = normalize(((
					    (v_42.xyz * tmpvar_41.x)
					   + 
					    (v_43.xyz * tmpvar_41.y)
					  ) + (v_44.xyz * tmpvar_41.z)));
					  highp vec3 tmpvar_46;
					  tmpvar_46 = normalize(tmpvar_41);
					  highp vec3 tmpvar_47;
					  tmpvar_47 = normalize(_glesTANGENT.xyz);
					  highp vec3 tmpvar_48;
					  highp vec3 tmpvar_49;
					  tmpvar_48 = _glesTANGENT.xyz;
					  tmpvar_49 = (((tmpvar_46.yzx * tmpvar_47.zxy) - (tmpvar_46.zxy * tmpvar_47.yzx)) * _glesTANGENT.w);
					  highp mat3 tmpvar_50;
					  tmpvar_50[0].x = tmpvar_48.x;
					  tmpvar_50[0].y = tmpvar_49.x;
					  tmpvar_50[0].z = tmpvar_41.x;
					  tmpvar_50[1].x = tmpvar_48.y;
					  tmpvar_50[1].y = tmpvar_49.y;
					  tmpvar_50[1].z = tmpvar_41.y;
					  tmpvar_50[2].x = tmpvar_48.z;
					  tmpvar_50[2].y = tmpvar_49.z;
					  tmpvar_50[2].z = tmpvar_41.z;
					  highp vec3 tmpvar_51;
					  tmpvar_51 = normalize((tmpvar_50 * (_World2Object * _WorldSpaceLightPos0).xyz));
					  tmpvar_6 = tmpvar_51;
					  tmpvar_3.xyz = tmpvar_45;
					  highp vec3 I_52;
					  I_52 = ((_Object2World * vertex_25).xyz - _WorldSpaceCameraPos);
					  highp vec3 tmpvar_53;
					  tmpvar_53 = normalize((I_52 - (2.0 * 
					    (dot (tmpvar_45, I_52) * tmpvar_45)
					  )));
					  tmpvar_4.xyz = tmpvar_53;
					  gl_Position = (glstate_matrix_mvp * vertex_25);
					  xlv_TEXCOORD0 = tmpvar_2;
					  xlv_TEXCOORD1 = tmpvar_3;
					  xlv_TEXCOORD2 = tmpvar_4;
					  xlv_COLOR0 = tmpvar_5;
					  xlv_TEXCOORD4 = vec4(0.0, 0.0, 0.0, 0.0);
					  xlv_TEXCOORD6 = tmpvar_6;
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform lowp vec4 glstate_lightmodel_ambient;
					uniform lowp vec4 _LightColor0;
					uniform lowp vec4 _Color;
					uniform sampler2D _MainTex;
					uniform sampler2D _V_CW_NormalMap;
					uniform mediump float _V_CW_NormalMap_UV_Scale;
					uniform mediump float _V_CW_NormalMapStrength;
					uniform lowp samplerCube _V_CW_Cube;
					uniform lowp vec4 _V_CW_ReflectColor;
					uniform lowp float _V_CW_ReflectStrengthAlphaOffset;
					varying mediump vec4 xlv_TEXCOORD0;
					varying mediump vec4 xlv_TEXCOORD2;
					varying highp vec4 xlv_COLOR0;
					varying mediump vec3 xlv_TEXCOORD6;
					void main ()
					{
					  lowp vec3 diff_1;
					  mediump vec3 normal_2;
					  lowp vec3 bumpNormal_3;
					  lowp vec4 retColor_4;
					  mediump vec4 mainTex_5;
					  lowp vec4 tmpvar_6;
					  tmpvar_6 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
					  mainTex_5 = tmpvar_6;
					  retColor_4 = mainTex_5;
					  retColor_4 = (retColor_4 * _Color);
					  retColor_4 = (retColor_4 * xlv_COLOR0);
					  mediump vec2 P_7;
					  P_7 = (xlv_TEXCOORD0.xy * _V_CW_NormalMap_UV_Scale);
					  lowp vec3 normal_8;
					  normal_8.xy = ((texture2D (_V_CW_NormalMap, P_7).wy * 2.0) - 1.0);
					  normal_8.z = sqrt((1.0 - clamp (
					    dot (normal_8.xy, normal_8.xy)
					  , 0.0, 1.0)));
					  mediump vec3 tmpvar_9;
					  tmpvar_9.xy = (normal_8.xy * vec2(_V_CW_NormalMapStrength));
					  tmpvar_9.z = normal_8.z;
					  mediump vec3 tmpvar_10;
					  tmpvar_10 = normalize(tmpvar_9);
					  bumpNormal_3 = tmpvar_10;
					  normal_2 = bumpNormal_3;
					  mediump float tmpvar_11;
					  tmpvar_11 = max (0.0, dot (normal_2, xlv_TEXCOORD6));
					  diff_1 = (_LightColor0.xyz * tmpvar_11);
					  diff_1 = (diff_1 + (glstate_lightmodel_ambient * 2.0).xyz);
					  retColor_4.xyz = (diff_1 * retColor_4.xyz);
					  mediump vec3 P_12;
					  P_12 = (xlv_TEXCOORD2.xyz + bumpNormal_3);
					  retColor_4.xyz = (retColor_4.xyz + ((textureCube (_V_CW_Cube, P_12) * _V_CW_ReflectColor).xyz * clamp (
					    (retColor_4.w + _V_CW_ReflectStrengthAlphaOffset)
					  , 0.0, 1.0)));
					  retColor_4.w = 1.0;
					  gl_FragData[0] = retColor_4;
					}
					
					
					#endif"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "V_CW_VERTEX_COLOR" "_NORMALMAP" "_EMISSION_OFF" "V_CW_RIM_OFF" "V_CW_SPECULAR_OFF" "V_CW_REFLECTIVE" "V_CW_FOG" "V_CW_USE_LIGHT_RAMP_TEXTURE_OFF" }
					"!!GLES
					#version 100
					
					#ifdef VERTEX
					attribute vec4 _glesTANGENT;
					attribute vec4 _glesVertex;
					attribute vec4 _glesColor;
					attribute vec3 _glesNormal;
					attribute vec4 _glesMultiTexCoord0;
					uniform highp vec4 _Time;
					uniform highp vec3 _WorldSpaceCameraPos;
					uniform highp vec4 _ProjectionParams;
					uniform mediump vec4 _WorldSpaceLightPos0;
					uniform highp mat4 glstate_matrix_mvp;
					uniform highp mat4 _Object2World;
					uniform highp mat4 _World2Object;
					uniform highp vec3 _V_CW_Bend;
					uniform highp vec3 _V_CW_Bias;
					uniform highp vec4 _V_CW_PivotPoint_Position;
					uniform highp vec4 _MainTex_ST;
					uniform lowp vec2 _V_CW_MainTex_Scroll;
					varying mediump vec4 xlv_TEXCOORD0;
					varying mediump vec4 xlv_TEXCOORD1;
					varying mediump vec4 xlv_TEXCOORD2;
					varying highp vec4 xlv_COLOR0;
					varying mediump vec4 xlv_TEXCOORD4;
					varying mediump vec3 xlv_TEXCOORD6;
					varying mediump vec4 xlv_TEXCOORD7;
					void main ()
					{
					  lowp vec4 tmpvar_1;
					  tmpvar_1 = _glesColor;
					  highp vec4 tmpvar_2;
					  mediump vec4 tmpvar_3;
					  mediump vec4 tmpvar_4;
					  mediump vec4 tmpvar_5;
					  highp vec4 tmpvar_6;
					  mediump vec3 tmpvar_7;
					  mediump vec4 tmpvar_8;
					  highp vec4 v_9;
					  highp float tmpvar_10;
					  tmpvar_10 = _World2Object[0].x;
					  v_9.x = tmpvar_10;
					  highp float tmpvar_11;
					  tmpvar_11 = _World2Object[1].x;
					  v_9.y = tmpvar_11;
					  highp float tmpvar_12;
					  tmpvar_12 = _World2Object[2].x;
					  v_9.z = tmpvar_12;
					  highp float tmpvar_13;
					  tmpvar_13 = _World2Object[3].x;
					  v_9.w = tmpvar_13;
					  highp vec4 v_14;
					  highp float tmpvar_15;
					  tmpvar_15 = _World2Object[0].y;
					  v_14.x = tmpvar_15;
					  highp float tmpvar_16;
					  tmpvar_16 = _World2Object[1].y;
					  v_14.y = tmpvar_16;
					  highp float tmpvar_17;
					  tmpvar_17 = _World2Object[2].y;
					  v_14.z = tmpvar_17;
					  highp float tmpvar_18;
					  tmpvar_18 = _World2Object[3].y;
					  v_14.w = tmpvar_18;
					  highp vec4 v_19;
					  highp float tmpvar_20;
					  tmpvar_20 = _World2Object[0].z;
					  v_19.x = tmpvar_20;
					  highp float tmpvar_21;
					  tmpvar_21 = _World2Object[1].z;
					  v_19.y = tmpvar_21;
					  highp float tmpvar_22;
					  tmpvar_22 = _World2Object[2].z;
					  v_19.z = tmpvar_22;
					  highp float tmpvar_23;
					  tmpvar_23 = _World2Object[3].z;
					  v_19.w = tmpvar_23;
					  highp vec3 tmpvar_24;
					  tmpvar_24 = normalize(((
					    (v_9.xyz * _glesNormal.x)
					   + 
					    (v_14.xyz * _glesNormal.y)
					  ) + (v_19.xyz * _glesNormal.z)));
					  highp mat3 tmpvar_25;
					  tmpvar_25[0] = _Object2World[0].xyz;
					  tmpvar_25[1] = _Object2World[1].xyz;
					  tmpvar_25[2] = _Object2World[2].xyz;
					  highp vec3 tmpvar_26;
					  tmpvar_26 = normalize((tmpvar_25 * _glesTANGENT.xyz));
					  highp vec4 vertex_27;
					  vertex_27.w = _glesVertex.w;
					  highp vec2 xzOff_28;
					  highp vec3 v2_29;
					  highp vec3 v1_30;
					  highp vec3 v0_31;
					  highp vec3 tmpvar_32;
					  tmpvar_32 = ((_Object2World * _glesVertex).xyz - _V_CW_PivotPoint_Position.xyz);
					  highp vec3 tmpvar_33;
					  tmpvar_33 = (tmpvar_32 + tmpvar_26);
					  v1_30.xz = tmpvar_33.xz;
					  highp vec3 tmpvar_34;
					  tmpvar_34 = (tmpvar_32 - ((tmpvar_24.yzx * tmpvar_26.zxy) - (tmpvar_24.zxy * tmpvar_26.yzx)));
					  v2_29.xz = tmpvar_34.xz;
					  highp vec2 tmpvar_35;
					  tmpvar_35.x = float((tmpvar_32.z >= 0.0));
					  tmpvar_35.y = float((tmpvar_32.x >= 0.0));
					  xzOff_28 = (max (vec2(0.0, 0.0), (
					    abs(tmpvar_32.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_35 * 2.0) - 1.0));
					  xzOff_28 = (xzOff_28 * xzOff_28);
					  highp vec3 tmpvar_36;
					  tmpvar_36.xz = vec2(0.0, 0.0);
					  tmpvar_36.y = (((_V_CW_Bend.x * xzOff_28.x) + (_V_CW_Bend.z * xzOff_28.y)) * 0.001);
					  v0_31 = (tmpvar_32 + tmpvar_36);
					  highp vec2 tmpvar_37;
					  tmpvar_37.x = float((tmpvar_33.z >= 0.0));
					  tmpvar_37.y = float((tmpvar_33.x >= 0.0));
					  xzOff_28 = (max (vec2(0.0, 0.0), (
					    abs(tmpvar_33.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_37 * 2.0) - 1.0));
					  xzOff_28 = (xzOff_28 * xzOff_28);
					  v1_30.y = (tmpvar_33.y + ((
					    (_V_CW_Bend.x * xzOff_28.x)
					   + 
					    (_V_CW_Bend.z * xzOff_28.y)
					  ) * 0.001));
					  highp vec2 tmpvar_38;
					  tmpvar_38.x = float((tmpvar_34.z >= 0.0));
					  tmpvar_38.y = float((tmpvar_34.x >= 0.0));
					  xzOff_28 = (max (vec2(0.0, 0.0), (
					    abs(tmpvar_34.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_38 * 2.0) - 1.0));
					  xzOff_28 = (xzOff_28 * xzOff_28);
					  v2_29.y = (tmpvar_34.y + ((
					    (_V_CW_Bend.x * xzOff_28.x)
					   + 
					    (_V_CW_Bend.z * xzOff_28.y)
					  ) * 0.001));
					  highp mat3 tmpvar_39;
					  tmpvar_39[0] = _World2Object[0].xyz;
					  tmpvar_39[1] = _World2Object[1].xyz;
					  tmpvar_39[2] = _World2Object[2].xyz;
					  vertex_27.xyz = (_glesVertex.xyz + (tmpvar_39 * tmpvar_36));
					  highp mat3 tmpvar_40;
					  tmpvar_40[0] = _World2Object[0].xyz;
					  tmpvar_40[1] = _World2Object[1].xyz;
					  tmpvar_40[2] = _World2Object[2].xyz;
					  highp vec3 a_41;
					  a_41 = (v2_29 - v0_31);
					  highp vec3 b_42;
					  b_42 = (v1_30 - v0_31);
					  highp vec3 tmpvar_43;
					  tmpvar_43 = normalize((tmpvar_40 * normalize(
					    ((a_41.yzx * b_42.zxy) - (a_41.zxy * b_42.yzx))
					  )));
					  tmpvar_2 = (glstate_matrix_mvp * vertex_27);
					  tmpvar_3.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
					  tmpvar_3.xy = (tmpvar_3.xy + (_V_CW_MainTex_Scroll * _Time.x));
					  tmpvar_6 = tmpvar_1;
					  highp vec4 v_44;
					  v_44.x = tmpvar_10;
					  v_44.y = tmpvar_11;
					  v_44.z = tmpvar_12;
					  v_44.w = tmpvar_13;
					  highp vec4 v_45;
					  v_45.x = tmpvar_15;
					  v_45.y = tmpvar_16;
					  v_45.z = tmpvar_17;
					  v_45.w = tmpvar_18;
					  highp vec4 v_46;
					  v_46.x = tmpvar_20;
					  v_46.y = tmpvar_21;
					  v_46.z = tmpvar_22;
					  v_46.w = tmpvar_23;
					  highp vec3 tmpvar_47;
					  tmpvar_47 = normalize(((
					    (v_44.xyz * tmpvar_43.x)
					   + 
					    (v_45.xyz * tmpvar_43.y)
					  ) + (v_46.xyz * tmpvar_43.z)));
					  highp vec3 tmpvar_48;
					  tmpvar_48 = normalize(tmpvar_43);
					  highp vec3 tmpvar_49;
					  tmpvar_49 = normalize(_glesTANGENT.xyz);
					  highp vec3 tmpvar_50;
					  highp vec3 tmpvar_51;
					  tmpvar_50 = _glesTANGENT.xyz;
					  tmpvar_51 = (((tmpvar_48.yzx * tmpvar_49.zxy) - (tmpvar_48.zxy * tmpvar_49.yzx)) * _glesTANGENT.w);
					  highp mat3 tmpvar_52;
					  tmpvar_52[0].x = tmpvar_50.x;
					  tmpvar_52[0].y = tmpvar_51.x;
					  tmpvar_52[0].z = tmpvar_43.x;
					  tmpvar_52[1].x = tmpvar_50.y;
					  tmpvar_52[1].y = tmpvar_51.y;
					  tmpvar_52[1].z = tmpvar_43.y;
					  tmpvar_52[2].x = tmpvar_50.z;
					  tmpvar_52[2].y = tmpvar_51.z;
					  tmpvar_52[2].z = tmpvar_43.z;
					  highp vec3 tmpvar_53;
					  tmpvar_53 = normalize((tmpvar_52 * (_World2Object * _WorldSpaceLightPos0).xyz));
					  tmpvar_7 = tmpvar_53;
					  tmpvar_4.xyz = tmpvar_47;
					  highp vec3 I_54;
					  I_54 = ((_Object2World * vertex_27).xyz - _WorldSpaceCameraPos);
					  highp vec3 tmpvar_55;
					  tmpvar_55 = normalize((I_54 - (2.0 * 
					    (dot (tmpvar_47, I_54) * tmpvar_47)
					  )));
					  tmpvar_5.xyz = tmpvar_55;
					  highp vec4 o_56;
					  highp vec4 tmpvar_57;
					  tmpvar_57 = (tmpvar_2 * 0.5);
					  highp vec2 tmpvar_58;
					  tmpvar_58.x = tmpvar_57.x;
					  tmpvar_58.y = (tmpvar_57.y * _ProjectionParams.x);
					  o_56.xy = (tmpvar_58 + tmpvar_57.w);
					  o_56.zw = tmpvar_2.zw;
					  tmpvar_8 = o_56;
					  gl_Position = tmpvar_2;
					  xlv_TEXCOORD0 = tmpvar_3;
					  xlv_TEXCOORD1 = tmpvar_4;
					  xlv_TEXCOORD2 = tmpvar_5;
					  xlv_COLOR0 = tmpvar_6;
					  xlv_TEXCOORD4 = vec4(0.0, 0.0, 0.0, 0.0);
					  xlv_TEXCOORD6 = tmpvar_7;
					  xlv_TEXCOORD7 = tmpvar_8;
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform lowp vec4 glstate_lightmodel_ambient;
					uniform lowp vec4 _LightColor0;
					uniform sampler2D _ShadowMapTexture;
					uniform lowp vec4 _Color;
					uniform sampler2D _MainTex;
					uniform sampler2D _V_CW_NormalMap;
					uniform mediump float _V_CW_NormalMap_UV_Scale;
					uniform mediump float _V_CW_NormalMapStrength;
					uniform lowp samplerCube _V_CW_Cube;
					uniform lowp vec4 _V_CW_ReflectColor;
					uniform lowp float _V_CW_ReflectStrengthAlphaOffset;
					varying mediump vec4 xlv_TEXCOORD0;
					varying mediump vec4 xlv_TEXCOORD2;
					varying highp vec4 xlv_COLOR0;
					varying mediump vec3 xlv_TEXCOORD6;
					varying mediump vec4 xlv_TEXCOORD7;
					void main ()
					{
					  lowp vec3 diff_1;
					  mediump vec3 normal_2;
					  lowp vec3 bumpNormal_3;
					  lowp vec4 retColor_4;
					  mediump vec4 mainTex_5;
					  lowp vec4 tmpvar_6;
					  tmpvar_6 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
					  mainTex_5 = tmpvar_6;
					  retColor_4 = mainTex_5;
					  retColor_4 = (retColor_4 * _Color);
					  retColor_4 = (retColor_4 * xlv_COLOR0);
					  mediump vec2 P_7;
					  P_7 = (xlv_TEXCOORD0.xy * _V_CW_NormalMap_UV_Scale);
					  lowp vec3 normal_8;
					  normal_8.xy = ((texture2D (_V_CW_NormalMap, P_7).wy * 2.0) - 1.0);
					  normal_8.z = sqrt((1.0 - clamp (
					    dot (normal_8.xy, normal_8.xy)
					  , 0.0, 1.0)));
					  mediump vec3 tmpvar_9;
					  tmpvar_9.xy = (normal_8.xy * vec2(_V_CW_NormalMapStrength));
					  tmpvar_9.z = normal_8.z;
					  mediump vec3 tmpvar_10;
					  tmpvar_10 = normalize(tmpvar_9);
					  bumpNormal_3 = tmpvar_10;
					  normal_2 = bumpNormal_3;
					  mediump float tmpvar_11;
					  tmpvar_11 = max (0.0, dot (normal_2, xlv_TEXCOORD6));
					  diff_1 = ((_LightColor0.xyz * texture2DProj (_ShadowMapTexture, xlv_TEXCOORD7).x) * tmpvar_11);
					  diff_1 = (diff_1 + (glstate_lightmodel_ambient * 2.0).xyz);
					  retColor_4.xyz = (diff_1 * retColor_4.xyz);
					  mediump vec3 P_12;
					  P_12 = (xlv_TEXCOORD2.xyz + bumpNormal_3);
					  retColor_4.xyz = (retColor_4.xyz + ((textureCube (_V_CW_Cube, P_12) * _V_CW_ReflectColor).xyz * clamp (
					    (retColor_4.w + _V_CW_ReflectStrengthAlphaOffset)
					  , 0.0, 1.0)));
					  retColor_4.w = 1.0;
					  gl_FragData[0] = retColor_4;
					}
					
					
					#endif"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_OFF" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "VERTEXLIGHT_ON" "V_CW_VERTEX_COLOR" "_NORMALMAP" "_EMISSION_OFF" "V_CW_RIM_OFF" "V_CW_SPECULAR_OFF" "V_CW_REFLECTIVE" "V_CW_FOG" "V_CW_USE_LIGHT_RAMP_TEXTURE_OFF" }
					"!!GLES
					#version 100
					
					#ifdef VERTEX
					attribute vec4 _glesTANGENT;
					attribute vec4 _glesVertex;
					attribute vec4 _glesColor;
					attribute vec3 _glesNormal;
					attribute vec4 _glesMultiTexCoord0;
					uniform highp vec4 _Time;
					uniform highp vec3 _WorldSpaceCameraPos;
					uniform mediump vec4 _WorldSpaceLightPos0;
					uniform highp mat4 glstate_matrix_mvp;
					uniform highp mat4 _Object2World;
					uniform highp mat4 _World2Object;
					uniform highp vec3 _V_CW_Bend;
					uniform highp vec3 _V_CW_Bias;
					uniform highp vec4 _V_CW_PivotPoint_Position;
					uniform highp vec4 _MainTex_ST;
					uniform lowp vec2 _V_CW_MainTex_Scroll;
					varying mediump vec4 xlv_TEXCOORD0;
					varying mediump vec4 xlv_TEXCOORD1;
					varying mediump vec4 xlv_TEXCOORD2;
					varying highp vec4 xlv_COLOR0;
					varying mediump vec4 xlv_TEXCOORD4;
					varying mediump vec3 xlv_TEXCOORD6;
					void main ()
					{
					  lowp vec4 tmpvar_1;
					  tmpvar_1 = _glesColor;
					  mediump vec4 tmpvar_2;
					  mediump vec4 tmpvar_3;
					  mediump vec4 tmpvar_4;
					  highp vec4 tmpvar_5;
					  mediump vec3 tmpvar_6;
					  highp vec4 v_7;
					  highp float tmpvar_8;
					  tmpvar_8 = _World2Object[0].x;
					  v_7.x = tmpvar_8;
					  highp float tmpvar_9;
					  tmpvar_9 = _World2Object[1].x;
					  v_7.y = tmpvar_9;
					  highp float tmpvar_10;
					  tmpvar_10 = _World2Object[2].x;
					  v_7.z = tmpvar_10;
					  highp float tmpvar_11;
					  tmpvar_11 = _World2Object[3].x;
					  v_7.w = tmpvar_11;
					  highp vec4 v_12;
					  highp float tmpvar_13;
					  tmpvar_13 = _World2Object[0].y;
					  v_12.x = tmpvar_13;
					  highp float tmpvar_14;
					  tmpvar_14 = _World2Object[1].y;
					  v_12.y = tmpvar_14;
					  highp float tmpvar_15;
					  tmpvar_15 = _World2Object[2].y;
					  v_12.z = tmpvar_15;
					  highp float tmpvar_16;
					  tmpvar_16 = _World2Object[3].y;
					  v_12.w = tmpvar_16;
					  highp vec4 v_17;
					  highp float tmpvar_18;
					  tmpvar_18 = _World2Object[0].z;
					  v_17.x = tmpvar_18;
					  highp float tmpvar_19;
					  tmpvar_19 = _World2Object[1].z;
					  v_17.y = tmpvar_19;
					  highp float tmpvar_20;
					  tmpvar_20 = _World2Object[2].z;
					  v_17.z = tmpvar_20;
					  highp float tmpvar_21;
					  tmpvar_21 = _World2Object[3].z;
					  v_17.w = tmpvar_21;
					  highp vec3 tmpvar_22;
					  tmpvar_22 = normalize(((
					    (v_7.xyz * _glesNormal.x)
					   + 
					    (v_12.xyz * _glesNormal.y)
					  ) + (v_17.xyz * _glesNormal.z)));
					  highp mat3 tmpvar_23;
					  tmpvar_23[0] = _Object2World[0].xyz;
					  tmpvar_23[1] = _Object2World[1].xyz;
					  tmpvar_23[2] = _Object2World[2].xyz;
					  highp vec3 tmpvar_24;
					  tmpvar_24 = normalize((tmpvar_23 * _glesTANGENT.xyz));
					  highp vec4 vertex_25;
					  vertex_25.w = _glesVertex.w;
					  highp vec2 xzOff_26;
					  highp vec3 v2_27;
					  highp vec3 v1_28;
					  highp vec3 v0_29;
					  highp vec3 tmpvar_30;
					  tmpvar_30 = ((_Object2World * _glesVertex).xyz - _V_CW_PivotPoint_Position.xyz);
					  highp vec3 tmpvar_31;
					  tmpvar_31 = (tmpvar_30 + tmpvar_24);
					  v1_28.xz = tmpvar_31.xz;
					  highp vec3 tmpvar_32;
					  tmpvar_32 = (tmpvar_30 - ((tmpvar_22.yzx * tmpvar_24.zxy) - (tmpvar_22.zxy * tmpvar_24.yzx)));
					  v2_27.xz = tmpvar_32.xz;
					  highp vec2 tmpvar_33;
					  tmpvar_33.x = float((tmpvar_30.z >= 0.0));
					  tmpvar_33.y = float((tmpvar_30.x >= 0.0));
					  xzOff_26 = (max (vec2(0.0, 0.0), (
					    abs(tmpvar_30.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_33 * 2.0) - 1.0));
					  xzOff_26 = (xzOff_26 * xzOff_26);
					  highp vec3 tmpvar_34;
					  tmpvar_34.xz = vec2(0.0, 0.0);
					  tmpvar_34.y = (((_V_CW_Bend.x * xzOff_26.x) + (_V_CW_Bend.z * xzOff_26.y)) * 0.001);
					  v0_29 = (tmpvar_30 + tmpvar_34);
					  highp vec2 tmpvar_35;
					  tmpvar_35.x = float((tmpvar_31.z >= 0.0));
					  tmpvar_35.y = float((tmpvar_31.x >= 0.0));
					  xzOff_26 = (max (vec2(0.0, 0.0), (
					    abs(tmpvar_31.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_35 * 2.0) - 1.0));
					  xzOff_26 = (xzOff_26 * xzOff_26);
					  v1_28.y = (tmpvar_31.y + ((
					    (_V_CW_Bend.x * xzOff_26.x)
					   + 
					    (_V_CW_Bend.z * xzOff_26.y)
					  ) * 0.001));
					  highp vec2 tmpvar_36;
					  tmpvar_36.x = float((tmpvar_32.z >= 0.0));
					  tmpvar_36.y = float((tmpvar_32.x >= 0.0));
					  xzOff_26 = (max (vec2(0.0, 0.0), (
					    abs(tmpvar_32.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_36 * 2.0) - 1.0));
					  xzOff_26 = (xzOff_26 * xzOff_26);
					  v2_27.y = (tmpvar_32.y + ((
					    (_V_CW_Bend.x * xzOff_26.x)
					   + 
					    (_V_CW_Bend.z * xzOff_26.y)
					  ) * 0.001));
					  highp mat3 tmpvar_37;
					  tmpvar_37[0] = _World2Object[0].xyz;
					  tmpvar_37[1] = _World2Object[1].xyz;
					  tmpvar_37[2] = _World2Object[2].xyz;
					  vertex_25.xyz = (_glesVertex.xyz + (tmpvar_37 * tmpvar_34));
					  highp mat3 tmpvar_38;
					  tmpvar_38[0] = _World2Object[0].xyz;
					  tmpvar_38[1] = _World2Object[1].xyz;
					  tmpvar_38[2] = _World2Object[2].xyz;
					  highp vec3 a_39;
					  a_39 = (v2_27 - v0_29);
					  highp vec3 b_40;
					  b_40 = (v1_28 - v0_29);
					  highp vec3 tmpvar_41;
					  tmpvar_41 = normalize((tmpvar_38 * normalize(
					    ((a_39.yzx * b_40.zxy) - (a_39.zxy * b_40.yzx))
					  )));
					  tmpvar_2.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
					  tmpvar_2.xy = (tmpvar_2.xy + (_V_CW_MainTex_Scroll * _Time.x));
					  tmpvar_5 = tmpvar_1;
					  highp vec4 v_42;
					  v_42.x = tmpvar_8;
					  v_42.y = tmpvar_9;
					  v_42.z = tmpvar_10;
					  v_42.w = tmpvar_11;
					  highp vec4 v_43;
					  v_43.x = tmpvar_13;
					  v_43.y = tmpvar_14;
					  v_43.z = tmpvar_15;
					  v_43.w = tmpvar_16;
					  highp vec4 v_44;
					  v_44.x = tmpvar_18;
					  v_44.y = tmpvar_19;
					  v_44.z = tmpvar_20;
					  v_44.w = tmpvar_21;
					  highp vec3 tmpvar_45;
					  tmpvar_45 = normalize(((
					    (v_42.xyz * tmpvar_41.x)
					   + 
					    (v_43.xyz * tmpvar_41.y)
					  ) + (v_44.xyz * tmpvar_41.z)));
					  highp vec3 tmpvar_46;
					  tmpvar_46 = normalize(tmpvar_41);
					  highp vec3 tmpvar_47;
					  tmpvar_47 = normalize(_glesTANGENT.xyz);
					  highp vec3 tmpvar_48;
					  highp vec3 tmpvar_49;
					  tmpvar_48 = _glesTANGENT.xyz;
					  tmpvar_49 = (((tmpvar_46.yzx * tmpvar_47.zxy) - (tmpvar_46.zxy * tmpvar_47.yzx)) * _glesTANGENT.w);
					  highp mat3 tmpvar_50;
					  tmpvar_50[0].x = tmpvar_48.x;
					  tmpvar_50[0].y = tmpvar_49.x;
					  tmpvar_50[0].z = tmpvar_41.x;
					  tmpvar_50[1].x = tmpvar_48.y;
					  tmpvar_50[1].y = tmpvar_49.y;
					  tmpvar_50[1].z = tmpvar_41.y;
					  tmpvar_50[2].x = tmpvar_48.z;
					  tmpvar_50[2].y = tmpvar_49.z;
					  tmpvar_50[2].z = tmpvar_41.z;
					  highp vec3 tmpvar_51;
					  tmpvar_51 = normalize((tmpvar_50 * (_World2Object * _WorldSpaceLightPos0).xyz));
					  tmpvar_6 = tmpvar_51;
					  tmpvar_3.xyz = tmpvar_45;
					  highp vec3 I_52;
					  I_52 = ((_Object2World * vertex_25).xyz - _WorldSpaceCameraPos);
					  highp vec3 tmpvar_53;
					  tmpvar_53 = normalize((I_52 - (2.0 * 
					    (dot (tmpvar_45, I_52) * tmpvar_45)
					  )));
					  tmpvar_4.xyz = tmpvar_53;
					  gl_Position = (glstate_matrix_mvp * vertex_25);
					  xlv_TEXCOORD0 = tmpvar_2;
					  xlv_TEXCOORD1 = tmpvar_3;
					  xlv_TEXCOORD2 = tmpvar_4;
					  xlv_COLOR0 = tmpvar_5;
					  xlv_TEXCOORD4 = vec4(0.0, 0.0, 0.0, 0.0);
					  xlv_TEXCOORD6 = tmpvar_6;
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform lowp vec4 glstate_lightmodel_ambient;
					uniform lowp vec4 _LightColor0;
					uniform lowp vec4 _Color;
					uniform sampler2D _MainTex;
					uniform sampler2D _V_CW_NormalMap;
					uniform mediump float _V_CW_NormalMap_UV_Scale;
					uniform mediump float _V_CW_NormalMapStrength;
					uniform lowp samplerCube _V_CW_Cube;
					uniform lowp vec4 _V_CW_ReflectColor;
					uniform lowp float _V_CW_ReflectStrengthAlphaOffset;
					varying mediump vec4 xlv_TEXCOORD0;
					varying mediump vec4 xlv_TEXCOORD2;
					varying highp vec4 xlv_COLOR0;
					varying mediump vec3 xlv_TEXCOORD6;
					void main ()
					{
					  lowp vec3 diff_1;
					  mediump vec3 normal_2;
					  lowp vec3 bumpNormal_3;
					  lowp vec4 retColor_4;
					  mediump vec4 mainTex_5;
					  lowp vec4 tmpvar_6;
					  tmpvar_6 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
					  mainTex_5 = tmpvar_6;
					  retColor_4 = mainTex_5;
					  retColor_4 = (retColor_4 * _Color);
					  retColor_4 = (retColor_4 * xlv_COLOR0);
					  mediump vec2 P_7;
					  P_7 = (xlv_TEXCOORD0.xy * _V_CW_NormalMap_UV_Scale);
					  lowp vec3 normal_8;
					  normal_8.xy = ((texture2D (_V_CW_NormalMap, P_7).wy * 2.0) - 1.0);
					  normal_8.z = sqrt((1.0 - clamp (
					    dot (normal_8.xy, normal_8.xy)
					  , 0.0, 1.0)));
					  mediump vec3 tmpvar_9;
					  tmpvar_9.xy = (normal_8.xy * vec2(_V_CW_NormalMapStrength));
					  tmpvar_9.z = normal_8.z;
					  mediump vec3 tmpvar_10;
					  tmpvar_10 = normalize(tmpvar_9);
					  bumpNormal_3 = tmpvar_10;
					  normal_2 = bumpNormal_3;
					  mediump float tmpvar_11;
					  tmpvar_11 = max (0.0, dot (normal_2, xlv_TEXCOORD6));
					  diff_1 = (_LightColor0.xyz * tmpvar_11);
					  diff_1 = (diff_1 + (glstate_lightmodel_ambient * 2.0).xyz);
					  retColor_4.xyz = (diff_1 * retColor_4.xyz);
					  mediump vec3 P_12;
					  P_12 = (xlv_TEXCOORD2.xyz + bumpNormal_3);
					  retColor_4.xyz = (retColor_4.xyz + ((textureCube (_V_CW_Cube, P_12) * _V_CW_ReflectColor).xyz * clamp (
					    (retColor_4.w + _V_CW_ReflectStrengthAlphaOffset)
					  , 0.0, 1.0)));
					  retColor_4.w = 1.0;
					  gl_FragData[0] = retColor_4;
					}
					
					
					#endif"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "VERTEXLIGHT_ON" "V_CW_VERTEX_COLOR" "_NORMALMAP" "_EMISSION_OFF" "V_CW_RIM_OFF" "V_CW_SPECULAR_OFF" "V_CW_REFLECTIVE" "V_CW_FOG" "V_CW_USE_LIGHT_RAMP_TEXTURE_OFF" }
					"!!GLES
					#version 100
					
					#ifdef VERTEX
					attribute vec4 _glesTANGENT;
					attribute vec4 _glesVertex;
					attribute vec4 _glesColor;
					attribute vec3 _glesNormal;
					attribute vec4 _glesMultiTexCoord0;
					uniform highp vec4 _Time;
					uniform highp vec3 _WorldSpaceCameraPos;
					uniform highp vec4 _ProjectionParams;
					uniform mediump vec4 _WorldSpaceLightPos0;
					uniform highp mat4 glstate_matrix_mvp;
					uniform highp mat4 _Object2World;
					uniform highp mat4 _World2Object;
					uniform highp vec3 _V_CW_Bend;
					uniform highp vec3 _V_CW_Bias;
					uniform highp vec4 _V_CW_PivotPoint_Position;
					uniform highp vec4 _MainTex_ST;
					uniform lowp vec2 _V_CW_MainTex_Scroll;
					varying mediump vec4 xlv_TEXCOORD0;
					varying mediump vec4 xlv_TEXCOORD1;
					varying mediump vec4 xlv_TEXCOORD2;
					varying highp vec4 xlv_COLOR0;
					varying mediump vec4 xlv_TEXCOORD4;
					varying mediump vec3 xlv_TEXCOORD6;
					varying mediump vec4 xlv_TEXCOORD7;
					void main ()
					{
					  lowp vec4 tmpvar_1;
					  tmpvar_1 = _glesColor;
					  highp vec4 tmpvar_2;
					  mediump vec4 tmpvar_3;
					  mediump vec4 tmpvar_4;
					  mediump vec4 tmpvar_5;
					  highp vec4 tmpvar_6;
					  mediump vec3 tmpvar_7;
					  mediump vec4 tmpvar_8;
					  highp vec4 v_9;
					  highp float tmpvar_10;
					  tmpvar_10 = _World2Object[0].x;
					  v_9.x = tmpvar_10;
					  highp float tmpvar_11;
					  tmpvar_11 = _World2Object[1].x;
					  v_9.y = tmpvar_11;
					  highp float tmpvar_12;
					  tmpvar_12 = _World2Object[2].x;
					  v_9.z = tmpvar_12;
					  highp float tmpvar_13;
					  tmpvar_13 = _World2Object[3].x;
					  v_9.w = tmpvar_13;
					  highp vec4 v_14;
					  highp float tmpvar_15;
					  tmpvar_15 = _World2Object[0].y;
					  v_14.x = tmpvar_15;
					  highp float tmpvar_16;
					  tmpvar_16 = _World2Object[1].y;
					  v_14.y = tmpvar_16;
					  highp float tmpvar_17;
					  tmpvar_17 = _World2Object[2].y;
					  v_14.z = tmpvar_17;
					  highp float tmpvar_18;
					  tmpvar_18 = _World2Object[3].y;
					  v_14.w = tmpvar_18;
					  highp vec4 v_19;
					  highp float tmpvar_20;
					  tmpvar_20 = _World2Object[0].z;
					  v_19.x = tmpvar_20;
					  highp float tmpvar_21;
					  tmpvar_21 = _World2Object[1].z;
					  v_19.y = tmpvar_21;
					  highp float tmpvar_22;
					  tmpvar_22 = _World2Object[2].z;
					  v_19.z = tmpvar_22;
					  highp float tmpvar_23;
					  tmpvar_23 = _World2Object[3].z;
					  v_19.w = tmpvar_23;
					  highp vec3 tmpvar_24;
					  tmpvar_24 = normalize(((
					    (v_9.xyz * _glesNormal.x)
					   + 
					    (v_14.xyz * _glesNormal.y)
					  ) + (v_19.xyz * _glesNormal.z)));
					  highp mat3 tmpvar_25;
					  tmpvar_25[0] = _Object2World[0].xyz;
					  tmpvar_25[1] = _Object2World[1].xyz;
					  tmpvar_25[2] = _Object2World[2].xyz;
					  highp vec3 tmpvar_26;
					  tmpvar_26 = normalize((tmpvar_25 * _glesTANGENT.xyz));
					  highp vec4 vertex_27;
					  vertex_27.w = _glesVertex.w;
					  highp vec2 xzOff_28;
					  highp vec3 v2_29;
					  highp vec3 v1_30;
					  highp vec3 v0_31;
					  highp vec3 tmpvar_32;
					  tmpvar_32 = ((_Object2World * _glesVertex).xyz - _V_CW_PivotPoint_Position.xyz);
					  highp vec3 tmpvar_33;
					  tmpvar_33 = (tmpvar_32 + tmpvar_26);
					  v1_30.xz = tmpvar_33.xz;
					  highp vec3 tmpvar_34;
					  tmpvar_34 = (tmpvar_32 - ((tmpvar_24.yzx * tmpvar_26.zxy) - (tmpvar_24.zxy * tmpvar_26.yzx)));
					  v2_29.xz = tmpvar_34.xz;
					  highp vec2 tmpvar_35;
					  tmpvar_35.x = float((tmpvar_32.z >= 0.0));
					  tmpvar_35.y = float((tmpvar_32.x >= 0.0));
					  xzOff_28 = (max (vec2(0.0, 0.0), (
					    abs(tmpvar_32.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_35 * 2.0) - 1.0));
					  xzOff_28 = (xzOff_28 * xzOff_28);
					  highp vec3 tmpvar_36;
					  tmpvar_36.xz = vec2(0.0, 0.0);
					  tmpvar_36.y = (((_V_CW_Bend.x * xzOff_28.x) + (_V_CW_Bend.z * xzOff_28.y)) * 0.001);
					  v0_31 = (tmpvar_32 + tmpvar_36);
					  highp vec2 tmpvar_37;
					  tmpvar_37.x = float((tmpvar_33.z >= 0.0));
					  tmpvar_37.y = float((tmpvar_33.x >= 0.0));
					  xzOff_28 = (max (vec2(0.0, 0.0), (
					    abs(tmpvar_33.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_37 * 2.0) - 1.0));
					  xzOff_28 = (xzOff_28 * xzOff_28);
					  v1_30.y = (tmpvar_33.y + ((
					    (_V_CW_Bend.x * xzOff_28.x)
					   + 
					    (_V_CW_Bend.z * xzOff_28.y)
					  ) * 0.001));
					  highp vec2 tmpvar_38;
					  tmpvar_38.x = float((tmpvar_34.z >= 0.0));
					  tmpvar_38.y = float((tmpvar_34.x >= 0.0));
					  xzOff_28 = (max (vec2(0.0, 0.0), (
					    abs(tmpvar_34.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_38 * 2.0) - 1.0));
					  xzOff_28 = (xzOff_28 * xzOff_28);
					  v2_29.y = (tmpvar_34.y + ((
					    (_V_CW_Bend.x * xzOff_28.x)
					   + 
					    (_V_CW_Bend.z * xzOff_28.y)
					  ) * 0.001));
					  highp mat3 tmpvar_39;
					  tmpvar_39[0] = _World2Object[0].xyz;
					  tmpvar_39[1] = _World2Object[1].xyz;
					  tmpvar_39[2] = _World2Object[2].xyz;
					  vertex_27.xyz = (_glesVertex.xyz + (tmpvar_39 * tmpvar_36));
					  highp mat3 tmpvar_40;
					  tmpvar_40[0] = _World2Object[0].xyz;
					  tmpvar_40[1] = _World2Object[1].xyz;
					  tmpvar_40[2] = _World2Object[2].xyz;
					  highp vec3 a_41;
					  a_41 = (v2_29 - v0_31);
					  highp vec3 b_42;
					  b_42 = (v1_30 - v0_31);
					  highp vec3 tmpvar_43;
					  tmpvar_43 = normalize((tmpvar_40 * normalize(
					    ((a_41.yzx * b_42.zxy) - (a_41.zxy * b_42.yzx))
					  )));
					  tmpvar_2 = (glstate_matrix_mvp * vertex_27);
					  tmpvar_3.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
					  tmpvar_3.xy = (tmpvar_3.xy + (_V_CW_MainTex_Scroll * _Time.x));
					  tmpvar_6 = tmpvar_1;
					  highp vec4 v_44;
					  v_44.x = tmpvar_10;
					  v_44.y = tmpvar_11;
					  v_44.z = tmpvar_12;
					  v_44.w = tmpvar_13;
					  highp vec4 v_45;
					  v_45.x = tmpvar_15;
					  v_45.y = tmpvar_16;
					  v_45.z = tmpvar_17;
					  v_45.w = tmpvar_18;
					  highp vec4 v_46;
					  v_46.x = tmpvar_20;
					  v_46.y = tmpvar_21;
					  v_46.z = tmpvar_22;
					  v_46.w = tmpvar_23;
					  highp vec3 tmpvar_47;
					  tmpvar_47 = normalize(((
					    (v_44.xyz * tmpvar_43.x)
					   + 
					    (v_45.xyz * tmpvar_43.y)
					  ) + (v_46.xyz * tmpvar_43.z)));
					  highp vec3 tmpvar_48;
					  tmpvar_48 = normalize(tmpvar_43);
					  highp vec3 tmpvar_49;
					  tmpvar_49 = normalize(_glesTANGENT.xyz);
					  highp vec3 tmpvar_50;
					  highp vec3 tmpvar_51;
					  tmpvar_50 = _glesTANGENT.xyz;
					  tmpvar_51 = (((tmpvar_48.yzx * tmpvar_49.zxy) - (tmpvar_48.zxy * tmpvar_49.yzx)) * _glesTANGENT.w);
					  highp mat3 tmpvar_52;
					  tmpvar_52[0].x = tmpvar_50.x;
					  tmpvar_52[0].y = tmpvar_51.x;
					  tmpvar_52[0].z = tmpvar_43.x;
					  tmpvar_52[1].x = tmpvar_50.y;
					  tmpvar_52[1].y = tmpvar_51.y;
					  tmpvar_52[1].z = tmpvar_43.y;
					  tmpvar_52[2].x = tmpvar_50.z;
					  tmpvar_52[2].y = tmpvar_51.z;
					  tmpvar_52[2].z = tmpvar_43.z;
					  highp vec3 tmpvar_53;
					  tmpvar_53 = normalize((tmpvar_52 * (_World2Object * _WorldSpaceLightPos0).xyz));
					  tmpvar_7 = tmpvar_53;
					  tmpvar_4.xyz = tmpvar_47;
					  highp vec3 I_54;
					  I_54 = ((_Object2World * vertex_27).xyz - _WorldSpaceCameraPos);
					  highp vec3 tmpvar_55;
					  tmpvar_55 = normalize((I_54 - (2.0 * 
					    (dot (tmpvar_47, I_54) * tmpvar_47)
					  )));
					  tmpvar_5.xyz = tmpvar_55;
					  highp vec4 o_56;
					  highp vec4 tmpvar_57;
					  tmpvar_57 = (tmpvar_2 * 0.5);
					  highp vec2 tmpvar_58;
					  tmpvar_58.x = tmpvar_57.x;
					  tmpvar_58.y = (tmpvar_57.y * _ProjectionParams.x);
					  o_56.xy = (tmpvar_58 + tmpvar_57.w);
					  o_56.zw = tmpvar_2.zw;
					  tmpvar_8 = o_56;
					  gl_Position = tmpvar_2;
					  xlv_TEXCOORD0 = tmpvar_3;
					  xlv_TEXCOORD1 = tmpvar_4;
					  xlv_TEXCOORD2 = tmpvar_5;
					  xlv_COLOR0 = tmpvar_6;
					  xlv_TEXCOORD4 = vec4(0.0, 0.0, 0.0, 0.0);
					  xlv_TEXCOORD6 = tmpvar_7;
					  xlv_TEXCOORD7 = tmpvar_8;
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform lowp vec4 glstate_lightmodel_ambient;
					uniform lowp vec4 _LightColor0;
					uniform sampler2D _ShadowMapTexture;
					uniform lowp vec4 _Color;
					uniform sampler2D _MainTex;
					uniform sampler2D _V_CW_NormalMap;
					uniform mediump float _V_CW_NormalMap_UV_Scale;
					uniform mediump float _V_CW_NormalMapStrength;
					uniform lowp samplerCube _V_CW_Cube;
					uniform lowp vec4 _V_CW_ReflectColor;
					uniform lowp float _V_CW_ReflectStrengthAlphaOffset;
					varying mediump vec4 xlv_TEXCOORD0;
					varying mediump vec4 xlv_TEXCOORD2;
					varying highp vec4 xlv_COLOR0;
					varying mediump vec3 xlv_TEXCOORD6;
					varying mediump vec4 xlv_TEXCOORD7;
					void main ()
					{
					  lowp vec3 diff_1;
					  mediump vec3 normal_2;
					  lowp vec3 bumpNormal_3;
					  lowp vec4 retColor_4;
					  mediump vec4 mainTex_5;
					  lowp vec4 tmpvar_6;
					  tmpvar_6 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
					  mainTex_5 = tmpvar_6;
					  retColor_4 = mainTex_5;
					  retColor_4 = (retColor_4 * _Color);
					  retColor_4 = (retColor_4 * xlv_COLOR0);
					  mediump vec2 P_7;
					  P_7 = (xlv_TEXCOORD0.xy * _V_CW_NormalMap_UV_Scale);
					  lowp vec3 normal_8;
					  normal_8.xy = ((texture2D (_V_CW_NormalMap, P_7).wy * 2.0) - 1.0);
					  normal_8.z = sqrt((1.0 - clamp (
					    dot (normal_8.xy, normal_8.xy)
					  , 0.0, 1.0)));
					  mediump vec3 tmpvar_9;
					  tmpvar_9.xy = (normal_8.xy * vec2(_V_CW_NormalMapStrength));
					  tmpvar_9.z = normal_8.z;
					  mediump vec3 tmpvar_10;
					  tmpvar_10 = normalize(tmpvar_9);
					  bumpNormal_3 = tmpvar_10;
					  normal_2 = bumpNormal_3;
					  mediump float tmpvar_11;
					  tmpvar_11 = max (0.0, dot (normal_2, xlv_TEXCOORD6));
					  diff_1 = ((_LightColor0.xyz * texture2DProj (_ShadowMapTexture, xlv_TEXCOORD7).x) * tmpvar_11);
					  diff_1 = (diff_1 + (glstate_lightmodel_ambient * 2.0).xyz);
					  retColor_4.xyz = (diff_1 * retColor_4.xyz);
					  mediump vec3 P_12;
					  P_12 = (xlv_TEXCOORD2.xyz + bumpNormal_3);
					  retColor_4.xyz = (retColor_4.xyz + ((textureCube (_V_CW_Cube, P_12) * _V_CW_ReflectColor).xyz * clamp (
					    (retColor_4.w + _V_CW_ReflectStrengthAlphaOffset)
					  , 0.0, 1.0)));
					  retColor_4.w = 1.0;
					  gl_FragData[0] = retColor_4;
					}
					
					
					#endif"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_OFF" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "FOG_LINEAR" "V_CW_VERTEX_COLOR" "_NORMALMAP" "_EMISSION_OFF" "V_CW_RIM_OFF" "V_CW_SPECULAR_OFF" "V_CW_REFLECTIVE" "V_CW_FOG" "V_CW_USE_LIGHT_RAMP_TEXTURE_OFF" }
					"!!GLES
					#version 100
					
					#ifdef VERTEX
					attribute vec4 _glesTANGENT;
					attribute vec4 _glesVertex;
					attribute vec4 _glesColor;
					attribute vec3 _glesNormal;
					attribute vec4 _glesMultiTexCoord0;
					uniform highp vec4 _Time;
					uniform highp vec3 _WorldSpaceCameraPos;
					uniform mediump vec4 _WorldSpaceLightPos0;
					uniform highp mat4 glstate_matrix_mvp;
					uniform highp mat4 _Object2World;
					uniform highp mat4 _World2Object;
					uniform highp vec4 unity_FogParams;
					uniform highp vec3 _V_CW_Bend;
					uniform highp vec3 _V_CW_Bias;
					uniform highp vec4 _V_CW_PivotPoint_Position;
					uniform highp vec4 _MainTex_ST;
					uniform lowp vec2 _V_CW_MainTex_Scroll;
					varying mediump vec4 xlv_TEXCOORD0;
					varying mediump vec4 xlv_TEXCOORD1;
					varying mediump vec4 xlv_TEXCOORD2;
					varying highp vec4 xlv_COLOR0;
					varying highp float xlv_TEXCOORD3;
					varying mediump vec4 xlv_TEXCOORD4;
					varying mediump vec3 xlv_TEXCOORD6;
					void main ()
					{
					  lowp vec4 tmpvar_1;
					  tmpvar_1 = _glesColor;
					  highp vec4 tmpvar_2;
					  mediump vec4 tmpvar_3;
					  mediump vec4 tmpvar_4;
					  mediump vec4 tmpvar_5;
					  highp vec4 tmpvar_6;
					  mediump vec3 tmpvar_7;
					  highp vec4 v_8;
					  highp float tmpvar_9;
					  tmpvar_9 = _World2Object[0].x;
					  v_8.x = tmpvar_9;
					  highp float tmpvar_10;
					  tmpvar_10 = _World2Object[1].x;
					  v_8.y = tmpvar_10;
					  highp float tmpvar_11;
					  tmpvar_11 = _World2Object[2].x;
					  v_8.z = tmpvar_11;
					  highp float tmpvar_12;
					  tmpvar_12 = _World2Object[3].x;
					  v_8.w = tmpvar_12;
					  highp vec4 v_13;
					  highp float tmpvar_14;
					  tmpvar_14 = _World2Object[0].y;
					  v_13.x = tmpvar_14;
					  highp float tmpvar_15;
					  tmpvar_15 = _World2Object[1].y;
					  v_13.y = tmpvar_15;
					  highp float tmpvar_16;
					  tmpvar_16 = _World2Object[2].y;
					  v_13.z = tmpvar_16;
					  highp float tmpvar_17;
					  tmpvar_17 = _World2Object[3].y;
					  v_13.w = tmpvar_17;
					  highp vec4 v_18;
					  highp float tmpvar_19;
					  tmpvar_19 = _World2Object[0].z;
					  v_18.x = tmpvar_19;
					  highp float tmpvar_20;
					  tmpvar_20 = _World2Object[1].z;
					  v_18.y = tmpvar_20;
					  highp float tmpvar_21;
					  tmpvar_21 = _World2Object[2].z;
					  v_18.z = tmpvar_21;
					  highp float tmpvar_22;
					  tmpvar_22 = _World2Object[3].z;
					  v_18.w = tmpvar_22;
					  highp vec3 tmpvar_23;
					  tmpvar_23 = normalize(((
					    (v_8.xyz * _glesNormal.x)
					   + 
					    (v_13.xyz * _glesNormal.y)
					  ) + (v_18.xyz * _glesNormal.z)));
					  highp mat3 tmpvar_24;
					  tmpvar_24[0] = _Object2World[0].xyz;
					  tmpvar_24[1] = _Object2World[1].xyz;
					  tmpvar_24[2] = _Object2World[2].xyz;
					  highp vec3 tmpvar_25;
					  tmpvar_25 = normalize((tmpvar_24 * _glesTANGENT.xyz));
					  highp vec4 vertex_26;
					  vertex_26.w = _glesVertex.w;
					  highp vec2 xzOff_27;
					  highp vec3 v2_28;
					  highp vec3 v1_29;
					  highp vec3 v0_30;
					  highp vec3 tmpvar_31;
					  tmpvar_31 = ((_Object2World * _glesVertex).xyz - _V_CW_PivotPoint_Position.xyz);
					  highp vec3 tmpvar_32;
					  tmpvar_32 = (tmpvar_31 + tmpvar_25);
					  v1_29.xz = tmpvar_32.xz;
					  highp vec3 tmpvar_33;
					  tmpvar_33 = (tmpvar_31 - ((tmpvar_23.yzx * tmpvar_25.zxy) - (tmpvar_23.zxy * tmpvar_25.yzx)));
					  v2_28.xz = tmpvar_33.xz;
					  highp vec2 tmpvar_34;
					  tmpvar_34.x = float((tmpvar_31.z >= 0.0));
					  tmpvar_34.y = float((tmpvar_31.x >= 0.0));
					  xzOff_27 = (max (vec2(0.0, 0.0), (
					    abs(tmpvar_31.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_34 * 2.0) - 1.0));
					  xzOff_27 = (xzOff_27 * xzOff_27);
					  highp vec3 tmpvar_35;
					  tmpvar_35.xz = vec2(0.0, 0.0);
					  tmpvar_35.y = (((_V_CW_Bend.x * xzOff_27.x) + (_V_CW_Bend.z * xzOff_27.y)) * 0.001);
					  v0_30 = (tmpvar_31 + tmpvar_35);
					  highp vec2 tmpvar_36;
					  tmpvar_36.x = float((tmpvar_32.z >= 0.0));
					  tmpvar_36.y = float((tmpvar_32.x >= 0.0));
					  xzOff_27 = (max (vec2(0.0, 0.0), (
					    abs(tmpvar_32.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_36 * 2.0) - 1.0));
					  xzOff_27 = (xzOff_27 * xzOff_27);
					  v1_29.y = (tmpvar_32.y + ((
					    (_V_CW_Bend.x * xzOff_27.x)
					   + 
					    (_V_CW_Bend.z * xzOff_27.y)
					  ) * 0.001));
					  highp vec2 tmpvar_37;
					  tmpvar_37.x = float((tmpvar_33.z >= 0.0));
					  tmpvar_37.y = float((tmpvar_33.x >= 0.0));
					  xzOff_27 = (max (vec2(0.0, 0.0), (
					    abs(tmpvar_33.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_37 * 2.0) - 1.0));
					  xzOff_27 = (xzOff_27 * xzOff_27);
					  v2_28.y = (tmpvar_33.y + ((
					    (_V_CW_Bend.x * xzOff_27.x)
					   + 
					    (_V_CW_Bend.z * xzOff_27.y)
					  ) * 0.001));
					  highp mat3 tmpvar_38;
					  tmpvar_38[0] = _World2Object[0].xyz;
					  tmpvar_38[1] = _World2Object[1].xyz;
					  tmpvar_38[2] = _World2Object[2].xyz;
					  vertex_26.xyz = (_glesVertex.xyz + (tmpvar_38 * tmpvar_35));
					  highp mat3 tmpvar_39;
					  tmpvar_39[0] = _World2Object[0].xyz;
					  tmpvar_39[1] = _World2Object[1].xyz;
					  tmpvar_39[2] = _World2Object[2].xyz;
					  highp vec3 a_40;
					  a_40 = (v2_28 - v0_30);
					  highp vec3 b_41;
					  b_41 = (v1_29 - v0_30);
					  highp vec3 tmpvar_42;
					  tmpvar_42 = normalize((tmpvar_39 * normalize(
					    ((a_40.yzx * b_41.zxy) - (a_40.zxy * b_41.yzx))
					  )));
					  tmpvar_2 = (glstate_matrix_mvp * vertex_26);
					  tmpvar_3.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
					  tmpvar_3.xy = (tmpvar_3.xy + (_V_CW_MainTex_Scroll * _Time.x));
					  tmpvar_6 = tmpvar_1;
					  highp vec4 v_43;
					  v_43.x = tmpvar_9;
					  v_43.y = tmpvar_10;
					  v_43.z = tmpvar_11;
					  v_43.w = tmpvar_12;
					  highp vec4 v_44;
					  v_44.x = tmpvar_14;
					  v_44.y = tmpvar_15;
					  v_44.z = tmpvar_16;
					  v_44.w = tmpvar_17;
					  highp vec4 v_45;
					  v_45.x = tmpvar_19;
					  v_45.y = tmpvar_20;
					  v_45.z = tmpvar_21;
					  v_45.w = tmpvar_22;
					  highp vec3 tmpvar_46;
					  tmpvar_46 = normalize(((
					    (v_43.xyz * tmpvar_42.x)
					   + 
					    (v_44.xyz * tmpvar_42.y)
					  ) + (v_45.xyz * tmpvar_42.z)));
					  highp vec3 tmpvar_47;
					  tmpvar_47 = normalize(tmpvar_42);
					  highp vec3 tmpvar_48;
					  tmpvar_48 = normalize(_glesTANGENT.xyz);
					  highp vec3 tmpvar_49;
					  highp vec3 tmpvar_50;
					  tmpvar_49 = _glesTANGENT.xyz;
					  tmpvar_50 = (((tmpvar_47.yzx * tmpvar_48.zxy) - (tmpvar_47.zxy * tmpvar_48.yzx)) * _glesTANGENT.w);
					  highp mat3 tmpvar_51;
					  tmpvar_51[0].x = tmpvar_49.x;
					  tmpvar_51[0].y = tmpvar_50.x;
					  tmpvar_51[0].z = tmpvar_42.x;
					  tmpvar_51[1].x = tmpvar_49.y;
					  tmpvar_51[1].y = tmpvar_50.y;
					  tmpvar_51[1].z = tmpvar_42.y;
					  tmpvar_51[2].x = tmpvar_49.z;
					  tmpvar_51[2].y = tmpvar_50.z;
					  tmpvar_51[2].z = tmpvar_42.z;
					  highp vec3 tmpvar_52;
					  tmpvar_52 = normalize((tmpvar_51 * (_World2Object * _WorldSpaceLightPos0).xyz));
					  tmpvar_7 = tmpvar_52;
					  tmpvar_4.xyz = tmpvar_46;
					  highp vec3 I_53;
					  I_53 = ((_Object2World * vertex_26).xyz - _WorldSpaceCameraPos);
					  highp vec3 tmpvar_54;
					  tmpvar_54 = normalize((I_53 - (2.0 * 
					    (dot (tmpvar_46, I_53) * tmpvar_46)
					  )));
					  tmpvar_5.xyz = tmpvar_54;
					  gl_Position = tmpvar_2;
					  xlv_TEXCOORD0 = tmpvar_3;
					  xlv_TEXCOORD1 = tmpvar_4;
					  xlv_TEXCOORD2 = tmpvar_5;
					  xlv_COLOR0 = tmpvar_6;
					  xlv_TEXCOORD3 = ((tmpvar_2.z * unity_FogParams.z) + unity_FogParams.w);
					  xlv_TEXCOORD4 = vec4(0.0, 0.0, 0.0, 0.0);
					  xlv_TEXCOORD6 = tmpvar_7;
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform lowp vec4 glstate_lightmodel_ambient;
					uniform lowp vec4 unity_FogColor;
					uniform lowp vec4 _LightColor0;
					uniform lowp vec4 _Color;
					uniform sampler2D _MainTex;
					uniform sampler2D _V_CW_NormalMap;
					uniform mediump float _V_CW_NormalMap_UV_Scale;
					uniform mediump float _V_CW_NormalMapStrength;
					uniform lowp samplerCube _V_CW_Cube;
					uniform lowp vec4 _V_CW_ReflectColor;
					uniform lowp float _V_CW_ReflectStrengthAlphaOffset;
					varying mediump vec4 xlv_TEXCOORD0;
					varying mediump vec4 xlv_TEXCOORD2;
					varying highp vec4 xlv_COLOR0;
					varying highp float xlv_TEXCOORD3;
					varying mediump vec3 xlv_TEXCOORD6;
					void main ()
					{
					  lowp vec3 diff_1;
					  mediump vec3 normal_2;
					  lowp vec3 bumpNormal_3;
					  lowp vec4 retColor_4;
					  mediump vec4 mainTex_5;
					  lowp vec4 tmpvar_6;
					  tmpvar_6 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
					  mainTex_5 = tmpvar_6;
					  retColor_4 = mainTex_5;
					  retColor_4 = (retColor_4 * _Color);
					  retColor_4 = (retColor_4 * xlv_COLOR0);
					  mediump vec2 P_7;
					  P_7 = (xlv_TEXCOORD0.xy * _V_CW_NormalMap_UV_Scale);
					  lowp vec3 normal_8;
					  normal_8.xy = ((texture2D (_V_CW_NormalMap, P_7).wy * 2.0) - 1.0);
					  normal_8.z = sqrt((1.0 - clamp (
					    dot (normal_8.xy, normal_8.xy)
					  , 0.0, 1.0)));
					  mediump vec3 tmpvar_9;
					  tmpvar_9.xy = (normal_8.xy * vec2(_V_CW_NormalMapStrength));
					  tmpvar_9.z = normal_8.z;
					  mediump vec3 tmpvar_10;
					  tmpvar_10 = normalize(tmpvar_9);
					  bumpNormal_3 = tmpvar_10;
					  normal_2 = bumpNormal_3;
					  mediump float tmpvar_11;
					  tmpvar_11 = max (0.0, dot (normal_2, xlv_TEXCOORD6));
					  diff_1 = (_LightColor0.xyz * tmpvar_11);
					  diff_1 = (diff_1 + (glstate_lightmodel_ambient * 2.0).xyz);
					  retColor_4.xyz = (diff_1 * retColor_4.xyz);
					  mediump vec3 P_12;
					  P_12 = (xlv_TEXCOORD2.xyz + bumpNormal_3);
					  retColor_4.xyz = (retColor_4.xyz + ((textureCube (_V_CW_Cube, P_12) * _V_CW_ReflectColor).xyz * clamp (
					    (retColor_4.w + _V_CW_ReflectStrengthAlphaOffset)
					  , 0.0, 1.0)));
					  highp float tmpvar_13;
					  tmpvar_13 = clamp (xlv_TEXCOORD3, 0.0, 1.0);
					  retColor_4.xyz = mix (unity_FogColor.xyz, retColor_4.xyz, vec3(tmpvar_13));
					  retColor_4.w = 1.0;
					  gl_FragData[0] = retColor_4;
					}
					
					
					#endif"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "FOG_LINEAR" "V_CW_VERTEX_COLOR" "_NORMALMAP" "_EMISSION_OFF" "V_CW_RIM_OFF" "V_CW_SPECULAR_OFF" "V_CW_REFLECTIVE" "V_CW_FOG" "V_CW_USE_LIGHT_RAMP_TEXTURE_OFF" }
					"!!GLES
					#version 100
					
					#ifdef VERTEX
					attribute vec4 _glesTANGENT;
					attribute vec4 _glesVertex;
					attribute vec4 _glesColor;
					attribute vec3 _glesNormal;
					attribute vec4 _glesMultiTexCoord0;
					uniform highp vec4 _Time;
					uniform highp vec3 _WorldSpaceCameraPos;
					uniform highp vec4 _ProjectionParams;
					uniform mediump vec4 _WorldSpaceLightPos0;
					uniform highp mat4 glstate_matrix_mvp;
					uniform highp mat4 _Object2World;
					uniform highp mat4 _World2Object;
					uniform highp vec4 unity_FogParams;
					uniform highp vec3 _V_CW_Bend;
					uniform highp vec3 _V_CW_Bias;
					uniform highp vec4 _V_CW_PivotPoint_Position;
					uniform highp vec4 _MainTex_ST;
					uniform lowp vec2 _V_CW_MainTex_Scroll;
					varying mediump vec4 xlv_TEXCOORD0;
					varying mediump vec4 xlv_TEXCOORD1;
					varying mediump vec4 xlv_TEXCOORD2;
					varying highp vec4 xlv_COLOR0;
					varying highp float xlv_TEXCOORD3;
					varying mediump vec4 xlv_TEXCOORD4;
					varying mediump vec3 xlv_TEXCOORD6;
					varying mediump vec4 xlv_TEXCOORD7;
					void main ()
					{
					  lowp vec4 tmpvar_1;
					  tmpvar_1 = _glesColor;
					  highp vec4 tmpvar_2;
					  mediump vec4 tmpvar_3;
					  mediump vec4 tmpvar_4;
					  mediump vec4 tmpvar_5;
					  highp vec4 tmpvar_6;
					  mediump vec3 tmpvar_7;
					  mediump vec4 tmpvar_8;
					  highp vec4 v_9;
					  highp float tmpvar_10;
					  tmpvar_10 = _World2Object[0].x;
					  v_9.x = tmpvar_10;
					  highp float tmpvar_11;
					  tmpvar_11 = _World2Object[1].x;
					  v_9.y = tmpvar_11;
					  highp float tmpvar_12;
					  tmpvar_12 = _World2Object[2].x;
					  v_9.z = tmpvar_12;
					  highp float tmpvar_13;
					  tmpvar_13 = _World2Object[3].x;
					  v_9.w = tmpvar_13;
					  highp vec4 v_14;
					  highp float tmpvar_15;
					  tmpvar_15 = _World2Object[0].y;
					  v_14.x = tmpvar_15;
					  highp float tmpvar_16;
					  tmpvar_16 = _World2Object[1].y;
					  v_14.y = tmpvar_16;
					  highp float tmpvar_17;
					  tmpvar_17 = _World2Object[2].y;
					  v_14.z = tmpvar_17;
					  highp float tmpvar_18;
					  tmpvar_18 = _World2Object[3].y;
					  v_14.w = tmpvar_18;
					  highp vec4 v_19;
					  highp float tmpvar_20;
					  tmpvar_20 = _World2Object[0].z;
					  v_19.x = tmpvar_20;
					  highp float tmpvar_21;
					  tmpvar_21 = _World2Object[1].z;
					  v_19.y = tmpvar_21;
					  highp float tmpvar_22;
					  tmpvar_22 = _World2Object[2].z;
					  v_19.z = tmpvar_22;
					  highp float tmpvar_23;
					  tmpvar_23 = _World2Object[3].z;
					  v_19.w = tmpvar_23;
					  highp vec3 tmpvar_24;
					  tmpvar_24 = normalize(((
					    (v_9.xyz * _glesNormal.x)
					   + 
					    (v_14.xyz * _glesNormal.y)
					  ) + (v_19.xyz * _glesNormal.z)));
					  highp mat3 tmpvar_25;
					  tmpvar_25[0] = _Object2World[0].xyz;
					  tmpvar_25[1] = _Object2World[1].xyz;
					  tmpvar_25[2] = _Object2World[2].xyz;
					  highp vec3 tmpvar_26;
					  tmpvar_26 = normalize((tmpvar_25 * _glesTANGENT.xyz));
					  highp vec4 vertex_27;
					  vertex_27.w = _glesVertex.w;
					  highp vec2 xzOff_28;
					  highp vec3 v2_29;
					  highp vec3 v1_30;
					  highp vec3 v0_31;
					  highp vec3 tmpvar_32;
					  tmpvar_32 = ((_Object2World * _glesVertex).xyz - _V_CW_PivotPoint_Position.xyz);
					  highp vec3 tmpvar_33;
					  tmpvar_33 = (tmpvar_32 + tmpvar_26);
					  v1_30.xz = tmpvar_33.xz;
					  highp vec3 tmpvar_34;
					  tmpvar_34 = (tmpvar_32 - ((tmpvar_24.yzx * tmpvar_26.zxy) - (tmpvar_24.zxy * tmpvar_26.yzx)));
					  v2_29.xz = tmpvar_34.xz;
					  highp vec2 tmpvar_35;
					  tmpvar_35.x = float((tmpvar_32.z >= 0.0));
					  tmpvar_35.y = float((tmpvar_32.x >= 0.0));
					  xzOff_28 = (max (vec2(0.0, 0.0), (
					    abs(tmpvar_32.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_35 * 2.0) - 1.0));
					  xzOff_28 = (xzOff_28 * xzOff_28);
					  highp vec3 tmpvar_36;
					  tmpvar_36.xz = vec2(0.0, 0.0);
					  tmpvar_36.y = (((_V_CW_Bend.x * xzOff_28.x) + (_V_CW_Bend.z * xzOff_28.y)) * 0.001);
					  v0_31 = (tmpvar_32 + tmpvar_36);
					  highp vec2 tmpvar_37;
					  tmpvar_37.x = float((tmpvar_33.z >= 0.0));
					  tmpvar_37.y = float((tmpvar_33.x >= 0.0));
					  xzOff_28 = (max (vec2(0.0, 0.0), (
					    abs(tmpvar_33.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_37 * 2.0) - 1.0));
					  xzOff_28 = (xzOff_28 * xzOff_28);
					  v1_30.y = (tmpvar_33.y + ((
					    (_V_CW_Bend.x * xzOff_28.x)
					   + 
					    (_V_CW_Bend.z * xzOff_28.y)
					  ) * 0.001));
					  highp vec2 tmpvar_38;
					  tmpvar_38.x = float((tmpvar_34.z >= 0.0));
					  tmpvar_38.y = float((tmpvar_34.x >= 0.0));
					  xzOff_28 = (max (vec2(0.0, 0.0), (
					    abs(tmpvar_34.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_38 * 2.0) - 1.0));
					  xzOff_28 = (xzOff_28 * xzOff_28);
					  v2_29.y = (tmpvar_34.y + ((
					    (_V_CW_Bend.x * xzOff_28.x)
					   + 
					    (_V_CW_Bend.z * xzOff_28.y)
					  ) * 0.001));
					  highp mat3 tmpvar_39;
					  tmpvar_39[0] = _World2Object[0].xyz;
					  tmpvar_39[1] = _World2Object[1].xyz;
					  tmpvar_39[2] = _World2Object[2].xyz;
					  vertex_27.xyz = (_glesVertex.xyz + (tmpvar_39 * tmpvar_36));
					  highp mat3 tmpvar_40;
					  tmpvar_40[0] = _World2Object[0].xyz;
					  tmpvar_40[1] = _World2Object[1].xyz;
					  tmpvar_40[2] = _World2Object[2].xyz;
					  highp vec3 a_41;
					  a_41 = (v2_29 - v0_31);
					  highp vec3 b_42;
					  b_42 = (v1_30 - v0_31);
					  highp vec3 tmpvar_43;
					  tmpvar_43 = normalize((tmpvar_40 * normalize(
					    ((a_41.yzx * b_42.zxy) - (a_41.zxy * b_42.yzx))
					  )));
					  tmpvar_2 = (glstate_matrix_mvp * vertex_27);
					  tmpvar_3.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
					  tmpvar_3.xy = (tmpvar_3.xy + (_V_CW_MainTex_Scroll * _Time.x));
					  tmpvar_6 = tmpvar_1;
					  highp vec4 v_44;
					  v_44.x = tmpvar_10;
					  v_44.y = tmpvar_11;
					  v_44.z = tmpvar_12;
					  v_44.w = tmpvar_13;
					  highp vec4 v_45;
					  v_45.x = tmpvar_15;
					  v_45.y = tmpvar_16;
					  v_45.z = tmpvar_17;
					  v_45.w = tmpvar_18;
					  highp vec4 v_46;
					  v_46.x = tmpvar_20;
					  v_46.y = tmpvar_21;
					  v_46.z = tmpvar_22;
					  v_46.w = tmpvar_23;
					  highp vec3 tmpvar_47;
					  tmpvar_47 = normalize(((
					    (v_44.xyz * tmpvar_43.x)
					   + 
					    (v_45.xyz * tmpvar_43.y)
					  ) + (v_46.xyz * tmpvar_43.z)));
					  highp vec3 tmpvar_48;
					  tmpvar_48 = normalize(tmpvar_43);
					  highp vec3 tmpvar_49;
					  tmpvar_49 = normalize(_glesTANGENT.xyz);
					  highp vec3 tmpvar_50;
					  highp vec3 tmpvar_51;
					  tmpvar_50 = _glesTANGENT.xyz;
					  tmpvar_51 = (((tmpvar_48.yzx * tmpvar_49.zxy) - (tmpvar_48.zxy * tmpvar_49.yzx)) * _glesTANGENT.w);
					  highp mat3 tmpvar_52;
					  tmpvar_52[0].x = tmpvar_50.x;
					  tmpvar_52[0].y = tmpvar_51.x;
					  tmpvar_52[0].z = tmpvar_43.x;
					  tmpvar_52[1].x = tmpvar_50.y;
					  tmpvar_52[1].y = tmpvar_51.y;
					  tmpvar_52[1].z = tmpvar_43.y;
					  tmpvar_52[2].x = tmpvar_50.z;
					  tmpvar_52[2].y = tmpvar_51.z;
					  tmpvar_52[2].z = tmpvar_43.z;
					  highp vec3 tmpvar_53;
					  tmpvar_53 = normalize((tmpvar_52 * (_World2Object * _WorldSpaceLightPos0).xyz));
					  tmpvar_7 = tmpvar_53;
					  tmpvar_4.xyz = tmpvar_47;
					  highp vec3 I_54;
					  I_54 = ((_Object2World * vertex_27).xyz - _WorldSpaceCameraPos);
					  highp vec3 tmpvar_55;
					  tmpvar_55 = normalize((I_54 - (2.0 * 
					    (dot (tmpvar_47, I_54) * tmpvar_47)
					  )));
					  tmpvar_5.xyz = tmpvar_55;
					  highp vec4 o_56;
					  highp vec4 tmpvar_57;
					  tmpvar_57 = (tmpvar_2 * 0.5);
					  highp vec2 tmpvar_58;
					  tmpvar_58.x = tmpvar_57.x;
					  tmpvar_58.y = (tmpvar_57.y * _ProjectionParams.x);
					  o_56.xy = (tmpvar_58 + tmpvar_57.w);
					  o_56.zw = tmpvar_2.zw;
					  tmpvar_8 = o_56;
					  gl_Position = tmpvar_2;
					  xlv_TEXCOORD0 = tmpvar_3;
					  xlv_TEXCOORD1 = tmpvar_4;
					  xlv_TEXCOORD2 = tmpvar_5;
					  xlv_COLOR0 = tmpvar_6;
					  xlv_TEXCOORD3 = ((tmpvar_2.z * unity_FogParams.z) + unity_FogParams.w);
					  xlv_TEXCOORD4 = vec4(0.0, 0.0, 0.0, 0.0);
					  xlv_TEXCOORD6 = tmpvar_7;
					  xlv_TEXCOORD7 = tmpvar_8;
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform lowp vec4 glstate_lightmodel_ambient;
					uniform lowp vec4 unity_FogColor;
					uniform lowp vec4 _LightColor0;
					uniform sampler2D _ShadowMapTexture;
					uniform lowp vec4 _Color;
					uniform sampler2D _MainTex;
					uniform sampler2D _V_CW_NormalMap;
					uniform mediump float _V_CW_NormalMap_UV_Scale;
					uniform mediump float _V_CW_NormalMapStrength;
					uniform lowp samplerCube _V_CW_Cube;
					uniform lowp vec4 _V_CW_ReflectColor;
					uniform lowp float _V_CW_ReflectStrengthAlphaOffset;
					varying mediump vec4 xlv_TEXCOORD0;
					varying mediump vec4 xlv_TEXCOORD2;
					varying highp vec4 xlv_COLOR0;
					varying highp float xlv_TEXCOORD3;
					varying mediump vec3 xlv_TEXCOORD6;
					varying mediump vec4 xlv_TEXCOORD7;
					void main ()
					{
					  lowp vec3 diff_1;
					  mediump vec3 normal_2;
					  lowp vec3 bumpNormal_3;
					  lowp vec4 retColor_4;
					  mediump vec4 mainTex_5;
					  lowp vec4 tmpvar_6;
					  tmpvar_6 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
					  mainTex_5 = tmpvar_6;
					  retColor_4 = mainTex_5;
					  retColor_4 = (retColor_4 * _Color);
					  retColor_4 = (retColor_4 * xlv_COLOR0);
					  mediump vec2 P_7;
					  P_7 = (xlv_TEXCOORD0.xy * _V_CW_NormalMap_UV_Scale);
					  lowp vec3 normal_8;
					  normal_8.xy = ((texture2D (_V_CW_NormalMap, P_7).wy * 2.0) - 1.0);
					  normal_8.z = sqrt((1.0 - clamp (
					    dot (normal_8.xy, normal_8.xy)
					  , 0.0, 1.0)));
					  mediump vec3 tmpvar_9;
					  tmpvar_9.xy = (normal_8.xy * vec2(_V_CW_NormalMapStrength));
					  tmpvar_9.z = normal_8.z;
					  mediump vec3 tmpvar_10;
					  tmpvar_10 = normalize(tmpvar_9);
					  bumpNormal_3 = tmpvar_10;
					  normal_2 = bumpNormal_3;
					  mediump float tmpvar_11;
					  tmpvar_11 = max (0.0, dot (normal_2, xlv_TEXCOORD6));
					  diff_1 = ((_LightColor0.xyz * texture2DProj (_ShadowMapTexture, xlv_TEXCOORD7).x) * tmpvar_11);
					  diff_1 = (diff_1 + (glstate_lightmodel_ambient * 2.0).xyz);
					  retColor_4.xyz = (diff_1 * retColor_4.xyz);
					  mediump vec3 P_12;
					  P_12 = (xlv_TEXCOORD2.xyz + bumpNormal_3);
					  retColor_4.xyz = (retColor_4.xyz + ((textureCube (_V_CW_Cube, P_12) * _V_CW_ReflectColor).xyz * clamp (
					    (retColor_4.w + _V_CW_ReflectStrengthAlphaOffset)
					  , 0.0, 1.0)));
					  highp float tmpvar_13;
					  tmpvar_13 = clamp (xlv_TEXCOORD3, 0.0, 1.0);
					  retColor_4.xyz = mix (unity_FogColor.xyz, retColor_4.xyz, vec3(tmpvar_13));
					  retColor_4.w = 1.0;
					  gl_FragData[0] = retColor_4;
					}
					
					
					#endif"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_OFF" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "FOG_LINEAR" "VERTEXLIGHT_ON" "V_CW_VERTEX_COLOR" "_NORMALMAP" "_EMISSION_OFF" "V_CW_RIM_OFF" "V_CW_SPECULAR_OFF" "V_CW_REFLECTIVE" "V_CW_FOG" "V_CW_USE_LIGHT_RAMP_TEXTURE_OFF" }
					"!!GLES
					#version 100
					
					#ifdef VERTEX
					attribute vec4 _glesTANGENT;
					attribute vec4 _glesVertex;
					attribute vec4 _glesColor;
					attribute vec3 _glesNormal;
					attribute vec4 _glesMultiTexCoord0;
					uniform highp vec4 _Time;
					uniform highp vec3 _WorldSpaceCameraPos;
					uniform mediump vec4 _WorldSpaceLightPos0;
					uniform highp mat4 glstate_matrix_mvp;
					uniform highp mat4 _Object2World;
					uniform highp mat4 _World2Object;
					uniform highp vec4 unity_FogParams;
					uniform highp vec3 _V_CW_Bend;
					uniform highp vec3 _V_CW_Bias;
					uniform highp vec4 _V_CW_PivotPoint_Position;
					uniform highp vec4 _MainTex_ST;
					uniform lowp vec2 _V_CW_MainTex_Scroll;
					varying mediump vec4 xlv_TEXCOORD0;
					varying mediump vec4 xlv_TEXCOORD1;
					varying mediump vec4 xlv_TEXCOORD2;
					varying highp vec4 xlv_COLOR0;
					varying highp float xlv_TEXCOORD3;
					varying mediump vec4 xlv_TEXCOORD4;
					varying mediump vec3 xlv_TEXCOORD6;
					void main ()
					{
					  lowp vec4 tmpvar_1;
					  tmpvar_1 = _glesColor;
					  highp vec4 tmpvar_2;
					  mediump vec4 tmpvar_3;
					  mediump vec4 tmpvar_4;
					  mediump vec4 tmpvar_5;
					  highp vec4 tmpvar_6;
					  mediump vec3 tmpvar_7;
					  highp vec4 v_8;
					  highp float tmpvar_9;
					  tmpvar_9 = _World2Object[0].x;
					  v_8.x = tmpvar_9;
					  highp float tmpvar_10;
					  tmpvar_10 = _World2Object[1].x;
					  v_8.y = tmpvar_10;
					  highp float tmpvar_11;
					  tmpvar_11 = _World2Object[2].x;
					  v_8.z = tmpvar_11;
					  highp float tmpvar_12;
					  tmpvar_12 = _World2Object[3].x;
					  v_8.w = tmpvar_12;
					  highp vec4 v_13;
					  highp float tmpvar_14;
					  tmpvar_14 = _World2Object[0].y;
					  v_13.x = tmpvar_14;
					  highp float tmpvar_15;
					  tmpvar_15 = _World2Object[1].y;
					  v_13.y = tmpvar_15;
					  highp float tmpvar_16;
					  tmpvar_16 = _World2Object[2].y;
					  v_13.z = tmpvar_16;
					  highp float tmpvar_17;
					  tmpvar_17 = _World2Object[3].y;
					  v_13.w = tmpvar_17;
					  highp vec4 v_18;
					  highp float tmpvar_19;
					  tmpvar_19 = _World2Object[0].z;
					  v_18.x = tmpvar_19;
					  highp float tmpvar_20;
					  tmpvar_20 = _World2Object[1].z;
					  v_18.y = tmpvar_20;
					  highp float tmpvar_21;
					  tmpvar_21 = _World2Object[2].z;
					  v_18.z = tmpvar_21;
					  highp float tmpvar_22;
					  tmpvar_22 = _World2Object[3].z;
					  v_18.w = tmpvar_22;
					  highp vec3 tmpvar_23;
					  tmpvar_23 = normalize(((
					    (v_8.xyz * _glesNormal.x)
					   + 
					    (v_13.xyz * _glesNormal.y)
					  ) + (v_18.xyz * _glesNormal.z)));
					  highp mat3 tmpvar_24;
					  tmpvar_24[0] = _Object2World[0].xyz;
					  tmpvar_24[1] = _Object2World[1].xyz;
					  tmpvar_24[2] = _Object2World[2].xyz;
					  highp vec3 tmpvar_25;
					  tmpvar_25 = normalize((tmpvar_24 * _glesTANGENT.xyz));
					  highp vec4 vertex_26;
					  vertex_26.w = _glesVertex.w;
					  highp vec2 xzOff_27;
					  highp vec3 v2_28;
					  highp vec3 v1_29;
					  highp vec3 v0_30;
					  highp vec3 tmpvar_31;
					  tmpvar_31 = ((_Object2World * _glesVertex).xyz - _V_CW_PivotPoint_Position.xyz);
					  highp vec3 tmpvar_32;
					  tmpvar_32 = (tmpvar_31 + tmpvar_25);
					  v1_29.xz = tmpvar_32.xz;
					  highp vec3 tmpvar_33;
					  tmpvar_33 = (tmpvar_31 - ((tmpvar_23.yzx * tmpvar_25.zxy) - (tmpvar_23.zxy * tmpvar_25.yzx)));
					  v2_28.xz = tmpvar_33.xz;
					  highp vec2 tmpvar_34;
					  tmpvar_34.x = float((tmpvar_31.z >= 0.0));
					  tmpvar_34.y = float((tmpvar_31.x >= 0.0));
					  xzOff_27 = (max (vec2(0.0, 0.0), (
					    abs(tmpvar_31.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_34 * 2.0) - 1.0));
					  xzOff_27 = (xzOff_27 * xzOff_27);
					  highp vec3 tmpvar_35;
					  tmpvar_35.xz = vec2(0.0, 0.0);
					  tmpvar_35.y = (((_V_CW_Bend.x * xzOff_27.x) + (_V_CW_Bend.z * xzOff_27.y)) * 0.001);
					  v0_30 = (tmpvar_31 + tmpvar_35);
					  highp vec2 tmpvar_36;
					  tmpvar_36.x = float((tmpvar_32.z >= 0.0));
					  tmpvar_36.y = float((tmpvar_32.x >= 0.0));
					  xzOff_27 = (max (vec2(0.0, 0.0), (
					    abs(tmpvar_32.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_36 * 2.0) - 1.0));
					  xzOff_27 = (xzOff_27 * xzOff_27);
					  v1_29.y = (tmpvar_32.y + ((
					    (_V_CW_Bend.x * xzOff_27.x)
					   + 
					    (_V_CW_Bend.z * xzOff_27.y)
					  ) * 0.001));
					  highp vec2 tmpvar_37;
					  tmpvar_37.x = float((tmpvar_33.z >= 0.0));
					  tmpvar_37.y = float((tmpvar_33.x >= 0.0));
					  xzOff_27 = (max (vec2(0.0, 0.0), (
					    abs(tmpvar_33.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_37 * 2.0) - 1.0));
					  xzOff_27 = (xzOff_27 * xzOff_27);
					  v2_28.y = (tmpvar_33.y + ((
					    (_V_CW_Bend.x * xzOff_27.x)
					   + 
					    (_V_CW_Bend.z * xzOff_27.y)
					  ) * 0.001));
					  highp mat3 tmpvar_38;
					  tmpvar_38[0] = _World2Object[0].xyz;
					  tmpvar_38[1] = _World2Object[1].xyz;
					  tmpvar_38[2] = _World2Object[2].xyz;
					  vertex_26.xyz = (_glesVertex.xyz + (tmpvar_38 * tmpvar_35));
					  highp mat3 tmpvar_39;
					  tmpvar_39[0] = _World2Object[0].xyz;
					  tmpvar_39[1] = _World2Object[1].xyz;
					  tmpvar_39[2] = _World2Object[2].xyz;
					  highp vec3 a_40;
					  a_40 = (v2_28 - v0_30);
					  highp vec3 b_41;
					  b_41 = (v1_29 - v0_30);
					  highp vec3 tmpvar_42;
					  tmpvar_42 = normalize((tmpvar_39 * normalize(
					    ((a_40.yzx * b_41.zxy) - (a_40.zxy * b_41.yzx))
					  )));
					  tmpvar_2 = (glstate_matrix_mvp * vertex_26);
					  tmpvar_3.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
					  tmpvar_3.xy = (tmpvar_3.xy + (_V_CW_MainTex_Scroll * _Time.x));
					  tmpvar_6 = tmpvar_1;
					  highp vec4 v_43;
					  v_43.x = tmpvar_9;
					  v_43.y = tmpvar_10;
					  v_43.z = tmpvar_11;
					  v_43.w = tmpvar_12;
					  highp vec4 v_44;
					  v_44.x = tmpvar_14;
					  v_44.y = tmpvar_15;
					  v_44.z = tmpvar_16;
					  v_44.w = tmpvar_17;
					  highp vec4 v_45;
					  v_45.x = tmpvar_19;
					  v_45.y = tmpvar_20;
					  v_45.z = tmpvar_21;
					  v_45.w = tmpvar_22;
					  highp vec3 tmpvar_46;
					  tmpvar_46 = normalize(((
					    (v_43.xyz * tmpvar_42.x)
					   + 
					    (v_44.xyz * tmpvar_42.y)
					  ) + (v_45.xyz * tmpvar_42.z)));
					  highp vec3 tmpvar_47;
					  tmpvar_47 = normalize(tmpvar_42);
					  highp vec3 tmpvar_48;
					  tmpvar_48 = normalize(_glesTANGENT.xyz);
					  highp vec3 tmpvar_49;
					  highp vec3 tmpvar_50;
					  tmpvar_49 = _glesTANGENT.xyz;
					  tmpvar_50 = (((tmpvar_47.yzx * tmpvar_48.zxy) - (tmpvar_47.zxy * tmpvar_48.yzx)) * _glesTANGENT.w);
					  highp mat3 tmpvar_51;
					  tmpvar_51[0].x = tmpvar_49.x;
					  tmpvar_51[0].y = tmpvar_50.x;
					  tmpvar_51[0].z = tmpvar_42.x;
					  tmpvar_51[1].x = tmpvar_49.y;
					  tmpvar_51[1].y = tmpvar_50.y;
					  tmpvar_51[1].z = tmpvar_42.y;
					  tmpvar_51[2].x = tmpvar_49.z;
					  tmpvar_51[2].y = tmpvar_50.z;
					  tmpvar_51[2].z = tmpvar_42.z;
					  highp vec3 tmpvar_52;
					  tmpvar_52 = normalize((tmpvar_51 * (_World2Object * _WorldSpaceLightPos0).xyz));
					  tmpvar_7 = tmpvar_52;
					  tmpvar_4.xyz = tmpvar_46;
					  highp vec3 I_53;
					  I_53 = ((_Object2World * vertex_26).xyz - _WorldSpaceCameraPos);
					  highp vec3 tmpvar_54;
					  tmpvar_54 = normalize((I_53 - (2.0 * 
					    (dot (tmpvar_46, I_53) * tmpvar_46)
					  )));
					  tmpvar_5.xyz = tmpvar_54;
					  gl_Position = tmpvar_2;
					  xlv_TEXCOORD0 = tmpvar_3;
					  xlv_TEXCOORD1 = tmpvar_4;
					  xlv_TEXCOORD2 = tmpvar_5;
					  xlv_COLOR0 = tmpvar_6;
					  xlv_TEXCOORD3 = ((tmpvar_2.z * unity_FogParams.z) + unity_FogParams.w);
					  xlv_TEXCOORD4 = vec4(0.0, 0.0, 0.0, 0.0);
					  xlv_TEXCOORD6 = tmpvar_7;
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform lowp vec4 glstate_lightmodel_ambient;
					uniform lowp vec4 unity_FogColor;
					uniform lowp vec4 _LightColor0;
					uniform lowp vec4 _Color;
					uniform sampler2D _MainTex;
					uniform sampler2D _V_CW_NormalMap;
					uniform mediump float _V_CW_NormalMap_UV_Scale;
					uniform mediump float _V_CW_NormalMapStrength;
					uniform lowp samplerCube _V_CW_Cube;
					uniform lowp vec4 _V_CW_ReflectColor;
					uniform lowp float _V_CW_ReflectStrengthAlphaOffset;
					varying mediump vec4 xlv_TEXCOORD0;
					varying mediump vec4 xlv_TEXCOORD2;
					varying highp vec4 xlv_COLOR0;
					varying highp float xlv_TEXCOORD3;
					varying mediump vec3 xlv_TEXCOORD6;
					void main ()
					{
					  lowp vec3 diff_1;
					  mediump vec3 normal_2;
					  lowp vec3 bumpNormal_3;
					  lowp vec4 retColor_4;
					  mediump vec4 mainTex_5;
					  lowp vec4 tmpvar_6;
					  tmpvar_6 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
					  mainTex_5 = tmpvar_6;
					  retColor_4 = mainTex_5;
					  retColor_4 = (retColor_4 * _Color);
					  retColor_4 = (retColor_4 * xlv_COLOR0);
					  mediump vec2 P_7;
					  P_7 = (xlv_TEXCOORD0.xy * _V_CW_NormalMap_UV_Scale);
					  lowp vec3 normal_8;
					  normal_8.xy = ((texture2D (_V_CW_NormalMap, P_7).wy * 2.0) - 1.0);
					  normal_8.z = sqrt((1.0 - clamp (
					    dot (normal_8.xy, normal_8.xy)
					  , 0.0, 1.0)));
					  mediump vec3 tmpvar_9;
					  tmpvar_9.xy = (normal_8.xy * vec2(_V_CW_NormalMapStrength));
					  tmpvar_9.z = normal_8.z;
					  mediump vec3 tmpvar_10;
					  tmpvar_10 = normalize(tmpvar_9);
					  bumpNormal_3 = tmpvar_10;
					  normal_2 = bumpNormal_3;
					  mediump float tmpvar_11;
					  tmpvar_11 = max (0.0, dot (normal_2, xlv_TEXCOORD6));
					  diff_1 = (_LightColor0.xyz * tmpvar_11);
					  diff_1 = (diff_1 + (glstate_lightmodel_ambient * 2.0).xyz);
					  retColor_4.xyz = (diff_1 * retColor_4.xyz);
					  mediump vec3 P_12;
					  P_12 = (xlv_TEXCOORD2.xyz + bumpNormal_3);
					  retColor_4.xyz = (retColor_4.xyz + ((textureCube (_V_CW_Cube, P_12) * _V_CW_ReflectColor).xyz * clamp (
					    (retColor_4.w + _V_CW_ReflectStrengthAlphaOffset)
					  , 0.0, 1.0)));
					  highp float tmpvar_13;
					  tmpvar_13 = clamp (xlv_TEXCOORD3, 0.0, 1.0);
					  retColor_4.xyz = mix (unity_FogColor.xyz, retColor_4.xyz, vec3(tmpvar_13));
					  retColor_4.w = 1.0;
					  gl_FragData[0] = retColor_4;
					}
					
					
					#endif"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "FOG_LINEAR" "VERTEXLIGHT_ON" "V_CW_VERTEX_COLOR" "_NORMALMAP" "_EMISSION_OFF" "V_CW_RIM_OFF" "V_CW_SPECULAR_OFF" "V_CW_REFLECTIVE" "V_CW_FOG" "V_CW_USE_LIGHT_RAMP_TEXTURE_OFF" }
					"!!GLES
					#version 100
					
					#ifdef VERTEX
					attribute vec4 _glesTANGENT;
					attribute vec4 _glesVertex;
					attribute vec4 _glesColor;
					attribute vec3 _glesNormal;
					attribute vec4 _glesMultiTexCoord0;
					uniform highp vec4 _Time;
					uniform highp vec3 _WorldSpaceCameraPos;
					uniform highp vec4 _ProjectionParams;
					uniform mediump vec4 _WorldSpaceLightPos0;
					uniform highp mat4 glstate_matrix_mvp;
					uniform highp mat4 _Object2World;
					uniform highp mat4 _World2Object;
					uniform highp vec4 unity_FogParams;
					uniform highp vec3 _V_CW_Bend;
					uniform highp vec3 _V_CW_Bias;
					uniform highp vec4 _V_CW_PivotPoint_Position;
					uniform highp vec4 _MainTex_ST;
					uniform lowp vec2 _V_CW_MainTex_Scroll;
					varying mediump vec4 xlv_TEXCOORD0;
					varying mediump vec4 xlv_TEXCOORD1;
					varying mediump vec4 xlv_TEXCOORD2;
					varying highp vec4 xlv_COLOR0;
					varying highp float xlv_TEXCOORD3;
					varying mediump vec4 xlv_TEXCOORD4;
					varying mediump vec3 xlv_TEXCOORD6;
					varying mediump vec4 xlv_TEXCOORD7;
					void main ()
					{
					  lowp vec4 tmpvar_1;
					  tmpvar_1 = _glesColor;
					  highp vec4 tmpvar_2;
					  mediump vec4 tmpvar_3;
					  mediump vec4 tmpvar_4;
					  mediump vec4 tmpvar_5;
					  highp vec4 tmpvar_6;
					  mediump vec3 tmpvar_7;
					  mediump vec4 tmpvar_8;
					  highp vec4 v_9;
					  highp float tmpvar_10;
					  tmpvar_10 = _World2Object[0].x;
					  v_9.x = tmpvar_10;
					  highp float tmpvar_11;
					  tmpvar_11 = _World2Object[1].x;
					  v_9.y = tmpvar_11;
					  highp float tmpvar_12;
					  tmpvar_12 = _World2Object[2].x;
					  v_9.z = tmpvar_12;
					  highp float tmpvar_13;
					  tmpvar_13 = _World2Object[3].x;
					  v_9.w = tmpvar_13;
					  highp vec4 v_14;
					  highp float tmpvar_15;
					  tmpvar_15 = _World2Object[0].y;
					  v_14.x = tmpvar_15;
					  highp float tmpvar_16;
					  tmpvar_16 = _World2Object[1].y;
					  v_14.y = tmpvar_16;
					  highp float tmpvar_17;
					  tmpvar_17 = _World2Object[2].y;
					  v_14.z = tmpvar_17;
					  highp float tmpvar_18;
					  tmpvar_18 = _World2Object[3].y;
					  v_14.w = tmpvar_18;
					  highp vec4 v_19;
					  highp float tmpvar_20;
					  tmpvar_20 = _World2Object[0].z;
					  v_19.x = tmpvar_20;
					  highp float tmpvar_21;
					  tmpvar_21 = _World2Object[1].z;
					  v_19.y = tmpvar_21;
					  highp float tmpvar_22;
					  tmpvar_22 = _World2Object[2].z;
					  v_19.z = tmpvar_22;
					  highp float tmpvar_23;
					  tmpvar_23 = _World2Object[3].z;
					  v_19.w = tmpvar_23;
					  highp vec3 tmpvar_24;
					  tmpvar_24 = normalize(((
					    (v_9.xyz * _glesNormal.x)
					   + 
					    (v_14.xyz * _glesNormal.y)
					  ) + (v_19.xyz * _glesNormal.z)));
					  highp mat3 tmpvar_25;
					  tmpvar_25[0] = _Object2World[0].xyz;
					  tmpvar_25[1] = _Object2World[1].xyz;
					  tmpvar_25[2] = _Object2World[2].xyz;
					  highp vec3 tmpvar_26;
					  tmpvar_26 = normalize((tmpvar_25 * _glesTANGENT.xyz));
					  highp vec4 vertex_27;
					  vertex_27.w = _glesVertex.w;
					  highp vec2 xzOff_28;
					  highp vec3 v2_29;
					  highp vec3 v1_30;
					  highp vec3 v0_31;
					  highp vec3 tmpvar_32;
					  tmpvar_32 = ((_Object2World * _glesVertex).xyz - _V_CW_PivotPoint_Position.xyz);
					  highp vec3 tmpvar_33;
					  tmpvar_33 = (tmpvar_32 + tmpvar_26);
					  v1_30.xz = tmpvar_33.xz;
					  highp vec3 tmpvar_34;
					  tmpvar_34 = (tmpvar_32 - ((tmpvar_24.yzx * tmpvar_26.zxy) - (tmpvar_24.zxy * tmpvar_26.yzx)));
					  v2_29.xz = tmpvar_34.xz;
					  highp vec2 tmpvar_35;
					  tmpvar_35.x = float((tmpvar_32.z >= 0.0));
					  tmpvar_35.y = float((tmpvar_32.x >= 0.0));
					  xzOff_28 = (max (vec2(0.0, 0.0), (
					    abs(tmpvar_32.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_35 * 2.0) - 1.0));
					  xzOff_28 = (xzOff_28 * xzOff_28);
					  highp vec3 tmpvar_36;
					  tmpvar_36.xz = vec2(0.0, 0.0);
					  tmpvar_36.y = (((_V_CW_Bend.x * xzOff_28.x) + (_V_CW_Bend.z * xzOff_28.y)) * 0.001);
					  v0_31 = (tmpvar_32 + tmpvar_36);
					  highp vec2 tmpvar_37;
					  tmpvar_37.x = float((tmpvar_33.z >= 0.0));
					  tmpvar_37.y = float((tmpvar_33.x >= 0.0));
					  xzOff_28 = (max (vec2(0.0, 0.0), (
					    abs(tmpvar_33.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_37 * 2.0) - 1.0));
					  xzOff_28 = (xzOff_28 * xzOff_28);
					  v1_30.y = (tmpvar_33.y + ((
					    (_V_CW_Bend.x * xzOff_28.x)
					   + 
					    (_V_CW_Bend.z * xzOff_28.y)
					  ) * 0.001));
					  highp vec2 tmpvar_38;
					  tmpvar_38.x = float((tmpvar_34.z >= 0.0));
					  tmpvar_38.y = float((tmpvar_34.x >= 0.0));
					  xzOff_28 = (max (vec2(0.0, 0.0), (
					    abs(tmpvar_34.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_38 * 2.0) - 1.0));
					  xzOff_28 = (xzOff_28 * xzOff_28);
					  v2_29.y = (tmpvar_34.y + ((
					    (_V_CW_Bend.x * xzOff_28.x)
					   + 
					    (_V_CW_Bend.z * xzOff_28.y)
					  ) * 0.001));
					  highp mat3 tmpvar_39;
					  tmpvar_39[0] = _World2Object[0].xyz;
					  tmpvar_39[1] = _World2Object[1].xyz;
					  tmpvar_39[2] = _World2Object[2].xyz;
					  vertex_27.xyz = (_glesVertex.xyz + (tmpvar_39 * tmpvar_36));
					  highp mat3 tmpvar_40;
					  tmpvar_40[0] = _World2Object[0].xyz;
					  tmpvar_40[1] = _World2Object[1].xyz;
					  tmpvar_40[2] = _World2Object[2].xyz;
					  highp vec3 a_41;
					  a_41 = (v2_29 - v0_31);
					  highp vec3 b_42;
					  b_42 = (v1_30 - v0_31);
					  highp vec3 tmpvar_43;
					  tmpvar_43 = normalize((tmpvar_40 * normalize(
					    ((a_41.yzx * b_42.zxy) - (a_41.zxy * b_42.yzx))
					  )));
					  tmpvar_2 = (glstate_matrix_mvp * vertex_27);
					  tmpvar_3.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
					  tmpvar_3.xy = (tmpvar_3.xy + (_V_CW_MainTex_Scroll * _Time.x));
					  tmpvar_6 = tmpvar_1;
					  highp vec4 v_44;
					  v_44.x = tmpvar_10;
					  v_44.y = tmpvar_11;
					  v_44.z = tmpvar_12;
					  v_44.w = tmpvar_13;
					  highp vec4 v_45;
					  v_45.x = tmpvar_15;
					  v_45.y = tmpvar_16;
					  v_45.z = tmpvar_17;
					  v_45.w = tmpvar_18;
					  highp vec4 v_46;
					  v_46.x = tmpvar_20;
					  v_46.y = tmpvar_21;
					  v_46.z = tmpvar_22;
					  v_46.w = tmpvar_23;
					  highp vec3 tmpvar_47;
					  tmpvar_47 = normalize(((
					    (v_44.xyz * tmpvar_43.x)
					   + 
					    (v_45.xyz * tmpvar_43.y)
					  ) + (v_46.xyz * tmpvar_43.z)));
					  highp vec3 tmpvar_48;
					  tmpvar_48 = normalize(tmpvar_43);
					  highp vec3 tmpvar_49;
					  tmpvar_49 = normalize(_glesTANGENT.xyz);
					  highp vec3 tmpvar_50;
					  highp vec3 tmpvar_51;
					  tmpvar_50 = _glesTANGENT.xyz;
					  tmpvar_51 = (((tmpvar_48.yzx * tmpvar_49.zxy) - (tmpvar_48.zxy * tmpvar_49.yzx)) * _glesTANGENT.w);
					  highp mat3 tmpvar_52;
					  tmpvar_52[0].x = tmpvar_50.x;
					  tmpvar_52[0].y = tmpvar_51.x;
					  tmpvar_52[0].z = tmpvar_43.x;
					  tmpvar_52[1].x = tmpvar_50.y;
					  tmpvar_52[1].y = tmpvar_51.y;
					  tmpvar_52[1].z = tmpvar_43.y;
					  tmpvar_52[2].x = tmpvar_50.z;
					  tmpvar_52[2].y = tmpvar_51.z;
					  tmpvar_52[2].z = tmpvar_43.z;
					  highp vec3 tmpvar_53;
					  tmpvar_53 = normalize((tmpvar_52 * (_World2Object * _WorldSpaceLightPos0).xyz));
					  tmpvar_7 = tmpvar_53;
					  tmpvar_4.xyz = tmpvar_47;
					  highp vec3 I_54;
					  I_54 = ((_Object2World * vertex_27).xyz - _WorldSpaceCameraPos);
					  highp vec3 tmpvar_55;
					  tmpvar_55 = normalize((I_54 - (2.0 * 
					    (dot (tmpvar_47, I_54) * tmpvar_47)
					  )));
					  tmpvar_5.xyz = tmpvar_55;
					  highp vec4 o_56;
					  highp vec4 tmpvar_57;
					  tmpvar_57 = (tmpvar_2 * 0.5);
					  highp vec2 tmpvar_58;
					  tmpvar_58.x = tmpvar_57.x;
					  tmpvar_58.y = (tmpvar_57.y * _ProjectionParams.x);
					  o_56.xy = (tmpvar_58 + tmpvar_57.w);
					  o_56.zw = tmpvar_2.zw;
					  tmpvar_8 = o_56;
					  gl_Position = tmpvar_2;
					  xlv_TEXCOORD0 = tmpvar_3;
					  xlv_TEXCOORD1 = tmpvar_4;
					  xlv_TEXCOORD2 = tmpvar_5;
					  xlv_COLOR0 = tmpvar_6;
					  xlv_TEXCOORD3 = ((tmpvar_2.z * unity_FogParams.z) + unity_FogParams.w);
					  xlv_TEXCOORD4 = vec4(0.0, 0.0, 0.0, 0.0);
					  xlv_TEXCOORD6 = tmpvar_7;
					  xlv_TEXCOORD7 = tmpvar_8;
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform lowp vec4 glstate_lightmodel_ambient;
					uniform lowp vec4 unity_FogColor;
					uniform lowp vec4 _LightColor0;
					uniform sampler2D _ShadowMapTexture;
					uniform lowp vec4 _Color;
					uniform sampler2D _MainTex;
					uniform sampler2D _V_CW_NormalMap;
					uniform mediump float _V_CW_NormalMap_UV_Scale;
					uniform mediump float _V_CW_NormalMapStrength;
					uniform lowp samplerCube _V_CW_Cube;
					uniform lowp vec4 _V_CW_ReflectColor;
					uniform lowp float _V_CW_ReflectStrengthAlphaOffset;
					varying mediump vec4 xlv_TEXCOORD0;
					varying mediump vec4 xlv_TEXCOORD2;
					varying highp vec4 xlv_COLOR0;
					varying highp float xlv_TEXCOORD3;
					varying mediump vec3 xlv_TEXCOORD6;
					varying mediump vec4 xlv_TEXCOORD7;
					void main ()
					{
					  lowp vec3 diff_1;
					  mediump vec3 normal_2;
					  lowp vec3 bumpNormal_3;
					  lowp vec4 retColor_4;
					  mediump vec4 mainTex_5;
					  lowp vec4 tmpvar_6;
					  tmpvar_6 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
					  mainTex_5 = tmpvar_6;
					  retColor_4 = mainTex_5;
					  retColor_4 = (retColor_4 * _Color);
					  retColor_4 = (retColor_4 * xlv_COLOR0);
					  mediump vec2 P_7;
					  P_7 = (xlv_TEXCOORD0.xy * _V_CW_NormalMap_UV_Scale);
					  lowp vec3 normal_8;
					  normal_8.xy = ((texture2D (_V_CW_NormalMap, P_7).wy * 2.0) - 1.0);
					  normal_8.z = sqrt((1.0 - clamp (
					    dot (normal_8.xy, normal_8.xy)
					  , 0.0, 1.0)));
					  mediump vec3 tmpvar_9;
					  tmpvar_9.xy = (normal_8.xy * vec2(_V_CW_NormalMapStrength));
					  tmpvar_9.z = normal_8.z;
					  mediump vec3 tmpvar_10;
					  tmpvar_10 = normalize(tmpvar_9);
					  bumpNormal_3 = tmpvar_10;
					  normal_2 = bumpNormal_3;
					  mediump float tmpvar_11;
					  tmpvar_11 = max (0.0, dot (normal_2, xlv_TEXCOORD6));
					  diff_1 = ((_LightColor0.xyz * texture2DProj (_ShadowMapTexture, xlv_TEXCOORD7).x) * tmpvar_11);
					  diff_1 = (diff_1 + (glstate_lightmodel_ambient * 2.0).xyz);
					  retColor_4.xyz = (diff_1 * retColor_4.xyz);
					  mediump vec3 P_12;
					  P_12 = (xlv_TEXCOORD2.xyz + bumpNormal_3);
					  retColor_4.xyz = (retColor_4.xyz + ((textureCube (_V_CW_Cube, P_12) * _V_CW_ReflectColor).xyz * clamp (
					    (retColor_4.w + _V_CW_ReflectStrengthAlphaOffset)
					  , 0.0, 1.0)));
					  highp float tmpvar_13;
					  tmpvar_13 = clamp (xlv_TEXCOORD3, 0.0, 1.0);
					  retColor_4.xyz = mix (unity_FogColor.xyz, retColor_4.xyz, vec3(tmpvar_13));
					  retColor_4.w = 1.0;
					  gl_FragData[0] = retColor_4;
					}
					
					
					#endif"
}
}
Program "fp" {
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_OFF" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "V_CW_VERTEX_COLOR" "_NORMALMAP" "_EMISSION_OFF" "V_CW_RIM_OFF" "V_CW_SPECULAR_OFF" "V_CW_REFLECTIVE" "V_CW_FOG" "V_CW_USE_LIGHT_RAMP_TEXTURE_OFF" }
					"!!GLES"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "V_CW_VERTEX_COLOR" "_NORMALMAP" "_EMISSION_OFF" "V_CW_RIM_OFF" "V_CW_SPECULAR_OFF" "V_CW_REFLECTIVE" "V_CW_FOG" "V_CW_USE_LIGHT_RAMP_TEXTURE_OFF" }
					"!!GLES"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_OFF" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "FOG_LINEAR" "V_CW_VERTEX_COLOR" "_NORMALMAP" "_EMISSION_OFF" "V_CW_RIM_OFF" "V_CW_SPECULAR_OFF" "V_CW_REFLECTIVE" "V_CW_FOG" "V_CW_USE_LIGHT_RAMP_TEXTURE_OFF" }
					"!!GLES"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "FOG_LINEAR" "V_CW_VERTEX_COLOR" "_NORMALMAP" "_EMISSION_OFF" "V_CW_RIM_OFF" "V_CW_SPECULAR_OFF" "V_CW_REFLECTIVE" "V_CW_FOG" "V_CW_USE_LIGHT_RAMP_TEXTURE_OFF" }
					"!!GLES"
}
}
 }
}
Fallback "Hidden/VacuumShaders/Curved World/VertexLit/Diffuse"
}