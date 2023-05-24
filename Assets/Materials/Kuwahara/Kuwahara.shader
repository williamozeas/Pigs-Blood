//Based on https://www.danielilett.com/2019-05-18-tut1-6-smo-painting/
Shader "Hidden/Custom/Kuwahara"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _KernelSize("Kernel Size (N)", Int) = 7
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always
        
        Tags { "RenderType" = "Opaque" }
        
        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = TransformObjectToHClip(v.vertex);
                o.uv = v.uv;
                return o;
            }

            struct region
            {
                float3 mean;
                float variance;
            };

            sampler2D _MainTex;
			float2 _MainTex_TexelSize;
			int _KernelSize;

            region calcRegion(int2 lower, int2 upper, int samples, float2 uv)
            {
                region r;

                float3 sum = 0.0;
                float3 squareSum = 0.0;

                for (int x = lower.x; x <= upper.x; ++x)
                {
                    for (int y = lower.y; y <= upper.y; ++y)
                    {
                        half2 offset = half2(_MainTex_TexelSize.x * x, _MainTex_TexelSize.y * y);
                        half3 tex = tex2D(_MainTex, uv + offset);

                        sum += tex;
                        squareSum += tex * tex;
                    }
                }

                r.mean = sum / samples;

                float3 variance = abs((squareSum / samples) - (r.mean * r.mean));
                r.variance = length(variance);

                return r;
            }
            
            float4 frag (v2f i) : SV_Target
            {
                int upper = (_KernelSize - 1) / 2;
                int lower = -upper;
                int samples = (upper + 1) * (upper + 1);

                region regionA = calcRegion(int2(lower, lower), int2(0, 0), samples, i.uv);
                region regionB = calcRegion(int2(0, lower), int2(upper, 0), samples, i.uv);
                region regionC = calcRegion(int2(lower, 0), int2(0, upper), samples, i.uv);
                region regionD = calcRegion(int2(0, 0), int2(upper, upper), samples, i.uv);

                half3 col = regionA.mean;
                half minVar = regionA.variance;

                float testVal;
                
                // Test region B.
                testVal = step(regionB.variance, minVar);
                col = lerp(col, regionB.mean, testVal);
                minVar = lerp(minVar, regionB.variance, testVal);

                // Test region C.
                testVal = step(regionC.variance, minVar);
                col = lerp(col, regionC.mean, testVal);
                minVar = lerp(minVar, regionC.variance, testVal);

                // Text region D.
                testVal = step(regionD.variance, minVar);
                col = lerp(col, regionD.mean, testVal);

                return float4(col, 1.0);
            }
            ENDHLSL
        }
    }
}
