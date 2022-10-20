struct appdata {
    float4 vertex : POSITION;
    float2 uv : TEXCOORD0;
    float4 color : COLOR;  // set from Image component property
    UNITY_VERTEX_INPUT_INSTANCE_ID
};

struct v2f {
    float2 uv : TEXCOORD0;
    float4 vertex : SV_POSITION;
    float4 color : COLOR;
    float4 worldPosition : TEXCOORD1;
    UNITY_VERTEX_OUTPUT_STEREO
};

v2f vert (appdata v) {
    v2f o;
    UNITY_SETUP_INSTANCE_ID(v);
    UNITY_INITIALIZE_OUTPUT(v2f, o);
    UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
    o.worldPosition = v.vertex;
    o.vertex = UnityObjectToClipPos(v.vertex);
    o.uv = v.uv;
    o.color = v.color;
    return o;
}

inline fixed4 mixAlpha(fixed4 mainTexColor, fixed4 color, float sdfAlpha){
    fixed4 col = mainTexColor * color;
    col.a = min(col.a, sdfAlpha);
    return col;
}