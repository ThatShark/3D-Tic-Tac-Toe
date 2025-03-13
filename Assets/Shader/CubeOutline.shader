Shader "Unlit/CubeOutline"
{
    Properties
    {
        _Color("Color", color) = (1,1,1,1)
        _Width("Width", range(0,0.5)) = 0.1
    }
    SubShader
    {
        Tags { "Queue"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off
        Pass
        {
            CGPROGRAM
            
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            
            uniform fixed4 _Color;
            uniform fixed _Width;
            
            struct a2v {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };
            
            struct v2f {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };
            
            v2f vert(a2v v) {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }
            
            float4 frag(v2f i) : SV_Target {
                fixed4 col = fixed4(0,0,0,0); // 設為完全透明
                
                // 計算邊框
                float border = saturate(
                    step(i.uv.x, _Width) + step(1 - _Width, i.uv.x) +
                    step(i.uv.y, _Width) + step(1 - _Width, i.uv.y)
                );
                
                // 只有邊框部分顯示顏色，其餘部分透明
                col.rgb = _Color.rgb * border;
                col.a = border; // 邊框部分不透明，其他部分透明
                
                return col;
            }
            
            ENDCG
        }
    }
}
