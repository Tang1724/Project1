Shader "Game/Presentation/ColdRegion"
{
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
            struct v2f { float4 vertex : SV_POSITION; float2 world : TEXCOORD0; };
            v2f vert(appdata_base v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.world = mul(unity_ObjectToWorld, v.vertex).xy;
                return o;
            }
            float hash(float2 p) { return frac(sin(dot(p, float2(127.1, 311.7))) * 43758.5453); }
            fixed4 frag(v2f i) : SV_Target
            {
                float t = _Time.y;
                float2 p = floor(i.world * 48) / 48;
                float2 snow = p * 0.75 + float2(-t * 0.09, t * 0.22);
                float2 cell = floor(snow);
                float seed = hash(cell);
                float2 center = float2(0.22 + seed * 0.56, 0.2 + hash(cell + 8) * 0.6);
                float2 d = abs(frac(snow) - center);
                float flake = (1 - smoothstep(0.014, 0.028, min(d.x, d.y))) * (1 - smoothstep(0.035, 0.052, max(d.x, d.y)));
                flake *= step(0.36, seed);
                float mist = pow(saturate(sin(p.y * 1.6 + sin(p.x * 0.5 - t * 0.22) - t * 0.18)), 8);
                return fixed4(lerp(fixed3(0.27, 0.64, 0.86), fixed3(0.88, 0.97, 1), flake), 0.18 + mist * 0.075 + flake * 0.5);
            }
            ENDCG
        }
    }
    Fallback Off
}
