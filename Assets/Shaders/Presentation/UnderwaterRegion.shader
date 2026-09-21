Shader "Game/Presentation/UnderwaterRegion"
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
                float2 flow = p * float2(0.8, 0.6) - float2(0, t * 0.24);
                float2 cell = floor(flow);
                float seed = hash(cell);
                float2 center = float2(0.2 + seed * 0.6 + sin(t * 1.3 + seed * 9) * 0.045, 0.15 + hash(cell + 7.9) * 0.7);
                float2 offset = (frac(flow) - center) / float2(0.8, 0.6);
                float radius = 0.045 + seed * 0.085;
                float ring = 1 - smoothstep(0.009, 0.028, abs(length(offset) - radius));
                ring *= step(0.35, hash(cell + float2(29, 3)));
                float glint = ring * step(offset.x + offset.y, 0);
                float wave = pow(saturate(sin(p.y * 3.7 + sin(p.x * 1.6 + t * 0.45) - t * 0.65)), 18);
                float alpha = 0.11 + wave * 0.055 + ring * 0.4;
                fixed3 color = lerp(fixed3(0.08, 0.48, 0.62), fixed3(0.61, 0.94, 1), saturate(ring + glint));
                return fixed4(color, alpha);
            }
            ENDCG
        }
    }
}
