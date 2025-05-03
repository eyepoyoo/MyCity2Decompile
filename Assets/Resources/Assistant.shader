Shader "Custom/Assistant" {
Properties {
 _MainTex ("Assistant", 2D) = "white" { }
 _Params ("period, length, bar, time", Vector) = (0,0,0,0)
 _Alpha ("fade", Float) = 0
}
SubShader { 
 LOD 200
 Tags { "QUEUE"="Transparent" "IGNOREPROJECTOR"="true" "RenderType"="Transparent" }
 Pass {
  Tags { "QUEUE"="Transparent" "IGNOREPROJECTOR"="true" "RenderType"="Transparent" }
  ZTest Always
  Blend SrcAlpha OneMinusSrcAlpha
  GpuProgramID 6773
Program "vp" {
SubProgram "gles " {
"!!GLES
					#version 100
					
					#ifdef VERTEX
					attribute vec4 _glesVertex;
					attribute vec4 _glesColor;
					attribute vec4 _glesMultiTexCoord0;
					uniform highp mat4 glstate_matrix_mvp;
					varying mediump vec4 xlv_COLOR;
					varying mediump vec2 xlv_TEXCOORD0;
					void main ()
					{
					  gl_Position = (glstate_matrix_mvp * _glesVertex);
					  xlv_COLOR = _glesColor;
					  xlv_TEXCOORD0 = _glesMultiTexCoord0.xy;
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform sampler2D _MainTex;
					uniform highp vec4 _Params;
					uniform highp float _Alpha;
					varying mediump vec2 xlv_TEXCOORD0;
					void main ()
					{
					  mediump vec4 tex_1;
					  highp float tmpvar_2;
					  tmpvar_2 = (_Params.w + (xlv_TEXCOORD0.y * _Params.x));
					  highp vec4 tmpvar_3;
					  tmpvar_3.zw = vec2(0.0, 0.0);
					  tmpvar_3.x = tmpvar_2;
					  tmpvar_3.y = (tmpvar_2 * _Params.z);
					  highp vec4 val_4;
					  highp vec4 s_5;
					  val_4 = (((
					    fract(tmpvar_3)
					   * 1.02) - 0.5) * 6.283185);
					  highp vec4 tmpvar_6;
					  tmpvar_6 = ((val_4 * val_4) * val_4);
					  highp vec4 tmpvar_7;
					  tmpvar_7 = ((tmpvar_6 * val_4) * val_4);
					  s_5 = (((
					    (tmpvar_6 * -0.1616162)
					   + val_4) + (tmpvar_7 * 0.0083333)) + ((tmpvar_7 * val_4) * (val_4 * -0.00019841)));
					  highp vec2 tmpvar_8;
					  tmpvar_8.x = (xlv_TEXCOORD0.x + (s_5.x * _Params.y));
					  tmpvar_8.y = xlv_TEXCOORD0.y;
					  lowp vec4 tmpvar_9;
					  tmpvar_9 = texture2D (_MainTex, tmpvar_8);
					  tex_1 = tmpvar_9;
					  highp float tmpvar_10;
					  tmpvar_10 = clamp (s_5.y, _Alpha, 1.0);
					  tex_1.w = (tex_1.w * tmpvar_10);
					  gl_FragData[0] = tex_1;
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