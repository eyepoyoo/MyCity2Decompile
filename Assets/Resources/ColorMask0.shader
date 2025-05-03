Shader "Hidden/VacuumShaders/Curved World/ColorMask0" {
SubShader { 
 Pass {
  Name "BASE"
  ColorMask 0
  GpuProgramID 32298
Program "vp" {
SubProgram "gles " {
"!!GLES
					#version 100
					
					#ifdef VERTEX
					attribute vec4 _glesVertex;
					uniform highp mat4 glstate_matrix_mvp;
					uniform highp mat4 _Object2World;
					uniform highp mat4 _World2Object;
					uniform highp vec3 _V_CW_Bend;
					uniform highp vec3 _V_CW_Bias;
					uniform highp vec4 _V_CW_PivotPoint_Position;
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
					}
					
					
					#endif
					#ifdef FRAGMENT
					void main ()
					{
					  gl_FragData[0] = vec4(0.0, 0.0, 0.0, 0.0);
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