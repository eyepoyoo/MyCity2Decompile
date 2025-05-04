Shader "Custom/City/Grid" {
Properties {
 _MainTex ("Base texture", 2D) = "white" { }
 _Color1 ("Color1", Color) = (0,0,0,1)
 _Color2 ("Color2", Color) = (1,1,1,1)
 _PartitionSize ("PartitionSize", Float) = 20
}
SubShader { 
 LOD 200
 Tags { "QUEUE"="Geometry" "RenderType"="Transparent" }
 Pass {
  Tags { "QUEUE"="Geometry" "RenderType"="Transparent" }
  Blend SrcAlpha OneMinusSrcAlpha
  GpuProgramID 59050
Program "vp" {
SubProgram "gles " {
"!!GLES
					#version 100
					
					#ifdef VERTEX
					attribute vec4 _glesVertex;
					attribute vec4 _glesColor;
					attribute vec4 _glesMultiTexCoord0;
					uniform highp mat4 glstate_matrix_mvp;
					uniform highp vec4 _MainTex_ST;
					uniform highp float _PartitionSize;
					varying highp vec2 xlv_TEXCOORD0;
					varying highp vec2 xlv_TEXCOORD1;
					varying highp float xlv_TEXCOORD2;
					varying highp vec4 xlv_COLOR;
					void main ()
					{
					  gl_Position = (glstate_matrix_mvp * _glesVertex);
					  xlv_TEXCOORD0 = (((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw) * _PartitionSize);
					  xlv_TEXCOORD1 = _glesMultiTexCoord0.xy;
					  xlv_TEXCOORD2 = clamp (((_glesMultiTexCoord0.y - 0.15) * 2.0), 0.0, 1.0);
					  xlv_COLOR = _glesColor;
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform mediump vec4 _Color1;
					uniform mediump vec4 _Color2;
					varying highp vec2 xlv_TEXCOORD0;
					varying highp vec2 xlv_TEXCOORD1;
					varying highp float xlv_TEXCOORD2;
					void main ()
					{
					  mediump float d_1;
					  mediump vec2 v_2;
					  mediump vec4 fin_3;
					  mediump vec2 fr_4;
					  highp vec2 tmpvar_5;
					  tmpvar_5 = fract(xlv_TEXCOORD0);
					  fr_4 = tmpvar_5;
					  mediump vec2 tmpvar_6;
					  tmpvar_6 = vec2(greaterThanEqual (vec2(0.1, 0.1), fr_4));
					  fin_3.xyz = mix (_Color1, _Color2, vec4((1.0 - (
					    (1.0 - tmpvar_6.x)
					   * 
					    (1.0 - tmpvar_6.y)
					  )))).xyz;
					  highp vec2 tmpvar_7;
					  tmpvar_7 = (xlv_TEXCOORD1 - vec2(0.5, 0.5));
					  v_2 = tmpvar_7;
					  d_1 = (dot (v_2, v_2) * 10.0);
					  mediump float tmpvar_8;
					  tmpvar_8 = clamp (d_1, 0.0, 1.0);
					  d_1 = (tmpvar_8 + tmpvar_8);
					  fin_3.w = (1.0 - d_1);
					  fin_3.w = (fin_3.w * xlv_TEXCOORD2);
					  gl_FragData[0] = fin_3;
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