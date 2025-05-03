Shader "Custom/GUI/BlobShadow" {
SubShader { 
 LOD 200
 Tags { "QUEUE"="Transparent" "RenderType"="Transparent" }
 Pass {
  Tags { "QUEUE"="Transparent" "RenderType"="Transparent" }
  Blend SrcAlpha OneMinusSrcAlpha
  GpuProgramID 23612
Program "vp" {
SubProgram "gles " {
"!!GLES
					#version 100
					
					#ifdef VERTEX
					attribute vec4 _glesVertex;
					attribute vec4 _glesColor;
					attribute vec4 _glesMultiTexCoord0;
					uniform highp mat4 glstate_matrix_mvp;
					varying highp vec2 xlv_TEXCOORD0;
					varying highp vec4 xlv_COLOR;
					void main ()
					{
					  gl_Position = (glstate_matrix_mvp * _glesVertex);
					  xlv_TEXCOORD0 = _glesMultiTexCoord0.xy;
					  xlv_COLOR = _glesColor;
					}
					
					
					#endif
					#ifdef FRAGMENT
					varying highp vec2 xlv_TEXCOORD0;
					varying highp vec4 xlv_COLOR;
					void main ()
					{
					  mediump vec4 tmpvar_1;
					  highp vec4 col_2;
					  col_2.xyz = xlv_COLOR.xyz;
					  highp vec2 tmpvar_3;
					  tmpvar_3 = (xlv_TEXCOORD0 - vec2(0.5, 0.5));
					  col_2.w = (1.0 - (dot (tmpvar_3, tmpvar_3) * 4.0));
					  tmpvar_1 = col_2;
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
}