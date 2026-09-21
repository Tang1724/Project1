Shader "Game/Presentation/DryingHeatMask"
{
    Properties
    {
        _Tint ("Heat Tint", Color) = (1, 0.55, 0.16, 1)
        _Opacity ("Mask Opacity", Range(0, 1)) = 0.36
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        GrabPass { "_DryingHeatBackground" }
        Pass
        {
            Cull Off
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _DryingHeatBackground;
            fixed4 _Tint;
            float _Opacity;

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float4 grab : TEXCOORD0;
                float2 world : TEXCOORD1;
            };

            v2f vert(appdata_base v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.grab = ComputeGrabScreenPos(o.vertex);
                o.world = mul(unity_ObjectToWorld, v.vertex).xy;
                return o;
            }

            float hash(float2 p)
            {
                return frac(sin(dot(p, float2(127.1, 311.7))) * 43758.5453);
            }

            float noise(float2 p)
            {
                float2 cell = floor(p);
                float2 f = frac(p);
                f = f * f * (3 - 2 * f);
                return lerp(lerp(hash(cell), hash(cell + float2(1, 0)), f.x),
                            lerp(hash(cell + float2(0, 1)), hash(cell + 1), f.x), f.y);
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float t = _Time.y;
                float2 flow = i.world * float2(1.4, 0.75) - float2(0, t * 0.45);
                float fog = noise(flow) * 0.7 + noise(flow * 2.4) * 0.3;
                float waves = sin(i.world.x * 9 + sin(i.world.y * 2.5 - t * 1.8) + fog * 3);
                float steam = pow(saturate(waves * 0.5 + 0.5), 9);
                float2 distortion = float2(sin(i.world.y * 10 - t * 3 + fog * 4),
                                           cos(i.world.x * 7 + t * 2)) * 0.002;
                float4 grab = i.grab;
                grab.xy += distortion * grab.w;
                fixed3 background = tex2Dproj(_DryingHeatBackground, UNITY_PROJ_COORD(grab)).rgb;
                fixed3 heat = lerp(background, _Tint.rgb, 0.42 + fog * 0.12);
                heat += steam * fixed3(0.13, 0.12, 0.09);
                return fixed4(heat, saturate(_Opacity + fog * 0.12));
            }
            ENDCG
        }
    }
    Fallback Off
}
