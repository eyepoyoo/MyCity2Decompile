Shader "Hidden/VacuumShaders/Curved World/Outline" {
Properties {
 _V_CW_OutlineColor ("Outline Color", Color) = (0,0,0,1)
 _V_CW_OutlineWidth ("Outline width", Float) = 0.005
}
SubShader { 
 Pass {
  Name "OUTLINE"
  Cull Front
  Blend SrcAlpha OneMinusSrcAlpha
  ColorMask RGB
  GpuProgramID 4190
Program "vp" {
}
Program "fp" {
}
 }
}
Fallback Off
}