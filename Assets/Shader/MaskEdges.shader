Shader "Custom/Unlit/Mask Edges" {
Properties {
 _MainTex ("Base (RGB), Alpha (A)", 2D) = "black" { }
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
  GpuProgramID 12219
Program "vp" {
SubProgram "gles " {
"!!GLES
					#version 100
					
					#ifdef VERTEX
					attribute vec4 _glesVertex;
					attribute vec4 _glesColor;
					attribute vec4 _glesMultiTexCoord0;
					uniform highp mat4 glstate_matrix_mvp;
					mediump vec2 tmpvar_1;
					varying mediump vec2 xlv_TEXCOORD0;
					varying lowp vec4 xlv_COLOR;
					void main ()
					{
					  highp vec2 tmpvar_2;
					  tmpvar_2 = _glesMultiTexCoord0.xy;
					  tmpvar_1 = tmpvar_2;
					  gl_Position = (glstate_matrix_mvp * _glesVertex);
					  xlv_TEXCOORD0 = tmpvar_1;
					  xlv_COLOR = _glesColor;
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform sampler2D _MainTex;
					varying mediump vec2 xlv_TEXCOORD0;
					varying lowp vec4 xlv_COLOR;
					void main ()
					{
					  lowp float edgeMask2_1;
					  lowp float edgeMask_2;
					  lowp vec4 c_3;
					  lowp vec4 tmpvar_4;
					  tmpvar_4 = texture2D (_MainTex, xlv_TEXCOORD0);
					  c_3.xyz = tmpvar_4.xyz;
					  c_3.w = (tmpvar_4.w * xlv_COLOR.w);
					  mediump float tmpvar_5;
					  tmpvar_5 = float((xlv_COLOR.x >= xlv_TEXCOORD0.x));
					  edgeMask_2 = tmpvar_5;
					  mediump float tmpvar_6;
					  tmpvar_6 = float((xlv_COLOR.y >= (1.0 - xlv_TEXCOORD0.x)));
					  edgeMask2_1 = tmpvar_6;
					  c_3.w = (c_3.w * edgeMask_2);
					  c_3.w = (c_3.w * edgeMask2_1);
					  gl_FragData[0] = c_3;
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
}