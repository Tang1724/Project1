Shader "Game/Presentation/ForestTerrain"
{
    Properties
    {
        _MainTex ("Forest Atlas", 2D) = "white" {}
        _AtlasRect ("Rock Region", Vector) = (0, 0, 1, 1)
        _Tint ("Rock Tint", Color) = (0.8, 1, 0.9, 1)
        _Repeat ("World Repeat", Vector) = (0.666667, 2, 0, 0)
        _Organic ("Earth Blend", Float) = 0
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        Cull Off
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            sampler2D _MainTex;
            float4 _AtlasRect;
            fixed4 _Tint;
            float4 _Repeat;
            float _Organic;
            struct v2f { float4 vertex : SV_POSITION; float2 world : TEXCOORD0; };
            v2f vert(appdata_base v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.world = mul(unity_ObjectToWorld, v.vertex).xy;
                return o;
            }
            fixed4 frag(v2f i) : SV_Target
            {
                float2 tile = frac(i.world * _Repeat.xy);
                float2 uv = _AtlasRect.xy + tile * _AtlasRect.zw;
                fixed4 rock = tex2D(_MainTex, uv) * _Tint;
                float variation = 0.94 + 0.06 * sin(i.world.x * 0.6 + sin(i.world.y * 0.4));
                if (_Organic > 0.5)
                {
                    float seams = sin(i.world.x * 1.13 + sin(i.world.y * 0.9) * 2.1) * sin(i.world.y * 1.37 + sin(i.world.x * 0.47));
                    float patch = smoothstep(0.05, 0.7, seams);
                    float luminance = dot(rock.rgb, float3(0.3, 0.59, 0.11));
                    fixed3 earth = lerp(fixed3(0.085, 0.105, 0.105), fixed3(0.17, 0.19, 0.175), patch);
                    rock.rgb = earth + luminance * lerp(0.07, 0.3, patch);
                    rock.rgb *= _Tint.rgb;
                }
                return fixed4(rock.rgb * variation, 1);
            }
            ENDCG
        }
    }
}
