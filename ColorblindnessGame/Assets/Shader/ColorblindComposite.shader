Shader "Custom/ColorblindComposite"
{
    Properties
    {
        _MainTex ("Grayscale Scene", 2D) = "white" {}
        _ProtanopiaTex ("Protanopia View", 2D) = "white" {}
        _TritanopiaTex ("Tritanopia View", 2D) = "white" {}
        _Player1Pos ("Player 1 Screen Pos", Vector) = (0.25, 0.5, 0, 0)
        _Player2Pos ("Player 2 Screen Pos", Vector) = (0.75, 0.5, 0, 0)
        _CircleRadius ("Circle Radius", Float) = 0.15
        _EdgeSoftness ("Edge Softness", Float) = 0.02
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "RenderPipeline"="UniversalPipeline" }
        
        Pass
        {
            Name "Composite"
            
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
            TEXTURE2D(_ProtanopiaTex);
            SAMPLER(sampler_ProtanopiaTex);
            TEXTURE2D(_TritanopiaTex);
            SAMPLER(sampler_TritanopiaTex);
            
            float4 _Player1Pos;
            float4 _Player2Pos;
            float _CircleRadius;
            float _EdgeSoftness;
            float _ScreenAspect;
            
            Varyings vert(Attributes input)
            {
                Varyings output;
                output.positionCS = TransformObjectToHClip(input.positionOS.xyz);
                output.uv = input.uv;
                return output;
            }
            
            float4 frag(Varyings input) : SV_Target
            {
                float2 uv = input.uv;
                
                // Correct for aspect ratio
                float2 aspectCorrectedUV = float2(uv.x * _ScreenAspect, uv.y);
                float2 p1Corrected = float2(_Player1Pos.x * _ScreenAspect, _Player1Pos.y);
                float2 p2Corrected = float2(_Player2Pos.x * _ScreenAspect, _Player2Pos.y);
                
                // Calculate distances to each player
                float dist1 = distance(aspectCorrectedUV, p1Corrected);
                float dist2 = distance(aspectCorrectedUV, p2Corrected);
                
                // Create soft circle masks
                float mask1 = 1.0 - smoothstep(_CircleRadius - _EdgeSoftness, _CircleRadius, dist1);
                float mask2 = 1.0 - smoothstep(_CircleRadius - _EdgeSoftness, _CircleRadius, dist2);
                
                // Sample all three versions
                float4 grayscale = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv);
                float4 protanopia = SAMPLE_TEXTURE2D(_ProtanopiaTex, sampler_ProtanopiaTex, uv);
                float4 tritanopia = SAMPLE_TEXTURE2D(_TritanopiaTex, sampler_TritanopiaTex, uv);
                
                // Composite: start with grayscale, blend in colorblind views
                float4 result = grayscale;
                result = lerp(result, protanopia, mask1);  // Player 1 = Protanopia
                result = lerp(result, tritanopia, mask2);  // Player 2 = Tritanopia
                
                // Handle overlap (average the two colorblind views)
                float overlapMask = mask1 * mask2;
                if (overlapMask > 0.01)
                {
                    float4 blended = (protanopia + tritanopia) * 0.5;
                    result = lerp(result, blended, overlapMask);
                }
                
                return result;
            }
            ENDHLSL
        }
    }
}
