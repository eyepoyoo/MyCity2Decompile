Shader "Custom/City/No Curve Vac Legacy Special Boat Render" {
Properties {
 _Color ("Main Color", Color) = (1,1,1,1)
 _SpecColor ("Specular Color", Color) = (0.5,0.5,0.5,1)
 _Shininess ("Shininess", Range(0.01,1)) = 0.078125
 _MainTex ("Base (RGB) Gloss (A)", 2D) = "white" { }
}
SubShader { 
 LOD 300
 Tags { "RenderType"="Opaque" "RenderQueue"="Geometry+1" }
 Pass {
  Name "FORWARD"
  Tags { "LIGHTMODE"="ForwardBase" "SHADOWSUPPORT"="true" "RenderType"="Opaque" "RenderQueue"="Geometry+1" }
  ZTest Greater
  GpuProgramID 56680
Program "vp" {
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_OFF" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" }
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
					  tmpvar_11 = texture2D (_MainTex, xlv_TEXCOORD0);
					  highp float x_12;
					  x_12 = -(xlv_TEXCOORD2.y);
					  if ((x_12 < 0.0)) {
					    discard;
					  };
					  highp float x_13;
					  x_13 = (xlv_TEXCOORD0.x - 0.5);
					  if ((x_13 < 0.0)) {
					    discard;
					  };
					  tmpvar_10 = ((tmpvar_11.xyz * _Color.xyz) * tmpvar_7.xyz);
					  tmpvar_1 = _LightColor0.xyz;
					  tmpvar_2 = lightDir_6;
					  mediump vec3 viewDir_14;
					  viewDir_14 = worldViewDir_5;
					  lowp vec4 c_15;
					  lowp vec4 c_16;
					  highp float nh_17;
					  lowp float diff_18;
					  mediump float tmpvar_19;
					  tmpvar_19 = max (0.0, dot (tmpvar_4, tmpvar_2));
					  diff_18 = tmpvar_19;
					  mediump float tmpvar_20;
					  tmpvar_20 = max (0.0, dot (tmpvar_4, normalize(
					    (tmpvar_2 + viewDir_14)
					  )));
					  nh_17 = tmpvar_20;
					  mediump float y_21;
					  y_21 = (_Shininess * 128.0);
					  highp float tmpvar_22;
					  tmpvar_22 = (pow (nh_17, y_21) * tmpvar_11.w);
					  c_16.xyz = (((tmpvar_10 * tmpvar_1) * diff_18) + ((tmpvar_1 * _SpecColor.xyz) * tmpvar_22));
					  c_16.w = (tmpvar_11.w * _Color.w);
					  c_15.w = c_16.w;
					  c_15.xyz = (c_16.xyz + (tmpvar_10 * xlv_TEXCOORD3));
					  c_3.xyz = c_15.xyz;
					  c_3.w = 1.0;
					  gl_FragData[0] = c_3;
					}
					
					
					#endif"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" }
					"!!GLES
					#version 100
					
					#ifdef VERTEX
					attribute vec4 _glesVertex;
					attribute vec4 _glesColor;
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
					uniform highp vec4 _MainTex_ST;
					varying highp vec2 xlv_TEXCOORD0;
					varying mediump vec3 xlv_TEXCOORD1;
					varying highp vec3 xlv_TEXCOORD2;
					varying lowp vec4 xlv_COLOR0;
					varying mediump vec3 xlv_TEXCOORD3;
					varying mediump vec4 xlv_TEXCOORD4;
					void main ()
					{
					  lowp vec3 worldNormal_1;
					  highp vec4 tmpvar_2;
					  mediump vec3 tmpvar_3;
					  mediump vec4 tmpvar_4;
					  tmpvar_2 = (glstate_matrix_mvp * _glesVertex);
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
					  tmpvar_3 = worldNormal_1;
					  mediump vec3 normal_9;
					  normal_9 = worldNormal_1;
					  mediump vec4 tmpvar_10;
					  tmpvar_10.w = 1.0;
					  tmpvar_10.xyz = normal_9;
					  mediump vec3 res_11;
					  mediump vec3 x_12;
					  x_12.x = dot (unity_SHAr, tmpvar_10);
					  x_12.y = dot (unity_SHAg, tmpvar_10);
					  x_12.z = dot (unity_SHAb, tmpvar_10);
					  mediump vec3 x1_13;
					  mediump vec4 tmpvar_14;
					  tmpvar_14 = (normal_9.xyzz * normal_9.yzzx);
					  x1_13.x = dot (unity_SHBr, tmpvar_14);
					  x1_13.y = dot (unity_SHBg, tmpvar_14);
					  x1_13.z = dot (unity_SHBb, tmpvar_14);
					  res_11 = (x_12 + (x1_13 + (unity_SHC.xyz * 
					    ((normal_9.x * normal_9.x) - (normal_9.y * normal_9.y))
					  )));
					  res_11 = max (((1.055 * 
					    pow (max (res_11, vec3(0.0, 0.0, 0.0)), vec3(0.4166667, 0.4166667, 0.4166667))
					  ) - 0.055), vec3(0.0, 0.0, 0.0));
					  highp vec4 o_15;
					  highp vec4 tmpvar_16;
					  tmpvar_16 = (tmpvar_2 * 0.5);
					  highp vec2 tmpvar_17;
					  tmpvar_17.x = tmpvar_16.x;
					  tmpvar_17.y = (tmpvar_16.y * _ProjectionParams.x);
					  o_15.xy = (tmpvar_17 + tmpvar_16.w);
					  o_15.zw = tmpvar_2.zw;
					  tmpvar_4 = o_15;
					  gl_Position = tmpvar_2;
					  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
					  xlv_TEXCOORD1 = tmpvar_3;
					  xlv_TEXCOORD2 = (_Object2World * _glesVertex).xyz;
					  xlv_COLOR0 = _glesColor;
					  xlv_TEXCOORD3 = max (vec3(0.0, 0.0, 0.0), res_11);
					  xlv_TEXCOORD4 = tmpvar_4;
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform highp vec3 _WorldSpaceCameraPos;
					uniform mediump vec4 _WorldSpaceLightPos0;
					uniform lowp vec4 _LightColor0;
					uniform lowp vec4 _SpecColor;
					uniform sampler2D _ShadowMapTexture;
					uniform sampler2D _MainTex;
					uniform lowp vec4 _Color;
					uniform mediump float _Shininess;
					varying highp vec2 xlv_TEXCOORD0;
					varying mediump vec3 xlv_TEXCOORD1;
					varying highp vec3 xlv_TEXCOORD2;
					varying lowp vec4 xlv_COLOR0;
					varying mediump vec3 xlv_TEXCOORD3;
					varying mediump vec4 xlv_TEXCOORD4;
					void main ()
					{
					  mediump float tmpvar_1;
					  mediump vec3 tmpvar_2;
					  mediump vec3 tmpvar_3;
					  lowp vec4 c_4;
					  lowp vec3 tmpvar_5;
					  lowp vec3 worldViewDir_6;
					  lowp vec3 lightDir_7;
					  highp vec4 tmpvar_8;
					  mediump vec3 tmpvar_9;
					  tmpvar_9 = _WorldSpaceLightPos0.xyz;
					  lightDir_7 = tmpvar_9;
					  highp vec3 tmpvar_10;
					  tmpvar_10 = normalize((_WorldSpaceCameraPos - xlv_TEXCOORD2));
					  worldViewDir_6 = tmpvar_10;
					  tmpvar_8 = xlv_COLOR0;
					  tmpvar_5 = xlv_TEXCOORD1;
					  lowp vec3 tmpvar_11;
					  lowp vec4 tmpvar_12;
					  tmpvar_12 = texture2D (_MainTex, xlv_TEXCOORD0);
					  highp float x_13;
					  x_13 = -(xlv_TEXCOORD2.y);
					  if ((x_13 < 0.0)) {
					    discard;
					  };
					  highp float x_14;
					  x_14 = (xlv_TEXCOORD0.x - 0.5);
					  if ((x_14 < 0.0)) {
					    discard;
					  };
					  tmpvar_11 = ((tmpvar_12.xyz * _Color.xyz) * tmpvar_8.xyz);
					  lowp float tmpvar_15;
					  tmpvar_15 = texture2DProj (_ShadowMapTexture, xlv_TEXCOORD4).x;
					  tmpvar_2 = _LightColor0.xyz;
					  tmpvar_3 = lightDir_7;
					  tmpvar_1 = tmpvar_15;
					  mediump vec3 tmpvar_16;
					  tmpvar_16 = (tmpvar_2 * tmpvar_1);
					  tmpvar_2 = tmpvar_16;
					  mediump vec3 viewDir_17;
					  viewDir_17 = worldViewDir_6;
					  lowp vec4 c_18;
					  lowp vec4 c_19;
					  highp float nh_20;
					  lowp float diff_21;
					  mediump float tmpvar_22;
					  tmpvar_22 = max (0.0, dot (tmpvar_5, tmpvar_3));
					  diff_21 = tmpvar_22;
					  mediump float tmpvar_23;
					  tmpvar_23 = max (0.0, dot (tmpvar_5, normalize(
					    (tmpvar_3 + viewDir_17)
					  )));
					  nh_20 = tmpvar_23;
					  mediump float y_24;
					  y_24 = (_Shininess * 128.0);
					  highp float tmpvar_25;
					  tmpvar_25 = (pow (nh_20, y_24) * tmpvar_12.w);
					  c_19.xyz = (((tmpvar_11 * tmpvar_16) * diff_21) + ((tmpvar_16 * _SpecColor.xyz) * tmpvar_25));
					  c_19.w = (tmpvar_12.w * _Color.w);
					  c_18.w = c_19.w;
					  c_18.xyz = (c_19.xyz + (tmpvar_11 * xlv_TEXCOORD3));
					  c_4.xyz = c_18.xyz;
					  c_4.w = 1.0;
					  gl_FragData[0] = c_4;
					}
					
					
					#endif"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_OFF" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "VERTEXLIGHT_ON" }
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
					  tmpvar_11 = texture2D (_MainTex, xlv_TEXCOORD0);
					  highp float x_12;
					  x_12 = -(xlv_TEXCOORD2.y);
					  if ((x_12 < 0.0)) {
					    discard;
					  };
					  highp float x_13;
					  x_13 = (xlv_TEXCOORD0.x - 0.5);
					  if ((x_13 < 0.0)) {
					    discard;
					  };
					  tmpvar_10 = ((tmpvar_11.xyz * _Color.xyz) * tmpvar_7.xyz);
					  tmpvar_1 = _LightColor0.xyz;
					  tmpvar_2 = lightDir_6;
					  mediump vec3 viewDir_14;
					  viewDir_14 = worldViewDir_5;
					  lowp vec4 c_15;
					  lowp vec4 c_16;
					  highp float nh_17;
					  lowp float diff_18;
					  mediump float tmpvar_19;
					  tmpvar_19 = max (0.0, dot (tmpvar_4, tmpvar_2));
					  diff_18 = tmpvar_19;
					  mediump float tmpvar_20;
					  tmpvar_20 = max (0.0, dot (tmpvar_4, normalize(
					    (tmpvar_2 + viewDir_14)
					  )));
					  nh_17 = tmpvar_20;
					  mediump float y_21;
					  y_21 = (_Shininess * 128.0);
					  highp float tmpvar_22;
					  tmpvar_22 = (pow (nh_17, y_21) * tmpvar_11.w);
					  c_16.xyz = (((tmpvar_10 * tmpvar_1) * diff_18) + ((tmpvar_1 * _SpecColor.xyz) * tmpvar_22));
					  c_16.w = (tmpvar_11.w * _Color.w);
					  c_15.w = c_16.w;
					  c_15.xyz = (c_16.xyz + (tmpvar_10 * xlv_TEXCOORD3));
					  c_3.xyz = c_15.xyz;
					  c_3.w = 1.0;
					  gl_FragData[0] = c_3;
					}
					
					
					#endif"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "VERTEXLIGHT_ON" }
					"!!GLES
					#version 100
					
					#ifdef VERTEX
					attribute vec4 _glesVertex;
					attribute vec4 _glesColor;
					attribute vec3 _glesNormal;
					attribute vec4 _glesMultiTexCoord0;
					uniform highp vec4 _ProjectionParams;
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
					varying mediump vec4 xlv_TEXCOORD4;
					void main ()
					{
					  lowp vec3 worldNormal_1;
					  highp vec4 tmpvar_2;
					  mediump vec3 tmpvar_3;
					  mediump vec3 tmpvar_4;
					  mediump vec4 tmpvar_5;
					  tmpvar_2 = (glstate_matrix_mvp * _glesVertex);
					  highp vec3 tmpvar_6;
					  tmpvar_6 = (_Object2World * _glesVertex).xyz;
					  highp vec4 v_7;
					  v_7.x = _World2Object[0].x;
					  v_7.y = _World2Object[1].x;
					  v_7.z = _World2Object[2].x;
					  v_7.w = _World2Object[3].x;
					  highp vec4 v_8;
					  v_8.x = _World2Object[0].y;
					  v_8.y = _World2Object[1].y;
					  v_8.z = _World2Object[2].y;
					  v_8.w = _World2Object[3].y;
					  highp vec4 v_9;
					  v_9.x = _World2Object[0].z;
					  v_9.y = _World2Object[1].z;
					  v_9.z = _World2Object[2].z;
					  v_9.w = _World2Object[3].z;
					  highp vec3 tmpvar_10;
					  tmpvar_10 = normalize(((
					    (v_7.xyz * _glesNormal.x)
					   + 
					    (v_8.xyz * _glesNormal.y)
					  ) + (v_9.xyz * _glesNormal.z)));
					  worldNormal_1 = tmpvar_10;
					  tmpvar_3 = worldNormal_1;
					  highp vec3 lightColor0_11;
					  lightColor0_11 = unity_LightColor[0].xyz;
					  highp vec3 lightColor1_12;
					  lightColor1_12 = unity_LightColor[1].xyz;
					  highp vec3 lightColor2_13;
					  lightColor2_13 = unity_LightColor[2].xyz;
					  highp vec3 lightColor3_14;
					  lightColor3_14 = unity_LightColor[3].xyz;
					  highp vec4 lightAttenSq_15;
					  lightAttenSq_15 = unity_4LightAtten0;
					  highp vec3 normal_16;
					  normal_16 = worldNormal_1;
					  highp vec3 col_17;
					  highp vec4 ndotl_18;
					  highp vec4 lengthSq_19;
					  highp vec4 tmpvar_20;
					  tmpvar_20 = (unity_4LightPosX0 - tmpvar_6.x);
					  highp vec4 tmpvar_21;
					  tmpvar_21 = (unity_4LightPosY0 - tmpvar_6.y);
					  highp vec4 tmpvar_22;
					  tmpvar_22 = (unity_4LightPosZ0 - tmpvar_6.z);
					  lengthSq_19 = (tmpvar_20 * tmpvar_20);
					  lengthSq_19 = (lengthSq_19 + (tmpvar_21 * tmpvar_21));
					  lengthSq_19 = (lengthSq_19 + (tmpvar_22 * tmpvar_22));
					  ndotl_18 = (tmpvar_20 * normal_16.x);
					  ndotl_18 = (ndotl_18 + (tmpvar_21 * normal_16.y));
					  ndotl_18 = (ndotl_18 + (tmpvar_22 * normal_16.z));
					  highp vec4 tmpvar_23;
					  tmpvar_23 = max (vec4(0.0, 0.0, 0.0, 0.0), (ndotl_18 * inversesqrt(lengthSq_19)));
					  ndotl_18 = tmpvar_23;
					  highp vec4 tmpvar_24;
					  tmpvar_24 = (tmpvar_23 * (1.0/((1.0 + 
					    (lengthSq_19 * lightAttenSq_15)
					  ))));
					  col_17 = (lightColor0_11 * tmpvar_24.x);
					  col_17 = (col_17 + (lightColor1_12 * tmpvar_24.y));
					  col_17 = (col_17 + (lightColor2_13 * tmpvar_24.z));
					  col_17 = (col_17 + (lightColor3_14 * tmpvar_24.w));
					  tmpvar_4 = col_17;
					  mediump vec3 normal_25;
					  normal_25 = worldNormal_1;
					  mediump vec3 ambient_26;
					  mediump vec4 tmpvar_27;
					  tmpvar_27.w = 1.0;
					  tmpvar_27.xyz = normal_25;
					  mediump vec3 res_28;
					  mediump vec3 x_29;
					  x_29.x = dot (unity_SHAr, tmpvar_27);
					  x_29.y = dot (unity_SHAg, tmpvar_27);
					  x_29.z = dot (unity_SHAb, tmpvar_27);
					  mediump vec3 x1_30;
					  mediump vec4 tmpvar_31;
					  tmpvar_31 = (normal_25.xyzz * normal_25.yzzx);
					  x1_30.x = dot (unity_SHBr, tmpvar_31);
					  x1_30.y = dot (unity_SHBg, tmpvar_31);
					  x1_30.z = dot (unity_SHBb, tmpvar_31);
					  res_28 = (x_29 + (x1_30 + (unity_SHC.xyz * 
					    ((normal_25.x * normal_25.x) - (normal_25.y * normal_25.y))
					  )));
					  res_28 = max (((1.055 * 
					    pow (max (res_28, vec3(0.0, 0.0, 0.0)), vec3(0.4166667, 0.4166667, 0.4166667))
					  ) - 0.055), vec3(0.0, 0.0, 0.0));
					  ambient_26 = (tmpvar_4 + max (vec3(0.0, 0.0, 0.0), res_28));
					  tmpvar_4 = ambient_26;
					  highp vec4 o_32;
					  highp vec4 tmpvar_33;
					  tmpvar_33 = (tmpvar_2 * 0.5);
					  highp vec2 tmpvar_34;
					  tmpvar_34.x = tmpvar_33.x;
					  tmpvar_34.y = (tmpvar_33.y * _ProjectionParams.x);
					  o_32.xy = (tmpvar_34 + tmpvar_33.w);
					  o_32.zw = tmpvar_2.zw;
					  tmpvar_5 = o_32;
					  gl_Position = tmpvar_2;
					  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
					  xlv_TEXCOORD1 = tmpvar_3;
					  xlv_TEXCOORD2 = tmpvar_6;
					  xlv_COLOR0 = _glesColor;
					  xlv_TEXCOORD3 = ambient_26;
					  xlv_TEXCOORD4 = tmpvar_5;
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform highp vec3 _WorldSpaceCameraPos;
					uniform mediump vec4 _WorldSpaceLightPos0;
					uniform lowp vec4 _LightColor0;
					uniform lowp vec4 _SpecColor;
					uniform sampler2D _ShadowMapTexture;
					uniform sampler2D _MainTex;
					uniform lowp vec4 _Color;
					uniform mediump float _Shininess;
					varying highp vec2 xlv_TEXCOORD0;
					varying mediump vec3 xlv_TEXCOORD1;
					varying highp vec3 xlv_TEXCOORD2;
					varying lowp vec4 xlv_COLOR0;
					varying mediump vec3 xlv_TEXCOORD3;
					varying mediump vec4 xlv_TEXCOORD4;
					void main ()
					{
					  mediump float tmpvar_1;
					  mediump vec3 tmpvar_2;
					  mediump vec3 tmpvar_3;
					  lowp vec4 c_4;
					  lowp vec3 tmpvar_5;
					  lowp vec3 worldViewDir_6;
					  lowp vec3 lightDir_7;
					  highp vec4 tmpvar_8;
					  mediump vec3 tmpvar_9;
					  tmpvar_9 = _WorldSpaceLightPos0.xyz;
					  lightDir_7 = tmpvar_9;
					  highp vec3 tmpvar_10;
					  tmpvar_10 = normalize((_WorldSpaceCameraPos - xlv_TEXCOORD2));
					  worldViewDir_6 = tmpvar_10;
					  tmpvar_8 = xlv_COLOR0;
					  tmpvar_5 = xlv_TEXCOORD1;
					  lowp vec3 tmpvar_11;
					  lowp vec4 tmpvar_12;
					  tmpvar_12 = texture2D (_MainTex, xlv_TEXCOORD0);
					  highp float x_13;
					  x_13 = -(xlv_TEXCOORD2.y);
					  if ((x_13 < 0.0)) {
					    discard;
					  };
					  highp float x_14;
					  x_14 = (xlv_TEXCOORD0.x - 0.5);
					  if ((x_14 < 0.0)) {
					    discard;
					  };
					  tmpvar_11 = ((tmpvar_12.xyz * _Color.xyz) * tmpvar_8.xyz);
					  lowp float tmpvar_15;
					  tmpvar_15 = texture2DProj (_ShadowMapTexture, xlv_TEXCOORD4).x;
					  tmpvar_2 = _LightColor0.xyz;
					  tmpvar_3 = lightDir_7;
					  tmpvar_1 = tmpvar_15;
					  mediump vec3 tmpvar_16;
					  tmpvar_16 = (tmpvar_2 * tmpvar_1);
					  tmpvar_2 = tmpvar_16;
					  mediump vec3 viewDir_17;
					  viewDir_17 = worldViewDir_6;
					  lowp vec4 c_18;
					  lowp vec4 c_19;
					  highp float nh_20;
					  lowp float diff_21;
					  mediump float tmpvar_22;
					  tmpvar_22 = max (0.0, dot (tmpvar_5, tmpvar_3));
					  diff_21 = tmpvar_22;
					  mediump float tmpvar_23;
					  tmpvar_23 = max (0.0, dot (tmpvar_5, normalize(
					    (tmpvar_3 + viewDir_17)
					  )));
					  nh_20 = tmpvar_23;
					  mediump float y_24;
					  y_24 = (_Shininess * 128.0);
					  highp float tmpvar_25;
					  tmpvar_25 = (pow (nh_20, y_24) * tmpvar_12.w);
					  c_19.xyz = (((tmpvar_11 * tmpvar_16) * diff_21) + ((tmpvar_16 * _SpecColor.xyz) * tmpvar_25));
					  c_19.w = (tmpvar_12.w * _Color.w);
					  c_18.w = c_19.w;
					  c_18.xyz = (c_19.xyz + (tmpvar_11 * xlv_TEXCOORD3));
					  c_4.xyz = c_18.xyz;
					  c_4.w = 1.0;
					  gl_FragData[0] = c_4;
					}
					
					
					#endif"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_OFF" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "FOG_LINEAR" }
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
					varying highp float xlv_TEXCOORD5;
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
					  xlv_TEXCOORD5 = ((tmpvar_2.z * unity_FogParams.z) + unity_FogParams.w);
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
					varying highp float xlv_TEXCOORD5;
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
					  tmpvar_11 = texture2D (_MainTex, xlv_TEXCOORD0);
					  highp float x_12;
					  x_12 = -(xlv_TEXCOORD2.y);
					  if ((x_12 < 0.0)) {
					    discard;
					  };
					  highp float x_13;
					  x_13 = (xlv_TEXCOORD0.x - 0.5);
					  if ((x_13 < 0.0)) {
					    discard;
					  };
					  tmpvar_10 = ((tmpvar_11.xyz * _Color.xyz) * tmpvar_7.xyz);
					  tmpvar_1 = _LightColor0.xyz;
					  tmpvar_2 = lightDir_6;
					  mediump vec3 viewDir_14;
					  viewDir_14 = worldViewDir_5;
					  lowp vec4 c_15;
					  lowp vec4 c_16;
					  highp float nh_17;
					  lowp float diff_18;
					  mediump float tmpvar_19;
					  tmpvar_19 = max (0.0, dot (tmpvar_4, tmpvar_2));
					  diff_18 = tmpvar_19;
					  mediump float tmpvar_20;
					  tmpvar_20 = max (0.0, dot (tmpvar_4, normalize(
					    (tmpvar_2 + viewDir_14)
					  )));
					  nh_17 = tmpvar_20;
					  mediump float y_21;
					  y_21 = (_Shininess * 128.0);
					  highp float tmpvar_22;
					  tmpvar_22 = (pow (nh_17, y_21) * tmpvar_11.w);
					  c_16.xyz = (((tmpvar_10 * tmpvar_1) * diff_18) + ((tmpvar_1 * _SpecColor.xyz) * tmpvar_22));
					  c_16.w = (tmpvar_11.w * _Color.w);
					  c_15.w = c_16.w;
					  c_15.xyz = (c_16.xyz + (tmpvar_10 * xlv_TEXCOORD3));
					  highp float tmpvar_23;
					  tmpvar_23 = clamp (xlv_TEXCOORD5, 0.0, 1.0);
					  c_3.xyz = mix (unity_FogColor.xyz, c_15.xyz, vec3(tmpvar_23));
					  c_3.w = 1.0;
					  gl_FragData[0] = c_3;
					}
					
					
					#endif"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "FOG_LINEAR" }
					"!!GLES
					#version 100
					
					#ifdef VERTEX
					attribute vec4 _glesVertex;
					attribute vec4 _glesColor;
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
					uniform highp vec4 _MainTex_ST;
					varying highp vec2 xlv_TEXCOORD0;
					varying mediump vec3 xlv_TEXCOORD1;
					varying highp vec3 xlv_TEXCOORD2;
					varying lowp vec4 xlv_COLOR0;
					varying mediump vec3 xlv_TEXCOORD3;
					varying mediump vec4 xlv_TEXCOORD4;
					varying highp float xlv_TEXCOORD5;
					void main ()
					{
					  lowp vec3 worldNormal_1;
					  highp vec4 tmpvar_2;
					  mediump vec3 tmpvar_3;
					  mediump vec4 tmpvar_4;
					  tmpvar_2 = (glstate_matrix_mvp * _glesVertex);
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
					  tmpvar_3 = worldNormal_1;
					  mediump vec3 normal_9;
					  normal_9 = worldNormal_1;
					  mediump vec4 tmpvar_10;
					  tmpvar_10.w = 1.0;
					  tmpvar_10.xyz = normal_9;
					  mediump vec3 res_11;
					  mediump vec3 x_12;
					  x_12.x = dot (unity_SHAr, tmpvar_10);
					  x_12.y = dot (unity_SHAg, tmpvar_10);
					  x_12.z = dot (unity_SHAb, tmpvar_10);
					  mediump vec3 x1_13;
					  mediump vec4 tmpvar_14;
					  tmpvar_14 = (normal_9.xyzz * normal_9.yzzx);
					  x1_13.x = dot (unity_SHBr, tmpvar_14);
					  x1_13.y = dot (unity_SHBg, tmpvar_14);
					  x1_13.z = dot (unity_SHBb, tmpvar_14);
					  res_11 = (x_12 + (x1_13 + (unity_SHC.xyz * 
					    ((normal_9.x * normal_9.x) - (normal_9.y * normal_9.y))
					  )));
					  res_11 = max (((1.055 * 
					    pow (max (res_11, vec3(0.0, 0.0, 0.0)), vec3(0.4166667, 0.4166667, 0.4166667))
					  ) - 0.055), vec3(0.0, 0.0, 0.0));
					  highp vec4 o_15;
					  highp vec4 tmpvar_16;
					  tmpvar_16 = (tmpvar_2 * 0.5);
					  highp vec2 tmpvar_17;
					  tmpvar_17.x = tmpvar_16.x;
					  tmpvar_17.y = (tmpvar_16.y * _ProjectionParams.x);
					  o_15.xy = (tmpvar_17 + tmpvar_16.w);
					  o_15.zw = tmpvar_2.zw;
					  tmpvar_4 = o_15;
					  gl_Position = tmpvar_2;
					  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
					  xlv_TEXCOORD1 = tmpvar_3;
					  xlv_TEXCOORD2 = (_Object2World * _glesVertex).xyz;
					  xlv_COLOR0 = _glesColor;
					  xlv_TEXCOORD3 = max (vec3(0.0, 0.0, 0.0), res_11);
					  xlv_TEXCOORD4 = tmpvar_4;
					  xlv_TEXCOORD5 = ((tmpvar_2.z * unity_FogParams.z) + unity_FogParams.w);
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform highp vec3 _WorldSpaceCameraPos;
					uniform mediump vec4 _WorldSpaceLightPos0;
					uniform lowp vec4 unity_FogColor;
					uniform lowp vec4 _LightColor0;
					uniform lowp vec4 _SpecColor;
					uniform sampler2D _ShadowMapTexture;
					uniform sampler2D _MainTex;
					uniform lowp vec4 _Color;
					uniform mediump float _Shininess;
					varying highp vec2 xlv_TEXCOORD0;
					varying mediump vec3 xlv_TEXCOORD1;
					varying highp vec3 xlv_TEXCOORD2;
					varying lowp vec4 xlv_COLOR0;
					varying mediump vec3 xlv_TEXCOORD3;
					varying mediump vec4 xlv_TEXCOORD4;
					varying highp float xlv_TEXCOORD5;
					void main ()
					{
					  mediump float tmpvar_1;
					  mediump vec3 tmpvar_2;
					  mediump vec3 tmpvar_3;
					  lowp vec4 c_4;
					  lowp vec3 tmpvar_5;
					  lowp vec3 worldViewDir_6;
					  lowp vec3 lightDir_7;
					  highp vec4 tmpvar_8;
					  mediump vec3 tmpvar_9;
					  tmpvar_9 = _WorldSpaceLightPos0.xyz;
					  lightDir_7 = tmpvar_9;
					  highp vec3 tmpvar_10;
					  tmpvar_10 = normalize((_WorldSpaceCameraPos - xlv_TEXCOORD2));
					  worldViewDir_6 = tmpvar_10;
					  tmpvar_8 = xlv_COLOR0;
					  tmpvar_5 = xlv_TEXCOORD1;
					  lowp vec3 tmpvar_11;
					  lowp vec4 tmpvar_12;
					  tmpvar_12 = texture2D (_MainTex, xlv_TEXCOORD0);
					  highp float x_13;
					  x_13 = -(xlv_TEXCOORD2.y);
					  if ((x_13 < 0.0)) {
					    discard;
					  };
					  highp float x_14;
					  x_14 = (xlv_TEXCOORD0.x - 0.5);
					  if ((x_14 < 0.0)) {
					    discard;
					  };
					  tmpvar_11 = ((tmpvar_12.xyz * _Color.xyz) * tmpvar_8.xyz);
					  lowp float tmpvar_15;
					  tmpvar_15 = texture2DProj (_ShadowMapTexture, xlv_TEXCOORD4).x;
					  tmpvar_2 = _LightColor0.xyz;
					  tmpvar_3 = lightDir_7;
					  tmpvar_1 = tmpvar_15;
					  mediump vec3 tmpvar_16;
					  tmpvar_16 = (tmpvar_2 * tmpvar_1);
					  tmpvar_2 = tmpvar_16;
					  mediump vec3 viewDir_17;
					  viewDir_17 = worldViewDir_6;
					  lowp vec4 c_18;
					  lowp vec4 c_19;
					  highp float nh_20;
					  lowp float diff_21;
					  mediump float tmpvar_22;
					  tmpvar_22 = max (0.0, dot (tmpvar_5, tmpvar_3));
					  diff_21 = tmpvar_22;
					  mediump float tmpvar_23;
					  tmpvar_23 = max (0.0, dot (tmpvar_5, normalize(
					    (tmpvar_3 + viewDir_17)
					  )));
					  nh_20 = tmpvar_23;
					  mediump float y_24;
					  y_24 = (_Shininess * 128.0);
					  highp float tmpvar_25;
					  tmpvar_25 = (pow (nh_20, y_24) * tmpvar_12.w);
					  c_19.xyz = (((tmpvar_11 * tmpvar_16) * diff_21) + ((tmpvar_16 * _SpecColor.xyz) * tmpvar_25));
					  c_19.w = (tmpvar_12.w * _Color.w);
					  c_18.w = c_19.w;
					  c_18.xyz = (c_19.xyz + (tmpvar_11 * xlv_TEXCOORD3));
					  highp float tmpvar_26;
					  tmpvar_26 = clamp (xlv_TEXCOORD5, 0.0, 1.0);
					  c_4.xyz = mix (unity_FogColor.xyz, c_18.xyz, vec3(tmpvar_26));
					  c_4.w = 1.0;
					  gl_FragData[0] = c_4;
					}
					
					
					#endif"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_OFF" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "FOG_LINEAR" "VERTEXLIGHT_ON" }
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
					varying highp float xlv_TEXCOORD5;
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
					  xlv_TEXCOORD5 = ((tmpvar_2.z * unity_FogParams.z) + unity_FogParams.w);
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
					varying highp float xlv_TEXCOORD5;
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
					  tmpvar_11 = texture2D (_MainTex, xlv_TEXCOORD0);
					  highp float x_12;
					  x_12 = -(xlv_TEXCOORD2.y);
					  if ((x_12 < 0.0)) {
					    discard;
					  };
					  highp float x_13;
					  x_13 = (xlv_TEXCOORD0.x - 0.5);
					  if ((x_13 < 0.0)) {
					    discard;
					  };
					  tmpvar_10 = ((tmpvar_11.xyz * _Color.xyz) * tmpvar_7.xyz);
					  tmpvar_1 = _LightColor0.xyz;
					  tmpvar_2 = lightDir_6;
					  mediump vec3 viewDir_14;
					  viewDir_14 = worldViewDir_5;
					  lowp vec4 c_15;
					  lowp vec4 c_16;
					  highp float nh_17;
					  lowp float diff_18;
					  mediump float tmpvar_19;
					  tmpvar_19 = max (0.0, dot (tmpvar_4, tmpvar_2));
					  diff_18 = tmpvar_19;
					  mediump float tmpvar_20;
					  tmpvar_20 = max (0.0, dot (tmpvar_4, normalize(
					    (tmpvar_2 + viewDir_14)
					  )));
					  nh_17 = tmpvar_20;
					  mediump float y_21;
					  y_21 = (_Shininess * 128.0);
					  highp float tmpvar_22;
					  tmpvar_22 = (pow (nh_17, y_21) * tmpvar_11.w);
					  c_16.xyz = (((tmpvar_10 * tmpvar_1) * diff_18) + ((tmpvar_1 * _SpecColor.xyz) * tmpvar_22));
					  c_16.w = (tmpvar_11.w * _Color.w);
					  c_15.w = c_16.w;
					  c_15.xyz = (c_16.xyz + (tmpvar_10 * xlv_TEXCOORD3));
					  highp float tmpvar_23;
					  tmpvar_23 = clamp (xlv_TEXCOORD5, 0.0, 1.0);
					  c_3.xyz = mix (unity_FogColor.xyz, c_15.xyz, vec3(tmpvar_23));
					  c_3.w = 1.0;
					  gl_FragData[0] = c_3;
					}
					
					
					#endif"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "FOG_LINEAR" "VERTEXLIGHT_ON" }
					"!!GLES
					#version 100
					
					#ifdef VERTEX
					attribute vec4 _glesVertex;
					attribute vec4 _glesColor;
					attribute vec3 _glesNormal;
					attribute vec4 _glesMultiTexCoord0;
					uniform highp vec4 _ProjectionParams;
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
					varying mediump vec4 xlv_TEXCOORD4;
					varying highp float xlv_TEXCOORD5;
					void main ()
					{
					  lowp vec3 worldNormal_1;
					  highp vec4 tmpvar_2;
					  mediump vec3 tmpvar_3;
					  mediump vec3 tmpvar_4;
					  mediump vec4 tmpvar_5;
					  tmpvar_2 = (glstate_matrix_mvp * _glesVertex);
					  highp vec3 tmpvar_6;
					  tmpvar_6 = (_Object2World * _glesVertex).xyz;
					  highp vec4 v_7;
					  v_7.x = _World2Object[0].x;
					  v_7.y = _World2Object[1].x;
					  v_7.z = _World2Object[2].x;
					  v_7.w = _World2Object[3].x;
					  highp vec4 v_8;
					  v_8.x = _World2Object[0].y;
					  v_8.y = _World2Object[1].y;
					  v_8.z = _World2Object[2].y;
					  v_8.w = _World2Object[3].y;
					  highp vec4 v_9;
					  v_9.x = _World2Object[0].z;
					  v_9.y = _World2Object[1].z;
					  v_9.z = _World2Object[2].z;
					  v_9.w = _World2Object[3].z;
					  highp vec3 tmpvar_10;
					  tmpvar_10 = normalize(((
					    (v_7.xyz * _glesNormal.x)
					   + 
					    (v_8.xyz * _glesNormal.y)
					  ) + (v_9.xyz * _glesNormal.z)));
					  worldNormal_1 = tmpvar_10;
					  tmpvar_3 = worldNormal_1;
					  highp vec3 lightColor0_11;
					  lightColor0_11 = unity_LightColor[0].xyz;
					  highp vec3 lightColor1_12;
					  lightColor1_12 = unity_LightColor[1].xyz;
					  highp vec3 lightColor2_13;
					  lightColor2_13 = unity_LightColor[2].xyz;
					  highp vec3 lightColor3_14;
					  lightColor3_14 = unity_LightColor[3].xyz;
					  highp vec4 lightAttenSq_15;
					  lightAttenSq_15 = unity_4LightAtten0;
					  highp vec3 normal_16;
					  normal_16 = worldNormal_1;
					  highp vec3 col_17;
					  highp vec4 ndotl_18;
					  highp vec4 lengthSq_19;
					  highp vec4 tmpvar_20;
					  tmpvar_20 = (unity_4LightPosX0 - tmpvar_6.x);
					  highp vec4 tmpvar_21;
					  tmpvar_21 = (unity_4LightPosY0 - tmpvar_6.y);
					  highp vec4 tmpvar_22;
					  tmpvar_22 = (unity_4LightPosZ0 - tmpvar_6.z);
					  lengthSq_19 = (tmpvar_20 * tmpvar_20);
					  lengthSq_19 = (lengthSq_19 + (tmpvar_21 * tmpvar_21));
					  lengthSq_19 = (lengthSq_19 + (tmpvar_22 * tmpvar_22));
					  ndotl_18 = (tmpvar_20 * normal_16.x);
					  ndotl_18 = (ndotl_18 + (tmpvar_21 * normal_16.y));
					  ndotl_18 = (ndotl_18 + (tmpvar_22 * normal_16.z));
					  highp vec4 tmpvar_23;
					  tmpvar_23 = max (vec4(0.0, 0.0, 0.0, 0.0), (ndotl_18 * inversesqrt(lengthSq_19)));
					  ndotl_18 = tmpvar_23;
					  highp vec4 tmpvar_24;
					  tmpvar_24 = (tmpvar_23 * (1.0/((1.0 + 
					    (lengthSq_19 * lightAttenSq_15)
					  ))));
					  col_17 = (lightColor0_11 * tmpvar_24.x);
					  col_17 = (col_17 + (lightColor1_12 * tmpvar_24.y));
					  col_17 = (col_17 + (lightColor2_13 * tmpvar_24.z));
					  col_17 = (col_17 + (lightColor3_14 * tmpvar_24.w));
					  tmpvar_4 = col_17;
					  mediump vec3 normal_25;
					  normal_25 = worldNormal_1;
					  mediump vec3 ambient_26;
					  mediump vec4 tmpvar_27;
					  tmpvar_27.w = 1.0;
					  tmpvar_27.xyz = normal_25;
					  mediump vec3 res_28;
					  mediump vec3 x_29;
					  x_29.x = dot (unity_SHAr, tmpvar_27);
					  x_29.y = dot (unity_SHAg, tmpvar_27);
					  x_29.z = dot (unity_SHAb, tmpvar_27);
					  mediump vec3 x1_30;
					  mediump vec4 tmpvar_31;
					  tmpvar_31 = (normal_25.xyzz * normal_25.yzzx);
					  x1_30.x = dot (unity_SHBr, tmpvar_31);
					  x1_30.y = dot (unity_SHBg, tmpvar_31);
					  x1_30.z = dot (unity_SHBb, tmpvar_31);
					  res_28 = (x_29 + (x1_30 + (unity_SHC.xyz * 
					    ((normal_25.x * normal_25.x) - (normal_25.y * normal_25.y))
					  )));
					  res_28 = max (((1.055 * 
					    pow (max (res_28, vec3(0.0, 0.0, 0.0)), vec3(0.4166667, 0.4166667, 0.4166667))
					  ) - 0.055), vec3(0.0, 0.0, 0.0));
					  ambient_26 = (tmpvar_4 + max (vec3(0.0, 0.0, 0.0), res_28));
					  tmpvar_4 = ambient_26;
					  highp vec4 o_32;
					  highp vec4 tmpvar_33;
					  tmpvar_33 = (tmpvar_2 * 0.5);
					  highp vec2 tmpvar_34;
					  tmpvar_34.x = tmpvar_33.x;
					  tmpvar_34.y = (tmpvar_33.y * _ProjectionParams.x);
					  o_32.xy = (tmpvar_34 + tmpvar_33.w);
					  o_32.zw = tmpvar_2.zw;
					  tmpvar_5 = o_32;
					  gl_Position = tmpvar_2;
					  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
					  xlv_TEXCOORD1 = tmpvar_3;
					  xlv_TEXCOORD2 = tmpvar_6;
					  xlv_COLOR0 = _glesColor;
					  xlv_TEXCOORD3 = ambient_26;
					  xlv_TEXCOORD4 = tmpvar_5;
					  xlv_TEXCOORD5 = ((tmpvar_2.z * unity_FogParams.z) + unity_FogParams.w);
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform highp vec3 _WorldSpaceCameraPos;
					uniform mediump vec4 _WorldSpaceLightPos0;
					uniform lowp vec4 unity_FogColor;
					uniform lowp vec4 _LightColor0;
					uniform lowp vec4 _SpecColor;
					uniform sampler2D _ShadowMapTexture;
					uniform sampler2D _MainTex;
					uniform lowp vec4 _Color;
					uniform mediump float _Shininess;
					varying highp vec2 xlv_TEXCOORD0;
					varying mediump vec3 xlv_TEXCOORD1;
					varying highp vec3 xlv_TEXCOORD2;
					varying lowp vec4 xlv_COLOR0;
					varying mediump vec3 xlv_TEXCOORD3;
					varying mediump vec4 xlv_TEXCOORD4;
					varying highp float xlv_TEXCOORD5;
					void main ()
					{
					  mediump float tmpvar_1;
					  mediump vec3 tmpvar_2;
					  mediump vec3 tmpvar_3;
					  lowp vec4 c_4;
					  lowp vec3 tmpvar_5;
					  lowp vec3 worldViewDir_6;
					  lowp vec3 lightDir_7;
					  highp vec4 tmpvar_8;
					  mediump vec3 tmpvar_9;
					  tmpvar_9 = _WorldSpaceLightPos0.xyz;
					  lightDir_7 = tmpvar_9;
					  highp vec3 tmpvar_10;
					  tmpvar_10 = normalize((_WorldSpaceCameraPos - xlv_TEXCOORD2));
					  worldViewDir_6 = tmpvar_10;
					  tmpvar_8 = xlv_COLOR0;
					  tmpvar_5 = xlv_TEXCOORD1;
					  lowp vec3 tmpvar_11;
					  lowp vec4 tmpvar_12;
					  tmpvar_12 = texture2D (_MainTex, xlv_TEXCOORD0);
					  highp float x_13;
					  x_13 = -(xlv_TEXCOORD2.y);
					  if ((x_13 < 0.0)) {
					    discard;
					  };
					  highp float x_14;
					  x_14 = (xlv_TEXCOORD0.x - 0.5);
					  if ((x_14 < 0.0)) {
					    discard;
					  };
					  tmpvar_11 = ((tmpvar_12.xyz * _Color.xyz) * tmpvar_8.xyz);
					  lowp float tmpvar_15;
					  tmpvar_15 = texture2DProj (_ShadowMapTexture, xlv_TEXCOORD4).x;
					  tmpvar_2 = _LightColor0.xyz;
					  tmpvar_3 = lightDir_7;
					  tmpvar_1 = tmpvar_15;
					  mediump vec3 tmpvar_16;
					  tmpvar_16 = (tmpvar_2 * tmpvar_1);
					  tmpvar_2 = tmpvar_16;
					  mediump vec3 viewDir_17;
					  viewDir_17 = worldViewDir_6;
					  lowp vec4 c_18;
					  lowp vec4 c_19;
					  highp float nh_20;
					  lowp float diff_21;
					  mediump float tmpvar_22;
					  tmpvar_22 = max (0.0, dot (tmpvar_5, tmpvar_3));
					  diff_21 = tmpvar_22;
					  mediump float tmpvar_23;
					  tmpvar_23 = max (0.0, dot (tmpvar_5, normalize(
					    (tmpvar_3 + viewDir_17)
					  )));
					  nh_20 = tmpvar_23;
					  mediump float y_24;
					  y_24 = (_Shininess * 128.0);
					  highp float tmpvar_25;
					  tmpvar_25 = (pow (nh_20, y_24) * tmpvar_12.w);
					  c_19.xyz = (((tmpvar_11 * tmpvar_16) * diff_21) + ((tmpvar_16 * _SpecColor.xyz) * tmpvar_25));
					  c_19.w = (tmpvar_12.w * _Color.w);
					  c_18.w = c_19.w;
					  c_18.xyz = (c_19.xyz + (tmpvar_11 * xlv_TEXCOORD3));
					  highp float tmpvar_26;
					  tmpvar_26 = clamp (xlv_TEXCOORD5, 0.0, 1.0);
					  c_4.xyz = mix (unity_FogColor.xyz, c_18.xyz, vec3(tmpvar_26));
					  c_4.w = 1.0;
					  gl_FragData[0] = c_4;
					}
					
					
					#endif"
}
}
Program "fp" {
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_OFF" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" }
					"!!GLES"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" }
					"!!GLES"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_OFF" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "FOG_LINEAR" }
					"!!GLES"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "FOG_LINEAR" }
					"!!GLES"
}
}
 }
 Pass {
  Name "FORWARD"
  Tags { "LIGHTMODE"="ForwardAdd" "RenderType"="Opaque" "RenderQueue"="Geometry+1" }
  ZTest Greater
  ZWrite Off
  Blend One One
  GpuProgramID 108080
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
					  tmpvar_11 = texture2D (_MainTex, xlv_TEXCOORD0);
					  highp float x_12;
					  x_12 = -(xlv_TEXCOORD2.y);
					  if ((x_12 < 0.0)) {
					    discard;
					  };
					  highp float x_13;
					  x_13 = (xlv_TEXCOORD0.x - 0.5);
					  if ((x_13 < 0.0)) {
					    discard;
					  };
					  tmpvar_10 = ((tmpvar_11.xyz * _Color.xyz) * tmpvar_7.xyz);
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
					  tmpvar_2 = lightDir_6;
					  tmpvar_1 = (tmpvar_1 * tmpvar_17);
					  mediump vec3 viewDir_18;
					  viewDir_18 = worldViewDir_5;
					  lowp vec4 c_19;
					  lowp vec4 c_20;
					  highp float nh_21;
					  lowp float diff_22;
					  mediump float tmpvar_23;
					  tmpvar_23 = max (0.0, dot (tmpvar_4, tmpvar_2));
					  diff_22 = tmpvar_23;
					  mediump float tmpvar_24;
					  tmpvar_24 = max (0.0, dot (tmpvar_4, normalize(
					    (tmpvar_2 + viewDir_18)
					  )));
					  nh_21 = tmpvar_24;
					  mediump float y_25;
					  y_25 = (_Shininess * 128.0);
					  highp float tmpvar_26;
					  tmpvar_26 = (pow (nh_21, y_25) * tmpvar_11.w);
					  c_20.xyz = (((tmpvar_10 * tmpvar_1) * diff_22) + ((tmpvar_1 * _SpecColor.xyz) * tmpvar_26));
					  c_20.w = (tmpvar_11.w * _Color.w);
					  c_19.w = c_20.w;
					  c_19.xyz = c_20.xyz;
					  c_3.xyz = c_19.xyz;
					  c_3.w = 1.0;
					  gl_FragData[0] = c_3;
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
					  tmpvar_11 = texture2D (_MainTex, xlv_TEXCOORD0);
					  highp float x_12;
					  x_12 = -(xlv_TEXCOORD2.y);
					  if ((x_12 < 0.0)) {
					    discard;
					  };
					  highp float x_13;
					  x_13 = (xlv_TEXCOORD0.x - 0.5);
					  if ((x_13 < 0.0)) {
					    discard;
					  };
					  tmpvar_10 = ((tmpvar_11.xyz * _Color.xyz) * tmpvar_7.xyz);
					  tmpvar_1 = _LightColor0.xyz;
					  tmpvar_2 = lightDir_6;
					  mediump vec3 viewDir_14;
					  viewDir_14 = worldViewDir_5;
					  lowp vec4 c_15;
					  lowp vec4 c_16;
					  highp float nh_17;
					  lowp float diff_18;
					  mediump float tmpvar_19;
					  tmpvar_19 = max (0.0, dot (tmpvar_4, tmpvar_2));
					  diff_18 = tmpvar_19;
					  mediump float tmpvar_20;
					  tmpvar_20 = max (0.0, dot (tmpvar_4, normalize(
					    (tmpvar_2 + viewDir_14)
					  )));
					  nh_17 = tmpvar_20;
					  mediump float y_21;
					  y_21 = (_Shininess * 128.0);
					  highp float tmpvar_22;
					  tmpvar_22 = (pow (nh_17, y_21) * tmpvar_11.w);
					  c_16.xyz = (((tmpvar_10 * tmpvar_1) * diff_18) + ((tmpvar_1 * _SpecColor.xyz) * tmpvar_22));
					  c_16.w = (tmpvar_11.w * _Color.w);
					  c_15.w = c_16.w;
					  c_15.xyz = c_16.xyz;
					  c_3.xyz = c_15.xyz;
					  c_3.w = 1.0;
					  gl_FragData[0] = c_3;
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
					  tmpvar_13 = texture2D (_MainTex, xlv_TEXCOORD0);
					  highp float x_14;
					  x_14 = -(xlv_TEXCOORD2.y);
					  if ((x_14 < 0.0)) {
					    discard;
					  };
					  highp float x_15;
					  x_15 = (xlv_TEXCOORD0.x - 0.5);
					  if ((x_15 < 0.0)) {
					    discard;
					  };
					  tmpvar_12 = ((tmpvar_13.xyz * _Color.xyz) * tmpvar_9.xyz);
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
					  tmpvar_2 = lightDir_8;
					  tmpvar_1 = (tmpvar_1 * atten_4);
					  mediump vec3 viewDir_24;
					  viewDir_24 = worldViewDir_7;
					  lowp vec4 c_25;
					  lowp vec4 c_26;
					  highp float nh_27;
					  lowp float diff_28;
					  mediump float tmpvar_29;
					  tmpvar_29 = max (0.0, dot (tmpvar_6, tmpvar_2));
					  diff_28 = tmpvar_29;
					  mediump float tmpvar_30;
					  tmpvar_30 = max (0.0, dot (tmpvar_6, normalize(
					    (tmpvar_2 + viewDir_24)
					  )));
					  nh_27 = tmpvar_30;
					  mediump float y_31;
					  y_31 = (_Shininess * 128.0);
					  highp float tmpvar_32;
					  tmpvar_32 = (pow (nh_27, y_31) * tmpvar_13.w);
					  c_26.xyz = (((tmpvar_12 * tmpvar_1) * diff_28) + ((tmpvar_1 * _SpecColor.xyz) * tmpvar_32));
					  c_26.w = (tmpvar_13.w * _Color.w);
					  c_25.w = c_26.w;
					  c_25.xyz = c_26.xyz;
					  c_3.xyz = c_25.xyz;
					  c_3.w = 1.0;
					  gl_FragData[0] = c_3;
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
					  tmpvar_11 = texture2D (_MainTex, xlv_TEXCOORD0);
					  highp float x_12;
					  x_12 = -(xlv_TEXCOORD2.y);
					  if ((x_12 < 0.0)) {
					    discard;
					  };
					  highp float x_13;
					  x_13 = (xlv_TEXCOORD0.x - 0.5);
					  if ((x_13 < 0.0)) {
					    discard;
					  };
					  tmpvar_10 = ((tmpvar_11.xyz * _Color.xyz) * tmpvar_7.xyz);
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
					  tmpvar_2 = lightDir_6;
					  tmpvar_1 = (tmpvar_1 * tmpvar_17);
					  mediump vec3 viewDir_18;
					  viewDir_18 = worldViewDir_5;
					  lowp vec4 c_19;
					  lowp vec4 c_20;
					  highp float nh_21;
					  lowp float diff_22;
					  mediump float tmpvar_23;
					  tmpvar_23 = max (0.0, dot (tmpvar_4, tmpvar_2));
					  diff_22 = tmpvar_23;
					  mediump float tmpvar_24;
					  tmpvar_24 = max (0.0, dot (tmpvar_4, normalize(
					    (tmpvar_2 + viewDir_18)
					  )));
					  nh_21 = tmpvar_24;
					  mediump float y_25;
					  y_25 = (_Shininess * 128.0);
					  highp float tmpvar_26;
					  tmpvar_26 = (pow (nh_21, y_25) * tmpvar_11.w);
					  c_20.xyz = (((tmpvar_10 * tmpvar_1) * diff_22) + ((tmpvar_1 * _SpecColor.xyz) * tmpvar_26));
					  c_20.w = (tmpvar_11.w * _Color.w);
					  c_19.w = c_20.w;
					  c_19.xyz = c_20.xyz;
					  c_3.xyz = c_19.xyz;
					  c_3.w = 1.0;
					  gl_FragData[0] = c_3;
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
					  tmpvar_11 = texture2D (_MainTex, xlv_TEXCOORD0);
					  highp float x_12;
					  x_12 = -(xlv_TEXCOORD2.y);
					  if ((x_12 < 0.0)) {
					    discard;
					  };
					  highp float x_13;
					  x_13 = (xlv_TEXCOORD0.x - 0.5);
					  if ((x_13 < 0.0)) {
					    discard;
					  };
					  tmpvar_10 = ((tmpvar_11.xyz * _Color.xyz) * tmpvar_7.xyz);
					  highp vec4 tmpvar_14;
					  tmpvar_14.w = 1.0;
					  tmpvar_14.xyz = xlv_TEXCOORD2;
					  highp vec2 tmpvar_15;
					  tmpvar_15 = (_LightMatrix0 * tmpvar_14).xy;
					  lowp float tmpvar_16;
					  tmpvar_16 = texture2D (_LightTexture0, tmpvar_15).w;
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
					  c_3.xyz = c_18.xyz;
					  c_3.w = 1.0;
					  gl_FragData[0] = c_3;
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
					  gl_Position = tmpvar_2;
					  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
					  xlv_TEXCOORD1 = tmpvar_3;
					  xlv_TEXCOORD2 = (_Object2World * _glesVertex).xyz;
					  xlv_COLOR0 = _glesColor;
					  xlv_TEXCOORD4 = ((tmpvar_2.z * unity_FogParams.z) + unity_FogParams.w);
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
					  tmpvar_11 = texture2D (_MainTex, xlv_TEXCOORD0);
					  highp float x_12;
					  x_12 = -(xlv_TEXCOORD2.y);
					  if ((x_12 < 0.0)) {
					    discard;
					  };
					  highp float x_13;
					  x_13 = (xlv_TEXCOORD0.x - 0.5);
					  if ((x_13 < 0.0)) {
					    discard;
					  };
					  tmpvar_10 = ((tmpvar_11.xyz * _Color.xyz) * tmpvar_7.xyz);
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
					  tmpvar_2 = lightDir_6;
					  tmpvar_1 = (tmpvar_1 * tmpvar_17);
					  mediump vec3 viewDir_18;
					  viewDir_18 = worldViewDir_5;
					  lowp vec4 c_19;
					  highp float nh_20;
					  lowp float diff_21;
					  mediump float tmpvar_22;
					  tmpvar_22 = max (0.0, dot (tmpvar_4, tmpvar_2));
					  diff_21 = tmpvar_22;
					  mediump float tmpvar_23;
					  tmpvar_23 = max (0.0, dot (tmpvar_4, normalize(
					    (tmpvar_2 + viewDir_18)
					  )));
					  nh_20 = tmpvar_23;
					  mediump float y_24;
					  y_24 = (_Shininess * 128.0);
					  highp float tmpvar_25;
					  tmpvar_25 = (pow (nh_20, y_24) * tmpvar_11.w);
					  c_19.xyz = (((tmpvar_10 * tmpvar_1) * diff_21) + ((tmpvar_1 * _SpecColor.xyz) * tmpvar_25));
					  c_19.w = (tmpvar_11.w * _Color.w);
					  highp float tmpvar_26;
					  tmpvar_26 = clamp (xlv_TEXCOORD4, 0.0, 1.0);
					  c_3.xyz = (c_19.xyz * vec3(tmpvar_26));
					  c_3.w = 1.0;
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
					  gl_Position = tmpvar_2;
					  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
					  xlv_TEXCOORD1 = tmpvar_3;
					  xlv_TEXCOORD2 = (_Object2World * _glesVertex).xyz;
					  xlv_COLOR0 = _glesColor;
					  xlv_TEXCOORD4 = ((tmpvar_2.z * unity_FogParams.z) + unity_FogParams.w);
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
					  tmpvar_11 = texture2D (_MainTex, xlv_TEXCOORD0);
					  highp float x_12;
					  x_12 = -(xlv_TEXCOORD2.y);
					  if ((x_12 < 0.0)) {
					    discard;
					  };
					  highp float x_13;
					  x_13 = (xlv_TEXCOORD0.x - 0.5);
					  if ((x_13 < 0.0)) {
					    discard;
					  };
					  tmpvar_10 = ((tmpvar_11.xyz * _Color.xyz) * tmpvar_7.xyz);
					  tmpvar_1 = _LightColor0.xyz;
					  tmpvar_2 = lightDir_6;
					  mediump vec3 viewDir_14;
					  viewDir_14 = worldViewDir_5;
					  lowp vec4 c_15;
					  highp float nh_16;
					  lowp float diff_17;
					  mediump float tmpvar_18;
					  tmpvar_18 = max (0.0, dot (tmpvar_4, tmpvar_2));
					  diff_17 = tmpvar_18;
					  mediump float tmpvar_19;
					  tmpvar_19 = max (0.0, dot (tmpvar_4, normalize(
					    (tmpvar_2 + viewDir_14)
					  )));
					  nh_16 = tmpvar_19;
					  mediump float y_20;
					  y_20 = (_Shininess * 128.0);
					  highp float tmpvar_21;
					  tmpvar_21 = (pow (nh_16, y_20) * tmpvar_11.w);
					  c_15.xyz = (((tmpvar_10 * tmpvar_1) * diff_17) + ((tmpvar_1 * _SpecColor.xyz) * tmpvar_21));
					  c_15.w = (tmpvar_11.w * _Color.w);
					  highp float tmpvar_22;
					  tmpvar_22 = clamp (xlv_TEXCOORD4, 0.0, 1.0);
					  c_3.xyz = (c_15.xyz * vec3(tmpvar_22));
					  c_3.w = 1.0;
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
					  gl_Position = tmpvar_2;
					  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
					  xlv_TEXCOORD1 = tmpvar_3;
					  xlv_TEXCOORD2 = (_Object2World * _glesVertex).xyz;
					  xlv_COLOR0 = _glesColor;
					  xlv_TEXCOORD4 = ((tmpvar_2.z * unity_FogParams.z) + unity_FogParams.w);
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
					varying highp float xlv_TEXCOORD4;
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
					  tmpvar_13 = texture2D (_MainTex, xlv_TEXCOORD0);
					  highp float x_14;
					  x_14 = -(xlv_TEXCOORD2.y);
					  if ((x_14 < 0.0)) {
					    discard;
					  };
					  highp float x_15;
					  x_15 = (xlv_TEXCOORD0.x - 0.5);
					  if ((x_15 < 0.0)) {
					    discard;
					  };
					  tmpvar_12 = ((tmpvar_13.xyz * _Color.xyz) * tmpvar_9.xyz);
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
					  tmpvar_2 = lightDir_8;
					  tmpvar_1 = (tmpvar_1 * atten_4);
					  mediump vec3 viewDir_24;
					  viewDir_24 = worldViewDir_7;
					  lowp vec4 c_25;
					  highp float nh_26;
					  lowp float diff_27;
					  mediump float tmpvar_28;
					  tmpvar_28 = max (0.0, dot (tmpvar_6, tmpvar_2));
					  diff_27 = tmpvar_28;
					  mediump float tmpvar_29;
					  tmpvar_29 = max (0.0, dot (tmpvar_6, normalize(
					    (tmpvar_2 + viewDir_24)
					  )));
					  nh_26 = tmpvar_29;
					  mediump float y_30;
					  y_30 = (_Shininess * 128.0);
					  highp float tmpvar_31;
					  tmpvar_31 = (pow (nh_26, y_30) * tmpvar_13.w);
					  c_25.xyz = (((tmpvar_12 * tmpvar_1) * diff_27) + ((tmpvar_1 * _SpecColor.xyz) * tmpvar_31));
					  c_25.w = (tmpvar_13.w * _Color.w);
					  highp float tmpvar_32;
					  tmpvar_32 = clamp (xlv_TEXCOORD4, 0.0, 1.0);
					  c_3.xyz = (c_25.xyz * vec3(tmpvar_32));
					  c_3.w = 1.0;
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
					  gl_Position = tmpvar_2;
					  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
					  xlv_TEXCOORD1 = tmpvar_3;
					  xlv_TEXCOORD2 = (_Object2World * _glesVertex).xyz;
					  xlv_COLOR0 = _glesColor;
					  xlv_TEXCOORD4 = ((tmpvar_2.z * unity_FogParams.z) + unity_FogParams.w);
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
					  tmpvar_11 = texture2D (_MainTex, xlv_TEXCOORD0);
					  highp float x_12;
					  x_12 = -(xlv_TEXCOORD2.y);
					  if ((x_12 < 0.0)) {
					    discard;
					  };
					  highp float x_13;
					  x_13 = (xlv_TEXCOORD0.x - 0.5);
					  if ((x_13 < 0.0)) {
					    discard;
					  };
					  tmpvar_10 = ((tmpvar_11.xyz * _Color.xyz) * tmpvar_7.xyz);
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
					  tmpvar_2 = lightDir_6;
					  tmpvar_1 = (tmpvar_1 * tmpvar_17);
					  mediump vec3 viewDir_18;
					  viewDir_18 = worldViewDir_5;
					  lowp vec4 c_19;
					  highp float nh_20;
					  lowp float diff_21;
					  mediump float tmpvar_22;
					  tmpvar_22 = max (0.0, dot (tmpvar_4, tmpvar_2));
					  diff_21 = tmpvar_22;
					  mediump float tmpvar_23;
					  tmpvar_23 = max (0.0, dot (tmpvar_4, normalize(
					    (tmpvar_2 + viewDir_18)
					  )));
					  nh_20 = tmpvar_23;
					  mediump float y_24;
					  y_24 = (_Shininess * 128.0);
					  highp float tmpvar_25;
					  tmpvar_25 = (pow (nh_20, y_24) * tmpvar_11.w);
					  c_19.xyz = (((tmpvar_10 * tmpvar_1) * diff_21) + ((tmpvar_1 * _SpecColor.xyz) * tmpvar_25));
					  c_19.w = (tmpvar_11.w * _Color.w);
					  highp float tmpvar_26;
					  tmpvar_26 = clamp (xlv_TEXCOORD4, 0.0, 1.0);
					  c_3.xyz = (c_19.xyz * vec3(tmpvar_26));
					  c_3.w = 1.0;
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
					  gl_Position = tmpvar_2;
					  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
					  xlv_TEXCOORD1 = tmpvar_3;
					  xlv_TEXCOORD2 = (_Object2World * _glesVertex).xyz;
					  xlv_COLOR0 = _glesColor;
					  xlv_TEXCOORD4 = ((tmpvar_2.z * unity_FogParams.z) + unity_FogParams.w);
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
					  tmpvar_11 = texture2D (_MainTex, xlv_TEXCOORD0);
					  highp float x_12;
					  x_12 = -(xlv_TEXCOORD2.y);
					  if ((x_12 < 0.0)) {
					    discard;
					  };
					  highp float x_13;
					  x_13 = (xlv_TEXCOORD0.x - 0.5);
					  if ((x_13 < 0.0)) {
					    discard;
					  };
					  tmpvar_10 = ((tmpvar_11.xyz * _Color.xyz) * tmpvar_7.xyz);
					  highp vec4 tmpvar_14;
					  tmpvar_14.w = 1.0;
					  tmpvar_14.xyz = xlv_TEXCOORD2;
					  highp vec2 tmpvar_15;
					  tmpvar_15 = (_LightMatrix0 * tmpvar_14).xy;
					  lowp float tmpvar_16;
					  tmpvar_16 = texture2D (_LightTexture0, tmpvar_15).w;
					  tmpvar_1 = _LightColor0.xyz;
					  tmpvar_2 = lightDir_6;
					  tmpvar_1 = (tmpvar_1 * tmpvar_16);
					  mediump vec3 viewDir_17;
					  viewDir_17 = worldViewDir_5;
					  lowp vec4 c_18;
					  highp float nh_19;
					  lowp float diff_20;
					  mediump float tmpvar_21;
					  tmpvar_21 = max (0.0, dot (tmpvar_4, tmpvar_2));
					  diff_20 = tmpvar_21;
					  mediump float tmpvar_22;
					  tmpvar_22 = max (0.0, dot (tmpvar_4, normalize(
					    (tmpvar_2 + viewDir_17)
					  )));
					  nh_19 = tmpvar_22;
					  mediump float y_23;
					  y_23 = (_Shininess * 128.0);
					  highp float tmpvar_24;
					  tmpvar_24 = (pow (nh_19, y_23) * tmpvar_11.w);
					  c_18.xyz = (((tmpvar_10 * tmpvar_1) * diff_20) + ((tmpvar_1 * _SpecColor.xyz) * tmpvar_24));
					  c_18.w = (tmpvar_11.w * _Color.w);
					  highp float tmpvar_25;
					  tmpvar_25 = clamp (xlv_TEXCOORD4, 0.0, 1.0);
					  c_3.xyz = (c_18.xyz * vec3(tmpvar_25));
					  c_3.w = 1.0;
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
 Pass {
  Name "PREPASS"
  Tags { "LIGHTMODE"="PrePassBase" "RenderType"="Opaque" "RenderQueue"="Geometry+1" }
  ZTest Greater
  GpuProgramID 138776
Program "vp" {
SubProgram "gles " {
"!!GLES
					#version 100
					
					#ifdef VERTEX
					attribute vec4 _glesVertex;
					attribute vec3 _glesNormal;
					attribute vec4 _glesMultiTexCoord0;
					uniform highp mat4 glstate_matrix_mvp;
					uniform highp mat4 _Object2World;
					uniform highp mat4 _World2Object;
					uniform highp vec4 _MainTex_ST;
					varying highp vec2 xlv_TEXCOORD0;
					varying mediump vec3 xlv_TEXCOORD1;
					varying highp vec3 xlv_TEXCOORD2;
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
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform mediump float _Shininess;
					varying highp vec2 xlv_TEXCOORD0;
					varying mediump vec3 xlv_TEXCOORD1;
					varying highp vec3 xlv_TEXCOORD2;
					void main ()
					{
					  lowp vec4 res_1;
					  lowp vec3 tmpvar_2;
					  tmpvar_2 = xlv_TEXCOORD1;
					  highp float x_3;
					  x_3 = -(xlv_TEXCOORD2.y);
					  if ((x_3 < 0.0)) {
					    discard;
					  };
					  highp float x_4;
					  x_4 = (xlv_TEXCOORD0.x - 0.5);
					  if ((x_4 < 0.0)) {
					    discard;
					  };
					  res_1.xyz = ((tmpvar_2 * 0.5) + 0.5);
					  res_1.w = _Shininess;
					  gl_FragData[0] = res_1;
					}
					
					
					#endif"
}
}
Program "fp" {
SubProgram "gles " {
"!!GLES"
}
}
 }
 Pass {
  Name "PREPASS"
  Tags { "LIGHTMODE"="PrePassFinal" "RenderType"="Opaque" "RenderQueue"="Geometry+1" }
  ZTest Greater
  ZWrite Off
  GpuProgramID 259105
Program "vp" {
SubProgram "gles " {
Keywords { "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "HDR_LIGHT_PREPASS_OFF" }
					"!!GLES
					#version 100
					
					#ifdef VERTEX
					attribute vec4 _glesVertex;
					attribute vec4 _glesColor;
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
					uniform highp vec4 _MainTex_ST;
					varying highp vec2 xlv_TEXCOORD0;
					varying highp vec3 xlv_TEXCOORD1;
					varying lowp vec4 xlv_COLOR0;
					varying highp vec4 xlv_TEXCOORD2;
					varying highp vec4 xlv_TEXCOORD3;
					varying highp vec3 xlv_TEXCOORD4;
					void main ()
					{
					  highp vec4 tmpvar_1;
					  highp vec4 tmpvar_2;
					  highp vec3 tmpvar_3;
					  tmpvar_1 = (glstate_matrix_mvp * _glesVertex);
					  highp vec4 o_4;
					  highp vec4 tmpvar_5;
					  tmpvar_5 = (tmpvar_1 * 0.5);
					  highp vec2 tmpvar_6;
					  tmpvar_6.x = tmpvar_5.x;
					  tmpvar_6.y = (tmpvar_5.y * _ProjectionParams.x);
					  o_4.xy = (tmpvar_6 + tmpvar_5.w);
					  o_4.zw = tmpvar_1.zw;
					  tmpvar_2.zw = vec2(0.0, 0.0);
					  tmpvar_2.xy = vec2(0.0, 0.0);
					  highp vec4 v_7;
					  v_7.x = _World2Object[0].x;
					  v_7.y = _World2Object[1].x;
					  v_7.z = _World2Object[2].x;
					  v_7.w = _World2Object[3].x;
					  highp vec4 v_8;
					  v_8.x = _World2Object[0].y;
					  v_8.y = _World2Object[1].y;
					  v_8.z = _World2Object[2].y;
					  v_8.w = _World2Object[3].y;
					  highp vec4 v_9;
					  v_9.x = _World2Object[0].z;
					  v_9.y = _World2Object[1].z;
					  v_9.z = _World2Object[2].z;
					  v_9.w = _World2Object[3].z;
					  highp vec4 tmpvar_10;
					  tmpvar_10.w = 1.0;
					  tmpvar_10.xyz = normalize(((
					    (v_7.xyz * _glesNormal.x)
					   + 
					    (v_8.xyz * _glesNormal.y)
					  ) + (v_9.xyz * _glesNormal.z)));
					  mediump vec4 normal_11;
					  normal_11 = tmpvar_10;
					  mediump vec3 res_12;
					  mediump vec3 x_13;
					  x_13.x = dot (unity_SHAr, normal_11);
					  x_13.y = dot (unity_SHAg, normal_11);
					  x_13.z = dot (unity_SHAb, normal_11);
					  mediump vec3 x1_14;
					  mediump vec4 tmpvar_15;
					  tmpvar_15 = (normal_11.xyzz * normal_11.yzzx);
					  x1_14.x = dot (unity_SHBr, tmpvar_15);
					  x1_14.y = dot (unity_SHBg, tmpvar_15);
					  x1_14.z = dot (unity_SHBb, tmpvar_15);
					  res_12 = (x_13 + (x1_14 + (unity_SHC.xyz * 
					    ((normal_11.x * normal_11.x) - (normal_11.y * normal_11.y))
					  )));
					  res_12 = max (((1.055 * 
					    pow (max (res_12, vec3(0.0, 0.0, 0.0)), vec3(0.4166667, 0.4166667, 0.4166667))
					  ) - 0.055), vec3(0.0, 0.0, 0.0));
					  tmpvar_3 = res_12;
					  gl_Position = tmpvar_1;
					  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
					  xlv_TEXCOORD1 = (_Object2World * _glesVertex).xyz;
					  xlv_COLOR0 = _glesColor;
					  xlv_TEXCOORD2 = o_4;
					  xlv_TEXCOORD3 = tmpvar_2;
					  xlv_TEXCOORD4 = tmpvar_3;
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform lowp vec4 _SpecColor;
					uniform sampler2D _MainTex;
					uniform lowp vec4 _Color;
					uniform sampler2D _LightBuffer;
					varying highp vec2 xlv_TEXCOORD0;
					varying highp vec3 xlv_TEXCOORD1;
					varying lowp vec4 xlv_COLOR0;
					varying highp vec4 xlv_TEXCOORD2;
					varying highp vec3 xlv_TEXCOORD4;
					void main ()
					{
					  lowp vec4 tmpvar_1;
					  mediump vec4 c_2;
					  mediump vec4 light_3;
					  highp vec4 tmpvar_4;
					  tmpvar_4 = xlv_COLOR0;
					  lowp vec3 tmpvar_5;
					  lowp vec4 tmpvar_6;
					  tmpvar_6 = texture2D (_MainTex, xlv_TEXCOORD0);
					  highp float x_7;
					  x_7 = -(xlv_TEXCOORD1.y);
					  if ((x_7 < 0.0)) {
					    discard;
					  };
					  highp float x_8;
					  x_8 = (xlv_TEXCOORD0.x - 0.5);
					  if ((x_8 < 0.0)) {
					    discard;
					  };
					  tmpvar_5 = ((tmpvar_6.xyz * _Color.xyz) * tmpvar_4.xyz);
					  lowp vec4 tmpvar_9;
					  tmpvar_9 = texture2DProj (_LightBuffer, xlv_TEXCOORD2);
					  light_3 = tmpvar_9;
					  light_3 = -(log2(max (light_3, vec4(0.001, 0.001, 0.001, 0.001))));
					  light_3.xyz = (light_3.xyz + xlv_TEXCOORD4);
					  lowp vec4 c_10;
					  lowp float spec_11;
					  mediump float tmpvar_12;
					  tmpvar_12 = (light_3.w * tmpvar_6.w);
					  spec_11 = tmpvar_12;
					  c_10.xyz = ((tmpvar_5 * light_3.xyz) + ((light_3.xyz * _SpecColor.xyz) * spec_11));
					  c_10.w = (tmpvar_6.w * _Color.w);
					  c_2.xyz = c_10.xyz;
					  c_2.w = 1.0;
					  tmpvar_1 = c_2;
					  gl_FragData[0] = tmpvar_1;
					}
					
					
					#endif"
}
SubProgram "gles " {
Keywords { "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "UNITY_HDR_ON" }
					"!!GLES
					#version 100
					
					#ifdef VERTEX
					attribute vec4 _glesVertex;
					attribute vec4 _glesColor;
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
					uniform highp vec4 _MainTex_ST;
					varying highp vec2 xlv_TEXCOORD0;
					varying highp vec3 xlv_TEXCOORD1;
					varying lowp vec4 xlv_COLOR0;
					varying highp vec4 xlv_TEXCOORD2;
					varying highp vec4 xlv_TEXCOORD3;
					varying highp vec3 xlv_TEXCOORD4;
					void main ()
					{
					  highp vec4 tmpvar_1;
					  highp vec4 tmpvar_2;
					  highp vec3 tmpvar_3;
					  tmpvar_1 = (glstate_matrix_mvp * _glesVertex);
					  highp vec4 o_4;
					  highp vec4 tmpvar_5;
					  tmpvar_5 = (tmpvar_1 * 0.5);
					  highp vec2 tmpvar_6;
					  tmpvar_6.x = tmpvar_5.x;
					  tmpvar_6.y = (tmpvar_5.y * _ProjectionParams.x);
					  o_4.xy = (tmpvar_6 + tmpvar_5.w);
					  o_4.zw = tmpvar_1.zw;
					  tmpvar_2.zw = vec2(0.0, 0.0);
					  tmpvar_2.xy = vec2(0.0, 0.0);
					  highp vec4 v_7;
					  v_7.x = _World2Object[0].x;
					  v_7.y = _World2Object[1].x;
					  v_7.z = _World2Object[2].x;
					  v_7.w = _World2Object[3].x;
					  highp vec4 v_8;
					  v_8.x = _World2Object[0].y;
					  v_8.y = _World2Object[1].y;
					  v_8.z = _World2Object[2].y;
					  v_8.w = _World2Object[3].y;
					  highp vec4 v_9;
					  v_9.x = _World2Object[0].z;
					  v_9.y = _World2Object[1].z;
					  v_9.z = _World2Object[2].z;
					  v_9.w = _World2Object[3].z;
					  highp vec4 tmpvar_10;
					  tmpvar_10.w = 1.0;
					  tmpvar_10.xyz = normalize(((
					    (v_7.xyz * _glesNormal.x)
					   + 
					    (v_8.xyz * _glesNormal.y)
					  ) + (v_9.xyz * _glesNormal.z)));
					  mediump vec4 normal_11;
					  normal_11 = tmpvar_10;
					  mediump vec3 res_12;
					  mediump vec3 x_13;
					  x_13.x = dot (unity_SHAr, normal_11);
					  x_13.y = dot (unity_SHAg, normal_11);
					  x_13.z = dot (unity_SHAb, normal_11);
					  mediump vec3 x1_14;
					  mediump vec4 tmpvar_15;
					  tmpvar_15 = (normal_11.xyzz * normal_11.yzzx);
					  x1_14.x = dot (unity_SHBr, tmpvar_15);
					  x1_14.y = dot (unity_SHBg, tmpvar_15);
					  x1_14.z = dot (unity_SHBb, tmpvar_15);
					  res_12 = (x_13 + (x1_14 + (unity_SHC.xyz * 
					    ((normal_11.x * normal_11.x) - (normal_11.y * normal_11.y))
					  )));
					  res_12 = max (((1.055 * 
					    pow (max (res_12, vec3(0.0, 0.0, 0.0)), vec3(0.4166667, 0.4166667, 0.4166667))
					  ) - 0.055), vec3(0.0, 0.0, 0.0));
					  tmpvar_3 = res_12;
					  gl_Position = tmpvar_1;
					  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
					  xlv_TEXCOORD1 = (_Object2World * _glesVertex).xyz;
					  xlv_COLOR0 = _glesColor;
					  xlv_TEXCOORD2 = o_4;
					  xlv_TEXCOORD3 = tmpvar_2;
					  xlv_TEXCOORD4 = tmpvar_3;
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform lowp vec4 _SpecColor;
					uniform sampler2D _MainTex;
					uniform lowp vec4 _Color;
					uniform sampler2D _LightBuffer;
					varying highp vec2 xlv_TEXCOORD0;
					varying highp vec3 xlv_TEXCOORD1;
					varying lowp vec4 xlv_COLOR0;
					varying highp vec4 xlv_TEXCOORD2;
					varying highp vec3 xlv_TEXCOORD4;
					void main ()
					{
					  lowp vec4 tmpvar_1;
					  mediump vec4 c_2;
					  mediump vec4 light_3;
					  highp vec4 tmpvar_4;
					  tmpvar_4 = xlv_COLOR0;
					  lowp vec3 tmpvar_5;
					  lowp vec4 tmpvar_6;
					  tmpvar_6 = texture2D (_MainTex, xlv_TEXCOORD0);
					  highp float x_7;
					  x_7 = -(xlv_TEXCOORD1.y);
					  if ((x_7 < 0.0)) {
					    discard;
					  };
					  highp float x_8;
					  x_8 = (xlv_TEXCOORD0.x - 0.5);
					  if ((x_8 < 0.0)) {
					    discard;
					  };
					  tmpvar_5 = ((tmpvar_6.xyz * _Color.xyz) * tmpvar_4.xyz);
					  lowp vec4 tmpvar_9;
					  tmpvar_9 = texture2DProj (_LightBuffer, xlv_TEXCOORD2);
					  light_3 = tmpvar_9;
					  mediump vec4 tmpvar_10;
					  tmpvar_10 = max (light_3, vec4(0.001, 0.001, 0.001, 0.001));
					  light_3.w = tmpvar_10.w;
					  light_3.xyz = (tmpvar_10.xyz + xlv_TEXCOORD4);
					  lowp vec4 c_11;
					  lowp float spec_12;
					  mediump float tmpvar_13;
					  tmpvar_13 = (tmpvar_10.w * tmpvar_6.w);
					  spec_12 = tmpvar_13;
					  c_11.xyz = ((tmpvar_5 * light_3.xyz) + ((light_3.xyz * _SpecColor.xyz) * spec_12));
					  c_11.w = (tmpvar_6.w * _Color.w);
					  c_2.xyz = c_11.xyz;
					  c_2.w = 1.0;
					  tmpvar_1 = c_2;
					  gl_FragData[0] = tmpvar_1;
					}
					
					
					#endif"
}
SubProgram "gles " {
Keywords { "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "FOG_LINEAR" "HDR_LIGHT_PREPASS_OFF" }
					"!!GLES
					#version 100
					
					#ifdef VERTEX
					attribute vec4 _glesVertex;
					attribute vec4 _glesColor;
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
					uniform highp vec4 _MainTex_ST;
					varying highp vec2 xlv_TEXCOORD0;
					varying highp vec3 xlv_TEXCOORD1;
					varying lowp vec4 xlv_COLOR0;
					varying highp vec4 xlv_TEXCOORD2;
					varying highp vec4 xlv_TEXCOORD3;
					varying highp vec3 xlv_TEXCOORD4;
					varying highp float xlv_TEXCOORD5;
					void main ()
					{
					  highp vec4 tmpvar_1;
					  highp vec4 tmpvar_2;
					  highp vec3 tmpvar_3;
					  tmpvar_1 = (glstate_matrix_mvp * _glesVertex);
					  highp vec4 o_4;
					  highp vec4 tmpvar_5;
					  tmpvar_5 = (tmpvar_1 * 0.5);
					  highp vec2 tmpvar_6;
					  tmpvar_6.x = tmpvar_5.x;
					  tmpvar_6.y = (tmpvar_5.y * _ProjectionParams.x);
					  o_4.xy = (tmpvar_6 + tmpvar_5.w);
					  o_4.zw = tmpvar_1.zw;
					  tmpvar_2.zw = vec2(0.0, 0.0);
					  tmpvar_2.xy = vec2(0.0, 0.0);
					  highp vec4 v_7;
					  v_7.x = _World2Object[0].x;
					  v_7.y = _World2Object[1].x;
					  v_7.z = _World2Object[2].x;
					  v_7.w = _World2Object[3].x;
					  highp vec4 v_8;
					  v_8.x = _World2Object[0].y;
					  v_8.y = _World2Object[1].y;
					  v_8.z = _World2Object[2].y;
					  v_8.w = _World2Object[3].y;
					  highp vec4 v_9;
					  v_9.x = _World2Object[0].z;
					  v_9.y = _World2Object[1].z;
					  v_9.z = _World2Object[2].z;
					  v_9.w = _World2Object[3].z;
					  highp vec4 tmpvar_10;
					  tmpvar_10.w = 1.0;
					  tmpvar_10.xyz = normalize(((
					    (v_7.xyz * _glesNormal.x)
					   + 
					    (v_8.xyz * _glesNormal.y)
					  ) + (v_9.xyz * _glesNormal.z)));
					  mediump vec4 normal_11;
					  normal_11 = tmpvar_10;
					  mediump vec3 res_12;
					  mediump vec3 x_13;
					  x_13.x = dot (unity_SHAr, normal_11);
					  x_13.y = dot (unity_SHAg, normal_11);
					  x_13.z = dot (unity_SHAb, normal_11);
					  mediump vec3 x1_14;
					  mediump vec4 tmpvar_15;
					  tmpvar_15 = (normal_11.xyzz * normal_11.yzzx);
					  x1_14.x = dot (unity_SHBr, tmpvar_15);
					  x1_14.y = dot (unity_SHBg, tmpvar_15);
					  x1_14.z = dot (unity_SHBb, tmpvar_15);
					  res_12 = (x_13 + (x1_14 + (unity_SHC.xyz * 
					    ((normal_11.x * normal_11.x) - (normal_11.y * normal_11.y))
					  )));
					  res_12 = max (((1.055 * 
					    pow (max (res_12, vec3(0.0, 0.0, 0.0)), vec3(0.4166667, 0.4166667, 0.4166667))
					  ) - 0.055), vec3(0.0, 0.0, 0.0));
					  tmpvar_3 = res_12;
					  gl_Position = tmpvar_1;
					  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
					  xlv_TEXCOORD1 = (_Object2World * _glesVertex).xyz;
					  xlv_COLOR0 = _glesColor;
					  xlv_TEXCOORD2 = o_4;
					  xlv_TEXCOORD3 = tmpvar_2;
					  xlv_TEXCOORD4 = tmpvar_3;
					  xlv_TEXCOORD5 = ((tmpvar_1.z * unity_FogParams.z) + unity_FogParams.w);
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform lowp vec4 unity_FogColor;
					uniform lowp vec4 _SpecColor;
					uniform sampler2D _MainTex;
					uniform lowp vec4 _Color;
					uniform sampler2D _LightBuffer;
					varying highp vec2 xlv_TEXCOORD0;
					varying highp vec3 xlv_TEXCOORD1;
					varying lowp vec4 xlv_COLOR0;
					varying highp vec4 xlv_TEXCOORD2;
					varying highp vec3 xlv_TEXCOORD4;
					varying highp float xlv_TEXCOORD5;
					void main ()
					{
					  lowp vec4 tmpvar_1;
					  mediump vec4 c_2;
					  mediump vec4 light_3;
					  highp vec4 tmpvar_4;
					  tmpvar_4 = xlv_COLOR0;
					  lowp vec3 tmpvar_5;
					  lowp vec4 tmpvar_6;
					  tmpvar_6 = texture2D (_MainTex, xlv_TEXCOORD0);
					  highp float x_7;
					  x_7 = -(xlv_TEXCOORD1.y);
					  if ((x_7 < 0.0)) {
					    discard;
					  };
					  highp float x_8;
					  x_8 = (xlv_TEXCOORD0.x - 0.5);
					  if ((x_8 < 0.0)) {
					    discard;
					  };
					  tmpvar_5 = ((tmpvar_6.xyz * _Color.xyz) * tmpvar_4.xyz);
					  lowp vec4 tmpvar_9;
					  tmpvar_9 = texture2DProj (_LightBuffer, xlv_TEXCOORD2);
					  light_3 = tmpvar_9;
					  light_3 = -(log2(max (light_3, vec4(0.001, 0.001, 0.001, 0.001))));
					  light_3.xyz = (light_3.xyz + xlv_TEXCOORD4);
					  lowp vec4 c_10;
					  lowp float spec_11;
					  mediump float tmpvar_12;
					  tmpvar_12 = (light_3.w * tmpvar_6.w);
					  spec_11 = tmpvar_12;
					  c_10.xyz = ((tmpvar_5 * light_3.xyz) + ((light_3.xyz * _SpecColor.xyz) * spec_11));
					  c_10.w = (tmpvar_6.w * _Color.w);
					  c_2 = c_10;
					  highp float tmpvar_13;
					  tmpvar_13 = clamp (xlv_TEXCOORD5, 0.0, 1.0);
					  c_2.xyz = mix (unity_FogColor.xyz, c_2.xyz, vec3(tmpvar_13));
					  c_2.w = 1.0;
					  tmpvar_1 = c_2;
					  gl_FragData[0] = tmpvar_1;
					}
					
					
					#endif"
}
SubProgram "gles " {
Keywords { "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "FOG_LINEAR" "UNITY_HDR_ON" }
					"!!GLES
					#version 100
					
					#ifdef VERTEX
					attribute vec4 _glesVertex;
					attribute vec4 _glesColor;
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
					uniform highp vec4 _MainTex_ST;
					varying highp vec2 xlv_TEXCOORD0;
					varying highp vec3 xlv_TEXCOORD1;
					varying lowp vec4 xlv_COLOR0;
					varying highp vec4 xlv_TEXCOORD2;
					varying highp vec4 xlv_TEXCOORD3;
					varying highp vec3 xlv_TEXCOORD4;
					varying highp float xlv_TEXCOORD5;
					void main ()
					{
					  highp vec4 tmpvar_1;
					  highp vec4 tmpvar_2;
					  highp vec3 tmpvar_3;
					  tmpvar_1 = (glstate_matrix_mvp * _glesVertex);
					  highp vec4 o_4;
					  highp vec4 tmpvar_5;
					  tmpvar_5 = (tmpvar_1 * 0.5);
					  highp vec2 tmpvar_6;
					  tmpvar_6.x = tmpvar_5.x;
					  tmpvar_6.y = (tmpvar_5.y * _ProjectionParams.x);
					  o_4.xy = (tmpvar_6 + tmpvar_5.w);
					  o_4.zw = tmpvar_1.zw;
					  tmpvar_2.zw = vec2(0.0, 0.0);
					  tmpvar_2.xy = vec2(0.0, 0.0);
					  highp vec4 v_7;
					  v_7.x = _World2Object[0].x;
					  v_7.y = _World2Object[1].x;
					  v_7.z = _World2Object[2].x;
					  v_7.w = _World2Object[3].x;
					  highp vec4 v_8;
					  v_8.x = _World2Object[0].y;
					  v_8.y = _World2Object[1].y;
					  v_8.z = _World2Object[2].y;
					  v_8.w = _World2Object[3].y;
					  highp vec4 v_9;
					  v_9.x = _World2Object[0].z;
					  v_9.y = _World2Object[1].z;
					  v_9.z = _World2Object[2].z;
					  v_9.w = _World2Object[3].z;
					  highp vec4 tmpvar_10;
					  tmpvar_10.w = 1.0;
					  tmpvar_10.xyz = normalize(((
					    (v_7.xyz * _glesNormal.x)
					   + 
					    (v_8.xyz * _glesNormal.y)
					  ) + (v_9.xyz * _glesNormal.z)));
					  mediump vec4 normal_11;
					  normal_11 = tmpvar_10;
					  mediump vec3 res_12;
					  mediump vec3 x_13;
					  x_13.x = dot (unity_SHAr, normal_11);
					  x_13.y = dot (unity_SHAg, normal_11);
					  x_13.z = dot (unity_SHAb, normal_11);
					  mediump vec3 x1_14;
					  mediump vec4 tmpvar_15;
					  tmpvar_15 = (normal_11.xyzz * normal_11.yzzx);
					  x1_14.x = dot (unity_SHBr, tmpvar_15);
					  x1_14.y = dot (unity_SHBg, tmpvar_15);
					  x1_14.z = dot (unity_SHBb, tmpvar_15);
					  res_12 = (x_13 + (x1_14 + (unity_SHC.xyz * 
					    ((normal_11.x * normal_11.x) - (normal_11.y * normal_11.y))
					  )));
					  res_12 = max (((1.055 * 
					    pow (max (res_12, vec3(0.0, 0.0, 0.0)), vec3(0.4166667, 0.4166667, 0.4166667))
					  ) - 0.055), vec3(0.0, 0.0, 0.0));
					  tmpvar_3 = res_12;
					  gl_Position = tmpvar_1;
					  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
					  xlv_TEXCOORD1 = (_Object2World * _glesVertex).xyz;
					  xlv_COLOR0 = _glesColor;
					  xlv_TEXCOORD2 = o_4;
					  xlv_TEXCOORD3 = tmpvar_2;
					  xlv_TEXCOORD4 = tmpvar_3;
					  xlv_TEXCOORD5 = ((tmpvar_1.z * unity_FogParams.z) + unity_FogParams.w);
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform lowp vec4 unity_FogColor;
					uniform lowp vec4 _SpecColor;
					uniform sampler2D _MainTex;
					uniform lowp vec4 _Color;
					uniform sampler2D _LightBuffer;
					varying highp vec2 xlv_TEXCOORD0;
					varying highp vec3 xlv_TEXCOORD1;
					varying lowp vec4 xlv_COLOR0;
					varying highp vec4 xlv_TEXCOORD2;
					varying highp vec3 xlv_TEXCOORD4;
					varying highp float xlv_TEXCOORD5;
					void main ()
					{
					  lowp vec4 tmpvar_1;
					  mediump vec4 c_2;
					  mediump vec4 light_3;
					  highp vec4 tmpvar_4;
					  tmpvar_4 = xlv_COLOR0;
					  lowp vec3 tmpvar_5;
					  lowp vec4 tmpvar_6;
					  tmpvar_6 = texture2D (_MainTex, xlv_TEXCOORD0);
					  highp float x_7;
					  x_7 = -(xlv_TEXCOORD1.y);
					  if ((x_7 < 0.0)) {
					    discard;
					  };
					  highp float x_8;
					  x_8 = (xlv_TEXCOORD0.x - 0.5);
					  if ((x_8 < 0.0)) {
					    discard;
					  };
					  tmpvar_5 = ((tmpvar_6.xyz * _Color.xyz) * tmpvar_4.xyz);
					  lowp vec4 tmpvar_9;
					  tmpvar_9 = texture2DProj (_LightBuffer, xlv_TEXCOORD2);
					  light_3 = tmpvar_9;
					  mediump vec4 tmpvar_10;
					  tmpvar_10 = max (light_3, vec4(0.001, 0.001, 0.001, 0.001));
					  light_3.w = tmpvar_10.w;
					  light_3.xyz = (tmpvar_10.xyz + xlv_TEXCOORD4);
					  lowp vec4 c_11;
					  lowp float spec_12;
					  mediump float tmpvar_13;
					  tmpvar_13 = (tmpvar_10.w * tmpvar_6.w);
					  spec_12 = tmpvar_13;
					  c_11.xyz = ((tmpvar_5 * light_3.xyz) + ((light_3.xyz * _SpecColor.xyz) * spec_12));
					  c_11.w = (tmpvar_6.w * _Color.w);
					  c_2 = c_11;
					  highp float tmpvar_14;
					  tmpvar_14 = clamp (xlv_TEXCOORD5, 0.0, 1.0);
					  c_2.xyz = mix (unity_FogColor.xyz, c_2.xyz, vec3(tmpvar_14));
					  c_2.w = 1.0;
					  tmpvar_1 = c_2;
					  gl_FragData[0] = tmpvar_1;
					}
					
					
					#endif"
}
}
Program "fp" {
SubProgram "gles " {
Keywords { "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "HDR_LIGHT_PREPASS_OFF" }
					"!!GLES"
}
SubProgram "gles " {
Keywords { "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "UNITY_HDR_ON" }
					"!!GLES"
}
SubProgram "gles " {
Keywords { "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "FOG_LINEAR" "HDR_LIGHT_PREPASS_OFF" }
					"!!GLES"
}
SubProgram "gles " {
Keywords { "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "FOG_LINEAR" "UNITY_HDR_ON" }
					"!!GLES"
}
}
 }
 Pass {
  Name "DEFERRED"
  Tags { "LIGHTMODE"="Deferred" "RenderType"="Opaque" "RenderQueue"="Geometry+1" }
  ZTest Greater
  GpuProgramID 267850
Program "vp" {
SubProgram "gles " {
Keywords { "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "HDR_LIGHT_PREPASS_OFF" }
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
					varying highp vec4 xlv_TEXCOORD3;
					varying mediump vec3 xlv_TEXCOORD4;
					void main ()
					{
					  lowp vec3 worldNormal_1;
					  mediump vec3 tmpvar_2;
					  highp vec4 tmpvar_3;
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
					  tmpvar_2 = worldNormal_1;
					  tmpvar_3.zw = vec2(0.0, 0.0);
					  tmpvar_3.xy = vec2(0.0, 0.0);
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
					  gl_Position = (glstate_matrix_mvp * _glesVertex);
					  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
					  xlv_TEXCOORD1 = tmpvar_2;
					  xlv_TEXCOORD2 = (_Object2World * _glesVertex).xyz;
					  xlv_COLOR0 = _glesColor;
					  xlv_TEXCOORD3 = tmpvar_3;
					  xlv_TEXCOORD4 = max (vec3(0.0, 0.0, 0.0), res_10);
					}
					
					
					#endif
					#ifdef FRAGMENT
					#extension GL_EXT_draw_buffers : enable
					uniform lowp vec4 _SpecColor;
					uniform sampler2D _MainTex;
					uniform lowp vec4 _Color;
					uniform mediump float _Shininess;
					varying highp vec2 xlv_TEXCOORD0;
					varying mediump vec3 xlv_TEXCOORD1;
					varying highp vec3 xlv_TEXCOORD2;
					varying lowp vec4 xlv_COLOR0;
					varying mediump vec3 xlv_TEXCOORD4;
					void main ()
					{
					  mediump vec4 outDiffuse_1;
					  mediump vec4 outEmission_2;
					  lowp vec3 tmpvar_3;
					  highp vec4 tmpvar_4;
					  tmpvar_4 = xlv_COLOR0;
					  tmpvar_3 = xlv_TEXCOORD1;
					  lowp vec3 tmpvar_5;
					  highp float x_6;
					  x_6 = -(xlv_TEXCOORD2.y);
					  if ((x_6 < 0.0)) {
					    discard;
					  };
					  highp float x_7;
					  x_7 = (xlv_TEXCOORD0.x - 0.5);
					  if ((x_7 < 0.0)) {
					    discard;
					  };
					  tmpvar_5 = ((texture2D (_MainTex, xlv_TEXCOORD0).xyz * _Color.xyz) * tmpvar_4.xyz);
					  mediump vec4 outDiffuseOcclusion_8;
					  mediump vec4 outNormal_9;
					  mediump vec4 emission_10;
					  lowp vec4 tmpvar_11;
					  tmpvar_11.w = 1.0;
					  tmpvar_11.xyz = tmpvar_5;
					  outDiffuseOcclusion_8 = tmpvar_11;
					  mediump vec4 tmpvar_12;
					  tmpvar_12.xyz = _SpecColor.xyz;
					  tmpvar_12.w = _Shininess;
					  lowp vec4 tmpvar_13;
					  tmpvar_13.w = 1.0;
					  tmpvar_13.xyz = ((tmpvar_3 * 0.5) + 0.5);
					  outNormal_9 = tmpvar_13;
					  lowp vec4 tmpvar_14;
					  tmpvar_14.w = 1.0;
					  tmpvar_14.xyz = vec3(0.0, 0.0, 0.0);
					  emission_10 = tmpvar_14;
					  emission_10.xyz = (emission_10.xyz + (tmpvar_5 * xlv_TEXCOORD4));
					  outDiffuse_1.xyz = outDiffuseOcclusion_8.xyz;
					  outEmission_2.w = emission_10.w;
					  outEmission_2.xyz = exp2(-(emission_10.xyz));
					  outDiffuse_1.w = 1.0;
					  gl_FragData[0] = outDiffuse_1;
					  gl_FragData[1] = tmpvar_12;
					  gl_FragData[2] = outNormal_9;
					  gl_FragData[3] = outEmission_2;
					}
					
					
					#endif"
}
SubProgram "gles " {
Keywords { "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "UNITY_HDR_ON" }
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
					varying highp vec4 xlv_TEXCOORD3;
					varying mediump vec3 xlv_TEXCOORD4;
					void main ()
					{
					  lowp vec3 worldNormal_1;
					  mediump vec3 tmpvar_2;
					  highp vec4 tmpvar_3;
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
					  tmpvar_2 = worldNormal_1;
					  tmpvar_3.zw = vec2(0.0, 0.0);
					  tmpvar_3.xy = vec2(0.0, 0.0);
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
					  gl_Position = (glstate_matrix_mvp * _glesVertex);
					  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
					  xlv_TEXCOORD1 = tmpvar_2;
					  xlv_TEXCOORD2 = (_Object2World * _glesVertex).xyz;
					  xlv_COLOR0 = _glesColor;
					  xlv_TEXCOORD3 = tmpvar_3;
					  xlv_TEXCOORD4 = max (vec3(0.0, 0.0, 0.0), res_10);
					}
					
					
					#endif
					#ifdef FRAGMENT
					#extension GL_EXT_draw_buffers : enable
					uniform lowp vec4 _SpecColor;
					uniform sampler2D _MainTex;
					uniform lowp vec4 _Color;
					uniform mediump float _Shininess;
					varying highp vec2 xlv_TEXCOORD0;
					varying mediump vec3 xlv_TEXCOORD1;
					varying highp vec3 xlv_TEXCOORD2;
					varying lowp vec4 xlv_COLOR0;
					varying mediump vec3 xlv_TEXCOORD4;
					void main ()
					{
					  mediump vec4 outDiffuse_1;
					  lowp vec3 tmpvar_2;
					  highp vec4 tmpvar_3;
					  tmpvar_3 = xlv_COLOR0;
					  tmpvar_2 = xlv_TEXCOORD1;
					  lowp vec3 tmpvar_4;
					  highp float x_5;
					  x_5 = -(xlv_TEXCOORD2.y);
					  if ((x_5 < 0.0)) {
					    discard;
					  };
					  highp float x_6;
					  x_6 = (xlv_TEXCOORD0.x - 0.5);
					  if ((x_6 < 0.0)) {
					    discard;
					  };
					  tmpvar_4 = ((texture2D (_MainTex, xlv_TEXCOORD0).xyz * _Color.xyz) * tmpvar_3.xyz);
					  mediump vec4 outDiffuseOcclusion_7;
					  mediump vec4 outNormal_8;
					  mediump vec4 emission_9;
					  lowp vec4 tmpvar_10;
					  tmpvar_10.w = 1.0;
					  tmpvar_10.xyz = tmpvar_4;
					  outDiffuseOcclusion_7 = tmpvar_10;
					  mediump vec4 tmpvar_11;
					  tmpvar_11.xyz = _SpecColor.xyz;
					  tmpvar_11.w = _Shininess;
					  lowp vec4 tmpvar_12;
					  tmpvar_12.w = 1.0;
					  tmpvar_12.xyz = ((tmpvar_2 * 0.5) + 0.5);
					  outNormal_8 = tmpvar_12;
					  lowp vec4 tmpvar_13;
					  tmpvar_13.w = 1.0;
					  tmpvar_13.xyz = vec3(0.0, 0.0, 0.0);
					  emission_9 = tmpvar_13;
					  emission_9.xyz = (emission_9.xyz + (tmpvar_4 * xlv_TEXCOORD4));
					  outDiffuse_1.xyz = outDiffuseOcclusion_7.xyz;
					  outDiffuse_1.w = 1.0;
					  gl_FragData[0] = outDiffuse_1;
					  gl_FragData[1] = tmpvar_11;
					  gl_FragData[2] = outNormal_8;
					  gl_FragData[3] = emission_9;
					}
					
					
					#endif"
}
}
Program "fp" {
SubProgram "gles " {
Keywords { "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "HDR_LIGHT_PREPASS_OFF" }
					"!!GLES"
}
SubProgram "gles " {
Keywords { "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "UNITY_HDR_ON" }
					"!!GLES"
}
}
 }
}
Fallback "Legacy Shaders/VertexLit"
}