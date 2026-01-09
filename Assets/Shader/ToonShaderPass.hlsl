#ifndef MY_TOON_SHADER_INCLUDE
#define MY_TOON_SHADER_INCLUDE
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/SpaceTransforms.hlsl"
// See ShaderVariablesFunctions.hlsl in com.unity.render-pipelines.universal/ShaderLibrary/ShaderVariablesFunctions.hlsl
///////////////////////////////////////////////////////////////////////////////
//                      CBUFFER                                              //
///////////////////////////////////////////////////////////////////////////////
/*
Unity URP requires us to set up a CBUFFER
(or "Constant Buffer") of Constant Variables.
These should be the same variables we set up 
in the Properties.
This CBUFFER is REQUIRED for Unity
to correctly handle per-material changes
as well as batching / instancing.
Don't skip it :)
*/
CBUFFER_START(UnityPerMaterial)
       TEXTURE2D(_ColorMap);
       SAMPLER(sampler_ColorMap);
       TEXTURE2D(_SMap);
       SAMPLER(sampler_SMap);
       float2 _Tilling;
       float4 _ColorMap_ST;
       float3 _Color;
       float3 _SColor;
       float _Smoothness;
       float _RimSharpness;
       float _Levels;
       float _PercentShadow;
       float3 _RimColor;
       float3 _WorldColor;
CBUFFER_END
///////////////////////////////////////////////////////////////////////////////
//                      STRUCTS                                              //
///////////////////////////////////////////////////////////////////////////////
/*
Our attributes struct is simple.
It contains the Object-Space Position
and Normal Direction as well as the 
UV0 coordinates for the mesh.
The Attributes struct is passed 
from the GPU to the Vertex function.
*/
struct Attributes
{
       float4 positionOS : POSITION;
       float3 normalOS   : NORMAL;
       float2 uv         : TEXCOORD0;
       
       // This line is required for VR SPI to work.
       UNITY_VERTEX_INPUT_INSTANCE_ID
};
/*
The Varyings struct is also straightforward.
It contains the Clip Space Position, the UV, and 
the World-Space Normals.
The Varyings struct is passed from the Vertex
function to the Fragment function.
*/
struct Varyings
{
       float4 positionHCS     : SV_POSITION;
       float2 uv              : TEXCOORD0;
       float3 positionWS      : TEXCOORD1;
       float3 normalWS        : TEXCOORD2;
       float3 viewDirectionWS   : TEXCOORD3;
       
       // This line is required for VR SPI to work.
    UNITY_VERTEX_INPUT_INSTANCE_ID
       UNITY_VERTEX_OUTPUT_STEREO
};
///////////////////////////////////////////////////////////////////////////////
//                      Common Lighting Transforms                           //
///////////////////////////////////////////////////////////////////////////////
// This is a global variable, Unity sets it for us.
float3 _LightDirection;

float4 GetClipSpacePosition(float3 positionWS, float3 normalWS)
{
       #if defined(SHADOW_CASTER_PASS)
           float4 positionHCS = TransformWorldToHClip(ApplyShadowBias(positionWS, normalWS, _LightDirection));
           
           #if UNITY_REVERSED_Z
               positionHCS.z = min(positionHCS.z, positionHCS.w * UNITY_NEAR_CLIP_VALUE);
           #else
               positionHCS.z = max(positionHCS.z, positionHCS.w * UNITY_NEAR_CLIP_VALUE);
           #endif
           
           return positionHCS;
       #endif
       
       return TransformWorldToHClip(positionWS);
}

float4 GetMainLightShadowCoord(float3 positionWS, float4 positionHCS)
{
       #if defined(_MAIN_LIGHT_SHADOWS_SCREEN)
           return ComputeScreenPos(positionHCS);
       #else
           return TransformWorldToShadowCoord(positionWS);
       #endif
}
float4 GetMainLightShadowCoord(float3 PositionWS)
{
       #if defined(_MAIN_LIGHT_SHADOWS_SCREEN)
           float4 clipPos = TransformWorldToHClip(PositionWS);
           return ComputeScreenPos(clipPos);
       #else
    return TransformWorldToShadowCoord(PositionWS);
       #endif
}

void GetMainLightData(float3 PositionWS, out Light light)
{
       float4 shadowCoord = GetMainLightShadowCoord(PositionWS);
       light = GetMainLight(shadowCoord);
}

