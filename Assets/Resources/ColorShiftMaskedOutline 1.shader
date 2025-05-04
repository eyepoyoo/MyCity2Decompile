Shader "Custom/City/ColorShiftMaskedOutline 1" {
Properties {
 _MainTex ("Base texture", 2D) = "white" { }
 _ColorDark ("ColorLow", Color) = (1,1,1,1)
 _ColorBright ("ColorHigh", Color) = (1,1,1,1)
 _BrightnessLow ("BrightnessLow", Float) = 1
 _BrightnessHigh ("BrightnessHigh", Float) = 1
 _Spec ("Spec", Range(0,1)) = 1
}
SubShader { 
 LOD 200
 Tags { "QUEUE"="Transparent" "IGNOREPROJECTOR"="true" "RenderType"="Transparent" }
 Pass {
  Tags { "QUEUE"="Transparent" "IGNOREPROJECTOR"="true" "RenderType"="Transparent" }
  ZWrite Off
  Cull Off
  Blend SrcAlpha OneMinusSrcAlpha
  Offset -1, -1
  GpuProgramID 33676
Program "vp" {
SubProgram "gles " {
"!!GLES
					#version 100
					
					#ifdef VERTEX
					attribute vec4 _glesVertex;
					attribute vec4 _glesColor;
					attribute vec4 _glesMultiTexCoord0;
					uniform highp mat4 glstate_matrix_mvp;
					uniform highp vec4 _ClipRange0;
					uniform highp vec4 _MainTex_ST;
					varying highp vec2 xlv_TEXCOORD0;
					varying highp vec2 xlv_TEXCOORD2;
					varying highp vec4 xlv_COLOR;
					varying highp vec2 xlv_TEXCOORD1;
					void main ()
					{
					  gl_Position = (glstate_matrix_mvp * _glesVertex);
					  highp vec2 tmpvar_1;
					  tmpvar_1 = (_glesMultiTexCoord0.xy * _MainTex_ST.xy);
					  xlv_TEXCOORD0 = (tmpvar_1 + _MainTex_ST.zw);
					  xlv_TEXCOORD2 = (((tmpvar_1 + _MainTex_ST.zw) * 1.05) - 0.025);
					  xlv_COLOR = _glesColor;
					  xlv_TEXCOORD1 = ((_glesVertex.xy * _ClipRange0.zw) + _ClipRange0.xy);
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform sampler2D _MainTex;
					uniform highp vec2 _ClipArgs0;
					uniform highp vec4 _ColorDark;
					uniform highp vec4 _ColorBright;
					uniform highp float _BrightnessLow;
					uniform highp float _BrightnessHigh;
					uniform highp float _Spec;
					varying highp vec2 xlv_TEXCOORD0;
					varying highp vec2 xlv_TEXCOORD2;
					varying highp vec2 xlv_TEXCOORD1;
					void main ()
					{
					  mediump vec4 c1_1;
					  mediump vec4 c0_2;
					  lowp vec4 tmpvar_3;
					  tmpvar_3 = texture2D (_MainTex, xlv_TEXCOORD0);
					  c0_2 = tmpvar_3;
					  lowp vec4 tmpvar_4;
					  tmpvar_4 = texture2D (_MainTex, xlv_TEXCOORD2);
					  c1_1 = tmpvar_4;
					  mediump float tmpvar_5;
					  tmpvar_5 = (((c0_2.x + c0_2.y) + c0_2.z) * 0.33);
					  mediump float tmpvar_6;
					  tmpvar_6 = clamp (((tmpvar_5 - 0.8) / 0.2), 0.0, 1.0);
					  highp vec4 tmpvar_7;
					  tmpvar_7 = mix (_ColorDark, _ColorBright, vec4(tmpvar_5));
					  highp float tmpvar_8;
					  tmpvar_8 = mix (_BrightnessLow, _BrightnessHigh, tmpvar_5);
					  c0_2 = (c0_2 * (tmpvar_7 * tmpvar_8));
					  c0_2.xyz = (c0_2.xyz + ((tmpvar_6 * 
					    (tmpvar_6 * (3.0 - (2.0 * tmpvar_6)))
					  ) * _Spec));
					  c0_2.xyz = (c0_2.xyz + (1.0 - c1_1.w));
					  highp vec2 tmpvar_9;
					  tmpvar_9 = ((vec2(1.0, 1.0) - abs(xlv_TEXCOORD1)) * _ClipArgs0);
					  highp float tmpvar_10;
					  tmpvar_10 = clamp (min (tmpvar_9.x, tmpvar_9.y), 0.0, 1.0);
					  c0_2.w = (c0_2.w * tmpvar_10);
					  gl_FragData[0] = c0_2;
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
}
Fallback "Diffuse"
}