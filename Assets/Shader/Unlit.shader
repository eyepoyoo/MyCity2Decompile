Shader "VacuumShaders/Curved World/Unlit" {
Properties {
[CurvedWorldGearMenu]  V_CW_Label_Tag ("", Float) = 0
[CurvedWorldLabel]  V_CW_Label_UnityDefaults ("Default Visual Options", Float) = 0
[CurvedWorldLargeLabel]  V_CW_Label_Modes ("Modes", Float) = 0
[CurvedWorldRenderingMode]  V_CW_Rendering_Mode ("  Rendering", Float) = 0
[CurvedWorldTextureMixMode]  V_CW_Texture_Mix_Mode ("  Texture Mix", Float) = 0
[CurvedWorldLargeLabel]  V_CW_Label_Albedo ("Albedo", Float) = 0
 _Color ("  Color", Color) = (1,1,1,1)
 _MainTex ("  Map (RGB) RefStr (A)", 2D) = "white" { }
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
[HideInInspector]  _V_CW_IBL_Matcap ("", 2D) = "Gray" { }
[HideInInspector]  _V_CW_ReflectColor ("", Color) = (1,1,1,1)
[HideInInspector]  _V_CW_ReflectStrengthAlphaOffset ("", Range(-1,1)) = 0
[HideInInspector]  _V_CW_Cube ("", CUBE) = "_Skybox" { }
[HideInInspector]  _V_CW_Fresnel_Bias ("", Range(-1,1)) = 0
[HideInInspector]  _V_CW_NormalMapStrength ("", Float) = 1
[HideInInspector]  _V_CW_NormalMap ("", 2D) = "bump" { }
[HideInInspector]  _V_CW_NormalMap_UV_Scale ("", Float) = 1
[HideInInspector]  _V_CW_SecondaryNormalMap ("", 2D) = "" { }
[HideInInspector]  _V_CW_SecondaryNormalMap_UV_Scale ("", Float) = 1
}
SubShader { 
 LOD 100
 Tags { "RenderType"="CurvedWorld_Opaque" "CurvedWorldTag"="Unlit/Opaque/Texture" "CurvedWorldNoneRemoveableKeywords"="" "CurvedWorldAvailableOptions"="V_CW_REFLECTIVE;V_CW_VERTEX_COLOR;V_CW_IBL;_EMISSION;V_CW_RIM;V_CW_FOG;_NORMALMAP;" }
 Pass {
  Name "BASE"
  Tags { "RenderType"="CurvedWorld_Opaque" "CurvedWorldTag"="Unlit/Opaque/Texture" "CurvedWorldNoneRemoveableKeywords"="" "CurvedWorldAvailableOptions"="V_CW_REFLECTIVE;V_CW_VERTEX_COLOR;V_CW_IBL;_EMISSION;V_CW_RIM;V_CW_FOG;_NORMALMAP;" }
  Cull Off
  GpuProgramID 30351
Program "vp" {
SubProgram "gles " {
Keywords { "V_CW_REFLECTIVE_OFF" "V_CW_VERTEX_COLOR_OFF" "_EMISSION_OFF" "V_CW_RIM_OFF" "_NORMALMAP_OFF" "V_CW_FOG_OFF" "V_CW_IBL_OFF" "V_CW_MATCAP_BLEND_MULTIPLY" }
					"!!GLES
					#version 100
					
					#ifdef VERTEX
					attribute vec4 _glesVertex;
					attribute vec4 _glesMultiTexCoord0;
					uniform highp vec4 _Time;
					uniform highp mat4 glstate_matrix_mvp;
					uniform highp mat4 _Object2World;
					uniform highp mat4 _World2Object;
					uniform highp vec3 _V_CW_Bend;
					uniform highp vec3 _V_CW_Bias;
					uniform highp vec4 _V_CW_PivotPoint_Position;
					uniform mediump vec4 _MainTex_ST;
					uniform lowp vec2 _V_CW_MainTex_Scroll;
					varying mediump vec4 xlv_TEXCOORD0;
					varying mediump vec4 xlv_TEXCOORD1;
					void main ()
					{
					  mediump vec4 tmpvar_1;
					  mediump vec4 tmpvar_2;
					  highp vec2 xzOff_3;
					  highp vec4 worldPos_4;
					  highp vec4 tmpvar_5;
					  tmpvar_5 = (_Object2World * _glesVertex);
					  worldPos_4.w = tmpvar_5.w;
					  worldPos_4.xyz = (tmpvar_5.xyz - _V_CW_PivotPoint_Position.xyz);
					  highp vec2 tmpvar_6;
					  tmpvar_6.x = float((worldPos_4.z >= 0.0));
					  tmpvar_6.y = float((worldPos_4.x >= 0.0));
					  xzOff_3 = (max (vec2(0.0, 0.0), (
					    abs(worldPos_4.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_6 * 2.0) - 1.0));
					  xzOff_3 = (xzOff_3 * xzOff_3);
					  highp vec4 tmpvar_7;
					  tmpvar_7.xzw = vec3(0.0, 0.0, 0.0);
					  tmpvar_7.y = (((_V_CW_Bend.x * xzOff_3.x) + (_V_CW_Bend.z * xzOff_3.y)) * 0.001);
					  worldPos_4 = tmpvar_7;
					  tmpvar_1.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
					  tmpvar_1.xy = (tmpvar_1.xy + (_V_CW_MainTex_Scroll * _Time.x));
					  gl_Position = (glstate_matrix_mvp * (_glesVertex + (_World2Object * tmpvar_7)));
					  xlv_TEXCOORD0 = tmpvar_1;
					  xlv_TEXCOORD1 = tmpvar_2;
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform lowp vec4 _Color;
					uniform sampler2D _MainTex;
					varying mediump vec4 xlv_TEXCOORD0;
					void main ()
					{
					  lowp vec4 retColor_1;
					  mediump vec4 mainTex_2;
					  lowp vec4 tmpvar_3;
					  tmpvar_3 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
					  mainTex_2 = tmpvar_3;
					  retColor_1 = mainTex_2;
					  retColor_1.xyz = (retColor_1 * _Color).xyz;
					  retColor_1.w = 1.0;
					  gl_FragData[0] = retColor_1;
					}
					
					
					#endif"
}
SubProgram "gles " {
Keywords { "FOG_LINEAR" "V_CW_REFLECTIVE_OFF" "V_CW_VERTEX_COLOR_OFF" "_EMISSION_OFF" "V_CW_RIM_OFF" "_NORMALMAP_OFF" "V_CW_FOG_OFF" "V_CW_IBL_OFF" "V_CW_MATCAP_BLEND_MULTIPLY" }
					"!!GLES
					#version 100
					
					#ifdef VERTEX
					attribute vec4 _glesVertex;
					attribute vec4 _glesMultiTexCoord0;
					uniform highp vec4 _Time;
					uniform highp mat4 glstate_matrix_mvp;
					uniform highp mat4 _Object2World;
					uniform highp mat4 _World2Object;
					uniform highp vec3 _V_CW_Bend;
					uniform highp vec3 _V_CW_Bias;
					uniform highp vec4 _V_CW_PivotPoint_Position;
					uniform mediump vec4 _MainTex_ST;
					uniform lowp vec2 _V_CW_MainTex_Scroll;
					varying mediump vec4 xlv_TEXCOORD0;
					varying mediump vec4 xlv_TEXCOORD1;
					varying highp float xlv_TEXCOORD3;
					void main ()
					{
					  mediump vec4 tmpvar_1;
					  mediump vec4 tmpvar_2;
					  highp float tmpvar_3;
					  highp vec2 xzOff_4;
					  highp vec4 worldPos_5;
					  highp vec4 tmpvar_6;
					  tmpvar_6 = (_Object2World * _glesVertex);
					  worldPos_5.w = tmpvar_6.w;
					  worldPos_5.xyz = (tmpvar_6.xyz - _V_CW_PivotPoint_Position.xyz);
					  highp vec2 tmpvar_7;
					  tmpvar_7.x = float((worldPos_5.z >= 0.0));
					  tmpvar_7.y = float((worldPos_5.x >= 0.0));
					  xzOff_4 = (max (vec2(0.0, 0.0), (
					    abs(worldPos_5.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_7 * 2.0) - 1.0));
					  xzOff_4 = (xzOff_4 * xzOff_4);
					  highp vec4 tmpvar_8;
					  tmpvar_8.xzw = vec3(0.0, 0.0, 0.0);
					  tmpvar_8.y = (((_V_CW_Bend.x * xzOff_4.x) + (_V_CW_Bend.z * xzOff_4.y)) * 0.001);
					  worldPos_5 = tmpvar_8;
					  tmpvar_1.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
					  tmpvar_1.xy = (tmpvar_1.xy + (_V_CW_MainTex_Scroll * _Time.x));
					  gl_Position = (glstate_matrix_mvp * (_glesVertex + (_World2Object * tmpvar_8)));
					  xlv_TEXCOORD0 = tmpvar_1;
					  xlv_TEXCOORD1 = tmpvar_2;
					  xlv_TEXCOORD3 = tmpvar_3;
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform lowp vec4 _Color;
					uniform sampler2D _MainTex;
					varying mediump vec4 xlv_TEXCOORD0;
					void main ()
					{
					  lowp vec4 retColor_1;
					  mediump vec4 mainTex_2;
					  lowp vec4 tmpvar_3;
					  tmpvar_3 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
					  mainTex_2 = tmpvar_3;
					  retColor_1 = mainTex_2;
					  retColor_1.xyz = (retColor_1 * _Color).xyz;
					  retColor_1.w = 1.0;
					  gl_FragData[0] = retColor_1;
					}
					
					
					#endif"
}
SubProgram "gles " {
Keywords { "V_CW_REFLECTIVE_OFF" "V_CW_VERTEX_COLOR_OFF" "_EMISSION_OFF" "V_CW_RIM_OFF" "_NORMALMAP_OFF" "V_CW_FOG" "V_CW_IBL_OFF" "V_CW_MATCAP_BLEND_MULTIPLY" }
					"!!GLES
					#version 100
					
					#ifdef VERTEX
					attribute vec4 _glesVertex;
					attribute vec4 _glesMultiTexCoord0;
					uniform highp vec4 _Time;
					uniform highp mat4 glstate_matrix_mvp;
					uniform highp mat4 _Object2World;
					uniform highp mat4 _World2Object;
					uniform highp vec3 _V_CW_Bend;
					uniform highp vec3 _V_CW_Bias;
					uniform highp vec4 _V_CW_PivotPoint_Position;
					uniform mediump vec4 _MainTex_ST;
					uniform lowp vec2 _V_CW_MainTex_Scroll;
					varying mediump vec4 xlv_TEXCOORD0;
					varying mediump vec4 xlv_TEXCOORD1;
					void main ()
					{
					  mediump vec4 tmpvar_1;
					  mediump vec4 tmpvar_2;
					  highp vec2 xzOff_3;
					  highp vec4 worldPos_4;
					  highp vec4 tmpvar_5;
					  tmpvar_5 = (_Object2World * _glesVertex);
					  worldPos_4.w = tmpvar_5.w;
					  worldPos_4.xyz = (tmpvar_5.xyz - _V_CW_PivotPoint_Position.xyz);
					  highp vec2 tmpvar_6;
					  tmpvar_6.x = float((worldPos_4.z >= 0.0));
					  tmpvar_6.y = float((worldPos_4.x >= 0.0));
					  xzOff_3 = (max (vec2(0.0, 0.0), (
					    abs(worldPos_4.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_6 * 2.0) - 1.0));
					  xzOff_3 = (xzOff_3 * xzOff_3);
					  highp vec4 tmpvar_7;
					  tmpvar_7.xzw = vec3(0.0, 0.0, 0.0);
					  tmpvar_7.y = (((_V_CW_Bend.x * xzOff_3.x) + (_V_CW_Bend.z * xzOff_3.y)) * 0.001);
					  worldPos_4 = tmpvar_7;
					  tmpvar_1.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
					  tmpvar_1.xy = (tmpvar_1.xy + (_V_CW_MainTex_Scroll * _Time.x));
					  gl_Position = (glstate_matrix_mvp * (_glesVertex + (_World2Object * tmpvar_7)));
					  xlv_TEXCOORD0 = tmpvar_1;
					  xlv_TEXCOORD1 = tmpvar_2;
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform lowp vec4 _Color;
					uniform sampler2D _MainTex;
					varying mediump vec4 xlv_TEXCOORD0;
					void main ()
					{
					  lowp vec4 retColor_1;
					  mediump vec4 mainTex_2;
					  lowp vec4 tmpvar_3;
					  tmpvar_3 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
					  mainTex_2 = tmpvar_3;
					  retColor_1 = mainTex_2;
					  retColor_1.xyz = (retColor_1 * _Color).xyz;
					  retColor_1.w = 1.0;
					  gl_FragData[0] = retColor_1;
					}
					
					
					#endif"
}
SubProgram "gles " {
Keywords { "FOG_LINEAR" "V_CW_REFLECTIVE_OFF" "V_CW_VERTEX_COLOR_OFF" "_EMISSION_OFF" "V_CW_RIM_OFF" "_NORMALMAP_OFF" "V_CW_FOG" "V_CW_IBL_OFF" "V_CW_MATCAP_BLEND_MULTIPLY" }
					"!!GLES
					#version 100
					
					#ifdef VERTEX
					attribute vec4 _glesVertex;
					attribute vec4 _glesMultiTexCoord0;
					uniform highp vec4 _Time;
					uniform highp mat4 glstate_matrix_mvp;
					uniform highp mat4 _Object2World;
					uniform highp mat4 _World2Object;
					uniform highp vec4 unity_FogParams;
					uniform highp vec3 _V_CW_Bend;
					uniform highp vec3 _V_CW_Bias;
					uniform highp vec4 _V_CW_PivotPoint_Position;
					uniform mediump vec4 _MainTex_ST;
					uniform lowp vec2 _V_CW_MainTex_Scroll;
					varying mediump vec4 xlv_TEXCOORD0;
					varying mediump vec4 xlv_TEXCOORD1;
					varying highp float xlv_TEXCOORD3;
					void main ()
					{
					  highp vec4 tmpvar_1;
					  mediump vec4 tmpvar_2;
					  mediump vec4 tmpvar_3;
					  highp vec2 xzOff_4;
					  highp vec4 worldPos_5;
					  highp vec4 tmpvar_6;
					  tmpvar_6 = (_Object2World * _glesVertex);
					  worldPos_5.w = tmpvar_6.w;
					  worldPos_5.xyz = (tmpvar_6.xyz - _V_CW_PivotPoint_Position.xyz);
					  highp vec2 tmpvar_7;
					  tmpvar_7.x = float((worldPos_5.z >= 0.0));
					  tmpvar_7.y = float((worldPos_5.x >= 0.0));
					  xzOff_4 = (max (vec2(0.0, 0.0), (
					    abs(worldPos_5.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_7 * 2.0) - 1.0));
					  xzOff_4 = (xzOff_4 * xzOff_4);
					  highp vec4 tmpvar_8;
					  tmpvar_8.xzw = vec3(0.0, 0.0, 0.0);
					  tmpvar_8.y = (((_V_CW_Bend.x * xzOff_4.x) + (_V_CW_Bend.z * xzOff_4.y)) * 0.001);
					  worldPos_5 = tmpvar_8;
					  tmpvar_1 = (glstate_matrix_mvp * (_glesVertex + (_World2Object * tmpvar_8)));
					  tmpvar_2.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
					  tmpvar_2.xy = (tmpvar_2.xy + (_V_CW_MainTex_Scroll * _Time.x));
					  gl_Position = tmpvar_1;
					  xlv_TEXCOORD0 = tmpvar_2;
					  xlv_TEXCOORD1 = tmpvar_3;
					  xlv_TEXCOORD3 = ((tmpvar_1.z * unity_FogParams.z) + unity_FogParams.w);
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform lowp vec4 unity_FogColor;
					uniform lowp vec4 _Color;
					uniform sampler2D _MainTex;
					varying mediump vec4 xlv_TEXCOORD0;
					varying highp float xlv_TEXCOORD3;
					void main ()
					{
					  lowp vec4 retColor_1;
					  mediump vec4 mainTex_2;
					  lowp vec4 tmpvar_3;
					  tmpvar_3 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
					  mainTex_2 = tmpvar_3;
					  retColor_1 = mainTex_2;
					  retColor_1 = (retColor_1 * _Color);
					  highp float tmpvar_4;
					  tmpvar_4 = clamp (xlv_TEXCOORD3, 0.0, 1.0);
					  retColor_1.xyz = mix (unity_FogColor.xyz, retColor_1.xyz, vec3(tmpvar_4));
					  retColor_1.w = 1.0;
					  gl_FragData[0] = retColor_1;
					}
					
					
					#endif"
}
SubProgram "gles " {
Keywords { "V_CW_VERTEX_COLOR" "V_CW_REFLECTIVE_OFF" "_EMISSION_OFF" "V_CW_RIM_OFF" "_NORMALMAP_OFF" "V_CW_FOG_OFF" "V_CW_IBL_OFF" "V_CW_MATCAP_BLEND_MULTIPLY" }
					"!!GLES
					#version 100
					
					#ifdef VERTEX
					attribute vec4 _glesVertex;
					attribute vec4 _glesColor;
					attribute vec4 _glesMultiTexCoord0;
					uniform highp vec4 _Time;
					uniform highp mat4 glstate_matrix_mvp;
					uniform highp mat4 _Object2World;
					uniform highp mat4 _World2Object;
					uniform highp vec3 _V_CW_Bend;
					uniform highp vec3 _V_CW_Bias;
					uniform highp vec4 _V_CW_PivotPoint_Position;
					uniform mediump vec4 _MainTex_ST;
					uniform lowp vec2 _V_CW_MainTex_Scroll;
					varying mediump vec4 xlv_TEXCOORD0;
					varying mediump vec4 xlv_TEXCOORD1;
					varying lowp vec4 xlv_COLOR;
					void main ()
					{
					  mediump vec4 tmpvar_1;
					  mediump vec4 tmpvar_2;
					  highp vec2 xzOff_3;
					  highp vec4 worldPos_4;
					  highp vec4 tmpvar_5;
					  tmpvar_5 = (_Object2World * _glesVertex);
					  worldPos_4.w = tmpvar_5.w;
					  worldPos_4.xyz = (tmpvar_5.xyz - _V_CW_PivotPoint_Position.xyz);
					  highp vec2 tmpvar_6;
					  tmpvar_6.x = float((worldPos_4.z >= 0.0));
					  tmpvar_6.y = float((worldPos_4.x >= 0.0));
					  xzOff_3 = (max (vec2(0.0, 0.0), (
					    abs(worldPos_4.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_6 * 2.0) - 1.0));
					  xzOff_3 = (xzOff_3 * xzOff_3);
					  highp vec4 tmpvar_7;
					  tmpvar_7.xzw = vec3(0.0, 0.0, 0.0);
					  tmpvar_7.y = (((_V_CW_Bend.x * xzOff_3.x) + (_V_CW_Bend.z * xzOff_3.y)) * 0.001);
					  worldPos_4 = tmpvar_7;
					  tmpvar_1.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
					  tmpvar_1.xy = (tmpvar_1.xy + (_V_CW_MainTex_Scroll * _Time.x));
					  gl_Position = (glstate_matrix_mvp * (_glesVertex + (_World2Object * tmpvar_7)));
					  xlv_TEXCOORD0 = tmpvar_1;
					  xlv_TEXCOORD1 = tmpvar_2;
					  xlv_COLOR = _glesColor;
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform lowp vec4 _Color;
					uniform sampler2D _MainTex;
					varying mediump vec4 xlv_TEXCOORD0;
					varying lowp vec4 xlv_COLOR;
					void main ()
					{
					  lowp vec4 retColor_1;
					  mediump vec4 mainTex_2;
					  lowp vec4 tmpvar_3;
					  tmpvar_3 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
					  mainTex_2 = tmpvar_3;
					  retColor_1 = mainTex_2;
					  retColor_1 = (retColor_1 * _Color);
					  retColor_1.xyz = (retColor_1 * xlv_COLOR).xyz;
					  retColor_1.w = 1.0;
					  gl_FragData[0] = retColor_1;
					}
					
					
					#endif"
}
SubProgram "gles " {
Keywords { "FOG_LINEAR" "V_CW_VERTEX_COLOR" "V_CW_REFLECTIVE_OFF" "_EMISSION_OFF" "V_CW_RIM_OFF" "_NORMALMAP_OFF" "V_CW_FOG_OFF" "V_CW_IBL_OFF" "V_CW_MATCAP_BLEND_MULTIPLY" }
					"!!GLES
					#version 100
					
					#ifdef VERTEX
					attribute vec4 _glesVertex;
					attribute vec4 _glesColor;
					attribute vec4 _glesMultiTexCoord0;
					uniform highp vec4 _Time;
					uniform highp mat4 glstate_matrix_mvp;
					uniform highp mat4 _Object2World;
					uniform highp mat4 _World2Object;
					uniform highp vec3 _V_CW_Bend;
					uniform highp vec3 _V_CW_Bias;
					uniform highp vec4 _V_CW_PivotPoint_Position;
					uniform mediump vec4 _MainTex_ST;
					uniform lowp vec2 _V_CW_MainTex_Scroll;
					varying mediump vec4 xlv_TEXCOORD0;
					varying mediump vec4 xlv_TEXCOORD1;
					varying highp float xlv_TEXCOORD3;
					varying lowp vec4 xlv_COLOR;
					void main ()
					{
					  mediump vec4 tmpvar_1;
					  mediump vec4 tmpvar_2;
					  highp float tmpvar_3;
					  highp vec2 xzOff_4;
					  highp vec4 worldPos_5;
					  highp vec4 tmpvar_6;
					  tmpvar_6 = (_Object2World * _glesVertex);
					  worldPos_5.w = tmpvar_6.w;
					  worldPos_5.xyz = (tmpvar_6.xyz - _V_CW_PivotPoint_Position.xyz);
					  highp vec2 tmpvar_7;
					  tmpvar_7.x = float((worldPos_5.z >= 0.0));
					  tmpvar_7.y = float((worldPos_5.x >= 0.0));
					  xzOff_4 = (max (vec2(0.0, 0.0), (
					    abs(worldPos_5.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_7 * 2.0) - 1.0));
					  xzOff_4 = (xzOff_4 * xzOff_4);
					  highp vec4 tmpvar_8;
					  tmpvar_8.xzw = vec3(0.0, 0.0, 0.0);
					  tmpvar_8.y = (((_V_CW_Bend.x * xzOff_4.x) + (_V_CW_Bend.z * xzOff_4.y)) * 0.001);
					  worldPos_5 = tmpvar_8;
					  tmpvar_1.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
					  tmpvar_1.xy = (tmpvar_1.xy + (_V_CW_MainTex_Scroll * _Time.x));
					  gl_Position = (glstate_matrix_mvp * (_glesVertex + (_World2Object * tmpvar_8)));
					  xlv_TEXCOORD0 = tmpvar_1;
					  xlv_TEXCOORD1 = tmpvar_2;
					  xlv_TEXCOORD3 = tmpvar_3;
					  xlv_COLOR = _glesColor;
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform lowp vec4 _Color;
					uniform sampler2D _MainTex;
					varying mediump vec4 xlv_TEXCOORD0;
					varying lowp vec4 xlv_COLOR;
					void main ()
					{
					  lowp vec4 retColor_1;
					  mediump vec4 mainTex_2;
					  lowp vec4 tmpvar_3;
					  tmpvar_3 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
					  mainTex_2 = tmpvar_3;
					  retColor_1 = mainTex_2;
					  retColor_1 = (retColor_1 * _Color);
					  retColor_1.xyz = (retColor_1 * xlv_COLOR).xyz;
					  retColor_1.w = 1.0;
					  gl_FragData[0] = retColor_1;
					}
					
					
					#endif"
}
SubProgram "gles " {
Keywords { "V_CW_VERTEX_COLOR" "V_CW_REFLECTIVE_OFF" "_EMISSION_OFF" "V_CW_RIM_OFF" "_NORMALMAP_OFF" "V_CW_FOG" "V_CW_IBL_OFF" "V_CW_MATCAP_BLEND_MULTIPLY" }
					"!!GLES
					#version 100
					
					#ifdef VERTEX
					attribute vec4 _glesVertex;
					attribute vec4 _glesColor;
					attribute vec4 _glesMultiTexCoord0;
					uniform highp vec4 _Time;
					uniform highp mat4 glstate_matrix_mvp;
					uniform highp mat4 _Object2World;
					uniform highp mat4 _World2Object;
					uniform highp vec3 _V_CW_Bend;
					uniform highp vec3 _V_CW_Bias;
					uniform highp vec4 _V_CW_PivotPoint_Position;
					uniform mediump vec4 _MainTex_ST;
					uniform lowp vec2 _V_CW_MainTex_Scroll;
					varying mediump vec4 xlv_TEXCOORD0;
					varying mediump vec4 xlv_TEXCOORD1;
					varying lowp vec4 xlv_COLOR;
					void main ()
					{
					  mediump vec4 tmpvar_1;
					  mediump vec4 tmpvar_2;
					  highp vec2 xzOff_3;
					  highp vec4 worldPos_4;
					  highp vec4 tmpvar_5;
					  tmpvar_5 = (_Object2World * _glesVertex);
					  worldPos_4.w = tmpvar_5.w;
					  worldPos_4.xyz = (tmpvar_5.xyz - _V_CW_PivotPoint_Position.xyz);
					  highp vec2 tmpvar_6;
					  tmpvar_6.x = float((worldPos_4.z >= 0.0));
					  tmpvar_6.y = float((worldPos_4.x >= 0.0));
					  xzOff_3 = (max (vec2(0.0, 0.0), (
					    abs(worldPos_4.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_6 * 2.0) - 1.0));
					  xzOff_3 = (xzOff_3 * xzOff_3);
					  highp vec4 tmpvar_7;
					  tmpvar_7.xzw = vec3(0.0, 0.0, 0.0);
					  tmpvar_7.y = (((_V_CW_Bend.x * xzOff_3.x) + (_V_CW_Bend.z * xzOff_3.y)) * 0.001);
					  worldPos_4 = tmpvar_7;
					  tmpvar_1.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
					  tmpvar_1.xy = (tmpvar_1.xy + (_V_CW_MainTex_Scroll * _Time.x));
					  gl_Position = (glstate_matrix_mvp * (_glesVertex + (_World2Object * tmpvar_7)));
					  xlv_TEXCOORD0 = tmpvar_1;
					  xlv_TEXCOORD1 = tmpvar_2;
					  xlv_COLOR = _glesColor;
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform lowp vec4 _Color;
					uniform sampler2D _MainTex;
					varying mediump vec4 xlv_TEXCOORD0;
					varying lowp vec4 xlv_COLOR;
					void main ()
					{
					  lowp vec4 retColor_1;
					  mediump vec4 mainTex_2;
					  lowp vec4 tmpvar_3;
					  tmpvar_3 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
					  mainTex_2 = tmpvar_3;
					  retColor_1 = mainTex_2;
					  retColor_1 = (retColor_1 * _Color);
					  retColor_1.xyz = (retColor_1 * xlv_COLOR).xyz;
					  retColor_1.w = 1.0;
					  gl_FragData[0] = retColor_1;
					}
					
					
					#endif"
}
SubProgram "gles " {
Keywords { "FOG_LINEAR" "V_CW_VERTEX_COLOR" "V_CW_REFLECTIVE_OFF" "_EMISSION_OFF" "V_CW_RIM_OFF" "_NORMALMAP_OFF" "V_CW_FOG" "V_CW_IBL_OFF" "V_CW_MATCAP_BLEND_MULTIPLY" }
					"!!GLES
					#version 100
					
					#ifdef VERTEX
					attribute vec4 _glesVertex;
					attribute vec4 _glesColor;
					attribute vec4 _glesMultiTexCoord0;
					uniform highp vec4 _Time;
					uniform highp mat4 glstate_matrix_mvp;
					uniform highp mat4 _Object2World;
					uniform highp mat4 _World2Object;
					uniform highp vec4 unity_FogParams;
					uniform highp vec3 _V_CW_Bend;
					uniform highp vec3 _V_CW_Bias;
					uniform highp vec4 _V_CW_PivotPoint_Position;
					uniform mediump vec4 _MainTex_ST;
					uniform lowp vec2 _V_CW_MainTex_Scroll;
					varying mediump vec4 xlv_TEXCOORD0;
					varying mediump vec4 xlv_TEXCOORD1;
					varying highp float xlv_TEXCOORD3;
					varying lowp vec4 xlv_COLOR;
					void main ()
					{
					  highp vec4 tmpvar_1;
					  mediump vec4 tmpvar_2;
					  mediump vec4 tmpvar_3;
					  highp vec2 xzOff_4;
					  highp vec4 worldPos_5;
					  highp vec4 tmpvar_6;
					  tmpvar_6 = (_Object2World * _glesVertex);
					  worldPos_5.w = tmpvar_6.w;
					  worldPos_5.xyz = (tmpvar_6.xyz - _V_CW_PivotPoint_Position.xyz);
					  highp vec2 tmpvar_7;
					  tmpvar_7.x = float((worldPos_5.z >= 0.0));
					  tmpvar_7.y = float((worldPos_5.x >= 0.0));
					  xzOff_4 = (max (vec2(0.0, 0.0), (
					    abs(worldPos_5.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_7 * 2.0) - 1.0));
					  xzOff_4 = (xzOff_4 * xzOff_4);
					  highp vec4 tmpvar_8;
					  tmpvar_8.xzw = vec3(0.0, 0.0, 0.0);
					  tmpvar_8.y = (((_V_CW_Bend.x * xzOff_4.x) + (_V_CW_Bend.z * xzOff_4.y)) * 0.001);
					  worldPos_5 = tmpvar_8;
					  tmpvar_1 = (glstate_matrix_mvp * (_glesVertex + (_World2Object * tmpvar_8)));
					  tmpvar_2.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
					  tmpvar_2.xy = (tmpvar_2.xy + (_V_CW_MainTex_Scroll * _Time.x));
					  gl_Position = tmpvar_1;
					  xlv_TEXCOORD0 = tmpvar_2;
					  xlv_TEXCOORD1 = tmpvar_3;
					  xlv_TEXCOORD3 = ((tmpvar_1.z * unity_FogParams.z) + unity_FogParams.w);
					  xlv_COLOR = _glesColor;
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform lowp vec4 unity_FogColor;
					uniform lowp vec4 _Color;
					uniform sampler2D _MainTex;
					varying mediump vec4 xlv_TEXCOORD0;
					varying highp float xlv_TEXCOORD3;
					varying lowp vec4 xlv_COLOR;
					void main ()
					{
					  lowp vec4 retColor_1;
					  mediump vec4 mainTex_2;
					  lowp vec4 tmpvar_3;
					  tmpvar_3 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
					  mainTex_2 = tmpvar_3;
					  retColor_1 = mainTex_2;
					  retColor_1 = (retColor_1 * _Color);
					  retColor_1 = (retColor_1 * xlv_COLOR);
					  highp float tmpvar_4;
					  tmpvar_4 = clamp (xlv_TEXCOORD3, 0.0, 1.0);
					  retColor_1.xyz = mix (unity_FogColor.xyz, retColor_1.xyz, vec3(tmpvar_4));
					  retColor_1.w = 1.0;
					  gl_FragData[0] = retColor_1;
					}
					
					
					#endif"
}
SubProgram "gles " {
Keywords { "_EMISSION" "V_CW_VERTEX_COLOR" "V_CW_REFLECTIVE_OFF" "V_CW_RIM_OFF" "_NORMALMAP_OFF" "V_CW_FOG" "V_CW_IBL_OFF" "V_CW_MATCAP_BLEND_MULTIPLY" }
					"!!GLES
					#version 100
					
					#ifdef VERTEX
					attribute vec4 _glesVertex;
					attribute vec4 _glesColor;
					attribute vec4 _glesMultiTexCoord0;
					uniform highp vec4 _Time;
					uniform highp mat4 glstate_matrix_mvp;
					uniform highp mat4 _Object2World;
					uniform highp mat4 _World2Object;
					uniform highp vec3 _V_CW_Bend;
					uniform highp vec3 _V_CW_Bias;
					uniform highp vec4 _V_CW_PivotPoint_Position;
					uniform mediump vec4 _MainTex_ST;
					uniform lowp vec2 _V_CW_MainTex_Scroll;
					varying mediump vec4 xlv_TEXCOORD0;
					varying mediump vec4 xlv_TEXCOORD1;
					varying lowp vec4 xlv_COLOR;
					void main ()
					{
					  mediump vec4 tmpvar_1;
					  mediump vec4 tmpvar_2;
					  highp vec2 xzOff_3;
					  highp vec4 worldPos_4;
					  highp vec4 tmpvar_5;
					  tmpvar_5 = (_Object2World * _glesVertex);
					  worldPos_4.w = tmpvar_5.w;
					  worldPos_4.xyz = (tmpvar_5.xyz - _V_CW_PivotPoint_Position.xyz);
					  highp vec2 tmpvar_6;
					  tmpvar_6.x = float((worldPos_4.z >= 0.0));
					  tmpvar_6.y = float((worldPos_4.x >= 0.0));
					  xzOff_3 = (max (vec2(0.0, 0.0), (
					    abs(worldPos_4.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_6 * 2.0) - 1.0));
					  xzOff_3 = (xzOff_3 * xzOff_3);
					  highp vec4 tmpvar_7;
					  tmpvar_7.xzw = vec3(0.0, 0.0, 0.0);
					  tmpvar_7.y = (((_V_CW_Bend.x * xzOff_3.x) + (_V_CW_Bend.z * xzOff_3.y)) * 0.001);
					  worldPos_4 = tmpvar_7;
					  tmpvar_1.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
					  tmpvar_1.xy = (tmpvar_1.xy + (_V_CW_MainTex_Scroll * _Time.x));
					  gl_Position = (glstate_matrix_mvp * (_glesVertex + (_World2Object * tmpvar_7)));
					  xlv_TEXCOORD0 = tmpvar_1;
					  xlv_TEXCOORD1 = tmpvar_2;
					  xlv_COLOR = _glesColor;
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform lowp vec4 _Color;
					uniform sampler2D _MainTex;
					uniform sampler2D _EmissionMap;
					uniform mediump vec4 _EmissionColor;
					varying mediump vec4 xlv_TEXCOORD0;
					varying lowp vec4 xlv_COLOR;
					void main ()
					{
					  lowp vec4 retColor_1;
					  mediump vec4 mainTex_2;
					  lowp vec4 tmpvar_3;
					  tmpvar_3 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
					  mainTex_2 = tmpvar_3;
					  retColor_1 = mainTex_2;
					  retColor_1 = (retColor_1 * _Color);
					  retColor_1 = (retColor_1 * xlv_COLOR);
					  retColor_1.xyz = (retColor_1.xyz + (texture2D (_EmissionMap, xlv_TEXCOORD0.xy).xyz * _EmissionColor.xyz));
					  retColor_1.w = 1.0;
					  gl_FragData[0] = retColor_1;
					}
					
					
					#endif"
}
SubProgram "gles " {
Keywords { "FOG_LINEAR" "_EMISSION" "V_CW_VERTEX_COLOR" "V_CW_REFLECTIVE_OFF" "V_CW_RIM_OFF" "_NORMALMAP_OFF" "V_CW_FOG" "V_CW_IBL_OFF" "V_CW_MATCAP_BLEND_MULTIPLY" }
					"!!GLES
					#version 100
					
					#ifdef VERTEX
					attribute vec4 _glesVertex;
					attribute vec4 _glesColor;
					attribute vec4 _glesMultiTexCoord0;
					uniform highp vec4 _Time;
					uniform highp mat4 glstate_matrix_mvp;
					uniform highp mat4 _Object2World;
					uniform highp mat4 _World2Object;
					uniform highp vec4 unity_FogParams;
					uniform highp vec3 _V_CW_Bend;
					uniform highp vec3 _V_CW_Bias;
					uniform highp vec4 _V_CW_PivotPoint_Position;
					uniform mediump vec4 _MainTex_ST;
					uniform lowp vec2 _V_CW_MainTex_Scroll;
					varying mediump vec4 xlv_TEXCOORD0;
					varying mediump vec4 xlv_TEXCOORD1;
					varying highp float xlv_TEXCOORD3;
					varying lowp vec4 xlv_COLOR;
					void main ()
					{
					  highp vec4 tmpvar_1;
					  mediump vec4 tmpvar_2;
					  mediump vec4 tmpvar_3;
					  highp vec2 xzOff_4;
					  highp vec4 worldPos_5;
					  highp vec4 tmpvar_6;
					  tmpvar_6 = (_Object2World * _glesVertex);
					  worldPos_5.w = tmpvar_6.w;
					  worldPos_5.xyz = (tmpvar_6.xyz - _V_CW_PivotPoint_Position.xyz);
					  highp vec2 tmpvar_7;
					  tmpvar_7.x = float((worldPos_5.z >= 0.0));
					  tmpvar_7.y = float((worldPos_5.x >= 0.0));
					  xzOff_4 = (max (vec2(0.0, 0.0), (
					    abs(worldPos_5.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_7 * 2.0) - 1.0));
					  xzOff_4 = (xzOff_4 * xzOff_4);
					  highp vec4 tmpvar_8;
					  tmpvar_8.xzw = vec3(0.0, 0.0, 0.0);
					  tmpvar_8.y = (((_V_CW_Bend.x * xzOff_4.x) + (_V_CW_Bend.z * xzOff_4.y)) * 0.001);
					  worldPos_5 = tmpvar_8;
					  tmpvar_1 = (glstate_matrix_mvp * (_glesVertex + (_World2Object * tmpvar_8)));
					  tmpvar_2.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
					  tmpvar_2.xy = (tmpvar_2.xy + (_V_CW_MainTex_Scroll * _Time.x));
					  gl_Position = tmpvar_1;
					  xlv_TEXCOORD0 = tmpvar_2;
					  xlv_TEXCOORD1 = tmpvar_3;
					  xlv_TEXCOORD3 = ((tmpvar_1.z * unity_FogParams.z) + unity_FogParams.w);
					  xlv_COLOR = _glesColor;
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform lowp vec4 unity_FogColor;
					uniform lowp vec4 _Color;
					uniform sampler2D _MainTex;
					uniform sampler2D _EmissionMap;
					uniform mediump vec4 _EmissionColor;
					varying mediump vec4 xlv_TEXCOORD0;
					varying highp float xlv_TEXCOORD3;
					varying lowp vec4 xlv_COLOR;
					void main ()
					{
					  lowp vec4 retColor_1;
					  mediump vec4 mainTex_2;
					  lowp vec4 tmpvar_3;
					  tmpvar_3 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
					  mainTex_2 = tmpvar_3;
					  retColor_1 = mainTex_2;
					  retColor_1 = (retColor_1 * _Color);
					  retColor_1 = (retColor_1 * xlv_COLOR);
					  retColor_1.xyz = (retColor_1.xyz + (texture2D (_EmissionMap, xlv_TEXCOORD0.xy).xyz * _EmissionColor.xyz));
					  highp float tmpvar_4;
					  tmpvar_4 = clamp (xlv_TEXCOORD3, 0.0, 1.0);
					  retColor_1.xyz = mix (unity_FogColor.xyz, retColor_1.xyz, vec3(tmpvar_4));
					  retColor_1.w = 1.0;
					  gl_FragData[0] = retColor_1;
					}
					
					
					#endif"
}
SubProgram "gles " {
Keywords { "V_CW_VERTEX_COLOR_OFF" "_EMISSION_OFF" "_NORMALMAP_OFF" "V_CW_RIM" "V_CW_REFLECTIVE" "V_CW_FOG" "V_CW_IBL_OFF" "V_CW_MATCAP_BLEND_MULTIPLY" }
					"!!GLES
					#version 100
					
					#ifdef VERTEX
					attribute vec4 _glesTANGENT;
					attribute vec4 _glesVertex;
					attribute vec3 _glesNormal;
					attribute vec4 _glesMultiTexCoord0;
					uniform highp vec4 _Time;
					uniform highp vec3 _WorldSpaceCameraPos;
					uniform highp mat4 glstate_matrix_mvp;
					uniform highp mat4 _Object2World;
					uniform highp mat4 _World2Object;
					uniform highp vec3 _V_CW_Bend;
					uniform highp vec3 _V_CW_Bias;
					uniform highp vec4 _V_CW_PivotPoint_Position;
					uniform mediump vec4 _MainTex_ST;
					uniform lowp vec2 _V_CW_MainTex_Scroll;
					uniform lowp float _V_CW_Rim_Bias;
					varying mediump vec4 xlv_TEXCOORD0;
					varying mediump vec4 xlv_TEXCOORD1;
					varying mediump vec4 xlv_TEXCOORD2;
					void main ()
					{
					  mediump vec4 tmpvar_1;
					  mediump vec4 tmpvar_2;
					  mediump vec4 tmpvar_3;
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
					  tmpvar_1.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
					  tmpvar_1.xy = (tmpvar_1.xy + (_V_CW_MainTex_Scroll * _Time.x));
					  highp vec4 tmpvar_39;
					  tmpvar_39.w = 1.0;
					  tmpvar_39.xyz = _WorldSpaceCameraPos;
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
					  highp vec3 I_44;
					  I_44 = ((_Object2World * vertex_22).xyz - _WorldSpaceCameraPos);
					  highp vec3 tmpvar_45;
					  tmpvar_45 = normalize((I_44 - (2.0 * 
					    (dot (tmpvar_43, I_44) * tmpvar_43)
					  )));
					  tmpvar_3.xyz = tmpvar_45;
					  mediump float tmpvar_46;
					  highp float tmpvar_47;
					  tmpvar_47 = clamp ((dot (tmpvar_38, 
					    normalize(((_World2Object * tmpvar_39).xyz - vertex_22.xyz))
					  ) + _V_CW_Rim_Bias), 0.0, 1.0);
					  tmpvar_46 = tmpvar_47;
					  tmpvar_2.w = (tmpvar_46 * tmpvar_46);
					  gl_Position = (glstate_matrix_mvp * vertex_22);
					  xlv_TEXCOORD0 = tmpvar_1;
					  xlv_TEXCOORD1 = tmpvar_2;
					  xlv_TEXCOORD2 = tmpvar_3;
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform lowp vec4 _Color;
					uniform sampler2D _MainTex;
					uniform lowp samplerCube _V_CW_Cube;
					uniform lowp vec4 _V_CW_ReflectColor;
					uniform lowp float _V_CW_ReflectStrengthAlphaOffset;
					uniform lowp vec4 _V_CW_Rim_Color;
					varying mediump vec4 xlv_TEXCOORD0;
					varying mediump vec4 xlv_TEXCOORD1;
					varying mediump vec4 xlv_TEXCOORD2;
					void main ()
					{
					  lowp vec4 retColor_1;
					  mediump vec4 mainTex_2;
					  lowp vec4 tmpvar_3;
					  tmpvar_3 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
					  mainTex_2 = tmpvar_3;
					  retColor_1 = mainTex_2;
					  retColor_1 = (retColor_1 * _Color);
					  mediump float tmpvar_4;
					  tmpvar_4 = clamp ((mainTex_2.w + _V_CW_ReflectStrengthAlphaOffset), 0.0, 1.0);
					  retColor_1.xyz = (retColor_1.xyz + ((textureCube (_V_CW_Cube, xlv_TEXCOORD2.xyz) * _V_CW_ReflectColor).xyz * tmpvar_4));
					  mediump vec3 tmpvar_5;
					  tmpvar_5 = mix (_V_CW_Rim_Color.xyz, retColor_1.xyz, xlv_TEXCOORD1.www);
					  retColor_1.xyz = tmpvar_5;
					  retColor_1.w = 1.0;
					  gl_FragData[0] = retColor_1;
					}
					
					
					#endif"
}
SubProgram "gles " {
Keywords { "FOG_LINEAR" "V_CW_VERTEX_COLOR_OFF" "_EMISSION_OFF" "_NORMALMAP_OFF" "V_CW_RIM" "V_CW_REFLECTIVE" "V_CW_FOG" "V_CW_IBL_OFF" "V_CW_MATCAP_BLEND_MULTIPLY" }
					"!!GLES
					#version 100
					
					#ifdef VERTEX
					attribute vec4 _glesTANGENT;
					attribute vec4 _glesVertex;
					attribute vec3 _glesNormal;
					attribute vec4 _glesMultiTexCoord0;
					uniform highp vec4 _Time;
					uniform highp vec3 _WorldSpaceCameraPos;
					uniform highp mat4 glstate_matrix_mvp;
					uniform highp mat4 _Object2World;
					uniform highp mat4 _World2Object;
					uniform highp vec4 unity_FogParams;
					uniform highp vec3 _V_CW_Bend;
					uniform highp vec3 _V_CW_Bias;
					uniform highp vec4 _V_CW_PivotPoint_Position;
					uniform mediump vec4 _MainTex_ST;
					uniform lowp vec2 _V_CW_MainTex_Scroll;
					uniform lowp float _V_CW_Rim_Bias;
					varying mediump vec4 xlv_TEXCOORD0;
					varying mediump vec4 xlv_TEXCOORD1;
					varying mediump vec4 xlv_TEXCOORD2;
					varying highp float xlv_TEXCOORD3;
					void main ()
					{
					  highp vec4 tmpvar_1;
					  mediump vec4 tmpvar_2;
					  mediump vec4 tmpvar_3;
					  mediump vec4 tmpvar_4;
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
					  tmpvar_1 = (glstate_matrix_mvp * vertex_23);
					  tmpvar_2.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
					  tmpvar_2.xy = (tmpvar_2.xy + (_V_CW_MainTex_Scroll * _Time.x));
					  highp vec4 tmpvar_40;
					  tmpvar_40.w = 1.0;
					  tmpvar_40.xyz = _WorldSpaceCameraPos;
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
					  highp vec3 I_45;
					  I_45 = ((_Object2World * vertex_23).xyz - _WorldSpaceCameraPos);
					  highp vec3 tmpvar_46;
					  tmpvar_46 = normalize((I_45 - (2.0 * 
					    (dot (tmpvar_44, I_45) * tmpvar_44)
					  )));
					  tmpvar_4.xyz = tmpvar_46;
					  mediump float tmpvar_47;
					  highp float tmpvar_48;
					  tmpvar_48 = clamp ((dot (tmpvar_39, 
					    normalize(((_World2Object * tmpvar_40).xyz - vertex_23.xyz))
					  ) + _V_CW_Rim_Bias), 0.0, 1.0);
					  tmpvar_47 = tmpvar_48;
					  tmpvar_3.w = (tmpvar_47 * tmpvar_47);
					  gl_Position = tmpvar_1;
					  xlv_TEXCOORD0 = tmpvar_2;
					  xlv_TEXCOORD1 = tmpvar_3;
					  xlv_TEXCOORD2 = tmpvar_4;
					  xlv_TEXCOORD3 = ((tmpvar_1.z * unity_FogParams.z) + unity_FogParams.w);
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform lowp vec4 unity_FogColor;
					uniform lowp vec4 _Color;
					uniform sampler2D _MainTex;
					uniform lowp samplerCube _V_CW_Cube;
					uniform lowp vec4 _V_CW_ReflectColor;
					uniform lowp float _V_CW_ReflectStrengthAlphaOffset;
					uniform lowp vec4 _V_CW_Rim_Color;
					varying mediump vec4 xlv_TEXCOORD0;
					varying mediump vec4 xlv_TEXCOORD1;
					varying mediump vec4 xlv_TEXCOORD2;
					varying highp float xlv_TEXCOORD3;
					void main ()
					{
					  lowp vec4 retColor_1;
					  mediump vec4 mainTex_2;
					  lowp vec4 tmpvar_3;
					  tmpvar_3 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
					  mainTex_2 = tmpvar_3;
					  retColor_1 = mainTex_2;
					  retColor_1 = (retColor_1 * _Color);
					  mediump float tmpvar_4;
					  tmpvar_4 = clamp ((mainTex_2.w + _V_CW_ReflectStrengthAlphaOffset), 0.0, 1.0);
					  retColor_1.xyz = (retColor_1.xyz + ((textureCube (_V_CW_Cube, xlv_TEXCOORD2.xyz) * _V_CW_ReflectColor).xyz * tmpvar_4));
					  mediump vec3 tmpvar_5;
					  tmpvar_5 = mix (_V_CW_Rim_Color.xyz, retColor_1.xyz, xlv_TEXCOORD1.www);
					  retColor_1.xyz = tmpvar_5;
					  highp float tmpvar_6;
					  tmpvar_6 = clamp (xlv_TEXCOORD3, 0.0, 1.0);
					  retColor_1.xyz = mix (unity_FogColor.xyz, retColor_1.xyz, vec3(tmpvar_6));
					  retColor_1.w = 1.0;
					  gl_FragData[0] = retColor_1;
					}
					
					
					#endif"
}
SubProgram "gles " {
Keywords { "V_CW_VERTEX_COLOR" "_EMISSION_OFF" "V_CW_RIM_OFF" "_NORMALMAP_OFF" "V_CW_REFLECTIVE" "V_CW_FOG_OFF" "V_CW_IBL_OFF" "V_CW_MATCAP_BLEND_MULTIPLY" }
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
					uniform highp mat4 glstate_matrix_mvp;
					uniform highp mat4 _Object2World;
					uniform highp mat4 _World2Object;
					uniform highp vec3 _V_CW_Bend;
					uniform highp vec3 _V_CW_Bias;
					uniform highp vec4 _V_CW_PivotPoint_Position;
					uniform mediump vec4 _MainTex_ST;
					uniform lowp vec2 _V_CW_MainTex_Scroll;
					varying mediump vec4 xlv_TEXCOORD0;
					varying mediump vec4 xlv_TEXCOORD1;
					varying mediump vec4 xlv_TEXCOORD2;
					varying lowp vec4 xlv_COLOR;
					void main ()
					{
					  mediump vec4 tmpvar_1;
					  mediump vec4 tmpvar_2;
					  mediump vec4 tmpvar_3;
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
					  tmpvar_1.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
					  tmpvar_1.xy = (tmpvar_1.xy + (_V_CW_MainTex_Scroll * _Time.x));
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
					  highp vec3 I_43;
					  I_43 = ((_Object2World * vertex_22).xyz - _WorldSpaceCameraPos);
					  highp vec3 tmpvar_44;
					  tmpvar_44 = normalize((I_43 - (2.0 * 
					    (dot (tmpvar_42, I_43) * tmpvar_42)
					  )));
					  tmpvar_3.xyz = tmpvar_44;
					  gl_Position = (glstate_matrix_mvp * vertex_22);
					  xlv_TEXCOORD0 = tmpvar_1;
					  xlv_TEXCOORD1 = tmpvar_2;
					  xlv_TEXCOORD2 = tmpvar_3;
					  xlv_COLOR = _glesColor;
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform lowp vec4 _Color;
					uniform sampler2D _MainTex;
					uniform lowp samplerCube _V_CW_Cube;
					uniform lowp vec4 _V_CW_ReflectColor;
					uniform lowp float _V_CW_ReflectStrengthAlphaOffset;
					varying mediump vec4 xlv_TEXCOORD0;
					varying mediump vec4 xlv_TEXCOORD2;
					varying lowp vec4 xlv_COLOR;
					void main ()
					{
					  lowp vec4 retColor_1;
					  mediump vec4 mainTex_2;
					  lowp vec4 tmpvar_3;
					  tmpvar_3 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
					  mainTex_2 = tmpvar_3;
					  retColor_1 = mainTex_2;
					  retColor_1 = (retColor_1 * _Color);
					  retColor_1 = (retColor_1 * xlv_COLOR);
					  mediump float tmpvar_4;
					  tmpvar_4 = clamp ((mainTex_2.w + _V_CW_ReflectStrengthAlphaOffset), 0.0, 1.0);
					  retColor_1.xyz = (retColor_1.xyz + ((textureCube (_V_CW_Cube, xlv_TEXCOORD2.xyz) * _V_CW_ReflectColor).xyz * tmpvar_4));
					  retColor_1.w = 1.0;
					  gl_FragData[0] = retColor_1;
					}
					
					
					#endif"
}
SubProgram "gles " {
Keywords { "FOG_LINEAR" "V_CW_VERTEX_COLOR" "_EMISSION_OFF" "V_CW_RIM_OFF" "_NORMALMAP_OFF" "V_CW_REFLECTIVE" "V_CW_FOG_OFF" "V_CW_IBL_OFF" "V_CW_MATCAP_BLEND_MULTIPLY" }
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
					uniform highp mat4 glstate_matrix_mvp;
					uniform highp mat4 _Object2World;
					uniform highp mat4 _World2Object;
					uniform highp vec3 _V_CW_Bend;
					uniform highp vec3 _V_CW_Bias;
					uniform highp vec4 _V_CW_PivotPoint_Position;
					uniform mediump vec4 _MainTex_ST;
					uniform lowp vec2 _V_CW_MainTex_Scroll;
					varying mediump vec4 xlv_TEXCOORD0;
					varying mediump vec4 xlv_TEXCOORD1;
					varying mediump vec4 xlv_TEXCOORD2;
					varying highp float xlv_TEXCOORD3;
					varying lowp vec4 xlv_COLOR;
					void main ()
					{
					  mediump vec4 tmpvar_1;
					  mediump vec4 tmpvar_2;
					  mediump vec4 tmpvar_3;
					  highp float tmpvar_4;
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
					  tmpvar_1.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
					  tmpvar_1.xy = (tmpvar_1.xy + (_V_CW_MainTex_Scroll * _Time.x));
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
					  highp vec3 I_44;
					  I_44 = ((_Object2World * vertex_23).xyz - _WorldSpaceCameraPos);
					  highp vec3 tmpvar_45;
					  tmpvar_45 = normalize((I_44 - (2.0 * 
					    (dot (tmpvar_43, I_44) * tmpvar_43)
					  )));
					  tmpvar_3.xyz = tmpvar_45;
					  gl_Position = (glstate_matrix_mvp * vertex_23);
					  xlv_TEXCOORD0 = tmpvar_1;
					  xlv_TEXCOORD1 = tmpvar_2;
					  xlv_TEXCOORD2 = tmpvar_3;
					  xlv_TEXCOORD3 = tmpvar_4;
					  xlv_COLOR = _glesColor;
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform lowp vec4 _Color;
					uniform sampler2D _MainTex;
					uniform lowp samplerCube _V_CW_Cube;
					uniform lowp vec4 _V_CW_ReflectColor;
					uniform lowp float _V_CW_ReflectStrengthAlphaOffset;
					varying mediump vec4 xlv_TEXCOORD0;
					varying mediump vec4 xlv_TEXCOORD2;
					varying lowp vec4 xlv_COLOR;
					void main ()
					{
					  lowp vec4 retColor_1;
					  mediump vec4 mainTex_2;
					  lowp vec4 tmpvar_3;
					  tmpvar_3 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
					  mainTex_2 = tmpvar_3;
					  retColor_1 = mainTex_2;
					  retColor_1 = (retColor_1 * _Color);
					  retColor_1 = (retColor_1 * xlv_COLOR);
					  mediump float tmpvar_4;
					  tmpvar_4 = clamp ((mainTex_2.w + _V_CW_ReflectStrengthAlphaOffset), 0.0, 1.0);
					  retColor_1.xyz = (retColor_1.xyz + ((textureCube (_V_CW_Cube, xlv_TEXCOORD2.xyz) * _V_CW_ReflectColor).xyz * tmpvar_4));
					  retColor_1.w = 1.0;
					  gl_FragData[0] = retColor_1;
					}
					
					
					#endif"
}
SubProgram "gles " {
Keywords { "V_CW_VERTEX_COLOR" "_EMISSION_OFF" "V_CW_RIM_OFF" "_NORMALMAP_OFF" "V_CW_REFLECTIVE" "V_CW_FOG" "V_CW_IBL_OFF" "V_CW_MATCAP_BLEND_MULTIPLY" }
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
					uniform highp mat4 glstate_matrix_mvp;
					uniform highp mat4 _Object2World;
					uniform highp mat4 _World2Object;
					uniform highp vec3 _V_CW_Bend;
					uniform highp vec3 _V_CW_Bias;
					uniform highp vec4 _V_CW_PivotPoint_Position;
					uniform mediump vec4 _MainTex_ST;
					uniform lowp vec2 _V_CW_MainTex_Scroll;
					varying mediump vec4 xlv_TEXCOORD0;
					varying mediump vec4 xlv_TEXCOORD1;
					varying mediump vec4 xlv_TEXCOORD2;
					varying lowp vec4 xlv_COLOR;
					void main ()
					{
					  mediump vec4 tmpvar_1;
					  mediump vec4 tmpvar_2;
					  mediump vec4 tmpvar_3;
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
					  tmpvar_1.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
					  tmpvar_1.xy = (tmpvar_1.xy + (_V_CW_MainTex_Scroll * _Time.x));
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
					  highp vec3 I_43;
					  I_43 = ((_Object2World * vertex_22).xyz - _WorldSpaceCameraPos);
					  highp vec3 tmpvar_44;
					  tmpvar_44 = normalize((I_43 - (2.0 * 
					    (dot (tmpvar_42, I_43) * tmpvar_42)
					  )));
					  tmpvar_3.xyz = tmpvar_44;
					  gl_Position = (glstate_matrix_mvp * vertex_22);
					  xlv_TEXCOORD0 = tmpvar_1;
					  xlv_TEXCOORD1 = tmpvar_2;
					  xlv_TEXCOORD2 = tmpvar_3;
					  xlv_COLOR = _glesColor;
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform lowp vec4 _Color;
					uniform sampler2D _MainTex;
					uniform lowp samplerCube _V_CW_Cube;
					uniform lowp vec4 _V_CW_ReflectColor;
					uniform lowp float _V_CW_ReflectStrengthAlphaOffset;
					varying mediump vec4 xlv_TEXCOORD0;
					varying mediump vec4 xlv_TEXCOORD2;
					varying lowp vec4 xlv_COLOR;
					void main ()
					{
					  lowp vec4 retColor_1;
					  mediump vec4 mainTex_2;
					  lowp vec4 tmpvar_3;
					  tmpvar_3 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
					  mainTex_2 = tmpvar_3;
					  retColor_1 = mainTex_2;
					  retColor_1 = (retColor_1 * _Color);
					  retColor_1 = (retColor_1 * xlv_COLOR);
					  mediump float tmpvar_4;
					  tmpvar_4 = clamp ((mainTex_2.w + _V_CW_ReflectStrengthAlphaOffset), 0.0, 1.0);
					  retColor_1.xyz = (retColor_1.xyz + ((textureCube (_V_CW_Cube, xlv_TEXCOORD2.xyz) * _V_CW_ReflectColor).xyz * tmpvar_4));
					  retColor_1.w = 1.0;
					  gl_FragData[0] = retColor_1;
					}
					
					
					#endif"
}
SubProgram "gles " {
Keywords { "FOG_LINEAR" "V_CW_VERTEX_COLOR" "_EMISSION_OFF" "V_CW_RIM_OFF" "_NORMALMAP_OFF" "V_CW_REFLECTIVE" "V_CW_FOG" "V_CW_IBL_OFF" "V_CW_MATCAP_BLEND_MULTIPLY" }
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
					uniform highp mat4 glstate_matrix_mvp;
					uniform highp mat4 _Object2World;
					uniform highp mat4 _World2Object;
					uniform highp vec4 unity_FogParams;
					uniform highp vec3 _V_CW_Bend;
					uniform highp vec3 _V_CW_Bias;
					uniform highp vec4 _V_CW_PivotPoint_Position;
					uniform mediump vec4 _MainTex_ST;
					uniform lowp vec2 _V_CW_MainTex_Scroll;
					varying mediump vec4 xlv_TEXCOORD0;
					varying mediump vec4 xlv_TEXCOORD1;
					varying mediump vec4 xlv_TEXCOORD2;
					varying highp float xlv_TEXCOORD3;
					varying lowp vec4 xlv_COLOR;
					void main ()
					{
					  highp vec4 tmpvar_1;
					  mediump vec4 tmpvar_2;
					  mediump vec4 tmpvar_3;
					  mediump vec4 tmpvar_4;
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
					  tmpvar_1 = (glstate_matrix_mvp * vertex_23);
					  tmpvar_2.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
					  tmpvar_2.xy = (tmpvar_2.xy + (_V_CW_MainTex_Scroll * _Time.x));
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
					  highp vec3 I_44;
					  I_44 = ((_Object2World * vertex_23).xyz - _WorldSpaceCameraPos);
					  highp vec3 tmpvar_45;
					  tmpvar_45 = normalize((I_44 - (2.0 * 
					    (dot (tmpvar_43, I_44) * tmpvar_43)
					  )));
					  tmpvar_4.xyz = tmpvar_45;
					  gl_Position = tmpvar_1;
					  xlv_TEXCOORD0 = tmpvar_2;
					  xlv_TEXCOORD1 = tmpvar_3;
					  xlv_TEXCOORD2 = tmpvar_4;
					  xlv_TEXCOORD3 = ((tmpvar_1.z * unity_FogParams.z) + unity_FogParams.w);
					  xlv_COLOR = _glesColor;
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform lowp vec4 unity_FogColor;
					uniform lowp vec4 _Color;
					uniform sampler2D _MainTex;
					uniform lowp samplerCube _V_CW_Cube;
					uniform lowp vec4 _V_CW_ReflectColor;
					uniform lowp float _V_CW_ReflectStrengthAlphaOffset;
					varying mediump vec4 xlv_TEXCOORD0;
					varying mediump vec4 xlv_TEXCOORD2;
					varying highp float xlv_TEXCOORD3;
					varying lowp vec4 xlv_COLOR;
					void main ()
					{
					  lowp vec4 retColor_1;
					  mediump vec4 mainTex_2;
					  lowp vec4 tmpvar_3;
					  tmpvar_3 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
					  mainTex_2 = tmpvar_3;
					  retColor_1 = mainTex_2;
					  retColor_1 = (retColor_1 * _Color);
					  retColor_1 = (retColor_1 * xlv_COLOR);
					  mediump float tmpvar_4;
					  tmpvar_4 = clamp ((mainTex_2.w + _V_CW_ReflectStrengthAlphaOffset), 0.0, 1.0);
					  retColor_1.xyz = (retColor_1.xyz + ((textureCube (_V_CW_Cube, xlv_TEXCOORD2.xyz) * _V_CW_ReflectColor).xyz * tmpvar_4));
					  highp float tmpvar_5;
					  tmpvar_5 = clamp (xlv_TEXCOORD3, 0.0, 1.0);
					  retColor_1.xyz = mix (unity_FogColor.xyz, retColor_1.xyz, vec3(tmpvar_5));
					  retColor_1.w = 1.0;
					  gl_FragData[0] = retColor_1;
					}
					
					
					#endif"
}
SubProgram "gles " {
Keywords { "V_CW_VERTEX_COLOR" "_NORMALMAP" "_EMISSION_OFF" "V_CW_RIM_OFF" "V_CW_REFLECTIVE" "V_CW_FOG" "V_CW_IBL_OFF" "V_CW_MATCAP_BLEND_MULTIPLY" }
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
					uniform highp mat4 glstate_matrix_mvp;
					uniform highp mat4 _Object2World;
					uniform highp mat4 _World2Object;
					uniform highp vec3 _V_CW_Bend;
					uniform highp vec3 _V_CW_Bias;
					uniform highp vec4 _V_CW_PivotPoint_Position;
					uniform mediump vec4 _MainTex_ST;
					uniform lowp vec2 _V_CW_MainTex_Scroll;
					varying mediump vec4 xlv_TEXCOORD0;
					varying mediump vec4 xlv_TEXCOORD1;
					varying mediump vec4 xlv_TEXCOORD2;
					varying lowp vec4 xlv_COLOR;
					void main ()
					{
					  mediump vec4 tmpvar_1;
					  mediump vec4 tmpvar_2;
					  mediump vec4 tmpvar_3;
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
					  tmpvar_1.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
					  tmpvar_1.xy = (tmpvar_1.xy + (_V_CW_MainTex_Scroll * _Time.x));
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
					  highp vec3 I_43;
					  I_43 = ((_Object2World * vertex_22).xyz - _WorldSpaceCameraPos);
					  highp vec3 tmpvar_44;
					  tmpvar_44 = normalize((I_43 - (2.0 * 
					    (dot (tmpvar_42, I_43) * tmpvar_42)
					  )));
					  tmpvar_3.xyz = tmpvar_44;
					  gl_Position = (glstate_matrix_mvp * vertex_22);
					  xlv_TEXCOORD0 = tmpvar_1;
					  xlv_TEXCOORD1 = tmpvar_2;
					  xlv_TEXCOORD2 = tmpvar_3;
					  xlv_COLOR = _glesColor;
					}
					
					
					#endif
					#ifdef FRAGMENT
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
					varying lowp vec4 xlv_COLOR;
					void main ()
					{
					  lowp vec3 bumpNormal_1;
					  lowp vec4 retColor_2;
					  mediump vec4 mainTex_3;
					  lowp vec4 tmpvar_4;
					  tmpvar_4 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
					  mainTex_3 = tmpvar_4;
					  retColor_2 = mainTex_3;
					  retColor_2 = (retColor_2 * _Color);
					  retColor_2 = (retColor_2 * xlv_COLOR);
					  mediump vec2 P_5;
					  P_5 = (xlv_TEXCOORD0.xy * _V_CW_NormalMap_UV_Scale);
					  lowp vec3 normal_6;
					  normal_6.xy = ((texture2D (_V_CW_NormalMap, P_5).wy * 2.0) - 1.0);
					  normal_6.z = sqrt((1.0 - clamp (
					    dot (normal_6.xy, normal_6.xy)
					  , 0.0, 1.0)));
					  mediump vec3 tmpvar_7;
					  tmpvar_7.xy = (normal_6.xy * vec2(_V_CW_NormalMapStrength));
					  tmpvar_7.z = normal_6.z;
					  mediump vec3 tmpvar_8;
					  tmpvar_8 = normalize(tmpvar_7);
					  bumpNormal_1 = tmpvar_8;
					  mediump vec3 P_9;
					  P_9 = (xlv_TEXCOORD2.xyz + bumpNormal_1);
					  mediump float tmpvar_10;
					  tmpvar_10 = clamp ((mainTex_3.w + _V_CW_ReflectStrengthAlphaOffset), 0.0, 1.0);
					  retColor_2.xyz = (retColor_2.xyz + ((textureCube (_V_CW_Cube, P_9) * _V_CW_ReflectColor).xyz * tmpvar_10));
					  retColor_2.w = 1.0;
					  gl_FragData[0] = retColor_2;
					}
					
					
					#endif"
}
SubProgram "gles " {
Keywords { "FOG_LINEAR" "V_CW_VERTEX_COLOR" "_NORMALMAP" "_EMISSION_OFF" "V_CW_RIM_OFF" "V_CW_REFLECTIVE" "V_CW_FOG" "V_CW_IBL_OFF" "V_CW_MATCAP_BLEND_MULTIPLY" }
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
					uniform highp mat4 glstate_matrix_mvp;
					uniform highp mat4 _Object2World;
					uniform highp mat4 _World2Object;
					uniform highp vec4 unity_FogParams;
					uniform highp vec3 _V_CW_Bend;
					uniform highp vec3 _V_CW_Bias;
					uniform highp vec4 _V_CW_PivotPoint_Position;
					uniform mediump vec4 _MainTex_ST;
					uniform lowp vec2 _V_CW_MainTex_Scroll;
					varying mediump vec4 xlv_TEXCOORD0;
					varying mediump vec4 xlv_TEXCOORD1;
					varying mediump vec4 xlv_TEXCOORD2;
					varying highp float xlv_TEXCOORD3;
					varying lowp vec4 xlv_COLOR;
					void main ()
					{
					  highp vec4 tmpvar_1;
					  mediump vec4 tmpvar_2;
					  mediump vec4 tmpvar_3;
					  mediump vec4 tmpvar_4;
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
					  tmpvar_1 = (glstate_matrix_mvp * vertex_23);
					  tmpvar_2.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
					  tmpvar_2.xy = (tmpvar_2.xy + (_V_CW_MainTex_Scroll * _Time.x));
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
					  highp vec3 I_44;
					  I_44 = ((_Object2World * vertex_23).xyz - _WorldSpaceCameraPos);
					  highp vec3 tmpvar_45;
					  tmpvar_45 = normalize((I_44 - (2.0 * 
					    (dot (tmpvar_43, I_44) * tmpvar_43)
					  )));
					  tmpvar_4.xyz = tmpvar_45;
					  gl_Position = tmpvar_1;
					  xlv_TEXCOORD0 = tmpvar_2;
					  xlv_TEXCOORD1 = tmpvar_3;
					  xlv_TEXCOORD2 = tmpvar_4;
					  xlv_TEXCOORD3 = ((tmpvar_1.z * unity_FogParams.z) + unity_FogParams.w);
					  xlv_COLOR = _glesColor;
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform lowp vec4 unity_FogColor;
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
					varying highp float xlv_TEXCOORD3;
					varying lowp vec4 xlv_COLOR;
					void main ()
					{
					  lowp vec3 bumpNormal_1;
					  lowp vec4 retColor_2;
					  mediump vec4 mainTex_3;
					  lowp vec4 tmpvar_4;
					  tmpvar_4 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
					  mainTex_3 = tmpvar_4;
					  retColor_2 = mainTex_3;
					  retColor_2 = (retColor_2 * _Color);
					  retColor_2 = (retColor_2 * xlv_COLOR);
					  mediump vec2 P_5;
					  P_5 = (xlv_TEXCOORD0.xy * _V_CW_NormalMap_UV_Scale);
					  lowp vec3 normal_6;
					  normal_6.xy = ((texture2D (_V_CW_NormalMap, P_5).wy * 2.0) - 1.0);
					  normal_6.z = sqrt((1.0 - clamp (
					    dot (normal_6.xy, normal_6.xy)
					  , 0.0, 1.0)));
					  mediump vec3 tmpvar_7;
					  tmpvar_7.xy = (normal_6.xy * vec2(_V_CW_NormalMapStrength));
					  tmpvar_7.z = normal_6.z;
					  mediump vec3 tmpvar_8;
					  tmpvar_8 = normalize(tmpvar_7);
					  bumpNormal_1 = tmpvar_8;
					  mediump vec3 P_9;
					  P_9 = (xlv_TEXCOORD2.xyz + bumpNormal_1);
					  mediump float tmpvar_10;
					  tmpvar_10 = clamp ((mainTex_3.w + _V_CW_ReflectStrengthAlphaOffset), 0.0, 1.0);
					  retColor_2.xyz = (retColor_2.xyz + ((textureCube (_V_CW_Cube, P_9) * _V_CW_ReflectColor).xyz * tmpvar_10));
					  highp float tmpvar_11;
					  tmpvar_11 = clamp (xlv_TEXCOORD3, 0.0, 1.0);
					  retColor_2.xyz = mix (unity_FogColor.xyz, retColor_2.xyz, vec3(tmpvar_11));
					  retColor_2.w = 1.0;
					  gl_FragData[0] = retColor_2;
					}
					
					
					#endif"
}
}
Program "fp" {
SubProgram "gles " {
Keywords { "V_CW_REFLECTIVE_OFF" "V_CW_VERTEX_COLOR_OFF" "_EMISSION_OFF" "V_CW_RIM_OFF" "_NORMALMAP_OFF" "V_CW_FOG_OFF" "V_CW_IBL_OFF" "V_CW_MATCAP_BLEND_MULTIPLY" }
					"!!GLES"
}
SubProgram "gles " {
Keywords { "FOG_LINEAR" "V_CW_REFLECTIVE_OFF" "V_CW_VERTEX_COLOR_OFF" "_EMISSION_OFF" "V_CW_RIM_OFF" "_NORMALMAP_OFF" "V_CW_FOG_OFF" "V_CW_IBL_OFF" "V_CW_MATCAP_BLEND_MULTIPLY" }
					"!!GLES"
}
SubProgram "gles " {
Keywords { "V_CW_REFLECTIVE_OFF" "V_CW_VERTEX_COLOR_OFF" "_EMISSION_OFF" "V_CW_RIM_OFF" "_NORMALMAP_OFF" "V_CW_FOG" "V_CW_IBL_OFF" "V_CW_MATCAP_BLEND_MULTIPLY" }
					"!!GLES"
}
SubProgram "gles " {
Keywords { "FOG_LINEAR" "V_CW_REFLECTIVE_OFF" "V_CW_VERTEX_COLOR_OFF" "_EMISSION_OFF" "V_CW_RIM_OFF" "_NORMALMAP_OFF" "V_CW_FOG" "V_CW_IBL_OFF" "V_CW_MATCAP_BLEND_MULTIPLY" }
					"!!GLES"
}
SubProgram "gles " {
Keywords { "V_CW_VERTEX_COLOR" "V_CW_REFLECTIVE_OFF" "_EMISSION_OFF" "V_CW_RIM_OFF" "_NORMALMAP_OFF" "V_CW_FOG_OFF" "V_CW_IBL_OFF" "V_CW_MATCAP_BLEND_MULTIPLY" }
					"!!GLES"
}
SubProgram "gles " {
Keywords { "FOG_LINEAR" "V_CW_VERTEX_COLOR" "V_CW_REFLECTIVE_OFF" "_EMISSION_OFF" "V_CW_RIM_OFF" "_NORMALMAP_OFF" "V_CW_FOG_OFF" "V_CW_IBL_OFF" "V_CW_MATCAP_BLEND_MULTIPLY" }
					"!!GLES"
}
SubProgram "gles " {
Keywords { "V_CW_VERTEX_COLOR" "V_CW_REFLECTIVE_OFF" "_EMISSION_OFF" "V_CW_RIM_OFF" "_NORMALMAP_OFF" "V_CW_FOG" "V_CW_IBL_OFF" "V_CW_MATCAP_BLEND_MULTIPLY" }
					"!!GLES"
}
SubProgram "gles " {
Keywords { "FOG_LINEAR" "V_CW_VERTEX_COLOR" "V_CW_REFLECTIVE_OFF" "_EMISSION_OFF" "V_CW_RIM_OFF" "_NORMALMAP_OFF" "V_CW_FOG" "V_CW_IBL_OFF" "V_CW_MATCAP_BLEND_MULTIPLY" }
					"!!GLES"
}
SubProgram "gles " {
Keywords { "_EMISSION" "V_CW_VERTEX_COLOR" "V_CW_REFLECTIVE_OFF" "V_CW_RIM_OFF" "_NORMALMAP_OFF" "V_CW_FOG" "V_CW_IBL_OFF" "V_CW_MATCAP_BLEND_MULTIPLY" }
					"!!GLES"
}
SubProgram "gles " {
Keywords { "FOG_LINEAR" "_EMISSION" "V_CW_VERTEX_COLOR" "V_CW_REFLECTIVE_OFF" "V_CW_RIM_OFF" "_NORMALMAP_OFF" "V_CW_FOG" "V_CW_IBL_OFF" "V_CW_MATCAP_BLEND_MULTIPLY" }
					"!!GLES"
}
SubProgram "gles " {
Keywords { "V_CW_VERTEX_COLOR_OFF" "_EMISSION_OFF" "_NORMALMAP_OFF" "V_CW_RIM" "V_CW_REFLECTIVE" "V_CW_FOG" "V_CW_IBL_OFF" "V_CW_MATCAP_BLEND_MULTIPLY" }
					"!!GLES"
}
SubProgram "gles " {
Keywords { "FOG_LINEAR" "V_CW_VERTEX_COLOR_OFF" "_EMISSION_OFF" "_NORMALMAP_OFF" "V_CW_RIM" "V_CW_REFLECTIVE" "V_CW_FOG" "V_CW_IBL_OFF" "V_CW_MATCAP_BLEND_MULTIPLY" }
					"!!GLES"
}
SubProgram "gles " {
Keywords { "V_CW_VERTEX_COLOR" "_EMISSION_OFF" "V_CW_RIM_OFF" "_NORMALMAP_OFF" "V_CW_REFLECTIVE" "V_CW_FOG_OFF" "V_CW_IBL_OFF" "V_CW_MATCAP_BLEND_MULTIPLY" }
					"!!GLES"
}
SubProgram "gles " {
Keywords { "FOG_LINEAR" "V_CW_VERTEX_COLOR" "_EMISSION_OFF" "V_CW_RIM_OFF" "_NORMALMAP_OFF" "V_CW_REFLECTIVE" "V_CW_FOG_OFF" "V_CW_IBL_OFF" "V_CW_MATCAP_BLEND_MULTIPLY" }
					"!!GLES"
}
SubProgram "gles " {
Keywords { "V_CW_VERTEX_COLOR" "_EMISSION_OFF" "V_CW_RIM_OFF" "_NORMALMAP_OFF" "V_CW_REFLECTIVE" "V_CW_FOG" "V_CW_IBL_OFF" "V_CW_MATCAP_BLEND_MULTIPLY" }
					"!!GLES"
}
SubProgram "gles " {
Keywords { "FOG_LINEAR" "V_CW_VERTEX_COLOR" "_EMISSION_OFF" "V_CW_RIM_OFF" "_NORMALMAP_OFF" "V_CW_REFLECTIVE" "V_CW_FOG" "V_CW_IBL_OFF" "V_CW_MATCAP_BLEND_MULTIPLY" }
					"!!GLES"
}
SubProgram "gles " {
Keywords { "V_CW_VERTEX_COLOR" "_NORMALMAP" "_EMISSION_OFF" "V_CW_RIM_OFF" "V_CW_REFLECTIVE" "V_CW_FOG" "V_CW_IBL_OFF" "V_CW_MATCAP_BLEND_MULTIPLY" }
					"!!GLES"
}
SubProgram "gles " {
Keywords { "FOG_LINEAR" "V_CW_VERTEX_COLOR" "_NORMALMAP" "_EMISSION_OFF" "V_CW_RIM_OFF" "V_CW_REFLECTIVE" "V_CW_FOG" "V_CW_IBL_OFF" "V_CW_MATCAP_BLEND_MULTIPLY" }
					"!!GLES"
}
}
 }
}
}