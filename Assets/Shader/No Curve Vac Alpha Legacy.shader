Shader "Custom/City/No Curve Vac Legacy Alpha" {
Properties {
 _Color ("Main Color", Color) = (1,1,1,1)
 _SpecColor ("Specular Color", Color) = (0.5,0.5,0.5,0)
 _Shininess ("Shininess", Range(0.01,1)) = 0.078125
 _MainTex ("Base (RGB) TransGloss (A)", 2D) = "white" { }
}
SubShader { 
 LOD 300
 Tags { "QUEUE"="Transparent" "IGNOREPROJECTOR"="true" "RenderType"="Transparent" }
 Pass {
  Name "FORWARD"
  Tags { "LIGHTMODE"="ForwardBase" "QUEUE"="Transparent" "IGNOREPROJECTOR"="true" "RenderType"="Transparent" }
  ZWrite Off
  Blend SrcAlpha OneMinusSrcAlpha
  ColorMask RGB
  GpuProgramID 9637
Program "vp" {
SubProgram "gles " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" }
					"!!GLES
					#version 100
					
					#ifdef VERTEX
					attribute vec4 _glesVertex;
					attribute vec4 _glesColor;
					attribute vec3 _glesNormal;
					attribute vec4 _glesMultiTexCoord0;
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
					  v_3.x = _World2Object[0].x;
					  v_3.y = _World2Object[1].x;
					  v_3.z = _World2Object[2].x;
					  v_3.w = _World2Object[3].x;
					  highp vec4 v_4;
					  v_4.x = _World2Object[0].y;
					  v_4.y = _World2Object[1].y;
					  v_4.z = _World2Object[2].y;
					  v_4.w = _World2Object[3].y;
					  highp vec4 v_5;
					  v_5.x = _World2Object[0].z;
					  v_5.y = _World2Object[1].z;
					  v_5.z = _World2Object[2].z;
					  v_5.w = _World2Object[3].z;
					  highp vec3 tmpvar_6;
					  tmpvar_6 = normalize(((
					    (v_3.xyz * _glesNormal.x)
					   + 
					    (v_4.xyz * _glesNormal.y)
					  ) + (v_5.xyz * _glesNormal.z)));
					  worldNormal_1 = tmpvar_6;
					  tmpvar_2 = worldNormal_1;
					  mediump vec3 normal_7;
					  normal_7 = worldNormal_1;
					  mediump vec4 tmpvar_8;
					  tmpvar_8.w = 1.0;
					  tmpvar_8.xyz = normal_7;
					  mediump vec3 res_9;
					  mediump vec3 x_10;
					  x_10.x = dot (unity_SHAr, tmpvar_8);
					  x_10.y = dot (unity_SHAg, tmpvar_8);
					  x_10.z = dot (unity_SHAb, tmpvar_8);
					  mediump vec3 x1_11;
					  mediump vec4 tmpvar_12;
					  tmpvar_12 = (normal_7.xyzz * normal_7.yzzx);
					  x1_11.x = dot (unity_SHBr, tmpvar_12);
					  x1_11.y = dot (unity_SHBg, tmpvar_12);
					  x1_11.z = dot (unity_SHBb, tmpvar_12);
					  res_9 = (x_10 + (x1_11 + (unity_SHC.xyz * 
					    ((normal_7.x * normal_7.x) - (normal_7.y * normal_7.y))
					  )));
					  res_9 = max (((1.055 * 
					    pow (max (res_9, vec3(0.0, 0.0, 0.0)), vec3(0.4166667, 0.4166667, 0.4166667))
					  ) - 0.055), vec3(0.0, 0.0, 0.0));
					  gl_Position = (glstate_matrix_mvp * _glesVertex);
					  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
					  xlv_TEXCOORD1 = tmpvar_2;
					  xlv_TEXCOORD2 = (_Object2World * _glesVertex).xyz;
					  xlv_COLOR0 = _glesColor;
					  xlv_TEXCOORD3 = max (vec3(0.0, 0.0, 0.0), res_9);
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform highp vec3 _WorldSpaceCameraPos;
					uniform mediump vec4 _WorldSpaceLightPos0;
					uniform lowp vec4 _LightColor0;
					uniform lowp vec4 _SpecColor;
					uniform sampler2D _MainTex;
					uniform lowp vec4 _Color;
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
					  lowp float tmpvar_10;
					  lowp vec4 tmpvar_11;
					  tmpvar_11 = texture2D (_MainTex, xlv_TEXCOORD0);
					  tmpvar_9 = ((tmpvar_11.xyz * _Color.xyz) * tmpvar_6.xyz);
					  tmpvar_10 = ((tmpvar_11.w * _Color.w) * tmpvar_6.w);
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
					  tmpvar_20 = (pow (nh_15, y_19) * tmpvar_11.w);
					  c_14.xyz = (((tmpvar_9 * tmpvar_1) * diff_16) + ((tmpvar_1 * _SpecColor.xyz) * tmpvar_20));
					  c_14.w = tmpvar_10;
					  c_13.w = c_14.w;
					  c_13.xyz = (c_14.xyz + (tmpvar_9 * xlv_TEXCOORD3));
					  gl_FragData[0] = c_13;
					}
					
					
					#endif"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "VERTEXLIGHT_ON" }
					"!!GLES
					#version 100
					
					#ifdef VERTEX
					attribute vec4 _glesVertex;
					attribute vec4 _glesColor;
					attribute vec3 _glesNormal;
					attribute vec4 _glesMultiTexCoord0;
					uniform highp vec4 unity_4LightPosX0;
					uniform highp vec4 unity_4LightPosY0;
					uniform highp vec4 unity_4LightPosZ0;
					uniform mediump vec4 unity_4LightAtten0;
					uniform mediump vec4 unity_LightColor[8];
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
					  highp vec3 tmpvar_4;
					  tmpvar_4 = (_Object2World * _glesVertex).xyz;
					  highp vec4 v_5;
					  v_5.x = _World2Object[0].x;
					  v_5.y = _World2Object[1].x;
					  v_5.z = _World2Object[2].x;
					  v_5.w = _World2Object[3].x;
					  highp vec4 v_6;
					  v_6.x = _World2Object[0].y;
					  v_6.y = _World2Object[1].y;
					  v_6.z = _World2Object[2].y;
					  v_6.w = _World2Object[3].y;
					  highp vec4 v_7;
					  v_7.x = _World2Object[0].z;
					  v_7.y = _World2Object[1].z;
					  v_7.z = _World2Object[2].z;
					  v_7.w = _World2Object[3].z;
					  highp vec3 tmpvar_8;
					  tmpvar_8 = normalize(((
					    (v_5.xyz * _glesNormal.x)
					   + 
					    (v_6.xyz * _glesNormal.y)
					  ) + (v_7.xyz * _glesNormal.z)));
					  worldNormal_1 = tmpvar_8;
					  tmpvar_2 = worldNormal_1;
					  highp vec3 lightColor0_9;
					  lightColor0_9 = unity_LightColor[0].xyz;
					  highp vec3 lightColor1_10;
					  lightColor1_10 = unity_LightColor[1].xyz;
					  highp vec3 lightColor2_11;
					  lightColor2_11 = unity_LightColor[2].xyz;
					  highp vec3 lightColor3_12;
					  lightColor3_12 = unity_LightColor[3].xyz;
					  highp vec4 lightAttenSq_13;
					  lightAttenSq_13 = unity_4LightAtten0;
					  highp vec3 normal_14;
					  normal_14 = worldNormal_1;
					  highp vec3 col_15;
					  highp vec4 ndotl_16;
					  highp vec4 lengthSq_17;
					  highp vec4 tmpvar_18;
					  tmpvar_18 = (unity_4LightPosX0 - tmpvar_4.x);
					  highp vec4 tmpvar_19;
					  tmpvar_19 = (unity_4LightPosY0 - tmpvar_4.y);
					  highp vec4 tmpvar_20;
					  tmpvar_20 = (unity_4LightPosZ0 - tmpvar_4.z);
					  lengthSq_17 = (tmpvar_18 * tmpvar_18);
					  lengthSq_17 = (lengthSq_17 + (tmpvar_19 * tmpvar_19));
					  lengthSq_17 = (lengthSq_17 + (tmpvar_20 * tmpvar_20));
					  ndotl_16 = (tmpvar_18 * normal_14.x);
					  ndotl_16 = (ndotl_16 + (tmpvar_19 * normal_14.y));
					  ndotl_16 = (ndotl_16 + (tmpvar_20 * normal_14.z));
					  highp vec4 tmpvar_21;
					  tmpvar_21 = max (vec4(0.0, 0.0, 0.0, 0.0), (ndotl_16 * inversesqrt(lengthSq_17)));
					  ndotl_16 = tmpvar_21;
					  highp vec4 tmpvar_22;
					  tmpvar_22 = (tmpvar_21 * (1.0/((1.0 + 
					    (lengthSq_17 * lightAttenSq_13)
					  ))));
					  col_15 = (lightColor0_9 * tmpvar_22.x);
					  col_15 = (col_15 + (lightColor1_10 * tmpvar_22.y));
					  col_15 = (col_15 + (lightColor2_11 * tmpvar_22.z));
					  col_15 = (col_15 + (lightColor3_12 * tmpvar_22.w));
					  tmpvar_3 = col_15;
					  mediump vec3 normal_23;
					  normal_23 = worldNormal_1;
					  mediump vec3 ambient_24;
					  mediump vec4 tmpvar_25;
					  tmpvar_25.w = 1.0;
					  tmpvar_25.xyz = normal_23;
					  mediump vec3 res_26;
					  mediump vec3 x_27;
					  x_27.x = dot (unity_SHAr, tmpvar_25);
					  x_27.y = dot (unity_SHAg, tmpvar_25);
					  x_27.z = dot (unity_SHAb, tmpvar_25);
					  mediump vec3 x1_28;
					  mediump vec4 tmpvar_29;
					  tmpvar_29 = (normal_23.xyzz * normal_23.yzzx);
					  x1_28.x = dot (unity_SHBr, tmpvar_29);
					  x1_28.y = dot (unity_SHBg, tmpvar_29);
					  x1_28.z = dot (unity_SHBb, tmpvar_29);
					  res_26 = (x_27 + (x1_28 + (unity_SHC.xyz * 
					    ((normal_23.x * normal_23.x) - (normal_23.y * normal_23.y))
					  )));
					  res_26 = max (((1.055 * 
					    pow (max (res_26, vec3(0.0, 0.0, 0.0)), vec3(0.4166667, 0.4166667, 0.4166667))
					  ) - 0.055), vec3(0.0, 0.0, 0.0));
					  ambient_24 = (tmpvar_3 + max (vec3(0.0, 0.0, 0.0), res_26));
					  tmpvar_3 = ambient_24;
					  gl_Position = (glstate_matrix_mvp * _glesVertex);
					  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
					  xlv_TEXCOORD1 = tmpvar_2;
					  xlv_TEXCOORD2 = tmpvar_4;
					  xlv_COLOR0 = _glesColor;
					  xlv_TEXCOORD3 = ambient_24;
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform highp vec3 _WorldSpaceCameraPos;
					uniform mediump vec4 _WorldSpaceLightPos0;
					uniform lowp vec4 _LightColor0;
					uniform lowp vec4 _SpecColor;
					uniform sampler2D _MainTex;
					uniform lowp vec4 _Color;
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
					  lowp float tmpvar_10;
					  lowp vec4 tmpvar_11;
					  tmpvar_11 = texture2D (_MainTex, xlv_TEXCOORD0);
					  tmpvar_9 = ((tmpvar_11.xyz * _Color.xyz) * tmpvar_6.xyz);
					  tmpvar_10 = ((tmpvar_11.w * _Color.w) * tmpvar_6.w);
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
					  tmpvar_20 = (pow (nh_15, y_19) * tmpvar_11.w);
					  c_14.xyz = (((tmpvar_9 * tmpvar_1) * diff_16) + ((tmpvar_1 * _SpecColor.xyz) * tmpvar_20));
					  c_14.w = tmpvar_10;
					  c_13.w = c_14.w;
					  c_13.xyz = (c_14.xyz + (tmpvar_9 * xlv_TEXCOORD3));
					  gl_FragData[0] = c_13;
					}
					
					
					#endif"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "FOG_LINEAR" }
					"!!GLES
					#version 100
					
					#ifdef VERTEX
					attribute vec4 _glesVertex;
					attribute vec4 _glesColor;
					attribute vec3 _glesNormal;
					attribute vec4 _glesMultiTexCoord0;
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
					  tmpvar_2 = (glstate_matrix_mvp * _glesVertex);
					  highp vec4 v_4;
					  v_4.x = _World2Object[0].x;
					  v_4.y = _World2Object[1].x;
					  v_4.z = _World2Object[2].x;
					  v_4.w = _World2Object[3].x;
					  highp vec4 v_5;
					  v_5.x = _World2Object[0].y;
					  v_5.y = _World2Object[1].y;
					  v_5.z = _World2Object[2].y;
					  v_5.w = _World2Object[3].y;
					  highp vec4 v_6;
					  v_6.x = _World2Object[0].z;
					  v_6.y = _World2Object[1].z;
					  v_6.z = _World2Object[2].z;
					  v_6.w = _World2Object[3].z;
					  highp vec3 tmpvar_7;
					  tmpvar_7 = normalize(((
					    (v_4.xyz * _glesNormal.x)
					   + 
					    (v_5.xyz * _glesNormal.y)
					  ) + (v_6.xyz * _glesNormal.z)));
					  worldNormal_1 = tmpvar_7;
					  tmpvar_3 = worldNormal_1;
					  mediump vec3 normal_8;
					  normal_8 = worldNormal_1;
					  mediump vec4 tmpvar_9;
					  tmpvar_9.w = 1.0;
					  tmpvar_9.xyz = normal_8;
					  mediump vec3 res_10;
					  mediump vec3 x_11;
					  x_11.x = dot (unity_SHAr, tmpvar_9);
					  x_11.y = dot (unity_SHAg, tmpvar_9);
					  x_11.z = dot (unity_SHAb, tmpvar_9);
					  mediump vec3 x1_12;
					  mediump vec4 tmpvar_13;
					  tmpvar_13 = (normal_8.xyzz * normal_8.yzzx);
					  x1_12.x = dot (unity_SHBr, tmpvar_13);
					  x1_12.y = dot (unity_SHBg, tmpvar_13);
					  x1_12.z = dot (unity_SHBb, tmpvar_13);
					  res_10 = (x_11 + (x1_12 + (unity_SHC.xyz * 
					    ((normal_8.x * normal_8.x) - (normal_8.y * normal_8.y))
					  )));
					  res_10 = max (((1.055 * 
					    pow (max (res_10, vec3(0.0, 0.0, 0.0)), vec3(0.4166667, 0.4166667, 0.4166667))
					  ) - 0.055), vec3(0.0, 0.0, 0.0));
					  gl_Position = tmpvar_2;
					  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
					  xlv_TEXCOORD1 = tmpvar_3;
					  xlv_TEXCOORD2 = (_Object2World * _glesVertex).xyz;
					  xlv_COLOR0 = _glesColor;
					  xlv_TEXCOORD3 = max (vec3(0.0, 0.0, 0.0), res_10);
					  xlv_TEXCOORD4 = ((tmpvar_2.z * unity_FogParams.z) + unity_FogParams.w);
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform highp vec3 _WorldSpaceCameraPos;
					uniform mediump vec4 _WorldSpaceLightPos0;
					uniform lowp vec4 unity_FogColor;
					uniform lowp vec4 _LightColor0;
					uniform lowp vec4 _SpecColor;
					uniform sampler2D _MainTex;
					uniform lowp vec4 _Color;
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
					  lowp float tmpvar_11;
					  lowp vec4 tmpvar_12;
					  tmpvar_12 = texture2D (_MainTex, xlv_TEXCOORD0);
					  tmpvar_10 = ((tmpvar_12.xyz * _Color.xyz) * tmpvar_7.xyz);
					  tmpvar_11 = ((tmpvar_12.w * _Color.w) * tmpvar_7.w);
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
					  tmpvar_21 = (pow (nh_16, y_20) * tmpvar_12.w);
					  c_15.xyz = (((tmpvar_10 * tmpvar_1) * diff_17) + ((tmpvar_1 * _SpecColor.xyz) * tmpvar_21));
					  c_15.w = tmpvar_11;
					  c_14.w = c_15.w;
					  c_14.xyz = (c_15.xyz + (tmpvar_10 * xlv_TEXCOORD3));
					  c_3.w = c_14.w;
					  highp float tmpvar_22;
					  tmpvar_22 = clamp (xlv_TEXCOORD4, 0.0, 1.0);
					  c_3.xyz = mix (unity_FogColor.xyz, c_14.xyz, vec3(tmpvar_22));
					  gl_FragData[0] = c_3;
					}
					
					
					#endif"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "FOG_LINEAR" "VERTEXLIGHT_ON" }
					"!!GLES
					#version 100
					
					#ifdef VERTEX
					attribute vec4 _glesVertex;
					attribute vec4 _glesColor;
					attribute vec3 _glesNormal;
					attribute vec4 _glesMultiTexCoord0;
					uniform highp vec4 unity_4LightPosX0;
					uniform highp vec4 unity_4LightPosY0;
					uniform highp vec4 unity_4LightPosZ0;
					uniform mediump vec4 unity_4LightAtten0;
					uniform mediump vec4 unity_LightColor[8];
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
					  tmpvar_2 = (glstate_matrix_mvp * _glesVertex);
					  highp vec3 tmpvar_5;
					  tmpvar_5 = (_Object2World * _glesVertex).xyz;
					  highp vec4 v_6;
					  v_6.x = _World2Object[0].x;
					  v_6.y = _World2Object[1].x;
					  v_6.z = _World2Object[2].x;
					  v_6.w = _World2Object[3].x;
					  highp vec4 v_7;
					  v_7.x = _World2Object[0].y;
					  v_7.y = _World2Object[1].y;
					  v_7.z = _World2Object[2].y;
					  v_7.w = _World2Object[3].y;
					  highp vec4 v_8;
					  v_8.x = _World2Object[0].z;
					  v_8.y = _World2Object[1].z;
					  v_8.z = _World2Object[2].z;
					  v_8.w = _World2Object[3].z;
					  highp vec3 tmpvar_9;
					  tmpvar_9 = normalize(((
					    (v_6.xyz * _glesNormal.x)
					   + 
					    (v_7.xyz * _glesNormal.y)
					  ) + (v_8.xyz * _glesNormal.z)));
					  worldNormal_1 = tmpvar_9;
					  tmpvar_3 = worldNormal_1;
					  highp vec3 lightColor0_10;
					  lightColor0_10 = unity_LightColor[0].xyz;
					  highp vec3 lightColor1_11;
					  lightColor1_11 = unity_LightColor[1].xyz;
					  highp vec3 lightColor2_12;
					  lightColor2_12 = unity_LightColor[2].xyz;
					  highp vec3 lightColor3_13;
					  lightColor3_13 = unity_LightColor[3].xyz;
					  highp vec4 lightAttenSq_14;
					  lightAttenSq_14 = unity_4LightAtten0;
					  highp vec3 normal_15;
					  normal_15 = worldNormal_1;
					  highp vec3 col_16;
					  highp vec4 ndotl_17;
					  highp vec4 lengthSq_18;
					  highp vec4 tmpvar_19;
					  tmpvar_19 = (unity_4LightPosX0 - tmpvar_5.x);
					  highp vec4 tmpvar_20;
					  tmpvar_20 = (unity_4LightPosY0 - tmpvar_5.y);
					  highp vec4 tmpvar_21;
					  tmpvar_21 = (unity_4LightPosZ0 - tmpvar_5.z);
					  lengthSq_18 = (tmpvar_19 * tmpvar_19);
					  lengthSq_18 = (lengthSq_18 + (tmpvar_20 * tmpvar_20));
					  lengthSq_18 = (lengthSq_18 + (tmpvar_21 * tmpvar_21));
					  ndotl_17 = (tmpvar_19 * normal_15.x);
					  ndotl_17 = (ndotl_17 + (tmpvar_20 * normal_15.y));
					  ndotl_17 = (ndotl_17 + (tmpvar_21 * normal_15.z));
					  highp vec4 tmpvar_22;
					  tmpvar_22 = max (vec4(0.0, 0.0, 0.0, 0.0), (ndotl_17 * inversesqrt(lengthSq_18)));
					  ndotl_17 = tmpvar_22;
					  highp vec4 tmpvar_23;
					  tmpvar_23 = (tmpvar_22 * (1.0/((1.0 + 
					    (lengthSq_18 * lightAttenSq_14)
					  ))));
					  col_16 = (lightColor0_10 * tmpvar_23.x);
					  col_16 = (col_16 + (lightColor1_11 * tmpvar_23.y));
					  col_16 = (col_16 + (lightColor2_12 * tmpvar_23.z));
					  col_16 = (col_16 + (lightColor3_13 * tmpvar_23.w));
					  tmpvar_4 = col_16;
					  mediump vec3 normal_24;
					  normal_24 = worldNormal_1;
					  mediump vec3 ambient_25;
					  mediump vec4 tmpvar_26;
					  tmpvar_26.w = 1.0;
					  tmpvar_26.xyz = normal_24;
					  mediump vec3 res_27;
					  mediump vec3 x_28;
					  x_28.x = dot (unity_SHAr, tmpvar_26);
					  x_28.y = dot (unity_SHAg, tmpvar_26);
					  x_28.z = dot (unity_SHAb, tmpvar_26);
					  mediump vec3 x1_29;
					  mediump vec4 tmpvar_30;
					  tmpvar_30 = (normal_24.xyzz * normal_24.yzzx);
					  x1_29.x = dot (unity_SHBr, tmpvar_30);
					  x1_29.y = dot (unity_SHBg, tmpvar_30);
					  x1_29.z = dot (unity_SHBb, tmpvar_30);
					  res_27 = (x_28 + (x1_29 + (unity_SHC.xyz * 
					    ((normal_24.x * normal_24.x) - (normal_24.y * normal_24.y))
					  )));
					  res_27 = max (((1.055 * 
					    pow (max (res_27, vec3(0.0, 0.0, 0.0)), vec3(0.4166667, 0.4166667, 0.4166667))
					  ) - 0.055), vec3(0.0, 0.0, 0.0));
					  ambient_25 = (tmpvar_4 + max (vec3(0.0, 0.0, 0.0), res_27));
					  tmpvar_4 = ambient_25;
					  gl_Position = tmpvar_2;
					  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
					  xlv_TEXCOORD1 = tmpvar_3;
					  xlv_TEXCOORD2 = tmpvar_5;
					  xlv_COLOR0 = _glesColor;
					  xlv_TEXCOORD3 = ambient_25;
					  xlv_TEXCOORD4 = ((tmpvar_2.z * unity_FogParams.z) + unity_FogParams.w);
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform highp vec3 _WorldSpaceCameraPos;
					uniform mediump vec4 _WorldSpaceLightPos0;
					uniform lowp vec4 unity_FogColor;
					uniform lowp vec4 _LightColor0;
					uniform lowp vec4 _SpecColor;
					uniform sampler2D _MainTex;
					uniform lowp vec4 _Color;
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
					  lowp float tmpvar_11;
					  lowp vec4 tmpvar_12;
					  tmpvar_12 = texture2D (_MainTex, xlv_TEXCOORD0);
					  tmpvar_10 = ((tmpvar_12.xyz * _Color.xyz) * tmpvar_7.xyz);
					  tmpvar_11 = ((tmpvar_12.w * _Color.w) * tmpvar_7.w);
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
					  tmpvar_21 = (pow (nh_16, y_20) * tmpvar_12.w);
					  c_15.xyz = (((tmpvar_10 * tmpvar_1) * diff_17) + ((tmpvar_1 * _SpecColor.xyz) * tmpvar_21));
					  c_15.w = tmpvar_11;
					  c_14.w = c_15.w;
					  c_14.xyz = (c_15.xyz + (tmpvar_10 * xlv_TEXCOORD3));
					  c_3.w = c_14.w;
					  highp float tmpvar_22;
					  tmpvar_22 = clamp (xlv_TEXCOORD4, 0.0, 1.0);
					  c_3.xyz = mix (unity_FogColor.xyz, c_14.xyz, vec3(tmpvar_22));
					  gl_FragData[0] = c_3;
					}
					
					
					#endif"
}
}
Program "fp" {
SubProgram "gles " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" }
					"!!GLES"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "FOG_LINEAR" }
					"!!GLES"
}
}
 }
 Pass {
  Name "FORWARD"
  Tags { "LIGHTMODE"="ForwardAdd" "QUEUE"="Transparent" "IGNOREPROJECTOR"="true" "RenderType"="Transparent" }
  ZWrite Off
  Blend SrcAlpha One
  ColorMask RGB
  GpuProgramID 80184
Program "vp" {
SubProgram "gles " {
Keywords { "POINT" }
					"!!GLES
					#version 100
					
					#ifdef VERTEX
					attribute vec4 _glesVertex;
					attribute vec4 _glesColor;
					attribute vec3 _glesNormal;
					attribute vec4 _glesMultiTexCoord0;
					uniform highp mat4 glstate_matrix_mvp;
					uniform highp mat4 _Object2World;
					uniform highp mat4 _World2Object;
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
					  v_3.x = _World2Object[0].x;
					  v_3.y = _World2Object[1].x;
					  v_3.z = _World2Object[2].x;
					  v_3.w = _World2Object[3].x;
					  highp vec4 v_4;
					  v_4.x = _World2Object[0].y;
					  v_4.y = _World2Object[1].y;
					  v_4.z = _World2Object[2].y;
					  v_4.w = _World2Object[3].y;
					  highp vec4 v_5;
					  v_5.x = _World2Object[0].z;
					  v_5.y = _World2Object[1].z;
					  v_5.z = _World2Object[2].z;
					  v_5.w = _World2Object[3].z;
					  highp vec3 tmpvar_6;
					  tmpvar_6 = normalize(((
					    (v_3.xyz * _glesNormal.x)
					   + 
					    (v_4.xyz * _glesNormal.y)
					  ) + (v_5.xyz * _glesNormal.z)));
					  worldNormal_1 = tmpvar_6;
					  tmpvar_2 = worldNormal_1;
					  gl_Position = (glstate_matrix_mvp * _glesVertex);
					  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
					  xlv_TEXCOORD1 = tmpvar_2;
					  xlv_TEXCOORD2 = (_Object2World * _glesVertex).xyz;
					  xlv_COLOR0 = _glesColor;
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform highp vec3 _WorldSpaceCameraPos;
					uniform highp vec4 _WorldSpaceLightPos0;
					uniform lowp vec4 _LightColor0;
					uniform lowp vec4 _SpecColor;
					uniform sampler2D _LightTexture0;
					uniform mediump mat4 _LightMatrix0;
					uniform sampler2D _MainTex;
					uniform lowp vec4 _Color;
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
					  lowp float tmpvar_10;
					  lowp vec4 tmpvar_11;
					  tmpvar_11 = texture2D (_MainTex, xlv_TEXCOORD0);
					  tmpvar_9 = ((tmpvar_11.xyz * _Color.xyz) * tmpvar_6.xyz);
					  tmpvar_10 = ((tmpvar_11.w * _Color.w) * tmpvar_6.w);
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
					  tmpvar_24 = (pow (nh_19, y_23) * tmpvar_11.w);
					  c_18.xyz = (((tmpvar_9 * tmpvar_1) * diff_20) + ((tmpvar_1 * _SpecColor.xyz) * tmpvar_24));
					  c_18.w = tmpvar_10;
					  c_17.w = c_18.w;
					  c_17.xyz = c_18.xyz;
					  gl_FragData[0] = c_17;
					}
					
					
					#endif"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" }
					"!!GLES
					#version 100
					
					#ifdef VERTEX
					attribute vec4 _glesVertex;
					attribute vec4 _glesColor;
					attribute vec3 _glesNormal;
					attribute vec4 _glesMultiTexCoord0;
					uniform highp mat4 glstate_matrix_mvp;
					uniform highp mat4 _Object2World;
					uniform highp mat4 _World2Object;
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
					  v_3.x = _World2Object[0].x;
					  v_3.y = _World2Object[1].x;
					  v_3.z = _World2Object[2].x;
					  v_3.w = _World2Object[3].x;
					  highp vec4 v_4;
					  v_4.x = _World2Object[0].y;
					  v_4.y = _World2Object[1].y;
					  v_4.z = _World2Object[2].y;
					  v_4.w = _World2Object[3].y;
					  highp vec4 v_5;
					  v_5.x = _World2Object[0].z;
					  v_5.y = _World2Object[1].z;
					  v_5.z = _World2Object[2].z;
					  v_5.w = _World2Object[3].z;
					  highp vec3 tmpvar_6;
					  tmpvar_6 = normalize(((
					    (v_3.xyz * _glesNormal.x)
					   + 
					    (v_4.xyz * _glesNormal.y)
					  ) + (v_5.xyz * _glesNormal.z)));
					  worldNormal_1 = tmpvar_6;
					  tmpvar_2 = worldNormal_1;
					  gl_Position = (glstate_matrix_mvp * _glesVertex);
					  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
					  xlv_TEXCOORD1 = tmpvar_2;
					  xlv_TEXCOORD2 = (_Object2World * _glesVertex).xyz;
					  xlv_COLOR0 = _glesColor;
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform highp vec3 _WorldSpaceCameraPos;
					uniform mediump vec4 _WorldSpaceLightPos0;
					uniform lowp vec4 _LightColor0;
					uniform lowp vec4 _SpecColor;
					uniform sampler2D _MainTex;
					uniform lowp vec4 _Color;
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
					  lowp float tmpvar_10;
					  lowp vec4 tmpvar_11;
					  tmpvar_11 = texture2D (_MainTex, xlv_TEXCOORD0);
					  tmpvar_9 = ((tmpvar_11.xyz * _Color.xyz) * tmpvar_6.xyz);
					  tmpvar_10 = ((tmpvar_11.w * _Color.w) * tmpvar_6.w);
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
					  tmpvar_20 = (pow (nh_15, y_19) * tmpvar_11.w);
					  c_14.xyz = (((tmpvar_9 * tmpvar_1) * diff_16) + ((tmpvar_1 * _SpecColor.xyz) * tmpvar_20));
					  c_14.w = tmpvar_10;
					  c_13.w = c_14.w;
					  c_13.xyz = c_14.xyz;
					  gl_FragData[0] = c_13;
					}
					
					
					#endif"
}
SubProgram "gles " {
Keywords { "SPOT" }
					"!!GLES
					#version 100
					
					#ifdef VERTEX
					attribute vec4 _glesVertex;
					attribute vec4 _glesColor;
					attribute vec3 _glesNormal;
					attribute vec4 _glesMultiTexCoord0;
					uniform highp mat4 glstate_matrix_mvp;
					uniform highp mat4 _Object2World;
					uniform highp mat4 _World2Object;
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
					  v_3.x = _World2Object[0].x;
					  v_3.y = _World2Object[1].x;
					  v_3.z = _World2Object[2].x;
					  v_3.w = _World2Object[3].x;
					  highp vec4 v_4;
					  v_4.x = _World2Object[0].y;
					  v_4.y = _World2Object[1].y;
					  v_4.z = _World2Object[2].y;
					  v_4.w = _World2Object[3].y;
					  highp vec4 v_5;
					  v_5.x = _World2Object[0].z;
					  v_5.y = _World2Object[1].z;
					  v_5.z = _World2Object[2].z;
					  v_5.w = _World2Object[3].z;
					  highp vec3 tmpvar_6;
					  tmpvar_6 = normalize(((
					    (v_3.xyz * _glesNormal.x)
					   + 
					    (v_4.xyz * _glesNormal.y)
					  ) + (v_5.xyz * _glesNormal.z)));
					  worldNormal_1 = tmpvar_6;
					  tmpvar_2 = worldNormal_1;
					  gl_Position = (glstate_matrix_mvp * _glesVertex);
					  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
					  xlv_TEXCOORD1 = tmpvar_2;
					  xlv_TEXCOORD2 = (_Object2World * _glesVertex).xyz;
					  xlv_COLOR0 = _glesColor;
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform highp vec3 _WorldSpaceCameraPos;
					uniform highp vec4 _WorldSpaceLightPos0;
					uniform lowp vec4 _LightColor0;
					uniform lowp vec4 _SpecColor;
					uniform sampler2D _LightTexture0;
					uniform mediump mat4 _LightMatrix0;
					uniform sampler2D _LightTextureB0;
					uniform sampler2D _MainTex;
					uniform lowp vec4 _Color;
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
					  lowp float tmpvar_12;
					  lowp vec4 tmpvar_13;
					  tmpvar_13 = texture2D (_MainTex, xlv_TEXCOORD0);
					  tmpvar_11 = ((tmpvar_13.xyz * _Color.xyz) * tmpvar_8.xyz);
					  tmpvar_12 = ((tmpvar_13.w * _Color.w) * tmpvar_8.w);
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
					  tmpvar_30 = (pow (nh_25, y_29) * tmpvar_13.w);
					  c_24.xyz = (((tmpvar_11 * tmpvar_1) * diff_26) + ((tmpvar_1 * _SpecColor.xyz) * tmpvar_30));
					  c_24.w = tmpvar_12;
					  c_23.w = c_24.w;
					  c_23.xyz = c_24.xyz;
					  gl_FragData[0] = c_23;
					}
					
					
					#endif"
}
SubProgram "gles " {
Keywords { "POINT_COOKIE" }
					"!!GLES
					#version 100
					
					#ifdef VERTEX
					attribute vec4 _glesVertex;
					attribute vec4 _glesColor;
					attribute vec3 _glesNormal;
					attribute vec4 _glesMultiTexCoord0;
					uniform highp mat4 glstate_matrix_mvp;
					uniform highp mat4 _Object2World;
					uniform highp mat4 _World2Object;
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
					  v_3.x = _World2Object[0].x;
					  v_3.y = _World2Object[1].x;
					  v_3.z = _World2Object[2].x;
					  v_3.w = _World2Object[3].x;
					  highp vec4 v_4;
					  v_4.x = _World2Object[0].y;
					  v_4.y = _World2Object[1].y;
					  v_4.z = _World2Object[2].y;
					  v_4.w = _World2Object[3].y;
					  highp vec4 v_5;
					  v_5.x = _World2Object[0].z;
					  v_5.y = _World2Object[1].z;
					  v_5.z = _World2Object[2].z;
					  v_5.w = _World2Object[3].z;
					  highp vec3 tmpvar_6;
					  tmpvar_6 = normalize(((
					    (v_3.xyz * _glesNormal.x)
					   + 
					    (v_4.xyz * _glesNormal.y)
					  ) + (v_5.xyz * _glesNormal.z)));
					  worldNormal_1 = tmpvar_6;
					  tmpvar_2 = worldNormal_1;
					  gl_Position = (glstate_matrix_mvp * _glesVertex);
					  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
					  xlv_TEXCOORD1 = tmpvar_2;
					  xlv_TEXCOORD2 = (_Object2World * _glesVertex).xyz;
					  xlv_COLOR0 = _glesColor;
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform highp vec3 _WorldSpaceCameraPos;
					uniform highp vec4 _WorldSpaceLightPos0;
					uniform lowp vec4 _LightColor0;
					uniform lowp vec4 _SpecColor;
					uniform lowp samplerCube _LightTexture0;
					uniform mediump mat4 _LightMatrix0;
					uniform sampler2D _LightTextureB0;
					uniform sampler2D _MainTex;
					uniform lowp vec4 _Color;
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
					  lowp float tmpvar_10;
					  lowp vec4 tmpvar_11;
					  tmpvar_11 = texture2D (_MainTex, xlv_TEXCOORD0);
					  tmpvar_9 = ((tmpvar_11.xyz * _Color.xyz) * tmpvar_6.xyz);
					  tmpvar_10 = ((tmpvar_11.w * _Color.w) * tmpvar_6.w);
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
					  tmpvar_24 = (pow (nh_19, y_23) * tmpvar_11.w);
					  c_18.xyz = (((tmpvar_9 * tmpvar_1) * diff_20) + ((tmpvar_1 * _SpecColor.xyz) * tmpvar_24));
					  c_18.w = tmpvar_10;
					  c_17.w = c_18.w;
					  c_17.xyz = c_18.xyz;
					  gl_FragData[0] = c_17;
					}
					
					
					#endif"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL_COOKIE" }
					"!!GLES
					#version 100
					
					#ifdef VERTEX
					attribute vec4 _glesVertex;
					attribute vec4 _glesColor;
					attribute vec3 _glesNormal;
					attribute vec4 _glesMultiTexCoord0;
					uniform highp mat4 glstate_matrix_mvp;
					uniform highp mat4 _Object2World;
					uniform highp mat4 _World2Object;
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
					  v_3.x = _World2Object[0].x;
					  v_3.y = _World2Object[1].x;
					  v_3.z = _World2Object[2].x;
					  v_3.w = _World2Object[3].x;
					  highp vec4 v_4;
					  v_4.x = _World2Object[0].y;
					  v_4.y = _World2Object[1].y;
					  v_4.z = _World2Object[2].y;
					  v_4.w = _World2Object[3].y;
					  highp vec4 v_5;
					  v_5.x = _World2Object[0].z;
					  v_5.y = _World2Object[1].z;
					  v_5.z = _World2Object[2].z;
					  v_5.w = _World2Object[3].z;
					  highp vec3 tmpvar_6;
					  tmpvar_6 = normalize(((
					    (v_3.xyz * _glesNormal.x)
					   + 
					    (v_4.xyz * _glesNormal.y)
					  ) + (v_5.xyz * _glesNormal.z)));
					  worldNormal_1 = tmpvar_6;
					  tmpvar_2 = worldNormal_1;
					  gl_Position = (glstate_matrix_mvp * _glesVertex);
					  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
					  xlv_TEXCOORD1 = tmpvar_2;
					  xlv_TEXCOORD2 = (_Object2World * _glesVertex).xyz;
					  xlv_COLOR0 = _glesColor;
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform highp vec3 _WorldSpaceCameraPos;
					uniform mediump vec4 _WorldSpaceLightPos0;
					uniform lowp vec4 _LightColor0;
					uniform lowp vec4 _SpecColor;
					uniform sampler2D _LightTexture0;
					uniform mediump mat4 _LightMatrix0;
					uniform sampler2D _MainTex;
					uniform lowp vec4 _Color;
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
					  lowp float tmpvar_10;
					  lowp vec4 tmpvar_11;
					  tmpvar_11 = texture2D (_MainTex, xlv_TEXCOORD0);
					  tmpvar_9 = ((tmpvar_11.xyz * _Color.xyz) * tmpvar_6.xyz);
					  tmpvar_10 = ((tmpvar_11.w * _Color.w) * tmpvar_6.w);
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
					  tmpvar_23 = (pow (nh_18, y_22) * tmpvar_11.w);
					  c_17.xyz = (((tmpvar_9 * tmpvar_1) * diff_19) + ((tmpvar_1 * _SpecColor.xyz) * tmpvar_23));
					  c_17.w = tmpvar_10;
					  c_16.w = c_17.w;
					  c_16.xyz = c_17.xyz;
					  gl_FragData[0] = c_16;
					}
					
					
					#endif"
}
SubProgram "gles " {
Keywords { "POINT" "FOG_LINEAR" }
					"!!GLES
					#version 100
					
					#ifdef VERTEX
					attribute vec4 _glesVertex;
					attribute vec4 _glesColor;
					attribute vec3 _glesNormal;
					attribute vec4 _glesMultiTexCoord0;
					uniform highp mat4 glstate_matrix_mvp;
					uniform highp mat4 _Object2World;
					uniform highp mat4 _World2Object;
					uniform highp vec4 unity_FogParams;
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
					  tmpvar_2 = (glstate_matrix_mvp * _glesVertex);
					  highp vec4 v_4;
					  v_4.x = _World2Object[0].x;
					  v_4.y = _World2Object[1].x;
					  v_4.z = _World2Object[2].x;
					  v_4.w = _World2Object[3].x;
					  highp vec4 v_5;
					  v_5.x = _World2Object[0].y;
					  v_5.y = _World2Object[1].y;
					  v_5.z = _World2Object[2].y;
					  v_5.w = _World2Object[3].y;
					  highp vec4 v_6;
					  v_6.x = _World2Object[0].z;
					  v_6.y = _World2Object[1].z;
					  v_6.z = _World2Object[2].z;
					  v_6.w = _World2Object[3].z;
					  highp vec3 tmpvar_7;
					  tmpvar_7 = normalize(((
					    (v_4.xyz * _glesNormal.x)
					   + 
					    (v_5.xyz * _glesNormal.y)
					  ) + (v_6.xyz * _glesNormal.z)));
					  worldNormal_1 = tmpvar_7;
					  tmpvar_3 = worldNormal_1;
					  gl_Position = tmpvar_2;
					  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
					  xlv_TEXCOORD1 = tmpvar_3;
					  xlv_TEXCOORD2 = (_Object2World * _glesVertex).xyz;
					  xlv_COLOR0 = _glesColor;
					  xlv_TEXCOORD3 = ((tmpvar_2.z * unity_FogParams.z) + unity_FogParams.w);
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform highp vec3 _WorldSpaceCameraPos;
					uniform highp vec4 _WorldSpaceLightPos0;
					uniform lowp vec4 _LightColor0;
					uniform lowp vec4 _SpecColor;
					uniform sampler2D _LightTexture0;
					uniform mediump mat4 _LightMatrix0;
					uniform sampler2D _MainTex;
					uniform lowp vec4 _Color;
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
					  lowp float tmpvar_11;
					  lowp vec4 tmpvar_12;
					  tmpvar_12 = texture2D (_MainTex, xlv_TEXCOORD0);
					  tmpvar_10 = ((tmpvar_12.xyz * _Color.xyz) * tmpvar_7.xyz);
					  tmpvar_11 = ((tmpvar_12.w * _Color.w) * tmpvar_7.w);
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
					  tmpvar_25 = (pow (nh_20, y_24) * tmpvar_12.w);
					  c_19.xyz = (((tmpvar_10 * tmpvar_1) * diff_21) + ((tmpvar_1 * _SpecColor.xyz) * tmpvar_25));
					  c_19.w = tmpvar_11;
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
Keywords { "DIRECTIONAL" "FOG_LINEAR" }
					"!!GLES
					#version 100
					
					#ifdef VERTEX
					attribute vec4 _glesVertex;
					attribute vec4 _glesColor;
					attribute vec3 _glesNormal;
					attribute vec4 _glesMultiTexCoord0;
					uniform highp mat4 glstate_matrix_mvp;
					uniform highp mat4 _Object2World;
					uniform highp mat4 _World2Object;
					uniform highp vec4 unity_FogParams;
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
					  tmpvar_2 = (glstate_matrix_mvp * _glesVertex);
					  highp vec4 v_4;
					  v_4.x = _World2Object[0].x;
					  v_4.y = _World2Object[1].x;
					  v_4.z = _World2Object[2].x;
					  v_4.w = _World2Object[3].x;
					  highp vec4 v_5;
					  v_5.x = _World2Object[0].y;
					  v_5.y = _World2Object[1].y;
					  v_5.z = _World2Object[2].y;
					  v_5.w = _World2Object[3].y;
					  highp vec4 v_6;
					  v_6.x = _World2Object[0].z;
					  v_6.y = _World2Object[1].z;
					  v_6.z = _World2Object[2].z;
					  v_6.w = _World2Object[3].z;
					  highp vec3 tmpvar_7;
					  tmpvar_7 = normalize(((
					    (v_4.xyz * _glesNormal.x)
					   + 
					    (v_5.xyz * _glesNormal.y)
					  ) + (v_6.xyz * _glesNormal.z)));
					  worldNormal_1 = tmpvar_7;
					  tmpvar_3 = worldNormal_1;
					  gl_Position = tmpvar_2;
					  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
					  xlv_TEXCOORD1 = tmpvar_3;
					  xlv_TEXCOORD2 = (_Object2World * _glesVertex).xyz;
					  xlv_COLOR0 = _glesColor;
					  xlv_TEXCOORD3 = ((tmpvar_2.z * unity_FogParams.z) + unity_FogParams.w);
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform highp vec3 _WorldSpaceCameraPos;
					uniform mediump vec4 _WorldSpaceLightPos0;
					uniform lowp vec4 _LightColor0;
					uniform lowp vec4 _SpecColor;
					uniform sampler2D _MainTex;
					uniform lowp vec4 _Color;
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
					  lowp float tmpvar_11;
					  lowp vec4 tmpvar_12;
					  tmpvar_12 = texture2D (_MainTex, xlv_TEXCOORD0);
					  tmpvar_10 = ((tmpvar_12.xyz * _Color.xyz) * tmpvar_7.xyz);
					  tmpvar_11 = ((tmpvar_12.w * _Color.w) * tmpvar_7.w);
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
					  tmpvar_21 = (pow (nh_16, y_20) * tmpvar_12.w);
					  c_15.xyz = (((tmpvar_10 * tmpvar_1) * diff_17) + ((tmpvar_1 * _SpecColor.xyz) * tmpvar_21));
					  c_15.w = tmpvar_11;
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
Keywords { "SPOT" "FOG_LINEAR" }
					"!!GLES
					#version 100
					
					#ifdef VERTEX
					attribute vec4 _glesVertex;
					attribute vec4 _glesColor;
					attribute vec3 _glesNormal;
					attribute vec4 _glesMultiTexCoord0;
					uniform highp mat4 glstate_matrix_mvp;
					uniform highp mat4 _Object2World;
					uniform highp mat4 _World2Object;
					uniform highp vec4 unity_FogParams;
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
					  tmpvar_2 = (glstate_matrix_mvp * _glesVertex);
					  highp vec4 v_4;
					  v_4.x = _World2Object[0].x;
					  v_4.y = _World2Object[1].x;
					  v_4.z = _World2Object[2].x;
					  v_4.w = _World2Object[3].x;
					  highp vec4 v_5;
					  v_5.x = _World2Object[0].y;
					  v_5.y = _World2Object[1].y;
					  v_5.z = _World2Object[2].y;
					  v_5.w = _World2Object[3].y;
					  highp vec4 v_6;
					  v_6.x = _World2Object[0].z;
					  v_6.y = _World2Object[1].z;
					  v_6.z = _World2Object[2].z;
					  v_6.w = _World2Object[3].z;
					  highp vec3 tmpvar_7;
					  tmpvar_7 = normalize(((
					    (v_4.xyz * _glesNormal.x)
					   + 
					    (v_5.xyz * _glesNormal.y)
					  ) + (v_6.xyz * _glesNormal.z)));
					  worldNormal_1 = tmpvar_7;
					  tmpvar_3 = worldNormal_1;
					  gl_Position = tmpvar_2;
					  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
					  xlv_TEXCOORD1 = tmpvar_3;
					  xlv_TEXCOORD2 = (_Object2World * _glesVertex).xyz;
					  xlv_COLOR0 = _glesColor;
					  xlv_TEXCOORD3 = ((tmpvar_2.z * unity_FogParams.z) + unity_FogParams.w);
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform highp vec3 _WorldSpaceCameraPos;
					uniform highp vec4 _WorldSpaceLightPos0;
					uniform lowp vec4 _LightColor0;
					uniform lowp vec4 _SpecColor;
					uniform sampler2D _LightTexture0;
					uniform mediump mat4 _LightMatrix0;
					uniform sampler2D _LightTextureB0;
					uniform sampler2D _MainTex;
					uniform lowp vec4 _Color;
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
					  lowp float tmpvar_13;
					  lowp vec4 tmpvar_14;
					  tmpvar_14 = texture2D (_MainTex, xlv_TEXCOORD0);
					  tmpvar_12 = ((tmpvar_14.xyz * _Color.xyz) * tmpvar_9.xyz);
					  tmpvar_13 = ((tmpvar_14.w * _Color.w) * tmpvar_9.w);
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
					  tmpvar_31 = (pow (nh_26, y_30) * tmpvar_14.w);
					  c_25.xyz = (((tmpvar_12 * tmpvar_1) * diff_27) + ((tmpvar_1 * _SpecColor.xyz) * tmpvar_31));
					  c_25.w = tmpvar_13;
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
Keywords { "POINT_COOKIE" "FOG_LINEAR" }
					"!!GLES
					#version 100
					
					#ifdef VERTEX
					attribute vec4 _glesVertex;
					attribute vec4 _glesColor;
					attribute vec3 _glesNormal;
					attribute vec4 _glesMultiTexCoord0;
					uniform highp mat4 glstate_matrix_mvp;
					uniform highp mat4 _Object2World;
					uniform highp mat4 _World2Object;
					uniform highp vec4 unity_FogParams;
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
					  tmpvar_2 = (glstate_matrix_mvp * _glesVertex);
					  highp vec4 v_4;
					  v_4.x = _World2Object[0].x;
					  v_4.y = _World2Object[1].x;
					  v_4.z = _World2Object[2].x;
					  v_4.w = _World2Object[3].x;
					  highp vec4 v_5;
					  v_5.x = _World2Object[0].y;
					  v_5.y = _World2Object[1].y;
					  v_5.z = _World2Object[2].y;
					  v_5.w = _World2Object[3].y;
					  highp vec4 v_6;
					  v_6.x = _World2Object[0].z;
					  v_6.y = _World2Object[1].z;
					  v_6.z = _World2Object[2].z;
					  v_6.w = _World2Object[3].z;
					  highp vec3 tmpvar_7;
					  tmpvar_7 = normalize(((
					    (v_4.xyz * _glesNormal.x)
					   + 
					    (v_5.xyz * _glesNormal.y)
					  ) + (v_6.xyz * _glesNormal.z)));
					  worldNormal_1 = tmpvar_7;
					  tmpvar_3 = worldNormal_1;
					  gl_Position = tmpvar_2;
					  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
					  xlv_TEXCOORD1 = tmpvar_3;
					  xlv_TEXCOORD2 = (_Object2World * _glesVertex).xyz;
					  xlv_COLOR0 = _glesColor;
					  xlv_TEXCOORD3 = ((tmpvar_2.z * unity_FogParams.z) + unity_FogParams.w);
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform highp vec3 _WorldSpaceCameraPos;
					uniform highp vec4 _WorldSpaceLightPos0;
					uniform lowp vec4 _LightColor0;
					uniform lowp vec4 _SpecColor;
					uniform lowp samplerCube _LightTexture0;
					uniform mediump mat4 _LightMatrix0;
					uniform sampler2D _LightTextureB0;
					uniform sampler2D _MainTex;
					uniform lowp vec4 _Color;
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
					  lowp float tmpvar_11;
					  lowp vec4 tmpvar_12;
					  tmpvar_12 = texture2D (_MainTex, xlv_TEXCOORD0);
					  tmpvar_10 = ((tmpvar_12.xyz * _Color.xyz) * tmpvar_7.xyz);
					  tmpvar_11 = ((tmpvar_12.w * _Color.w) * tmpvar_7.w);
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
					  tmpvar_25 = (pow (nh_20, y_24) * tmpvar_12.w);
					  c_19.xyz = (((tmpvar_10 * tmpvar_1) * diff_21) + ((tmpvar_1 * _SpecColor.xyz) * tmpvar_25));
					  c_19.w = tmpvar_11;
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
Keywords { "DIRECTIONAL_COOKIE" "FOG_LINEAR" }
					"!!GLES
					#version 100
					
					#ifdef VERTEX
					attribute vec4 _glesVertex;
					attribute vec4 _glesColor;
					attribute vec3 _glesNormal;
					attribute vec4 _glesMultiTexCoord0;
					uniform highp mat4 glstate_matrix_mvp;
					uniform highp mat4 _Object2World;
					uniform highp mat4 _World2Object;
					uniform highp vec4 unity_FogParams;
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
					  tmpvar_2 = (glstate_matrix_mvp * _glesVertex);
					  highp vec4 v_4;
					  v_4.x = _World2Object[0].x;
					  v_4.y = _World2Object[1].x;
					  v_4.z = _World2Object[2].x;
					  v_4.w = _World2Object[3].x;
					  highp vec4 v_5;
					  v_5.x = _World2Object[0].y;
					  v_5.y = _World2Object[1].y;
					  v_5.z = _World2Object[2].y;
					  v_5.w = _World2Object[3].y;
					  highp vec4 v_6;
					  v_6.x = _World2Object[0].z;
					  v_6.y = _World2Object[1].z;
					  v_6.z = _World2Object[2].z;
					  v_6.w = _World2Object[3].z;
					  highp vec3 tmpvar_7;
					  tmpvar_7 = normalize(((
					    (v_4.xyz * _glesNormal.x)
					   + 
					    (v_5.xyz * _glesNormal.y)
					  ) + (v_6.xyz * _glesNormal.z)));
					  worldNormal_1 = tmpvar_7;
					  tmpvar_3 = worldNormal_1;
					  gl_Position = tmpvar_2;
					  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
					  xlv_TEXCOORD1 = tmpvar_3;
					  xlv_TEXCOORD2 = (_Object2World * _glesVertex).xyz;
					  xlv_COLOR0 = _glesColor;
					  xlv_TEXCOORD3 = ((tmpvar_2.z * unity_FogParams.z) + unity_FogParams.w);
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform highp vec3 _WorldSpaceCameraPos;
					uniform mediump vec4 _WorldSpaceLightPos0;
					uniform lowp vec4 _LightColor0;
					uniform lowp vec4 _SpecColor;
					uniform sampler2D _LightTexture0;
					uniform mediump mat4 _LightMatrix0;
					uniform sampler2D _MainTex;
					uniform lowp vec4 _Color;
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
					  lowp float tmpvar_11;
					  lowp vec4 tmpvar_12;
					  tmpvar_12 = texture2D (_MainTex, xlv_TEXCOORD0);
					  tmpvar_10 = ((tmpvar_12.xyz * _Color.xyz) * tmpvar_7.xyz);
					  tmpvar_11 = ((tmpvar_12.w * _Color.w) * tmpvar_7.w);
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
					  tmpvar_24 = (pow (nh_19, y_23) * tmpvar_12.w);
					  c_18.xyz = (((tmpvar_10 * tmpvar_1) * diff_20) + ((tmpvar_1 * _SpecColor.xyz) * tmpvar_24));
					  c_18.w = tmpvar_11;
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
}
Program "fp" {
SubProgram "gles " {
Keywords { "POINT" }
					"!!GLES"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" }
					"!!GLES"
}
SubProgram "gles " {
Keywords { "SPOT" }
					"!!GLES"
}
SubProgram "gles " {
Keywords { "POINT_COOKIE" }
					"!!GLES"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL_COOKIE" }
					"!!GLES"
}
SubProgram "gles " {
Keywords { "POINT" "FOG_LINEAR" }
					"!!GLES"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "FOG_LINEAR" }
					"!!GLES"
}
SubProgram "gles " {
Keywords { "SPOT" "FOG_LINEAR" }
					"!!GLES"
}
SubProgram "gles " {
Keywords { "POINT_COOKIE" "FOG_LINEAR" }
					"!!GLES"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL_COOKIE" "FOG_LINEAR" }
					"!!GLES"
}
}
 }
}
Fallback "Legacy Shaders/Transparent/VertexLit"
}