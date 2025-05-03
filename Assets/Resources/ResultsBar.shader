Shader "Custom/ResultsBarAtlassed" {
Properties {
 _MainTex ("Empty Bar", 2D) = "white" { }
 _UV ("FullUV Offset X,Y, MaskUV X, Y", Vector) = (0,0,0,0)
 _Data ("Current,FallOff,Unused,Unused", Vector) = (0,0.01,0.01,0)
}
SubShader { 
 LOD 200
 Tags { "QUEUE"="Transparent" "IGNOREPROJECTOR"="true" "RenderType"="Transparent" }
 Pass {
  Tags { "QUEUE"="Transparent" "IGNOREPROJECTOR"="true" "RenderType"="Transparent" }
  ZTest Always
  Blend SrcAlpha OneMinusSrcAlpha
  GpuProgramID 8284
Program "vp" {
SubProgram "gles " {
"!!GLES
					#version 100
					
					#ifdef VERTEX
					attribute vec4 _glesVertex;
					attribute vec4 _glesColor;
					attribute vec4 _glesMultiTexCoord0;
					uniform highp mat4 glstate_matrix_mvp;
					uniform highp vec4 _UV;
					varying mediump vec4 xlv_COLOR;
					varying mediump vec2 xlv_TEXCOORD0;
					varying mediump vec2 xlv_TEXCOORD1;
					varying mediump vec2 xlv_TEXCOORD2;
					void main ()
					{
					  mediump vec2 tmpvar_1;
					  mediump vec2 tmpvar_2;
					  tmpvar_1 = (_glesMultiTexCoord0.xy + _UV.xy);
					  tmpvar_2 = (_glesMultiTexCoord0.xy + _UV.zw);
					  gl_Position = (glstate_matrix_mvp * _glesVertex);
					  xlv_COLOR = _glesColor;
					  xlv_TEXCOORD0 = _glesMultiTexCoord0.xy;
					  xlv_TEXCOORD1 = tmpvar_1;
					  xlv_TEXCOORD2 = tmpvar_2;
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform sampler2D _MainTex;
					uniform highp vec4 _Data;
					varying mediump vec2 xlv_TEXCOORD0;
					varying mediump vec2 xlv_TEXCOORD1;
					varying mediump vec2 xlv_TEXCOORD2;
					void main ()
					{
					  mediump vec4 tmpvar_1;
					  mediump vec4 mask_2;
					  mediump vec4 texfront_3;
					  mediump vec4 texback_4;
					  lowp vec4 tmpvar_5;
					  tmpvar_5 = texture2D (_MainTex, xlv_TEXCOORD0);
					  texback_4 = tmpvar_5;
					  lowp vec4 tmpvar_6;
					  tmpvar_6 = texture2D (_MainTex, xlv_TEXCOORD1);
					  texfront_3 = tmpvar_6;
					  lowp vec4 tmpvar_7;
					  tmpvar_7 = texture2D (_MainTex, xlv_TEXCOORD2);
					  mask_2 = tmpvar_7;
					  highp float edge0_8;
					  edge0_8 = (_Data.x - _Data.y);
					  highp float tmpvar_9;
					  tmpvar_9 = clamp (((mask_2.x - edge0_8) / (_Data.x - edge0_8)), 0.0, 1.0);
					  highp vec4 tmpvar_10;
					  tmpvar_10 = mix (texfront_3, texback_4, vec4((tmpvar_9 * (tmpvar_9 * 
					    (3.0 - (2.0 * tmpvar_9))
					  ))));
					  texfront_3 = tmpvar_10;
					  highp vec4 tmpvar_11;
					  tmpvar_11 = mix (texfront_3, texback_4, vec4(float((mask_2.x >= _Data.x))));
					  tmpvar_1 = tmpvar_11;
					  gl_FragData[0] = tmpvar_1;
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