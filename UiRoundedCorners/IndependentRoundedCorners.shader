Shader "UI/RoundedCorners/IndependentRoundedCorners" {
    
    Properties{
        _MainTex ("Texture", 2D) = "white" {}
        // Comment for future to debug
        //    _r ("Radiuses", Vector) = (0,0,0,0)
        //    _rect2props ("Rect 2 Props", Vector) = (0,0,0,0)
    }
    
    SubShader{
        Tags { 
            "RenderType"="Transparent"
            "Queue"="Transparent" 
        }

        Pass{
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #include "UnityCG.cginc"
            #include "SDFRoundedRectangle.cginc"

            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR; // set from Image component property
            };

            struct v2f {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };
            
            float4 _r;
            float4 _halfSize;
            float4 _rect2props;
            sampler2D _MainTex;
            
            v2f vert (appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.color = v.color;
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target {
                float alpha = CalcAlphaForIndependentCorners(i.uv, _halfSize.xy, _rect2props, _r);
                fixed4 col = tex2D(_MainTex, i.uv) * i.color;
                col.a = min(col.a, alpha);
                return col;
            }
            
            ENDCG
        }
    }
}
