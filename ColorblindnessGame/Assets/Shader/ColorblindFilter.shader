Shader "Custom/ColorblindFilter"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _FilterType ("Filter Type", Float) = 0 // 0 = Protanopia, 1 = Tritanopia, 2 = Grayscale
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "RenderPipeline"="UniversalPipeline" }
        
        Pass
        {
            Name "ColorblindFilter"
            
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            
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
            
            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            float _FilterType;
            
            Varyings vert(Attributes input)
            {
                Varyings output;
                output.positionCS = TransformObjectToHClip(input.positionOS.xyz);
                output.uv = input.uv;
                return output;
            }
            
            // Colorblindness simulation matrices (Brettel et al. model)
            float3 ApplyProtanopia(float3 color)
            {
                // Protanopia: reduced sensitivity to red
                float3x3 protanopia = float3x3(
                    0.567, 0.433, 0.000,
                    0.558, 0.442, 0.000,
                    0.000, 0.242, 0.758
                );
                return mul(protanopia, color);
            }
            
            float3 ApplyTritanopia(float3 color)
            {
                // Tritanopia: reduced sensitivity to blue
                float3x3 tritanopia = float3x3(
                    0.950, 0.050, 0.000,
                    0.000, 0.433, 0.567,
                    0.000, 0.475, 0.525
                );
                return mul(tritanopia, color);
            }
            
            float3 ApplyGrayscale(float3 color)
            {
                float gray = dot(color, float3(0.299, 0.587, 0.114));
                return float3(gray, gray, gray);
            }
            
            float4 frag(Varyings input) : SV_Target
            {
                float4 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, input.uv);
                
                if (_FilterType < 0.5)
                    color.rgb = ApplyProtanopia(color.rgb);
                else if (_FilterType < 1.5)
                    color.rgb = ApplyTritanopia(color.rgb);
                else
                    color.rgb = ApplyGrayscale(color.rgb);
                
                return color;
            }
            ENDHLSL
        }
    }
}
