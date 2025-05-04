Shader "Custom/LavaSwell" {
Properties {
 _MainTex ("Base texture", 2D) = "white" { }
 _Settings ("x = Wave Length, y = Amplitude, z = Speed", Vector) = (0,0,0,0)
}
SubShader { 
 LOD 200
 Tags { "QUEUE"="Geometry" "RenderType"="Opaque" }
 Pass {
  Tags { "QUEUE"="Geometry" "SHADOWSUPPORT"="true" "RenderType"="Opaque" }
  GpuProgramID 4850
Program "vp" {
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_OFF" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" }
					"!!GLES
					#version 100
					
					#ifdef VERTEX
					attribute vec4 _glesVertex;
					attribute vec4 _glesColor;
					attribute vec4 _glesMultiTexCoord0;
					uniform highp vec4 _Time;
					uniform highp mat4 _Object2World;
					uniform highp mat4 unity_MatrixVP;
					uniform highp vec4 _Settings;
					uniform highp vec4 _MainTex_ST;
					varying highp vec2 xlv_TEXCOORD0;
					varying highp vec4 xlv_COLOR;
					void main ()
					{
					  mediump float offset_1;
					  mediump float time_2;
					  mediump vec4 worldPos_3;
					  highp vec4 tmpvar_4;
					  tmpvar_4 = (_Object2World * _glesVertex);
					  worldPos_3 = tmpvar_4;
					  highp float tmpvar_5;
					  tmpvar_5 = (_Time.y * _Settings.z);
					  time_2 = tmpvar_5;
					  highp float tmpvar_6;
					  tmpvar_6 = (0.5 + ((
					    sin(((worldPos_3.x * _Settings.x) + time_2))
					   + 
					    cos(((worldPos_3.z * _Settings.x) + (time_2 * 1.3)))
					  ) / 4.0));
					  offset_1 = tmpvar_6;
					  offset_1 = (offset_1 * _Settings.y);
					  worldPos_3.y = (worldPos_3.y + offset_1);
					  gl_Position = (unity_MatrixVP * worldPos_3);
					  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
					  xlv_COLOR = _glesColor;
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform sampler2D _MainTex;
					varying highp vec2 xlv_TEXCOORD0;
					void main ()
					{
					  mediump vec4 tmpvar_1;
					  lowp vec4 tmpvar_2;
					  tmpvar_2 = texture2D (_MainTex, xlv_TEXCOORD0);
					  tmpvar_1 = tmpvar_2;
					  gl_FragData[0] = tmpvar_1;
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
					attribute vec4 _glesMultiTexCoord0;
					uniform highp vec4 _Time;
					uniform highp mat4 _Object2World;
					uniform highp mat4 unity_MatrixVP;
					uniform highp vec4 _Settings;
					uniform highp vec4 _MainTex_ST;
					varying highp vec2 xlv_TEXCOORD0;
					varying highp vec4 xlv_COLOR;
					void main ()
					{
					  mediump float offset_1;
					  mediump float time_2;
					  mediump vec4 worldPos_3;
					  highp vec4 tmpvar_4;
					  tmpvar_4 = (_Object2World * _glesVertex);
					  worldPos_3 = tmpvar_4;
					  highp float tmpvar_5;
					  tmpvar_5 = (_Time.y * _Settings.z);
					  time_2 = tmpvar_5;
					  highp float tmpvar_6;
					  tmpvar_6 = (0.5 + ((
					    sin(((worldPos_3.x * _Settings.x) + time_2))
					   + 
					    cos(((worldPos_3.z * _Settings.x) + (time_2 * 1.3)))
					  ) / 4.0));
					  offset_1 = tmpvar_6;
					  offset_1 = (offset_1 * _Settings.y);
					  worldPos_3.y = (worldPos_3.y + offset_1);
					  gl_Position = (unity_MatrixVP * worldPos_3);
					  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
					  xlv_COLOR = _glesColor;
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform sampler2D _MainTex;
					varying highp vec2 xlv_TEXCOORD0;
					void main ()
					{
					  mediump vec4 tmpvar_1;
					  lowp vec4 tmpvar_2;
					  tmpvar_2 = texture2D (_MainTex, xlv_TEXCOORD0);
					  tmpvar_1 = tmpvar_2;
					  gl_FragData[0] = tmpvar_1;
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
					attribute vec4 _glesMultiTexCoord0;
					uniform highp vec4 _Time;
					uniform highp mat4 _Object2World;
					uniform highp mat4 unity_MatrixVP;
					uniform highp vec4 _Settings;
					uniform highp vec4 _MainTex_ST;
					varying highp vec2 xlv_TEXCOORD0;
					varying highp vec4 xlv_COLOR;
					void main ()
					{
					  mediump float offset_1;
					  mediump float time_2;
					  mediump vec4 worldPos_3;
					  highp vec4 tmpvar_4;
					  tmpvar_4 = (_Object2World * _glesVertex);
					  worldPos_3 = tmpvar_4;
					  highp float tmpvar_5;
					  tmpvar_5 = (_Time.y * _Settings.z);
					  time_2 = tmpvar_5;
					  highp float tmpvar_6;
					  tmpvar_6 = (0.5 + ((
					    sin(((worldPos_3.x * _Settings.x) + time_2))
					   + 
					    cos(((worldPos_3.z * _Settings.x) + (time_2 * 1.3)))
					  ) / 4.0));
					  offset_1 = tmpvar_6;
					  offset_1 = (offset_1 * _Settings.y);
					  worldPos_3.y = (worldPos_3.y + offset_1);
					  gl_Position = (unity_MatrixVP * worldPos_3);
					  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
					  xlv_COLOR = _glesColor;
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform sampler2D _MainTex;
					varying highp vec2 xlv_TEXCOORD0;
					void main ()
					{
					  mediump vec4 tmpvar_1;
					  lowp vec4 tmpvar_2;
					  tmpvar_2 = texture2D (_MainTex, xlv_TEXCOORD0);
					  tmpvar_1 = tmpvar_2;
					  gl_FragData[0] = tmpvar_1;
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
					attribute vec4 _glesMultiTexCoord0;
					uniform highp vec4 _Time;
					uniform highp mat4 _Object2World;
					uniform highp mat4 unity_MatrixVP;
					uniform highp vec4 _Settings;
					uniform highp vec4 _MainTex_ST;
					varying highp vec2 xlv_TEXCOORD0;
					varying highp vec4 xlv_COLOR;
					void main ()
					{
					  mediump float offset_1;
					  mediump float time_2;
					  mediump vec4 worldPos_3;
					  highp vec4 tmpvar_4;
					  tmpvar_4 = (_Object2World * _glesVertex);
					  worldPos_3 = tmpvar_4;
					  highp float tmpvar_5;
					  tmpvar_5 = (_Time.y * _Settings.z);
					  time_2 = tmpvar_5;
					  highp float tmpvar_6;
					  tmpvar_6 = (0.5 + ((
					    sin(((worldPos_3.x * _Settings.x) + time_2))
					   + 
					    cos(((worldPos_3.z * _Settings.x) + (time_2 * 1.3)))
					  ) / 4.0));
					  offset_1 = tmpvar_6;
					  offset_1 = (offset_1 * _Settings.y);
					  worldPos_3.y = (worldPos_3.y + offset_1);
					  gl_Position = (unity_MatrixVP * worldPos_3);
					  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
					  xlv_COLOR = _glesColor;
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform sampler2D _MainTex;
					varying highp vec2 xlv_TEXCOORD0;
					void main ()
					{
					  mediump vec4 tmpvar_1;
					  lowp vec4 tmpvar_2;
					  tmpvar_2 = texture2D (_MainTex, xlv_TEXCOORD0);
					  tmpvar_1 = tmpvar_2;
					  gl_FragData[0] = tmpvar_1;
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
}
 }
}
Fallback "Diffuse"
}