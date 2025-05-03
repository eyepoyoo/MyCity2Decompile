Shader "VacuumShaders/Curved World/Sprites/Default" {
Properties {
[CurvedWorldGearMenu]  V_CW_Label_Tag ("", Float) = 0
[CurvedWorldLabel]  V_CW_Label_UnityDefaults ("Default Visual Options", Float) = 0
[PerRendererData]  _MainTex ("Sprite Texture", 2D) = "white" { }
 _Color ("Tint", Color) = (1,1,1,1)
[MaterialToggle]  PixelSnap ("Pixel snap", Float) = 0
[CurvedWorldLabel]  V_CW_Label_UnityDefaults ("Curved World Optionals", Float) = 0
}
SubShader { 
 Tags { "QUEUE"="Transparent" "IGNOREPROJECTOR"="true" "RenderType"="Transparent" "PreviewType"="Plane" "CanUseSpriteAtlas"="true" "CurvedWorldTag"="Sprites/Default" "CurvedWorldNoneRemoveableKeywords"="" "CurvedWorldAvailableOptions"="" }
 Pass {
  Tags { "QUEUE"="Transparent" "IGNOREPROJECTOR"="true" "RenderType"="Transparent" "PreviewType"="Plane" "CanUseSpriteAtlas"="true" "CurvedWorldTag"="Sprites/Default" "CurvedWorldNoneRemoveableKeywords"="" "CurvedWorldAvailableOptions"="" }
  ZWrite Off
  Blend One OneMinusSrcAlpha
  GpuProgramID 57030
Program "vp" {
SubProgram "gles " {
"!!GLES
					#version 100
					
					#ifdef VERTEX
					attribute vec4 _glesVertex;
					attribute vec4 _glesColor;
					attribute vec4 _glesMultiTexCoord0;
					uniform highp mat4 glstate_matrix_mvp;
					uniform highp mat4 _Object2World;
					uniform highp mat4 _World2Object;
					uniform highp vec3 _V_CW_Bend;
					uniform highp vec3 _V_CW_Bias;
					uniform highp vec4 _V_CW_PivotPoint_Position;
					uniform lowp vec4 _Color;
					varying lowp vec4 xlv_COLOR;
					varying mediump vec2 xlv_TEXCOORD0;
					void main ()
					{
					  highp vec2 tmpvar_1;
					  tmpvar_1 = _glesMultiTexCoord0.xy;
					  lowp vec4 tmpvar_2;
					  mediump vec2 tmpvar_3;
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
					  tmpvar_3 = tmpvar_1;
					  tmpvar_2 = (_glesColor * _Color);
					  gl_Position = (glstate_matrix_mvp * (_glesVertex + (_World2Object * tmpvar_8)));
					  xlv_COLOR = tmpvar_2;
					  xlv_TEXCOORD0 = tmpvar_3;
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform sampler2D _MainTex;
					uniform sampler2D _AlphaTex;
					uniform highp float _AlphaSplitEnabled;
					varying lowp vec4 xlv_COLOR;
					varying mediump vec2 xlv_TEXCOORD0;
					void main ()
					{
					  lowp vec4 c_1;
					  highp vec2 uv_2;
					  uv_2 = xlv_TEXCOORD0;
					  lowp vec4 color_3;
					  color_3 = texture2D (_MainTex, uv_2);
					  if (bool(_AlphaSplitEnabled)) {
					    color_3.w = texture2D (_AlphaTex, uv_2).x;
					  };
					  lowp vec4 tmpvar_4;
					  tmpvar_4 = (color_3 * xlv_COLOR);
					  c_1.w = tmpvar_4.w;
					  c_1.xyz = (tmpvar_4.xyz * tmpvar_4.w);
					  gl_FragData[0] = c_1;
					}
					
					
					#endif"
}
SubProgram "gles " {
Keywords { "PIXELSNAP_ON" }
					"!!GLES
					#version 100
					
					#ifdef VERTEX
					attribute vec4 _glesVertex;
					attribute vec4 _glesColor;
					attribute vec4 _glesMultiTexCoord0;
					uniform highp vec4 _ScreenParams;
					uniform highp mat4 glstate_matrix_mvp;
					uniform highp mat4 _Object2World;
					uniform highp mat4 _World2Object;
					uniform highp vec3 _V_CW_Bend;
					uniform highp vec3 _V_CW_Bias;
					uniform highp vec4 _V_CW_PivotPoint_Position;
					uniform lowp vec4 _Color;
					varying lowp vec4 xlv_COLOR;
					varying mediump vec2 xlv_TEXCOORD0;
					void main ()
					{
					  highp vec2 tmpvar_1;
					  tmpvar_1 = _glesMultiTexCoord0.xy;
					  highp vec4 tmpvar_2;
					  lowp vec4 tmpvar_3;
					  mediump vec2 tmpvar_4;
					  highp vec2 xzOff_5;
					  highp vec4 worldPos_6;
					  highp vec4 tmpvar_7;
					  tmpvar_7 = (_Object2World * _glesVertex);
					  worldPos_6.w = tmpvar_7.w;
					  worldPos_6.xyz = (tmpvar_7.xyz - _V_CW_PivotPoint_Position.xyz);
					  highp vec2 tmpvar_8;
					  tmpvar_8.x = float((worldPos_6.z >= 0.0));
					  tmpvar_8.y = float((worldPos_6.x >= 0.0));
					  xzOff_5 = (max (vec2(0.0, 0.0), (
					    abs(worldPos_6.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_8 * 2.0) - 1.0));
					  xzOff_5 = (xzOff_5 * xzOff_5);
					  highp vec4 tmpvar_9;
					  tmpvar_9.xzw = vec3(0.0, 0.0, 0.0);
					  tmpvar_9.y = (((_V_CW_Bend.x * xzOff_5.x) + (_V_CW_Bend.z * xzOff_5.y)) * 0.001);
					  worldPos_6 = tmpvar_9;
					  tmpvar_2 = (glstate_matrix_mvp * (_glesVertex + (_World2Object * tmpvar_9)));
					  tmpvar_4 = tmpvar_1;
					  tmpvar_3 = (_glesColor * _Color);
					  highp vec4 pos_10;
					  pos_10.zw = tmpvar_2.zw;
					  highp vec2 tmpvar_11;
					  tmpvar_11 = (_ScreenParams.xy * 0.5);
					  pos_10.xy = ((floor(
					    (((tmpvar_2.xy / tmpvar_2.w) * tmpvar_11) + vec2(0.5, 0.5))
					  ) / tmpvar_11) * tmpvar_2.w);
					  tmpvar_2 = pos_10;
					  gl_Position = pos_10;
					  xlv_COLOR = tmpvar_3;
					  xlv_TEXCOORD0 = tmpvar_4;
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform sampler2D _MainTex;
					uniform sampler2D _AlphaTex;
					uniform highp float _AlphaSplitEnabled;
					varying lowp vec4 xlv_COLOR;
					varying mediump vec2 xlv_TEXCOORD0;
					void main ()
					{
					  lowp vec4 c_1;
					  highp vec2 uv_2;
					  uv_2 = xlv_TEXCOORD0;
					  lowp vec4 color_3;
					  color_3 = texture2D (_MainTex, uv_2);
					  if (bool(_AlphaSplitEnabled)) {
					    color_3.w = texture2D (_AlphaTex, uv_2).x;
					  };
					  lowp vec4 tmpvar_4;
					  tmpvar_4 = (color_3 * xlv_COLOR);
					  c_1.w = tmpvar_4.w;
					  c_1.xyz = (tmpvar_4.xyz * tmpvar_4.w);
					  gl_FragData[0] = c_1;
					}
					
					
					#endif"
}
}
Program "fp" {
SubProgram "gles " {
"!!GLES"
}
SubProgram "gles " {
Keywords { "PIXELSNAP_ON" }
					"!!GLES"
}
}
 }
}
}