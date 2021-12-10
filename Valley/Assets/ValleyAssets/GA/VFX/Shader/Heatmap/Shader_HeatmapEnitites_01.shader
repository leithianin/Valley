Shader "Valley/Shader_HeatmapEnitites_01"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        [HDR]_Emission("Emission", Color) = (.0, .0, .0, .0)
        ///////
        _SnowColor("Snow Color", Color) = (1.0, 1.0, 1.0, 1.0)
        _SnowAngle("Snow Angle", float) = .0
        ///////
    }
    SubShader
    {
        Pass
        {
            Tags {"LightMode" = "Deferred"}

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct gbuffer 
            {
                float4 albedo : SV_Target0;
                float4 specular : SV_Target1;
                float4 normal : SV_Target2;
                float4 emission : SV_Target3;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            float4 _Emission;

            float4 _SnowColor;
            float _SnowAngle;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.normal = UnityObjectToWorldNormal(v.normal);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            gbuffer frag(v2f i)
            {
                i.normal = normalize(i.normal);

                gbuffer o;

                /////// dot for snow
                float normalDot = dot(i.normal, float3(.0f, 1.0f, .0f));

                if (normalDot < _SnowAngle)
                    o.albedo = tex2D(_MainTex, i.uv);
                else
                    o.albedo = _SnowColor;
                o.specular = float4(.0f, .0f, .0f, .0f);
                o.normal = float4(i.normal * .5f + .5f, .0f);
                o.emission = _Emission;

                return o;
            }

            ENDHLSL
        }
    }
}
