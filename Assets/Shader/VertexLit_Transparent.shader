Shader "Hidden/VacuumShaders/Curved World/VertexLit/Transparent" {
Properties {
[CurvedWorldGearMenu]  V_CW_Label_Tag ("", Float) = 0
[CurvedWorldLabel]  V_CW_Label_UnityDefaults ("Default Visual Options", Float) = 0
[CurvedWorldLargeLabel]  V_CW_Label_Albedo ("Albedo", Float) = 0
 _Color ("  Color", Color) = (1,1,1,1)
 _MainTex ("  Map (RGB) Trans (A)", 2D) = "white" { }
[CurvedWorldUVScroll]  _V_CW_MainTex_Scroll ("    ", Vector) = (0,0,0,0)
[CurvedWorldLabel]  V_CW_CW_OPTIONS ("Curved World Optionals", Float) = 0
[HideInInspector]  _V_CW_Rim_Color ("", Color) = (1,1,1,1)
[HideInInspector]  _V_CW_Rim_Bias ("", Range(-1,1)) = 0.2
[HideInInspector]  _V_CW_Rim_Power ("", Range(0.5,8)) = 3
[HideInInspector]  _EmissionMap ("", 2D) = "white" { }
[HideInInspector]  _EmissionColor ("", Color) = (1,1,1,1)
[HideInInspector]  _V_CW_IBL_Intensity ("", Float) = 1
[HideInInspector]  _V_CW_IBL_Contrast ("", Float) = 1
[HideInInspector]  _V_CW_IBL_Cube ("", CUBE) = "" { }
}
SubShader { 
 LOD 100
 Tags { "QUEUE"="Transparent+1" "IGNOREPROJECTOR"="true" "RenderType"="Transparent" "CurvedWorldTag"="VertexLit/Transparent" "CurvedWorldNoneRemoveableKeywords"="" "CurvedWorldAvailableOptions"="V_CW_VERTEX_COLOR;V_CW_RIM;V_CW_FOG;" }
 Pass {
  Tags { "LIGHTMODE"="Vertex" "QUEUE"="Transparent+1" "IGNOREPROJECTOR"="true" "RenderType"="Transparent" "CurvedWorldTag"="VertexLit/Transparent" "CurvedWorldNoneRemoveableKeywords"="" "CurvedWorldAvailableOptions"="V_CW_VERTEX_COLOR;V_CW_RIM;V_CW_FOG;" }
  Lighting On
  ZWrite Off
  Blend SrcAlpha OneMinusSrcAlpha
  GpuProgramID 43744
Program "vp" {
}
Program "fp" {
}
 }
 Pass {
  Tags { "LIGHTMODE"="VertexLM" "QUEUE"="Transparent+1" "IGNOREPROJECTOR"="true" "RenderType"="Transparent" "CurvedWorldTag"="VertexLit/Transparent" "CurvedWorldNoneRemoveableKeywords"="" "CurvedWorldAvailableOptions"="V_CW_VERTEX_COLOR;V_CW_RIM;V_CW_FOG;" }
  ZWrite Off
  Blend SrcAlpha OneMinusSrcAlpha
  GpuProgramID 94672
Program "vp" {
}
Program "fp" {
}
 }
 Pass {
  Tags { "LIGHTMODE"="VertexLMRGBM" "QUEUE"="Transparent+1" "IGNOREPROJECTOR"="true" "RenderType"="Transparent" "CurvedWorldTag"="VertexLit/Transparent" "CurvedWorldNoneRemoveableKeywords"="" "CurvedWorldAvailableOptions"="V_CW_VERTEX_COLOR;V_CW_RIM;V_CW_FOG;" }
  ZWrite Off
  Blend SrcAlpha OneMinusSrcAlpha
  GpuProgramID 166395
Program "vp" {
}
Program "fp" {
}
 }
}
Fallback Off
}