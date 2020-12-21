Shader "UI/RoundedCorners/RoundedCorners" {
    Properties {
        [HideInInspector] _MainTex ("Texture", 2D) = "white" {}

        // --- Mask support ---
        [HideInInspector] _StencilComp ("Stencil Comparison", Float) = 8
        [HideInInspector] _Stencil ("Stencil ID", Float) = 0
        [HideInInspector] _StencilOp ("Stencil Operation", Float) = 0
        [HideInInspector] _StencilWriteMask ("Stencil Write Mask", Float) = 255
        [HideInInspector] _StencilReadMask ("Stencil Read Mask", Float) = 255
        [HideInInspector] _ColorMask ("Color Mask", Float) = 15
        [HideInInspector] _UseUIAlphaClip ("Use Alpha Clip", Float) = 0
		// Here we store data required to normalize UV
		[HideInInspector] _Rect("Rect Display", Vector) = (0,0,1,1)
        
        // Definition in Properties section is required to Mask works properly
        _WidthHeightRadius ("WidthHeightRadius", Vector) = (0,0,0,0)
        // ---
    }
    
    SubShader {
        Tags {
            "RenderType"="Transparent"
            "Queue"="Transparent"
        }

        // --- Mask support ---
        Stencil {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp]
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }
        Cull Off
        Lighting Off
        ZTest [unity_GUIZTestMode]
        ColorMask [_ColorMask]
        // ---
        
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off

        Pass {
            CGPROGRAM
            
            #include "UnityCG.cginc"
            #include "SDFUtils.cginc"
            #include "ShaderSetup.cginc"

            #pragma vertex vert
            #pragma fragment frag

            float4 _WidthHeightRadius;
			float4 _Rect;
            sampler2D _MainTex;

            fixed4 frag (v2f i) : SV_Target {
				float2 localuv = i.uv;
				// Normalized UV: https://forum.unity.com/threads/uv-texture-coordinates-bounds-using-sprite-packer.400592/#post-3585072
				float2 uv = (localuv - _Rect.xy) / (_Rect.zw - _Rect.xy);;
				float alpha = CalcAlpha(uv, _WidthHeightRadius.xy, _WidthHeightRadius.z);
				return mixAlpha(tex2D(_MainTex, localuv), i.color, alpha);
            }
            
            ENDCG
        }
    }
}
