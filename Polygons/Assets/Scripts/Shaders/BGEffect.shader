Shader "PostPro/BGEffect"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Stripe("Stripe", Float) = 10.0
        _StripeCol("StripeColor", Color) = (1, 1, 1, 1)
        _StripeOffset("StripeOffset", Float) = 0.0
        _NonStripeAlpha("NonStripeAlpha", Range(0, 1)) = 0.2
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

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
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
            float _Stripe;
            float4 _StripeCol;
            float _StripeOffset;
            float _NonStripeAlpha;

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                float y = i.uv.y;
                y = _StripeOffset + y * _Stripe;
                y = fmod(y, 1.0);

                float isStripe = 1000 * (y - 0.5);
                isStripe = saturate(isStripe);
                float4 newCol = lerp(col, _StripeCol, _NonStripeAlpha);
                col = lerp(_StripeCol, newCol, isStripe);

                return col;
            }
            ENDCG
        }
    }
}
