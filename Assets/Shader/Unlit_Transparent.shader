Shader "Hidden/VacuumShaders/Curved World/Unlit/Transparent/Texture" {
Properties {
[CurvedWorldGearMenu]  V_CW_Label_Tag ("", Float) = 0
[CurvedWorldLabel]  V_CW_Label_UnityDefaults ("Default Visual Options", Float) = 0
[CurvedWorldLargeLabel]  V_CW_Label_Modes ("Modes", Float) = 0
[CurvedWorldRenderingMode]  V_CW_Rendering_Mode ("  Rendering", Float) = 0
[CurvedWorldTextureMixMode]  V_CW_Texture_Mix_Mode ("  Texture Mix", Float) = 0
[CurvedWorldLargeLabel]  V_CW_Label_Albedo ("Albedo", Float) = 0
 _Color ("  Color", Color) = (1,1,1,1)
 _MainTex ("  Map (RGB) RefStr & Trans (A)", 2D) = "white" { }
[CurvedWorldUVScroll]  _V_CW_MainTex_Scroll ("    ", Vector) = (0,0,0,0)
[CurvedWorldLabel]  V_CW_CW_OPTIONS ("Curved World Optionals", Float) = 0
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
 Tags { "QUEUE"="Transparent+1" "IGNOREPROJECTOR"="true" "RenderType"="Transparent" "CurvedWorldTag"="Unlit/Transparent/Texture" "CurvedWorldNoneRemoveableKeywords"="" "CurvedWorldAvailableOptions"="V_CW_REFLECTIVE;V_CW_VERTEX_COLOR;V_CW_IBL;_EMISSION;V_CW_RIM;V_CW_FOG;_NORMALMAP;" }
 Pass {
  Name "BASE"
  Tags { "QUEUE"="Transparent+1" "IGNOREPROJECTOR"="true" "RenderType"="Transparent" "CurvedWorldTag"="Unlit/Transparent/Texture" "CurvedWorldNoneRemoveableKeywords"="" "CurvedWorldAvailableOptions"="V_CW_REFLECTIVE;V_CW_VERTEX_COLOR;V_CW_IBL;_EMISSION;V_CW_RIM;V_CW_FOG;_NORMALMAP;" }
  ZWrite Off
  Blend SrcAlpha OneMinusSrcAlpha
  GpuProgramID 36064
Program "vp" {
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
					  retColor_1 = (retColor_1 * xlv_COLOR);
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
					  gl_FragData[0] = retColor_1;
					}
					
					
					#endif"
}
}
Program "fp" {
SubProgram "gles " {
Keywords { "V_CW_VERTEX_COLOR" "V_CW_REFLECTIVE_OFF" "_EMISSION_OFF" "V_CW_RIM_OFF" "_NORMALMAP_OFF" "V_CW_FOG" "V_CW_IBL_OFF" "V_CW_MATCAP_BLEND_MULTIPLY" }
					"!!GLES"
}
SubProgram "gles " {
Keywords { "FOG_LINEAR" "V_CW_VERTEX_COLOR" "V_CW_REFLECTIVE_OFF" "_EMISSION_OFF" "V_CW_RIM_OFF" "_NORMALMAP_OFF" "V_CW_FOG" "V_CW_IBL_OFF" "V_CW_MATCAP_BLEND_MULTIPLY" }
					"!!GLES"
}
}
 }
}
}