float easysmoothstep(float min, float x)
{ 
       return smoothstep(min, min + 0.01, x);
}
///////////////////////////////////////////////////////////////////////////////
//                      Functions                                            //
///////////////////////////////////////////////////////////////////////////////
Varyings Vertex(Attributes IN)
{
       Varyings OUT = (Varyings)0;
       
       // These macros are required for VR SPI compatibility
       UNITY_SETUP_INSTANCE_ID(IN);
    UNITY_TRANSFER_INSTANCE_ID(IN, OUT);
    UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
       
       
       // Set up each field of the Varyings struct, then return it.
       OUT.positionWS = mul(unity_ObjectToWorld, IN.positionOS).xyz;
       OUT.normalWS = TransformObjectToWorldNormal(IN.normalOS);
       OUT.positionHCS = GetClipSpacePosition(OUT.positionWS, OUT.normalWS);
       OUT.uv = TRANSFORM_TEX(IN.uv, _ColorMap);
       

       OUT.viewDirectionWS = normalize(GetWorldSpaceViewDir(OUT.positionWS));
       
       return OUT;
}
float FragmentDepthOnly(Varyings IN) : SV_Target
{
       // These macros are required for VR SPI compatibility
       UNITY_SETUP_INSTANCE_ID(IN);
    UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(IN);
       
       return 0;
}
float4 FragmentDepthNormalsOnly(Varyings IN) : SV_Target
{
       // These macros are required for VR SPI compatibility
       UNITY_SETUP_INSTANCE_ID(IN);
    UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(IN);
       
       return float4(normalize(IN.normalWS), 0);
}

float3 Fragment(Varyings IN) : SV_Target
{
       // These macros are required for VR SPI compatibility
    UNITY_SETUP_INSTANCE_ID(IN);
    UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(IN);

       IN.normalWS = normalize(IN.normalWS);
       IN.viewDirectionWS = normalize(IN.viewDirectionWS);
       
       Light light;
       GetMainLightData(IN.positionWS, light);
       
       float NoL =  dot(IN.normalWS, light.direction) + _PercentShadow;
       // the light and shadow for cel shading/tooning
       float toonLighting = floor(saturate(NoL) * (_Levels + 1.0)) / _Levels;
       toonLighting = saturate(toonLighting);
       //showdows cast on the object
       float toonShadows = easysmoothstep(0.5, light.shadowAttenuation );

       // the specular ligthing part the little bright part on  it
       float3 halfVector = normalize(light.direction + IN.viewDirectionWS);
       float NoH =  max(dot(IN.normalWS, halfVector), 0);
       float specularTerm = pow(NoH, _Smoothness * _Smoothness);
       specularTerm *= toonLighting * toonShadows;
       specularTerm *= easysmoothstep(0.01, specularTerm);

       // rim light
       float NoV = max(dot(IN.normalWS, IN.viewDirectionWS), 0);
       float rimTerm = pow(1.0 - NoV, _RimSharpness);
       rimTerm *= toonLighting * toonShadows;
       rimTerm = easysmoothstep(0.01, rimTerm);

       //the base texture
       float3 surfaceColor = _Color * SAMPLE_TEXTURE2D(_ColorMap, sampler_ColorMap, IN.uv);

       //adding things together to het ligh shadows specular light
       float3 directionalLighting = toonLighting * toonShadows * light.color;
       float3 specularLighting = specularTerm * light.color;
       float3 rimLighting = rimTerm * _RimColor;
       
       float3 finalLighting = float3(0,0,0);
       finalLighting += directionalLighting;
       finalLighting += specularLighting;
       finalLighting += rimLighting;
       //set up uv for the tilled shadow texture
       float2 tiledUV = IN.uv * _Tilling;
       //how to get the shadow mask
       float4 shadowTexture = SAMPLE_TEXTURE2D(_SMap, sampler_SMap, tiledUV);
       
       float shadowMask = 1.0 - saturate(toonLighting * toonShadows);
       //color the shadows
       float3 shadowedColor = lerp(surfaceColor, _WorldColor, shadowTexture.r); 
       
       float3 litColor = surfaceColor * finalLighting; 
       //combine the two
       float3 finalOutput = lerp(litColor, shadowedColor, shadowMask);

       return finalOutput;
}
#endif