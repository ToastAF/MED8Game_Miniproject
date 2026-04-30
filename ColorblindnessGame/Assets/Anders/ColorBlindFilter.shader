Shader "Custom/ColorBlindFilter"
{
    Properties
    {
        _ColorBlindMode ("Mode", Int) = 0
    }
    SubShader
    {
        Tags { "RenderPipeline"="UniversalPipeline" }
        Pass
        {
            ZTest Always ZWrite Off Cull Off

            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.core/Runtime/Utilities/Blit.hlsl"

            int _ColorBlindMode;

            // Deuteranopia (red-green, missing green cone) — Vienot 1999
            float3 Deuteranopia(float3 c)
            {
                float3 result;
                result.r = 0.56667 * c.r + 0.43333 * c.g + 0.00000 * c.b;
                result.g = 0.55833 * c.r + 0.44167 * c.g + 0.00000 * c.b;
                result.b = 0.00000 * c.r + 0.24167 * c.g + 0.75833 * c.b;
                return result;
            }

            // Tritanopia (blue-yellow, missing blue cone) — Vienot 1999
            float3 Tritanopia(float3 c)
            {
                float3 result;
                result.r = 0.95000 * c.r + 0.05000 * c.g + 0.00000 * c.b;
                result.g = 0.00000 * c.r + 0.43333 * c.g + 0.56667 * c.b;
                result.b = 0.00000 * c.r + 0.47500 * c.g + 0.52500 * c.b;
                return result;
            }

            half4 frag(Varyings i) : SV_Target
            {
                half4 col = SAMPLE_TEXTURE2D_X(_BlitTexture, sampler_LinearClamp, i.texcoord);

                if (_ColorBlindMode == 0)
                    col.rgb = Deuteranopia(col.rgb);
                else
                    col.rgb = Tritanopia(col.rgb);

                return col;
            }
            ENDHLSL
        }
    }
}