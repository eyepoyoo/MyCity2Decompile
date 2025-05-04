Shader "Custom/City/Curved World Lightmapped Fog" {
Properties {
 _MainTex ("Albedo (RGB)", 2D) = "white" { }
 _Lightmap ("Lightmap", 2D) = "white" { }
}
SubShader { 
 Tags { "RenderType"="CurvedWorld_Opaque" }
 Pass {
  Tags { "RenderType"="CurvedWorld_Opaque" }
  GpuProgramID 16656
Program "vp" {
SubProgram "gles " {
"!!GLES
					#version 100
					
					#ifdef VERTEX
					attribute vec4 _glesVertex;
					attribute vec4 _glesMultiTexCoord0;
					attribute vec4 _glesMultiTexCoord1;
					uniform highp mat4 glstate_matrix_mvp;
					uniform highp mat4 _Object2World;
					uniform highp mat4 _World2Object;
					uniform highp vec3 _V_CW_Bend;
					uniform highp vec3 _V_CW_Bias;
					uniform highp vec4 _V_CW_PivotPoint_Position;
					varying highp vec2 xlv_TEXCOORD0;
					varying highp vec2 xlv_TEXCOORD1;
					void main ()
					{
					  highp vec2 xzOff_1;
					  highp vec4 worldPos_2;
					  highp vec4 tmpvar_3;
					  tmpvar_3 = (_Object2World * _glesVertex);
					  worldPos_2.w = tmpvar_3.w;
					  worldPos_2.xyz = (tmpvar_3.xyz - _V_CW_PivotPoint_Position.xyz);
					  highp vec2 tmpvar_4;
					  tmpvar_4.x = float((worldPos_2.z >= 0.0));
					  tmpvar_4.y = float((worldPos_2.x >= 0.0));
					  xzOff_1 = (max (vec2(0.0, 0.0), (
					    abs(worldPos_2.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_4 * 2.0) - 1.0));
					  xzOff_1 = (xzOff_1 * xzOff_1);
					  highp vec4 tmpvar_5;
					  tmpvar_5.xzw = vec3(0.0, 0.0, 0.0);
					  tmpvar_5.y = (((_V_CW_Bend.x * xzOff_1.x) + (_V_CW_Bend.z * xzOff_1.y)) * 0.001);
					  worldPos_2 = tmpvar_5;
					  gl_Position = (glstate_matrix_mvp * (_glesVertex + (_World2Object * tmpvar_5)));
					  xlv_TEXCOORD0 = _glesMultiTexCoord0.xy;
					  xlv_TEXCOORD1 = _glesMultiTexCoord1.xy;
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform sampler2D _MainTex;
					uniform sampler2D _Lightmap;
					varying highp vec2 xlv_TEXCOORD0;
					varying highp vec2 xlv_TEXCOORD1;
					void main ()
					{
					  lowp vec4 tmpvar_1;
					  tmpvar_1 = (texture2D (_MainTex, xlv_TEXCOORD0) * texture2D (_Lightmap, xlv_TEXCOORD1));
					  gl_FragData[0] = tmpvar_1;
					}
					
					
					#endif"
}
SubProgram "gles " {
Keywords { "FOG_LINEAR" }
					"!!GLES
					#version 100
					
					#ifdef VERTEX
					attribute vec4 _glesVertex;
					attribute vec4 _glesMultiTexCoord0;
					attribute vec4 _glesMultiTexCoord1;
					uniform highp mat4 glstate_matrix_mvp;
					uniform highp mat4 _Object2World;
					uniform highp mat4 _World2Object;
					uniform highp vec4 unity_FogParams;
					uniform highp vec3 _V_CW_Bend;
					uniform highp vec3 _V_CW_Bias;
					uniform highp vec4 _V_CW_PivotPoint_Position;
					varying highp vec2 xlv_TEXCOORD0;
					varying highp vec2 xlv_TEXCOORD1;
					varying highp float xlv_TEXCOORD2;
					void main ()
					{
					  highp vec4 tmpvar_1;
					  highp vec2 xzOff_2;
					  highp vec4 worldPos_3;
					  highp vec4 tmpvar_4;
					  tmpvar_4 = (_Object2World * _glesVertex);
					  worldPos_3.w = tmpvar_4.w;
					  worldPos_3.xyz = (tmpvar_4.xyz - _V_CW_PivotPoint_Position.xyz);
					  highp vec2 tmpvar_5;
					  tmpvar_5.x = float((worldPos_3.z >= 0.0));
					  tmpvar_5.y = float((worldPos_3.x >= 0.0));
					  xzOff_2 = (max (vec2(0.0, 0.0), (
					    abs(worldPos_3.zx)
					   - _V_CW_Bias.xz)) * ((tmpvar_5 * 2.0) - 1.0));
					  xzOff_2 = (xzOff_2 * xzOff_2);
					  highp vec4 tmpvar_6;
					  tmpvar_6.xzw = vec3(0.0, 0.0, 0.0);
					  tmpvar_6.y = (((_V_CW_Bend.x * xzOff_2.x) + (_V_CW_Bend.z * xzOff_2.y)) * 0.001);
					  worldPos_3 = tmpvar_6;
					  tmpvar_1 = (glstate_matrix_mvp * (_glesVertex + (_World2Object * tmpvar_6)));
					  gl_Position = tmpvar_1;
					  xlv_TEXCOORD0 = _glesMultiTexCoord0.xy;
					  xlv_TEXCOORD1 = _glesMultiTexCoord1.xy;
					  xlv_TEXCOORD2 = ((tmpvar_1.z * unity_FogParams.z) + unity_FogParams.w);
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform lowp vec4 unity_FogColor;
					uniform sampler2D _MainTex;
					uniform sampler2D _Lightmap;
					varying highp vec2 xlv_TEXCOORD0;
					varying highp vec2 xlv_TEXCOORD1;
					varying highp float xlv_TEXCOORD2;
					void main ()
					{
					  lowp vec4 col_1;
					  lowp vec4 tmpvar_2;
					  tmpvar_2 = (texture2D (_MainTex, xlv_TEXCOORD0) * texture2D (_Lightmap, xlv_TEXCOORD1));
					  col_1.w = tmpvar_2.w;
					  highp float tmpvar_3;
					  tmpvar_3 = clamp (xlv_TEXCOORD2, 0.0, 1.0);
					  col_1.xyz = mix (unity_FogColor.xyz, tmpvar_2.xyz, vec3(tmpvar_3));
					  gl_FragData[0] = col_1;
					}
					
					
					#endif"
}
}
Program "fp" {
SubProgram "gles " {
"!!GLES"
}
SubProgram "gles " {
Keywords { "FOG_LINEAR" }
					"!!GLES"
}
}
 }
}
}