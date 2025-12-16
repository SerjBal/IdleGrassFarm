Shader "Custom/GrassShader"
{
    Properties
    {
        [MainTexture] _BaseMap("Base Map", 2D) = "white" {}
        [MainColor] _BaseColor("Base Color", Color) = (1, 1, 1, 1)
        _SecondAlbedoMap("Second Albedo Map", 2D) = "white" {}
        _SecondAlbedoColor("Second Albedo Color", Color) = (1, 1, 1, 1)
        _BlendMask("Blend Mask", 2D) = "white" {}
        
        [Toggle(_ALPHATEST_ON)] _AlphaClipToggle("Alpha Clipping", Float) = 0
        _Cutoff("Alpha Cutoff", Range(0, 1)) = 0.5
        
        [Toggle(_SPECULAR_SETUP)] _MetallicSpecToggle("Workflow: Specular (if on) or Metallic (if off)", Float) = 0
        
        _SpecGloss("Specular Gloss", Range(0, 1)) = 0.5
        _SpecColor("Specular Color", Color) = (0.5, 0.5, 0.5, 0.5)
        
        _Metallic("Metallic", Range(0, 1)) = 0
        
        [Toggle(_METALLICSPECGLOSSMAP)] _MetallicSpecMapToggle("Use Metallic/Specular Map", Float) = 0
        _MetallicSpecMap("Metallic/Specular Map", 2D) = "black" {}
        
        [Toggle(_NORMALMAP)] _NormalMapToggle("Use Normal Map", Float) = 0
        [NoScaleOffset] _BumpMap("Normal Map", 2D) = "bump" {}
        _BumpScale("Bump Scale", Float) = 1
        
        [Toggle(_OCCLUSIONMAP)] _OcclusionToggle("Use Occlusion Map", Float) = 0
        [NoScaleOffset] _OcclusionMap("Occlusion Map", 2D) = "white" {}
        _OcclusionStrength("Occlusion Strength", Range(0, 1)) = 1
    }

    SubShader
    {
        Tags 
        { 
            "RenderPipeline" = "UniversalPipeline"
            "RenderType" = "Opaque"
            "Queue" = "Geometry"
        }

        HLSLINCLUDE
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        
        TEXTURE2D(_MetallicSpecMap);   SAMPLER(sampler_MetallicSpecMap);
        TEXTURE2D(_OcclusionMap);      SAMPLER(sampler_OcclusionMap);
        TEXTURE2D(_SecondAlbedoMap);   SAMPLER(sampler_SecondAlbedoMap);
        TEXTURE2D(_BlendMask);         SAMPLER(sampler_BlendMask);
        
        float4 _BaseColor;
        float4 _BaseMap_ST;
        float4 _SecondAlbedoColor;
        float4 _SecondAlbedoMap_ST;
        float4 _BlendMask_ST;
        float _Cutoff;
        float _SpecGloss;
        float4 _SpecColor;
        float _Metallic;
        float _BumpScale;
        float _OcclusionStrength;
        ENDHLSL

        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode" = "UniversalForward" }
            
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #pragma shader_feature_local _NORMALMAP
            #pragma shader_feature_local_fragment _ALPHATEST_ON
            #pragma shader_feature_local_fragment _ALPHAPREMULTIPLY_ON
            #pragma shader_feature_local_fragment _METALLICSPECGLOSSMAP
            #pragma shader_feature_local_fragment _OCCLUSIONMAP
            #pragma shader_feature_local_fragment _SPECULAR_SETUP
            #pragma shader_feature_local _RECEIVE_SHADOWS_OFF
            
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE _MAIN_LIGHT_SHADOWS_SCREEN
            #pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
            #pragma multi_compile_fragment _ _ADDITIONAL_LIGHT_SHADOWS
            #pragma multi_compile_fragment _ _SHADOWS_SOFT
            #pragma multi_compile_fragment _ _SCREEN_SPACE_OCCLUSION
            #pragma multi_compile _ LIGHTMAP_SHADOW_MIXING
            #pragma multi_compile _ SHADOWS_SHADOWMASK
            
            #pragma multi_compile _ LIGHTMAP_ON
            #pragma multi_compile _ DIRLIGHTMAP_COMBINED
            
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/SurfaceInput.hlsl"
            
            struct Attributes
            {
                float4 positionOS : POSITION;
                #ifdef _NORMALMAP
                float4 tangentOS : TANGENT;
                #endif
                float4 normalOS : NORMAL;
                float2 uv : TEXCOORD0;
                float2 lightmapUV : TEXCOORD1;
            };
            
            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float3 positionWS : TEXCOORD0;
                float2 uv : TEXCOORD1;
                float2 uvSecondAlbedo : TEXCOORD2;
                float2 uvBlendMask : TEXCOORD3;
                
                #ifdef _NORMALMAP
                half4 normalWS : TEXCOORD4;
                half4 tangentWS : TEXCOORD5;
                half4 bitangentWS : TEXCOORD6;
                #else
                half3 normalWS : TEXCOORD4;
                #endif
                
                DECLARE_LIGHTMAP_OR_SH(lightmapUV, vertexSH, 7);
                
                #if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
                float4 shadowCoord : TEXCOORD8;
                #endif
            };
            
            half SampleOcclusion(float2 uv)
            {
                #ifdef _OCCLUSIONMAP
                half occ = SAMPLE_TEXTURE2D(_OcclusionMap, sampler_OcclusionMap, uv).g;
                return LerpWhiteTo(occ, _OcclusionStrength);
                #else
                return 1.0;
                #endif
            }
            
            half4 SampleMetallicSpecGloss(float2 uv, half albedoAlpha)
            {
                half4 specGloss;
                #ifdef _METALLICSPECGLOSSMAP
                specGloss = half4(SAMPLE_METALLICSPECULAR(uv));
                specGloss.a *= _SpecGloss;
                #else
                #if _SPECULAR_SETUP
                specGloss.rgb = _SpecColor.rgb;
                #else
                specGloss.rgb = _Metallic.rrr;
                #endif
                specGloss.a = _SpecGloss;
                #endif
                return specGloss;
            }
            
            void InitializeSurfaceData(Varyings i, out SurfaceData surfaceData)
            {
                surfaceData = (SurfaceData)0;
                
                half4 albedoAlpha1 = SampleAlbedoAlpha(i.uv, TEXTURE2D_ARGS(_BaseMap, sampler_BaseMap));
                half4 albedoAlpha2 = SAMPLE_TEXTURE2D(_SecondAlbedoMap, sampler_SecondAlbedoMap, i.uvSecondAlbedo);
                half blendFactor = SAMPLE_TEXTURE2D(_BlendMask, sampler_BlendMask, i.uvBlendMask).r;
                
                half4 albedoAlpha = lerp(albedoAlpha1 * _BaseColor, albedoAlpha2 * _SecondAlbedoColor, blendFactor);
                
                surfaceData.alpha = Alpha(albedoAlpha.a, _BaseColor, _Cutoff);
                surfaceData.albedo = albedoAlpha.rgb;
                
                #ifdef _NORMALMAP
                surfaceData.normalTS = SampleNormal(i.uv, TEXTURE2D_ARGS(_BumpMap, sampler_BumpMap), _BumpScale);
                #else
                surfaceData.normalTS = half3(0, 0, 1);
                #endif
                
                surfaceData.occlusion = SampleOcclusion(i.uv);
                
                half4 specGloss = SampleMetallicSpecGloss(i.uv, albedoAlpha.a);
                
                #if _SPECULAR_SETUP
                surfaceData.metallic = 1.0h;
                surfaceData.specular = specGloss.rgb;
                #else
                surfaceData.metallic = specGloss.r;
                surfaceData.specular = half3(0.0h, 0.0h, 0.0h);
                #endif
                
                surfaceData.smoothness = specGloss.a;
                surfaceData.emission = half3(0, 0, 0);
            }
            
            void InitializeInputData(Varyings input, half3 normalTS, out InputData inputData)
            {
                inputData = (InputData)0;
                inputData.positionWS = input.positionWS;
                
                #ifdef _NORMALMAP
                half3 viewDirWS = half3(input.normalWS.w, input.tangentWS.w, input.bitangentWS.w);
                inputData.normalWS = TransformTangentToWorld(normalTS, 
                    half3x3(input.tangentWS.xyz, input.bitangentWS.xyz, input.normalWS.xyz));
                #else
                half3 viewDirWS = GetWorldSpaceNormalizeViewDir(inputData.positionWS);
                inputData.normalWS = input.normalWS;
                #endif
                
                inputData.normalWS = NormalizeNormalPerPixel(inputData.normalWS);
                viewDirWS = SafeNormalize(viewDirWS);
                inputData.viewDirectionWS = viewDirWS;
                
                #if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
                inputData.shadowCoord = input.shadowCoord;
                #elif defined(MAIN_LIGHT_CALCULATE_SHADOWS)
                inputData.shadowCoord = TransformWorldToShadowCoord(inputData.positionWS);
                #else
                inputData.shadowCoord = float4(0, 0, 0, 0);
                #endif
                
                inputData.bakedGI = SAMPLE_GI(input.lightmapUV, input.vertexSH, inputData.normalWS);
                inputData.normalizedScreenSpaceUV = GetNormalizedScreenSpaceUV(input.positionCS);
                inputData.shadowMask = SAMPLE_SHADOWMASK(input.lightmapUV);
            }
            
            Varyings vert(Attributes i)
            {
                Varyings v;
                
                VertexPositionInputs positionInputs = GetVertexPositionInputs(i.positionOS.xyz);
                
                #ifdef _NORMALMAP
                VertexNormalInputs normalInputs = GetVertexNormalInputs(i.normalOS.xyz, i.tangentOS);
                #else
                VertexNormalInputs normalInputs = GetVertexNormalInputs(i.normalOS.xyz);
                #endif
                
                v.positionCS = positionInputs.positionCS;
                v.positionWS = positionInputs.positionWS;
                
                half3 viewDirWS = GetWorldSpaceViewDir(positionInputs.positionWS);
                half3 vertexLight = VertexLighting(positionInputs.positionWS, normalInputs.normalWS);
                
                #ifdef _NORMALMAP
                v.normalWS = half4(normalInputs.normalWS, viewDirWS.x);
                v.tangentWS = half4(normalInputs.tangentWS, viewDirWS.y);
                v.bitangentWS = half4(normalInputs.bitangentWS, viewDirWS.z);
                #else
                v.normalWS = NormalizeNormalPerVertex(normalInputs.normalWS);
                #endif
                
                OUTPUT_LIGHTMAP_UV(i.lightmapUV, unity_LightmapST, v.lightmapUV);
                OUTPUT_SH(v.normalWS.xyz, v.vertexSH);
                
                #if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
                v.shadowCoord = GetShadowCoord(positionInputs);
                #endif
                
                v.uv = TRANSFORM_TEX(i.uv, _BaseMap);
                v.uvSecondAlbedo = TRANSFORM_TEX(i.uv, _SecondAlbedoMap);
                v.uvBlendMask = TRANSFORM_TEX(i.uv, _BlendMask);
                
                return v;
            }
            
            half4 frag(Varyings i) : SV_Target
            {
                SurfaceData surfaceData;
                InitializeSurfaceData(i, surfaceData);
                
                InputData inputData;
                InitializeInputData(i, surfaceData.normalTS, inputData);
                
                half4 color = UniversalFragmentPBR(inputData, surfaceData);
                return color;
            }
            ENDHLSL
        }
    }
    
    Fallback "Universal Render Pipeline/Lit"
}