Shader "Custom/SelectiveRed"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Intensity ("Intensity", Range(0,1)) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Pass
        {
            ZTest Always Cull Off ZWrite Off

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            float _Intensity;

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = IN.uv;
                return OUT;
            }

            float3 RGBToGray(float3 color)
            {
                float luminance = dot(color, float3(0.299, 0.587, 0.114));
                return float3(luminance, luminance, luminance);
            }

            float4 frag(Varyings IN) : SV_Target
            {
                float4 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv);

                // Detección suave de rojo dominante
                float redDiff = color.r - max(color.g, color.b);
                float redMask = smoothstep(0.0, 0.3, redDiff);

                // Escala de grises
                float3 gray = RGBToGray(color.rgb);

                // Mezcla entre gris y color según máscara
                float3 selectiveColor = lerp(gray, color.rgb, redMask);

                // Aplicar intensidad del efecto (0 = sin efecto, 1 = efecto completo)
                float3 finalColor = lerp(color.rgb, selectiveColor, _Intensity);

                return float4(finalColor, color.a);
            }
            ENDHLSL
        }
    }
}