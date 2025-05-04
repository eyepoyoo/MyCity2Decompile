Shader "Custom/GUI/BlobShadow 1" {
SubShader { 
 LOD 200
 Tags { "QUEUE"="Transparent" "RenderType"="Transparent" }
 Pass {
  Tags { "QUEUE"="Transparent" "RenderType"="Transparent" }
  Blend SrcAlpha OneMinusSrcAlpha
  GpuProgramID 5566
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
					varying highp vec2 xlv_TEXCOORD0;
					varying highp vec4 xlv_COLOR;
					varying highp vec2 xlv_TEXCOORD1;
					void main ()
					{
					  gl_Position = (glstate_matrix_mvp * _glesVertex);
					  xlv_TEXCOORD0 = _glesMultiTexCoord0.xy;
					  xlv_COLOR = _glesColor;
					  xlv_TEXCOORD1 = ((_glesVertex.xy * _ClipRange0.zw) + _ClipRange0.xy);
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform highp vec2 _ClipArgs0;
					varying highp vec2 xlv_TEXCOORD0;
					varying highp vec4 xlv_COLOR;
					varying highp vec2 xlv_TEXCOORD1;
					void main ()
					{
					  mediump vec4 tmpvar_1;
					  highp vec4 col_2;
					  col_2.xyz = xlv_COLOR.xyz;
					  highp vec2 tmpvar_3;
					  tmpvar_3 = (xlv_TEXCOORD0 - vec2(0.5, 0.5));
					  col_2.w = (1.0 - (dot (tmpvar_3, tmpvar_3) * 4.0));
					  col_2.w = (col_2.w * 2.0);
					  highp vec2 tmpvar_4;
					  tmpvar_4 = ((vec2(1.0, 1.0) - abs(xlv_TEXCOORD1)) * _ClipArgs0);
					  col_2.w = (col_2.w * xlv_COLOR.w);
					  col_2.w = (col_2.w * clamp (min (tmpvar_4.x, tmpvar_4.y), 0.0, 1.0));
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