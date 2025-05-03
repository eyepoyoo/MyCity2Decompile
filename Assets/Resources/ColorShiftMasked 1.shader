Shader "Custom/City/ColorShiftMasked 1" {
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
  GpuProgramID 22601
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
					varying highp vec4 xlv_COLOR;
					varying highp vec2 xlv_TEXCOORD1;
					void main ()
					{
					  gl_Position = (glstate_matrix_mvp * _glesVertex);
					  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
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
					varying highp vec2 xlv_TEXCOORD1;
					void main ()
					{
					  mediump vec4 c0_1;
					  lowp vec4 tmpvar_2;
					  tmpvar_2 = texture2D (_MainTex, xlv_TEXCOORD0);
					  c0_1 = tmpvar_2;
					  mediump float tmpvar_3;
					  tmpvar_3 = (((c0_1.x + c0_1.y) + c0_1.z) * 0.33);
					  mediump float tmpvar_4;
					  tmpvar_4 = clamp (((tmpvar_3 - 0.8) / 0.2), 0.0, 1.0);
					  highp vec4 tmpvar_5;
					  tmpvar_5 = mix (_ColorDark, _ColorBright, vec4(tmpvar_3));
					  highp float tmpvar_6;
					  tmpvar_6 = mix (_BrightnessLow, _BrightnessHigh, tmpvar_3);
					  c0_1 = (c0_1 * (tmpvar_5 * tmpvar_6));
					  c0_1.xyz = (c0_1.xyz + ((tmpvar_4 * 
					    (tmpvar_4 * (3.0 - (2.0 * tmpvar_4)))
					  ) * _Spec));
					  highp vec2 tmpvar_7;
					  tmpvar_7 = ((vec2(1.0, 1.0) - abs(xlv_TEXCOORD1)) * _ClipArgs0);
					  highp float tmpvar_8;
					  tmpvar_8 = clamp (min (tmpvar_7.x, tmpvar_7.y), 0.0, 1.0);
					  c0_1.w = (c0_1.w * tmpvar_8);
					  gl_FragData[0] = c0_1;
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