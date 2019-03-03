float AlphaForRoundedCorners(float2 uv, float w, float h, float r){
    float2 newUVInPixels = float2((uv.x - 0.5) * w + .5, (uv.y - 0.5) * h + .5);
    float2 muv = abs(newUVInPixels * 2 - 1) - float2(w, h) + r * 2;
    float d = length(max(0, muv)) / (r * 2);
    return saturate((1 - d) / fwidth(d));
}
