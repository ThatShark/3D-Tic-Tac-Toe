Shader "Unlit/CubeOutline"
{
    Properties
    {
        _Color("Color", color) = (1,1,1,1)
        _Width("Width", range(0,0.5)) = 0.01
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
                fixed4 col = fixed4(0,0,0,0); // �]�������z��
                
                // �p�����
                float border = saturate(
                    step(i.uv.x, _Width) + step(1 - _Width, i.uv.x) +
                    step(i.uv.y, _Width) + step(1 - _Width, i.uv.y)
                );
                
                // �u����س�������C��A��l�����z��
                col.rgb = _Color.rgb * border;
                col.a = border; // ��س������z���A��L�����z��
                
                return col;
            }
            
            ENDCG
        }
    }
}
