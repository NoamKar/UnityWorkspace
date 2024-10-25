Shader "Custom/SmudgeShader_Extended"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _Transparency("Transparency", Range(0, 1)) = 1.0
        _SmudgeAmount("Smudge Amount", Range(0, 1)) = 0.0
        _SmudgeOffset("Smudge Offset", Range(0, 1)) = 0.2 // Controls how far the smudge extends
        _SmudgeDirection("Smudge Direction", Vector) = (0, -1, 0) // Default is downward
    }
        SubShader
        {
            Tags { "Queue" = "Transparent" }
            LOD 200

            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Cull Off

            Pass
            {
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #include "UnityCG.cginc"

                sampler2D _MainTex;
                float _Transparency;
                float _SmudgeAmount;
                float _SmudgeOffset;
                float3 _SmudgeDirection;  // Direction for the smudge effect

                struct appdata
                {
                    float4 vertex : POSITION;
                    float2 uv : TEXCOORD0;
                };

                struct v2f
                {
                    float4 pos : SV_POSITION;
                    float2 uv : TEXCOORD0;
                };

                v2f vert(appdata v)
                {
                    v2f o;
                    o.pos = UnityObjectToClipPos(v.vertex);

                    // Adjust the UV based on the smudge amount and direction
                    o.uv = v.uv + _SmudgeAmount * (_SmudgeDirection.xy + float2(_SmudgeOffset, _SmudgeOffset));

                    return o;
                }

                fixed4 frag(v2f i) : SV_Target
                {
                    // Get the base color and apply transparency
                    fixed4 col = tex2D(_MainTex, i.uv);
                    col.a *= _Transparency; // Apply transparency control
                    return col;
                }
                ENDCG
            }
        }
}
