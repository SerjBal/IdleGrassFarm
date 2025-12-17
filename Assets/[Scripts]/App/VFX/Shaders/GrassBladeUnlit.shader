Shader "Unlit/GrassBladeUnlit"
{
    Properties
    {
        [Header(Grass Mask)]
        _GrassMaskTex ("Grass Mask (R)", 2D) = "white" {}
        _GrassMaskScale ("Mask Scale", Float) = 1
        
        [Header(Grass Colors)]
        _YoungGrassColor ("Young Grass Color", Color) = (0.3, 0.7, 0.2, 1)
        _OldGrassColor ("Old Grass Color", Color) = (0.1, 0.5, 0.1, 1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "RenderPipeline"="UniversalPipeline" }

        Pass
        {
            Tags { "LightMode"="UniversalForward" }

            Cull Off

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct GrassBlade
            {
                float3 position;
                float rotationY;
                float windNoise;
                float ageNoise;
            };

            #include "./shared/Transformations.cginc"
            #include "./shared/GrassVertexManipulations.cginc"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD;
                float4 normal : NORMAL;
            };

            struct Varyings
            {
                float4 pos : SV_POSITION;
                float ageNoise : TEXCOORD0;
                float3 positionWS : TEXCOORD1; // Мировые координаты ВЕРШИНЫ (не основания!)
                float2 maskUV : TEXCOORD2;
            };

            StructuredBuffer<GrassBlade> GrassBladesBuffer;
            float3 WindDirection;
            float WindForce;
            
            TEXTURE2D(_GrassMaskTex);
            SAMPLER(sampler_GrassMaskTex);
            float4 _GrassMaskTex_ST;
            float _GrassMaskScale;
            
            half4 _YoungGrassColor;
            half4 _OldGrassColor;

            Varyings vert (Attributes IN, uint vertex_id: SV_VERTEXID, uint instance_id: SV_INSTANCEID)
            {
                Varyings OUT;

                GrassBlade grassBlade;
                
                // Вычисляем мировую позицию с учетом ветра
                grassBlade = GrassBladesBuffer[instance_id];
                float4 worldPosition = positionVertexInWorld(grassBlade, IN.positionOS);
                worldPosition = applyWind(grassBlade, IN.uv, worldPosition, WindDirection, WindForce);
                
                // Сохраняем мировую позицию ВЕРШИНЫ для теней
                OUT.positionWS = worldPosition.xyz;
                
                // Преобразуем в clip позицию
                OUT.pos = TransformWorldToHClip(worldPosition.xyz);
                
                // Локальная позиция основания травинки для маски
                float3 bladeBasePosLS = grassBlade.position;
                
                // UV для маски из локальных координат основания
                OUT.maskUV = TRANSFORM_TEX(bladeBasePosLS.xz, _GrassMaskTex);

                OUT.ageNoise = grassBlade.ageNoise;

                return OUT;
            }

            half4 frag (Varyings IN) : SV_Target
            {
                // Проверяем маску травы
                float mask = SAMPLE_TEXTURE2D(_GrassMaskTex, sampler_GrassMaskTex, IN.maskUV / _GrassMaskScale).r;
                clip(mask - 0.1);

                // Тени - используем мировую позицию вершины
                float4 shadowCoord = TransformWorldToShadowCoord(IN.positionWS);
                float shadow = MainLightRealtimeShadow(shadowCoord);
                
                return lerp(_YoungGrassColor, _OldGrassColor, IN.ageNoise) * clamp(shadow, 0.2, 1);
            }
            ENDHLSL
        }

        Pass
        {
            Tags { "LightMode"="ShadowCaster" }

            Cull Off

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing
            #pragma multi_compile _ _CASTING_PUNCTUAL_LIGHT_SHADOW

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"

            struct GrassBlade
            {
                float3 position;
                float rotationY;
                float windNoise;
                float ageNoise;
            };

            #include "./shared/Transformations.cginc"
            #include "./shared/GrassVertexManipulations.cginc"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 maskUV : TEXCOORD0;
                float3 positionWS : TEXCOORD1; // Добавляем для ShadowCaster
            };

            StructuredBuffer<GrassBlade> GrassBladesBuffer;
            float3 WindDirection;
            float WindForce;
            
            TEXTURE2D(_GrassMaskTex);
            SAMPLER(sampler_GrassMaskTex);
            float4 _GrassMaskTex_ST;
            float _GrassMaskScale;

            Varyings vert (Attributes IN, uint vertex_id: SV_VERTEXID, uint instance_id: SV_INSTANCEID)
            {
                Varyings OUT;

                GrassBlade grassBlade;
                
                // Вычисляем мировую позицию с учетом ветра
                grassBlade = GrassBladesBuffer[instance_id];
                float4 worldPosition = positionVertexInWorld(grassBlade, IN.positionOS);
                worldPosition = applyWind(grassBlade, IN.uv, worldPosition, WindDirection, WindForce);
                
                // Сохраняем мировую позицию для ShadowCaster
                OUT.positionWS = worldPosition.xyz;
                
                // Преобразуем в clip позицию
                OUT.positionHCS = TransformWorldToHClip(worldPosition.xyz);

                // Локальные координаты для маски
                float3 bladeBasePosLS = grassBlade.position;
                OUT.maskUV = TRANSFORM_TEX(bladeBasePosLS.xz, _GrassMaskTex);

                return OUT;
            }

            half4 frag (Varyings IN) : SV_Target
            {
                float mask = SAMPLE_TEXTURE2D(_GrassMaskTex, sampler_GrassMaskTex, IN.maskUV / _GrassMaskScale).r;
                clip(mask - 0.1);
                
                return half4(1,1,1,1);
            }
            ENDHLSL
        }
    }
}