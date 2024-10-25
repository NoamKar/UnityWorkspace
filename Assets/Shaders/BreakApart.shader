Shader "Custom/BreakApartWithTransparency"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _BreakAmount("Break Amount", Range(0, 1)) = 0.0
        _NoiseTex("Noise Texture", 2D) = "white" {}
        _DisplaceMagnitude("Displace Magnitude", Range(0, 1)) = 0.5
        _Cutoff("Alpha Cutoff", Range(0,1)) = 0.5
    }
        SubShader
        {
            Tags { "RenderType" = "Transparent" }
            LOD 200

            CGPROGRAM
            #pragma surface surf Lambert alpha vertex:vert

            sampler2D _MainTex;
            sampler2D _NoiseTex;
            float _BreakAmount;
            float _DisplaceMagnitude;
            float _Cutoff;

            struct Input
            {
                float2 uv_MainTex;
                float3 worldPos;
            };

            void vert(inout appdata_full v)
            {
                float4 noise = tex2Dlod(_NoiseTex, float4(v.vertex.xy, 0, 0));
                float displacement = noise.r * _BreakAmount * _DisplaceMagnitude;
                v.vertex.xyz += v.normal * displacement;
            }

            void surf(Input IN, inout SurfaceOutput o)
            {
                fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
                float alphaValue = tex2D(_NoiseTex, IN.uv_MainTex).r;
                c.a = saturate(alphaValue - _BreakAmount); // Controls the fading based on break amount
                clip(c.a - _Cutoff); // Apply alpha cutoff to simulate parts fading out
                o.Albedo = c.rgb;
                o.Alpha = c.a;
            }
            ENDCG
        }
            FallBack "Transparent/Cutout/VertexLit"
}
