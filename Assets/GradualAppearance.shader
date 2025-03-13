Shader "Custom/GradualAppearance"
{
    Properties
    {
        _Color ("颜色", Color) = (1,1,1,1)
        _MainTex ("主纹理", 2D) = "white" {}
        _GradientDirection ("渐变方向", Vector) = (1,0,0,0)
        _StartPosition ("起始位置", Float) = -0.5
        _EndPosition ("结束位置", Float) = 0.5
        _Progress ("进度", Range(0,1)) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100

        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

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
                float3 worldPos : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _Color;
            float3 _GradientDirection;
            float _StartPosition;
            float _EndPosition;
            float _Progress;
            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                // 计算物体在渐变方向上的投影位置
                float projectedPosition = dot(i.worldPos, normalize(_GradientDirection));
                
                // 计算当前位置的显示程度
                float gradientPosition = (projectedPosition - _StartPosition) / (_EndPosition - _StartPosition);
                float visibility = smoothstep(0, 1, (gradientPosition - (1 - _Progress)));
                
                // 采样纹理并应用颜色
                fixed4 col = tex2D(_MainTex, i.uv) * _Color;
                col.a *= visibility;
                
                return col;
            }
            ENDCG
        }
    }
} 